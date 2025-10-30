namespace SpotiNet.Client.Models;

/// <summary>
/// Response shape for GET /me/top/artists.
/// </summary>
public sealed class TopArtistsResponse : Paging<Artist> { }

/// <summary>
/// Response shape for GET /me/top/tracks.
/// </summary>
public sealed class TopTracksResponse : Paging<Track> { }
