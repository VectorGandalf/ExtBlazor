using System.Text.Json;
using Microsoft.AspNetCore.SignalR;

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

    public static HubEndpointConventionBuilder UseSignalREventService(this IHost host)
    {
        var endpointRouteBuilder = (IEndpointRouteBuilder)host;

        var uri = host.Services.GetService<EventListenerConfiguration>()!.SignalRHubPath;
        var hubEndpointConventionBuilder = endpointRouteBuilder.MapHub<EventHub>(uri);

        var eventService = host.Services.GetService<IEventService>()
            ?? throw new NullReferenceException("No Event Service (IEventService) added!");

        //TODO: Add configurable event router!
        eventService.Register<IEvent>((IEvent @event, IHubContext<EventHub> hub) =>
            hub.Clients.All.SendAsync(
                "send_event",
                JsonSerializer.Serialize(new JsonParcel(@event))));

        return hubEndpointConventionBuilder;
    }
}