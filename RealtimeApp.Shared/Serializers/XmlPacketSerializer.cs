using System.Xml.Serialization;

namespace RealtimeApp.Shared.Serializers;

public class XmlPacketSerializer : IPacketSerializer
{
    public byte[] Serialize<T>(T value)
    {
        using var ms = new MemoryStream();
        var xml = new XmlSerializer(typeof(T));
        xml.Serialize(ms, value);
        return ms.ToArray();
    }

    public T? Deserialize<T>(byte[] buffer)
    {
        using var ms = new MemoryStream(buffer);
        var xml = new XmlSerializer(typeof(T));
        return (T)xml.Deserialize(ms)!;
    }
}
