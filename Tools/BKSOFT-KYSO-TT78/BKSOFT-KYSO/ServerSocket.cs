using BKSOFT_KYSO.Modal;
using BKSOFT_KYSO.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSockets;
using WebSockets.Common;

namespace BKSOFT_KYSO
{
    public class ServerSocket
    {
        private static IWebSocketLogger _logger;

        private WebServer server;

        private Thread _polling_thread;

        public Setting Setting;

        public void Start(Setting setting)
        {
            Setting = setting;
            // Setting
            _logger = new WebSocketLogger();
            // Create thread handle receive data
            _polling_thread = new Thread(PollingHandler);
            // Thread run background
            _polling_thread.IsBackground = false;
            // Begin thread
            _polling_thread.Start();
        }

        private void PollingHandler()
        {
            try
            {
                //X509Certificate2 cert = CertificateUtil.GetAllCertificateFromStore();
                //string xml = XmlSignatureHelper.SignFromURL("https://localhost:44383/uploaded/DemoTwoInvoice/xml/unsign/test.xml", cert);
                int port = Setting.Port;
                string webRoot = Setting.WebRoot;
                if (!Directory.Exists(webRoot))
                {
                    string baseFolder = AppDomain.CurrentDomain.BaseDirectory;
                    webRoot = baseFolder;
                }

                // used to decide what to do with incoming connections
                ServiceFactory serviceFactory = new ServiceFactory(webRoot, _logger);

                server = new WebServer(serviceFactory, _logger);
                if (port == 443)
                {
                    X509Certificate2 cert = GetCertificate();
                    server.Listen(port, cert);
                }
                else
                {
                    server.Listen(port);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(typeof(Program), ex);
                Console.ReadKey();
            }
        }

        private static X509Certificate2 GetCertificate()
        {
            // it is clearly WRONG to store the certificate and password insecurely on disk like this but this is a demo
            // you would normally use the built in windows certificate store
            string certFile = Settings.Default.CertificateFile;
            if (!File.Exists(certFile))
            {
                throw new FileNotFoundException("Certificate file not found: " + certFile);
            }

            string certPassword = Settings.Default.CertificatePassword;
            var cert = new X509Certificate2(certFile, certPassword);
            _logger.Information(typeof(Program), "Successfully loaded certificate");
            return cert;
        }

        public void Stop()
        {
            try
            {
                // Close Listen
                server.Dispose();

                // Thread abort
                _polling_thread.Abort();
            }
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();
            }
        }
    }
}
