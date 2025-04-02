namespace ExtBlazor.Events.SignalR.Server;
public class EventHandlerHostedService(
    IEventService eventService,
    IEventPublisher eventPublisher,
    ILogger<EventHandlerHostedService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (eventPublisher is ChannelEventPublisher channelEventPublisher)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var @event = channelEventPublisher.Read(stoppingToken);
                    eventService.Handle(await @event);
                }
                catch (TaskCanceledException)
                {
                    logger.LogInformation("Canceled");
                    break;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, ex.Message);
                }
            }
        }
    }
}