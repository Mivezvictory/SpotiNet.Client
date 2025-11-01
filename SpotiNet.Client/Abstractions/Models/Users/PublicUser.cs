using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace SpotiNet.Client.Models;

/// <summary>
/// A public user object (subset of a Spotify user profile).
/// </summary>
public sealed class PublicUser
{
    /// <summary>Display name of the user (may be null).</summary>
    [JsonPropertyName("display_name")]
    public string? DisplayName { get; set; }

    /// <summary>Known external URLs for this user.</summary>
    [JsonPropertyName("external_urls")]
    public ExternalUrls? ExternalUrls { get; set; }

    /// <summary>
    /// Information about the user's followers.
    /// </summary>
    [JsonPropertyName("followers")]
    public Followers? Followers { get; set; }

    /// <summary>API endpoint URL for this user.</summary>
    [JsonPropertyName("href")]
    public string? Href { get; set; }

    /// <summary>The Spotify ID for the user.</summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// The user's profile images (usually multiple sizes).
    /// </summary>
    [JsonPropertyName("images")]
    public List<ImageObject>? Images { get; set; }


    /// <summary>Object type: always "user".</summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>The Spotify URI for the user.</summary>
    [JsonPropertyName("uri")]
    public string? Uri { get; set; }
}