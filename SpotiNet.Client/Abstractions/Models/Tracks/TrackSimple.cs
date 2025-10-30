using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace SpotiNet.Client.Models;

/// <summary>
/// A simplified Track object (commonly used in lists/playlists).
/// </summary>
public sealed class TrackSimple
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
    /// The track length in milliseconds.
    /// </summary>
    [JsonPropertyName("duration_ms")]
    public int? DurationMs { get; set; }

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
    /// The track name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// The Spotify URI for the track.
    /// </summary>
    [JsonPropertyName("uri")]
    public string? Uri { get; set; }

   
}