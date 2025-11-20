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

    /// <summary>
    /// Artists endpoints.
    /// </summary>
    IArtistsApi Artists { get; }

    /// <summary>
    /// Albums endpoints.
    /// </summary>
    IAlbumsApi Albums { get; }

    /// <summary>
    /// Tracks endpoints.
    /// </summary>
    ITracksApi Tracks { get; }

    /// <summary>
    /// Search endpoints
    /// </summary>
    ISearchApi Search { get; }

    
}