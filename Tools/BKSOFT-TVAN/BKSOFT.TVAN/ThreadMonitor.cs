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

        private string DomainApi = string.Empty;

        private Thread polling_thread;

        private bool is_running = true;

        public ThreadMonitor()
        {
        }

        public void Start()
        {
            DomainApi = ConfigurationManager.AppSettings["UrlAPI"];
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
                            string xML = tivan.DataXML;

                            // Get Thông tin chung.
                            TTChung info = GetTTChungFromXML(xML);
                            if (string.IsNullOrEmpty(info.MTDiep))
                            {
                                continue;
                            }

                            // Re-Push To Server
                            Task<bool> task = HTTPHelper.TCTPostData(this.DomainApi, Utilities.Base64Encode(xML), info.MTDTChieu, info.MST);
                            task.Wait();

                            // Check status
                            tivan.Status = task.Result;

                            // Save database.
                            db.SaveChanges();
                        }
                    }

                    Thread.Sleep(60 * 1000);
                }
                catch (Exception ex)
                {
                    GPSFileLog.WriteLog(string.Empty, ex);
                }
            }

            Thread.Sleep(500);
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