using Microsoft.AspNetCore.SignalR.Client;

namespace ExtBlazor.Events.SignalR.Client
{
    public class EventsHubConnectionBuilder()
    {
        public Func<Uri, IHubConnectionBuilder> Builder { get; internal set; } = (Uri uri) => new HubConnectionBuilder()
            .WithUrl(uri)
            .WithAutomaticReconnect();
    }
}