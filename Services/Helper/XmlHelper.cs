using DLL;
using DLL.Constants;
using DLL.Entity;
using DLL.Entity.QuyDinhKyThuat;
using DLL.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services.Helper;
using Services.Helper.Constants;
using Services.Helper.XmlModel;
using Services.ViewModels.XML;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Services.Helper
{
    public static class XmlHelper
    {
        public static TTChungThongDiep GetTTChungFromStringXML(string strXml)
        {
            try
            {
                TTChungThongDiep result = new TTChungThongDiep();
                byte[] encodedString = Encoding.UTF8.GetBytes(strXml);
                using (MemoryStream ms = new MemoryStream(encodedString))
                {
                    using (StreamReader reader = new StreamReader(ms))
                    {
                        XDocument xDoc = XDocument.Load(reader);
                        var res = xDoc.Descendants("TTChung").FirstOrDefault();
                        result = new TTChungThongDiep
                        {
                            PBan = res.Element(nameof(result.PBan)).Value,
                            MNGui = res.Element(nameof(result.MNGui)).Value,
                            MNNhan = res.Element(nameof(result.MNNhan)).Value,
                            MLTDiep = res.Element(nameof(result.MLTDiep)).Value,
                            MTDiep = res.Element(nameof(result.MTDiep)).Value,
                            MTDTChieu = res.Element(nameof(result.MTDTChieu)) != null ? res.Element(nameof(result.MTDTChieu)).Value : string.Empty,
                            MST = res.Element(nameof(result.MST)).Value
                        };
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static T DeserializeFileToObject<T>(string filePath) where T : class
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using (StreamReader sr = new StreamReader(filePath))
            {
                return (T)ser.Deserialize(sr);
            }
        }

        public static T DeserializeStringToObject<T>(string dataXML) where T : class
        {
            try
            {
                System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

                using (TextReader sr = new StringReader(dataXML))
                {
                    return (T)ser.Deserialize(sr);
                }
            }
            catch (Exception e)
            {
                Tracert.WriteLog("DeserializeStringToObject", e);
                Tracert.WriteLog("dataXML: " + dataXML);
            }

            return null;
        }


        //public static TTChungThongDiep GetTTChungFromBase64(string base64)
        //{
        //    TTChungThongDiep result = new TTChungThongDiep();

        //    var xmlContent = DataHelper.Base64Decode(base64);
        //    byte[] encodedString = Encoding.UTF8.GetBytes(xmlContent);
        //    MemoryStream ms = new MemoryStream(encodedString);
        //    ms.Flush();
        //    ms.Position = 0;
        //    using (StreamReader reader = new StreamReader(ms))
        //    {
        //        XDocument xDoc = XDocument.Load(reader);
        //        result = xDoc.Descendants("TTChung")
        //           .Select(x => new TTChungThongDiep
        //           {
        //               PBan = x.Element(nameof(result.PBan)).Value,
        //               MNGui = x.Element(nameof(result.MNGui)).Value,
        //               MNNhan = x.Element(nameof(result.MNNhan)).Value,
        //               MLTDiep = x.Element(nameof(result.MLTDiep)).Value,
        //               MTDiep = x.Element(nameof(result.MTDiep)).Value,
        //               MST = x.Element(nameof(result.MST)).Value,
        //           })
        //           .FirstOrDefault();
        //    }
        //    return result;
        //}



        //public static async Task<bool> InsertThongDiepNhanAsync(ThongDiepPhanHoiParams @params, Datacontext dataContext)
        //{
        //    string id = Guid.NewGuid().ToString();

        //    // Save file
        //    //string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
        //    //string folderPath = $"FilesUpload/{databaseName}/{ManageFolderPath.XML_MESSAGE}/{id}";
        //    //string fullFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, folderPath);
        //    //if (!Directory.Exists(fullFolderPath))
        //    //{
        //    //    Directory.CreateDirectory(fullFolderPath);
        //    //}

        //    switch (@params.MLTDiep)
        //    {
        //        case (int)MLTDiep.TBTNToKhai: // 102
        //            var tDiep102 = DataHelper.ConvertBase64ToObject<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._10.TDiep>(@params.DataXML);
        //            var tdc102 = new ThongDiepChung
        //            {
        //                ThongDiepChungId = id,
        //                PhienBan = tDiep102.TTChung.PBan,
        //                MaNoiGui = tDiep102.TTChung.MNGui,
        //                MaNoiNhan = tDiep102.TTChung.MNNhan,
        //                MaLoaiThongDiep = int.Parse(tDiep102.TTChung.MLTDiep),
        //                MaThongDiep = tDiep102.TTChung.MTDiep,
        //                MaThongDiepThamChieu = tDiep102.TTChung.MTDTChieu,
        //                MaSoThue = tDiep102.TTChung.MST,
        //                SoLuong = tDiep102.TTChung.SLuong,
        //                ThongDiepGuiDi = false,
        //                HinhThuc = 0,
        //                NgayThongBao = DateTime.Now,
        //            };
        //            await dataContext.ThongDiepChungs.AddAsync(tdc102);
        //            break;
        //        case (int)MLTDiep.TBCNToKhai: // 103
        //            var tDiep103 = DataHelper.ConvertBase64ToObject<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._11.TDiep>(@params.DataXML);
        //            var tdc103 = new ThongDiepChung
        //            {
        //                ThongDiepChungId = id,
        //                PhienBan = tDiep103.TTChung.PBan,
        //                MaNoiGui = tDiep103.TTChung.MNGui,
        //                MaNoiNhan = tDiep103.TTChung.MNNhan,
        //                TrangThaiGui = tDiep103.DLieu.TBao.DLTBao.TTXNCQT == TTXNCQT.ChapNhan ? 5 : 6,
        //                MaLoaiThongDiep = int.Parse(tDiep103.TTChung.MLTDiep),
        //                MaThongDiep = tDiep103.TTChung.MTDiep,
        //                MaThongDiepThamChieu = tDiep103.TTChung.MTDTChieu,
        //                MaSoThue = tDiep103.TTChung.MST,
        //                SoLuong = tDiep103.TTChung.SLuong,
        //                ThongDiepGuiDi = false,
        //                HinhThuc = 0,
        //                NgayThongBao = DateTime.Now,
        //            };
        //            await dataContext.ThongDiepChungs.AddAsync(tdc103);
        //            break;
        //        case (int)MLTDiep.TBCNToKhaiUN: // 104
        //            var tDiep104 = DataHelper.ConvertBase64ToObject<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._12.TDiep>(@params.DataXML);
        //            var tdc104 = new ThongDiepChung
        //            {
        //                ThongDiepChungId = id,
        //                PhienBan = tDiep104.TTChung.PBan,
        //                MaNoiGui = tDiep104.TTChung.MNGui,
        //                MaNoiNhan = tDiep104.TTChung.MNNhan,
        //                MaLoaiThongDiep = int.Parse(tDiep104.TTChung.MLTDiep),
        //                MaThongDiep = tDiep104.TTChung.MTDiep,
        //                MaThongDiepThamChieu = tDiep104.TTChung.MTDTChieu,
        //                MaSoThue = tDiep104.TTChung.MST,
        //                SoLuong = tDiep104.TTChung.SLuong,
        //                ThongDiepGuiDi = false,
        //                HinhThuc = 0,
        //                NgayThongBao = DateTime.Now,
        //                //FileXML = fileName
        //            };
        //            await dataContext.ThongDiepChungs.AddAsync(tdc104);
        //            break;
        //        case (int)MLTDiep.TBKQCMHDon: // 202
        //            var tDiep202 = DataHelper.ConvertBase64ToObject<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6.TDiep>(@params.DataXML);
        //            ThongDiepChung tdc202 = new ThongDiepChung
        //            {
        //                ThongDiepChungId = id,
        //                PhienBan = tDiep202.TTChung.PBan,
        //                MaNoiGui = tDiep202.TTChung.MNGui,
        //                MaNoiNhan = tDiep202.TTChung.MNNhan,
        //                MaLoaiThongDiep = int.Parse(tDiep202.TTChung.MLTDiep),
        //                MaThongDiep = tDiep202.TTChung.MTDiep,
        //                MaThongDiepThamChieu = tDiep202.TTChung.MTDTChieu,
        //                MaSoThue = tDiep202.TTChung.MST,
        //                SoLuong = tDiep202.TTChung.SLuong,
        //                ThongDiepGuiDi = false,
        //                HinhThuc = 0,
        //                NgayThongBao = DateTime.Now,
        //                // FileXML = fileName
        //            };
        //            await dataContext.ThongDiepChungs.AddAsync(tdc202);
        //            break;
        //        case (int)MLTDiep.TDTBKQKTDLHDon: // 204
        //            var tDiep204 = DataHelper.ConvertBase64ToObject<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._8.TDiep>(@params.DataXML);
        //            ThongDiepChung tdc204 = new ThongDiepChung
        //            {
        //                ThongDiepChungId = id,
        //                PhienBan = tDiep204.TTChung.PBan,
        //                MaNoiGui = tDiep204.TTChung.MNGui,
        //                MaNoiNhan = tDiep204.TTChung.MNNhan,
        //                MaLoaiThongDiep = int.Parse(tDiep204.TTChung.MLTDiep),
        //                MaThongDiep = tDiep204.TTChung.MTDiep,
        //                MaThongDiepThamChieu = tDiep204.TTChung.MTDTChieu,
        //                MaSoThue = tDiep204.TTChung.MST,
        //                SoLuong = tDiep204.TTChung.SLuong,
        //                ThongDiepGuiDi = false,
        //                HinhThuc = 0,
        //                NgayThongBao = DateTime.Now,
        //                // FileXML = fileName
        //            };
        //            await dataContext.ThongDiepChungs.AddAsync(tdc204);
        //            break;
        //        case (int)MLTDiep.TDCDLTVANUQCTQThue: // 999
        //            var tDiep999 = DataHelper.ConvertBase64ToObject<ViewModels.XML.QuyDinhKyThuatHDDT.PhanI.IV._6.TDiep>(@params.DataXML);
        //            ThongDiepChung tdc999 = new ThongDiepChung
        //            {
        //                ThongDiepChungId = id,
        //                PhienBan = tDiep999.TTChung.PBan,
        //                MaNoiGui = tDiep999.TTChung.MNGui,
        //                MaNoiNhan = tDiep999.TTChung.MNNhan,
        //                MaLoaiThongDiep = int.Parse(tDiep999.TTChung.MLTDiep),
        //                MaThongDiep = tDiep999.TTChung.MTDiep,
        //                MaThongDiepThamChieu = tDiep999.TTChung.MTDTChieu,
        //                MaSoThue = tDiep999.TTChung.MST,
        //                SoLuong = tDiep999.TTChung.SLuong,
        //                ThongDiepGuiDi = false,
        //                HinhThuc = 0,
        //                NgayThongBao = DateTime.Now,
        //                // FileXML = fileName
        //            };

        //            ThongDiepChung tdGui = await dataContext.ThongDiepChungs.FirstOrDefaultAsync(x => x.MaThongDiep == tDiep999.TTChung.MTDTChieu);
        //            if (tdGui != null)
        //            {
        //                tdGui.NgayThongBao = DateTime.Parse(tDiep999.DLieu.TBao.NNhan);
        //                tdGui.MaThongDiepPhanHoi = tDiep999.TTChung.MTDiep;
        //                tdGui.TrangThaiGui = 5;
        //            }

        //            await dataContext.ThongDiepChungs.AddAsync(tdc999);
        //            break;
        //        case (int)MLTDiep.TBTNVKQXLHDDTSSot: // 301
        //            var tDiep301 = DataHelper.ConvertBase64ToObject<ViewModels.XML.ThongDiepGuiNhanCQT.TDiepNhanHDonSaiSot.TDiep>(@params.DataXML);
        //            ThongDiepChung tdc301 = new ThongDiepChung
        //            {
        //                ThongDiepChungId = id,
        //                PhienBan = tDiep301.TTChung.PBan,
        //                MaNoiGui = tDiep301.TTChung.MNGui,
        //                MaNoiNhan = tDiep301.TTChung.MNNhan,
        //                MaLoaiThongDiep = tDiep301.TTChung.MLTDiep,
        //                MaThongDiep = tDiep301.TTChung.MTDiep,
        //                MaThongDiepThamChieu = tDiep301.TTChung.MTDTChieu,
        //                MaSoThue = tDiep301.TTChung.MST,
        //                SoLuong = tDiep301.TTChung.SLuong,
        //                ThongDiepGuiDi = false,
        //                HinhThuc = (int)HThuc.ChinhThuc,
        //                NgayThongBao = DateTime.Now,
        //                // FileXML = fileName
        //            };
        //            await dataContext.ThongDiepChungs.AddAsync(tdc301);
        //            break;
        //        case (int)MLTDiep.TDTBHDDTCRSoat: // 302
        //            var tDiep302 = DataHelper.ConvertBase64ToObject<ViewModels.XML.ThongDiepGuiNhanCQT.TDiepNhanHDonRaSoat.TDiep>(@params.DataXML);
        //            ThongDiepChung tdc302 = new ThongDiepChung
        //            {
        //                ThongDiepChungId = id,
        //                PhienBan = tDiep302.TTChung.PBan,
        //                MaNoiGui = tDiep302.TTChung.MNGui,
        //                MaNoiNhan = tDiep302.TTChung.MNNhan,
        //                MaLoaiThongDiep = tDiep302.TTChung.MLTDiep,
        //                MaThongDiep = tDiep302.TTChung.MTDiep,
        //                MaThongDiepThamChieu = tDiep302.TTChung.MTDTChieu,
        //                MaSoThue = tDiep302.TTChung.MST,
        //                SoLuong = tDiep302.TTChung.SLuong,
        //                ThongDiepGuiDi = false,
        //                HinhThuc = (int)HThuc.ChinhThuc,
        //                NgayThongBao = DateTime.Now,
        //                //FileXML = fileName
        //            };
        //            await dataContext.ThongDiepChungs.AddAsync(tdc302);
        //            break;
        //        default:
        //            break;
        //    }


        //    var fileData = new FileData
        //    {
        //        RefId = id,
        //        Type = 1,
        //        DateTime = DateTime.Now,
        //        Content = @params.DataXML,
        //        Binary = Encoding.ASCII.GetBytes(@params.DataXML),
        //    };
        //    await dataContext.FileDatas.AddAsync(fileData);

        //    var result = await dataContext.SaveChangesAsync();
        //    return result > 0;
        //}
    }
}
