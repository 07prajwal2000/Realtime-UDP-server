namespace RealtimeApp.Server.Initializers
{
    public interface IServerRpcCollection
    {
        Task Invoke(string name, string ip, byte[] data);
    }
}