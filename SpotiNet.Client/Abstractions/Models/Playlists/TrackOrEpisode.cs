using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SpotiNet.Client.Models;

/// <summary>
/// A discriminated union that can hold either a Track or an Episode from a playlist.
/// Check the <see cref="Type"/> property to determine which type of content this represents.
/// </summary>
public sealed class TrackOrEpisode
{
    /// <summary>
    /// The object type: "track" or "episode".
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>
    /// The Spotify ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// The Spotify URI.
    /// </summary>
    [JsonPropertyName("uri")]
    public string? Uri { get; set; }

    /// <summary>
    /// The name (track name or episode title).
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Duration in milliseconds.
    /// </summary>
    [JsonPropertyName("duration_ms")]
    public int? DurationMs { get; set; }

    /// <summary>
    /// Whether the content is explicit.
    /// </summary>
    [JsonPropertyName("explicit")]
    public bool? Explicit { get; set; }

    /// <summary>
    /// Known external URLs.
    /// </summary>
    [JsonPropertyName("external_urls")]
    public ExternalUrls? ExternalUrls { get; set; }

    /// <summary>
    /// API endpoint URL for this object.
    /// </summary>
    [JsonPropertyName("href")]
    public string? Href { get; set; }

    // Track-specific properties
    /// <summary>
    /// (Track only) The album this track appears on.
    /// </summary>
    [JsonPropertyName("album")]
    public AlbumSimple? Album { get; set; }

    /// <summary>
    /// (Track only) The artists who performed the track.
    /// </summary>
    [JsonPropertyName("artists")]
    public List<ArtistSimple>? Artists { get; set; }

    /// <summary>
    /// (Track only) Disc number (usually 1 unless multi-disc album).
    /// </summary>
    [JsonPropertyName("disc_number")]
    public int? DiscNumber { get; set; }

    /// <summary>
    /// (Track only) Track number on the album.
    /// </summary>
    [JsonPropertyName("track_number")]
    public int? TrackNumber { get; set; }

    /// <summary>
    /// (Track only) Popularity score (0-100).
    /// </summary>
    [JsonPropertyName("popularity")]
    public int? Popularity { get; set; }

    /// <summary>
    /// (Track only) Preview URL for 30-second clip.
    /// </summary>
    [JsonPropertyName("preview_url")]
    public string? PreviewUrl { get; set; }

    /// <summary>
    /// (Track only) External IDs (ISRC, EAN, UPC).
    /// </summary>
    [JsonPropertyName("external_ids")]
    public ExternalIds? ExternalIds { get; set; }

    /// <summary>
    /// (Track only) Whether this is a local track.
    /// </summary>
    [JsonPropertyName("is_local")]
    public bool? IsLocal { get; set; }

    // Episode-specific properties
    /// <summary>
    /// (Episode only) Preview MP3 URL.
    /// </summary>
    [JsonPropertyName("audio_preview_url")]
    public string? AudioPreviewUrl { get; set; }

    /// <summary>
    /// (Episode only) Episode description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// (Episode only) HTML-formatted description.
    /// </summary>
    [JsonPropertyName("html_description")]
    public string? HtmlDescription { get; set; }

    /// <summary>
    /// (Episode only) Episode artwork images.
    /// </summary>
    [JsonPropertyName("images")]
    public List<ImageObject>? Images { get; set; }

    /// <summary>
    /// (Episode only) Whether hosted externally.
    /// </summary>
    [JsonPropertyName("is_externally_hosted")]
    public bool? IsExternallyHosted { get; set; }

    /// <summary>
    /// (Episode only) Whether playable in current market.
    /// </summary>
    [JsonPropertyName("is_playable")]
    public bool? IsPlayable { get; set; }

    /// <summary>
    /// (Episode only) Spoken languages.
    /// </summary>
    [JsonPropertyName("languages")]
    public List<string>? Languages { get; set; }

    /// <summary>
    /// (Episode only) Release date.
    /// </summary>
    [JsonPropertyName("release_date")]
    public string? ReleaseDate { get; set; }

    /// <summary>
    /// (Episode only) Release date precision.
    /// </summary>
    [JsonPropertyName("release_date_precision")]
    public string? ReleaseDatePrecision { get; set; }

    /// <summary>
    /// (Episode only) User's resume position.
    /// </summary>
    [JsonPropertyName("resume_point")]
    public ResumePoint? ResumePoint { get; set; }

    /// <summary>
    /// (Episode only) The show this episode belongs to.
    /// </summary>
    [JsonPropertyName("show")]
    public ShowSimple? Show { get; set; }

    /// <summary>
    /// Returns true if this is a track (type == "track").
    /// </summary>
    [JsonIgnore]
    public bool IsTrack => Type?.Equals("track", StringComparison.OrdinalIgnoreCase) == true;

    /// <summary>
    /// Returns true if this is an episode (type == "episode").
    /// </summary>
    [JsonIgnore]
    public bool IsEpisode => Type?.Equals("episode", StringComparison.OrdinalIgnoreCase) == true;
}
