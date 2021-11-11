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
        static void Main(string[] args)
        {
            // Send data to TVAN
            // 1. Gửi dữ liệu đăng ký hóa đơn điện tử.

            //// 1.1 Load xml to sign
            //string path = @"D:\SRC\02-Phan-Mem-Bach-Khoa\SRC\bill-back-end\Tools\BKSOFT-SEND-TVAN\BKSOFT.TCT.EMU\bin\Debug\XML\ToKhai.xml";
            //XMLHelper.XMLSignFromFile(path);

            //// 1.2 Send to tờ khai đăng ký TVAN
            //XmlDocument xmlDoc = new XmlDocument();
            //xmlDoc.Load(@"D:\SRC\02-Phan-Mem-Bach-Khoa\SRC\bill-back-end\Tools\BKSOFT-SEND-TVAN\BKSOFT.TCT.EMU\bin\Debug\XML\20211109101169.xml");
            ////xmlDoc.PreserveWhitespace = true;
            //TVANHelper.TVANSendData("api/invoice/send", xmlDoc.OuterXml);

            // Get server information
            RabbitQueueCfg cfg = new RabbitQueueCfg();
            cfg.UserName = ConfigurationManager.AppSettings[Constants.TVAN_USER_NAME];
            cfg.Password = ConfigurationManager.AppSettings[Constants.TVAN_PASS_WORD];
            cfg.HostName = ConfigurationManager.AppSettings[Constants.TVAN_HOST_NAME];
            cfg.Port = Convert.ToInt32(ConfigurationManager.AppSettings[Constants.TVAN_PORT]);
            cfg.QueueNameOut = ConfigurationManager.AppSettings[Constants.TVAN_QUEUE_OUT];
            ThreadQueueOut pthread = new ThreadQueueOut(cfg);
            pthread.Start();

            Console.ReadKey();
        }
    }
}
