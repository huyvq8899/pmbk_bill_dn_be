using Microsoft.Extensions.Logging;

namespace API.Extentions
{
    internal interface IScopedProcessingService
    {
        void DoWork();
    }
    internal class ScopedProcessingService : IScopedProcessingService
    {
        private readonly ILogger _logger;
        public ScopedProcessingService(ILogger<ScopedProcessingService> logger)
        {
            _logger = logger;
        }

        public void DoWork()
        {
            _logger.LogInformation("Scoped Processing Service is working.");
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
