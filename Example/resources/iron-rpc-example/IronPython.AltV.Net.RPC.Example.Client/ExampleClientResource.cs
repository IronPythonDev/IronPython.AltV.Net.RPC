using AltV.Net.Client;
using AltV.Net.Client.Async;
using IronPython.AltV.Net.RPC.Example.Shared.RPC.Requests;
using IronPython.AltV.Net.RPC.Example.Shared.RPC.Responses;

namespace IronPython.AltV.Net.RPC.Example.Client
{
    public class ExampleClientResource : AsyncResource
    {
        public override void OnStart()
        {
            RPC.Client.RPC.InitRPCClient();

            Alt.OnPlayerSpawn += Alt_OnPlayerSpawn;
        }

        private void Alt_OnPlayerSpawn()
        {
            Task.Run(async () =>
            {

                var exampleResponse = await RPC.Client.RPC.Execute<ExampleResponse, ExampleRequest>("rpc:exampleInvokeServer", new ExampleRequest
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