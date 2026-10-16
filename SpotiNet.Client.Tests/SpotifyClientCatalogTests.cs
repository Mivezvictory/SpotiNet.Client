using System.Net;
using System.Threading.Tasks;
using Xunit;

public class SpotifyClientCatalogTests
{
    [Fact]
    public async Task Artists_GetAsync_DeserializesArtist()
    {
        var handler = new RecordingHandler(TestResponses.Json(HttpStatusCode.OK, "{\"id\":\"a1\",\"name\":\"Fela Kuti\"}"));
        var client = TestSpotifyClientFactory.Create(handler);

        var artist = await client.Artists.GetAsync("a1");

        Assert.Equal("Fela Kuti", artist.Name);
        Assert.Equal("/v1/artists/a1", handler.Requests[0].RequestUri!.AbsolutePath);
    }

    [Fact]
    public async Task Albums_GetAsync_DeserializesAlbum_WithMarketQueryParam()
    {
        var handler = new RecordingHandler(TestResponses.Json(HttpStatusCode.OK, "{\"id\":\"al1\",\"name\":\"Zenith\"}"));
        var client = TestSpotifyClientFactory.Create(handler);

        var album = await client.Albums.GetAsync("al1", market: "US");

        Assert.Equal("Zenith", album.Name);
        Assert.Contains("market=US", handler.Requests[0].RequestUri!.Query);
    }

    [Fact]
    public async Task Tracks_GetAsync_DeserializesTrack_WithMarketQueryParam()
    {
        var handler = new RecordingHandler(TestResponses.Json(HttpStatusCode.OK, "{\"id\":\"t1\",\"name\":\"Zombie\"}"));
        var client = TestSpotifyClientFactory.Create(handler);

        var track = await client.Tracks.GetAsync("t1", market: "NG");

        Assert.Equal("Zombie", track.Name);
        Assert.Contains("market=NG", handler.Requests[0].RequestUri!.Query);
    }
}
