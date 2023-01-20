using Microsoft.Extensions.Caching.Distributed;
using RealtimeApp.Shared;
using System.Text;

namespace RealtimeApp.Server;

internal class EventProcessor
{
    private readonly IDistributedCache cache;
    private Server server;

    public EventProcessor(IDistributedCache cache)
    {
        this.cache = cache;
    }

    public void ProcessEvents(Server server)
    {
        ServerEvents.OnConnected += OnConnected;
        ServerEvents.OnDisconnected += OnDisconnected;
        ServerEvents.OnDataReceived += OnDataReceived;
        this.server = server;
    }

    private Task OnDataReceived(byte[] buffer, string ip, TransportLayer layer)
    {
        return Task.CompletedTask;
    }

    private Task OnDisconnected(string ip)
    {
        return cache.RemoveAsync(KeyBuilder(ip));
    }

    private Task OnConnected(string ip)
    {
        return Task.Run(async () =>
        {
            using var packet = new WriterPacket();
            packet.WriteBool(true);
            packet.WriteObject(DateTime.Now);
            packet.WriteBytes(HashValue.Generate(DateTime.Now.ToShortDateString().ToBytes()));
            await cache.SetAsync(KeyBuilder(ip), ip.ToBytes());
            await Task.Delay(2000);
            return server.Send(packet, ip);
        });
    }

    private string KeyBuilder(string ip)
    {
        return $"USR/{ip}";
    }
}
