using ExtBlazor.RemoteMediator.Client;
using ExtBlazor.Stash;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddHttpClient("Default", httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
});

builder.Services.AddRemoteMediatorClient();
builder.Services.AddStashService();
await builder.Build().RunAsync();
