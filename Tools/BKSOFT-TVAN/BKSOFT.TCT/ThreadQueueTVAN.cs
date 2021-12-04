using BKSOFT.TCT.DAL;
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

namespace BKSOFT.TCT
{
    public class ThreadQueueTVAN
    {
        private static string queueName = "Q-0200784873-4CEECB41BCEA4768BE4352E0E3DCE70A";

        private static string URI = "amqp://U-0200784873-68B111A604454C12A6F616CF1C569A9A:ZjM&1O6ds2EQ@14.225.17.176:5671/";

        private string DomainApi = string.Empty;

        private Queue<string> que_datas = new Queue<string>();

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
            DomainApi = ConfigurationManager.AppSettings["UrlAPI"];
            // Set flag
            is_running = true;
            // Create thread handle receive data
            polling_thread = new Thread(PollingHandler);
            // Thread run background
            polling_thread.IsBackground = false;
            // Begin thread
            polling_thread.Start();
        }

        private bool OpenTVanQueue()
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

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            try
            {
                var body = Encoding.UTF8.GetString(e.Body.ToArray());

                // Push to queue handler
                lock (que_datas)
                {
                    que_datas.Enqueue(body);
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
            OpenTVanQueue();

            while (is_running)
            {
                if (que_datas.Count > 0)
                {
                    lock (que_datas)
                    {
                        var xML = que_datas.Dequeue();

                        GPSFileLog.WriteLog(xML);

                        try
                        {
                            if (string.IsNullOrEmpty(xML))
                            {
                                continue;
                            }
                            xML = xML.Trim();

                            // Get Thông tin chung.
                            TTChung info = GetTTChungFromXML(xML);
                            if (string.IsNullOrEmpty(info.MTDiep))
                            {
                                continue;
                            }

                            // Push To Server GPHD
                            Task<bool> task = HTTPHelper.TCTPostData(this.DomainApi, Utilities.Base64Encode(xML), info.MTDTChieu, info.MST);
                            task.Wait();

                            // Check status
                            if(task.Result)
                            {
                                this.PostSuc += 1;
                            }    
                            else
                            {
                                this.PostErr += 1;
                            }    

                            // Add to log
                            AddToLogTransfer(info, xML, task.Result);
                            AddToTCTTransfer2(info, xML, task.Result);

                            // Push to web test
                            if (!string.IsNullOrWhiteSpace(info.MST) && (info.MST.Contains("0105987432-999") || info.MST.Contains("0105987432-998")))
                            {
                                string urlTest = "https://hoadon-da.pmbk.vn/api/ThongDiepPhanHoi/GetPhanHoiTuCQT";
                                Task<bool> taskTest = HTTPHelper.TCTPostData(urlTest, Utilities.Base64Encode(xML), info.MTDTChieu, info.MST);
                                taskTest.Wait();
                            }
                        }
                        catch (Exception ex)
                        {
                            GPSFileLog.WriteLog(string.Empty, ex);
                        }
                    }
                }

                Thread.Sleep(500);
            }
        }

        public uint GetMessageCount()
        {
            return channel.MessageCount(queueName);
        }

        private TTChung GetTTChungFromXML(string strXMl)
        {
            TTChung info = new TTChung();
            try
            {
                strXMl = strXMl.Trim();

                // Get Thông tin chung
                byte[] bytes = Encoding.UTF8.GetBytes(strXMl);
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    using (StreamReader reader = new StreamReader(ms))
                    {
                        XDocument xDoc = XDocument.Load(reader);
                        info = xDoc.Descendants("TTChung")
                                       .Select(x => new TTChung
                                       {
                                           PBan = x.Element(nameof(info.PBan)).Value,
                                           MNGui = x.Element(nameof(info.MNGui)).Value,
                                           MNNhan = x.Element(nameof(info.MNNhan)).Value,
                                           MLTDiep = x.Element(nameof(info.MLTDiep)).Value,
                                           MTDiep = x.Element(nameof(info.MTDiep)).Value,
                                           MTDTChieu = x.Element(nameof(info.MTDTChieu)).Value,
                                           MST = x.Element(nameof(info.MST)).Value
                                       }).FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                GPSFileLog.WriteLog(string.Empty, ex);
            }

            return info;
        }

        private bool AddToLogTransfer(TTChung info, string Xml, bool status)
        {
            bool res = false;
            try
            {
                // Write log
                using (var db = new TCTTranferEntities())
                {
                    db.TIVans.Add(new TIVan
                    {
                        Id = Guid.NewGuid(),
                        DateTime = DateTime.Now,
                        MNGui = info.MNGui,
                        MNNhan = info.MNNhan,
                        MLTDiep = Convert.ToInt32(info.MLTDiep),
                        MTDiep = info.MTDiep,
                        MTDTChieu = info.MTDTChieu,
                        MST = info.MST,
                        DataXML = Xml,
                        BinXML = Encoding.UTF8.GetBytes(Xml),
                        Status = status
                    });

                    db.SaveChanges();
                }
                res = true;
            }
            catch (Exception ex)
            {
                GPSFileLog.WriteLog(string.Empty, ex);
            }

            return res;
        }

        private bool AddToTCTTransfer2(TTChung info, string Xml, bool status)
        {
            bool res = false;

            try
            {
                using (var db = new TCTTranferEntities())
                {
                    db.usp_InsertMessage(DateTime.Now, info.MNGui, info.MNNhan, Convert.ToInt32(info.MLTDiep), info.MTDiep, info.MTDTChieu, info.MST, Xml, status);
                }

                res = true;
            }
            catch (Exception ex)
            {
                GPSFileLog.WriteLog(string.Empty, ex);
            }

            return res;
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