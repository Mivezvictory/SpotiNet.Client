using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SpotiNet.Client.Http;

/// <summary>
/// Retries Spotify requests on 429 (hobouring Retry-After)
/// </summary>
internal sealed class RetryAfterDelegatingHandler : DelegatingHandler
{
    private readonly IRetryDelayStrategy _delay;
    private readonly int _maxRetries;

    public RetryAfterDelegatingHandler(IRetryDelayStrategy delay) : this(delay, 3)
    { }

    internal RetryAfterDelegatingHandler(
        IRetryDelayStrategy delay,
        int maxRetries
    )
    {
        _delay = delay;
        _maxRetries = Math.Max(0, maxRetries);
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken ct

    )
    {
        var attempt = 0;
        while (true)
        {
            attempt++;
            var response = await base.SendAsync(request, ct);
            if (!IsTransient(response) || attempt > _maxRetries)
                return response;

            var serverDelay = GetServerSiggestedDelay(response);
            var delay = serverDelay ?? _delay.ComputeBackOffDelay(attempt);

            if (delay > TimeSpan.Zero)
                await _delay.DelayAsync(delay, ct);

            response.Dispose();
        }
    }

    private static bool IsTransient(HttpResponseMessage res)
    {
        var code = (int)res.StatusCode;
        return code == 429 || (code >= 500 && code < 600);
    }
    
    private static TimeSpan? GetServerSiggestedDelay(HttpResponseMessage res)
    {
        if ((int)res.StatusCode != 429) return null;

        var retryAfter = res.Headers.RetryAfter;
        if (retryAfter == null) return null;

        if (retryAfter.Delta is TimeSpan timeSpan && timeSpan >= TimeSpan.Zero)
            return timeSpan;

        if (retryAfter.Date is DateTimeOffset when)
        {
            var remainingTime = when - DateTimeOffset.UtcNow;
            return remainingTime > TimeSpan.Zero ? remainingTime : TimeSpan.Zero;
        }

        return null;
    }


}