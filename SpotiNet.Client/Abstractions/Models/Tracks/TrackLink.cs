using System.Text.Json.Serialization;

namespace SpotiNet.Client.Models;
/// <summary>
/// A link to another track (used for relinking).
/// </summary>
public sealed class TrackLink
{
    /// <summary>
    /// Known external URLs for the track (e.g., Spotify Web URL).
    /// </summary>
    [JsonPropertyName("external_urls")]
    public ExternalUrls? ExternalUrls { get; set; }

    /// <summary>
    /// Link to the Web API endpoint for this linked track.
    /// </summary>
    [JsonPropertyName("href")]
    public string? Href { get; set; }

    /// <summary>
    /// The Spotify ID for this linked track.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// The object type: always "track".
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>
    /// The Spotify URI for this linked track.
    /// </summary>
    [JsonPropertyName("uri")]
    public string? Uri { get; set; }
}





