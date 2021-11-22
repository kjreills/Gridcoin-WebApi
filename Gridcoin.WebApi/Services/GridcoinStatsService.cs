using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Prometheus;
using System.Linq;

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

            var (infoSuccess, info) = await _gridcoinService.GetInfo();
            var (miningSuccess, miningInfo) = await _gridcoinService.GetMiningInfo();
            var (superblocksSuccess, superblocks) = await _gridcoinService.SuperBlocks();

            if (infoSuccess)
            {
                Metrics.CreateCounter("gridcoin_blocks", "The number of blocks in the chain").IncTo(info.Blocks);
                Metrics.CreateGauge("gridcoin_difficulty_current", "Current difficulty").Set(info.Difficulty.Current);
                Metrics.CreateGauge("gridcoin_difficulty_target", "Target difficulty").Set(info.Difficulty.Target);
                Metrics.CreateGauge("gridcoin_total_supply", "Total supply of Gridcoin").Set(info.MoneySupply);
            }            

            if (miningSuccess)
            {
                Metrics.CreateGauge("gridcoin_net_staking_grc_value", "Net value of all staging GRC").Set(miningInfo.NetStakingGRCValue);
                Metrics.CreateGauge("gridcoin_net_stake_weight", "Net weight of staking GRC").Set(miningInfo.NetStakeWeight);
            }

            if (superblocksSuccess)
            {
                var latest = superblocks.OrderByDescending(x => x.Height).FirstOrDefault();

                if (latest != null)
                {
                    Metrics.CreateGauge("gridcoin_total_cpids", "Total number of CPIDs").Set(latest.TotalCPIDs);
                    Metrics.CreateGauge("gridcoin_active_beacons", "Total number of active beacons").Set(latest.ActiveBeacons);
                    Metrics.CreateGauge("gridcoin_inactive_beacons", "Total number of inactive beacons").Set(latest.InactiveBeacons);
                    Metrics.CreateGauge("gridcoin_total_projects", "Total number of whitelisted projects").Set(latest.TotalProjects);
                    Metrics.CreateGauge("gridcoin_total_magnitude", "Total magnitude across all projects").Set(latest.TotalMagnitude);
                    Metrics.CreateGauge("gridcoin_average_magnitude", "Average magnitude").Set(latest.AverageMagnitude);
                    Metrics.CreateGauge("gridcoin_suberblock_height", "Blockheight of last superblock").Set(latest.Height);
                }
            }

            _logger.LogInformation("Finished gathering Gridcoin Stats");
        }
    }
}
