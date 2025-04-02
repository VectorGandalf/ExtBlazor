
namespace ExtBlazor.Events.SignalR.Server
{
    public interface IEventPublisher
    {
        ValueTask Publish(IEvent @event, CancellationToken ct = default);
    }
}