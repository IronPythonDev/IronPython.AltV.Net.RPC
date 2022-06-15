using IronPython.AltV.Net.RPC.Shared;
using System.Numerics;

namespace IronPython.AltV.Net.RPC.Example.Shared.RPC.Requests
{
    public class ExampleRequest
    {
        public string ExampleString { get; set; }
        public RPCVector3 Vector3 { get; set; }
    }
}
