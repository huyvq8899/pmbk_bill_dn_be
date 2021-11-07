using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace BKSOFT.TCT
{
    public class Server : IDisposable
    {
        private const int DELAY_LISTENER_CONNECTION = 250;

        private ServerSetting m_setting;

        public Socket _listener;

        private List<Client> _lst_clients = new List<Client>();

        private Thread _acceptConnectionThread;

        private bool _isRunning = false;

        public int NumConvertSuccess { set; get; }

        public int NumConvertError { set; get; }

        public Server(ServerSetting settings)
        {
            this.m_setting = settings;
        }

        public void Start()
        {
            try
            {
                if (!_isRunning)
                {
                    // Any IP
                    IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Any, m_setting.ServerListenerPort);
                    _listener = new Socket(IPAddress.Any.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                    // Listener port
                    _listener.Bind(serverEndPoint);
                    _listener.Listen((int)SocketOptionName.MaxConnections);

                    // Create thread accept connection
                    if (_acceptConnectionThread == null)
                    {
                        _acceptConnectionThread = new Thread(ConnectionHandler);
                        _acceptConnectionThread.IsBackground = false;
                        _acceptConnectionThread.Start();
                    }

                    this._isRunning = true;
                }
            }
            catch (Exception ex)
            {
                GPSFileLog.WriteLog("Error occors when starting server : ", ex);
            }
        }

        private void ConnectionHandler()
        {
            while (_isRunning)
            {
                try
                {
                    Socket socket = _listener.Accept();
                    if (socket != null)
                    {
                        Client client = new Client(socket, this.m_setting);

                        client.ClientDisconnected += OnClientDisconnected;
                        client.Start();

                        // Add to client
                        lock (_lst_clients)
                        {
                            _lst_clients.Add(client);
                        }
                    }
                }
                catch (Exception ex)
                {
                    GPSFileLog.WriteLog("Connection Handler", ex);
                }

                Thread.Sleep(DELAY_LISTENER_CONNECTION);
            }
        }

        private void OnClientFirstMessage(string device_id, string uuid)
        {
            try
            {
                lock (_lst_clients)
                {
                    // Find client.
                    var clis = _lst_clients.Where(o => o.UUID != uuid).ToList();
                    if (clis == null || clis.Count == 0) return;

                    foreach (Client cli in clis)
                    {
                        cli.Stop();
                        _lst_clients.Remove(cli);
                    }
                }
            }
            catch (Exception ex)
            {
                GPSFileLog.WriteLog(string.Empty, ex);
            }
        }

        private void OnClientDisconnected(string uuid)
        {
            try
            {
                lock (_lst_clients)
                {
                    // Find client.
                    Client cli = _lst_clients.Where(o => o.UUID == uuid).FirstOrDefault();
                    _lst_clients.Remove(cli);

                    cli.Stop();

                    // Remove all client is stop
                    _lst_clients.RemoveAll(o => o._isRunning == false);
                }
            }
            catch (Exception ex)
            {
                GPSFileLog.WriteLog(string.Empty, ex);
            }
        }

        public List<string> GetCurrentGPSConnected()
        {
            List<string> result = new List<string>();
            try
            {
                lock (_lst_clients)
                {
                    result = _lst_clients.Select(o => o.UUID).ToList();
                }
            }
            catch (Exception ex)
            {
                GPSFileLog.WriteLog(string.Empty, ex);
            }

            return result;
        }

        public int GetNumberGPSConnected()
        {
            int _count = 0;

            lock (_lst_clients)
            {
                _count = _lst_clients.Count;
            }

            return _count;
        }

        private void CloseGabClients()
        {
            foreach (Client c in this._lst_clients)
            {
                c.Dispose();
            }

            _lst_clients.Clear();
        }

        public void Stop()
        {
            // Stop running
            this._isRunning = false;

            // Stop clients
            foreach (Client c in _lst_clients)
            {
                if (c != null) c.Stop();
            }
            _lst_clients.Clear();

            // Close socket
            try
            {
                _listener.Shutdown(SocketShutdown.Both);
                _listener.Close();
            }
            catch (Exception ex)
            {
                GPSFileLog.WriteLog(string.Empty, ex);
            }

            // Close thread connection thread
            try
            {
                _acceptConnectionThread.Abort();
            }
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();
            }
        }

        public void Dispose()
        {
            Stop();
        }

        ~Server()
        {
            this.Dispose();
        }
    }
}
