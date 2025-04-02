using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace ExtBlazor.Events;

public class InProcessEventService(IServiceScopeFactory? serviceScopeFactory = null) : IEventService
{
    private ConcurrentDictionary<Guid, EventHandlerRegistration> eventHandlers = [];

    public void Handle(IEvent @event)
    {
        var eventTypes = ExtractTypesImplementingIEvent(@event).ToArray();
        var matchingEventHandlers = GetEventHandlers(eventTypes);

        foreach (var eventHandler in matchingEventHandlers)
        {
            using var serviceScope = serviceScopeFactory?.CreateScope();

            var arguments = ResolveArguments(@event, eventHandler, eventTypes, serviceScope?.ServiceProvider);
            eventHandler.DynamicInvoke(arguments);
        }
    }

    public Guid Subscribe<TEvent>(Delegate eventHandler) where TEvent : IEvent
    {
        var id = Guid.NewGuid();
        var handler = new EventHandlerRegistration(typeof(TEvent), eventHandler);
        eventHandlers.AddOrUpdate(id, handler, (key, value) => handler);
        return id;
    }

    public void Unsubscribe(Guid subscriptionId)
    {
        eventHandlers.Remove(subscriptionId, out var _);
    }

    private IEnumerable<Delegate> GetEventHandlers(Type[] eventTypes)
        => eventHandlers.Values
        .Where(subscription => eventTypes.Contains(subscription.EventType))
        .Select(handler => handler.EventHandler);

    private object[] ResolveArguments(IEvent @event, Delegate eventHandler, Type[] eventTypes, IServiceProvider? serviceProvider)
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
                var argument = ResolveSeviceArgument(parameterType, serviceProvider);
                arguments.Add(argument);
            }
        }

        return arguments.ToArray();
    }

    private object ResolveSeviceArgument(Type parameterType, IServiceProvider? serviceProvider
        )
    {
        if (serviceProvider is null)
        {
            throw new ArgumentNullException("Could not resolve " + GetType().Name + ". serviceProvider argument is null!");
        }

        var argument = serviceProvider.GetService(parameterType);
        if (argument is null)
        {
            throw new ArgumentNullException(GetType().Name + " Could not resolve dependency " + parameterType.Name);
        }

        return argument;
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