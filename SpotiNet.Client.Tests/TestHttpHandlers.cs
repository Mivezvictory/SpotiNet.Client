using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using SpotiNet.Client;

/// <summary>
/// Fake HTTP handler shared by SpotifyClient tests: answers via a caller-supplied responder
/// and records every outgoing request, so tests can assert on method/URL/body without a mocking library.
/// </summary>
internal sealed class RecordingHandler : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, HttpResponseMessage> _responder;
    private readonly List<HttpRequestMessage> _requests = new();
    private readonly List<string?> _requestBodies = new();

    public RecordingHandler(Func<HttpRequestMessage, HttpResponseMessage> responder) => _responder = responder;

    public RecordingHandler(HttpResponseMessage fixedResponse) : this(_ => fixedResponse) { }

    public IReadOnlyList<HttpRequestMessage> Requests => _requests;

    /// <summary>
    /// Request bodies read eagerly at send time. Production code disposes each HttpRequestMessage
    /// (and its Content) right after its SendAsync call returns, so reading Content afterward throws.
    /// </summary>
    public IReadOnlyList<string?> RequestBodies => _requestBodies;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _requests.Add(request);
        _requestBodies.Add(request.Content is null ? null : await request.Content.ReadAsStringAsync(cancellationToken));
        return _responder(request);
    }
}

internal static class TestResponses
{
    public static HttpResponseMessage Json(HttpStatusCode status, string json) => new(status)
    {
        Content = new StringContent(json, Encoding.UTF8, "application/json")
    };
}

internal static class TestSpotifyClientFactory
{
    public static SpotifyClient Create(HttpMessageHandler handler)
    {
        var http = new HttpClient(handler) { BaseAddress = new Uri("https://api.spotify.com/v1/") };
        return new SpotifyClient(http, Options.Create(new SpotifyClientOptions()), NullLogger<SpotifyClient>.Instance);
    }
}

internal static class TestUris
{
    /// <summary>Pulls an integer query parameter out of a request URI without needing a full query-string parser.</summary>
    public static int QueryInt(Uri uri, string name, int fallback)
    {
        foreach (var pair in uri.Query.TrimStart('?').Split('&', StringSplitOptions.RemoveEmptyEntries))
        {
            var parts = pair.Split('=', 2);
            if (parts.Length == 2 && parts[0] == name && int.TryParse(Uri.UnescapeDataString(parts[1]), out var value))
                return value;
        }
        return fallback;
    }
}
