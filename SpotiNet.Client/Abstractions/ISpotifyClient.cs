namespace SpotiNet.Client;

/// <summary>
/// Root entry point for typed Spotify Web API access
/// </summary>
public interface ISpotifyClient
{
    /// <summary>
    /// Users endpoints.
    /// </summary>
    IUsersApi Users { get; }

    /// <summary>
    /// Playlists endpoints.
    /// </summary>
    IPlaylistsApi Playlists { get; }
}