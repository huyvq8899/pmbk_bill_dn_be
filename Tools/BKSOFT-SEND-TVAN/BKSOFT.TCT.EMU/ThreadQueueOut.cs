using ProtoBuf;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BKSOFT.TCT.EMU
{
    public class ThreadQueueOut
    {
        private Queue<string> que_datas = new Queue<string>();

        private Thread polling_thread;

        private RabbitQueueCfg queue_cfg;

        private IModel channel;

        private bool is_running = true;

        public ThreadQueueOut(RabbitQueueCfg cfg)
        {
            this.queue_cfg = cfg;
        }

        public void Start()
        {
            // Open queue
            var factory = new ConnectionFactory()
            {
                HostName = queue_cfg.HostName,
                Port = queue_cfg.Port,
                UserName = queue_cfg.UserName,
                Password = queue_cfg.Password,
                VirtualHost = "/",
                AutomaticRecoveryEnabled = true,
                RequestedHeartbeat = 60,
                ContinuationTimeout = new TimeSpan(0, 0, 5)        // 5 second
            };
            var connection = factory.CreateConnection();
            channel = connection.CreateModel();
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += Consumer_Received;
            // this consumer tag identifies the subscription
            // when it has to be cancelled
            string consumerTag = channel.BasicConsume(queue_cfg.QueueNameOut, false, consumer);
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
                var body = System.Text.Encoding.UTF8.GetString(e.Body.ToArray());

                // Push to queue handler
                lock (que_datas)
                {
                    que_datas.Enqueue(body);
                }

                channel.BasicAck(e.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(string.Empty, ex);
            }
        }

        private void PollingHandler()
        {
            while (is_running)
            {
                if (que_datas.Count > 0)
                {
                    lock (que_datas)
                    {
                        var xML = que_datas.Dequeue();

                        FileLog.WriteLog(xML);

                        //try
                        //{
                        //    if (string.IsNullOrEmpty(strXML))
                        //    {
                        //        continue;
                        //    }

                        //    strXML = strXML.Trim();

                        //    // MTDTChieu
                        //    string mTDTChieu = XMLHelper.GetMTDTChieu(strXML);

                        //    // Write log
                        //    using (var db = new TCTTranferEntities())
                        //    {
                        //        // Get taxCode
                        //        var taxCode = db.QueueIns.Where(o => o.MTDiep == mTDTChieu).Select(o => o.MST).FirstOrDefault();
                        //        if (string.IsNullOrEmpty(taxCode))
                        //        {
                        //            taxCode = string.Empty;
                        //        }

                        //        // Push to server
                        //        string strXMLEncode = Utilities.Base64Encode(strXML);
                        //        Task<bool> task = HTTPHelper.TCTPostData(DomainApi, strXMLEncode, mTDTChieu, taxCode);
                        //        task.Wait();

                        //        // Write to log
                        //        DateTime dt = DateTime.Now;
                        //        db.QueueOuts.Add(new QueueOut
                        //        {
                        //            Id = Guid.NewGuid(),
                        //            CreatedDate = dt,
                        //            ModifiedDate = dt,
                        //            MTDTChieu = mTDTChieu,
                        //            DataXML = strXML,
                        //            Status = task.Result
                        //        });

                        //        db.SaveChanges();
                        //    }
                        //}
                        //catch (Exception ex)
                        //{
                        //    FileLog.WriteLog(string.Empty, ex);
                        //}
                    }
                }

                Thread.Sleep(500);
            }
        }

        private string Deserialize(byte[] body)
        {
            using (var stream = new MemoryStream(body))
            {
                return Serializer.Deserialize<string>(stream);
            }
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