using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SpotiNet.Client.Models;

/// <summary>
/// A full Spotify Album object (returned by GET /albums/{id}).
/// </summary>
public sealed class Album
{
    /// <summary>The type of the album: "album", "single", or "compilation".</summary>
    [JsonPropertyName("album_type")]
    public string? AlbumType { get; set; }

    /// <summary>Total number of tracks on the album.</summary>
    [JsonPropertyName("total_tracks")]
    public int? TotalTracks { get; set; }

    /// <summary>ISO 3166-1 alpha-2 market codes where the album is available.</summary>
    [JsonPropertyName("available_markets")]
    public List<string>? AvailableMarkets { get; set; }

    /// <summary>Known external URLs for the album (e.g., web URL).</summary>
    [JsonPropertyName("external_urls")]
    public ExternalUrls? ExternalUrls { get; set; }

    /// <summary>API endpoint URL for this album.</summary>
    [JsonPropertyName("href")]
    public string? Href { get; set; }

    /// <summary>The Spotify ID for the album.</summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>Cover art images in various sizes, widest first.</summary>
    [JsonPropertyName("images")]
    public List<ImageObject>? Images { get; set; }

    /// <summary>The album name (may be empty on takedown).</summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>The date the album was first released.</summary>
    [JsonPropertyName("release_date")]
    public string? ReleaseDate { get; set; }

    /// <summary>Precision for <see cref="ReleaseDate"/>: "year" | "month" | "day".</summary>
    [JsonPropertyName("release_date_precision")]
    public string? ReleaseDatePrecision { get; set; }

    /// <summary>Content restrictions (e.g., "market", "product", "explicit").</summary>
    [JsonPropertyName("restrictions")]
    public Restrictions? Restrictions { get; set; }

    /// <summary>Object type: always "album".</summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>The Spotify URI for the album.</summary>
    [JsonPropertyName("uri")]
    public string? Uri { get; set; }

    /// <summary>The artists of the album (simplified artist objects).</summary>
    [JsonPropertyName("artists")]
    public List<ArtistSimple>? Artists { get; set; }

    /// <summary>Album tracks as an offset-based paging container of simplified tracks.</summary>
    [JsonPropertyName("tracks")]
    public Paging<TrackSimple>? Tracks { get; set; }

    /// <summary>Copyright statements for the album.</summary>
    [JsonPropertyName("copyrights")]
    public List<CopyrightObject>? Copyrights { get; set; }

    /// <summary>Known external IDs for the album (e.g., UPC/EAN/ISRC).</summary>
    [JsonPropertyName("external_ids")]
    public ExternalIds? ExternalIds { get; set; }

    /// <summary>Genres (deprecated on albums and typically empty).</summary>
    [JsonPropertyName("genres")]
    public List<string>? Genres { get; set; }

    /// <summary>The label associated with the album.</summary>
    [JsonPropertyName("label")]
    public string? Label { get; set; }

    /// <summary>Album popularity (0–100), 100 = most popular.</summary>
    [JsonPropertyName("popularity")]
    public int? Popularity { get; set; }

    /// <summary>
    /// Converts a full <see cref="Album"/> to a simplified
    /// <see cref="AlbumSimple"/>
    /// AlbumSimple can be null if Album is null
    /// </summary>
    public AlbumSimple ToSimple() => new()
    {
        Artists = Artists,
        Id = Id,
        Name = Name,
        ReleaseDate = ReleaseDate,
        ReleaseDatePrecision = ReleaseDatePrecision,
        TotalTracks = TotalTracks,
        Uri = Uri
    };

    /// <summary>
    /// Returns simplified Album object
    /// </summary>
    [System.Text.Json.Serialization.JsonIgnore]
    public AlbumSimple Simple => ToSimple();
}