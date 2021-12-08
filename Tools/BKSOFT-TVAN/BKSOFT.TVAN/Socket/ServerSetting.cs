using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BKSOFT.TVAN
{
    public class ServerSetting
    {
        public string ServerListenerIP { get; set; }
        public int ServerListenerPort { get; set; }
        //public RabbitQueueCfg RabbitQueueCfg { set; get; } = new RabbitQueueCfg();
    }
}
