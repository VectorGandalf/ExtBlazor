namespace ExtBlazor.Stash;

public interface IStashService
{
    /// <summary>
    /// Roemoves all objects from stash for all Uri:s and keys.
    /// </summary>
    void Clear();

    /// <summary>
    /// Gets an object from the stash associated with the current Uri
    /// </summary>
    /// <typeparam name="T">Type of the object</typeparam>
    /// <param name="key">Key used to identify the object in stash for the current Uri</param>
    /// <returns>Stashed object</returns>
    T? Get<T>(string key);

    /// <summary>
    /// Check if an object has a value for the key and currrent Uri
    /// </summary>
    /// <param name="key">Key used to identify the object in stash for the current Uri</param>
    /// <returns>True if the object exists in stash</returns>
    bool HasValue(string key);

    /// <summary>
    /// Puts an object in the stash associated with the current Uri
    /// </summary>
    /// <param name="key">Key used to identify the object in stash for the current Uri</param>
    /// <param name="value">The object to stash</param>
    void Put(string key, object value);
}