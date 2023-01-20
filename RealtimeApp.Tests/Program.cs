//PacketTests.Run();

//using RealtimeApp.Shared;

//using var writerPacket = new WriterPacket();
//writerPacket.WriteBytes(RealtimeApp.Tests.Constants.GetHash().Hash);
//writerPacket.WriteString("username");
//writerPacket.WriteString("password");

//var hash1 = RealtimeApp.Tests.Constants.GetHash(writerPacket);
//var hash2 = RealtimeApp.Tests.Constants.GetHash(writerPacket);

//Console.WriteLine(hash1.Compare(hash2));

using Microsoft.Extensions.DependencyInjection;
using SuperSimpleTcp;

var services = new ServiceCollection();
services.AddSingleton(new SimpleTcpServer("127.0.0.1", 8888));

var builder = services.BuildServiceProvider();

var tcpServer = builder.GetRequiredService<SimpleTcpServer>();

tcpServer.Events.ClientConnected += OnClientConnected;
tcpServer.Events.ClientDisconnected += OnClientDisconnected;
tcpServer.Events.DataReceived += OnDataReceived;

void OnDataReceived(object? sender, DataReceivedEventArgs e)
{
    
}

void OnClientDisconnected(object? sender, ConnectionEventArgs e)
{
    
}

tcpServer.Events.ClientConnected += OnClientConnected;

void OnClientConnected(object? sender, ConnectionEventArgs e)
{
    
}

tcpServer.Start();