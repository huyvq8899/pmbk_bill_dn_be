using System.Net.Sockets;
using WebSockets.Server.WebSocket;
using WebSockets.Common;
using System.IO;

namespace BKSOFT_KYSO
{
    internal class ChatWebSocketService : WebSocketService
    {
        private readonly IWebSocketLogger _logger;

        public ChatWebSocketService(Stream stream, TcpClient tcpClient, string header, IWebSocketLogger logger)
            : base(stream, tcpClient, header, true, logger)
        {
            _logger = logger;
        }

        protected override void OnTextFrame(string text)
        {
            string result = Handler.ProcessData(text);

            //string response = "SERVER RECEIVE TEXT: " + original;
            base.Send(result);
        }

        protected override void OnBinaryFrame(byte[] payload)
        {
            base.Send("BYTE");
        }
    }
}
