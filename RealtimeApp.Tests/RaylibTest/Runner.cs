using Raylib_CsLo;
using RealtimeApp.Shared.Serializers;
using RealtimeApp.Shared;
using System.Numerics;
using System.Net;
using Color = Raylib_CsLo.Color;

namespace RealtimeApp.Tests.RaylibTest;


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

public enum TransportLayer
{
    UDP,
    TCP,
}
// (ID)6 + (msg type header) 4 + (x axis) 4 + (y axis) 4
internal class Runner
{
    static string myId = Guid.NewGuid().ToString().Substring(0, 6);
    static List<Entity> entities = new();

    public static async Task Run()
    {
        BasePacket.InitializeSerializer(new JsonPacketSerializer());

        Console.ForegroundColor = ConsoleColor.Red;
        Console.BackgroundColor = ConsoleColor.Cyan;
        Console.WriteLine("MY ID: " + myId);
        Console.ResetColor();

        var windowName = "Demo " + myId;
        using var demoClient = new Client();

        Client.OnReceivedEvent += OnDataReceived;
        Client.OnConnected += OnConnectedToServer;

        Raylib.InitWindow(900, 480, windowName);
        Raylib.SetWindowState(ConfigFlags.FLAG_WINDOW_RESIZABLE);
        Raylib.SetTargetFPS(60);

        demoClient.Start();
        // Main game loop
        while (!Raylib.WindowShouldClose()) // Detect window close button or ESC key
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.SKYBLUE);
            Raylib.DrawFPS(10, 10);

            foreach (var entity in entities)
            {
                entity.Update();
            }
            Raylib.EndDrawing();
        }
        Raylib.CloseWindow();
    }

    static void OnConnectedToServer(string ip)
    {
        using var packet = new WriterPacket();
        packet.WriteInt((int) MessageType.JOIN_TO_SERVER);
        packet.WriteString(myId);
        Client.Send(packet, TransportLayer.TCP);
    }

    static void OnDataReceived(byte[] data, string ip, TransportLayer tl)
    {
        if (tl == TransportLayer.TCP)
        {
            HandleTcpData(data);
            return;
        }

        HandleUdpData(data);
    }

    private static void HandleUdpData(byte[] data)
    {
        using var packet = new ReaderPacket(data);
        var type = (MessageType)packet.ReadInt();
        switch (type)
        {
            case MessageType.SYNC_POSITION:
                break;
            case MessageType.REGISTER_UDP:
                var msg = packet.ReadString();
                Console.WriteLine("UDP registered. msg: " + msg);
                break;
        }
    }

    private static void HandleTcpData(byte[] data)
    {
        using var packet = new ReaderPacket(data);
        var type = (MessageType)packet.ReadInt();
        switch (type)
        {
            case MessageType.SUCCESSFULLY_CONNECTED:
                SuccessfullyConnected(packet);
                break;

            case MessageType.SYNC_EXISTING_STATE:
                LoadExistingState(packet);
                break;
            
            case MessageType.NEW_PLAYER_JOINED:
                NewPlayerJoined(packet);
                break;
        }
    }

    private static void NewPlayerJoined(ReaderPacket packet)
    {
        var playerId = packet.ReadString();
        var r = packet.ReadInt();
        var g = packet.ReadInt();
        var b = packet.ReadInt();

        var newPlayer = new Entity(playerId, new Color(r, g, b, 255), false);
        
        entities.Add(newPlayer);
    }

    private static void LoadExistingState(ReaderPacket packet)
    {
        var states = packet.ReadObject<PlayerState[]>();
        foreach (var state in states!)
        {
            entities.Add(new Entity(state.playerId, 
                new Color
                {
                    r = (byte)state.color.r,
                    g = (byte)state.color.g,
                    b = (byte)state.color.b,
                },
                false, new Vector3(state.x, state.y, 0)));
        }

        Console.WriteLine("Existing state synced.\n\tTotal players: " + states.Length + "\n");
    }

    private static void SuccessfullyConnected(ReaderPacket packet)
    {
        var r = packet.ReadInt();
        var g = packet.ReadInt();
        var b = packet.ReadInt();
        
        entities.Add(new Entity(myId, new Color(r, g, b, 255), true));
        Console.WriteLine($"Successfully connected to server. \n\tcolor: <{r},{g},{b}>\n");

        using var p = new WriterPacket();
        p.WriteInt((int) MessageType.REGISTER_UDP);
        p.WriteString(myId);
        Client.Send(p, TransportLayer.UDP);
    }
}
