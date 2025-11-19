using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SpotiNet.Client.Models;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;


namespace SpotiNet.Client;

/// <summary>
/// Concrete implementation of ISpotifyClient and sub-APIs.
/// Current capabilities - pipeline (auth + retries + error mapping).
/// </summary>
internal sealed class SpotifyClient : ISpotifyClient, IUsersApi, IPlaylistsApi, ISearchApi
{
    private readonly HttpClient _http;
    private readonly JsonSerializerOptions _json;
    private readonly ILogger<SpotifyClient> _log;

    public SpotifyClient(
        HttpClient http,
        IOptions<SpotifyClientOptions> opts,
        ILogger<SpotifyClient> log)
    {
        _http = http;
        _json = opts.Value.Json;
        _log = log;
    }

    public IUsersApi Users => this;
    public IPlaylistsApi Playlists => this;

    public ISearchApi Search => this;

    // -------- Core send/deserialize ----------
    private async Task<T> SendAsync<T>(HttpRequestMessage req, CancellationToken ct)
    {
        using var res = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);
        var body = await res.Content.ReadAsStringAsync(ct);

        if (!res.IsSuccessStatusCode)
        {
            var msg = SpotifyErrorParser.TryReadMessage(body) ?? $"HTTP {(int)res.StatusCode} {res.ReasonPhrase}";
            _log.LogError("Spotify {Method} {Url} failed: {Status} {Message}", req.Method, req.RequestUri, (int)res.StatusCode, msg);
            throw new SpotifyApiException(res.StatusCode, msg, body);
        }

        _log.LogDebug("Response body length: {Length} chars", body?.Length ?? 0);
        
        var data = JsonSerializer.Deserialize<T>(body, _json);
        if (data is null)
            throw new SpotifyApiException(HttpStatusCode.InternalServerError, "Failed to deserialize Spotify response.", body);

        return data;
    }

    // -------- Users ----------
    async Task<CurrentUserProfile> IUsersApi.GetMeAsync(CancellationToken ct)
        => await SendAsync<CurrentUserProfile>(new(HttpMethod.Get, "me"), ct);

    async Task<TopArtistsResponse> IUsersApi.GetUserTopArtistsAsync(
        string? timeRange,
        int? limit,
        int? offset,
        CancellationToken ct)
    {
        using var req = new HttpRequestMessage(HttpMethod.Get, $"me/top/artists?time_range={timeRange}&limit={limit}&offset={offset}");
        return await SendAsync<TopArtistsResponse>(req, ct);
    }

    async Task<TopTracksResponse> IUsersApi.GetUserTopTracksAsync(
        string? timeRange,
        int? limit,
        int? offset,
        CancellationToken ct)
    {
        
        using var req = new HttpRequestMessage(HttpMethod.Get, $"me/top/tracks?time_range={timeRange}&limit={limit}&offset={offset}");
        return await SendAsync<TopTracksResponse>(req, ct);
    }
    
    async Task<PublicUser> IUsersApi.GetUserAsync(
        string userId,
        CancellationToken ct)
    => await SendAsync<PublicUser>(new(HttpMethod.Get, $"users/{userId}"), ct);


    // -------- Playlists ----------
    async Task<Playlist> IPlaylistsApi.CreateAsync(
        string userId,
        string name,
        string? description,
        bool isPublic,
        CancellationToken ct)
    {
        var payload = new { name, description, @public = isPublic };
        using var req = new HttpRequestMessage(HttpMethod.Post, $"users/{Uri.EscapeDataString(userId)}/playlists")
        { Content = JsonContent.Create(payload) };
        return await SendAsync<Playlist>(req, ct);
    }

    async Task IPlaylistsApi.AddItemsAsync(
        string playlistId,
        IEnumerable<string> trackUris,
        CancellationToken ct)
    {
        var payload = new { uris = System.Linq.Enumerable.ToArray(trackUris) };
        using var req = new HttpRequestMessage(HttpMethod.Post, $"playlists/{Uri.EscapeDataString(playlistId)}/tracks")
        { Content = JsonContent.Create(payload) };
        _ = await SendAsync<JsonElement>(req, ct); // returns snapshot_id; not needed here
    }

    // -------- Search ----------
    private sealed class SearchTracksEnvelope
    {
        [JsonPropertyName("tracks")]
        public Paging<Track>? Tracks { get; set; }
    }
    
    private sealed class SearchArtistsEnvelope
    {
        [JsonPropertyName("artists")]
        public Paging<Artist>? Artists { get; set; }
    }



    async IAsyncEnumerable<Track> ISearchApi.SearchTracksAsync(
        string query,
        int? limit,
        int? offset,
        string? market,
        string? includeExternal,
        [EnumeratorCancellation] CancellationToken ct)
    {
        // Start with a relative URL; Spotify returns an absolute 'next'
        //string? url = $"search?q={Uri.EscapeDataString(query)}&type=track&limit={limit}&offset={offset}";
        string? url = BuildUrl(
                "search",
                ("q", query),
                ("type", "track"),
                ("limit", limit?.ToString()),
                ("offset", offset?.ToString()),
                ("market", market),
                ("include_external", includeExternal)
        );

        await foreach (var t in StreamPagesAsync(
            initialUrl: url,
            fetchPage: async (url, token) =>
            {
                using var req = new HttpRequestMessage(HttpMethod.Get, url);
                var env = await SendAsync<SearchTracksEnvelope>(req, ct);
                var tracks = env.Tracks;
                return new Page<Track>
                {
                    Items = tracks.Items ?? new List<Track>(),
                    Next = tracks.Next,
                    Total = tracks.Total
                };
            },
            1000,
            ct
            ))
        {
            yield return t;
        } 
    }

    async IAsyncEnumerable<Artist> ISearchApi.SearchArtistsAsync(
       string query,
       int? limit,
       int? offset,
       string? market,
       string? includeExternal,
       [EnumeratorCancellation] CancellationToken ct)
    {
        // Start with a relative URL; Spotify returns an absolute 'next'
        //string? url = $"search?q={Uri.EscapeDataString(query)}&type=artist&limit={limit}&offset={offset}";
        string? url = BuildUrl(
               "search",
               ("q", query),
               ("type", "artist"),
               ("limit", limit?.ToString()),
               ("offset", offset?.ToString()),
               ("market", market),
               ("include_external", includeExternal)
        );

        await foreach (var t in StreamPagesAsync(
            initialUrl: url,
            fetchPage: async (url, token) =>
            {
                using var req = new HttpRequestMessage(HttpMethod.Get, url);
                var env = await SendAsync<SearchArtistsEnvelope>(req, ct);
                var artists = env.Artists;
                
                _log.LogInformation("Artist search page: Total={Total}, Items={ItemCount}, Next={Next}", 
                    artists?.Total, artists?.Items?.Count ?? 0, artists?.Next);
                
                return new Page<Artist>
                {
                    Items = artists?.Items ?? new List<Artist>(),
                    Next = artists?.Next,
                    Total = artists?.Total
                };
            },
            1000,
            ct
            ))
        {
            yield return t;
        }
    }

    //// -------- Search ----------
    //private sealed record SearchTracksEnvelope(Paging<Track> Track);
    //private sealed record SearchArtistsEnvelope(Paging<Artist> Artist);
    //private sealed record SearchAlbumsEnvelope(Paging<Album> Album);

    //async IAsyncEnumerable<Track> ISearchApi.SearchTracksAsync(
    //    string query,
    //    int? limit,
    //    int? offset,
    //    string? market,
    //    string? includeExternal,
    //    [EnumeratorCancellation] CancellationToken ct)
    //{
    //    // Start with a relative URL; Spotify returns an absolute 'next'
    //    string? url = $"search?q={Uri.EscapeDataString(query)}&type=track&limit={limit}&offset={offset}";
    //    //string? url = BuildUrl(
    //    //        "search",
    //    //        ("q", query),
    //    //        ("type", "track"),
    //    //        ("limit", limit.ToString()),
    //    //        ("offset", offset.ToString()),
    //    //        ("market", market),
    //    //        ("include_external", includeExternal)
    //    //);

    //    await foreach (var t in StreamPagesAsync(
    //        initialUrl: url,
    //        fetchPage: async (url, token) =>
    //        {
    //            using var req = new HttpRequestMessage(HttpMethod.Get, url);
    //            var env = await SendAsync<SearchTracksEnvelope>(req, ct);
    //            var tracks = env.Track;
    //            return new Page<Track>
    //            {
    //                Items = tracks.Items ?? new List<Track>(),
    //                Next = tracks.Next,
    //                Total = tracks.Total
    //            };
    //        },
    //        1000,
    //        ct
    //        ))
    //    {
    //        yield return t;
    //    }
    //}

    //async IAsyncEnumerable<Artist> ISearchApi.SearchArtistsAsync(
    //   string query,
    //   int? limit,
    //   int? offset,
    //   string? market,
    //   string? includeExternal,
    //   [EnumeratorCancellation] CancellationToken ct)
    //{
    //    // Start with a relative URL; Spotify returns an absolute 'next'
    //    //string? url = $"search?q={Uri.EscapeDataString(query)}&type=track&limit={limit}&offset={offset}";
    //    string? url = BuildUrl(
    //            "search",
    //            ("q", query),
    //            ("type", "artist"),
    //            ("limit", limit.ToString()),
    //            ("offset", offset.ToString()),
    //            ("market", market),
    //            ("include_external", includeExternal)
    //    );

    //    await foreach (var t in StreamPagesAsync(
    //        initialUrl: url,
    //        fetchPage: async (url, token) =>
    //        {
    //            using var req = new HttpRequestMessage(HttpMethod.Get, url);
    //            var env = await SendAsync<SearchArtistsEnvelope>(req, ct);
    //            var artists = env.Artist;
    //            return new Page<Artist>
    //            {
    //                Items = artists.Items ?? new List<Artist>(),
    //                Next = artists.Next,
    //                Total = artists.Total
    //            };
    //        },
    //        1000,
    //        ct
    //        ))
    //    {
    //        yield return t;
    //    }
    //}

    //async IAsyncEnumerable<Album> ISearchApi.SearchAlbumsAsync(
    //   string query,
    //   int? limit,
    //   int? offset,
    //   string? market,
    //   string? includeExternal,
    //   [EnumeratorCancellation] CancellationToken ct)
    //{
    //    // Start with a relative URL; Spotify returns an absolute 'next'
    //    //string? url = $"search?q={Uri.EscapeDataString(query)}&type=track&limit={limit}&offset={offset}";
    //    string? url = BuildUrl(
    //            "search",
    //            ("q", query),
    //            ("type", "album"),
    //            ("limit", limit.ToString()),
    //            ("offset", offset.ToString()),
    //            ("market", market),
    //            ("include_external", includeExternal)
    //    );

    //    await foreach (var t in StreamPagesAsync(
    //        initialUrl: url,
    //        fetchPage: async (url, token) =>
    //        {
    //            using var req = new HttpRequestMessage(HttpMethod.Get, url);
    //            var env = await SendAsync<SearchAlbumsEnvelope>(req, ct);
    //            var albums = env.Album;
    //            return new Page<Album>
    //            {
    //                Items = albums.Items ?? new List<Album>(),
    //                Next = albums.Next,
    //                Total = albums.Total
    //            };
    //        },
    //        1000,
    //        ct
    //        ))
    //    {
    //        yield return t;
    //    }
    //}

    private static string BuildUrl(
        string path,
        params(string name, string? value)[] queryParameters)
    {
        var sb = new StringBuilder(path);
        var first = true;

        foreach(var (name, value) in queryParameters)
        {
            if (string.IsNullOrEmpty(value))
                continue;
            sb.Append(first ? '?' : '&');
            first = false;

            sb.Append(Uri.EscapeDataString(name));
            sb.Append('=');
            sb.Append(Uri.EscapeDataString(value));

        }
        return sb.ToString();
    }

    internal sealed class Page<T>
    {
        public required IReadOnlyList<T> Items { get; init; }
        public string? Next { get; init; }
        public int? Total { get; init; }
    }


    private async IAsyncEnumerable<T> StreamPagesAsync<T>(
        string? initialUrl, 
        Func<string, CancellationToken, Task<Page<T>>> fetchPage,
        int maxTotalCap, 
        [EnumeratorCancellation] CancellationToken ct)
    {
        var seenTracks = new HashSet<T>();
        //string? previous = null;
        string? url = initialUrl;
        int? firstToal = null;
        int stopAt = int.MaxValue;

        while (!string.IsNullOrEmpty(url))
        {
            var page = await fetchPage(url, ct);
            var items = page.Items;
            if (items is null || items.Count == 0)
                yield break;

            if (firstToal is null)
            {
                firstToal = page.Total ?? 0;
                stopAt = Math.Min(Math.Max((int)firstToal, 0), maxTotalCap);
            }

            int count = 0;
            foreach (var t in items)
            {
                ct.ThrowIfCancellationRequested();
                yield return t;
                seenTracks.Add(t);
                count++;
            }

            if (count == 0) yield break;
            if (seenTracks.Count >= stopAt) yield break;

            var next = string.IsNullOrEmpty(page.Next) ? null : page.Next;

            if (next is null)
                yield break;

            url = next;
        }
    }

}
