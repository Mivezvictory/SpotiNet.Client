using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SpotiNet.Client;

/// <summary>Playlists endpoints.</summary>
public interface IPlaylistsApi
{
    /// <summary>
    /// POST /v1/users/{user_id}/playlists — create a playlist.
    /// Scopes: playlist-modify-public or playlist-modify-private.
    /// </summary>
    Task<Playlist> CreateAsync(string userId, string name, string? description = null, bool isPublic = false, CancellationToken ct = default);

    /// <summary>
    /// POST /v1/playlists/{playlist_id}/tracks — add tracks by URI.
    /// Scopes: playlist-modify-public or playlist-modify-private.
    /// </summary>
    Task AddItemsAsync(string playlistId, IEnumerable<string> trackUris, CancellationToken ct = default);
}
