using Microsoft.Extensions.Logging;
using SuperSimpleTcp;
using System.Net;
using System.Net.Sockets;

namespace RealtimeApp.Server;

public enum TransportLayer
{
    TCP,
    UDP
}

public static class ServerEvents
{
    public static Func<string, Task> OnConnected;
    public static Func<string, Task> OnDisconnected;
    public static Func<byte[], string, TransportLayer, Task> OnDataReceived;
}

internal class Server : IServer
{
    private readonly CancellationTokenSource tokenSource = new();
    private readonly SimpleTcpServer server;
    private readonly UdpClient udpServer;
    private readonly ILogger<Server> logger;
    private readonly EventProcessor eventProcessor;
    private bool started;

    public const string SERVER_IP = "127.0.0.1";
    public const ushort SERVER_PORT = 8888;

    public Server(ILogger<Server> logger, EventProcessor eventProcessor)
	{
		server = new SimpleTcpServer(SERVER_IP, SERVER_PORT);
        udpServer = new UdpClient(SERVER_IP, SERVER_PORT);
        this.logger = logger;
        this.eventProcessor = eventProcessor;
    }

	public void Start()
	{
        if (started)
        {
            return;
        }

		try
		{
            RegisterEvents();
            server.StartAsync();
            UdpReceiveLoop();
            LogStart();
            eventProcessor.ProcessEvents(this);
            started = true;

            while (Console.ReadKey().Key != ConsoleKey.Escape)
            {
                tokenSource.Cancel();
            }
        }
		catch (Exception e)
		{
            logger.LogError(e, e.Message);
		}
	}

    private Task UdpReceiveLoop()
    {
        return Task.Run(() =>
        {
            while (!tokenSource.IsCancellationRequested)
            {
                var endpoint = new IPEndPoint(0, default);
                var bytes = udpServer.Receive(ref endpoint);
                ServerEvents.OnDataReceived.Invoke(bytes, endpoint.ToString(), TransportLayer.UDP);
            }
        });
    }

    public void RegisterEvents()
    {
        server.Events.ClientConnected += ClientConnected;
        server.Events.ClientDisconnected += ClientDisconnected;
        server.Events.DataReceived += DataReceived;
    }

    private void DataReceived(object? sender, DataReceivedEventArgs e)
    {
        Task.Run(() => ServerEvents.OnDataReceived(e.Data.Array!, e.IpPort, TransportLayer.TCP));
    }

    private void ClientDisconnected(object? sender, ConnectionEventArgs e)
    {
        Task.Run(() => ServerEvents.OnDisconnected(e.IpPort));
    }

    private void ClientConnected(object? sender, ConnectionEventArgs e)
    {
        Task.Run(() => ServerEvents.OnConnected(e.IpPort));
    }

    public Task Send(byte[] data, string endpoint, TransportLayer layer = TransportLayer.TCP, CancellationToken token = default)
    {
        if (layer == TransportLayer.UDP)
        {
            var ip = new IPEndPoint(IPAddress.Parse(endpoint), 0);
            return udpServer.SendAsync(data, ip, token).AsTask();
        }
        return server.SendAsync(endpoint, data, token);
    }

    private void LogStart()
    {
        logger.LogDebug($"TCP & UDP Servers started. \nAddress : {SERVER_IP} \t port : {SERVER_PORT}");
        logger.LogDebug("Press escape to close the server");
    }
}
