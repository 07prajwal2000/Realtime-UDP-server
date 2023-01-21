using RealtimeApp.Server.Abstractions;
using RealtimeApp.Server.Initializers.Attributes;

namespace RealtimeApp.Server.Rpc;

[ServerRPCInit]
public sealed class TestRpc
{
    private readonly IDataSender dataSender;

    public TestRpc(IDataSender dataSender)
	{
        this.dataSender = dataSender;
    }

    [ServerRpc]
    public Task TestMethod(string ip, byte[] data)
    {
        return Task.CompletedTask;
    }
}
