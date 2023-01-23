using RealtimeApp.Server.Abstractions;
using RealtimeApp.Shared;

namespace RealtimeApp.Server;

public enum MessageType
{
    SYNC_POSITION = 1,
    NEW_PLAYER_JOIN,
    JOIN_TO_SERVER,
    SUCCESSFULLY_CONNECTED,
    SYNC_EXISTING_STATE,
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

internal class TempMultiplayerServer
{
    private readonly ISender sender;

    private Dictionary<string, string> clients = new();

    private Dictionary<string, PlayerState> serverState = new();

    public TempMultiplayerServer(ISender sender)
	{
        this.sender = sender;
    }

	public Task OnDataReceived(byte[] buffer, string ip, TransportLayer layer)
	{
        try
        {
            if (layer == TransportLayer.UDP)
            {
                ParseUdpData(buffer, ip);
            }
            else
            {
                ParseTcpData(buffer, ip);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return Task.CompletedTask;
	}

    int counter = 0;

    private void ParseUdpData(byte[] buffer, string ip)
    {
        using var packet = new ReaderPacket(buffer);
        var type = (MessageType)packet.ReadInt();
        switch (type)
        {
            case MessageType.SYNC_POSITION:
                break;

            case MessageType.NEW_PLAYER_JOIN:
                break;

            case MessageType.JOIN_TO_SERVER:
                break;

            case MessageType.SUCCESSFULLY_CONNECTED:
                break;

            case MessageType.SYNC_EXISTING_STATE:
                break;
        }
    }

    private void ParseTcpData(byte[] buffer, string ip)
    {
        using var packet = new ReaderPacket(buffer);
        var type = (MessageType)packet.ReadInt();
        switch (type)
        {
            case MessageType.SYNC_POSITION:
                break;

            case MessageType.NEW_PLAYER_JOIN:
                break;

            case MessageType.JOIN_TO_SERVER:
                break;

            case MessageType.SUCCESSFULLY_CONNECTED:
                break;

            case MessageType.SYNC_EXISTING_STATE:
                break;
        }
    }

    public Task OnDisconnected(string ip)
    {
        Console.WriteLine("Client disconnected - " + ip);
        clients.Remove(ip);
        //serverState.Remove(ip);
        return Task.CompletedTask;
    }

    public Task OnConnected(string ip)
    {
        Console.WriteLine("Client connected - " + ip);
        return Task.CompletedTask;
    }

}
