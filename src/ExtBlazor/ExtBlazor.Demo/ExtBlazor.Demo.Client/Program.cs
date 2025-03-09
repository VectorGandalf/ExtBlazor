using ExtBlazor.Events.SignalR.Client;
using ExtBlazor.RemoteMediator.Client;
using ExtBlazor.Stash;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddSignalREventServiceClient();
builder.Services.AddHttpClient("Default", httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
});

builder.Services.AddRemoteMediatorClient();
builder.Services.AddStashService();
await builder.Build().RunAsync();
