using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RealtimeApp.Server;
using RealtimeApp.Server.Abstractions;
using RealtimeApp.Server.Initializers;
using RealtimeApp.Shared;
using RealtimeApp.Shared.Serializers;

BasePacket.InitializeSerializer(new JsonPacketSerializer());

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var services = new ServiceCollection();

services.AddLogging(b =>
{
    b.ClearProviders();
    b.SetMinimumLevel(LogLevel.Trace);
    b.AddConsole();
});

services.AddSingleton<IConfiguration>(configuration);
services.AddServerEvents();
services.AddSingleton<IServer, Server>();
services.AddSingleton<IDataSender>(provider => provider.GetRequiredService<IServer>());

services.AddSingleton<EventProcessor>();
services.AddStackExchangeRedisCache(o =>
{
    o.Configuration = configuration.GetConnectionString("Redis");
});

using var builder = services.BuildServiceProvider();
var server = builder.GetRequiredService<IServer>();

server.Start();