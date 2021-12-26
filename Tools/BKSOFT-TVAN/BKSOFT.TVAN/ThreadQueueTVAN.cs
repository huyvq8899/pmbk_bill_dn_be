using BKSOFT.TVAN.DAL;
using ProtoBuf;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BKSOFT.TVAN
{
    public class ThreadQueueTVAN
    {
        private static string queueName = "Q-0200784873-4CEECB41BCEA4768BE4352E0E3DCE70A";

        private static string URI = "amqp://U-0200784873-68B111A604454C12A6F616CF1C569A9A:ZjM&1O6ds2EQ@14.225.17.176:5671/";

        private string _api = string.Empty;

        private Thread polling_thread;

        private IModel channel;

        private bool is_running = true;

        public uint PostSuc { set; get; } = 0;

        public uint PostErr { set; get; } = 0;

        public ThreadQueueTVAN()
        {
        }

        public void Start()
        {
            // Set flag
            is_running = true;
            // Create thread handle receive data
            polling_thread = new Thread(PollingHandler);
            // Thread run background
            polling_thread.IsBackground = false;
            // Begin thread
            polling_thread.Start();
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            try
            {
                var body = Encoding.UTF8.GetString(e.Body.ToArray());

                // Handler message
                bool res = XMLHelper.HandlMessageFromTCT(body);
                if (res)
                {
                    this.PostSuc += 1;
                }
                else
                {
                    this.PostErr += 1;
                }

                //// Nack: thông báo trả lại message cho queue với trường hợp xử lý lỗi hoặc muốn xử lý sau, mess sẽ push lại queue
                //channel.BasicNack(deliveryTag: e.DeliveryTag, multiple: false, requeue: true);

                //// Ack: thông báo đã xử lý message thành công và xóa khỏi queue
                channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                GPSFileLog.WriteLog(string.Empty, ex);
            }
        }

        private void PollingHandler()
        {
            // Open queue
            OpenSoftDreamQueue();

            // Loop
            while (is_running)
            {
                // CODE here

                Thread.Sleep(2000);
            }
        }

        private bool OpenSoftDreamQueue()
        {
            bool res = false;

            try
            {
                // Open queue TVAN 
                IList<string> hostsName = new List<string>();
                hostsName.Add("14.225.17.176");
                hostsName.Add("14.225.17.177");
                hostsName.Add("14.225.17.178");
                var factory = new ConnectionFactory()
                {
                    Uri = new Uri(URI),
                    Ssl = new SslOption()
                    {
                        Enabled = true,
                        ServerName = "14.225.17.176",
                        Version = SslProtocols.Tls12,
                        AcceptablePolicyErrors = SslPolicyErrors.RemoteCertificateNameMismatch |
                                                SslPolicyErrors.RemoteCertificateChainErrors
                    },
                    RequestedHeartbeat = TimeSpan.FromSeconds(60),
                    VirtualHost = "/",
                };

                var connection = factory.CreateConnection(hostsName);
                channel = connection.CreateModel();
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += Consumer_Received;
                channel.BasicConsume(queueName, false, consumer);
                res = true;
            }
            catch (Exception ex)
            {
                GPSFileLog.WriteLog(string.Empty, ex);
            }

            return res;
        }

        public uint GetMessageCount()
        {
            if(channel != null)
            {
                return channel.MessageCount(queueName);
            }

            return 0;
        }

        public void Dispose()
        {
            try
            {
                is_running = false;

                polling_thread.Abort();
            }
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();
            }
        }
    }
}