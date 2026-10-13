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
}
