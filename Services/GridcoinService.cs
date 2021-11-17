using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Gridcoin.WebApi.Models;
using Microsoft.Extensions.Logging;

namespace Gridcoin.WebApi.Services
{
    public class GridcoinService
    {
        public const string HttpClientKey = "gridcoin";

        private readonly ILogger<GridcoinService> _logger;
        private readonly IHttpClientFactory _http;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new(JsonSerializerDefaults.Web);

        public GridcoinService(ILogger<GridcoinService> logger, IHttpClientFactory http)
        {
            _logger = logger;
            _http = http;
        }

        public Task<(bool, GetInfoResponse)> GetInfo()
        {
            return MakeRpcRequest<GetInfoResponse>(nameof(GetInfo));
        }

        private Task<(bool, T)> MakeRpcRequest<T>(string method)
        {
            return MakeRpcRequest<T>(new RpcRequest(method));
        }

        private async Task<(bool, T)> MakeRpcRequest<T>(RpcRequest rpcRequest)
        {
            _logger.LogInformation($"{rpcRequest.Method} called");

            var json = JsonSerializer.Serialize(rpcRequest, _jsonSerializerOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _http.CreateClient(HttpClientKey);
            var response = await client.PostAsync("/", content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Error connecting to Gridcoin wallet", new { response.StatusCode, Content = response.Content.ReadAsStringAsync() });

                return (false, default);
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            var responseContent = JsonSerializer.Deserialize<T>(responseBody);

            _logger.LogInformation($"{rpcRequest.Method} finished", responseContent);

            return (true, responseContent);
        }
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
}
