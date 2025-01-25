using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

namespace ExtBlazor.Events.SignalR.Client;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddEventServiceClient(
        this IServiceCollection services, 
        string signalRHubPath = "/eventhub")
    {
        services.AddScoped<IEventService, InProcessEventService>();
        services.AddSingleton(new EventListenerConfiguration 
        { 
            SignalRHubPath = signalRHubPath 
        });
        return services;
    }
}