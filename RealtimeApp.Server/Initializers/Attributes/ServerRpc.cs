using RealtimeApp.Server.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtimeApp.Server.Initializers.Attributes;

/// <summary>
/// Decorate the method with own name or it uses method name. Used to call a server function when data received.
/// Parameters for method is required - <see cref="ISender">Sender</see> , <see cref="string">Client IP</see> , <see cref="byte">byte[] Data</see>
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class ServerRpc : Attribute
{
    public readonly string Name = null!;

    public ServerRpc(string name)
    {
        Name = name;
    }

    public ServerRpc()
	{

	}
}
