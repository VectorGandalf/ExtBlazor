using Microsoft.AspNetCore.Components;

namespace ExtBlazor.Demo.Client.Services;

public class StashService(IServiceProvider serviceProvider) : IStashService
{
    private Dictionary<string, Dictionary<string, object>> store = new();
    private string? CurrentUri => serviceProvider.GetService<NavigationManager>()?.Uri;
    public virtual void Put(string key, object value)
    {
        if (CurrentUri == null)
        {
            return;
        }
        if (!store.ContainsKey(CurrentUri))
        {
            store.Add(CurrentUri, new() { { key, value } });
        }
        else if (store.ContainsKey(CurrentUri) && !store[CurrentUri].ContainsKey(key))
        {
            store[CurrentUri].Add(key, value);
        }
        else
        {
            store[CurrentUri][key] = value;
        }
    }
    public virtual T? Get<T>(string key)
    {
        if (CurrentUri != null && store.ContainsKey(CurrentUri) && store[CurrentUri].ContainsKey(key))
        {
            var value = store[CurrentUri][key];
            return value is T
                ? (T)value
                : default(T);
        }

        return default(T);
    }

    public virtual void Clear()
    {
        store = new();
    }

    private string? ToAbsoluteUri(string path)
    {
        if (CurrentUri == null)
        {
            return null;
        }
        var host = new Uri(CurrentUri).GetLeftPart(UriPartial.Authority);
        return host + path;
    }
}
