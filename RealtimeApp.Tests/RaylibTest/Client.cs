using RealtimeApp.Shared;
using SuperSimpleTcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RealtimeApp.Tests.RaylibTest;

internal class Client : IDisposable
{
    private UdpClient udpClient;
    private SimpleTcpClient tcpClient;
    private CancellationTokenSource token;

    private static Client Instance;

    public static Action<byte[], string, TransportLayer> OnReceivedEvent;
    public static Action<string> OnConnected;
    public static Action<string> OnDisconnected;

    public Client()
    {
        Instance = this;
    }

    public void Dispose()
    {
        token.Cancel();
        udpClient.Dispose();
        tcpClient.Disconnect();
        tcpClient.Dispose();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="tl">0 is udp, rest is tcp</param>
    public static void Send(byte[] data, TransportLayer tl)
    {
        Task.Run(() =>
        {
            if (tl == TransportLayer.UDP)
            {
                Instance.udpClient.Send(data);
                return;
            }
            Instance.tcpClient.Send(data);
        });
    }

    public void Start()
    {
        // const string SERVER_IP = "127.0.0.1";
        const string SERVER_IP = "192.168.1.7";
        const ushort SERVER_PORT = 8888;
        IPEndPoint SERVER_UDP_IP = new IPEndPoint(IPAddress.Parse(SERVER_IP), SERVER_PORT);
        //IPEndPoint SERVER_UDP_IP = new IPEndPoint(IPAddress.Loopback, SERVER_PORT + 1);

        udpClient = new UdpClient();
        udpClient.Connect(SERVER_UDP_IP);
        token = new CancellationTokenSource();

        Task.Run(async () =>
        {

            while (!token.IsCancellationRequested)
            {
                try
                {
                    var response = await udpClient.ReceiveAsync();
                    OnReceivedEvent?.Invoke(response.Buffer, response.RemoteEndPoint.ToString(), TransportLayer.UDP);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            //await writerTask;
        });

        tcpClient = new SimpleTcpClient(SERVER_IP, SERVER_PORT);
        tcpClient.Events.DataReceived += Events_DataReceived;
        tcpClient.Events.Connected += Events_Connected;
        tcpClient.Events.Disconnected += Events_Disconnected;


        tcpClient.Connect();
    }

    private void Events_Disconnected(object? sender, ConnectionEventArgs e)
    {
        OnDisconnected?.Invoke(e.IpPort);
    }

    private void Events_DataReceived(object? sender, DataReceivedEventArgs e)
    {
        OnReceivedEvent?.Invoke(e.Data.Array!, e.IpPort, TransportLayer.TCP);
    }

    private void Events_Connected(object? sender, ConnectionEventArgs e)
    {
        OnConnected?.Invoke(e.IpPort);
    }
}
