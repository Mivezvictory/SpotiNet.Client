using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace SpotiNet.Client.Models;

/// <summary>
/// Full response from Spotify's GET /me (Current User’s Profile).
/// </summary>
public sealed class CurrentUserProfile
{
    /// <summary>
    /// The country of the user as an ISO 3166-1 alpha-2 code.
    /// Present only when the app has the <c>user-read-private</c> scope.
    /// </summary>
    [JsonPropertyName("country")]
    public string? Country { get; set; }

    /// <summary>
    /// The name displayed on the user's profile. May be null.
    /// </summary>
    [JsonPropertyName("display_name")]
    public string? DisplayName { get; set; }

    /// <summary>
    /// The user's email address (unverified).
    /// Present only when the app has the <c>user-read-email</c> scope.
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    /// <summary>
    /// The user's explicit content settings.
    /// Present only when the app has the <c>user-read-private</c> scope.
    /// </summary>
    [JsonPropertyName("explicit_content")]
    public ExplicitContentSettings? ExplicitContent { get; set; }

    /// <summary>
    /// Known external URLs for this user (typically only the Spotify URL).
    /// </summary>
    [JsonPropertyName("external_urls")]
    public ExternalUrls? ExternalUrls { get; set; }

    /// <summary>
    /// Information about the user's followers.
    /// </summary>
    [JsonPropertyName("followers")]
    public Followers? Followers { get; set; }

    /// <summary>
    /// A link to the Web API endpoint for this user.
    /// </summary>
    [JsonPropertyName("href")]
    public string? Href { get; set; }

    /// <summary>
    /// The Spotify user ID for the current user.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// The user's profile images (usually multiple sizes).
    /// </summary>
    [JsonPropertyName("images")]
    public List<ImageObject>? Images { get; set; }

    /// <summary>
    /// The user's Spotify subscription level: "premium", "free", etc.
    /// Present only when the app has the <c>user-read-private</c> scope.
    /// </summary>
    [JsonPropertyName("product")]
    public string? Product { get; set; }

    /// <summary>
    /// The object type. For this response it is always "user".
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>
    /// The Spotify URI for the user (e.g., "spotify:user:...").
    /// </summary>
    [JsonPropertyName("uri")]
    public string? Uri { get; set; }

    /// <summary>
    /// Converts a full <see cref="CurrentUserProfile"/> to a simplified
    /// <see cref="CurrentUserProfileSimple"/> with just Id and DisplayName.
    /// CurrentUserProfileSimple can be null if CurrentUserProfile is null
    /// </summary>
    public CurrentUserProfileSimple ToSimple() => new()
    {
        Id = Id,
        DisplayName = DisplayName
    };

    /// <summary>
    /// Returns simplified UserProfile
    /// </summary>
    [System.Text.Json.Serialization.JsonIgnore]
    public CurrentUserProfileSimple Simple => ToSimple();
}

