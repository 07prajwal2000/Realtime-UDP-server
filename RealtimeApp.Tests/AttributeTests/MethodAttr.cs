using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtimeApp.Tests.AttributeTests;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
internal class MethodAttr : Attribute
{
    public readonly string name;

    public MethodAttr()
    {

    }

    public MethodAttr(string name)
	{
        this.name = name;
    }
}
