using System.Threading.Channels;

namespace ExtBlazor.Events.SignalR.Server;
public static partial class IServiceCollectionExtensions
{
    public class EventServiceOptions
    {
        public string SignalRHubPath { get; set; } = EventHubPathConfiguration.DEFAULT_PATH;
        public Func<IEvent, EventRoute>? EventRouter { get; set; } = null;
        public BoundedChannelOptions? ChannelOptions { get; set; }

        public BoundedChannelOptions DefaultChannelOptions => new BoundedChannelOptions(10)
        {
            AllowSynchronousContinuations = false,
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = true,
            SingleWriter = false
        };
    }
}