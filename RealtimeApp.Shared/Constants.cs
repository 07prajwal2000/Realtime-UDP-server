using System.Net;

namespace RealtimeApp.Shared;

public static class Constants
{
    public static readonly IPEndPoint ServerEndPoint = new(IPAddress.IPv6Any, Random.Shared.Next(10000, 30000));
}