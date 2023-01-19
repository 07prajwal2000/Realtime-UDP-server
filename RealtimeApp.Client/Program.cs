﻿// Client
using System.Net;
using System.Net.Sockets;
using System.Text;

using var udpClient = new UdpClient();

Console.WriteLine("Client connected to - " + IPAddress.Loopback);

udpClient.Connect(IPAddress.Loopback, 8888);

Task.Run(async () =>
{
    var da = await udpClient.ReceiveAsync();
    PrintMessage(da.Buffer);
});

while (true)
{
    Console.WriteLine("Enter a message - ");
    var enteredMsg = Console.ReadLine() ?? "null";
    var msg = Encoding.UTF8.GetBytes(enteredMsg);
    udpClient.Send(msg);
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("-------\n");
    Console.ResetColor();
    
    if (enteredMsg.Contains("null") || enteredMsg.Contains("CLOSE"))
    {
        break;
    }
}

Console.WriteLine("------------------------------------------------");
Console.WriteLine("Client closed");

void PrintMessage(byte[] msg)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(Encoding.UTF8.GetString(msg));
    Console.ResetColor();
}

Console.WriteLine("Hello, World!");