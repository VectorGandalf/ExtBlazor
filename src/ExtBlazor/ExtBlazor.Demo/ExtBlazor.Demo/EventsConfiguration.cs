using ExtBlazor.Demo.Client.Models;
using ExtBlazor.Events;
using ExtBlazor.Events.SignalR.Server;
using Microsoft.AspNetCore.SignalR;

namespace ExtBlazor.Demo;

public static class EventsConfiguration
{
    public static EventRoute EventRouter(IEvent @event)
    {
        if (@event is TickEvent)
        {
            return new()
            {
                Groups = ["TickReceiver"]
            };
        }

        return new()
        {
            All = true
        };
    }
    public static Action<HubOptions<EventHub>> HubOptions => options =>
    {
        options.AddFilter(new HubFilter());
    };
}

