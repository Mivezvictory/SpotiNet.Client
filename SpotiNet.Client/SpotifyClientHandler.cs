using System;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace SpotiNet.Client;

/// <summary>
/// 
/// </summary>
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
        var result = JsonSerializer.Deserialize<T>(body, _json);
        if (result == null)
        {
            logger.LogError("Deserialization failed for Spotify response: {Body}", body);
            throw new InvalidOperationException("Failed to deserialize Spotify API response.");
        }
        return result;

    }

    private static HttpMethod GetHttpMethod(string method)
    {
        return method.ToLower() switch
        {
            "get" => HttpMethod.Get,
            "post" => HttpMethod.Post,
            _ => throw new ArgumentException($"Unsupported HTTP method: {method}")
        };
    }
}
