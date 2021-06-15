using AutoMapper;
using DLL;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MimeKit;
using Services.Repositories.Interfaces;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace API.Extentions
{
    internal interface IScopedProcessingService
    {
        void DoWork();
    }
    internal class ScopedProcessingService : IScopedProcessingService
    {
        private readonly ILogger _logger;
        private static readonly HttpClient client = new HttpClient();
        public ScopedProcessingService(ILogger<ScopedProcessingService> logger)
        {
            _logger = logger;
        }

        public async void DoWork()
        {
            _logger.LogInformation("Scoped Processing Service is working.");
            bool isSend = false;
            int currentDay = DateTime.Now.Day;
            //while (true)
            //{
            //    Thread.Sleep((int)30000);
            //    if (DateTime.Now.Hour == 6 && isSend == false)
            //    {
            //        var rs = _ITaskRespositories.SendMail();
            //        //var responseString = await client.GetStringAsync("https://wework.pmbk.vn/api/Task/SendMail");
            //        isSend = true;
            //    }
            //    if (currentDay != DateTime.Now.Day)
            //    {
            //        isSend = false;
            //    }
            //    currentDay = DateTime.Now.Day;
            //}

        }
    }
}
