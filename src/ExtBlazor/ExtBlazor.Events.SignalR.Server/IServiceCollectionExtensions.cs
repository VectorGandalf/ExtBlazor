using System.Text.Json;
using Microsoft.AspNetCore.SignalR;

namespace ExtBlazor.Events.SignalR.Server;
public static class IServiceCollectionExtensions
{
    public static ISignalRServerBuilder AddSignalREventService(this IServiceCollection services, 
        string signalRHubPath = "/eventhub", 
        Func<IEvent, EventRoute>? eventRouter = null)
    {
        services.AddSingleton(new EventListenerConfiguration
        {
            SignalRHubPath = signalRHubPath
        });
        services.AddSingleton<IEventService, InProcessEventService>();
        services.AddSingleton(new EventRouterConfiguration
        {
            EventRouter = eventRouter
        });

        return services.AddSignalR();
    }

    public static HubEndpointConventionBuilder UseSignalREventService(this IHost host)
    {        
        var eventService = host.Services.GetService<IEventService>()
            ?? throw new NullReferenceException("No Event Service (IEventService) added!");
                
        eventService.Register<IEvent>((IEvent @event, IHubContext<EventHub> hub, EventRouterConfiguration eventRouterConfiguration) =>
        {
            var method = "send_event";
            var parcel = JsonSerializer.Serialize(new JsonParcel(@event));
            if (eventRouterConfiguration.EventRouter is null)
            {
                hub.Clients.All.SendAsync(method, parcel);
            }
            else 
            {
                var router = eventRouterConfiguration.EventRouter;
                var route = router(@event);
                if (route.All)
                {
                    hub.Clients.All.SendAsync(method, parcel);
                }
                else 
                {
                    hub.Clients.Groups(route.Groups);
                    hub.Clients.Users(route.Users);
                    hub.Clients.Clients(route.Clients);
                }
            }
        });

        var uri = host.Services.GetService<EventListenerConfiguration>()
            ?.SignalRHubPath ?? throw new NullReferenceException("SignalRHubPath must have a value.");

        return ((IEndpointRouteBuilder)host).MapHub<EventHub>(uri);
    }
}