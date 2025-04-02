using System.Threading.Channels;

namespace ExtBlazor.Events.SignalR.Server;
public class ChannelEventPublisher(BoundedChannelOptions options) : IEventPublisher
{
    private Channel<IEvent> eventBusChannel = Channel.CreateBounded<IEvent>(options);

    public ValueTask Publish(IEvent @event, CancellationToken ct = default)
        => eventBusChannel.Writer.WriteAsync(@event, ct);

    public ValueTask<IEvent> Read(CancellationToken ct = default)
        => eventBusChannel.Reader.ReadAsync(ct);
}
