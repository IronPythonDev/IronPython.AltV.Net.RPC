using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using IronPython.AltV.Net.RPC.Shared;
using System.Reflection;
using System.Text.Json;

namespace IronPython.AltV.Net.RPC;

public class RPCHandler
{
    public string Name { get; set; }
    public MethodInfo MethodInfo { get; set; }
    public object? Target { get; set; }
    public Type BodyType { get; set; }
}

public static class RPC
{
    static IList<RPCHandler> Handlers = new List<RPCHandler>();

    public static void OnClient<TBody, TResponseBody>(string methodName, Func<IPlayer, TBody, TResponseBody> handler) where TBody : new() =>
        Handlers.Add(new RPCHandler
        {
            Name = methodName,
            MethodInfo = handler.Method,
            Target = handler.Target,
            BodyType = typeof(TBody)
        });

    public static object? ExecuteHandler(IPlayer from, string methodName, string body)
    {
        var handler = Handlers.FirstOrDefault(p => p.Name == methodName);

        return handler?.MethodInfo.Invoke(handler.Target, new object[] { from, JsonSerializer.Deserialize(body, handler.BodyType) });
    }

    public static void InitRPCServer()
    {
        AltAsync.OnClient<IPlayer, string>("__ironPython:RPC:execute", (player, jsonMessage) =>
        {
            var message = JsonSerializer.Deserialize<RPCMessage>(jsonMessage);

            var result = ExecuteHandler(player, message.MethodName, message.RequestBody);

            player.Emit("__ironPython:RPC:setResponse", $"{message.Id}", JsonSerializer.Serialize(result));
        });
    }
}
