using Microsoft.Extensions.Logging;
using Services.Helper;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace API.Services
{
    internal interface IScopedProcessingService
    {
        Task DoWork(CancellationToken stoppingToken);
    }

    internal class ScopedProcessingService : IScopedProcessingService
    {
        private int executionCount = 0;
        private readonly ILogger _logger;

        public ScopedProcessingService(ILogger<ScopedProcessingService> logger)
        {
            _logger = logger;
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                executionCount++;

                _logger.LogInformation(
                    "Scoped Processing Service is working. Count: {Count}", executionCount);

                Tracert.WriteLog("Test: " + DateTime.Now.ToString("HH:mm:ss"));

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
