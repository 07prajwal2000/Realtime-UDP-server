using Microsoft.Extensions.DependencyInjection;
using RealtimeApp.Server.Abstractions;
using RealtimeApp.Server.Initializers.Attributes;
using System.Reflection;

namespace RealtimeApp.Server.Initializers;

public static class RegisterFunctions
{
    private static bool registered;

    /// <summary>
    /// Adds all the methods to a collection for RPC which is decorated with <see cref="ServerRPCInit"/> and methods decorated with <see cref="ServerRpc"/>
    /// </summary>
    /// <param name="services">Extension for <see cref="IServiceCollection"/></param>
    /// <returns></returns>
    public static IServiceCollection AddServerEvents(this IServiceCollection services)
    {
        if (registered)
        {
            return services;
        }

        services.AddSingleton<IServerRpcCollection, ServerRpcCollection>(provider => new ServerRpcCollection(AssemblySearch(provider)));

        return services;
    }

    private static Dictionary<string, MethodMetadata> AssemblySearch(IServiceProvider provider)
    {
        Dictionary<string, MethodMetadata> registeredFunctions = new();

        var members = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(x => !x.IsInterface && !x.IsAbstract)
            .Where(x => x.GetCustomAttribute<ServerRPCInit>() != null);

        var dataSender = provider.GetRequiredService<IDataSender>();

        foreach (var member in members)
        {
            var metaData = member
                .GetMethods()
                .Where(x => x.GetCustomAttribute<ServerRpc>() != null)
                .Select(x =>
                {
                    // NOTE: Want to add any params to server rpc class, add it here.
                    return new MethodMetadata(x.GetCustomAttribute<ServerRpc>()?.Name ?? x.Name, x, Activator.CreateInstance(member, args: dataSender)!);
                });

            foreach (var m in metaData)
            {
                if (!registeredFunctions.TryAdd(m.MethodName, m))
                {
                    throw new MethodExistsException(m.MethodName);
                }
            }

        }

        return registeredFunctions;
    }

}
