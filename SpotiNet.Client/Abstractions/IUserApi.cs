using System.Threading;
using System.Threading.Tasks;
using SpotiNet.Client.Models;

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

    /// <summary>
    /// Get /v1/me/top/artists -Get the current user's top artists
    /// Scopes:user-top-read
    /// </summary>
    /// <param name="timeRange">(optional) The time frame. medium_term (approximately last 6 months), short_term (approximately last 4 weeks). Default: medium_term </param>
    /// <param name="limit">(optional) The maximum number of items to return. Default: 20</param>
    /// <param name="offset">(Optional) The idex of the first item to return. Default: 0</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns></returns>
    Task<TopArtistsResponse> GetUserTopArtistsAsync(
        string? timeRange = "medium_term",
        int? limit = 20,
        int? offset = 0,
        CancellationToken ct = default
    );

    /// <summary>
    /// Get /v1/me/top/tracks -Get the current user's top artists
    /// Scopes:user-top-read
    /// </summary>
    /// <param name="timeRange">(optional) The time frame. medium_term (approximately last 6 months), short_term (approximately last 4 weeks). Default: medium_term </param>
    /// <param name="limit">(optional) The maximum number of items to return. Default: 20</param>
    /// <param name="offset">(Optional) The idex of the first item to return. Default: 0</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns></returns>
    Task<TopTracksResponse> GetUserTopTracksAsync(
        string? timeRange = "medium_term",
        int? limit = 20,
        int? offset = 0,
        CancellationToken ct = default
    );

    /// <summary>
    /// Get /v1/users - Get public profiel information about a Spotify user
    /// Scopes: none
    /// </summary>
    /// <param name="userId">The Users Spotify user ID</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns></returns>
    Task<PublicUser> GetUserAsync(string userId, CancellationToken ct = default);
}