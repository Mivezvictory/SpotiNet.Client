using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SpotiNet.Client.Models;

/// <summary>
/// A full Spotify Artist object.
/// </summary>
public sealed class Artist
{
    /// <summary>
    /// Known external URLs for the artist (e.g., Spotify Web URL).
    /// </summary>
    [JsonPropertyName("external_urls")]
    public ExternalUrls? ExternalUrls { get; set; }

    /// <summary>
    /// Information about the artist’s followers (total count, etc.).
    /// </summary>
    [JsonPropertyName("followers")]
    public Followers? Followers { get; set; }

    /// <summary>
    /// Genres associated with the artist. Not necessarily comprehensive.
    /// </summary>
    [JsonPropertyName("genres")]
    public List<string>? Genres { get; set; }

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
    /// Images of the artist in various sizes, widest first.
    /// </summary>
    [JsonPropertyName("images")]
    public List<ImageObject>? Images { get; set; }

    /// <summary>
    /// The artist’s name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Artist popularity (0–100), with 100 being most popular.
    /// </summary>
    [JsonPropertyName("popularity")]
    public int? Popularity { get; set; }

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

    /// <summary>
    /// Converts a full <see cref="Artist"/> to a simplified
    /// <see cref="ArtistSimple"/> 
    /// ArtistSimple can be null if Artist is null
    /// </summary>
    public ArtistSimple ToSimple() => new()
    {
        ExternalUrls = ExternalUrls,
        Href = Href,
        Id = Id,
        Name = Name,
        Type = Type,
        Uri = Uri
    };

    /// <summary>
    /// Returns simplified Artist
    /// </summary>
    [System.Text.Json.Serialization.JsonIgnore]
    public ArtistSimple Simple => ToSimple();
}
