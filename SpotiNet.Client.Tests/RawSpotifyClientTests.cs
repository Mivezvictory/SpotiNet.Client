using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SpotiNet.Client;
using Xunit;

public class RawSpotifyClientTests
{
    [Fact]
    public async Task GetAsync_Deserializes_Json()
    {
        // Arrange: a fake response payload
        var json = "{\"name\":\"Stub Artist\"}";
        var handler = new StubHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        });

        var http = new HttpClient(handler) { BaseAddress = new Uri("https://api.spotify.com/v1/") };
        var options = Options.Create(new SpotifyClientOptions());
        var client = new RawSpotifyClient(http, options);

        // Act
        var doc = await client.GetAsync<JsonDocument>("artists/anything");

        // Assert
        Assert.Equal("Stub Artist", doc.RootElement.GetProperty("name").GetString());
    }

    private sealed class StubHandler : HttpMessageHandler
    {
        private readonly HttpResponseMessage _response;
        public StubHandler(HttpResponseMessage response) => _response = response;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => Task.FromResult(_response);
    }
}
