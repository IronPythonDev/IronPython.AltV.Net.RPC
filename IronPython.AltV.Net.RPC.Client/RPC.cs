using AltV.Net.Client;
using IronPython.AltV.Net.RPC.Shared;
using System.Text.Json;

namespace IronPython.AltV.Net.RPC.Client;

public static class RPC
{
    static IList<RPCMessage> WaitReponseMessages = new List<RPCMessage>();

    public static Task<TResponseBody> Execute<TResponseBody, TRequestBody>(string methodName, TRequestBody body)
    {
        var message = new RPCMessage
        {
            Id = Guid.NewGuid(),
            RequestBody = JsonSerializer.Serialize(body),
            MethodName = methodName
        };

        Alt.EmitServer("__ironPython:RPC:execute", JsonSerializer.Serialize(message));

        WaitReponseMessages.Add(message);

        while (string.IsNullOrEmpty(WaitReponseMessages.FirstOrDefault(p => p.Id == message.Id)?.ResponseBody)) { }

        var response = JsonSerializer.Deserialize<TResponseBody>(WaitReponseMessages.First(p => p.Id == message.Id).ResponseBody);

        return Task.FromResult(response)!;
    }

    public static void InitRPCClient()
    {
        new RPCVector3(0, 0, 0);
        Alt.OnServer<string, string>("__ironPython:RPC:setResponse", (messageId, responseBody) =>
        {
            Task.Run(() =>
            {
                try
                {
                    var response = WaitReponseMessages.First(p => p.Id == Guid.Parse(messageId));

                    response.ResponseBody = responseBody;
                }
                catch (Exception) { }
            });
        });
    }
}
