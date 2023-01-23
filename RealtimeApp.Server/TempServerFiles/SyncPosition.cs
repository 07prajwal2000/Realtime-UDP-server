using RealtimeApp.Shared;

namespace RealtimeApp.Server;

internal partial class TempMultiplayerServer
{

    private readonly Dictionary<string, string> udpClients = new();

    private Task SYNC_POSITION(ReaderPacket readerPacket, string ip)
    {
        if (clients.Count <= 1 )//|| readerPacket.Length != 18)
        {
            return Task.CompletedTask;
        }
        var playerId = readerPacket.ReadString();
        var x = readerPacket.ReadFloat();
        var y = readerPacket.ReadFloat();

        ip = udpClients.FirstOrDefault(x => x.Value == playerId).Key;

        using var packet = new WriterPacket();
        packet.WriteInt((int) MessageType.SYNC_POSITION);
        packet.WriteString(playerId);
        packet.WriteFloat(x);
        packet.WriteFloat(y);

        // var tasks = new Task[clients.Count - 1];
        // var i = 0;
        foreach (var (endpoint, _) in udpClients)
        {
            if (endpoint == ip ) continue;
            sender.Send(packet, endpoint, TransportLayer.UDP);
        }
        return Task.CompletedTask;
        // return Task.WhenAll(tasks);
    }
    
    private Task REGISTER_UDP(ReaderPacket readerPacket, string ip)
    {
        var id = readerPacket.ReadString();
        udpClients.Add(ip, id);
        Console.WriteLine($"UDP client registered. ID: {id}");
        
        using var writer = new WriterPacket(readerPacket);
        return sender.Send(writer, ip, TransportLayer.UDP);
        return Task.CompletedTask;
    }
}