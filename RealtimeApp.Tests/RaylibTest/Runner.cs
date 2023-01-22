using Raylib_CsLo;
using RealtimeApp.Shared.Serializers;
using RealtimeApp.Shared;
using System.Numerics;
using System.Net;

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

internal class Runner
{
    static string myId = Guid.NewGuid().ToString().Substring(0, 6);
    static List<Entity> entities = new List<Entity>();

    public static async Task Run()
    {
        BasePacket.InitializeSerializer(new JsonPacketSerializer());

        var windowName = "Demo";
        using var demoClient = new Client();

        {
            entities.Add(new Entity("Cadc", Raylib.GREEN, true));
        }

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
        
    }

    static void OnDataReceived(byte[] data, string ip, TransportLayer tl)
    {
        
    }

}
