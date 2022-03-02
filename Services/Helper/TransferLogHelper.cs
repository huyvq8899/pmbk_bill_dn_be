using DLL;
using DLL.Entity;
using Microsoft.AspNetCore.Http;
using Services.Helper.XmlModel;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

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

                TTChungThongDiep info = new TTChungThongDiep();

                // Get Thông tin chung
                byte[] bytes = Encoding.UTF8.GetBytes(dataXML);
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    using (StreamReader reader = new StreamReader(ms))
                    {
                        XDocument xDoc = XDocument.Load(reader);
                        info = xDoc.Descendants("TTChung")
                                       .Select(x => new TTChungThongDiep
                                       {
                                           PBan = x.Element(nameof(info.PBan)).Value,
                                           MNGui = x.Element(nameof(info.MNGui)).Value,
                                           MNNhan = x.Element(nameof(info.MNNhan)).Value,
                                           MLTDiep = x.Element(nameof(info.MLTDiep)).Value,
                                           MTDiep = x.Element(nameof(info.MTDiep)).Value,
                                           MTDTChieu = x.Element(nameof(info.MTDTChieu))?.Value,
                                           MST = x.Element(nameof(info.MST)).Value
                                       }).FirstOrDefault();
                    }
                }

                TransferLog log = new TransferLog
                {
                    TransferLogId = Guid.NewGuid().ToString(),
                    DateTime = DateTime.Now,
                    Type = type,
                    MNGui = info.MNGui,
                    MNNhan = info.MNNhan,
                    MLTDiep = Convert.ToInt32(info.MLTDiep),
                    MTDiep = info.MTDiep,
                    MTDTChieu = info.MTDTChieu,
                    XMLData = dataXML
                };

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
