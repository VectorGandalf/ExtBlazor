using System.Text.Json;
using Microsoft.AspNetCore.SignalR;

namespace ExtBlazor.Events.SignalR.Server;

public static class IHostExtensions
{
    public static HubEndpointConventionBuilder UseSignalREventService(this IHost host)
    {
        using var serviceScope = host.Services.CreateScope();

        var eventService = serviceScope.ServiceProvider.GetService<IEventService>()
            ?? throw new NullReferenceException("No Event Service (IEventService) added!");

        eventService.Subscribe<IEvent>(SendEvent);

        var uri = host.Services.GetService<EventHubPathConfiguration>()
            ?.Path ?? throw new NullReferenceException("SignalRHubPath must have a value.");

        return ((IEndpointRouteBuilder)host).MapHub<EventHub>(uri);
    }

    private static Task SendEvent(IEvent @event, IHubContext<EventHub> hub, EventRouterConfiguration eventRouterConfiguration)
    {
        var method = "send_event";
        var parcel = JsonSerializer.Serialize(new JsonParcel(@event));

        if (eventRouterConfiguration.EventRouter is null)
        {
            return hub.Clients.All.SendAsync(method, parcel);
        }

        var route = eventRouterConfiguration.EventRouter(@event);
        if (route.All)
        {
            return hub.Clients.All.SendAsync(method, parcel);
        }

        return Task.WhenAll(
            hub.Clients.Groups(route.Groups).SendAsync(method, parcel),
            hub.Clients.Users(route.Users).SendAsync(method, parcel),
            hub.Clients.Clients(route.Clients).SendAsync(method, parcel));
    }
}