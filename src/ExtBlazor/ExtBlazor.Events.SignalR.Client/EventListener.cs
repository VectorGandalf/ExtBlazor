using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace ExtBlazor.Events.SignalR.Client;

public class EventListener() :
    ComponentBase,
    IAsyncDisposable
{
    [Inject]
    public required NavigationManager Navigation { get; set; }

    [Inject]
    public required IEventService EventService { get; set; }

    [Inject]
    public required EventHubPathConfiguration Configuration { get; set; }

    [Inject]
    public required EventsHubConnectionBuilder HubConnectionBuilder { get; set; }

    private HubConnection? hubConnection;

    protected override async Task OnInitializedAsync()
    {
        if (OperatingSystem.IsBrowser())
        {
            var uri = Navigation.ToAbsoluteUri(Configuration.Path);
            hubConnection = HubConnectionBuilder.Builder(uri)
                .Build();

            hubConnection.On<string>("send_event", HandleEvent);
            await hubConnection.StartAsync();
        }
    }

    private void HandleEvent(string eventMessage)
    {
        var parcel = JsonSerializer.Deserialize<JsonParcel>(eventMessage);
        if (parcel is null)
        {
            return;
        }

        if (parcel.ToObject() is IEvent @event)
        {
            EventService.Handle((@event));
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (hubConnection is null)
        {
            return;
        }

        await hubConnection.StopAsync();
    }
}
