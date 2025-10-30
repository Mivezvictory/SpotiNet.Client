using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SpotiNet.Client.Models;

/// <summary>
/// A simplified Show object as embedded on an Episode.
/// </summary>
public sealed class ShowSimple
{
    /// <summary>
    /// The Spotify ID for the show.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Languages used in the show, ordered by preference.
    /// </summary>
    [JsonPropertyName("languages")]
    public List<string>? Languages { get; set; }

    /// <summary>
    /// The show title.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// The Spotify URI for the show.
    /// </summary>
    [JsonPropertyName("uri")]
    public string? Uri { get; set; }
}