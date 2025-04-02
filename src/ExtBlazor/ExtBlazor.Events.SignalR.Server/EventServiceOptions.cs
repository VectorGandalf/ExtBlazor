using System.Threading.Channels;

namespace ExtBlazor.Events.SignalR.Server;
public static partial class IServiceCollectionExtensions
{
    public class EventServiceOptions
    {
        public string SignalRHubPath { get; set; } = "/eventhub";
        public Func<IEvent, EventRoute>? EventRouter { get; set; } = null;
        public BoundedChannelOptions? ChannelOptions { get; set; }
    }
}