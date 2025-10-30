using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SpotiNet.Client.Models;

/// <summary>
/// A simplified Album object 
/// </summary>
public sealed class AlbumSimple
{
    /// <summary>
    /// The artists who created the album. Each artist is simplified.
    /// </summary>
    [JsonPropertyName("artists")]
    public List<ArtistSimple>? Artists { get; set; }

    /// <summary>
    /// The Spotify ID for the album.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// The album name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// The date the album was first released (precision defined by <see cref="ReleaseDatePrecision"/>).
    /// </summary>
    [JsonPropertyName("release_date")]
    public string? ReleaseDate { get; set; }

    /// <summary>
    /// The precision of the release date: "year", "month", or "day".
    /// </summary>
    [JsonPropertyName("release_date_precision")]
    public string? ReleaseDatePrecision { get; set; }

    /// <summary>
    /// Total number of tracks on the album.
    /// </summary>
    [JsonPropertyName("total_tracks")]
    public int? TotalTracks { get; set; }

    /// <summary>The Spotify URI for the album.</summary>
    [JsonPropertyName("uri")]
    public string? Uri { get; set; }


}