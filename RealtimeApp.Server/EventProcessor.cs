﻿using Microsoft.Extensions.Caching.Distributed;
using RealtimeApp.Server.Abstractions;
using RealtimeApp.Server.Initializers;
using RealtimeApp.Shared;

namespace RealtimeApp.Server;

/// <summary>
/// This is the main entry point for the server where data starts flowing here and all the event registrations happens here.
/// </summary>
internal class EventProcessor
{
    private readonly IDistributedCache cache;
    private IServer sender;
    private readonly IServerRpcCollection rpcCollection;

    public EventProcessor(IDistributedCache cache, IServerRpcCollection rpcCollection)
    {
        this.cache = cache;
        this.rpcCollection = rpcCollection;
    }

    public void ProcessEvents(IServer sender)
    {
        ServerEvents.OnConnected += OnConnected;
        ServerEvents.OnDisconnected += OnDisconnected;
        ServerEvents.OnDataReceived += OnDataReceived;
        this.sender = sender;
    }

    private Task OnDataReceived(byte[] buffer, string ip, TransportLayer layer)
    {
        if (layer == TransportLayer.UDP)
        {
            rpcCollection.Invoke(sender, "TestMethod", ip, buffer);
            return sender.Send(buffer, ip, layer, default);
        }
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
            return sender.Send(packet, ip);
        });
    }

    private string KeyBuilder(string ip)
    {
        return $"USR/{ip}";
    }
}
