using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

public class SpotifyClientPlaylistsTests
{
    [Fact]
    public async Task CreateAsync_PostsToMePlaylists_WithNameDescriptionPublic_AndNoUserId()
    {
        var handler = new RecordingHandler(TestResponses.Json(HttpStatusCode.Created, "{\"id\":\"pl1\",\"name\":\"SpotiNet Test\"}"));
        var client = TestSpotifyClientFactory.Create(handler);

        var playlist = await client.Playlists.CreateAsync("SpotiNet Test", "desc", isPublic: false);

        var req = Assert.Single(handler.Requests);
        Assert.Equal(HttpMethod.Post, req.Method);
        Assert.Equal("/v1/me/playlists", req.RequestUri!.AbsolutePath);
        var body = handler.RequestBodies[0];
        Assert.Contains("\"name\":\"SpotiNet Test\"", body);
        Assert.Contains("\"description\":\"desc\"", body);
        Assert.Contains("\"public\":false", body);
        Assert.Equal("pl1", playlist.Id);
    }

    [Fact]
    public async Task AddItemsAsync_PostsToItemsPath_WithUrisAndPosition()
    {
        var handler = new RecordingHandler(TestResponses.Json(HttpStatusCode.OK, "{\"snapshot_id\":\"abc\"}"));
        var client = TestSpotifyClientFactory.Create(handler);

        await client.Playlists.AddItemsAsync("pl1", new[] { "spotify:track:aaa", "spotify:track:bbb" }, position: 3);

        var req = Assert.Single(handler.Requests);
        Assert.Equal(HttpMethod.Post, req.Method);
        Assert.Equal("/v1/playlists/pl1/items", req.RequestUri!.AbsolutePath);
        var body = handler.RequestBodies[0];
        Assert.Contains("\"uris\":[\"spotify:track:aaa\",\"spotify:track:bbb\"]", body);
        Assert.Contains("\"position\":3", body);
    }

    [Fact]
    public async Task AddItemsAsync_OmitsPosition_SendsNullWhenNotSpecified()
    {
        var handler = new RecordingHandler(TestResponses.Json(HttpStatusCode.OK, "{\"snapshot_id\":\"abc\"}"));
        var client = TestSpotifyClientFactory.Create(handler);

        await client.Playlists.AddItemsAsync("pl1", new[] { "spotify:track:aaa" });

        Assert.Single(handler.Requests);
        var body = handler.RequestBodies[0];
        Assert.Contains("\"position\":null", body);
    }

    [Fact]
    public async Task RemoveItemsAsync_DeletesToItemsPath_ButBodyKeyStaysTracks()
    {
        var handler = new RecordingHandler(TestResponses.Json(HttpStatusCode.OK, "{\"snapshot_id\":\"abc\"}"));
        var client = TestSpotifyClientFactory.Create(handler);

        await client.Playlists.RemoveItemsAsync("pl1", new[] { "spotify:track:aaa" });

        var req = Assert.Single(handler.Requests);
        Assert.Equal(HttpMethod.Delete, req.Method);
        Assert.Equal("/v1/playlists/pl1/items", req.RequestUri!.AbsolutePath);
        var body = handler.RequestBodies[0];
        Assert.Contains("\"tracks\":[{\"uri\":\"spotify:track:aaa\"}]", body);
    }

    [Fact]
    public async Task GetAsync_ParsesItemsField_IntoPlaylistItemsProperty()
    {
        // Regression test for Spotify's Feb 2026 rename: "tracks"/"track" -> "items"/"item".
        var json = """
        {
          "id": "pl1",
          "name": "My Playlist",
          "items": {
            "total": 1,
            "items": [
              {
                "added_at": "2026-01-01T00:00:00Z",
                "item": {
                  "type": "track",
                  "id": "t1",
                  "name": "Zombie",
                  "uri": "spotify:track:t1"
                }
              }
            ]
          }
        }
        """;
        var handler = new RecordingHandler(TestResponses.Json(HttpStatusCode.OK, json));
        var client = TestSpotifyClientFactory.Create(handler);

        var playlist = await client.Playlists.GetAsync("pl1");

        Assert.Equal(1, playlist.Items!.Total);
        var item = Assert.Single(playlist.Items!.Items!);
        Assert.Equal("Zombie", item.Item!.Name);
        Assert.True(item.Item!.IsTrack);
    }

    [Fact]
    public async Task GetCurrentUserPlaylistsAsync_BuildsQueryString_WithLimitAndOffset()
    {
        var handler = new RecordingHandler(TestResponses.Json(HttpStatusCode.OK, "{\"items\":[]}"));
        var client = TestSpotifyClientFactory.Create(handler);

        await client.Playlists.GetCurrentUserPlaylistsAsync(limit: 10, offset: 5);

        var req = Assert.Single(handler.Requests);
        Assert.Equal("/v1/me/playlists", req.RequestUri!.AbsolutePath);
        Assert.Equal(10, TestUris.QueryInt(req.RequestUri!, "limit", -1));
        Assert.Equal(5, TestUris.QueryInt(req.RequestUri!, "offset", -1));
    }
}
