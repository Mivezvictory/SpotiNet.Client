using System.Threading;
using System.Threading.Tasks;
using SpotiNet.Client.Models;

namespace SpotiNet.Client;

/// <summary>
/// Albums endpoints.
/// </summary>
public interface IAlbumsApi
{
    /// <summary>
    /// GET /v1/albums/{id} - Get Spotify catalog information for a single album.
    /// Works with client-credentials or user tokens.
    /// </summary>
    /// <param name="albumId">The Spotify ID of the album</param>
    /// <param name="market">(optional) An ISO 3166-1 alpha-2 country code to specify available content</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The album object</returns>
    Task<Album> GetAsync(
        string albumId,
        string? market = null,
        CancellationToken ct = default);
}
