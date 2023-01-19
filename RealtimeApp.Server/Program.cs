// Server
using System.Net;
using System.Net.Sockets;
using System.Text;

using var udpClient = new UdpClient(new IPEndPoint(IPAddress.Loopback, 8888));
var source = new CancellationTokenSource();
var message = Encoding.UTF8.GetBytes("HELLO FROM SERVER");
Console.CancelKeyPress += (_, _) => { source.Cancel(); };

Console.WriteLine("Server started on - " + IPAddress.Loopback);

while (!source.IsCancellationRequested)
{
    var msg = await udpClient.ReceiveAsync();
    if (msg.Buffer.Length == 0)
    {
        Console.WriteLine("LOG:Empty msg");
        continue;
    }
    var printedMsg = PrintMessage(msg.Buffer);
    if (printedMsg.Contains("CLOSE"))
    {
        message = Encoding.UTF8.GetBytes("Server closing");
        source.Cancel();
    }
    udpClient.Send(message, msg.RemoteEndPoint);
    
    string PrintMessage(byte[] bufMsg)
    {
        ReadOnlySpan<char> message = Encoding.UTF8.GetString(bufMsg);
        Console.WriteLine("Message received :");
        Console.WriteLine(message.ToString());
        if (message[0] == '/')
        {
            var i = 1;
            ReadOnlySpan<char> method;
            while (i < message.Length)
            {
                var c = message[i];
                if (c == ' ')
                {
                    method = message.Slice(1, i - 1);
                    ProcessMessage(message, method, i);
                    break;
                }
                i++;
            void ProcessMessage(ReadOnlySpan<char> message, ReadOnlySpan<char> methodName, int continueIndex)
            {
                var argsStream = message[(continueIndex + 1)..];
                var i = 0;
                // Message processing
                var breakList = new List<int>();
                while (i < argsStream.Length)
                {
                    if (argsStream[i] == ' ')
                    {
                        breakList.Add(i);
                    }
                    i++;
                }
                breakList.Add(argsStream.Length);

                var arguments = new List<int>();
                var initial = 0;
                foreach (var bp in breakList)
                {
                    var res = int.TryParse(argsStream.Slice(initial, bp - initial), out var num);
                    arguments.Add(res ? num : 0);
                    initial = bp;
                }
                
                if (methodName.ToString() == "add")
                {
                    // Sending the results
                    udpClient.Send(Encoding.UTF8.GetBytes("Result of Add is " + arguments.Sum()), msg.RemoteEndPoint);
                    Console.WriteLine("Result of Add is " + arguments.Sum());
                }
                else if (methodName.ToString() == "sub")
                {
                    var sub = 0;
                    arguments.ForEach(x =>
                    {
                        sub -= x;
                    });
                    udpClient.Send(Encoding.UTF8.GetBytes("Result of Add is " + sub), msg.RemoteEndPoint);
                    Console.WriteLine("Result of Add is " + sub);
                }
                else
                {
                    // method not supported
                    udpClient.Send(Encoding.UTF8.GetBytes($"ERR: method '{methodName.ToString()}' not supported"), msg.RemoteEndPoint);
                    Console.WriteLine("ERR: method '{methodName}' not supported");
                }
            }
            }
        }
    
        return message.ToString();
    }

}

Console.WriteLine("------------------------------------------------");
Console.WriteLine("Server closed");


Console.WriteLine("Hello, World!");