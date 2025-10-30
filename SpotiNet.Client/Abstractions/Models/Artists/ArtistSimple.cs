using System.Text.Json.Serialization;
namespace SpotiNet.Client.Models;

/// <summary>
/// A simplified Artist object (used in nested contexts like tracks/albums).
/// </summary>
public sealed class ArtistSimple
{
    /// <summary>
    /// Known external URLs for the artist (e.g., Spotify Web URL).
    /// </summary>
    [JsonPropertyName("external_urls")]
    public ExternalUrls? ExternalUrls { get; set; }

    /// <summary>
    /// Link to the Web API endpoint for this artist.
    /// </summary>
    [JsonPropertyName("href")]
    public string? Href { get; set; }

    /// <summary>
    /// The Spotify ID for the artist.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// The artist’s name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// The object type: always "artist".
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>
    /// The Spotify URI for the artist.
    /// </summary>
    [JsonPropertyName("uri")]
    public string? Uri { get; set; }
}