using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Gridcoin.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GridcoinController : ControllerBase
    {
        private readonly ILogger<GridcoinController> _logger;
        private readonly GridcoinSettings _settings;
        private HttpClient _http;

        public WeatherForecastController(ILogger<GridcoinController> logger, GridcoinSettings settings)
        {
            _logger = logger;
            _settings = settings;
        }

        [HttpGet]
        public async Task<object> GetInfo()
        {
            var response = await client.PostAsync(_settings.Uri);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
        }
    }

    public class GridcoinSettings 
    {
        public Uri Uri { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
