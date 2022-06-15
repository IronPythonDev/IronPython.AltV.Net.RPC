using IronPython.AltV.Net.RPC.Shared;
using System.Numerics;

namespace IronPython.AltV.Net.RPC.Example.Shared.RPC.Responses
{
    public class ExampleResponse
    {
        public Guid Id { get; set; }
        public RPCVector3 Vector3 { get; set; }
    }
}
