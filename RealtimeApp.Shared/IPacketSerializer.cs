namespace RealtimeApp.Shared;

public interface IPacketSerializer
{
    byte[] Serialize<T>(T value);
    T? Deserialize<T>(byte[] buffer);
}