using System.Net;
using System.Threading.Tasks;
using Xunit;

public class SpotifyClientUsersTests
{
    [Fact]
    public async Task GetMeAsync_Deserializes_AccountIdAndProfile()
    {
        var json = """
        {
          "account_id": "acc-123",
          "display_name": "Ada Lovelace",
          "id": "ada",
          "type": "user",
          "uri": "spotify:user:ada"
        }
        """;
        var handler = new RecordingHandler(TestResponses.Json(HttpStatusCode.OK, json));
        var client = TestSpotifyClientFactory.Create(handler);

        var me = await client.Users.GetMeAsync();

        Assert.Equal("acc-123", me.AccountId);
        Assert.Equal("Ada Lovelace", me.DisplayName);
        Assert.Equal("ada", me.Id);
    }

    [Fact]
    public async Task GetUserTopArtistsAsync_BuildsQueryString_WithGivenValues()
    {
        var handler = new RecordingHandler(TestResponses.Json(HttpStatusCode.OK, "{\"items\":[]}"));
        var client = TestSpotifyClientFactory.Create(handler);

        await client.Users.GetUserTopArtistsAsync(timeRange: "long_term", limit: 10, offset: 5);

        var req = Assert.Single(handler.Requests);
        Assert.Contains("time_range=long_term", req.RequestUri!.Query);
        Assert.Equal(10, TestUris.QueryInt(req.RequestUri!, "limit", -1));
        Assert.Equal(5, TestUris.QueryInt(req.RequestUri!, "offset", -1));
    }

    [Fact]
    public async Task GetUserTopArtistsAsync_UsesDefaults_WhenNotSpecified()
    {
        var handler = new RecordingHandler(TestResponses.Json(HttpStatusCode.OK, "{\"items\":[]}"));
        var client = TestSpotifyClientFactory.Create(handler);

        await client.Users.GetUserTopArtistsAsync();

        var req = Assert.Single(handler.Requests);
        Assert.Contains("time_range=medium_term", req.RequestUri!.Query);
        Assert.Equal(20, TestUris.QueryInt(req.RequestUri!, "limit", -1));
        Assert.Equal(0, TestUris.QueryInt(req.RequestUri!, "offset", -1));
    }

    [Fact]
    public async Task GetUserTopTracksAsync_BuildsQueryString_WithGivenValues()
    {
        var handler = new RecordingHandler(TestResponses.Json(HttpStatusCode.OK, "{\"items\":[]}"));
        var client = TestSpotifyClientFactory.Create(handler);

        await client.Users.GetUserTopTracksAsync(timeRange: "short_term", limit: 15, offset: 2);

        var req = Assert.Single(handler.Requests);
        Assert.Contains("time_range=short_term", req.RequestUri!.Query);
        Assert.Equal(15, TestUris.QueryInt(req.RequestUri!, "limit", -1));
        Assert.Equal(2, TestUris.QueryInt(req.RequestUri!, "offset", -1));
    }
}
