using DLL;
using ManagementServices.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Services.Helper;
using Services.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
        private readonly IDatabaseService _databaseService;
        private readonly IConfiguration _configuration;

        public ScopedProcessingService(ILogger<ScopedProcessingService> logger,
            IDatabaseService databaseService,
            IConfiguration configuration)
        {
            _logger = logger;
            _databaseService = databaseService;
            _configuration = configuration;
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                executionCount++;

                _logger.LogInformation(
                    "Scoped Processing Service is working. Count: {Count}", executionCount);

                await DoSendHoaDonKhongMaToCQTAsync();

                await Task.Delay(1000, stoppingToken);
            }
        }

        /// <summary>
        /// Do Send hóa đơn không mã đến cơ quan thuế
        /// </summary>
        /// <returns></returns>
        public async Task DoSendHoaDonKhongMaToCQTAsync()
        {
            var time = _configuration["Config:TimeToSendCQTAutomatic"];
            if (DateTime.Now.ToString("HH:mm:ss") == time)
            {
                var companies = await _databaseService.GetCompanies();
                //companies = companies.Where(x => x.DataBaseName == "UAT0200784873998Invoice").ToList();

                var tasks = new List<Task>();

                foreach (var item in companies)
                {
                    tasks.Add(SendAPIGuiThongDiepDuLieuHDDTBackground(item));
                }

                await Task.WhenAll(tasks);

                Tracert.WriteLog("Sent to CQT");
            }
        }

        /// <summary>
        /// Send hóa đơn không mã đến cơ quan thuế
        /// </summary>
        /// <param name="companyModel"></param>
        /// <returns></returns>
        public async Task SendAPIGuiThongDiepDuLieuHDDTBackground(CompanyModel companyModel)
        {
            var keyParams = new KeyParams
            {
                KeyString = companyModel.ConnectionString.Base64Encode(),
                DatabaseName = companyModel.DataBaseName
            };

            using (var client = new HttpClient())
            {
                string url = _configuration["Config:Domain"];
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var res = await client.PostAsJsonAsync("/api/ThongDiepGuiDuLieuHDDT/GuiThongDiepDuLieuHDDTBackground", keyParams);

                if (!res.IsSuccessStatusCode)
                {
                    Tracert.WriteLog("Send Successfully: " + companyModel.DataBaseName);
                }
                else
                {
                    Tracert.WriteLog("Failed send to CQT: " + companyModel.DataBaseName);
                }
            }
        }
    }
}
