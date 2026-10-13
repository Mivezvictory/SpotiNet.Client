## SpotiNet.Client
> SpotiNet.Client is an intuitive .NET C# library to access the Spotify(unofficial) REST APIs. 
> You are responsible for complying with Spotify’s Developer Terms and any applicable rate limits or usage policies.

**Breaking changes in 2.0.0**

Spotify removed or reshaped several endpoints in its Feb/March 2026 Web API changes. This release follows suit:
- `Users.GetUserAsync(userId)` is **removed** — Spotify no longer exposes any way to fetch another user's profile (`GET /users/{id}` is gone); only `GetMeAsync()` for the token owner remains.
- `Artists.GetSeveralAsync`, `Albums.GetSeveralAsync`, `Tracks.GetSeveralAsync` are **removed** — the bulk-fetch-by-ids endpoints were removed with no replacement. Fetch items individually with `GetAsync` instead.
- `Playlists.CreateAsync` no longer takes a `userId` — the new `POST /me/playlists` endpoint only ever creates a playlist for the token owner.
- `Playlists.AddItemsAsync` gained an optional `position` parameter (the new `/items` endpoint supports inserting at a specific index).
- `Playlist.Tracks` is renamed to `Playlist.Items`, and `PlaylistItem.Track` is renamed to `PlaylistItem.Item`, matching Spotify's own field rename.
- `Search.*` default/max `limit` dropped from 20/50 to 5/10 (matching Spotify's new per-request page cap); requesting a larger total still works, it just takes more page fetches under the hood.

**What's included**
- DI-first client with retries for `429` (honors `Retry-After`) + basic `5xx`
- Typed APIs :
    - Users
        - `Users.GetMeAsync()`
        - `Users.GetUserTopArtistsAsync(timeRange?, limt?, offset?)`
        - `Users.GetUserTopTracksAsync(timeRange?, limt?, offset?)`
    - Playlists
        - `Playlists.CreateAsync(name, description?, isPublic?)`
        - `Playlists.AddItemsAsync(playlistId, IEnumerable<string> trackUris, position?)`
        - `Playlists.RemoveItemsAsync(playlistId, IEnumerable<string> trackUris)`
        - `Playlists.GetAsync(playlistId, market?)`
        - `Playlists.GetCurrentUserPlaylistsAsync(limit?, offset?)`
    - Artists
        - `Artists.GetAsync(artistId)`
    - Albums
        - `Albums.GetAsync(albumId, market?)`
    - Tracks
        - `Tracks.GetAsync(trackId, market?)`
    - Search (streaming results with automatic pagination)
        - `Search.SearchTracksAsync(query, limit?, offset?, market?, includeExternal?)`
        - `Search.SearchArtistsAsync(query, limit?, offset?, market?, includeExternal?)`
        - `Search.SearchAlbumsAsync(query, limit?, offset?, market?, includeExternal?)`
- Clean errors via `SpotifyApiException` (status + message + raw body)

**Tokens & scopes**
- These endpoints require a **user access token** (not client-credentials).
- Scopes:
    - `Users.GetMeAsync`: none for basic profile (add `user-read-email` if you need email & `user-read-private` if you need user country)
    - `Playlists.CreateAsync` / `AddItemsAsync` / `RemoveItemsAsync`: `playlist-modify-public` or `playlist-modify-private` (match `isPublic`)
    - `Playlists.GetAsync`: none for public playlists, `playlist-read-private` for private playlists
    - `Playlists.GetCurrentUserPlaylistsAsync`: `playlist-read-private` for private playlists, `playlist-read-collaborative` for collaborative playlists
    - `Artists.*`: works with both **user tokens** and **client-credentials** tokens
    - `Albums.*`: works with both **user tokens** and **client-credentials** tokens
    - `Tracks.*`: works with both **user tokens** and **client-credentials** tokens
    - `Search.*`: works with both **user tokens** and **client-credentials** tokens

### Install

```bash
dotnet add package SpotiNet.Client
```

**Register**
```csharp
    using SpotiNet.Client;

    builder.Services.AddSpotifyClient(); // reads token from env var SPOTIFY_ACCESS_TOKEN by default

```
`Provide your own token source by implementing IAccessTokenProvider and passing a factory to AddSpotifyClient(...).`


**Minimal usage (create playlist + add tracks)**
```csharp
var api = services.GetRequiredService<ISpotifyClient>();

// 1) Who am I?
var me = await api.Users.GetMeAsync();
Console.WriteLine($"Hello, {me.DisplayName} ({me.Id})");

// 2) Create a playlist (always created for the current user)
var playlist = await api.Playlists.CreateAsync("SpotiNet Test", "Created via SpotiNet.Client", isPublic: false);

// 3) Add tracks
await api.Playlists.AddItemsAsync(playlist.Id, new[]
{
    "spotify:track:5BLrEOEDKoDDg5T8PzdIHN",
    "spotify:track:77Ie9frENeQwYUGHrrS0pk"
});

Console.WriteLine($"Playlist created: {playlist.Name} ({playlist.Id})");
```

**Playlist management usage**
```csharp
var api = services.GetRequiredService<ISpotifyClient>();

// Get a specific playlist
var playlist = await api.Playlists.GetAsync("37i9dQZF1DXcBWIGoYBM5M");
Console.WriteLine($"Playlist: {playlist.Name} by {playlist.Owner?.DisplayName}");
Console.WriteLine($"Tracks: {playlist.Items?.Total}");

// Get all current user's playlists (paginated)
var myPlaylists = await api.Playlists.GetCurrentUserPlaylistsAsync(limit: 50);
foreach (var pl in myPlaylists.Items ?? [])
{
    Console.WriteLine($"{pl.Name} ({pl.Id})");
}

// Remove tracks from a playlist
await api.Playlists.RemoveItemsAsync(playlist.Id, new[]
{
    "spotify:track:5BLrEOEDKoDDg5T8PzdIHN"
});
```

**Artists usage**
```csharp
var api = services.GetRequiredService<ISpotifyClient>();

// Get a single artist
var artist = await api.Artists.GetAsync("2QsynagSdAqZj3U9HgDzjD"); // Bob Marley
Console.WriteLine($"{artist.Name} - Followers: {artist.Followers?.Total}");
Console.WriteLine($"Genres: {string.Join(", ", artist.Genres ?? [])}");

// Spotify removed bulk "get several" fetches, so fetch individually (in parallel if you like)
var artistIds = new[] { "2QsynagSdAqZj3U9HgDzjD", "2VAvhf61HjWOeE3NPa7Msw" }; // Bob Marley, Fela Kuti
var artists = await Task.WhenAll(artistIds.Select(id => api.Artists.GetAsync(id)));
foreach (var a in artists)
{
    Console.WriteLine($"{a.Name} - Popularity: {a.Popularity}");
}
```

**Albums usage**
```csharp
var api = services.GetRequiredService<ISpotifyClient>();

// Get a single album
var album = await api.Albums.GetAsync("4LH4d3cOWNNsVw41Gqt2kv"); // The Dark Side of the Moon by Pink Floyd
Console.WriteLine($"{album.Name} by {string.Join(", ", album.Artists?.Select(a => a.Name) ?? [])}");
Console.WriteLine($"Release Date: {album.ReleaseDate}");
Console.WriteLine($"Total Tracks: {album.Tracks?.Total}");

// Spotify removed bulk "get several" fetches, so fetch individually (in parallel if you like)
var albumIds = new[] { "4LH4d3cOWNNsVw41Gqt2kv", "4R2kfJ5lfYTmPAV9jkMWTd" }; // Dark Side of the Moon, Graceland
var albums = await Task.WhenAll(albumIds.Select(id => api.Albums.GetAsync(id)));
foreach (var alb in albums)
{
    Console.WriteLine($"{alb.Name} - Popularity: {alb.Popularity}");
}
```

**Tracks usage**
```csharp
var api = services.GetRequiredService<ISpotifyClient>();

// Get a single track
var track = await api.Tracks.GetAsync("3ZOEjWhUKEahPYCdALWEAC"); // Water No Get Enemy by Fela Kuti
Console.WriteLine($"{track.Name} by {string.Join(", ", track.Artists?.Select(a => a.Name) ?? [])}");
Console.WriteLine($"Album: {track.Album?.Name}");
Console.WriteLine($"Duration: {TimeSpan.FromMilliseconds(track.DurationMs ?? 0)}");

// Spotify removed bulk "get several" fetches, so fetch individually (in parallel if you like)
var trackIds = new[] { "3ZOEjWhUKEahPYCdALWEAC", "3DXncPQOG4VBw3QHh3S817" }; // Water No Get Enemy, Zombie
var tracks = await Task.WhenAll(trackIds.Select(id => api.Tracks.GetAsync(id)));
foreach (var t in tracks)
{
    Console.WriteLine($"{t.Name} - Popularity: {t.Popularity}");
}
```

**Search usage (streaming results)**
```csharp
var api = services.GetRequiredService<ISpotifyClient>();

// Search for tracks - returns IAsyncEnumerable that automatically paginates.
// limit is the total you want across all pages; Spotify's own per-request page cap is 10.
await foreach (var track in api.Search.SearchTracksAsync("No Woman No Cry", limit: 20))
{
    Console.WriteLine($"{track.Name} by {string.Join(", ", track.Artists?.Select(a => a.Name) ?? [])}");
}

// Search for artists
await foreach (var artist in api.Search.SearchArtistsAsync("Bob Marley"))
{
    Console.WriteLine($"{artist.Name} - Popularity: {artist.Popularity}");
}

// Search for albums
await foreach (var album in api.Search.SearchAlbumsAsync("Legend"))
{
    Console.WriteLine($"{album.Name} by {string.Join(", ", album.Artists?.Select(a => a.Name) ?? [])}");
}
```

**Supplying a user token**
```bash
# Powershell
$env:SPOTIFY_ACCESS_TOKEN="eyJhbGciOi..."

# bash/zsh
export SPOTIFY_ACCESS_TOKEN="eyJhbGciOi..."
```

**Or implement:**
```csharp
public sealed class MyUserTokenProvider : IAccessTokenProvider
{
    public Task<string> GetAccessTokenAsync(CancellationToken ct=default)
        => Task.FromResult(GetFromMyAuthSystem());
}

// Registration:
services.AddSpotifyClient(tokenProviderFactory: _ => new MyUserTokenProvider());
```

**Errors**

Non-2xx results throw SpotifyApiException:

- StatusCode (e.g., 401, 403, 429)
- Message (Spotify’s error.message when available)
- ResponseBody (raw response)
