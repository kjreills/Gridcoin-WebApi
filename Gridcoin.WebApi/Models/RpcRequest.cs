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
        public long MinorVersion { get; set; }

        public long ProtocolVersion { get; set; }
        public long XalletVersion { get; set; }
        public double Balance { get; set; }
        public double NewMlong { get; set; }
        public double Stake { get; set; }
        public long Blocks { get; set; }

        [JsonPropertyName("in_sync")]
        public bool InSync { get; set; }

        public long TimeOffset { get; set; }
        public long UpTime { get; set; }
        public double MoneySupply { get; set; }
        public long Connections { get; set; }
        public string Proxy { get; set; }
        public string IP { get; set; }
        public Difficulty Difficulty { get; set; }
        public bool Testnet { get; set; }
        public long KeyPoolOldest { get; set; }
        public long KeyPoolSize { get; set; }
        public double PayTXFee { get; set; }
        public double MinInput { get; set; }
        public string Errors { get; set; }
    }


    public class GetMiningInfoResponse
    {
        public long Blocks { get; set; }
        public StakeWeight StakeWeight { get; set; }
        public float NetStakeWeight { get; set; }
        public float NetStakingGRCValue { get; set; }
    }

    public class StakeWeight
    {
        public long Minimum { get; set; }
        public long Maximum { get; set; }
        public long Combined { get; set; }
        public float ValueSum { get; set; }
        public float Legacy { get; set; }
    }

    public class SuperBlocksResponse
    {
        [JsonPropertyName("total_cpids")]
        public long TotalCPIDs { get; set; }

        [JsonPropertyName("active_beacons")]
        public long ActiveBeacons { get; set; }

        [JsonPropertyName("inactive_beacons")]
        public long InactiveBeacons { get; set; }

        [JsonPropertyName("total_projects")]
        public long TotalProjects { get; set; }

        [JsonPropertyName("total_magnitude")]
        public long TotalMagnitude { get; set; }

        [JsonPropertyName("average_magnitude")]
        public double AverageMagnitude { get; set; }

        public long Height { get; set; }
    }
}
