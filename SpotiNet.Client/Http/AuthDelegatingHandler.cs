using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace SpotiNet.Client.Http;

internal sealed class AuthDelegatingHandler : DelegatingHandler
{
    /// <summary>
    /// HTTP message handler that injects the <c>Authorization: Bearer</c> header
    /// using an <see cref="SpotiNet.Client.IAccessTokenProvider"/>.
    /// Also sets <c>Accept: application/json</c>.
    /// </summary>
    private readonly SpotiNet.Client.IAccessTokenProvider _tokenProvider;

    /// <summary>
    /// Initializes a new instance of <see cref="AuthDelegatingHandler"/>.
    /// </summary>
    /// <param name="tokenProvider">Provider used to obtain access tokens.</param>
    public AuthDelegatingHandler(SpotiNet.Client.IAccessTokenProvider tokenProvider) => _tokenProvider = tokenProvider;

    /// <summary>
    /// Adds the bearer token to the outgoing request before sending it.
    /// </summary>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        var token = await _tokenProvider.GetAccessTokenAsync(ct);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Headers.Accept.ParseAdd("application/json");
        return await base.SendAsync(request, ct);
    }
}
