using System.Text.Json.Serialization;
using System.Collections.Generic;
namespace SpotiNet.Client.Models;

/// <summary>
/// A full Spotify Track object.
/// </summary>
public sealed class Track
{
    /// <summary>
    /// The album on which the track appears (simplified album object).
    /// </summary>
    [JsonPropertyName("album")]
    public Album? Album { get; set; }

    /// <summary>
    /// The artists who performed the track. Each artist is simplified.
    /// </summary>
    [JsonPropertyName("artists")]
    public List<ArtistSimple>? Artists { get; set; }

    /// <summary>
    /// Markets where the track is available, as ISO 3166-1 alpha-2 codes.
    /// </summary>
    [JsonPropertyName("available_markets")]
    public List<string>? AvailableMarkets { get; set; }

    /// <summary>
    /// The disc number (1-indexed) the track appears on.
    /// </summary>
    [JsonPropertyName("disc_number")]
    public int? DiscNumber { get; set; }

    /// <summary>
    /// The track length in milliseconds.
    /// </summary>
    [JsonPropertyName("duration_ms")]
    public int? DurationMs { get; set; }

    /// <summary>
    /// Whether the track has explicit lyrics (true) or not (false).
    /// </summary>
    [JsonPropertyName("explicit")]
    public bool? Explicit { get; set; }

    /// <summary>
    /// Known external IDs for the track (e.g., ISRC).
    /// </summary>
    [JsonPropertyName("external_ids")]
    public ExternalIds? ExternalIds { get; set; }

    /// <summary>
    /// Known external URLs for the track (e.g., Spotify Web URL).
    /// </summary>
    [JsonPropertyName("external_urls")]
    public ExternalUrls? ExternalUrls { get; set; }

    /// <summary>
    /// Link to the Web API endpoint for this track.
    /// </summary>
    [JsonPropertyName("href")]
    public string? Href { get; set; }

    /// <summary>
    /// The Spotify ID for the track.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Whether the track is playable in the current context/market.
    /// </summary>
    [JsonPropertyName("is_playable")]
    public bool? IsPlayable { get; set; }

    /// <summary>
    /// Link to the original track if this is a relinked version.
    /// </summary>
    [JsonPropertyName("linked_from")]
    public TrackLink? LinkedFrom { get; set; }

    /// <summary>
    /// Restrictions that may apply to the track (e.g., market).
    /// </summary>
    [JsonPropertyName("restrictions")]
    public Restrictions? Restrictions { get; set; }

    /// <summary>
    /// The track name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Track popularity (0–100), with 100 being most popular.
    /// </summary>
    [JsonPropertyName("popularity")]
    public int? Popularity { get; set; }

    /// <summary>
    /// A short (typically 30s) preview URL for the track, if available.
    /// </summary>
    [JsonPropertyName("preview_url")]
    public string? PreviewUrl { get; set; }

    /// <summary>
    /// The track’s number on its disc (1-indexed).
    /// </summary>
    [JsonPropertyName("track_number")]
    public int? TrackNumber { get; set; }

    /// <summary>
    /// The object type: always "track".
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>
    /// The Spotify URI for the track.
    /// </summary>
    [JsonPropertyName("uri")]
    public string? Uri { get; set; }

    /// <summary>
    /// Whether the track is a local file.
    /// </summary>
    [JsonPropertyName("is_local")]
    public bool? IsLocal { get; set; }

    /// <summary>
    /// Converts a full <see cref="Track"/> to a simplified
    /// <see cref="TrackSimple"/>
    /// TrackSimple can be null if Track is null
    /// </summary>
    public TrackSimple ToSimple() => new()
    {
        Album = Album,
        Artists = Artists,
        DurationMs = DurationMs,
        Id = Id,
        IsPlayable = IsPlayable,
        Name = Name,
        Uri = Uri
    };

    /// <summary>
    /// Returns simplified Track
    /// </summary>
    [System.Text.Json.Serialization.JsonIgnore]
    public TrackSimple Simple => ToSimple();
}