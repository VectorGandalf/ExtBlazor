using System.Threading;
using ExtBlazor.Demo.Client.Models;
using ExtBlazor.Events;

namespace ExtBlazor.Demo.Services
{
    public class TickerHostedService(IEventService eventService) : IHostedService, IDisposable
    {
        private int tick = 0;
        private Timer? _timer = null;

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(0.5));

            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            Interlocked.Increment(ref tick);
            eventService.Handle(new TickEvent(tick));            
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
