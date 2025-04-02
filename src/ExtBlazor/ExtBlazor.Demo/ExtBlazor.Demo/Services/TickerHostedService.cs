using ExtBlazor.Demo.Client.Models;
using ExtBlazor.Events.SignalR.Server;

namespace ExtBlazor.Demo.Services
{
    public class TickerHostedService(IEventPublisher eventBus) : IHostedService, IDisposable
    {
        private int tick = 0;
        private Timer? timer = null;
        public Task StartAsync(CancellationToken stoppingToken)
        {
            timer = new Timer(async _ => await DoWork(stoppingToken), null, TimeSpan.Zero, TimeSpan.FromMilliseconds(50));
            return Task.CompletedTask;
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            Interlocked.Increment(ref tick);
            await eventBus.Publish(new TickEvent(tick), stoppingToken);
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
