using Microsoft.Extensions.DependencyInjection;

namespace ExtBlazor.RemoteMediator.Client;
public static class IServiceCollecationExtensions
{
    public static IServiceCollection AddRemoteMediatorClient(this IServiceCollection services,
        Action<HttpRemoteMediatorConfig>? configBuilder = default)
    {
        var config = new HttpRemoteMediatorConfig();
        configBuilder?.Invoke(config);

        services
            .AddSingleton(config)
            .AddScoped<IRemoteMediator, HttpRemoteMediator>();

        return services;
    }
}
