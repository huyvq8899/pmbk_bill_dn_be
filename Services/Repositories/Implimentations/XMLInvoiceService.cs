using DLL;
using DLL.Constants;
using DLL.Entity.DanhMuc;
using DLL.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Services.Helper;
using Services.Repositories.Interfaces;
using Services.ViewModels.QuanLyHoaDonDienTu;
using Services.ViewModels.XML.HoaDonDienTu;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Services.Repositories.Implimentations
{
    public class XMLInvoiceService : IXMLInvoiceService
    {
        private readonly Datacontext _dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;

        public XMLInvoiceService(Datacontext dataContext, IHttpContextAccessor httpContextAccessor, IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _dataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        public async Task<bool> CreateXMLInvoice(string xmlFilePath, HoaDonDienTuViewModel model)
        {
            try
            {
                string linkSearch = _configuration["Config:LinkSearchInvoice"];
                var taxCode = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.TAX_CODE)?.Value;

                LoaiTien loaiTien = await _dataContext.LoaiTiens.AsNoTracking().FirstOrDefaultAsync(x => x.LoaiTienId == model.LoaiTienId);
                var hoSoHDDT = await _dataContext.HoSoHDDTs.AsNoTracking().FirstOrDefaultAsync();
                if (hoSoHDDT == null)
                {
                    hoSoHDDT = new HoSoHDDT { MaSoThue = taxCode };
                }

                //TTChung
                TTChung _ttChung = new TTChung
                {
                    PBan = "1.1.0",
                    THDon = ((LoaiHoaDon)model.LoaiHoaDon).GetDescription(),
                    KHMSHDon = model.MauSo,
                    KHHDon = model.KyHieu,
                    SHDon = model.SoHoaDon,
                    NLap = model.NgayHoaDon.Value.ToString("yyyy-MM-dd"),
                    DVTTe = loaiTien != null ? loaiTien.Ma : string.Empty,
                    TGia = model.TyGia.Value,
                    TTNCC = "",
                    DDTCuu = linkSearch,
                    MTCuu = model.MaTraCuu,
                    HTTToan = 1,
                    THTTTKhac = string.Empty
                };
                #region NDHDon
                //NBan

                NBan _nBan = new NBan
                {
                    Ten = hoSoHDDT.TenDonVi,
                    MST = hoSoHDDT.MaSoThue,
                    DChi = hoSoHDDT.DiaChi,
                    SDThoai = hoSoHDDT.SoDienThoaiLienHe,
                    DCTDTu = hoSoHDDT.EmailLienHe,
                    STKNHang = hoSoHDDT.SoTaiKhoanNganHang,
                    TNHang = hoSoHDDT.TenNganHang,
                    Fax = hoSoHDDT.Fax,
                    Website = hoSoHDDT.Website,
                };
                //NMua
                NMua _nMua = new NMua
                {
                    Ten = model.TenKhachHang,
                    MST = model.MaSoThue,
                    DChi = model.DiaChi,
                    SDThoai = model.SoDienThoaiNguoiMuaHang,
                    DCTDTu = model.EmailNguoiMuaHang,
                    HVTNMHang = model.HoTenNguoiMuaHang,
                    STKNHang = model.SoTaiKhoanNganHang,
                };

                //HHDVus
                List<HHDVu> _dSHHDVu = new List<HHDVu>();
                int _STT = 1;
                foreach (var item in model.HoaDonChiTiets)
                {
                    HHDVu _hhdv = new HHDVu
                    {
                        STT = _STT,
                        TChat = 1,
                        THHDVu = item.TenHang,
                        DVTinh = item.DonViTinh != null ? item.DonViTinh.Ten : string.Empty,
                        SLuong = item.SoLuong.Value,
                        DGia = item.DonGia.Value,
                        ThTien = item.ThanhTien.Value,
                        TSuat = item.ThueGTGT,
                    };
                    _STT += 1;
                    _dSHHDVu.Add(_hhdv);
                }

                //TToan
                List<LTSuat> listTLSuat = new List<LTSuat>();
                LTSuat tLSuat = new LTSuat
                {
                    TSuat = model.HoaDonChiTiets.Count == 0 ? "" : model.HoaDonChiTiets.FirstOrDefault().ThueGTGT,
                    ThTien = model.TongTienHangQuyDoi.Value,
                    TThue = model.TongTienThueGTGTQuyDoi.Value
                };
                listTLSuat.Add(tLSuat);

                TToan _TToan = new TToan
                {
                    TgTCThue = model.TongTienHangQuyDoi.Value,
                    TgTThue = model.TongTienThueGTGTQuyDoi.Value,
                    TgTTTBSo = model.TongTienThanhToanQuyDoi.Value,
                    TgTTTBChu = model.SoTienBangChu,
                    THTTLTSuat = listTLSuat
                };

                NDHDon _nDHDon = new NDHDon
                {
                    NBan = _nBan,
                    NMua = _nMua,
                    DSHHDVu = _dSHHDVu,
                    TToan = _TToan,
                };

                ///

                HDon _hDon = new HDon();
                _hDon.DLHDon = new DLHDon
                {
                    TTChung = _ttChung,
                    NDHDon = _nDHDon,
                };
                _hDon.DSCKS = new DSCKS
                {
                    NBan = "",
                    NMua = "",
                };
                #endregion

                GenerateBillXML2(_hDon, xmlFilePath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> CreateXMLBienBan(string xmlFilePath, BienBanXoaBoViewModel model)
        {
            try
            {
                string linkSearch = _configuration["Config:LinkSearchInvoice"];
                var taxCode = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.TAX_CODE)?.Value;

                LoaiTien loaiTien = await _dataContext.LoaiTiens.AsNoTracking().FirstOrDefaultAsync(x => x.LoaiTienId == model.HoaDonDienTu.LoaiTienId);
                var hoSoHDDT = await _dataContext.HoSoHDDTs.AsNoTracking().FirstOrDefaultAsync();
                if (hoSoHDDT == null)
                {
                    hoSoHDDT = new HoSoHDDT { MaSoThue = taxCode };
                }

                //TTChung
                TTChung _ttChung = new TTChung
                {
                    PBan = "1.1.0",
                    THDon = ((LoaiHoaDon)model.HoaDonDienTu.LoaiHoaDon).GetDescription(),
                    KHMSHDon = model.HoaDonDienTu.MauSo,
                    KHHDon = model.HoaDonDienTu.KyHieu,
                    SHDon = model.HoaDonDienTu.SoHoaDon,
                    NLap = model.HoaDonDienTu.NgayHoaDon.Value.ToString("yyyy-MM-dd"),
                    DVTTe = loaiTien != null ? loaiTien.Ma : string.Empty,
                    TGia = model.HoaDonDienTu.TyGia.Value,
                    TTNCC = "",
                    DDTCuu = linkSearch,
                    MTCuu = model.HoaDonDienTu.MaTraCuu,
                    HTTToan = 1,
                    THTTTKhac = string.Empty
                };
                #region NDHDon
                //NBan

                NBan _nBan = new NBan
                {
                    Ten = hoSoHDDT.TenDonVi,
                    MST = hoSoHDDT.MaSoThue,
                    DChi = hoSoHDDT.DiaChi,
                    SDThoai = hoSoHDDT.SoDienThoaiLienHe,
                    DCTDTu = hoSoHDDT.EmailLienHe,
                    STKNHang = hoSoHDDT.SoTaiKhoanNganHang,
                    TNHang = hoSoHDDT.TenNganHang,
                    Fax = hoSoHDDT.Fax,
                    Website = hoSoHDDT.Website,
                };
                //NMua
                NMua _nMua = new NMua
                {
                    Ten = model.HoaDonDienTu.TenKhachHang,
                    MST = model.HoaDonDienTu.MaSoThue,
                    DChi = model.HoaDonDienTu.DiaChi,
                    SDThoai = model.HoaDonDienTu.SoDienThoaiNguoiMuaHang,
                    DCTDTu = model.HoaDonDienTu.EmailNguoiMuaHang,
                    HVTNMHang = model.HoaDonDienTu.HoTenNguoiMuaHang,
                    STKNHang = model.HoaDonDienTu.SoTaiKhoanNganHang,
                };

                //HHDVus
                List<HHDVu> _dSHHDVu = new List<HHDVu>();
                int _STT = 1;
                foreach (var item in model.HoaDonDienTu.HoaDonChiTiets)
                {
                    HHDVu _hhdv = new HHDVu
                    {
                        STT = _STT,
                        TChat = 1,
                        THHDVu = item.TenHang,
                        DVTinh = item.DonViTinh != null ? item.DonViTinh.Ten : string.Empty,
                        SLuong = item.SoLuong.Value,
                        DGia = item.DonGia.Value,
                        ThTien = item.ThanhTien.Value,
                        TSuat = item.ThueGTGT,
                    };
                    _STT += 1;
                    _dSHHDVu.Add(_hhdv);
                }

                //TToan
                List<LTSuat> listTLSuat = new List<LTSuat>();
                LTSuat tLSuat = new LTSuat
                {
                    TSuat = model.HoaDonDienTu.HoaDonChiTiets.FirstOrDefault().ThueGTGT,
                    ThTien = model.HoaDonDienTu.TongTienHangQuyDoi.Value,
                    TThue = model.HoaDonDienTu.TongTienThueGTGTQuyDoi.Value
                };
                listTLSuat.Add(tLSuat);

                TToan _TToan = new TToan
                {
                    TgTCThue = model.HoaDonDienTu.TongTienHangQuyDoi.Value,
                    TgTThue = model.HoaDonDienTu.TongTienThueGTGTQuyDoi.Value,
                    TgTTTBSo = model.HoaDonDienTu.TongTienThanhToanQuyDoi.Value,
                    TgTTTBChu = model.HoaDonDienTu.SoTienBangChu,
                    THTTLTSuat = listTLSuat
                };

                NDHDon _nDHDon = new NDHDon
                {
                    NBan = _nBan,
                    NMua = _nMua,
                    DSHHDVu = _dSHHDVu,
                    TToan = _TToan,
                };

                ///
                TTBienBan _ttBienBan = new TTBienBan
                {
                    PBan = "1.1.0",
                    NgayBienBan = model.NgayBienBan,
                    SoBienBan = model.SoBienBan,
                    ThongTu = model.ThongTu,
                    MaSoThueBenA = model.MaSoThueBenA,
                    SoDienThoaiBenA = model.SoDienThoaiBenA,
                    TenCongTyBenA = model.TenCongTyBenA,
                    DiaChiBenA = model.DiaChiBenA,
                    DaiDienBenA = model.DaiDienBenA,
                    ChucVuBenA = model.ChucVuBenA,
                    TenKhachHang = model.TenKhachHang,
                    MaSoThue = model.MaSoThue,
                    DiaChi = model.DiaChi,
                    ChucVu = model.ChucVu,
                    DaiDien = model.DaiDien,
                    SoDienThoai = model.SoDienThoai
                };

                BBHuy _hDon = new BBHuy();
                _hDon.DLTTBienBan = _ttBienBan;
                _hDon.DLHDon = new DLHDon
                {
                    TTChung = _ttChung,
                    NDHDon = _nDHDon,
                };
                _hDon.DSCKS = new DSCKS
                {
                    NBan = "",
                    NMua = "",
                };
                #endregion

                GenerateBillXML2(_hDon, xmlFilePath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Chuyển đổi object to xml. Hide null value propety of object
        /// </summary>
        /// <typeparam name="T">Template class</typeparam>
        /// <param name="obj">Object to convert xml</param>
        /// <returns>string xml</returns>
        public string ConvertToXML<T>(T obj)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode root = doc.CreateNode(XmlNodeType.Element, obj.GetType().Name, string.Empty);
            doc.AppendChild(root);
            XmlNode childNode;

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            foreach (PropertyDescriptor prop in properties)
            {
                if (prop.GetValue(obj) != null)
                {
                    childNode = doc.CreateNode(XmlNodeType.Element, prop.Name, string.Empty);

                    //// Check type value
                    //var type = obj.GetType();
                    //if (type == typeof(string))
                    //{

                    //}
                    //else if(type == typeof(decimal))
                    //{

                    //}
                    //else if (type == typeof(DateTime))
                    //{

                    //}

                    childNode.InnerText = prop.GetValue(obj).ToString();
                    root.AppendChild(childNode);
                }
            }

            return doc.OuterXml;
        }

        public string CreateFileXML<T>(T obj, string folderName)
        {
            string fileName = $"{Guid.NewGuid().ToString().Replace("-","")}.xml";
            var databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.HoaDonDienTu);
            string assetsFolder = $"FilesUpload/{databaseName}/{loaiNghiepVu}/{folderName}";
            var fullXmlFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);
            #region create folder
            if (!Directory.Exists(fullXmlFolder))
            {
                Directory.CreateDirectory(fullXmlFolder);
            }
            else
            {
                string[] files = Directory.GetFiles(fullXmlFolder);
                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }
            #endregion
            var fullXMLFile = Path.Combine(fullXmlFolder, fileName);
            var xmlContent = ConvertToXML(obj);
            File.WriteAllText(fullXMLFile, xmlContent);
            return fileName;
        }

        private void GenerateBillXML2(HDon data, string path)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            XmlSerializer serialiser = new XmlSerializer(typeof(HDon));

            using (TextWriter filestream = new StreamWriter(path))
            {
                serialiser.Serialize(filestream, data, ns);
            }
        }

        private void GenerateBillXML2(BBHuy data, string path)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            XmlSerializer serialiser = new XmlSerializer(typeof(BBHuy));

            using (TextWriter filestream = new StreamWriter(path))
            {
                serialiser.Serialize(filestream, data, ns);
            }
        }
    }
}
