using System;
using RealtimeApp.Server.Abstractions;
using RealtimeApp.Server.Initializers.Attributes;

namespace RealtimeApp.Server.Rpc;

[ServerRPCInit]
public sealed class TestRpc
{
    [ServerRpc]
    public Task TestMethod(ISender sender, string ip, byte[] data)
    {
        Console.WriteLine("Test rpc working");
        return Task.CompletedTask;
    }
}
