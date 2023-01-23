using RealtimeApp.Server.Abstractions;
using RealtimeApp.Shared;

namespace RealtimeApp.Server;

public enum MessageType
{
    SYNC_POSITION = 1,
    JOIN_TO_SERVER,
    SUCCESSFULLY_CONNECTED,
    SYNC_EXISTING_STATE,
    NEW_PLAYER_JOINED,
    REGISTER_UDP,
}

public struct PlayerState
{
    public struct Color
    {
        public int r { get; set; }
        public int g { get; set; }
        public int b { get; set; }
    }

    public string playerId { get; set; }
    public float x { get; set; }
    public float y { get; set; }
    public Color color { get; set; }
}

internal partial class TempMultiplayerServer
{
    private readonly ISender sender;

    /// <summary>
    /// (key= Client IP, value= client id) 
    /// </summary>
    private Dictionary<string, string> clients = new();

    /// <summary>
    /// (key= Client IP, value= player state) 
    /// </summary>
    private Dictionary<string, PlayerState> serverState = new();

    public TempMultiplayerServer(ISender sender)
	{
        this.sender = sender;
    }

	public Task OnDataReceived(byte[] buffer, string ip, TransportLayer layer)
    {
        Task task = null;
        try
        {
            if (layer == TransportLayer.UDP)
            {
                task = ParseUdpData(buffer, ip);
            }
            else
            {
                task = ParseTcpData(buffer, ip);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return task ?? Task.CompletedTask;
	}

    private Task ParseUdpData(byte[] buffer, string ip)
    {
        using var packet = new ReaderPacket(buffer);
        var type = (MessageType)packet.ReadInt();
        switch (type)
        {
            case MessageType.SYNC_POSITION:
                return SYNC_POSITION(packet, ip);
            case MessageType.REGISTER_UDP:
                return REGISTER_UDP(packet, ip);
            default:
                return Task.CompletedTask;
        }
    }

    private Task ParseTcpData(byte[] buffer, string ip)
    {
        using var packet = new ReaderPacket(buffer);
        var type = (MessageType)packet.ReadInt();
        switch (type)
        {
            case MessageType.JOIN_TO_SERVER:
                return JOIN_TO_SERVER(packet, ip);

            default:
                return Task.CompletedTask;
        }
    }

    public Task OnDisconnected(string ip)
    {
        Console.WriteLine("Client disconnected - " + ip);
        
        if (clients.TryGetValue(ip, out var id) && clients.Remove(ip))
        {
            // var udpIp = udpClients.FirstOrDefault(x => x.Value == id).Key;
            // udpClients.Remove(udpIp);
            serverState.Remove(ip);
        }
        
        return Task.CompletedTask;
    }

    public Task OnConnected(string ip)
    {
        Console.WriteLine("Client connected - " + ip);
        return Task.CompletedTask;
    }

}
