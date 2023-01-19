namespace RealtimeApp.Shared;

public interface IPacketReader
{
    internal BinaryReader Reader { get; set; }
    bool ReadBool();
    byte ReadByte();
    char ReadChar();
    uint ReadUInt();
    int ReadInt();
    ulong ReadULong();
    long ReadLong();
    string ReadString();
    byte[] ReadBytes(int count);
    char[] ReadChars(int count);
}