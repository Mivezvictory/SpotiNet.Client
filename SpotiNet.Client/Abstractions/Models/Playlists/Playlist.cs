using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SpotiNet.Client.Models;

/// <summary>
/// A full Spotify Playlist object (e.g., from GET /playlists/{id}).
/// </summary>
public sealed class Playlist
{
    /// <summary>
    /// True if the owner allows other users to add/modify items.
    /// </summary>
    [JsonPropertyName("collaborative")]
    public bool? Collaborative { get; set; }

    /// <summary>
    /// Playlist description, may be empty or null. Can contain HTML.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Known external URLs for the playlist (e.g., Spotify web URL).
    /// </summary>
    [JsonPropertyName("external_urls")]
    public ExternalUrls? ExternalUrls { get; set; }

    /// <summary>
    /// Follower count information for the playlist.
    /// </summary>
    [JsonPropertyName("followers")]
    public Followers? Followers { get; set; }

    /// <summary>
    /// API endpoint URL for this playlist object.
    /// </summary>
    [JsonPropertyName("href")]
    public string? Href { get; set; }

    /// <summary>
    /// The Spotify ID for the playlist.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Playlist cover images in various sizes, widest first.
    /// </summary>
    [JsonPropertyName("images")]
    public List<ImageObject>? Images { get; set; }

    /// <summary>
    /// The name of the playlist.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// The user who owns the playlist (public profile subset).
    /// </summary>
    [JsonPropertyName("owner")]
    public PublicUser? Owner { get; set; }

    /// <summary>
    /// True if the playlist is public; false if private; null if not relevant.
    /// </summary>
    [JsonPropertyName("public")]
    public bool? Public { get; set; }

    /// <summary>
    /// Version identifier for the current playlist snapshot.
    /// </summary>
    [JsonPropertyName("snapshot_id")]
    public string? SnapshotId { get; set; }

    /// <summary>
    /// The tracks (and episodes) in the playlist as a paging container.
    /// </summary>
    [JsonPropertyName("tracks")]
    public Paging<PlaylistItem>? Tracks { get; set; }

    /// <summary>
    /// Object type: always "playlist".
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>
    /// The Spotify URI for the playlist.
    /// </summary>
    [JsonPropertyName("uri")]
    public string? Uri { get; set; }

    /// <summary>
    /// Converts a full <see cref="Playlist"/> to a simplified
    /// <see cref="PlaylistSimple"/>
    /// PlaylistSimple can be null if Playlist is null
    /// </summary>
    public PlaylistSimple ToSimple() => new()
    {
        Id = Id,
        Name = Name,
    };

    /// <summary>
    /// Returns simplified Track
    /// </summary>
    [System.Text.Json.Serialization.JsonIgnore]
    public PlaylistSimple Simple => ToSimple();
}