using System.Collections.Generic;

namespace Gridcoin.WebApi.Models
{
    public class RpcRequest
    {
        public string JsonRpc { get; } = "2.0";
        public int Id { get; } = 0;
        public string Method { get; }
        public IEnumerable<object> Params { get; } = new List<object>();

        public RpcRequest() { }
        public RpcRequest(string method) { Method = method.ToLowerInvariant(); }
        public RpcRequest(string method, params object[] parameters)
        {
            Method = method.ToLowerInvariant();
            Params = parameters;
        }

    }
}
