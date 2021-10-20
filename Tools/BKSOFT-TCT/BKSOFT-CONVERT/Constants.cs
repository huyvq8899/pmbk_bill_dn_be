using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BKSOFT.TCT
{
	public class Constants
    {
        public const string CONNECTION_STRING_CUSMAN = "ConnectionCusMan";
        public const string SERVER_LISTENER_IP = "ServerListenerIP";
        public const string SERVER_LISTENER_PORT = "ServerListenerPort";

        public const string TCT_USER_NAME = "TCT_UserName";
        public const string TCT_PASS_WORD = "TCT_Password";
        public const string TCT_HOST_NAME = "TCT_HostName";
        public const string TCT_PORT = "TCT_Port";
        public const string TCT_EXCHANGE = "TCT_Exchange";
        public const string TCT_ROUTING_KEY_IN = "TCT_RoutingKeyIn";
        public const string TCT_QUEUE_IN = "TCT_QueueIn";
        public const string TCT_ROUTING_KEY_OUT = "TCT_RoutingKeyOut";
        public const string TCT_QUEUE_OUT = "TCT_QueueOut";
        public const string NCC_DOMAIN_API = "NCC_Api";
        

        public const string STARTUP_KEY = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
        public const string STARTUP_VALUE = "BKSOFT.TCT";
    }
}
