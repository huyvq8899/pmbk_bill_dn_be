using DLL;
using DLL.Entity;
using Services.Helper.XmlModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
    }
}
