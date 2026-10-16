using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using SpotiNet.Client.Models;
using Xunit;

public class SpotifyClientSearchTests
{
    [Fact]
    public async Task SearchTracksAsync_ClampsOutgoingPageSizeTo10_WhileAccumulatingRequestedTotal()
    {
        const int pageSize = 10;
        const int requestedTotal = 30;
        const int catalogTotal = 100;

        HttpResponseMessage Responder(HttpRequestMessage req)
        {
            var offset = TestUris.QueryInt(req.RequestUri!, "offset", 0);
            var limit = TestUris.QueryInt(req.RequestUri!, "limit", pageSize);
            var items = Enumerable.Range(offset, limit)
                .Select(i => $"{{\"id\":\"t{i}\",\"name\":\"Track {i}\"}}");
            var nextOffset = offset + limit;
            var next = nextOffset < catalogTotal
                ? $"\"https://api.spotify.com/v1/search?q=lofi&type=track&limit={limit}&offset={nextOffset}\""
                : "null";
            var json = $"{{\"tracks\":{{\"total\":{catalogTotal},\"items\":[{string.Join(",", items)}],\"next\":{next}}}}}";
            return TestResponses.Json(HttpStatusCode.OK, json);
        }

        var handler = new RecordingHandler(Responder);
        var client = TestSpotifyClientFactory.Create(handler);

        var results = new List<Track>();
        await foreach (var t in client.Search.SearchTracksAsync("lofi", limit: requestedTotal))
            results.Add(t);

        Assert.Equal(requestedTotal, results.Count);
        Assert.NotEmpty(handler.Requests);
        Assert.All(handler.Requests, r => Assert.Equal(pageSize, TestUris.QueryInt(r.RequestUri!, "limit", -1)));
    }

    [Fact]
    public async Task SearchTracksAsync_DefaultLimit_Sends5WhenNotSpecified()
    {
        var handler = new RecordingHandler(TestResponses.Json(HttpStatusCode.OK, "{\"tracks\":{\"total\":0,\"items\":[],\"next\":null}}"));
        var client = TestSpotifyClientFactory.Create(handler);

        var results = new List<Track>();
        await foreach (var t in client.Search.SearchTracksAsync("lofi"))
            results.Add(t);

        Assert.Empty(results);
        var req = Assert.Single(handler.Requests);
        Assert.Equal(5, TestUris.QueryInt(req.RequestUri!, "limit", -1));
    }

    [Fact]
    public async Task SearchArtistsAsync_UnwrapsArtistsEnvelope()
    {
        var json = "{\"artists\":{\"total\":1,\"items\":[{\"id\":\"a1\",\"name\":\"Bob Marley\"}],\"next\":null}}";
        var handler = new RecordingHandler(TestResponses.Json(HttpStatusCode.OK, json));
        var client = TestSpotifyClientFactory.Create(handler);

        var results = new List<Artist>();
        await foreach (var a in client.Search.SearchArtistsAsync("bob marley"))
            results.Add(a);

        var artist = Assert.Single(results);
        Assert.Equal("Bob Marley", artist.Name);
    }

    [Fact]
    public async Task SearchAlbumsAsync_UnwrapsAlbumsEnvelope()
    {
        var json = "{\"albums\":{\"total\":1,\"items\":[{\"id\":\"al1\",\"name\":\"Legend\"}],\"next\":null}}";
        var handler = new RecordingHandler(TestResponses.Json(HttpStatusCode.OK, json));
        var client = TestSpotifyClientFactory.Create(handler);

        var results = new List<Album>();
        await foreach (var al in client.Search.SearchAlbumsAsync("legend"))
            results.Add(al);

        var album = Assert.Single(results);
        Assert.Equal("Legend", album.Name);
    }
}
