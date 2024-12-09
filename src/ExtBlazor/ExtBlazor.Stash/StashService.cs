namespace ExtBlazor.Stash;

public class StashService(ICurrentUriProvider currentUriProvider) : IStashService
{
    private Dictionary<string, Dictionary<string, object>> store = new();
    private string? CurrentUri => currentUriProvider.Uri;
    public void Put(string key, object value)
    {
        if (CurrentUri == null)
        {
            return;
        }
        else if (!store.ContainsKey(CurrentUri))
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
    public T? Get<T>(string key)
    {
        if (CurrentUri != null && store.ContainsKey(CurrentUri) && store[CurrentUri].ContainsKey(key))
        {
            var value = store[CurrentUri][key];
            return value is T
                ? (T)value
                : default;
        }

        return default;
    }

    public bool HasValue(string key)
    {
        if (CurrentUri != null && store.ContainsKey(CurrentUri) && store[CurrentUri].ContainsKey(key))
        {
            var value = store[CurrentUri][key];
            return value is not null;
        }

        return false;
    }

    public void Clear()
    {
        store = new();
    }
}
