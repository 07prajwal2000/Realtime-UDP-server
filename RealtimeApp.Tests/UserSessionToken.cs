using RealtimeApp.Shared;
using System.Security.Cryptography;
using System.Text;

namespace RealtimeApp.Tests;

public static class Constants
{
    public static readonly byte[] Key = Encoding.UTF8.GetBytes("nv5++4*(98*789*(7*(c");
    public static readonly SHA256 sHA256 = SHA256.Create();

    public static HashValue GetHash()
    {
        Span<byte> hash = sHA256.ComputeHash(Key);
        return new HashValue(hash.Slice(0, 8).ToArray());
    }
    
    public static HashValue GetHash(byte[] value)
    {
        Span<byte> hash = sHA256.ComputeHash(value);
        return new HashValue(hash.Slice(0, 16).ToArray());
    }
}

public class UserSessionToken
{
    private static WriterPacket packet;

    public static UserSessionToken Build(string username, string password)
    {
        var sessionID = Constants.GetHash();
        var unameBytes = username.ToBytes();
        var pwdBytes = password.ToBytes();

        packet = new WriterPacket();
        
        packet.WriteBytes(sessionID.Hash);
        packet.WriteBytes(unameBytes);
        packet.WriteBytes(pwdBytes);

        return new UserSessionToken();
    }
}

public struct HashValue
{
    public static readonly SHA256 sHA256 = SHA256.Create();
    public byte[] Hash { get; set; }

    public HashValue(byte[] hash)
    {
        Hash = hash;
    }

    public static HashValue Generate(byte[] value)
    {
        Span<byte> hash = sHA256.ComputeHash(value);
        return new HashValue(hash.Slice(0, 16).ToArray());
    }

    public bool Compare(byte[] other)
    {
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