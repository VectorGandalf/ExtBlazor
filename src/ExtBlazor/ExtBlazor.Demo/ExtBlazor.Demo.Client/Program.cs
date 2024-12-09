using ExtBlazor.RemoteMediator.Client;
using ExtBlazor.Stash;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddHttpClient("Default", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://localhost:7074");
});

builder.Services.AddRemoteMediatorClient();
builder.Services.AddStashService();
await builder.Build().RunAsync();
