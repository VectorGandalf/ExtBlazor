using ExtBlazor.Events;
using Microsoft.Extensions.DependencyInjection;

namespace ExtBlazor.Tests.Units.Events;

public class InProcessEventServiceTests
{
    [Fact]
    public void Can_Register_Event_Handler()
    {
        //Arrange
        IEventService eventService = new InProcessEventService();

        //Act
        var handlerId = eventService.Subscribe<IEvent>(() => { });

        //Assert
        Assert.NotEqual(Guid.Empty, handlerId);
    }

    [Fact]
    public void Can_Handle_Event()
    {
        //Arrange
        var effect = 1;

        IEventService eventService = new InProcessEventService();

        _ = eventService.Subscribe<CustomEvent>(() => effect = 2);

        //Act
        eventService.Handle(new CustomEvent());

        //Assert
        Assert.Equivalent(2, effect);
    }

    [Fact]
    public void Can_Handle_Event_Async()
    {
        //Arrange
        var effect = 1;
        IEventService eventService = new InProcessEventService();

        _ = eventService.Subscribe<CustomEvent>(async () => { effect = 2; await Task.CompletedTask; });

        //Act
        eventService.Handle(new CustomEvent());

        //Assert
        Assert.Equivalent(2, effect);
    }

    [Fact]
    public void Can_Handle_Event_Parameter()
    {
        //Arrange
        int effect = 0;

        IEventService eventService = new InProcessEventService();

        _ = eventService.Subscribe<CustomEvent2>((CustomEvent2 customEvent) => effect = customEvent.Id);

        //Act
        eventService.Handle(new CustomEvent2(2));

        //Assert
        Assert.Equivalent(2, effect);
    }

    [Fact]
    public void Can_Execute_Handler_With_Injected_Parameter()
    {
        //Arrange
        IServiceProvider serviceProvider = new ServiceCollection()
                .AddSingleton<TestService>()
                .BuildServiceProvider();

        int effect = 0;

        IEventService eventService = new InProcessEventService(new TestServiceScopeFactory(serviceProvider));

        _ = eventService.Subscribe<CustomEvent2>((CustomEvent2 customEvent, TestService testService) => effect = testService.Value);

        //Act
        eventService.Handle(new CustomEvent2(2));

        //Assert
        Assert.Equivalent(4, effect);
    }

    [Fact]
    public void Can_Execute_Handler_With_2_Injected_Parameters()
    {
        //Arrange
        var serviceProvider = new ServiceCollection()
            .AddTransient<TestService2>()
            .AddTransient<TestService>()
            .BuildServiceProvider();

        int effect1 = 0;
        int effect2 = 0;

        IEventService eventService = new InProcessEventService(new TestServiceScopeFactory(serviceProvider));

        _ = eventService.Subscribe<CustomEvent>((TestService testService, TestService2 testService2) =>
        {
            effect1 = testService.Value;
            effect2 = testService2.GetHiddenValue();
        });

        //Act
        eventService.Handle(new CustomEvent());

        //Assert
        Assert.Equivalent(4, effect1);
        Assert.Equivalent(5, effect2);
    }

    [Fact]
    public void Can_Handle_Event_On_Interface()
    {
        //Arrange
        int effect = 0;
        IEventService eventService = new InProcessEventService();

        _ = eventService.Subscribe<IEvent>(() => effect = 99);

        //Act
        eventService.Handle(new CustomEvent());

        //Assert
        Assert.Equivalent(99, effect);
    }

    [Fact]
    public void Can_Handle_Event_On_Interface2()
    {
        //Arrange
        int effect = 0;

        IEventService eventService = new InProcessEventService();

        _ = eventService.Subscribe<ICustomEvent>(() => effect = 98);

        //Act
        eventService.Handle(new CustomEvent());

        //Assert
        Assert.Equivalent(98, effect);
    }

    [Fact]
    public void Can_Handle_Event_On_Base_Class()
    {
        //Arrange
        int effect = 0;

        IEventService eventService = new InProcessEventService();

        _ = eventService.Subscribe<SuperEvent>(() => effect = 94);

        //Act
        eventService.Handle(new CustomEvent());

        //Assert
        Assert.Equivalent(94, effect);
    }

    [Fact]
    public void Can_Unregister_Event_Handler()
    {
        //Arrange
        int effect = 0;

        IEventService eventService = new InProcessEventService();

        var handlerId = eventService.Subscribe<CustomEvent>(() => effect = 100);

        //Act
        eventService.Unsubscribe(handlerId);
        eventService.Handle(new CustomEvent());

        //Assert
        Assert.Equivalent(0, effect);
    }

    [Fact]
    public void Can_Handle_Event_2_Handlers_For_The_Same_Event_Type()
    {
        //Arrange
        int effect1 = 0;
        int effect2 = 0;

        IEventService eventService = new InProcessEventService();

        _ = eventService.Subscribe<CustomEvent>(() => effect1 = 4);
        _ = eventService.Subscribe<CustomEvent>(() => effect2 = 5);

        //Act
        eventService.Handle(new CustomEvent());

        //Assert
        Assert.Equivalent(4, effect1);
        Assert.Equivalent(5, effect2);
    }
}

public record CustomEvent : SuperEvent;
public record SuperEvent : ICustomEvent;
public interface ICustomEvent : IEvent;

public record CustomEvent2(int Id) : IEvent;

public class TestService
{
    public int Value { get; } = 4;
}

public class TestService2
{
    private int hiddenValue = 5;
    public int GetHiddenValue() => hiddenValue;
}

public class TestServiceScopeFactory(IServiceProvider provider) : IServiceScopeFactory
{
    public IServiceScope CreateScope() => provider.CreateScope();
}