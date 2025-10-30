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
