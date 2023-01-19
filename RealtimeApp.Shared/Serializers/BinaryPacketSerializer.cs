using System.Runtime.Serialization.Formatters.Binary;

namespace RealtimeApp.Shared.Serializers;

/// <summary>
/// When using this serializer, make sure the class has <see cref="System.Serializable"/> attribute decorated
/// </summary>
[Obsolete(".Net runtime has made to obsolete")]
public class BinaryPacketSerializer : IPacketSerializer
{
    private readonly BinaryFormatter formatter;

    public BinaryPacketSerializer()
    {
        formatter = new BinaryFormatter();
    }
    
    public byte[] Serialize<T>(T value)
    {
        using var ms = new MemoryStream();
        formatter.Serialize(ms, value);
        return ms.ToArray();
    }

    public T? Deserialize<T>(byte[] buffer)
    {
        using var ms = new MemoryStream(buffer);
        return (T)formatter.Deserialize(ms);
    }
}