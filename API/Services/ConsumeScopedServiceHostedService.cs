using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Services.Helper;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace API.Services
{
    public class ConsumeScopedServiceHostedService : BackgroundService
    {
        private readonly ILogger<ConsumeScopedServiceHostedService> _logger;

        public ConsumeScopedServiceHostedService(IServiceProvider services,
            ILogger<ConsumeScopedServiceHostedService> logger)
        {
            Services = services;
            _logger = logger;
        }

        public IServiceProvider Services { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service running.");

            Tracert.WriteLog($"Consume Scoped Service Hosted Service running: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");

            await DoWork(stoppingToken);
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation(
                "Consume Scoped Service Hosted Service is working.");

                using (var scope = Services.CreateScope())
                {
                    var scopedProcessingService = scope.ServiceProvider.GetRequiredService<IScopedProcessingService>();
                    await scopedProcessingService.DoWork(stoppingToken);
                }
            }
            catch (Exception ex)
            {
                Tracert.WriteLog("ConsumeScopedServiceHostedService: ", ex);
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is stopping.");

            Tracert.WriteLog($"Consume Scoped Service Hosted Service is stopping: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");

            await base.StopAsync(stoppingToken);
        }
    }
}
