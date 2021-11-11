using DLL;
using DLL.Entity;
using Services.Helper.XmlModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Services.Helper
{
    public static class TransferLogHelper
    {
        public static async Task<bool> AddTransferLogReceiveAsync(this Datacontext db, ThongDiepPhanHoiParams data)
        {
            bool res = false;
            try
            {
                if (data == null)
                {
                    return res;
                }

                TransferLog log = new TransferLog
                {
                    TransferLogId = Guid.NewGuid().ToString(),
                    DateTime = DateTime.Now,
                    MLTDiep = data.MLTDiep,
                    MTDiep = data.MTDiep,
                    MTDTChieu = data.MTDTChieu,
                    Type = 2,
                    XMLData = data.DataXML
                };

                // Save database.
                await db.TransferLogs.AddAsync(log);
                await db.SaveChangesAsync();

                res = true;
            }
            catch (Exception ex)
            {
                Tracert.WriteLog(string.Empty, ex);
            }

            return res;
        }

        public static async Task<bool> AddTransferLogSendAsync(this Datacontext db, ThongDiepPhanHoiParams data)
        {
            bool res = false;
            try
            {
                if (data == null)
                {
                    return res;
                }

                TransferLog log = new TransferLog
                {
                    TransferLogId = Guid.NewGuid().ToString(),
                    DateTime = DateTime.Now,
                    MLTDiep = data.MLTDiep,
                    MTDiep = data.MTDiep,
                    MTDTChieu = data.MTDTChieu,
                    Type = 1,
                    XMLData = data.DataXML
                };

                // Save database.
                await db.TransferLogs.AddAsync(log);
                await db.SaveChangesAsync();

                res = true;
            }
            catch (Exception ex)
            {
                Tracert.WriteLog(string.Empty, ex);
            }

            return res;
        }

        public static async Task<bool> AddTransferLog(this Datacontext db, string dataXML, int type = 1)
        {
            bool res = false;
            try
            {
                if (string.IsNullOrEmpty(dataXML))
                {
                    return res;
                }

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(dataXML);

                TransferLog log = new TransferLog
                {
                    TransferLogId = Guid.NewGuid().ToString(),
                    DateTime = DateTime.Now,
                    XMLData = dataXML
                };

                // MNGui
                XmlNodeList elemList = doc.GetElementsByTagName("MNGui");
                log.MNGui = elemList?[0].InnerXml;

                // MNNhan
                elemList = doc.GetElementsByTagName("MNNhan");
                log.MNNhan = elemList?[0].InnerXml;

                // MLTDiep
                elemList = doc.GetElementsByTagName("MLTDiep");
                if (elemList != null)
                {
                    log.MLTDiep = Convert.ToInt32(elemList?[0].InnerXml);
                }

                // MTDiep
                elemList = doc.GetElementsByTagName("MTDiep");
                log.MTDiep = elemList?[0].InnerXml;

                // MTDTChieu
                elemList = doc.GetElementsByTagName("MTDTChieu");
                log.MTDTChieu = elemList?[0].InnerXml;

                // Save database.
                await db.TransferLogs.AddAsync(log);
                await db.SaveChangesAsync();

                res = true;
            }
            catch (Exception ex)
            {
                Tracert.WriteLog(dataXML, ex);
            }

            return res;
        }
    }
}
