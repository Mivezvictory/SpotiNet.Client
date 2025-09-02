using System.Text.Json;

namespace SpotiNet.Client;

/// <summary>
/// Helpers to parse Spotify's error envelope: { "error": { "status": int, "message": string } }
/// </summary>
internal static class SpotifyErrorParser
{
    private sealed record ErrorContainer(SpotifyError error);
    private sealed record SpotifyError(int status, string message);

    /// <summary>Attempts to read the "error.message" from the body; returns null if parsing fails.</summary>
    public static string? TryReadMessage(string body)
    {
        try
        {
            var parsed = JsonSerializer.Deserialize<ErrorContainer>(body);
            return parsed?.error.message;
        }
        catch
        {
            return null;
        }
    }
}
