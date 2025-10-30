using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SpotiNet.Client.Models;

/// <summary>
/// Generic offset-based paging wrapper returned by Spotify endpoints.
/// </summary>
public class Paging<TItem>
{
    /// <summary>
    /// A link to the WebAPI endpoint returning the full result of the request
    /// </summary>
    [JsonPropertyName("href")] public string? Href { get; set; }

    /// <summary>
    /// List of TItem(Artist/Album)
    /// </summary>
    [JsonPropertyName("items")] public List<TItem>? Items { get; set; }

    /// <summary>
    /// The maximum number of items in the response
    /// </summary>
    [JsonPropertyName("limit")] public int? Limit { get; set; }

    /// <summary>
    /// URL to the next page of items
    /// </summary>
    [JsonPropertyName("next")] public string? Next { get; set; }

    /// <summary>
    /// The offset of the ites returned
    /// </summary>
    [JsonPropertyName("offset")] public int? Offset { get; set; }

    /// <summary>
    /// URL to the previous page of items
    /// </summary>
    [JsonPropertyName("previous")] public string? Previous { get; set; }

    /// <summary>
    /// Total number of items available to return
    /// </summary>
    [JsonPropertyName("total")] public int? Total { get; set; }
}
