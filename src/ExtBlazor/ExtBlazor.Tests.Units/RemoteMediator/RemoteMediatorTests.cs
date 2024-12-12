using ExtBlazor.RemoteMediator.Client;
using ExtBlazor.Tests.RemoteMediator.Web.Application;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ExtBlazor.Tests.Units.RemoteMediator;

[Collection("Sequential")]
public class RemoteMediatorTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task Send()
    {
        // Arrange
        var httpClient = factory.CreateClient();
        var httpClientFactory = new FakeHttpClientFactory(httpClient);
        var remoteMediator = new HttpRemoteMediator(httpClientFactory, new());
        await remoteMediator.Send(new CreateObjectCommand(1, "Test object"));

        // Act
        var objects = await remoteMediator.Send(new GetObjectsQuery());

        // Assert
        Assert.NotNull(objects);
        Assert.True(objects?.Any(o => o.Id == 1 && o.Name == "Test object"));
    }
}

public class FakeHttpClientFactory(HttpClient client) : IHttpClientFactory
{
    public HttpClient CreateClient(string name = "Default")
    {
        return client;
    }
}


