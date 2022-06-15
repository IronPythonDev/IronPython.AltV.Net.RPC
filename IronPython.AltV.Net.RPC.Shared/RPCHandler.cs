using System.Reflection;

namespace IronPython.AltV.Net.RPC.Shared;

public class RPCHandler
{
    public string Name { get; set; }
    public MethodInfo MethodInfo { get; set; }
    public object? Target { get; set; }
    public Type BodyType { get; set; }
}
