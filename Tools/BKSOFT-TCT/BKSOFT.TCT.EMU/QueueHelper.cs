using ProtoBuf;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKSOFT.TCT.EMU
{
    public class RabbitQueueCfg
    {
        public string UserName { set; get; }

        public string Password { set; get; }

        public string HostName { set; get; }

        public int Port { set; get; }

        public string Exchange { set; get; }

        public string RoutingKeyIn { set; get; }

        public string QueueNameIn { set; get; }

        public string RoutingKeyOut { set; get; }

        public string QueueNameOut { set; get; }
    }

    public class QueueHelper
    {
        public static byte[] Serialize(string _invoice)
        {
            byte[] b = null;
            using (var ms = new MemoryStream())
            {
                Serializer.Serialize<string>(ms, _invoice);
                b = new byte[ms.Position];
                var fullB = ms.GetBuffer();
                Array.Copy(fullB, b, b.Length);
            }
            return b;
        }

        public static bool SendMsgIn(string inv, RabbitQueueCfg cfg)
        {
            bool res = false;

            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = cfg.HostName,
                    Port = cfg.Port,
                    UserName = cfg.UserName,
                    Password = cfg.Password,
                    VirtualHost = "/",
                    ContinuationTimeout = new TimeSpan(0, 0, 5)        // 5 second
                };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    //byte[] body = Serialize(inv);
                    byte[] body = Encoding.ASCII.GetBytes(inv);

                    channel.BasicPublish(exchange: cfg.Exchange,
                                         routingKey: cfg.RoutingKeyIn,
                                         basicProperties: null,
                                         body: body);
                }

                res = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return res;
        }

        public static bool SendMsgQueueOut(string inv, RabbitQueueCfg cfg)
        {
            bool res = false;

            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = cfg.HostName,
                    Port = cfg.Port,
                    UserName = cfg.UserName,
                    Password = cfg.Password,
                    VirtualHost = "/",
                    ContinuationTimeout = new TimeSpan(0, 0, 5)        // 5 second
                };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    byte[] body = Serialize(inv);

                    channel.BasicPublish(exchange: cfg.Exchange,
                                         routingKey: cfg.RoutingKeyOut,
                                         basicProperties: null,
                                         body: body);
                }

                res = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return res;
        }
    }
}
