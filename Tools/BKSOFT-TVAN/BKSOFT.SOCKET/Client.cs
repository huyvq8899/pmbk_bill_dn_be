using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;

using System.Configuration;
using Microsoft.Office.Interop.Word;
using Newtonsoft.Json;

namespace BKSOFT.SOCKET
{
    public class Client : IDisposable
    {
        private const int DELAY_GET_CLIENT_DATA = 250;

        private const int TIME_1_MINUTE = 60000;

        private const int TIME_20_MINUTE = 1200000;

        public delegate void OnClientDisconnected(string uuid);

        public event OnClientDisconnected ClientDisconnected;

        public delegate void OnClientConvertionToPDF(string result);

        public event OnClientConvertionToPDF ClientConvertionToPDF;

        private Socket _tcpClient;

        private Thread _pollingThread;

        public bool _isRunning = false;

        private ServerSetting _settings;

        public string UUID { set; get; }

        public int _iPingTimerCounter = 0;

        public int _iUpTime { set; get; }

        public int _iNoConnected { set; get; }

        public int _noDataReceivedCounter { set; get; }

        #region Methods

        public Client(Socket tcpClient, ServerSetting settings)
        {
            this._tcpClient = tcpClient;
            this._settings = settings;
            this.UUID = Guid.NewGuid().ToString();
        }

        public void Start()
        {
            try
            {
                _isRunning = true;
                // Create thread handle receive data
                _pollingThread = new Thread(PollingHandler);
                // Thread run background
                _pollingThread.IsBackground = false;
                // Begin thread
                _pollingThread.Start();
            }
            catch (Exception ex)
            {
                GPSFileLog.WriteLog(string.Empty, ex);
            }
        }

        public void Stop()
        {
            try
            {
                _isRunning = false;

                // Close socket
                _tcpClient.Close();
                _tcpClient.Shutdown(SocketShutdown.Both);

                try
                {
                    // Thread stop
                    _pollingThread.Abort();
                }
                catch (ThreadAbortException)
                {
                    Thread.ResetAbort();
                }

                // Call GC
                GC.SuppressFinalize(this);
            }
            catch (Exception)
            {
            }
        }

        private void PollingHandler()
        {
            while (_isRunning)
            {
                if (this._tcpClient.Connected)
                {
                    Receive();

                    _iNoConnected = 0;
                }
                else
                {
                    _iNoConnected++;
                }

                if (_iNoConnected > 40 && ClientDisconnected != null)
                {
                    ClientDisconnected(this.UUID);
                }

                Thread.Sleep(DELAY_GET_CLIENT_DATA);
            }
        }

        public void Receive()
        {
            try
            {
                // Not data
                _iUpTime += DELAY_GET_CLIENT_DATA;

                // Check connected
                if (!IsSocketConnected(_tcpClient))
                {
                    if (ClientDisconnected != null)
                    {
                        ClientDisconnected(this.UUID);
                    }

                    return;
                }

                if (_tcpClient.Available <= 5)
                {
                    _noDataReceivedCounter += DELAY_GET_CLIENT_DATA;
                    if (_iUpTime > TIME_20_MINUTE)
                    {
                        // Close socket
                        if (ClientDisconnected != null)
                        {
                            ClientDisconnected(this.UUID);
                        }
                    }

                    return;
                }
                _noDataReceivedCounter = 0;

                // Create new array byte.
                byte[] bytes = new byte[_tcpClient.Available];
                // Receive data from socket
                int length = _tcpClient.Receive(bytes);
                // Byte to string
                string utfString = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                // Decode
                string json = Utilities.Base64Decode(utfString);
                // Json to object
                var obj = JsonConvert.DeserializeObject<QueueInObj>(json);
                // Execute convert
                string resp = SendToQueueIn(obj);

                // Send to reply client
                this.Send(resp);
            }
            catch (Exception ex)
            {
                GPSFileLog.WriteLog(string.Empty, ex);

                // Close socket
                if (ClientDisconnected != null)
                {
                    ClientDisconnected(this.UUID);
                }
            }
        }

        private string SendToQueueIn(QueueInObj obj)
        {
            string result = "false";
            try
            {
                // Send to client

                //    bool res = false;
                //    switch (obj.Type)
                //    {
                //        case 0:     // Doc To PDF
                //            if (_settings.TypeConvertion == 0)           // Use Microsoft.Office save PDF
                //            {
                //                res = FileHelper.OfficeSaveDocToPDF(obj.MTDiep, obj.DataXML, WdSaveFormat.wdFormatPDF);
                //            }
                //            else if (_settings.TypeConvertion == 1)      // Use Microsoft.Office export PDF 
                //            {
                //                res = FileHelper.OfficeExportDocToPDF(obj.MTDiep, obj.DataXML);
                //            }
                //            else // Use Sprie doc to PDF 
                //            {
                //                res = FileHelper.SpireDocToPDF(obj.MTDiep, obj.DataXML);
                //            }
                //            break;
                //        case 1:     // Excel To PDF
                //            break;
                //        case 100:   // Get configuration
                //            if (!string.IsNullOrEmpty(obj.SiteName))
                //            {
                //                string cfg = Utilities.GetPhysicalPathFromSiteName(obj.SiteName);
                //                if (!string.IsNullOrEmpty(cfg))
                //                {
                //                    result = Utilities.GetConnectionStringFromConfig($"{cfg}\\appsettings.json");

                //                    return result;
                //                }
                //            }
                //            break;
                //        default:
                //            break;
                //    }

                //    // Response
                //    if (res)
                //    {
                //        result = "true";
                //    }
                //    else
                //    {
                //        result = "false";
                //    }
            }
            catch (Exception ex)
            {
                result = "false";

                GPSFileLog.WriteLog(string.Empty, ex);
            }

            return result;
        }

        public void Send(string data)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(data))
                {
                    _tcpClient.Send(ASCIIEncoding.ASCII.GetBytes(data));
                }
            }
            catch (Exception)
            {
                if (ClientDisconnected != null)
                {
                    ClientDisconnected(this.UUID);
                }
            }
        }

        public void Send(byte[] bytes)
        {
            try
            {
                if (bytes == null) return;

                _tcpClient.Send(bytes);
            }
            catch (Exception)
            {
                if (ClientDisconnected != null)
                {
                    ClientDisconnected(this.UUID);
                }
            }
        }

        public bool IsSocketConnected(Socket s)
        {
            return !((s.Poll(1000, SelectMode.SelectRead) && (s.Available == 0)) || !s.Connected);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Stop();
        }

        ~Client()
        {
            Dispose();
        }
        #endregion
    }
}
