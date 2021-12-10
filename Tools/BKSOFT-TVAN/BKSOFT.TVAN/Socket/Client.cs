﻿using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;
using BKSOFT.TVAN.DAL;
using System.IO;
using System.Xml.Linq;
using System.Linq;

namespace BKSOFT.TVAN
{
    public class Client : IDisposable
    {
        private const int DELAY_GET_CLIENT_DATA = 250;

        private const int TIME_1_MINUTE = 60000;

        private const int TIME_20_MINUTE = 1200000;

        public delegate void OnClientDisconnected(string uuid);

        public event OnClientDisconnected ClientDisconnected;

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
                string XML = Utilities.Base64Decode(utfString);
                // Add to log
                bool resp = AddToTCTTransfer(XML);
                // Send to reply client
                this.Send(resp.ToString());
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

        private bool AddToTCTTransfer(string Xml)
        {
            bool res = false;

            try
            {
                //TTChung info, string Xml, bool status

                using (var db = new TCTTranferEntities())
                {
                    TTChung info = GetTTChungFromXML(Xml);

                    db.usp_InsertMessage(DateTime.Now, info.MNGui, info.MNNhan, Convert.ToInt32(info.MLTDiep), info.MTDiep, info.MTDTChieu, info.MST, Xml, true);
                }

                res = true;
            }
            catch (Exception ex)
            {
                GPSFileLog.WriteLog(string.Empty, ex);
            }

            return res;
        }

        private TTChung GetTTChungFromXML(string strXMl)
        {
            TTChung info = new TTChung();
            try
            {
                strXMl = strXMl.Trim();

                // Get Thông tin chung
                byte[] bytes = Encoding.UTF8.GetBytes(strXMl);
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    using (StreamReader reader = new StreamReader(ms))
                    {
                        XDocument xDoc = XDocument.Load(reader);
                        info = xDoc.Descendants("TTChung")
                                       .Select(x => new TTChung
                                       {
                                           PBan = x.Element(nameof(info.PBan)).Value,
                                           MNGui = x.Element(nameof(info.MNGui)).Value,
                                           MNNhan = x.Element(nameof(info.MNNhan)).Value,
                                           MLTDiep = x.Element(nameof(info.MLTDiep)).Value,
                                           MTDiep = x.Element(nameof(info.MTDiep)).Value,
                                           MTDTChieu = x.Element(nameof(info.MTDTChieu)).Value,
                                           MST = x.Element(nameof(info.MST)).Value
                                       }).FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                GPSFileLog.WriteLog(string.Empty, ex);
            }

            return info;
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