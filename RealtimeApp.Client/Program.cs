//// Client
//using System.Net;
//using System.Net.Sockets;
//using System.Text;

//using var udpClient = new UdpClient();
//var token = new CancellationTokenSource();

//Console.WriteLine("Client connected to - " + IPAddress.Loopback);

//udpClient.Connect(IPAddress.Loopback, 8888);
//using var file = File.AppendText("D:\\test.txt");

//Task.Run(async () =>
//{
//    while (!token.IsCancellationRequested)
//    {
//        var da = await udpClient.ReceiveAsync(token.Token);
//        PrintMessage(da.Buffer);
//        file.Write(Encoding.UTF8.GetString(da.Buffer));
//    }
//}, token.Token);

//while (!token.IsCancellationRequested)
//{
//    Console.WriteLine("Enter a message - ");
//    var enteredMsg = Console.ReadLine() ?? "null";
//    var msg = Encoding.UTF8.GetBytes(enteredMsg);
//    udpClient.Send(msg);
//    Console.ForegroundColor = ConsoleColor.Yellow;
//    Console.WriteLine("-------\n");
//    Console.ResetColor();

//    if (enteredMsg.Contains("null") || enteredMsg.Contains("CLOSE"))
//    {
//        token.Cancel();
//        break;
//    }
//}

//Console.WriteLine("------------------------------------------------");
//Console.WriteLine("Client closed");

//void PrintMessage(byte[] msg)
//{
//    Console.ForegroundColor = ConsoleColor.Green;
//    Console.WriteLine(Encoding.UTF8.GetString(msg));
//    Console.ResetColor();
//}

//Console.WriteLine("Hello, World!");

using RealtimeApp;
using RealtimeApp.Shared;
using RealtimeApp.Shared.Serializers;
using SuperSimpleTcp;
using System.Net;
using System.Net.Sockets;
using System.Text;

BasePacket.InitializeSerializer(new JsonPacketSerializer());

const string SERVER_IP = "127.0.0.1";
const ushort SERVER_PORT = 8888;
IPEndPoint SERVER_UDP_IP = new IPEndPoint(IPAddress.Loopback, SERVER_PORT + 1);

using var udpClient = new UdpClient();
udpClient.Connect(SERVER_UDP_IP);
var token = new CancellationTokenSource();

Task.Run(async () =>
{
    var writerTask = Task.Run(async () =>
    {
        var i = 0;

        while (i++ < 10)
        {
            Console.WriteLine("Sent - " + i + " times");
            udpClient.Send(Encoding.UTF8.GetBytes($"from udp client - {i}"));
            await Task.Delay(1000);
        }

    }, token.Token);

    while (!token.IsCancellationRequested)
    {
        try
        {
            var response = await udpClient.ReceiveAsync();
            var msg = "UDP: Server response - " + response.Buffer.GetString();
            File.AppendAllText("D:\\test.txt", msg);
            Console.WriteLine(msg);
        }
        catch (Exception e)
        {
            File.AppendAllText("D:\\test.txt", $"UDP: Error: {e}");
        }
    }
    await writerTask;
});

var client = new SimpleTcpClient(SERVER_IP, SERVER_PORT);
client.Events.DataReceived += Events_DataReceived;

void Events_DataReceived(object? sender, DataReceivedEventArgs e)
{
    using var packet = new ReaderPacket(e.Data.Array!);

    Console.WriteLine(packet.ReadBool());
    Console.WriteLine(packet.ReadObject<DateTime>());
    Console.WriteLine(packet.ReadBytes().GetString());
}

client.Connect();
Console.WriteLine("Client connected");

Console.WriteLine("Press esc to close");

while (Console.ReadKey().Key != ConsoleKey.Escape)
{
    token.Cancel();
}

client.Disconnect();
Console.WriteLine("client disconnected");