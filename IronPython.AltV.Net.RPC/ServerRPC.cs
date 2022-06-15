using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using IronPython.AltV.Net.RPC.Shared;
using System.Text.Json;

namespace IronPython.AltV.Net.RPC;

public static class ServerRPC
{
    static RPCHandlersController Controller = new RPCHandlersController();
    static RPCRequestsController RequestsController = new RPCRequestsController();

    public static void On<TRequestBody, TResponseBody>(string methodName, Func<TRequestBody, TResponseBody> handler) where TRequestBody : new() =>
        Controller.On(methodName, handler);
    public static void On<TRequestBody, TResponseBody>(string methodName, Func<IPlayer, TRequestBody, TResponseBody> handler) where TRequestBody : new() =>
        Controller.On(methodName, handler);

    public static Task<TResponseBody> ExecuteClientMethod<TResponseBody, TRequestBody>(IPlayer player, string methodName, TRequestBody body)
    {
        var request = RequestsController.AddRequest(methodName, body);

        player.Emit("__ironPython:RPC:execute", JsonSerializer.Serialize(request));

        TResponseBody? response = default;

        RequestsController.OnNewReponse += (newResponse) =>
        {
            if (newResponse.Id != request.Id) return;

            response = JsonSerializer.Deserialize<TResponseBody>(newResponse.ResponseBody);
        };

        while (response == null) { }

        return Task.FromResult(response)!;
    }

    public static void InitServerRPC()
    {
        AltAsync.OnClient<IPlayer, string>("__ironPython:RPC:execute", (player, jsonMessage) =>
        {
            var message = JsonSerializer.Deserialize<RPCRequest>(jsonMessage)!;

            var handler = Controller.GetHandler(message.MethodName);

            var result = Controller.ExecuteLocalHandler(handler, player, message.RequestBody);

            player.Emit("__ironPython:RPC:setResponse", $"{message.Id}", JsonSerializer.Serialize(result));
        });

        AltAsync.OnClient<IPlayer, string, string>("__ironPython:RPC:setResponse", (player, messageId, responseBody) =>
        {
            try
            {
                RequestsController.AddResponse(Guid.Parse(messageId), responseBody);
            }
            catch (Exception) { }
        });
    }
}
