using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace ExtBlazor.Stash;

public interface ICurrentUriProvider
{
    string? Uri { get; }
}
public class NavigationCurrentUriProvider(IServiceProvider services) : ICurrentUriProvider
{
    public string? Uri => services.GetService<NavigationManager>()?.Uri;
}

