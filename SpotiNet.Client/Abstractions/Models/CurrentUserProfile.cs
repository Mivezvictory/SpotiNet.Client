using System.Text.Json.Serialization;
namespace SpotiNet.Client;

/// <summary>Shape for /v1/me.</summary>
public sealed class CurrentUserProfile
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


