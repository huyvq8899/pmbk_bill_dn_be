using DLL;
using DLL.Entity;
using Microsoft.AspNetCore.Http;
using Services.Helper.XmlModel;
using Services.ViewModels.QuyDinhKyThuat;
using Services.ViewModels.XML;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6;
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
using System.Xml.XPath;

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
        public static async Task<bool> AddTransferLog(this Datacontext db, string dataXML, int type = 1, string idThongDiep = null, string idThongDiepThamChieu = null, int? MaLoaiThongDiep = null)
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
                    XMLData = dataXML
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

        /// <summary>
        /// Đọc mã giao dịch điện tử từ file xml
        /// </summary>
        /// <param name="dataXml"></param>
        /// <param name="MaLoaiThongDiep"></param>
        /// <returns></returns>
        public static string GetMGDDTuToXml(string dataXml, int MaLoaiThongDiep)
        {
            string MGDDTu = "";
            if (MaLoaiThongDiep == (int)MLTDiep.TBTNToKhai || MaLoaiThongDiep == (int)MLTDiep.TBCNToKhaiUN || MaLoaiThongDiep == (int)MLTDiep.TDTBKQKTDLHDon || MaLoaiThongDiep == (int)MLTDiep.TBTNVKQXLHDDTSSot)
            {
                try
                {
                    if (!string.IsNullOrEmpty(dataXml))
                    {
                        XDocument docx = XDocument.Parse(dataXml);
                        MGDDTu = docx.XPathSelectElement("/TDiep/DLieu/TBao/DLTBao/MGDDTu") != null ? docx.XPathSelectElement("/TDiep/DLieu/TBao/DLTBao/MGDDTu").Value : "";
                    }
                }
                catch (Exception ex)
                {
                    MGDDTu = "";
                }
            }

            return MGDDTu;
        }

        /// <summary>
        /// Lấy giá trị của thẻ từ file XML
        /// </summary>
        /// <param name="dataXml"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static string GetValueTagToXml(string dataXml, string tag)
        {
            XDocument docx = XDocument.Parse(dataXml);
            return docx.XPathSelectElement(tag) != null ? docx.XPathSelectElement(tag).Value : "";
        }

        /// <summary>
        /// Lấy thông tin thông báo từ thẻ XML
        /// </summary>
        /// <param name="dataXml"></param>
        /// <param name="MaLoaiThongDiep"></param>
        /// <returns></returns>
        public static ThongTinThongBao GetThongTinThongBao(string dataXml, int MaLoaiThongDiep)
        {
            try
            {
                //bool checkMGDDTu = false;
                bool checkThongBao = false;
                //if (MaLoaiThongDiep == (int)MLTDiep.TBTNToKhai || MaLoaiThongDiep == (int)MLTDiep.TBCNToKhaiUN || MaLoaiThongDiep == (int)MLTDiep.TDTBKQKTDLHDon || MaLoaiThongDiep == (int)MLTDiep.TBTNVKQXLHDDTSSot)
                //{
                //    checkMGDDTu = true;
                //}
                /// List các loại thông điệp nhận có mẫu số thông báo, số thông báo, ngày thông báo
                List<int> loaiThongBao = new List<int>() { 102, 103, 104, 105, 204, 205, 301, 302 };
                if (loaiThongBao.Contains(MaLoaiThongDiep))
                {
                    checkThongBao = true;
                }
                var result = new ThongTinThongBao();

                if (checkThongBao)
                {
                    result.MGDDTu = GetValueTagToXml(dataXml, "/TDiep/DLieu/TBao/DLTBao/MGDDTu");
                    result.MSoTBao = GetValueTagToXml(dataXml, "/TDiep/DLieu/TBao/DLTBao/MSo");
                    if (MaLoaiThongDiep == 102 || MaLoaiThongDiep == 204)
                    {
                        result.SoTBao = GetValueTagToXml(dataXml, "/TDiep/DLieu/TBao/DLTBao/So");
                        var ngayThongBao = GetValueTagToXml(dataXml, "/TDiep/DLieu/TBao/DLTBao/NTBao");
                        result.NgayTBao = DateTime.Parse(ngayThongBao);
                    }
                    else
                    {
                        result.SoTBao = GetValueTagToXml(dataXml, "/TDiep/DLieu/TBao/STBao/So");
                        var ngayThongBao = GetValueTagToXml(dataXml, "/TDiep/DLieu/TBao/STBao/NTBao");
                        result.NgayTBao = DateTime.Parse(ngayThongBao);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Tracert.WriteLog(ex.Message);
            }

            return null;
        }
    }
}
