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
            RabbitQueueCfg cfg = new RabbitQueueCfg();

            cfg.UserName = ConfigurationManager.AppSettings[Constants.TCT_USER_NAME];
            cfg.Password = ConfigurationManager.AppSettings[Constants.TCT_PASS_WORD];
            cfg.HostName = ConfigurationManager.AppSettings[Constants.TCT_HOST_NAME];
            cfg.Port = Convert.ToInt32(ConfigurationManager.AppSettings[Constants.TCT_PORT]);
            cfg.Exchange = ConfigurationManager.AppSettings[Constants.TCT_EXCHANGE];
            cfg.RoutingKeyIn = ConfigurationManager.AppSettings[Constants.TCT_ROUTING_KEY_IN];
            cfg.QueueNameIn = ConfigurationManager.AppSettings[Constants.TCT_QUEUE_IN];
            cfg.RoutingKeyOut = ConfigurationManager.AppSettings[Constants.TCT_ROUTING_KEY_OUT];
            cfg.QueueNameOut = ConfigurationManager.AppSettings[Constants.TCT_QUEUE_OUT];

            if(args != null && args.Length == 2)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(args[1]);
                string strXML = doc.InnerXml;

                if(args[0] == "-in")
                {
                    bool res = QueueHelper.SendMsgIn(strXML, cfg);
                    if (!res)
                    {
                        Console.WriteLine("Send To Queue TCT In ERROR!!!");
                    }
                    else
                    {
                        Console.WriteLine("Send To Queue TCT In Success");
                    }
                }
                else if (args[0] == "-out")
                {
                    bool res = QueueHelper.SendMsgQueueOut(strXML, cfg);
                    if (!res)
                    {
                        Console.WriteLine("Send To Queue TCT Out ERROR!!!");
                    }
                    else
                    {
                        Console.WriteLine("Send To Queue TCT Out Success");
                    }
                }
            }
        }
    }
}
