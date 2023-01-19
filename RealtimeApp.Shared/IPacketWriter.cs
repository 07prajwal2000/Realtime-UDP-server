namespace RealtimeApp.Shared;

public interface IPacketWriter
{
    internal BinaryWriter Writer { get; set; }
    void WriteBool(bool value);
    void WriteByte(byte value);
    void WriteChar(char value);
    void WriteUInt(uint value);
    void WriteInt(int value);
    void WriteULong(ulong value);
    void WriteLong(long value);
    void WriteString(string value);
    void WriteBytes(byte[] value);
    void WriteChars(char[] value);
}