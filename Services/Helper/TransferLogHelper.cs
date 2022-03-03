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

                // Get Thông tin chung
                QC1450.TDiep tdiep = XmlHelper.DeserializeStringToObject<QC1450.TDiep>(dataXML);

                // Save to database.
                TransferLog log = new TransferLog
                {
                    TransferLogId = Guid.NewGuid().ToString(),
                    DateTime = DateTime.Now,
                    Type = type,
                    MNGui = (tdiep.TTChung).MNGui,
                    MNNhan = (tdiep.TTChung).MNNhan,
                    MLTDiep = Convert.ToInt32((tdiep.TTChung).MLTDiep),
                    MTDiep = (tdiep.TTChung).MTDiep,
                    MTDTChieu = (tdiep.TTChung).MTDTChieu,
                    DataXML = dataXML
                };

                // Save database.
                await db.TransferLogs.AddAsync(log);
                int val = await db.SaveChangesAsync();
                if(val > 0)
                {
                    res = true;
                }    
            }
            catch (Exception ex)
            {
                Tracert.WriteLog(dataXML, ex);
            }

            return res;
        }
    }
}
