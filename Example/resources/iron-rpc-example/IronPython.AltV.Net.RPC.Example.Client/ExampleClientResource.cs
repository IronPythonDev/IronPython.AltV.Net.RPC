using AltV.Net.Client;
using AltV.Net.Client.Async;
using IronPython.AltV.Net.RPC.Example.Shared.RPC.Requests;
using IronPython.AltV.Net.RPC.Example.Shared.RPC.Responses;
using System.Text.Json;

namespace IronPython.AltV.Net.RPC.Example.Client
{
    public class ExampleClientResource : AsyncResource
    {
        public override void OnStart()
        {
            RPC.Client.ClientRPC.InitRPCClient();

            Alt.OnPlayerSpawn += Alt_OnPlayerSpawn;

            RPC.Client.ClientRPC.On("exampleClientRPCMethod", (ExampleRequest request) =>
            {
                Alt.Log($"Server executed exampleClientRPCMethod method");
                Alt.Log($"Body value -> ExampleString: {request.ExampleString}, Vector3: {JsonSerializer.Serialize(request.Vector3)}");

                return new ExampleResponse
                {
                    Id = Guid.NewGuid(),
                    Vector3 = new RPC.Shared.RPCVector3(45f, 45f, 45f)
                };
            });
        }

        private void Alt_OnPlayerSpawn()
        {
            Task.Run(async () =>
            {

                var exampleResponse = await RPC.Client.ClientRPC.ExecuteServerMethod<ExampleResponse, ExampleRequest>("rpc:exampleInvokeServer", new ExampleRequest
                {
                    ExampleString = "value",
                    Vector3 = new RPC.Shared.RPCVector3(0, 0, 71)
                });

                Alt.Log($"Vector3: {System.Text.Json.JsonSerializer.Serialize(exampleResponse.Vector3)}");
                Alt.Log($"Id: {exampleResponse.Id}");
            });
        }

        public override void OnStop() { }
    }
}