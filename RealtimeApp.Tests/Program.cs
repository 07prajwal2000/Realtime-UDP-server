using System.Text;
using RealtimeApp.Shared;

using var writer = new WriterPacket();
writer.WriteBool(true);
writer.WriteBool(false);
writer.WriteInt(1234);
writer.WriteString("hashcoasdni oahod");

var bytes = Encoding.UTF8.GetBytes("jcaosndowadwaocawcjr");
writer.WriteInt(bytes.Length);
writer.WriteBytes(bytes);

using var reader = new ReaderPacket(writer);
Console.WriteLine(reader.ReadBool());
Console.WriteLine(reader.ReadBool());
Console.WriteLine(reader.ReadInt());
Console.WriteLine(reader.ReadString());

var length = reader.ReadInt();
Console.WriteLine(Encoding.UTF8.GetString(reader.ReadBytes(length)));
