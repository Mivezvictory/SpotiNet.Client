using System;
using System.Threading;
using System.Threading.Tasks;

namespace SpotiNet.Client.Auth;
/// <summary>
/// Simple <see cref="IAccessTokenProvider"/> that reads a token
/// from an environment variable (default: <c>SPOTIFY_ACCESS_TOKEN</c>).
/// Useful for quick local testing.
/// </summary>
internal sealed class EnvVarAccessTokenProvider : IAccessTokenProvider
{
    private readonly string _envVar;
    /// <summary>
    /// Creates a provider that reads from the specified environment variable.
    /// Gives uses the ability to provide whatever environment variable holds their spotify access token.
    /// </summary>
    /// <param name="envVar">The environment variable name that contains the token.</param>
    public EnvVarAccessTokenProvider(string envVar = "SPOTIFY_ACCESS_TOKEN") => _envVar = envVar;

    /// <inheritdoc />
    public Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        var token = Environment.GetEnvironmentVariable(_envVar);
        if (string.IsNullOrWhiteSpace(token))
            throw new InvalidOperationException($"No access token found in environment variable '{_envVar}'.");
        return Task.FromResult(token);
    }
}
