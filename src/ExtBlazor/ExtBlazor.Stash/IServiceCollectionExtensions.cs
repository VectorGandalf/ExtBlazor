using Microsoft.Extensions.DependencyInjection;

namespace ExtBlazor.Stash;
public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddStashService(this IServiceCollection services)
    {
        return services
            .AddScoped<ICurrentUriProvider, NavigationCurrentUriProvider>()
            .AddScoped<IStashService, StashService>();
    }
}
