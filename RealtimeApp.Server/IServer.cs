namespace RealtimeApp.Server;

internal interface IServer
{
    Task Send(byte[] data, string endpoint, TransportLayer layer = TransportLayer.TCP, CancellationToken token = default);
    void Start();
}