using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace API.Services
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private Timer _timer;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public TimedHostedService(
            ILogger<TimedHostedService> logger,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromHours(1));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            if (DateTime.Now.ToString("hh:mm tt") == "3 AM")
            {
                DeleteFiles();
            }

            _logger.LogInformation("Timed Background Service is working.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private void DeleteFiles()
        {
            DirectoryInfo excelDir = new DirectoryInfo(_hostingEnvironment.WebRootPath + $@"\FilesUpload\excels");
            if (excelDir.Exists)
            {
                foreach (FileInfo fi in excelDir.GetFiles())
                {
                    fi.Delete();
                }
            }
            else
            {
                excelDir.Create();
            }

            DirectoryInfo pdfDir = new DirectoryInfo(_hostingEnvironment.WebRootPath + $@"\FilesUpload\pdf");
            if (pdfDir.Exists)
            {
                foreach (FileInfo fi in pdfDir.GetFiles())
                {
                    fi.Delete();
                }
            }
            else
            {
                pdfDir.Create();
            }
        }
    }
}
