using DLL;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Services.Helper;
using Services.Helper.XmlModel;
using Services.Repositories.Implimentations.QuanLyHoaDon;
using System;
using System.Collections.Generic;
using System.Net.Security;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace API.Extentions
{
    public class ConsumeScopedServiceHostedService : IHostedService
    {
        private readonly IConfiguration iConfiguration;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IServiceProvider Services { get; }
        private Queue<string> que_datas = new Queue<string>();
        private IModel channel;

        public ConsumeScopedServiceHostedService(IServiceProvider services,
                    IConfiguration IConfiguration,
                    IServiceScopeFactory scopeFactory,
                    Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment,
                    IHttpContextAccessor httpContextAccessor
                    )
        {
            Services = services;
            iConfiguration = IConfiguration;
            _scopeFactory = scopeFactory;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Tracert.WriteLog("Consume Scoped Service Hosted Service is starting.");

            await DoWorkAsync(cancellationToken);
        }

        private async Task DoWorkAsync(CancellationToken cancellationToken)
        {
            try
            {
                Tracert.WriteLog("Consume Scoped Service Hosted Service is working.");

                string queueName = "Q-0200784873-4CEECB41BCEA4768BE4352E0E3DCE70A";
                string URI = "amqp://U-0200784873-68B111A604454C12A6F616CF1C569A9A:ZjM&1O6ds2EQ@14.225.17.176:5671/";

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
                //var connection = factory.CreateConnection();
                //using (var channel = connection.CreateModel())
                //{
                //    var consumer = new EventingBasicConsumer(channel);
                //    consumer.Received += (model, ea) =>
                //    {
                //        var body = ea.Body.ToArray();
                //        var message = Encoding.UTF8.GetString(body);

                //        // Push to queue handler
                //        que_datas.Enqueue(message);

                //        // Nack: thông báo trả lại message cho queue với trường hợp xử lý lỗi hoặc muốn xử lý sau, mess sẽ push lại queue
                //        //channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);

                //        // Ack: thông báo đã xử lý message thành công và xóa khỏi queue
                //        //channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                //        //Console.WriteLine("Consumer: " + message);

                //        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                //    };

                //    // autoAck: true : đọc xong sẽ xóa trên queue, false: vẫn giữ lại trên queue
                //    channel.BasicConsume(queue: queueName,
                //                         autoAck: false,
                //                         consumer: consumer);
                //}

                channel = connection.CreateModel();
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += Consumer_Received;
                channel.BasicConsume(queueName, false, consumer);
                // autoAck: true : đọc xong sẽ xóa trên queue, false: vẫn giữ lại trên queue
                channel.BasicConsume(queue: queueName,
                                     autoAck: false,
                                     consumer: consumer);


                //// Open queue TVAN
                //string hostName = iConfiguration["RabbitQueue:HostName"];
                //string userName = iConfiguration["RabbitQueue:UserName"];
                //string password = iConfiguration["RabbitQueue:Password"];
                //int port = Convert.ToInt32(iConfiguration["RabbitQueue:Port"]);
                //string queueName = iConfiguration["RabbitQueue:QueueName"];

                //// Open queue
                //var factory = new ConnectionFactory()
                //{
                //    HostName = hostName,
                //    Port = port,
                //    UserName = userName,
                //    Password = password,
                //    VirtualHost = "/",
                //    AutomaticRecoveryEnabled = true,
                //    ContinuationTimeout = new TimeSpan(0, 0, 5)        // 5 second
                //};
                //var connection = factory.CreateConnection();
                //channel = connection.CreateModel();
                //var consumer = new EventingBasicConsumer(channel);
                //consumer.Received += Consumer_Received;
                //channel.BasicConsume(queueName, false, consumer);

                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        // TODO TASK
                        if (que_datas.Count > 0)
                        {
                            // Lấy dữ liệu nhận được trong Queue
                            var xML = que_datas.Dequeue();
                            if (!string.IsNullOrEmpty(xML))
                            {
                                xML = xML.Trim();
                            }

                            // Phân tích dữ liệu XML
                            bool res = await AnalysisXMLFromTvan(xML);

                            // Ghi log
                            Tracert.WriteLog(xML);
                        }
                    }
                    catch (Exception ex)
                    {
                        Tracert.WriteLog(string.Empty, ex);
                    }
                    finally
                    {
                        await Task.Delay(1000 * 5, cancellationToken);
                    }
                }
            }
            catch (Exception ex)
            {
                Tracert.WriteLog("WORK READ QUEUE: ", ex);
            }
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            try
            {
                var body = System.Text.Encoding.UTF8.GetString(e.Body.ToArray());

                // Push to queue handler
                que_datas.Enqueue(body);

                // Nack: thông báo trả lại message cho queue với trường hợp xử lý lỗi hoặc muốn xử lý sau, mess sẽ push lại queue
                channel.BasicNack(deliveryTag: e.DeliveryTag, multiple: false, requeue: true);

                // Ack: thông báo đã xử lý message thành công và xóa khỏi queue
                //channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false); 

                //channel.BasicAck(e.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                Tracert.WriteLog(string.Empty, ex);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Tracert.WriteLog("Consume Scoped Service Hosted Service is stopping.");

            return Task.CompletedTask;
        }

        public async Task<bool> AnalysisXMLFromTvan(string strXML)
        {
            bool res = true;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(strXML);
                doc.PreserveWhitespace = true;

                // Find tag MLTDiep
                XmlNodeList elemList = doc.GetElementsByTagName("MLTDiep");
                if (elemList == null || elemList.Count != 1)
                {
                    Tracert.WriteLog(strXML);
                    return false;
                }
                int iMLTDiep = Convert.ToInt32(elemList[0].InnerXml);

                // Create param
                var model = new ThongDiepPhanHoiParams
                {
                    MLTDiep = iMLTDiep,
                    DataXML = strXML
                };

                // Handler
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<Datacontext>();

                    // Write to log
                    await dbContext.AddTransferLog(strXML, 2);

                    // Parser data
                    await XmlHelper.InsertThongDiepNhanAsync(model, dbContext);

                    // Xử lý dữ liệu nhận về từ CQT
                    var thongDiepGuiNhanCQTService = scope.ServiceProvider.GetService<ThongDiepGuiNhanCQTService>();
                    if (thongDiepGuiNhanCQTService != null)
                    {
                        await thongDiepGuiNhanCQTService.XuLyDuLieuNhanVeTuCQT(model);
                    }
                }
            }
            catch (Exception ex)
            {
                res = false;

                Tracert.WriteLog(strXML, ex);
            }

            return res;
        }
    }
}
