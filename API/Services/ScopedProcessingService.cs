using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Services.Helper;
using Services.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly ILogger _logger;
        private readonly IDatabaseService _databaseService;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ScopedProcessingService(ILogger<ScopedProcessingService> logger,
            IDatabaseService databaseService,
            IConfiguration configuration,
            IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _databaseService = databaseService;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation(
                        $"Scoped Processing Service is working. DateTime: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");

                    await DoSendHoaDonKhongMaToCQTAsync();

                    await Task.WhenAny(Task.Delay(1000 * 60, stoppingToken));
                }
            }
            catch (Exception e)
            {
                Tracert.WriteLog("DoWorkException: ", e);
            }

            Tracert.WriteLog($"Token cancelled: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
        }

        /// <summary>
        /// Do Send hóa đơn không mã đến cơ quan thuế
        /// </summary>
        /// <returns></returns>
        public async Task DoSendHoaDonKhongMaToCQTAsync()
        {
            // var time = _configuration["Config:TimeToSendCQTAutomatic"];

            if (DateTime.Now.ToString("HH") == "23") // 11pm
            {
                Tracert.WriteLog($"Start to send: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");

                var companies = await _databaseService.GetCompanies();

                var tasks = new List<Task<string>>();

                foreach (var item in companies)
                {
                    tasks.Add(SendAPIGuiThongDiepDuLieuHDDTBackground(item));
                }

                var result = await Task.WhenAll(tasks);

                foreach (var item in result)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        Tracert.WriteLog(item);
                    }
                }

                Tracert.WriteLog($"Sent {DateTime.Now:dd/MM/yyyy HH:mm:ss}: " + result.Length);
            }
        }

        /// <summary>
        /// Send hóa đơn không mã đến cơ quan thuế
        /// </summary>
        /// <param name="companyModel"></param>
        /// <returns></returns>
        public async Task<string> SendAPIGuiThongDiepDuLieuHDDTBackground(CompanyModel companyModel)
        {
            var keyParams = new KeyParams
            {
                KeyString = companyModel.ConnectionString.Base64Encode(),
                DatabaseName = companyModel.DataBaseName
            };

            using (var client = new HttpClient())
            {
                var url = GetDomain();

                if (string.IsNullOrEmpty(url))
                {
                    return companyModel.DataBaseName + ": Domain is null";
                }

                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var res = await client.PostAsJsonAsync("/api/ThongDiepGuiDuLieuHDDT/GuiThongDiepDuLieuHDDTBackground", keyParams);

                var resContent = await res.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(resContent) && resContent != "\"\"")
                {
                    return companyModel.DataBaseName + ": " + await res.Content.ReadAsStringAsync();
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// get domain
        /// </summary>
        private string GetDomain()
        {
            var envPath = Path.Combine(_hostingEnvironment.ContentRootPath, "ClientApp/env.js");
            if (File.Exists(envPath))
            {
                var lines = File.ReadLines(envPath);
                foreach (var line in lines)
                {
                    if (line.Contains("window.__env.apiUrl") && !line.Trim().StartsWith("//"))
                    {
                        var httpIndex = line.IndexOf("https");
                        var result = line.Substring(httpIndex, line.Length - httpIndex - 2);
                        return result;
                    }
                }
            }

            return null;
        }
    }
}
