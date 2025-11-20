using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotiNet.Client.Models;

namespace SpotiNet.Client;

/// <summary>
/// Playlists endpoints.
/// </summary>
public interface IPlaylistsApi
{
    /// <summary>
    /// POST /v1/users/{user_id}/playlists - Create a new playlist for a user.
    /// Scopes: playlist-modify-public or playlist-modify-private (depending on isPublic parameter).
    /// </summary>
    /// <param name="userId">The user's Spotify user ID</param>
    /// <param name="name">The name for the new playlist</param>
    /// <param name="description">(optional) The playlist description as displayed in Spotify Clients and in the Web API</param>
    /// <param name="isPublic">(optional) Whether the playlist will be public (true) or private (false). Default: false</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The newly created playlist object</returns>
    Task<Playlist> CreateAsync(
        string userId, 
        string name, 
        string? description = null, 
        bool isPublic = false, 
        CancellationToken ct = default);

    /// <summary>
    /// POST /v1/playlists/{playlist_id}/tracks - Add one or more items to a user's playlist.
    /// Scopes: playlist-modify-public or playlist-modify-private.
    /// </summary>
    /// <param name="playlistId">The Spotify ID of the playlist</param>
    /// <param name="trackUris">A collection of Spotify URIs to add (e.g., "spotify:track:4iV5W9uYEdYUVa79Axb7Rh")</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task AddItemsAsync(
        string playlistId, 
        IEnumerable<string> trackUris, 
        CancellationToken ct = default);

    /// <summary>
    /// DELETE /v1/playlists/{playlist_id}/tracks - Remove one or more items from a user's playlist.
    /// Scopes: playlist-modify-public or playlist-modify-private.
    /// </summary>
    /// <param name="playlistId">The Spotify ID of the playlist</param>
    /// <param name="trackUris">A collection of Spotify URIs to remove (e.g., "spotify:track:4iV5W9uYEdYUVa79Axb7Rh")</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task RemoveItemsAsync(
        string playlistId,
        IEnumerable<string> trackUris,
        CancellationToken ct = default);

    /// <summary>
    /// GET /v1/playlists/{playlist_id} - Get a playlist owned by a Spotify user.
    /// Scopes: none required for public playlists, playlist-read-private for private playlists.
    /// </summary>
    /// <param name="playlistId">The Spotify ID of the playlist</param>
    /// <param name="market">(optional) An ISO 3166-1 alpha-2 country code to specify available content</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The playlist object</returns>
    Task<Playlist> GetAsync(
        string playlistId,
        string? market = null,
        CancellationToken ct = default);

    /// <summary>
    /// GET /v1/me/playlists - Get a list of the playlists owned or followed by the current Spotify user.
    /// Scopes: playlist-read-private for private playlists, playlist-read-collaborative for collaborative playlists.
    /// </summary>
    /// <param name="limit">(optional) The maximum number of items to return. Default: 20, Min: 1, Max: 50</param>
    /// <param name="offset">(optional) The index of the first playlist to return. Default: 0</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>A paged collection of playlists</returns>
    Task<Paging<PlaylistSimple>> GetCurrentUserPlaylistsAsync(
        int? limit = 20,
        int? offset = 0,
        CancellationToken ct = default);
}
