using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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

        public Task<(bool, GetMiningInfoResponse)> GetMiningInfo()
        {
            return MakeRpcRequest<GetMiningInfoResponse>(nameof(GetMiningInfo));
        }

        public Task<(bool, IEnumerable<SuperBlocksResponse>)> SuperBlocks()
        {
            return MakeRpcRequest<IEnumerable<SuperBlocksResponse>>(nameof(SuperBlocks));
        }

        private Task<(bool, T)> MakeRpcRequest<T>(string method)
        {
            return MakeRpcRequest<T>(new RpcRequest(method));
        }

        private async Task<(bool, T)> MakeRpcRequest<T>(RpcRequest rpcRequest)
        {
            _logger.LogInformation("{Method} called", rpcRequest.Method);

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
            var responseContent = JsonSerializer.Deserialize<RpcResponse<T>>(responseBody, _jsonSerializerOptions);

            _logger.LogInformation("{Method} finished. Response: {Response}", rpcRequest.Method, responseContent);

            return (true, responseContent.Result);
        }
    }
}
