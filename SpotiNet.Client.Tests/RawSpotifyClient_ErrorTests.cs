using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using SpotiNet.Client;
using Xunit;

public class RawSpotifyClient_ErrorTests
{
    [Fact]
    public async Task GetAsync_WhenSpotifyErrorEnvelope_ThrowsSpotifyApiException_WithMessage()
    {
        // Arrange: Spotify-style error body
        var errorJson = "{\"error\":{\"status\":401,\"message\":\"Invalid access token\"}}";
        var handler = new StubHandler(new HttpResponseMessage(HttpStatusCode.Unauthorized)
        {
            Content = new StringContent(errorJson, Encoding.UTF8, "application/json"),
            ReasonPhrase = "Unauthorized"
        });

        var http = new HttpClient(handler) { BaseAddress = new Uri("https://api.spotify.com/v1/") };
        var options = Options.Create(new SpotifyClientOptions());
        var client = new RawSpotifyClient(http, options, NullLogger<RawSpotifyClient>.Instance);

        // Act + Assert
        var ex = await Assert.ThrowsAsync<SpotifyApiException>(() => client.GetAsync<object>("me"));
        Assert.Equal(HttpStatusCode.Unauthorized, ex.StatusCode);
        Assert.Contains("Invalid access token", ex.Message);
        Assert.Equal(errorJson, ex.ResponseBody);
    }

    [Fact]
    public async Task GetAsync_WhenNonJsonError_ThrowsSpotifyApiException_WithHttpReason()
    {
        // Arrange: Non-JSON body; parser will return null, we fall back to HTTP code/reason
        const string body = "Gateway crashed";
        var handler = new StubHandler(new HttpResponseMessage(HttpStatusCode.BadGateway)
        {
            Content = new StringContent(body, Encoding.UTF8, "text/plain"),
            ReasonPhrase = "Bad Gateway"
        });

        var http = new HttpClient(handler) { BaseAddress = new Uri("https://api.spotify.com/v1/") };
        var options = Options.Create(new SpotifyClientOptions());
        var client = new RawSpotifyClient(http, options, NullLogger<RawSpotifyClient>.Instance);

        // Act + Assert
        var ex = await Assert.ThrowsAsync<SpotifyApiException>(() => client.GetAsync<object>("anything"));
        Assert.Equal(HttpStatusCode.BadGateway, ex.StatusCode);
        Assert.Contains("HTTP 502 Bad Gateway", ex.Message);
        Assert.Equal(body, ex.ResponseBody);
    }

    private sealed class StubHandler : HttpMessageHandler
    {
        private readonly HttpResponseMessage _response;
        public StubHandler(HttpResponseMessage response) => _response = response;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => Task.FromResult(_response);
    }
}
