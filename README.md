SpotiNet.Client
===============

> Lightweight .NET client for typed Spotify Web API calls (**unofficial**).

SpotiNet.Client is a minimal C# helper for calling Spotify Web API endpoints and getting strongly-typed JSON back. It exposes a single generic entry point---so you can focus on your models, not plumbing.

Signature
---------

`public static async Task<T> GetSpotifyResponseJson<T>(  string url,
    string token,
    string method,
    ILogger logger)`

Features
--------

-   Bearer authentication

-   HTTP verbs (`GET`, `POST`, etc.)

-   Structured logging via `ILogger`

-   JSON deserialization into `T`

> **Note:** This project is not affiliated with or endorsed by Spotify.