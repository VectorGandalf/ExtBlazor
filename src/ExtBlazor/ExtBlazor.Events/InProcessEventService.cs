using System.Collections.Concurrent;
using System.Reflection;

namespace ExtBlazor.Events;

public class InProcessEventService(IServiceProvider? serviceProvider = null) : IEventService
{
    private ConcurrentDictionary<Guid, EventHandlerRegistration> eventHandlers = [];

    public void Handle(IEvent @event)
    {
        var eventTypes = ExtractTypesImplementingIEvent(@event).ToArray();
        var matchingEventHandlers = GetEventHandlers(eventTypes);

        foreach (var eventHandler in matchingEventHandlers)
        {
            var arguments = ResolveArguments(@event, eventHandler, eventTypes);
            eventHandler.DynamicInvoke(arguments);
        }
    }

    public Guid Register<TEvent>(Delegate eventHandler) where TEvent : IEvent
    {
        var id = Guid.NewGuid();
        var handler = new EventHandlerRegistration(typeof(TEvent), eventHandler);
        eventHandlers.AddOrUpdate(id, handler, (key, value) => handler);
        return id;
    }

    public void Unregister(Guid subscriptionId)
    {
        eventHandlers.Remove(subscriptionId, out var _);
    }

    private IEnumerable<Delegate> GetEventHandlers(Type[] eventTypes)
        => eventHandlers.Values
        .Where(subscription => eventTypes.Contains(subscription.EventType))
        .Select(handler => handler.EventHandler);

    private object[] ResolveArguments(IEvent @event, Delegate eventHandler, Type[] eventTypes)
    {
        List<object> arguments = [];
        var methodInfo = eventHandler.GetMethodInfo();
        var parameters = methodInfo.GetParameters();

        foreach (var parameterType in parameters.Select(p => p.ParameterType))
        {
            if (eventTypes.Contains(parameterType))
            {
                arguments.Add(@event);
            }
            else
            {
                if (serviceProvider != null)
                {
                    var argument = serviceProvider.GetService(parameterType);
                    arguments.Add(argument ?? new ArgumentNullException(GetType().Name + " Could not resolve dependency " + parameterType.Name));
                }
                else
                {
                    throw new ArgumentNullException(GetType().Name + " serviceProvider null constructor argument!");
                }
            }
        }

        return arguments.ToArray();
    }

    private static IEnumerable<Type> ExtractTypesImplementingIEvent(IEvent @event)
        => GetInterfaces(@event)
        .Concat([@event.GetType()])
        .Concat(GetBaseTypes(@event));

    private static IEnumerable<Type> GetInterfaces(IEvent @event)
    {
        return @event.GetType()
                .GetInterfaces()
                .Where(type => type.GetInterfaces().Contains(typeof(IEvent)) || type.Equals(typeof(IEvent)));
    }

    private static IEnumerable<Type> GetBaseTypes(IEvent @event)
    {
        return GetAllBaseTypes(@event.GetType())
            .Where(type => type.GetInterfaces().Contains(typeof(IEvent)));
    }

    private static IEnumerable<Type> GetAllBaseTypes(Type type)
    {
        List<Type> baseTypes = [];
        Type? baseType = type.BaseType;
        while (baseType != null)
        {
            baseTypes.Add(baseType);
            baseType = baseType.BaseType;
        }

        return baseTypes;
    }
}