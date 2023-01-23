using RealtimeApp.Shared;

namespace RealtimeApp.Server;

internal partial class TempMultiplayerServer
{
    /// <summary>
    /// when client requests to join the server
    /// </summary>
    private Task JOIN_TO_SERVER(ReaderPacket readerPacket, string ip)
    {
        var playerId = readerPacket.ReadString();
        clients.Add(ip, playerId);

        var entity = new PlayerState
        {
            color = new PlayerState.Color
            {
                r = Random.Shared.Next(1, 255),
                g = Random.Shared.Next(1, 255),
                b = Random.Shared.Next(1, 255)
            },
            playerId = playerId
        };
        
        serverState.Add(ip, entity);

        var tasks = new Task[3];
        
        // TODO: send player details. i,e color pos, id
        return SendPlayerDetails(ip, entity);
        //
        // // TODO: sync existing state.
        // tasks[1] =SendExistingStateToClient(ip);
        //
        // // TODO: broadcast to connected players that new player has joined.
        // tasks[2] = BroadcastNewPlayerJoined(entity);

        return Task.WhenAll(tasks);
    }

    private Task BroadcastNewPlayerJoined(string ip)
    {
        var entity = serverState[ip];
        
        using var packet = new WriterPacket();
        packet.WriteInt((int) MessageType.NEW_PLAYER_JOINED);
        packet.WriteString(entity.playerId);
        packet.WriteInt(entity.color.r);
        packet.WriteInt(entity.color.g);
        packet.WriteInt(entity.color.b);
        
        var tasks = new Task[clients.Count - 1];
        var i = 0;
        foreach (var (endpoint, id) in clients)
        {
            if (entity.playerId == id)  continue;
            tasks[i] = sender.Send(packet, endpoint);
            i++;
        }

        Console.WriteLine("Broadcasted to " + (clients.Count - 1));

        return Task.WhenAll(tasks);
    }

    private Task SendExistingStateToClient(string ip)
    {
        using var packet = new WriterPacket();

        packet.WriteInt((int) MessageType.SYNC_EXISTING_STATE);
        
        var state = serverState
            .Where(x => x.Key != ip)
            .Select(x => x.Value)
            .ToArray();
        packet.WriteObject(state);

        Console.WriteLine($"sent state to {ip} client");
        
        sender.Send(packet, ip);
        return BroadcastNewPlayerJoined(ip);
    }

    private Task SendPlayerDetails(string ip, PlayerState entity)
    {
        using var packet = new WriterPacket();
        packet.WriteInt((int)MessageType.SUCCESSFULLY_CONNECTED);
        packet.WriteInt(entity.color.r);
        packet.WriteInt(entity.color.g);
        packet.WriteInt(entity.color.b);
        sender.Send(packet, ip);
        return SendExistingStateToClient(ip);
    }
}