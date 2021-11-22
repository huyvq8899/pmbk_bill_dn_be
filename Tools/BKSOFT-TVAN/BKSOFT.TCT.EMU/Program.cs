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
            TVanInfo info = new TVanInfo();
            info.ApiUrl = ConfigurationManager.AppSettings["TVAN_Url"];
            info.ApiTaxCode = ConfigurationManager.AppSettings["TVAN_TaxCode"];
            info.ApiUserName = ConfigurationManager.AppSettings["TVAN_UserName"];
            info.ApiPassword = ConfigurationManager.AppSettings["TVAN_PassWord"];

            // Send hoa don cap ma
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(@"D:\XML\1C21TKE-7-b913ebba-d4d3-4032-8046-79a5de0c2bc7.xml");

            // Send Tvan
            TVANHelper tvan = new TVANHelper(info);
            tvan.TVANSendData("api/invoice/send", xmlDoc.OuterXml);
        }
    }
}
