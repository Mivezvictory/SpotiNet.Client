using System.Threading;
using System.Threading.Tasks;

namespace SpotiNet.Client;

/// <summary>
/// User endpoints
/// </summary>
public interface IUsersApi
{
    /// <summary>
    /// Get /v1/me - current user profile (requires user token).
    /// Scopes: none for basic profile; add user-read-email if needed
    /// </summary>
    /// <param name="ct">CancellationToken</param>
    /// <returns></returns>
    Task<CurrentUserProfile> GetMeAsync(CancellationToken ct = default);
}