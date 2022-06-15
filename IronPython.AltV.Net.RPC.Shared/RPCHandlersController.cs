using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using System.Text.Json;

namespace IronPython.AltV.Net.RPC.Shared
{
    public class RPCHandlersController
    {
        #region Global
        static IList<RPCHandler> Handlers = new List<RPCHandler>();

        public void On<TRequestBody, TResponseBody>(string methodName, Func<TRequestBody, TResponseBody> handler) where TRequestBody : new() =>
            Handlers.Add(new RPCHandler
            {
                Name = methodName,
                MethodInfo = handler.Method,
                Target = handler.Target,
                BodyType = typeof(TRequestBody)
            });

        public void On<TRequestBody, TResponseBody>(string methodName, Func<IPlayer, TRequestBody, TResponseBody> handler) where TRequestBody : new() =>
            Handlers.Add(new RPCHandler
            {
                Name = methodName,
                MethodInfo = handler.Method,
                Target = handler.Target,
                BodyType = typeof(TRequestBody)
            });

        public RPCHandler GetHandler(string methodName) => Handlers.First(p => p.Name == methodName);

        public object? ExecuteLocalHandler(RPCHandler handler, IPlayer player, string body) =>
            handler.MethodInfo.Invoke(handler.Target, new object[] { player, JsonSerializer.Deserialize(body, handler.BodyType)! });

        public object? ExecuteLocalHandlerWithoutExecutor(RPCHandler handler, string body) =>
            handler.MethodInfo.Invoke(handler.Target, new object[] { JsonSerializer.Deserialize(body, handler.BodyType)! });
        #endregion
    }
}
