using BKSOFT.TVAN.DAL;
using ProtoBuf;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BKSOFT.TVAN
{
    public class ThreadMonitor
    {
        private Thread polling_thread;

        private bool is_running = true;

        public ThreadMonitor()
        {
        }

        public void Start()
        {
            // Create thread handle receive data
            polling_thread = new Thread(PollingHandler);
            // Thread run background
            polling_thread.IsBackground = false;
            // Begin thread
            polling_thread.Start();
        }

        private void PollingHandler()
        {
            while (is_running)
            {
                try
                {
                    // Write log
                    using (var db = new TCTTranferEntities())
                    {
                        var tivan = db.TIVans.Where(o => o.Status == false).FirstOrDefault();
                        if (tivan != null)
                        {
                            // Re-post
                            bool res = XMLHelper.HandlMessageError(tivan.DataXML);
                            if (res == true)
                            {
                                // Check status
                                tivan.Status = res;

                                // Save database.
                                db.SaveChanges();
                            }
                        }
                    }

                    Thread.Sleep(60 * 1000);
                }
                catch (Exception ex)
                {
                    GPSFileLog.WriteLog(string.Empty, ex);
                }
            }
        }

        public void Dispose()
        {
            try
            {
                is_running = false;

                polling_thread.Abort();
            }
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();
            }
        }
    }
}