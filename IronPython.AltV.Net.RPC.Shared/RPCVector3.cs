namespace IronPython.AltV.Net.RPC.Shared
{
    public class RPCVector3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public RPCVector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static RPCVector3 FromVector3(System.Numerics.Vector3 vector3) =>
            new RPCVector3(vector3.X, vector3.Y, vector3.Z);
    }
}
