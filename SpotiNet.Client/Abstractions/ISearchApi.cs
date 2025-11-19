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
    ///  GET /v1/search/track - Stream tracks that match <paramref name="query"/> using Spotify paging
    ///  Works with client-credentials or user tokens.
    /// </summary>
    /// <param name="query">Your search query</param>
    /// <param name="market">(optional) An ISO 3166-1 alpha-2 country code to specify available content.</param>
    /// <param name="includeExternal">(optional) Signals that the client can play externally hosted audio content, and marks the content as playable in the response. </param>
    /// <param name="limit">(optional) The maximum number of results to return in each item type. Default: 20</param>
    /// <param name="offset">(optional) The index of the first result to return. Default: 0</param>
    /// <param name="ct"></param>
    /// <returns></returns>
    IAsyncEnumerable<Track> SearchTracksAsync(
        string query,
        int? limit = 20,
        int? offset = 0,
        string? market = null,
        string? includeExternal = null,
        CancellationToken ct = default);

    /// <summary>
    ///  GET /v1/search/artists - Stream artist that match <paramref name="query"/> using Spotify paging
    ///  Works with client-credentials or user tokens.
    /// </summary>
    /// <param name="query">Your search query</param>
    /// <param name="market">(optional) An ISO 3166-1 alpha-2 country code to specify available content.</param>
    /// <param name="includeExternal">(optional) Signals that the client can play externally hosted audio content, and marks the content as playable in the response. </param>
    /// <param name="limit">(optional) The maximum number of results to return in each item type. Default: 20</param>
    /// <param name="offset">(optional) The index of the first result to return. Default: 0</param>
    /// <param name="ct"></param>
    /// <returns></returns>
    IAsyncEnumerable<Artist> SearchArtistsAsync(
        string query,
        int? limit = 20,
        int? offset = 0,
        string? market = null,
        string? includeExternal = null,
        CancellationToken ct = default);

    ///// <summary>
    /////  GET /v1/search/albums - Stream albums that match <paramref name="query"/> using Spotify paging
    /////  Works with client-credentials or user tokens.
    ///// </summary>
    ///// <param name="query">Your search query</param>
    ///// <param name="market">(optional) An ISO 3166-1 alpha-2 country code to specify available content.</param>
    ///// <param name="includeExternal">(optional) Signals that the client can play externally hosted audio content, and marks the content as playable in the response. </param>
    ///// <param name="limit">(optional) The maximum number of results to return in each item type. Default: 20</param>
    ///// <param name="offset">(optional) The index of the first result to return. Default: 0</param>
    ///// <param name="ct"></param>
    ///// <returns></returns>
    //IAsyncEnumerable<Album> SearchAlbumsAsync(
    //    string query,
    //    int? limit = 20,
    //    int? offset = 0,
    //    string? market = null,
    //    string? includeExternal = null,
    //    CancellationToken ct = default);
}