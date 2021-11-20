using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Prometheus;

namespace Gridcoin.WebApi.Services
{
    public class GridcoinStatsService : ScopedProcessingService
    {
        private readonly ILogger<GridcoinStatsService> _logger;
        private readonly GridcoinService _gridcoinService;

        public GridcoinStatsService(ILogger<GridcoinStatsService> logger, TimeSpan frequency, GridcoinService gridcoinService) : base(frequency)
        {
            _logger = logger;
            _gridcoinService = gridcoinService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Begin Gridcoin Stats gathering");

            var (success, info) = await _gridcoinService.GetInfo();

            if (success)
            {
                Metrics.CreateCounter("gridcoin_blocks", "The number of blocks in the chain").IncTo(info.Blocks);
                Metrics.CreateGauge("gridcoin_difficulty_current", "Current difficulty").Set(info.Difficulty.Current);
                Metrics.CreateGauge("gridcoin_difficulty_target", "Target difficulty").Set(info.Difficulty.Target);
                Metrics.CreateGauge("gridcoin_total_supply", "Total supply of Gridcoin").Set(info.MoneySupply);
                Metrics.CreateGauge("gridcoin_total_supply", "Total supply of Gridcoin").Set(info.MoneySupply);
            }

            _logger.LogInformation("Finished gathering Gridcoin Stats");
        }
    }
}
