using RabbitMQ.Client;
using System;

namespace Services.Helper
{
    public static class QueueHelper
    {
        private static string HostName = "113.61.111.146";
        private static int Port = 27059;
        private static string UserName = "tcgp-bachkhoa-0200784873";
        private static string Password = "Vttr993Jzq3DBVMMA4prmd8DtGmpFs95";
        private static string VHost = "tcgp-bachkhoa-0200784873";

        public static bool SendMsg(byte[] body)
        {
            bool res = false;

            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = HostName,
                    Port = Port,
                    UserName = UserName,
                    Password = Password,
                    VirtualHost = VHost,
                    ContinuationTimeout = new TimeSpan(0, 0, 5) // 5 second
                };

                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.BasicPublish(exchange: "EXCHANGE_TCGP_TCTN",
                                         routingKey: "Q_0200784873_TCGP_TCTN",
                                         basicProperties: null,
                                         body: body);
                }

                res = true;
            }
            catch (Exception ex)
            {
                Tracert.WriteLog(string.Empty, ex);
            }

            return res;
        }
    }
}
