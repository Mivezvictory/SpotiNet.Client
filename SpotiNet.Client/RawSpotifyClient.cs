using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace SpotiNet.Client;

/// <summary>
/// Minimal raw client used in Phase 1 to validate dependency injection and authentication plumbing.
/// Later phases introduce typed APIs (e.g., Users, Playlists) that call into this client or replace it.
/// </summary>
public sealed class RawSpotifyClient
{
    private readonly HttpClient _http;
    private readonly JsonSerializerOptions _json;

    /// <summary>
    /// Creates a new <see cref="RawSpotifyClient"/>.
    /// </summary>
    /// <param name="http">The configured <see cref="HttpClient"/> with base address and handlers.</param>
    /// <param name="options">Options that provide default JSON behavior.</param>
    public RawSpotifyClient(HttpClient http, IOptions<SpotifyClientOptions> options)
    {
        _http = http;
        _json = options.Value.Json;
    }

    /// <summary>
    /// Issues a GET request to the Spotify API using a relative path and deserializes the JSON response to <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Target type for deserialization.</typeparam>
    /// <param name="relativeUrl">Relative path under the Spotify base URL (e.g., <c>artists/{id}</c>).</param>
    /// <param name="ct">A cancellation token.</param>
    /// <returns>The deserialized response.</returns>
    /// <exception cref="HttpRequestException">Thrown when the response is not successful (non 2xx).</exception>
    /// <exception cref="InvalidOperationException">Thrown when the response cannot be deserialized to <typeparamref name="T"/>.</exception>
    public async Task<T> GetAsync<T>(string relativeUrl, CancellationToken ct = default)
    {
        using var req = new HttpRequestMessage(HttpMethod.Get, relativeUrl);
        using var res = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);
        var body = await res.Content.ReadAsStringAsync(ct);

        if (!res.IsSuccessStatusCode)
            throw new HttpRequestException(
                $"Spotify GET {relativeUrl} failed: {(int)res.StatusCode} {res.ReasonPhrase}{Environment.NewLine}{body}");

        var data = JsonSerializer.Deserialize<T>(body, _json);
        if (data is null)
            throw new InvalidOperationException("Failed to deserialize Spotify response.");
        return data;
    }
}
