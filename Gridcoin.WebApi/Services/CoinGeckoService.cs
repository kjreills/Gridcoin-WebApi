using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Gridcoin.WebApi.Models;
using Microsoft.Extensions.Logging;

namespace Gridcoin.WebApi.Services
{
    public class CoinGeckoService
    {
        public const string HttpClientKey = "coinGecko";

        private readonly ILogger<CoinGeckoService> _logger;
        private readonly IHttpClientFactory _http;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new(JsonSerializerDefaults.Web);

        public CoinGeckoService(ILogger<CoinGeckoService> logger, IHttpClientFactory http)
        {
            _logger = logger;
            _http = http;
        }

        public async Task<(bool, TickersResponse)> GetTickers()
        {
            //https://api.coingecko.com/api/v3/coins/gridcoin-research/tickers?exchange_ids=south_xchange

            _logger.LogInformation("Getting market information from CoinGecko");

            var client = _http.CreateClient(HttpClientKey);
            var response = await client.GetAsync("/api/v3/coins/gridcoin-research/tickers?exchange_ids=south_xchange");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Error connecting to CoinGecko API", new { response.StatusCode, Content = response.Content.ReadAsStringAsync() });

                return (false, default);
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            var responseContent = JsonSerializer.Deserialize<TickersResponse>(responseBody, _jsonSerializerOptions);

            _logger.LogInformation("Finished getting market information from CoinGecko");

            return (true, responseContent);
        }
    }
    public class TickersResponse
    {
        public string name { get; set; }
        public Ticker[] tickers { get; set; }
    }

    public class Ticker
    {
        public string Base { get; set; }
        public string target { get; set; }
        public Market market { get; set; }
        public float last { get; set; }
        public float volume { get; set; }
        public ConvertedLast converted_last { get; set; }
        public ConvertedVolume converted_volume { get; set; }
        public string trust_score { get; set; }
        public float bid_ask_spread_percentage { get; set; }
        public DateTime timestamp { get; set; }
        public DateTime last_traded_at { get; set; }
        public DateTime last_fetch_at { get; set; }
        public bool is_anomaly { get; set; }
        public bool is_stale { get; set; }
        public string trade_url { get; set; }
        public object token_info_url { get; set; }
        public string coin_id { get; set; }
        public string target_coin_id { get; set; }
    }

    public class Market
    {
        public string name { get; set; }
        public string identifier { get; set; }
        public bool has_trading_incentive { get; set; }
    }

    public class ConvertedLast
    {
        public float btc { get; set; }
        public float eth { get; set; }
        public float usd { get; set; }
    }

    public class ConvertedVolume
    {
        public float btc { get; set; }
        public float eth { get; set; }
        public float usd { get; set; }
    }
}
