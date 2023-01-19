namespace RealtimeApp.Shared;

public sealed class ReaderPacket : BasePacket, IPacketReader
{
    private BinaryReader _reader;

    public ReaderPacket()
    {
        _reader = new BinaryReader(MemoryStream);
    }
    public ReaderPacket(byte[] buffer) : base(buffer)
    {
        _reader = new BinaryReader(MemoryStream);
    }

    public bool ReadBool() => _reader.ReadBoolean();
    public byte ReadByte() => _reader.ReadByte();
    public char ReadChar() => _reader.ReadChar();
    
    public uint ReadUInt() => _reader.ReadUInt32();
    public int ReadInt() => _reader.ReadInt32();
    
    public ulong ReadULong() => _reader.ReadUInt64();
    public long ReadLong() => _reader.ReadInt64();
    
    public string ReadString() => _reader.ReadString();
    public byte[] ReadBytes(int count) => _reader.ReadBytes(count);
    public char[] ReadChars(int count) => _reader.ReadChars(count);
    public T? ReadObject<T>()
    {
        var length = _reader.ReadInt32();
        var buffer = _reader.ReadBytes(length);
        return GetSerializer().Deserialize<T>(buffer);
    }
    
    #region USELESS
    
    public static implicit operator ReaderPacket(byte[] buffer) => new (buffer);

    BinaryReader IPacketReader.Reader
    {
        get => _reader;
        set => _reader = value;
    }
    
    public override void Dispose()
    {
        if (Disposed) return;

        base.Dispose();
        _reader.Dispose();
    }
    #endregion
}