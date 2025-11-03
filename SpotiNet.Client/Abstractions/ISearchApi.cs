using System.Collections.Generic;
using System.Threading;
using SpotiNet.Client.Models;

namespace SpotiNet.Client;
/// <summary>
/// 
/// </summary>
public interface ISearchApi
{

    /// <summary>
    ///  Stream tracks that match <paramref name="query"/> using Spotify paging
    ///  Works with client-credentials or user tokens.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="limit"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    IAsyncEnumerable<Track> SearchTracksAsync(
        string query,
        int? limit = 20,
        CancellationToken ct = default);
}