using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtimeApp.Server.Abstractions;

public interface IDataSender
{
    Task Send(byte[] data, string endpoint, TransportLayer layer = TransportLayer.TCP, CancellationToken token = default);
}
