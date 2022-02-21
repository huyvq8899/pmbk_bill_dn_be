using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Services.Helper
{
    public static class SocketHelper
    {
        public static string SendViaSocketConvert(string ip, int port, string msg)
        {
            string recString = string.Empty;

            try
            {
                // Data buffer for incoming data.  
                byte[] bytes = new byte[1024];

                // Connect to a remote device.  
                try
                {
                    IPAddress ipAddress = IPAddress.Parse(ip);
                    IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                    // Create a TCP/IP  socket.  
                    Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                    // Connect the socket to the remote endpoint. Catch any errors.  
                    try
                    {
                        sender.Connect(remoteEP);

                        // Send the data through the socket.  
                        int bytesSent = sender.Send(Encoding.ASCII.GetBytes(msg));

                        // Receive the response from the remote device.  
                        int bytesRec = sender.Receive(bytes);
                        recString = Encoding.ASCII.GetString(bytes, 0, bytesRec);

                        // Release the socket.  
                        sender.Shutdown(SocketShutdown.Both);
                        sender.Close();

                    }
                    catch (ArgumentNullException ane)
                    {
                        Tracert.WriteLog(string.Empty, ane);
                    }
                    catch (SocketException se)
                    {
                        Tracert.WriteLog(string.Empty, se);
                    }
                    catch (Exception e)
                    {
                        Tracert.WriteLog(string.Empty, e);
                    }

                }
                catch (Exception e)
                {
                    Tracert.WriteLog(string.Empty, e);
                }
            }
            catch (Exception ex)
            {
                Tracert.WriteLog(string.Empty, ex);
            }

            return recString;
        }
    }
}
