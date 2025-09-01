using System.Text.Json;

namespace SpotiNet.Client;

/// <summary>
/// Options used to configure the Spotify HTTP client.
/// </summary>
public sealed class SpotifyClientOptions
{
    /// <summary>
    /// Base address for the Spotify Web API.
    /// Defaults to <c>https://api.spotify.com/v1/</c>.
    /// </summary>
    public string BaseUrl { get; set; } = "https://api.spotify.com/v1/";

    /// <summary>
    /// Default JSON serialization options used when (de)serializing responses.
    /// The default enables case-insensitive property matching.
    /// </summary>
    public JsonSerializerOptions Json { get; set; } = new()
    {
        PropertyNameCaseInsensitive = true
    };
}
