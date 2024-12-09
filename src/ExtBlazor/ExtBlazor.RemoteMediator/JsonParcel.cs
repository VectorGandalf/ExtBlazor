using System.Text.Json.Serialization;

namespace ExtBlazor.RemoteMediator;
public record JsonParcel
{
    public string Assembly { get; private set; }
    public string Type { get; private set; }
    public string Data { get; private set; }

    public JsonParcel(object data)
    {
        Assembly = data.GetType().Assembly.GetName().FullName;
        Type = data.GetType().FullName ?? throw new Exception("Type must have full name");
        Data = System.Text.Json.JsonSerializer.Serialize(data);
    }

    [JsonConstructor]
    public JsonParcel(string assembly, string type, string data)
    {
        Assembly = assembly;
        Type = type;
        Data = data;
    }

    public object ToObject()
    {
        var assembly = System.Reflection.Assembly.Load(Assembly);
        var type = assembly?.GetType(Type) ?? throw new NullReferenceException();
        return System.Text.Json.JsonSerializer.Deserialize(Data, type) ?? new NullReferenceException();
    }
}