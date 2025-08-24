using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace SpotiNet.Client;

public static class SpotifyClientHandler
{
    private static readonly HttpClient _http = new HttpClient();
    private static readonly JsonSerializerOptions _json = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    /// <summary>Query Spotify with url and token.</summary>
    public static async Task<T> GetSpotifyResponseJson<T>(string url, string token, string method, ILogger logger)
    {
        using var req = new HttpRequestMessage(GetHttpMethod(method), url);
        req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
            "Bearer",
            token
        );

        using var res = await _http.SendAsync(req);
        var body = await res.Content.ReadAsStringAsync();

        if (!res.IsSuccessStatusCode)
        {
            logger.LogError("Spotify GET {Url} failed: {Status} {Body}", url, res.StatusCode, body);
            throw new InvalidOperationException("Spotify API request failed.");
        }
        return JsonSerializer.Deserialize<T>(body, _json);

    }

    private static HttpMethod GetHttpMethod(string method)
    {
        return method.ToLower() switch
        {
            "get" => HttpMethod.Get,
            "post" => HttpMethod.Post,
        };
    }
}
