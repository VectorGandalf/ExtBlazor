using System.Reflection;
using System.Text.Json;

namespace ExtRemoting.Types;

public class Envelope(object obj) : IEnvelope
{
    public string AssemblyName { get; } = obj.GetType().Assembly.FullName!;
    public string TypeName { get; } = obj.GetType().FullName!;
    public string Payload { get; } = JsonSerializer.Serialize(obj);

    public object ToObject()
    {
        var assembly = Assembly.Load(AssemblyName);
        var type = assembly!.GetType(TypeName)!;
        return JsonSerializer.Deserialize(Payload, type, JsonSerializerOptions.Default)!;
    }
}
