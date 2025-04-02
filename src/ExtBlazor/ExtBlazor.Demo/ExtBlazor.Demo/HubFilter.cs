using Microsoft.AspNetCore.SignalR;

namespace ExtBlazor.Demo;
public class HubFilter : IHubFilter
{
    public async ValueTask<object?> InvokeMethodAsync(
        HubInvocationContext invocationContext,
        Func<HubInvocationContext, ValueTask<object?>> next)
    {
        return await next(invocationContext);
    }

    public Task OnConnectedAsync(HubLifetimeContext context, Func<HubLifetimeContext, Task> next)
    {
        context.Hub.Groups.AddToGroupAsync(context.Context.ConnectionId, "TickReceiver");
        return next(context);
    }

    public Task OnDisconnectedAsync(
        HubLifetimeContext context,
        Exception? exception,
        Func<HubLifetimeContext, Exception?, Task> next)
    {
        return next(context, exception);
    }
}