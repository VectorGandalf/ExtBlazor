using ExtBlazor.RemoteMediator.Client;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddHttpClient("Default", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://localhost:7074");
});

builder.Services.AddRemoteMediatorClient();

await builder.Build().RunAsync();
