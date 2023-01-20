//PacketTests.Run();

using RealtimeApp;
using RealtimeApp.Shared;
using RealtimeApp.Tests;

using var writerPacket = new WriterPacket();
writerPacket.WriteBytes(RealtimeApp.Tests.Constants.GetHash().Hash);
writerPacket.WriteString("username");
writerPacket.WriteString("password");

var hash1 = HashValue.Generate(writerPacket);
var hash2 = HashValue.Generate(writerPacket);

Console.WriteLine(hash1.Compare(hash2));