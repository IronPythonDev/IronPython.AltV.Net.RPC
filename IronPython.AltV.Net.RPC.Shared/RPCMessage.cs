using System.Text.Json;

namespace IronPython.AltV.Net.RPC.Shared;
public class RPCMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string MethodName { get; set; }
    public string RequestBody { get; set; }
    public string ResponseBody { get; set; }
}
