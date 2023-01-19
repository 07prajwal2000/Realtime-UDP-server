namespace RealtimeApp.Shared;

public abstract class BasePacket : IDisposable
{
    protected bool Disposed;
    protected readonly MemoryStream MemoryStream;

    public long Length => MemoryStream.Length;

    public BasePacket()
    {
        MemoryStream = new MemoryStream();
    }
    
    public BasePacket(byte[] buffer)
    {
        MemoryStream = new MemoryStream(buffer, 0, buffer.Length, true);
    }

    /// <summary>
    /// Resets the underlying memory stream's Position to 0. 
    /// </summary>
    public void ResetPosition() => MemoryStream.Position = 0;
    /// <summary>
    /// Resets the underlying memory stream's Position to 0 and Clears all the buffer it contains.
    /// </summary>
    public void ClearStream() => MemoryStream.SetLength(0);

    /// <summary>
    /// Gets the Packet's data in Byte[] format
    /// </summary>
    /// <returns>Buffer</returns>
    public byte[] ToArray() => MemoryStream.ToArray();

    public static implicit operator byte[](BasePacket packet) => packet.ToArray();

    public virtual void Dispose()
    {
        if (Disposed) return;

        Disposed = true;
        MemoryStream.Dispose();
    }
}