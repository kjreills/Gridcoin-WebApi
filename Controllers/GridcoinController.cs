using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Gridcoin.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GridcoinController : ControllerBase
    {
        private readonly ILogger<GridcoinController> _logger;
        private readonly IHttpClientFactory _http;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new(JsonSerializerDefaults.Web);

        public GridcoinController(ILogger<GridcoinController> logger, IHttpClientFactory http)
        {
            _logger = logger;
            _http = http;            
        }

        [HttpGet]
        public async Task<object> GetInfo()
        {
            _logger.LogInformation("GetInfo called");

            var rpcRequest = new RpcRequest("getinfo");
            var json = JsonSerializer.Serialize(rpcRequest, _jsonSerializerOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _http.CreateClient("gridcoin");
            var response = await client.PostAsync("/", content);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var responseContent = JsonSerializer.Deserialize<object>(responseBody);

            _logger.LogInformation("GetInfo finished", responseContent);

            return responseContent;
        }
    }

    public class GridcoinSettings 
    {
        public Uri Uri { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RpcRequest
    {
        public RpcRequest() { }
        public RpcRequest(string method) { Method = method; }
        public RpcRequest(string method, IEnumerable<object> parameters)
        {
            Method = method;
            Params = parameters;
        }

        public string JsonRpc { get; } = "2.0";
        public int Id { get; } = 0;
        public string Method { get; set; }
        public IEnumerable<object> Params { get; set; } = new List<object>();
    }
}
