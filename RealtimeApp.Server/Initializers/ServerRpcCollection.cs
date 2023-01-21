namespace RealtimeApp.Server.Initializers;

public class ServerRpcCollection : IServerRpcCollection
{
    private readonly Dictionary<string, MethodMetadata> registeredFunctions;

    public ServerRpcCollection(Dictionary<string, MethodMetadata> registeredFunctions)
	{
        this.registeredFunctions = registeredFunctions;
    }

    public Task Invoke(string name, string ip, byte[] data)
    {
        if(!registeredFunctions.TryGetValue(name, out var method))
        {
            return Task.CompletedTask;
        }
        return Task.Run(() => method.Invoke(ip, data));
    }

}
