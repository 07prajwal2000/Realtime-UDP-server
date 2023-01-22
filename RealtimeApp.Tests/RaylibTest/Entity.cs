using Raylib_CsLo;
using RealtimeApp.Shared;
using System.Numerics;

namespace RealtimeApp.Tests.RaylibTest;

public enum MessageType
{
    SYNC_POSITION = 1,
    NEW_PLAYER_JOIN,
    JOIN_TO_SERVER,
    SUCCESSFULLY_CONNECTED,
    SYNC_EXISTING_STATE,
}

internal class Entity
{
    private readonly Interval timer;
    private Vector3 pos;

    private string playerID;
    public bool owner = false;
    private readonly Color myColor;

    public Entity(string entityId, Color color, bool owner = false, Vector3 currentPos = new Vector3())
    {
        timer = new Interval(.20f);
        playerID = entityId;
        myColor = color;
        pos = currentPos;
        this.owner = owner;
        RegisterEvents();
    }

    private float frameTime;
    private float moveSpeed = 220;

    private Vector3 networkPos;

    private void RegisterEvents()
    {
        Client.OnReceivedEvent += OnDataReceived;
    }

    private void OnDataReceived(byte[] data, string ip, TransportLayer transportLayer)
    {
        if (transportLayer == TransportLayer.UDP)
        {
            HandleUdpData(data, ip);
            return;
        }
        HandleTcpData(data, ip);
    }

    private void HandleTcpData(byte[] data, string ip)
    {
        using var packet = new ReaderPacket(data);

    }

    private void HandleUdpData(byte[] data, string ip)
    {
        using var packet = new ReaderPacket(data);
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

            default:
                break;
        }
    }

    public void Update()
    {
        frameTime = Raylib.GetFrameTime();

        //SendPositionToServer();

        var newPos = Movement();

        pos = Vector3.Lerp(pos, new Vector3(newPos.X, newPos.Y, 0), .16f);
        Raylib.DrawRectangle(800, 10, 10, 10, myColor);
        Raylib.DrawCircle((int)pos.X, (int)pos.Y, 28, owner ? Raylib.DARKGREEN : Raylib.RED);
        Raylib.DrawCircle((int)pos.X, (int)pos.Y, 25, myColor);
    }

    private void SendPositionToServer()
    {
        if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT) || Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT) || Raylib.IsKeyDown(KeyboardKey.KEY_UP) || Raylib.IsKeyDown(KeyboardKey.KEY_DOWN))
        {
            timer.Count(counter =>
            {
                using var packet = new WriterPacket();
                packet.WriteInt((int)MessageType.SYNC_POSITION);
                packet.WriteFloat(pos.X);
                packet.WriteFloat(pos.Y);

                Client.Send(packet, 0);
            });
        }
    }

    private Vector3 Movement()
    {
        if (!owner)
        {
            return networkPos;
        }
        var newPos = new Vector3(pos.X, pos.Y, 0);
        if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT))
        {
            newPos.X -= moveSpeed * frameTime;
        }
        else if (Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT))
        {
            newPos.X += moveSpeed * frameTime;
        }
        if (Raylib.IsKeyDown(KeyboardKey.KEY_UP))
        {
            newPos.Y -= moveSpeed * frameTime;
        }
        else if (Raylib.IsKeyDown(KeyboardKey.KEY_DOWN))
        {
            newPos.Y += moveSpeed * frameTime;
        }
        return newPos;
    }
}

public class Interval
{
    private float timer;
    private readonly float interval;
    private ulong counter;

    public Interval(float interval)
    {
        timer = interval;
        this.interval = interval;
    }

    public void Count(Action<ulong> callback)
    {
        timer -= Raylib.GetFrameTime();
        if (timer <= 0)
        {
            counter++;
            callback(counter);
            timer = interval;
        }
    }
}