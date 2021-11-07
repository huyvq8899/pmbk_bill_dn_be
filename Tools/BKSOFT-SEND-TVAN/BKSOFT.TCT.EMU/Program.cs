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

            // 1.2 Send to tờ khai đăng ký TVAN
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(@"D:\SRC\02-Phan-Mem-Bach-Khoa\SRC\bill-back-end\Tools\BKSOFT-SEND-TVAN\BKSOFT.TCT.EMU\bin\Debug\XML\to-khai-0109205608.xml");
            //xmlDoc.PreserveWhitespace = true;
            TVANHelper.TVANSendData("api/invoice/send", xmlDoc.OuterXml);
        }
    }    
}
