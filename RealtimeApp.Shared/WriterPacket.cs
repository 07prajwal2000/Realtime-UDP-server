namespace RealtimeApp.Shared;

public sealed class WriterPacket : BasePacket, IPacketWriter
{
    private BinaryWriter _writer;

    public WriterPacket()
    {
        _writer = new BinaryWriter(MemoryStream);
    }
    public WriterPacket(byte[] buffer) : base(buffer)
    {
        _writer = new BinaryWriter(MemoryStream);
    }
    
    public void WriteBool(bool value) => _writer.Write(value);
    public void WriteByte(byte value) => _writer.Write(value);
    public void WriteChar(char value) => _writer.Write(value);
    public void WriteUInt(uint value) => _writer.Write(value);
    public void WriteInt(int value) => _writer.Write(value);
    public void WriteULong(ulong value) => _writer.Write(value);
    public void WriteLong(long value) => _writer.Write(value);
    public void WriteString(string value) => _writer.Write(value);
    public void WriteBytes(byte[] value) => _writer.Write(value);
    public void WriteChars(char[] value) => _writer.Write(value);
    public void WriteObject<T>(T value)
    {
        var buffer = GetSerializer().Serialize(value);
        _writer.Write(buffer.Length);
        _writer.Write(buffer);
    }

    #region USELESS

    public override void Dispose()
    {
        if (Disposed) return;
        
        base.Dispose();
        _writer.Dispose();
    }

    BinaryWriter IPacketWriter.Writer
    {
        get => _writer;
        set => _writer = value;
    }

    #endregion
}