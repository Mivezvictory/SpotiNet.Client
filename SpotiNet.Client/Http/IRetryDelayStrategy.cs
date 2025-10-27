using System;
using System.Threading;
using System.Threading.Tasks;

namespace SpotiNet.Client.Http;

/// <summary>
/// Strategy for computing and awaiting retry delays.
/// </summary>
internal interface IRetryDelayStrategy
{
    /// <summary>
    /// Computes delay for a transient error when server does not supply one.
    /// </summary>
    /// <param name="attempt"></param>
    /// <returns></returns>
    TimeSpan ComputeBackOffDelay(int attempt);

    /// <summary>
    /// Awaits the given delay.
    /// Honour the cancellation token 
    /// </summary>
    /// <param name="delay">The given delay Timespan</param>
    /// <param name="ct">The CancellationToken</param>
    /// <returns></returns>
    Task DelayAsync(TimeSpan delay, CancellationToken ct);
}