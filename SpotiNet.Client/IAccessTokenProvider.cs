using System.Threading;
using System.Threading.Tasks;

namespace SpotiNet.Client;

/// <summary>
/// Supplies OAuth access tokens for calling the Spotify Web API.
/// </summary>
public interface IAccessTokenProvider
{
    /// <summary>
    /// Gets a valid OAuth access token to include in the <c>Authorization: Bearer</c> header.
    /// Implementations may cache and refresh tokens as needed.
    /// </summary>
    /// <param name="cancellationToken">A token to observe while waiting for the token.</param>
    /// <returns>The access token string.</returns>
    Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default);
}
