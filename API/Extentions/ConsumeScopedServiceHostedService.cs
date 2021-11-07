﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Services.Helper;
using Services.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Xml;

namespace API.Extentions
{
    public class ConsumeScopedServiceHostedService : IHostedService
    {
        private readonly IConfiguration iConfiguration;
        public IServiceProvider Services { get; }
        private Queue<string> que_datas = new Queue<string>();
        private IModel channel;

        public ConsumeScopedServiceHostedService(IServiceProvider services,
                    IConfiguration IConfiguration)
        {
            Services = services;
            iConfiguration = IConfiguration;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Tracert.WriteLog("Consume Scoped Service Hosted Service is starting.");

            await DoWorkAsync(cancellationToken);
        }

        private async Task DoWorkAsync(CancellationToken cancellationToken)
        {
            Tracert.WriteLog("Consume Scoped Service Hosted Service is working.");

            // Open queue TVAN
            string hostName = iConfiguration["RabbitQueue:HostName"];
            string userName = iConfiguration["RabbitQueue:UserName"];
            string password = iConfiguration["RabbitQueue:Password"];
            int port = Convert.ToInt32(iConfiguration["RabbitQueue:Port"]);
            string queueName = iConfiguration["RabbitQueue:QueueName"];

            // Open queue
            var factory = new ConnectionFactory()
            {
                HostName = hostName,
                Port = port,
                UserName = userName,
                Password = password,
                VirtualHost = "/",
                AutomaticRecoveryEnabled = true,
                ContinuationTimeout = new TimeSpan(0, 0, 5)        // 5 second
            };
            var connection = factory.CreateConnection();
            channel = connection.CreateModel();
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += Consumer_Received;
            channel.BasicConsume(queueName, false, consumer);

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

                        // Ghi lịch sử dữ liệu TVAN

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

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            try
            {
                var body = System.Text.Encoding.UTF8.GetString(e.Body.ToArray());

                // Push to queue handler
                que_datas.Enqueue(body);

                channel.BasicAck(e.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                Tracert.WriteLog(string.Empty, ex);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Tracert.WriteLog(
                "Consume Scoped Service Hosted Service is stopping.");

            return Task.CompletedTask;
        }

        public async Task<bool> AnalysisXMLFromTvan(string strXML)
        {
            bool res = true;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(strXML);

                // Find tag MLTDiep
                XmlNodeList elemList = doc.GetElementsByTagName("MLTDiep");
                if (elemList == null || elemList.Count != 1)
                {
                    res = false;

                    Tracert.WriteLog(strXML);
                }

                //// Đội code ...

                //// Loại Thông Điệp
                //int iMLTDiep = Convert.ToInt32(elemList[0].InnerXml);
                //switch(iMLTDiep)
                //{
                //    case 100:
                //        break;
                //    default:
                //        break;
                //}

                // Quy định kĩ thuật
                //await _quyDinhKyThuatService.InsertThongDiepNhanAsync(model);
            }
            catch (Exception ex)
            {
                res = true;

                Tracert.WriteLog(strXML, ex);
            }

            return res;
        }
    }
}
