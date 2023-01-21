using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtimeApp.Tests.AttributeTests;

[Attr]
internal class AttrImpl
{
    [MethodAttr("Method1")]
    public void Method1(byte[] data)
    {
        Console.WriteLine("hello from method 1");
        Console.WriteLine(data.GetString());
    }

    [MethodAttr("test1")]
    public void test1(byte[] data)
    {
        Console.WriteLine("hello from test1");
        Console.WriteLine(data.GetString());
    }
}
