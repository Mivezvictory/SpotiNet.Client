using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SpotiNet.Client.Models;

/// <summary>
/// A full Spotify Episode object (used in shows and playlists).
/// </summary>
public sealed class Episode
{
    /// <summary>
    /// A URL to a preview MP3 for the episode, if available.
    /// </summary>
    [JsonPropertyName("audio_preview_url")]
    public string? AudioPreviewUrl { get; set; }

    /// <summary>
    /// Markets where the episode is available (ISO 3166-1 alpha-2).
    /// </summary>
    [JsonPropertyName("available_markets")]
    public List<string>? AvailableMarkets { get; set; }

    /// <summary>
    /// Episode description in plain text (may be truncated).
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Episode description with HTML formatting.
    /// </summary>
    [JsonPropertyName("html_description")]
    public string? HtmlDescription { get; set; }

    /// <summary>
    /// Episode length in milliseconds.
    /// </summary>
    [JsonPropertyName("duration_ms")]
    public int? DurationMs { get; set; }

    /// <summary>
    /// True if the episode contains explicit content.
    /// </summary>
    [JsonPropertyName("explicit")]
    public bool? Explicit { get; set; }

    /// <summary>
    /// Known external URLs for this episode (e.g., Spotify web URL).
    /// </summary>
    [JsonPropertyName("external_urls")]
    public ExternalUrls? ExternalUrls { get; set; }

    /// <summary>
    /// API endpoint URL for this episode object.
    /// </summary>
    [JsonPropertyName("href")]
    public string? Href { get; set; }

    /// <summary>
    /// The Spotify ID for the episode.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Episode artwork images in various sizes, widest first.
    /// </summary>
    [JsonPropertyName("images")]
    public List<ImageObject>? Images { get; set; }

    /// <summary>
    /// True if the episode’s audio is hosted outside Spotify.
    /// </summary>
    [JsonPropertyName("is_externally_hosted")]
    public bool? IsExternallyHosted { get; set; }

    /// <summary>
    /// True if the episode is playable in the current market and context.
    /// </summary>
    [JsonPropertyName("is_playable")]
    public bool? IsPlayable { get; set; }

    /// <summary>
    /// Deprecated single language code (use <see cref="Languages"/>).
    /// </summary>
    [JsonPropertyName("language")]
    public string? Language { get; set; }

    /// <summary>
    /// Spoken languages for the episode, ordered by preference.
    /// </summary>
    [JsonPropertyName("languages")]
    public List<string>? Languages { get; set; }

    /// <summary>
    /// The episode title.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Date the episode was first released (format depends on precision).
    /// </summary>
    [JsonPropertyName("release_date")]
    public string? ReleaseDate { get; set; }

    /// <summary>
    /// Precision of <see cref="ReleaseDate"/>: "year" | "month" | "day".
    /// </summary>
    [JsonPropertyName("release_date_precision")]
    public string? ReleaseDatePrecision { get; set; }

    /// <summary>
    /// Information about the user’s current playback position on this episode.
    /// </summary>
    [JsonPropertyName("resume_point")]
    public ResumePoint? ResumePoint { get; set; }

    /// <summary>
    /// Content restrictions (e.g., market, product).
    /// </summary>
    [JsonPropertyName("restrictions")]
    public Restrictions? Restrictions { get; set; }

    /// <summary>
    /// Simplified show that this episode belongs to.
    /// </summary>
    [JsonPropertyName("show")]
    public Show? Show { get; set; }

    /// <summary>
    /// Object type: always "episode".
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>
    /// The Spotify URI for the episode.
    /// </summary>
    [JsonPropertyName("uri")]
    public string? Uri { get; set; }
}

/// <summary>
/// The user’s resume position for an episode.
/// </summary>
public sealed class ResumePoint
{
    /// <summary>
    /// True if the episode has been fully played by the user.
    /// </summary>
    [JsonPropertyName("fully_played")]
    public bool? FullyPlayed { get; set; }

    /// <summary>
    /// The user’s last playback position in milliseconds.
    /// </summary>
    [JsonPropertyName("resume_position_ms")]
    public int? ResumePositionMs { get; set; }
}



