using System.Text.Json;

namespace IronPython.AltV.Net.RPC.Shared
{
    public class RPCRequestsController
    {
        IList<RPCRequest> WaitReponseRequests = new List<RPCRequest>();

        public delegate void OnNewReponseDelegate(RPCRequest request);
        public event OnNewReponseDelegate OnNewReponse;

        public RPCRequest AddRequest<TRequestBody>(string methodName, TRequestBody body)
        {
            var request = new RPCRequest
            {
                Id = Guid.NewGuid(),
                RequestBody = JsonSerializer.Serialize(body),
                MethodName = methodName
            };

            WaitReponseRequests.Add(request);

            return request;
        }

        public void AddResponse(Guid id, string body)
        {
            var request = WaitReponseRequests.FirstOrDefault(p => p.Id == id);

            if (request == null) return;

            request.ResponseBody = body;

            OnNewReponse?.Invoke(request);
        }
    }
}
