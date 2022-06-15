using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using IronPython.AltV.Net.RPC.Example.Shared.RPC.Requests;
using IronPython.AltV.Net.RPC.Example.Shared.RPC.Responses;
using System.Text.Json;

namespace IronPython.AltV.Net.RPC.Example
{
    public class ExampleResource : AsyncResource
    {
        public override void OnStart()
        {
            ServerRPC.InitServerRPC();
            ServerRPC.On<ExampleRequest, ExampleResponse>("rpc:exampleInvokeServer", (player, body) =>
            {
                Alt.Log($"ExampleString: {body.ExampleString}");
                Alt.Log($"Position: {body.Vector3.X}, {body.Vector3.Y}, {body.Vector3.Z}");

                return new ExampleResponse
                {
                    Id = Guid.NewGuid(),
                    Vector3 = new Net.RPC.Shared.RPCVector3(23f, 23f, 23f)
                };
            });

            AltAsync.OnPlayerConnect += AltAsync_OnPlayerConnect;
        }

        private async Task AltAsync_OnPlayerConnect(IPlayer player, string reason)
        {
            var response = await ServerRPC.ExecuteClientMethod<ExampleResponse, ExampleRequest>(player, "exampleClientRPCMethod", new ExampleRequest
            {
                ExampleString = "value1234",
                Vector3 = new RPC.Shared.RPCVector3(56f, 56f, 56f)
            });

            Alt.Log($"[exampleClientRPCMethod]Response from client");
            Alt.Log($"[exampleClientRPCMethod]Body value -> Id: {response.Id}, Vector3: {JsonSerializer.Serialize(response.Vector3)}");
        }

        public override void OnStop()
        {

        }
    }
}