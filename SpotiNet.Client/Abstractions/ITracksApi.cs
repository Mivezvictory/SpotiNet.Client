using System.Threading;
using System.Threading.Tasks;
using SpotiNet.Client.Models;

namespace SpotiNet.Client;

/// <summary>
/// Tracks endpoints.
/// </summary>
public interface ITracksApi
{
    /// <summary>
    /// GET /v1/tracks/{id} - Get Spotify catalog information for a single track.
    /// Works with client-credentials or user tokens.
    /// </summary>
    /// <param name="trackId">The Spotify ID of the track</param>
    /// <param name="market">(optional) An ISO 3166-1 alpha-2 country code to specify available content</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The track object</returns>
    Task<Track> GetAsync(
        string trackId,
        string? market = null,
        CancellationToken ct = default);
}
