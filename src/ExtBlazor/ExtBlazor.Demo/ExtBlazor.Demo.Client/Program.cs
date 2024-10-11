using ExtBlazor.Demo.Client.Services;
using ExtBlazor.RemoteMediator.Client;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddHttpClient("Default", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://localhost:7074");
});

builder.Services.AddRemoteMediatorClient();
builder.Services.AddSingleton<IStashService, StashService>();
await builder.Build().RunAsync();
