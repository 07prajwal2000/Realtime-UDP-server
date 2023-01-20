//PacketTests.Run();

using RealtimeApp.Shared;

using var writerPacket = new WriterPacket();
writerPacket.WriteBytes(RealtimeApp.Tests.Constants.GetHash().Hash);
writerPacket.WriteString("username");
writerPacket.WriteString("password");

var hash1 = RealtimeApp.Tests.Constants.GetHash(writerPacket);
var hash2 = RealtimeApp.Tests.Constants.GetHash(writerPacket);

Console.WriteLine(hash1.Compare(hash2));