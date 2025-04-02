namespace ExtBlazor.Events.SignalR.Server;
public class EventRouterConfiguration
{
    public Func<IEvent, EventRoute>? EventRouter { get; set; }
}