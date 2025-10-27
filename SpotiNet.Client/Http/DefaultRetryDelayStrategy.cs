using System;
using System.Threading;
using System.Threading.Tasks;

namespace SpotiNet.Client.Http;

internal sealed class DefaultRetyDelayStrategy : IRetryDelayStrategy
{
    private static readonly Random _random = new();
    public TimeSpan ComputeBackOffDelay(int attempt)
    {
        var baseMs = 250 * (int)Math.Pow(2, attempt - 1);
        var jitter = _random.Next(0, 200);
        return TimeSpan.FromMilliseconds(baseMs + jitter);
    }

    public Task DelayAsync(TimeSpan delay, CancellationToken ct)
    {
        return Task.Delay(delay, ct);
    }
}