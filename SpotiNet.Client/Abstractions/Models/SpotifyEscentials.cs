using System.Text.Json.Serialization;

namespace SpotiNet.Client.Models;

// <summary>
/// The user's explicit content settings.
/// </summary>
public sealed class ExplicitContentSettings
{
    /// <summary>
    /// When true, explicit content should not be played.
    /// </summary>
    [JsonPropertyName("filter_enabled")]
    public bool? FilterEnabled { get; set; }

    /// <summary>
    /// When true, the explicit content setting is locked and cannot be changed by the user.
    /// </summary>
    [JsonPropertyName("filter_locked")]
    public bool? FilterLocked { get; set; }
}

/// <summary>
/// Known external URLs for the user.
/// </summary>
public sealed class ExternalUrls
{
    /// <summary>
    /// The Spotify URL for the user.
    /// </summary>
    [JsonPropertyName("spotify")]
    public string? Spotify { get; set; }
}

/// <summary>
/// Known external IDs for a track (e.g., ISRC).
/// </summary>
public sealed class ExternalIds
{
    /// <summary>
    /// International Standard Recording Code.
    /// </summary>
    [JsonPropertyName("isrc")]
    public string? Isrc { get; set; }

    /// <summary>
    /// International Article Number (EAN-13).
    /// </summary>
    [JsonPropertyName("ean")]
    public string? Ean { get; set; }

    /// <summary>
    /// Universal Product Code.
    /// </summary>
    [JsonPropertyName("upc")]
    public string? Upc { get; set; }
}

/// <summary>
/// Follower information for a user.
/// </summary>
public sealed class Followers
{
    /// <summary>
    /// A link to the Web API endpoint returning the full result for the followers.
    /// Spotify currently always returns null for this field.
    /// </summary>
    [JsonPropertyName("href")]
    public string? Href { get; set; }

    /// <summary>
    /// The total number of followers.
    /// </summary>
    [JsonPropertyName("total")]
    public int? Total { get; set; }
}

/// <summary>
/// An image object (URL and dimensions).
/// </summary>
public sealed class ImageObject
{
    /// <summary>
    /// The source URL of the image.
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    /// <summary>
    /// The image height in pixels. May be null.
    /// </summary>
    [JsonPropertyName("height")]
    public int? Height { get; set; }

    /// <summary>
    /// The image width in pixels. May be null.
    /// </summary>
    [JsonPropertyName("width")]
    public int? Width { get; set; }
}

/// <summary>
/// Restrictions for content availability.
/// </summary>
public sealed class Restrictions
{
    /// <summary>
    /// Reason for the restriction (e.g., "market").
    /// </summary>
    [JsonPropertyName("reason")]
    public string? Reason { get; set; }
}

/// <summary>Copyright statement.</summary>
public sealed class CopyrightObject
{
    /// <summary>Copyright text.</summary>
    [JsonPropertyName("text")] public string? Text { get; set; }

    /// <summary>Type: "C" (copyright) or "P" (sound recording).</summary>
    [JsonPropertyName("type")] public string? Type { get; set; }
}