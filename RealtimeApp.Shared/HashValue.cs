using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RealtimeApp.Shared;

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