using System.Text;
using System.Text.Json;

namespace RealtimeApp.Shared.Serializers;

public class JsonPacketSerializer : IPacketSerializer
{
    public byte[] Serialize<T>(T value)
    {
        return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value));
    }

    public T? Deserialize<T>(byte[] buffer)
    {
        var str = Encoding.UTF8.GetString(buffer);
        return JsonSerializer.Deserialize<T>(str);
    }
}