namespace ExtBlazor.Demo.Client.Services;

public interface IStashService
{
    void Clear();
    T? Get<T>(string key);
    bool HasValue(string key);
    void Put(string key, object value);
}