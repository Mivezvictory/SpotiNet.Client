using System;
using System.Net;

namespace SpotiNet.Client;

/// <summary>
/// Exception representing a non-success response from the Spotify Web API.
/// </summary>
public sealed class SpotifyApiException : Exception
{
    /// <summary>HTTP status code returned by Spotify.</summary>
    public HttpStatusCode StatusCode { get; }

    /// <summary>Raw response body returned by Spotify (for diagnostics).</summary>
    public string ResponseBody { get; }

    public SpotifyApiException(HttpStatusCode statusCode, string message, string responseBody)
        : base(message)
    {
        StatusCode = statusCode;
        ResponseBody = responseBody;
    }
}
