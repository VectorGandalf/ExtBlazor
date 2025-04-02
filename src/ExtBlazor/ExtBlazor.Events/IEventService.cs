namespace ExtBlazor.Events;
public interface IEventService
{
    Guid Subscribe<TEvent>(Delegate eventHandler) where TEvent : IEvent;
    void Unsubscribe(Guid eventHandlerId);
    void Handle(IEvent @event);
}
