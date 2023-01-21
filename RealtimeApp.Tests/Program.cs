//PacketTests.Run();

//using RealtimeApp.Shared;

//using var writerPacket = new WriterPacket();
//writerPacket.WriteBytes(RealtimeApp.Tests.Constants.GetHash().Hash);
//writerPacket.WriteString("username");
//writerPacket.WriteString("password");

//var hash1 = RealtimeApp.Tests.Constants.GetHash(writerPacket);
//var hash2 = RealtimeApp.Tests.Constants.GetHash(writerPacket);

//Console.WriteLine(hash1.Compare(hash2));

//using Microsoft.Extensions.DependencyInjection;
//using SuperSimpleTcp;

//var services = new ServiceCollection();
//services.AddSingleton(new SimpleTcpServer("127.0.0.1", 8888));

//var builder = services.BuildServiceProvider();

//var tcpServer = builder.GetRequiredService<SimpleTcpServer>();

//tcpServer.Events.ClientConnected += OnClientConnected;
//tcpServer.Events.ClientDisconnected += OnClientDisconnected;
//tcpServer.Events.DataReceived += OnDataReceived;

//void OnDataReceived(object? sender, DataReceivedEventArgs e)
//{

//}

//void OnClientDisconnected(object? sender, ConnectionEventArgs e)
//{

//}

//tcpServer.Events.ClientConnected += OnClientConnected;

//void OnClientConnected(object? sender, ConnectionEventArgs e)
//{

//}

//tcpServer.Start();

using RealtimeApp.Tests.AttributeTests;
using System.Reflection;
using System.Text;

var members = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(x => !x.IsInterface && !x.IsAbstract);

var attrs = members.Where(x => x.GetCustomAttribute<Attr>() != null);

var methodDict = new Dictionary<string, MC>();

foreach (var attr in attrs)
{
    var methods = attr.GetMethods();
    var methodWithAttrs = methods
        .Where(x => x.GetCustomAttribute<MethodAttr>() != null)
        .Select(x => 
        new MC(x.GetCustomAttribute<MethodAttr>()?.name ?? x.Name, x, Activator.CreateInstance(attr)!));

    var inst = Activator.CreateInstance(attr);
    foreach (var m in methodWithAttrs)
    {
        if (!methodDict.TryAdd(m.MethodName, m))
        {
            throw new MethodExistsException(m.MethodName);
        }
    }
}

methodDict.First().Value.Invoke(Encoding.UTF8.GetBytes("PRAJWAL ARADHYA - method key - " + methodDict.First().Key));

Console.WriteLine();

public class MC
{
    public readonly string MethodName;
    private readonly MethodInfo Info;
    private readonly object Owner;

    public MC(string methodName, MethodInfo info, object owner)
    {
        MethodName = methodName;
        Info = info;
        Owner = owner;
    }

    public void Invoke(byte[] data)
    {
        Info.Invoke(Owner, new object[] { data });
    }
}