using System.Security.Cryptography;

namespace RealtimeApp.Shared;

public readonly struct HashValue
{
    private static readonly SHA256 Sha256;
    public readonly byte[] Hash;

    #region CTOR
    
    public HashValue(byte[] hash)
    {
        Hash = hash;
    }
    static HashValue()
    {
        Sha256 = SHA256.Create();
    }
    
    #endregion
    
    public static HashValue Generate(byte[] value)
    {
        Span<byte> hash = Sha256.ComputeHash(value);
        return new HashValue(hash.Slice(0, 16).ToArray());
    }
    
    public bool Compare(byte[] other)
    {
        if (other.Length != Hash.Length)
        {
            return false;
        }
        for (var i = 0; i < Hash.Length; i++)
        {
            if (Hash[i] != other[i])
            {
                return false;
            }
        }
        return true;
    }
    public bool Compare(HashValue otherValue)
    {
        var other = otherValue.Hash;

        if (other.Length != Hash.Length)
        {
            return false;
        }
        for (int i = 0; i < Hash.Length; i++)
        {
            if (Hash[i] != other[i])
            {
                return false;
            }
        }
        return true;
    }

    public static implicit operator byte[](HashValue v) => v.Hash;
}