## SpotiNet.Client
SpotiNet.Client is an intuitive .NET C# library to access the Spotify REST APIs. 

**What's included**
- DI-first client with retries for `429` (honors `Retry-After`) + basic `5xx`
- Typed APIs :
    - Users
        - `Users.GetMeAsync()`
        - `Users.GetUserTopArtistsAsync(timeRange = "medium_term, limt = 20, offset =0)`
        - `Users.GetUserTopTracksAsync(timeRange = "medium_term, limt = 20, offset =0)`
        - `Users.GetUserAsync(userId)`
    - Playlists
        - `Playlists.CreateAsync(userId, name, description?, isPublic?)`
        - `Playlists.AddItemsAsync(playlistId, IEnumerable<string> trackUris)`
        - `Playlists.RemoveItemsAsync(playlistId, IEnumerable<string> trackUris)`
        - `Playlists.GetAsync(playlistId, market?)`
        - `Playlists.GetCurrentUserPlaylistsAsync(limit?, offset?)`
    - Artists
        - `Artists.GetAsync(artistId)`
        - `Artists.GetSeveralAsync(IEnumerable<string> artistIds)`
    - Albums
        - `Albums.GetAsync(albumId, market?)`
        - `Albums.GetSeveralAsync(IEnumerable<string> albumIds, market?)`
    - Tracks
        - `Tracks.GetAsync(trackId, market?)`
        - `Tracks.GetSeveralAsync(IEnumerable<string> trackIds, market?)`
    - Search (streaming results with automatic pagination)
        - `Search.SearchTracksAsync(query, limit?, offset?, market?, includeExternal?)`
        - `Search.SearchArtistsAsync(query, limit?, offset?, market?, includeExternal?)`
        - `Search.SearchAlbumsAsync(query, limit?, offset?, market?, includeExternal?)`
- Clean errors via `SpotifyApiException` (status + message + raw body)

**Tokens & scopes**
- These endpoints require a **user access token** (not client-credentials).
- Scopes:
    - `Users.GetMeAsync`: none for basic profile (add `user-read-email` if you need email & `user-read-private` if you need user country)
    - `Users.GetUserAsync`: `user-top-read`
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

// 2) Create a playlist
var playlist = await api.Playlists.CreateAsync(me.Id, "SpotiNet Test", "Created via SpotiNet.Client", isPublic: false);

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
Console.WriteLine($"Tracks: {playlist.Tracks?.Total}");

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

// Get several artists at once (up to 50)
var artists = await api.Artists.GetSeveralAsync(new[]
{
    "2QsynagSdAqZj3U9HgDzjD", // Bob Marley
    "2VAvhf61HjWOeE3NPa7Msw"  // Fela Kuti
});
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

// Get several albums at once (up to 20)
var albums = await api.Albums.GetSeveralAsync(new[]
{
    "4LH4d3cOWNNsVw41Gqt2kv", // The Dark Side of the Moon
    "4R2kfJ5lfYTmPAV9jkMWTd"  // Graceland by Paul Simon
});
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

// Get several tracks at once (up to 50)
var tracks = await api.Tracks.GetSeveralAsync(new[]
{
    "3ZOEjWhUKEahPYCdALWEAC", // Water No Get Enemy
    "3DXncPQOG4VBw3QHh3S817"  // Zombie by Fela Kuti
});
foreach (var t in tracks)
{
    Console.WriteLine($"{t.Name} - Popularity: {t.Popularity}");
}
```

**Search usage (streaming results)**
```csharp
var api = services.GetRequiredService<ISpotifyClient>();

// Search for tracks - returns IAsyncEnumerable that automatically paginates
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