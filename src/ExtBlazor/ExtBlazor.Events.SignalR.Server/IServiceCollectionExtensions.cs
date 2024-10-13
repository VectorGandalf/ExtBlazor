namespace ExtBlazor.Events.SignalR.Server;
public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddEventServer(this IServiceCollection services)
    {
        services.AddSignalR();
        return services;
    }
}
