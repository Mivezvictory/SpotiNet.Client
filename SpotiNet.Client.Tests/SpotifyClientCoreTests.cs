using System.Net;
using System.Threading.Tasks;
using SpotiNet.Client;
using Xunit;

// SpotifyClient's private generic SendAsync<T> duplicates RawSpotifyClient's error-handling logic
// but had zero coverage of its own before this file.
public class SpotifyClientCoreTests
{
    [Fact]
    public async Task TypedCall_WhenSpotifyErrorEnvelope_ThrowsSpotifyApiException_WithMessage()
    {
        var errorJson = "{\"error\":{\"status\":403,\"message\":\"Insufficient client scope\"}}";
        var handler = new RecordingHandler(TestResponses.Json(HttpStatusCode.Forbidden, errorJson));
        var client = TestSpotifyClientFactory.Create(handler);

        var ex = await Assert.ThrowsAsync<SpotifyApiException>(() => client.Users.GetMeAsync());

        Assert.Equal(HttpStatusCode.Forbidden, ex.StatusCode);
        Assert.Contains("Insufficient client scope", ex.Message);
        Assert.Equal(errorJson, ex.ResponseBody);
    }

    [Fact]
    public async Task TypedCall_WhenNonJsonError_ThrowsSpotifyApiException_WithHttpReason()
    {
        const string body = "Gateway crashed";
        var handler = new RecordingHandler(new HttpResponseMessage(HttpStatusCode.BadGateway)
        {
            Content = new StringContent(body),
            ReasonPhrase = "Bad Gateway"
        });
        var client = TestSpotifyClientFactory.Create(handler);

        var ex = await Assert.ThrowsAsync<SpotifyApiException>(() => client.Artists.GetAsync("any"));

        Assert.Equal(HttpStatusCode.BadGateway, ex.StatusCode);
        Assert.Contains("HTTP 502 Bad Gateway", ex.Message);
        Assert.Equal(body, ex.ResponseBody);
    }
}
