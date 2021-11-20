using System.Collections.Generic;
using System.Text.Json.Serialization;

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

    public class RpcResponse<T>
    {
        public T Result { get; set; }
        public string Error { get; set; }
        public int Id { get; set; }
    }


    public class Difficulty
    {
        public double Current { get; set; }
        public double Target { get; set; }
    }

    public class GetInfoResponse
    {
        public string Version { get; set; }

        [JsonPropertyName("minor_version")]
        public int MinorVersion { get; set; }

        public int ProtocolVersion { get; set; }
        public int XalletVersion { get; set; }
        public double Balance { get; set; }
        public double NewMint { get; set; }
        public double Stake { get; set; }
        public int Blocks { get; set; }

        [JsonPropertyName("in_sync")]
        public bool InSync { get; set; }

        public int TimeOffset { get; set; }
        public int UpTime { get; set; }
        public double MoneySupply { get; set; }
        public int Connections { get; set; }
        public string Proxy { get; set; }
        public string IP { get; set; }
        public Difficulty Difficulty { get; set; }
        public bool Testnet { get; set; }
        public int KeyPoolOldest { get; set; }
        public int KeyPoolSize { get; set; }
        public double PayTXFee { get; set; }
        public double MinInput { get; set; }
        public string Errors { get; set; }
    }


    public class GetMiningInfoResponse
    {
        public int Blocks { get; set; }
        public StakeWeight StakeWeight { get; set; }
        public float NetStakeWeight { get; set; }
        public float NetStakingGRCValue { get; set; }
    }

    public class StakeWeight
    {
        public int Minimum { get; set; }
        public int Maximum { get; set; }
        public int Combined { get; set; }
        public float ValueSum { get; set; }
        public float Legacy { get; set; }
    }
}
