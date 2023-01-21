namespace RealtimeApp.Server.Abstractions;

public interface IServer : ISender
{
    void Start();
    Task Send(byte[] data, string endpoint, TransportLayer layer = TransportLayer.TCP, CancellationToken token = default);
}

public interface ISender
{
    Task Send(byte[] data, string endpoint, TransportLayer layer = TransportLayer.TCP, CancellationToken token = default);
}
