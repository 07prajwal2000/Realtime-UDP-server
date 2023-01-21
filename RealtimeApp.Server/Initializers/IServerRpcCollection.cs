using RealtimeApp.Server.Abstractions;

namespace RealtimeApp.Server.Initializers
{
    public interface IServerRpcCollection
    {
        Task Invoke(ISender sender, string name, string ip, byte[] data);
    }
}