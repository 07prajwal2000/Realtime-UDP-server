using System.Text;
using RealtimeApp.Shared;
using RealtimeApp.Shared.Serializers;

public class PacketTests
{
    public static void Run()
    {
        BasePacket.InitializeSerializer(new XmlPacketSerializer());

        using var writer = new WriterPacket();
        writer.WriteBool(true);
        writer.WriteBool(false);
        writer.WriteInt(1234);
        writer.WriteString("hashcoasdni oahod");
        writer.WriteObject(new User
        {
            Age = 22,
            Name = "Prajwal Aradhya",
            UserAddress = new User.Address()
            {
                AddressName = "tumkur",
                CountryCode = 91
            }
        });


        var bytes = Encoding.UTF8.GetBytes("jcaosndowadwaocawcjr");
        writer.WriteInt(bytes.Length);
        writer.WriteBytes(bytes);

        using var reader = new ReaderPacket(writer);
        Console.WriteLine(reader.ReadBool());
        Console.WriteLine(reader.ReadBool());
        Console.WriteLine(reader.ReadInt());
        Console.WriteLine(reader.ReadString());
        Console.WriteLine(reader.ReadObject<User>());

        var length = reader.ReadInt();
        Console.WriteLine(Encoding.UTF8.GetString(reader.ReadBytes(length)));
    }
}

[Serializable]
public class User
{
    public int Age { get; set; }
    public string Name { get; set; }

    public Address UserAddress { get; set; }

    public class Address
    {
        public string AddressName { get; set; }
        public int CountryCode { get; set; }
        public override string ToString()
        {
            return $"{AddressName}, Code - {CountryCode}";
        }
    }

    public override string ToString()
    {
        return $"\n---\n{Name} - {Age}\n{UserAddress}\n---\n";
    }
}