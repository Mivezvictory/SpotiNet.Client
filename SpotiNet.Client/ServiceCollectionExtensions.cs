using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SpotiNet.Client.Auth;
using SpotiNet.Client.Http;

namespace SpotiNet.Client;

/// <summary>
/// Dependency injection extensions for registering the Spotify client.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers a DI-first Spotify client backed by <see cref="HttpClient"/> and
    /// <see cref="IAccessTokenProvider"/>. If no token provider factory is supplied,
    /// a simple environment-variable provider is used (reads <c>SPOTIFY_ACCESS_TOKEN</c>).
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configure">Optional configuration for <see cref="SpotifyClientOptions"/>.</param>
    /// <param name="tokenProviderFactory">
    /// Optional factory for supplying a custom <see cref="IAccessTokenProvider"/>.
    /// Provide this when you have client-credentials or user tokens from your own auth flow.
    /// </param>
    /// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
    /// <example>
    /// <code>
    /// services.AddSpotifyClient(opts =>
    /// {
    ///     opts.BaseUrl = "https://api.spotify.com/v1/";
    /// });
    /// </code>
    /// </example>
    public static IServiceCollection AddSpotifyClient(
        this IServiceCollection services,
        Action<SpotifyClientOptions>? configure = null,
        Func<IServiceProvider, IAccessTokenProvider>? tokenProviderFactory = null)
    {
        if (configure != null) services.Configure(configure);

        // Token provider registration
        if (tokenProviderFactory != null)
            services.AddSingleton(tokenProviderFactory);
        else
            services.AddSingleton<IAccessTokenProvider>(_ => new EnvVarAccessTokenProvider("SPOTIFY_ACCESS_TOKEN"));

        // HTTP pipeline
        services.AddTransient<AuthDelegatingHandler>();

        services.AddHttpClient<RawSpotifyClient>((sp, http) =>
        {
            var opts = sp.GetRequiredService<IOptions<SpotifyClientOptions>>().Value;
            http.BaseAddress = new Uri(opts.BaseUrl, UriKind.Absolute);
            http.Timeout = TimeSpan.FromSeconds(30);
        })
        .AddHttpMessageHandler<AuthDelegatingHandler>();

        return services;
    }
}
