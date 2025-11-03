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
    private sealed record SearchTracksEnvelope(Paging<Track> Tracks);

    
    async IAsyncEnumerable<Track> ISearchApi.SearchTracksAsync(
        string query,
        int? limit,
        [EnumeratorCancellation] CancellationToken ct)
    {
        // Start with a relative URL; Spotify returns an absolute 'next'
        string? url = $"search?q={Uri.EscapeDataString(query)}&type=track&limit={limit}";
        var seen = new HashSet<string>(StringComparer.Ordinal);
        //string? previous = null;


        while (!string.IsNullOrEmpty(url))
        {
            using var req = new HttpRequestMessage(HttpMethod.Get, url);
            var env = await SendAsync<SearchTracksEnvelope>(req, ct);
            var tracks = env.Tracks;
            if (tracks?.Items is null || tracks.Items.Count == 0)
                yield break;

            foreach (var t in tracks.Items)
            {
                ct.ThrowIfCancellationRequested();
                yield return t;
            }

            var next = string.IsNullOrEmpty(tracks.Next) ? null : tracks.Next;

            if (next is null || !seen.Add(next))
                yield break;

            url = next;
        }

    }
    
    private static string Rel(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) return string.Empty;

        // If it's already an absolute URI, leave it alone (BaseAddress will be ignored).
        if (Uri.TryCreate(path, UriKind.Absolute, out _)) return path;

        // Otherwise ensure it's a clean relative segment (no leading slash).
        return path.TrimStart('/');
    }
    //private async 

}
