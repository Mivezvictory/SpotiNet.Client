using System.Text.Json.Serialization;
namespace SpotiNet.Client;

/// <summary>Shape for playlist objects.</summary>
public sealed class PlaylistSimple
{
    /// <summary>
    /// The ID of created playlist
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    /// <summary>
    /// The name of the playlist
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
