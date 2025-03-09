using ExtBlazor.Events;

namespace ExtBlazor.Demo.Client.Models;
public record TickEvent(int Tick) : IEvent;