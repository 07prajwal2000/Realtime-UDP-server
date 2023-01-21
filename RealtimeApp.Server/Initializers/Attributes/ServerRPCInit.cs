using System.Reflection;

namespace RealtimeApp.Server.Initializers.Attributes;

/// <summary>
/// Use this attribute to reigster the class for RPC from client,
/// add this <see cref="ServerRpc"/> attribute for methods to register methods.
/// Required parameters in constructor is ISender sender => which is used for sending the data to client/clients
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
internal class ServerRPCInit : Attribute
{
	public ServerRPCInit()
	{

	}
}
