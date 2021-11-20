using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Gridcoin.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Consumes("application/x-www-form-urlencoded")]
    public class OAuthController : ControllerBase
    {
        public const string HttpClientKey = "OAuth";
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly OAuthSettings _oAuthSettings;

        public OAuthController(IHttpClientFactory httpClientFactory, OAuthSettings oAuthSettings)
        {
            _httpClientFactory = httpClientFactory;
            _oAuthSettings = oAuthSettings;
        }

        [HttpPost("token")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<object> Token([FromForm] ClientCredentials clientCredentials)
        {
            var authHeader = HttpContext.Request.Headers["Authorization"];

            if (authHeader == StringValues.Empty)
            {
                return Unauthorized();
            }

            var encoded = authHeader[0].Split(' ')[1];

            var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
            var idAndSecret = decoded.Split(':');
            clientCredentials.ClientId = idAndSecret[0];
            clientCredentials.ClientSecret = idAndSecret[1];
            clientCredentials.Audience = _oAuthSettings.Audience;

            var http = _httpClientFactory.CreateClient(HttpClientKey);

            var json = JsonSerializer.Serialize(clientCredentials);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await http.PostAsync("/oauth/token", content);

            var responseBody = await response.Content.ReadAsStringAsync();
            var responseContent = JsonSerializer.Deserialize<object>(responseBody);

            return responseContent;
        }
    }

    public class OAuthSettings
    {
        public string Audience { get; set; }
    }

    public class ClientCredentials
    {
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }

        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; set; }

        [JsonPropertyName("audience")]
        public string Audience { get; set; }

        [JsonPropertyName("grant_type")]
        public string GrantType { get; } = "client_credentials";
    }
}
