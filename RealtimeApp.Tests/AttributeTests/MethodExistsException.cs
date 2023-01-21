using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtimeApp.Tests.AttributeTests;

internal class MethodExistsException : Exception
{
    private readonly string methodName;

    public MethodExistsException(string methodName) : base($"Method with {methodName} already exists")
    {
        this.methodName = methodName;
    }

    public override string ToString()
    {
        return $"Method with {methodName} already exists";
    }
}
