using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtimeApp.Tests.AttributeTests;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
internal class Attr : Attribute
{
}
