namespace ExtBlazor.Events;
public interface IEventService
{
    Guid Register<TEvent>(Delegate eventHandler) where TEvent : IEvent;
    void Unregister(Guid eventHandlerId);
    void Handle(IEvent @event);
}
