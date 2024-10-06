using Microsoft.Extensions.DependencyInjection;

namespace ExtBlazor.RemoteMediator.Server;

public static class IServiceCollecationExtensions
{
    public static IServiceCollection AddRemoteMediatorServer(this IServiceCollection services, Action<HttpRemoteMediatorServerConfig>? configBuilder)
    {
        var config = new HttpRemoteMediatorServerConfig();
        configBuilder?.Invoke(config);
        services.AddSingleton(config);
        return services;
    }
}
