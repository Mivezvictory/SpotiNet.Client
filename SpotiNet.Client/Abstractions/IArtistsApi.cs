using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotiNet.Client.Models;

namespace SpotiNet.Client;

/// <summary>
/// Artists endpoints.
/// </summary>
public interface IArtistsApi
{
    /// <summary>
    /// GET /v1/artists/{id} - Get Spotify catalog information for a single artist.
    /// Works with client-credentials or user tokens.
    /// </summary>
    /// <param name="artistId">The Spotify ID of the artist</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The artist object</returns>
    Task<Artist> GetAsync(
        string artistId,
        CancellationToken ct = default);

    /// <summary>
    /// GET /v1/artists - Get Spotify catalog information for several artists based on their Spotify IDs.
    /// Works with client-credentials or user tokens.
    /// </summary>
    /// <param name="artistIds">A collection of Spotify IDs for artists (up to 50 IDs)</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>A collection of artist objects</returns>
    Task<IReadOnlyList<Artist>> GetSeveralAsync(
        IEnumerable<string> artistIds,
        CancellationToken ct = default);
}
