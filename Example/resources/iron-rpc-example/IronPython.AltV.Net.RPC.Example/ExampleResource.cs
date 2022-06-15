using AltV.Net;
using AltV.Net.Async;
using IronPython.AltV.Net.RPC.Example.Shared.RPC.Requests;
using IronPython.AltV.Net.RPC.Example.Shared.RPC.Responses;
using System.Numerics;

namespace IronPython.AltV.Net.RPC.Example
{
    public class ExampleResource : AsyncResource
    {
        public override void OnStart()
        {
            RPC.InitRPCServer();
            RPC.OnClient<ExampleRequest, ExampleResponse>("rpc:exampleInvokeServer", (player, body) =>
            {
                Alt.Log($"ExampleString: {body.ExampleString}");
                Alt.Log($"Position: {body.Vector3.X}, {body.Vector3.Y}, {body.Vector3.Z}");

                return new ExampleResponse
                {
                    Id = Guid.NewGuid(),
                    Vector3 = new Net.RPC.Shared.RPCVector3(23f, 23f, 23f)
                };
            });
        }

        public override void OnStop()
        {
            
        }
    }
}