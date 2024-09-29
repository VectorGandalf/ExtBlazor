namespace ExtBlazor.RemoteDispatcher;
public record Envelope
{
    public string Assembly { get; private set; }
    public string Type { get; private set; }
    public string Data { get; private set; }

    public Envelope(object data)
    {
        Assembly = data.GetType().Assembly.GetName().FullName;
        Type = data.GetType().Name;
        Data = System.Text.Json.JsonSerializer.Serialize(Data);
    }

    public object ToObject()
    {
        var assembly = System.Reflection.Assembly.Load(Assembly);
        var type = assembly?.GetType(Type) ?? throw new NullReferenceException();
        return System.Text.Json.JsonSerializer.Deserialize(Data, type) ?? new NullReferenceException();
    }
}