using DLL;
using DLL.Entity;
using Microsoft.AspNetCore.Http;
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
        /// <summary>
        /// Add log send & receive from TVAN
        /// </summary>
        /// <param name="db"></param>
        /// <param name="dataXML"></param>
        /// <param name="type"></param>
        /// <returns></returns>
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
                    Type = type,
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
