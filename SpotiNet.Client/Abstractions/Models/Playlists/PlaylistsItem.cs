using System;
using System.Text.Json.Serialization;

namespace SpotiNet.Client.Models;

/// <summary>
/// An item in a playlist (track or episode) with metadata about its addition.
/// </summary>
public sealed class PlaylistItem
{
    /// <summary>
    /// The date and time the item was added (ISO 8601), or null if unknown.
    /// </summary>
    [JsonPropertyName("added_at")]
    public DateTimeOffset? AddedAt { get; set; }

    /// <summary>
    /// The user who added the item, if known.
    /// </summary>
    [JsonPropertyName("added_by")]
    public PublicUser? AddedBy { get; set; }

    /// <summary>
    /// Whether the item is a local file.
    /// </summary>
    [JsonPropertyName("is_local")]
    public bool? IsLocal { get; set; }

    /// <summary>
    /// The track or episode for this item. Renamed from "track" to "item" in Spotify's Feb 2026 API changes.
    /// </summary>
    [JsonPropertyName("item")]
    public TrackOrEpisode? Item { get; set; }
}
