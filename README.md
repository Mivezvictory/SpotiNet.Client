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
    - `Playlists.CreateAsync` / `AddItemsAsync`: `playlist-modify-public` or `playlist-modify-private` (match `isPublic`)
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

**Search usage (streaming results)**
```csharp
var api = services.GetRequiredService<ISpotifyClient>();

// Search for tracks - returns IAsyncEnumerable that automatically paginates
await foreach (var track in api.Search.SearchTracksAsync("Bohemian Rhapsody", limit: 20))
{
    Console.WriteLine($"{track.Name} by {string.Join(", ", track.Artists?.Select(a => a.Name) ?? [])}");
}

// Search for artists
await foreach (var artist in api.Search.SearchArtistsAsync("Queen"))
{
    Console.WriteLine($"{artist.Name} - Popularity: {artist.Popularity}");
}

// Search for albums
await foreach (var album in api.Search.SearchAlbumsAsync("A Night at the Opera"))
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