using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BKSOFT.TCT.EMU
{
    class Program
    {
        //public static EventHandler<BasicDeliverEventArgs> Consumer_Received { get; private set; }

        static void Main(string[] args)
        {
            //var _connectionFactory = new ConnectionFactory
            //{
            //    Uri = "amqp://RabbitMQUsername:RabbitMQPassword@port",
            //};
            //_connectionFactory.RequestedHeartbeat = 60;
            //var _connection = _connectionFactory.CreateConnection();
            //var _channel = _connection.CreateModel();

            //var consumer = new EventingBasicConsumer(_channel);

            //consumer.Received += (model, ea) =>
            //{
            //    var body = System.Text.Encoding.UTF8.GetString(ea.Body.ToArray());
            //    // body là message thông điệp trả về
            //    // code here
            //    // xử lý ở đây
            //    // TODO: service
            //};
            //_channel.BasicConsume(queue: RabbitMQQueceName,
            //                     noAck: true,
            //                     consumer: consumer);














            //try
            //{


            //    TVanInfo info = new TVanInfo();

            //    // Queue 
            //    info.QueHostName = ConfigurationManager.AppSettings["TVAN_HostName"];
            //    info.QueUserName = ConfigurationManager.AppSettings["Que_UserName"];
            //    info.QuePassword = "ZjM&1O6ds2EQ";
            //    info.QuePort = Convert.ToInt32(ConfigurationManager.AppSettings["TVAN_Port"]);
            //    info.QueName = ConfigurationManager.AppSettings["TVAN_QueueName"];

            //    info.ApiUrl = ConfigurationManager.AppSettings["TVAN_Url"];
            //    info.ApiTaxCode = ConfigurationManager.AppSettings["TVAN_TaxCode"];
            //    info.ApiUserName = ConfigurationManager.AppSettings["TVAN_UserName"];
            //    info.ApiPassword = ConfigurationManager.AppSettings["TVAN_PassWord"];

            //    //// Open queue
            //    //ConnectionFactory factory = new ConnectionFactory();
            //    //factory.UserName = "E65566E0BD464B0890038BAC43C2373B";
            //    //factory.Password = "7^NM6vzxBGFV";
            //    //factory.VirtualHost = "/";
            //    ////factory.Protocol = Protocols.FromEnvironment();
            //    //factory.HostName = "mq.softdreams.vn";
            //    //factory.Port = info.QuePort;
            //    //IConnection conn = factory.CreateConnection();

            //    var factory = new ConnectionFactory()
            //    {
            //        HostName = info.QueHostName,
            //        Port = info.QuePort,
            //        UserName = info.QueUserName,
            //        Password = info.QuePassword,
            //        VirtualHost = "/",
            //        AutomaticRecoveryEnabled = true,
            //        RequestedHeartbeat = 60,
            //        ContinuationTimeout = new TimeSpan(0, 0, 5)        // 5 second
            //    };
            //    var connection = factory.CreateConnection();
            //    var channel = connection.CreateModel();
            //    var consumer = new EventingBasicConsumer(channel);
            //    consumer.Received += Consumer_Received;
            //    // this consumer tag identifies the subscription
            //    // when it has to be cancelled
            //    string consumerTag = channel.BasicConsume(info.QueName, false, consumer);

            //    //// Send hoa don cap ma
            //    //XmlDocument xmlDoc = new XmlDocument();
            //    //xmlDoc.Load(@"D:\XML\TD-0e37dc3b-134c-40d3-89f7-490981029e5d.xml");

            //    //// Send Tvan
            //    //TVANHelper tvan = new TVANHelper(info);
            //    //tvan.TVANSendData("api/invoice/send", xmlDoc.OuterXml);

            //    Console.ReadKey();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.ToString());

            //    FileLog.WriteLog(string.Empty, ex);
            //}
        }

        private static void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            try
            {
                var body = System.Text.Encoding.UTF8.GetString(e.Body.ToArray());

                FileLog.WriteLog(body);

                Console.WriteLine(body);
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(string.Empty, ex);
            }
        }
    }
}
