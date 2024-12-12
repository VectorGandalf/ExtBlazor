using Microsoft.AspNetCore.SignalR;

namespace ExtBlazor.Events.SignalR.Server;
public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddEventService(this IServiceCollection services)
    {
        services.AddSignalR();
        services.AddSingleton<IEventService, InProcessEventService>();
        return services;
    }

    public static void UseEventService(this IHost host)
    {
        var eventService = host.Services.GetService<IEventService>()
            ?? throw new NullReferenceException("No Event Service (IEventService) added!");

        eventService.Register<IEvent>((IEvent @event, IHubContext hub) =>
        {
            hub.Clients.All.SendAsync("send_event", new JsonParcel(@event));
        });
    }
}