﻿using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

namespace ExtBlazor.Events.SignalR.Client;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddSignalREventServiceClient(
        this IServiceCollection services,
        string signalRHubPath = EventHubPathConfiguration.DEFAULT_PATH,
        Func<Uri, HubConnectionBuilder>? hubConnectionBuilder = null)
    {
        var hubBuilderConfiguration = new EventsHubConnectionBuilder();
        if (hubConnectionBuilder is not null)
        {
            hubBuilderConfiguration.Builder = hubConnectionBuilder;
        }

        services.AddScoped<EventsHubConnectionBuilder>(_ => hubBuilderConfiguration);
        if (System.OperatingSystem.IsBrowser())
        {
            services.AddScoped<IEventService, InProcessEventService>();
            services.AddSingleton(new EventHubPathConfiguration
            {
                Path = signalRHubPath
            });
        }

        return services;
    }
}