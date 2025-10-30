using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace SpotiNet.Client;

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

    [System.Text.Json.Serialization.JsonIgnore]
    public CurrentUserProfileSimple Simple => ToSimple();
}

/// <summary>
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
/// Represents a simplified profile of the current user on Spotify.
/// </summary>
/// <remarks>This class provides basic information about the current user, including their Spotify ID and display
/// name. It is typically used to retrieve or display user-specific data in applications interacting with the Spotify
/// API.</remarks>
public sealed class CurrentUserProfileSimple
{
    /// <summary>
    /// The Spotify ID of current user
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// The display name of current user
    /// </summary>
    [JsonPropertyName("display_name")]
    public string? DisplayName { get; set; }

}


