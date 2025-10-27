using System.Collections;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using SpotiNet.Client.Http;

//429 → honors Retry-After and retries once.
//503 → uses strategy backoff and retries.
//Cancellation → stops before sleeping/retrying.
public class RetryAfterDelegatingHandlerTests
{
    [Fact]
    public async Task Retries_On429_HonorsRetryAfterHeader()
    {
        var seq = new SequenceHandler(new[]
        {
            MakeResponse(HttpStatusCode.TooManyRequests, "{\"error\":\"rate\"}", raSeconds: 1),
            MakeResponse(HttpStatusCode.OK, "{\"ok\":true}")
        });

        var delays = new List<TimeSpan>();
        var delayStrategy = new TestDelayStrategy(d => delays.Add(d));

        var retry = new RetryAfterDelegatingHandler(delayStrategy, maxRetries: 3) { InnerHandler = seq };
        var http = new HttpClient(retry) { BaseAddress = new Uri("https://api.spotify.com/v1/") };

        var res = await http.GetAsync("artist/anything");

        // Assert

        Assert.Equal(2, seq.CallCount);
        Assert.Single(delays);
        Assert.InRange(delays[0].TotalSeconds, 0.9, 1.1); // ~1s from header
        Assert.True(res.IsSuccessStatusCode);

    }

    [Fact]
    public async Task Retries_On5xx_UsesBackoff_FromStrategy()
    {
        // Arrange: 503 then 200
        var seq = new SequenceHandler(new[]
        {
            MakeResponse(HttpStatusCode.ServiceUnavailable, "down"),
            MakeResponse(HttpStatusCode.OK, "ok")
        });

        var delays = new List<TimeSpan>();
        var delayStrategy = new TestDelayStrategy(d => delays.Add(d), backOffFactory: attempt => TimeSpan.FromMilliseconds(10 * attempt));

        var retry = new RetryAfterDelegatingHandler(delayStrategy, maxRetries: 3) { InnerHandler = seq };
        var http = new HttpClient(retry) { BaseAddress = new Uri("https://api.spotify.com/v1/") };
;

        // Act
        var res = await http.GetAsync("anything");

        // Assert
        Assert.Equal(2, seq.CallCount);           // one retry
        Assert.Single(delays);
        Assert.Equal(TimeSpan.FromMilliseconds(10), delays[0]); // attempt=1
        Assert.True(res.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Cancellation_AbortsBeforeRetryDelay()
    {
        // Arrange: immediate 429 with Retry-After 2s, but CT already cancelled
        var seq = new SequenceHandler(new[]
        {
            MakeResponse(HttpStatusCode.TooManyRequests, "slow", raSeconds: 2),
        });

        var delayStrategy = new TestDelayStrategy(_ => { /* would be called, but should be canceled */ });

        var retry = new RetryAfterDelegatingHandler(delayStrategy, maxRetries: 3) { InnerHandler = seq };
        var http = new HttpClient(retry) { BaseAddress = new Uri("https://api.spotify.com/v1/") };
;

        var cts = new CancellationTokenSource();
        cts.Cancel(); // already canceled

        // Act + Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => http.GetAsync("anything", cts.Token));
        Assert.Equal(1, seq.CallCount); // no additional retries
    }

    //helpers
    /// <summary>
    /// Creates a HttpResponseMessage using the provide status code, body and RetryAfter time
    /// </summary>
    /// <param name="code">Status code</param>
    /// <param name="body">Message content</param>
    /// <param name="raSeconds">Message retry after time</param>
    /// <returns>A HttpResponse Message</returns>
    private static HttpResponseMessage MakeResponse(HttpStatusCode code, string body, int? raSeconds = null)
    {
        var msg = new HttpResponseMessage(code)
        {
            Content = new StringContent(body, Encoding.UTF8, "application/json")
        };

        if (raSeconds.HasValue)
            msg.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromSeconds(raSeconds.Value));
        return msg;
    }

    private sealed class SequenceHandler : HttpMessageHandler
    {
        private readonly Queue<HttpResponseMessage> _responses;
        public int CallCount { get; private set; }

        public SequenceHandler(IEnumerable<HttpResponseMessage> responses)
        {
            _responses = new Queue<HttpResponseMessage>(responses);
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage req,
            CancellationToken ct)
        {
            CallCount++;
            if (_responses.Count == 0)
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
            return Task.FromResult(_responses.Dequeue());

        }
    }

    private sealed class TestDelayStrategy : IRetryDelayStrategy
    {
        //simulate delay and backing off
        private readonly Action<TimeSpan> _onDelay;
        private readonly Func<int, TimeSpan> _backOffFactory;

        public TestDelayStrategy(
            Action<TimeSpan> onDelay,
            Func<int, TimeSpan>? backOffFactory = null
        )
        {
            _onDelay = onDelay;
            _backOffFactory = backOffFactory ?? (n => TimeSpan.FromMilliseconds(1));
        }

        public TimeSpan ComputeBackOffDelay(int attempts)
        {
            return _backOffFactory(attempts);
        }

        public Task DelayAsync(TimeSpan delay, CancellationToken ct)
        {
            _onDelay(delay);
            if (ct.IsCancellationRequested) return Task.FromCanceled(ct);
            return Task.CompletedTask;
        }

    }
}