using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SpotiNet.Client;

/// <summary>
/// Minimal raw client used in Phase 1 to validate dependency injection and authentication plumbing.
/// Phase 2 adds structured errors and logging. Typed APIs arrive in later phases.
/// </summary>
public sealed class RawSpotifyClient
{
    private readonly HttpClient _http;
    private readonly JsonSerializerOptions _json;
    private readonly ILogger<RawSpotifyClient> _log;

    /// <summary>
    /// Creates a new <see cref="RawSpotifyClient"/>.
    /// </summary>
    /// <param name="http">Configured <see cref="HttpClient"/> with base address and handlers.</param>
    /// <param name="options">Options that provide default JSON behavior.</param>
    /// <param name="log">Logger used to record failures and diagnostics.</param>
    public RawSpotifyClient(HttpClient http, IOptions<SpotifyClientOptions> options, ILogger<RawSpotifyClient> log)
    {
        _http = http;
        _json = options.Value.Json;
        _log = log;
    }

    /// <summary>
    /// Issues a GET request and deserializes the JSON response to <typeparamref name="T"/>.
    /// Throws <see cref="SpotifyApiException"/> on non-success responses with a helpful message.
    /// </summary>
    public async Task<T> GetAsync<T>(string relativeUrl, CancellationToken ct = default)
    {
        using var req = new HttpRequestMessage(HttpMethod.Get, relativeUrl);
        using var res = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);
        var body = await res.Content.ReadAsStringAsync(ct);

        if (!res.IsSuccessStatusCode)
        {
            // Prefer Spotify's error.message; otherwise fall back to HTTP status text
            var msg = SpotifyErrorParser.TryReadMessage(body)
                      ?? $"HTTP {(int)res.StatusCode} {res.ReasonPhrase}";
            _log.LogError("Spotify {Method} {Url} failed: {StatusCode} {Message}",
                req.Method, req.RequestUri, (int)res.StatusCode, msg);

            throw new SpotifyApiException(res.StatusCode, msg, body);
        }

        var data = JsonSerializer.Deserialize<T>(body, _json);
        if (data is null)
            throw new SpotifyApiException(HttpStatusCode.InternalServerError,
                "Failed to deserialize Spotify response.", body);

        return data;
    }
}
