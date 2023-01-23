using RealtimeApp.Server.Abstractions;
using System.Reflection;

namespace RealtimeApp.Server.Initializers;

public class MethodMetadata
{
    public readonly string MethodName;
    private readonly MethodInfo Info;
    private readonly object Owner;

    public MethodMetadata(string methodName, MethodInfo info, object owner)
    {
        MethodName = methodName;
        Info = info;
        Owner = owner;
    }

    public void Invoke(ISender sender, string ip, byte[] data)
    {
        Info.Invoke(Owner, new object[] { sender, ip, data });
    }
}