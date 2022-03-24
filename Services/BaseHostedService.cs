using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Gridcoin.WebApi.Services
{
    public class BaseHostedService<T> : BackgroundService, IDisposable where T: IScopedProcessingService
    {
        private readonly IServiceProvider _services;
        protected readonly ILogger<BaseHostedService<T>> _logger;

        public BaseHostedService(IServiceProvider services, ILogger<BaseHostedService<T>> logger)
        {
            _services = services;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("{BackgroundService} is starting", nameof(T));

            using var scope = _services.CreateScope();

            var service = scope.ServiceProvider.GetRequiredService<T>();

            return service.ProcessLoop(stoppingToken);
        }

        public override Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            return base.StopAsync(stoppingToken);
        }
    }

    public interface IScopedProcessingService
    {
        Task ProcessLoop(CancellationToken stoppingToken);
    }

    public class ScopedProcessingService : IScopedProcessingService
    {
        public ScopedProcessingService(TimeSpan period)
        {
            Period = period;
        }

        public TimeSpan Period { get; protected set; }

        public async Task ProcessLoop(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ExecuteAsync(stoppingToken);
                await Task.Delay(Period, stoppingToken);
            }
        }

        protected virtual Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
    }
}