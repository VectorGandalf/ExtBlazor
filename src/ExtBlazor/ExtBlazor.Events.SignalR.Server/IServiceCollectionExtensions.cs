using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace ExtBlazor.Events.SignalR.Server;
public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddSignalREventService(this IServiceCollection services, string signalRHubPath = "/eventhub")
    {
        services.AddSignalR();
        services.AddSingleton(new EventListenerConfiguration { SignalRHubPath = signalRHubPath });
        services.AddSingleton<IEventService, InProcessEventService>();
        return services;
    }

    public static void UseSignalREventService(this IHost host)
    {
        if (host is IEndpointRouteBuilder endpointRouteBuilder)
        {
            var uri = host.Services.GetService<EventListenerConfiguration>()!.SignalRHubPath;
            endpointRouteBuilder.MapHub<EventHub>(uri);
        }

        var eventService = host.Services.GetService<IEventService>()
            ?? throw new NullReferenceException("No Event Service (IEventService) added!");

        eventService.Register<IEvent>((IEvent @event, IHubContext<EventHub> hub) =>
            hub.Clients.All.SendAsync(
                "send_event",
                JsonSerializer.Serialize(new JsonParcel(@event))));
    }
}