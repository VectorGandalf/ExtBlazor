using System.Threading.Channels;
using Microsoft.AspNetCore.SignalR;

namespace ExtBlazor.Events.SignalR.Server;
public static partial class IServiceCollectionExtensions
{
    public static ISignalRServerBuilder AddSignalREventService(this IServiceCollection services,
        EventServiceOptions options)
    {
        services.AddHostedService<EventHandlerHostedService>();

        services.AddSingleton(new EventHubPathConfiguration
        {
            Path = options.SignalRHubPath
        });

        var channelOptions = options.ChannelOptions ?? new BoundedChannelOptions(10)
        {
            AllowSynchronousContinuations = false,
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = true,
            SingleWriter = false
        };

        services.AddSingleton<IEventPublisher, ChannelEventPublisher>(c => new ChannelEventPublisher(channelOptions));
        services.AddSingleton<IEventService, InProcessEventService>();
        services.AddSingleton(new EventRouterConfiguration
        {
            EventRouter = options.EventRouter
        });

        return services.AddSignalR();
    }
}