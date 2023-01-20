using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtimeApp;

public static class Extensions
{
    public static byte[] ToBytes(this string value) => Encoding.UTF8.GetBytes(value);

    public static string GetString(this byte[] value) => Encoding.UTF8.GetString(value);
}
