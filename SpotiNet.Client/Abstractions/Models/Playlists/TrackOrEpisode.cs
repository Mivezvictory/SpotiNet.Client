using System.Text.Json.Serialization;

namespace SpotiNet.Client.Models;

/// <summary>
/// A discriminated wrapper that can hold either a Track or an Episode from the playlist.
/// Only one of <see cref="Track"/> or <see cref="Episode"/> will be non-null.
/// </summary>
public sealed class TrackOrEpisode
{
    /// <summary>The track, when this playlist item is a track.</summary>
    [JsonPropertyName("track")]
    public Track? Track { get; set; }

    /// <summary>The episode, when this playlist item is a podcast episode.</summary>
    [JsonPropertyName("episode")]
    public Episode? Episode { get; set; }
}
