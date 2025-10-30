using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SpotiNet.Client.Models;
/// <summary>
/// A full Spotify Show object (podcast series).
/// </summary>
public sealed class Show
{
    /// <summary>
    /// Markets where the show is available (ISO 3166-1 alpha-2 codes).
    /// </summary>
    [JsonPropertyName("available_markets")]
    public List<string>? AvailableMarkets { get; set; }

    /// <summary>
    /// Copyright statements associated with the show.
    /// </summary>
    [JsonPropertyName("copyrights")]
    public List<CopyrightObject>? Copyrights { get; set; }

    /// <summary>
    /// Plain-text description of the show.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// HTML-formatted description of the show.
    /// </summary>
    [JsonPropertyName("html_description")]
    public string? HtmlDescription { get; set; }

    /// <summary>
    /// Offset-based paging of the show’s episodes (simplified episode objects).
    /// </summary>
    [JsonPropertyName("episodes")]
    public Paging<Episode>? Episodes { get; set; }

    /// <summary>
    /// True if the show contains explicit content.
    /// </summary>
    [JsonPropertyName("explicit")]
    public bool? Explicit { get; set; }

    /// <summary>
    /// Known external URLs for this show (e.g., Spotify web URL).
    /// </summary>
    [JsonPropertyName("external_urls")]
    public ExternalUrls? ExternalUrls { get; set; }

    /// <summary>
    /// API endpoint URL for this show object.
    /// </summary>
    [JsonPropertyName("href")]
    public string? Href { get; set; }

    /// <summary>
    /// The Spotify ID for the show.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Show artwork images in various sizes, widest first.
    /// </summary>
    [JsonPropertyName("images")]
    public List<ImageObject>? Images { get; set; }

    /// <summary>
    /// True if the show’s audio is hosted outside Spotify.
    /// </summary>
    [JsonPropertyName("is_externally_hosted")]
    public bool? IsExternallyHosted { get; set; }

    /// <summary>
    /// Spoken languages for the show, ordered by preference.
    /// </summary>
    [JsonPropertyName("languages")]
    public List<string>? Languages { get; set; }

    /// <summary>
    /// The media type of the show’s content (e.g., "audio").
    /// </summary>
    [JsonPropertyName("media_type")]
    public string? MediaType { get; set; }

    /// <summary>
    /// The show title.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Name of the show’s publisher.
    /// </summary>
    [JsonPropertyName("publisher")]
    public string? Publisher { get; set; }

    /// <summary>
    /// Object type: always "show".
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>
    /// The Spotify URI for the show.
    /// </summary>
    [JsonPropertyName("uri")]
    public string? Uri { get; set; }

    /// <summary>
    /// Total number of episodes available for this show.
    /// </summary>
    [JsonPropertyName("total_episodes")]
    public int? TotalEpisodes { get; set; }

    /// <summary>
    /// Converts a full <see cref="Show"/> to a simplified
    /// <see cref="ShowSimple"/> 
    /// ShowSimple can be null if Show is null
    /// </summary>
    public ShowSimple ToSimple() => new()
    {
        Id = Id,
        Name = Name,
        Languages = Languages,
        Uri = Uri
    };

    /// <summary>
    /// Returns simplified Show object
    /// </summary>
    [System.Text.Json.Serialization.JsonIgnore]
    public ShowSimple Simple => ToSimple();
}