using ExtBlazor.Demo.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp =>
    new HttpClient
    {
        BaseAddress = new Uri("https://localhost:7074")
    });

builder.Services.AddScoped<IQueryService, QueryService>();

await builder.Build().RunAsync();
