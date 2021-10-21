using DLL;
using DLL.Constants;
using DLL.Entity.DanhMuc;
using DLL.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Services.Enums;
using Services.Helper;
using Services.Repositories.Interfaces;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.QuanLyHoaDonDienTu;
using Services.ViewModels.QuyDinhKyThuat;
using Services.ViewModels.XML;
using Services.ViewModels.XML.HoaDonDienTu;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

/// Hóa đơn giá trị gia tăng
using HDonGTGT = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.HDon;

namespace Services.Repositories.Implimentations
{
    public class XMLInvoiceService : IXMLInvoiceService
    {
        private readonly Datacontext _dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IHoSoHDDTService _hoSoHDDTService;

        public XMLInvoiceService(
            Datacontext dataContext,
            IHttpContextAccessor httpContextAccessor,
            IHostingEnvironment hostingEnvironment,
            IConfiguration configuration,
            IHoSoHDDTService hoSoHDDTService)
        {
            _dataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _hoSoHDDTService = hoSoHDDTService;
        }

        public async Task<bool> CreateXMLInvoice(string xmlFilePath, HoaDonDienTuViewModel model)
        {
            try
            {
                if (model.MauHoaDon != null)
                {
                    switch (model.MauHoaDon.QuyDinhApDung)
                    {
                        case QuyDinhApDung.ND512010TT322021:
                            await CreateInvoiceND51TT32Async(xmlFilePath, model);
                            break;
                        case QuyDinhApDung.ND1232020TT782021:
                            await CreateInvoiceND123TT78Async(xmlFilePath, model);
                            break;
                        default:
                            break;
                    }

                    return true;
                }

                return false;
            }
            catch
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
            string fileName = $"{Guid.NewGuid().ToString().Replace("-", "")}.xml";
            string assetsFolder = $"FilesUpload/QuyDinhKyThuat/{folderName}/unsigned/";
            var fullXmlFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);
            #region create folder
            if (!Directory.Exists(fullXmlFolder))
            {
                Directory.CreateDirectory(fullXmlFolder);
            }
            #endregion
            var fullXMLFile = Path.Combine(fullXmlFolder, fileName);
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            XmlSerializer serialiser = new XmlSerializer(typeof(T));

            using (TextWriter filestream = new StreamWriter(fullXMLFile))
            {
                serialiser.Serialize(filestream, obj, ns);
            }
            return fileName;
        }

        public void CreateQuyDinhKyThuat_PhanII_II_7(string xmlFilePath, ThongDiepChungViewModel model)
        {
            ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._7.TDiep tDiep = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._7.TDiep
            {
                TTChung = new ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities.TTChungThongDiep
                {
                    PBan = model.PhienBan,
                    MNGui = model.MaNoiGui,
                    MNNhan = model.MaNoiNhan,
                    MLTDiep = model.MaLoaiThongDiep.ToString(),
                    MTDiep = model.MaThongDiep,
                    MTDTChieu = model.MaThongDiepThamChieu,
                    MST = model.MaSoThue,
                    SLuong = model.SoLuong,
                },
            };

            GenerateXML(tDiep, xmlFilePath);

            XmlDocument xml = new XmlDocument();
            xml.Load(xmlFilePath);
            xml.DocumentElement.AppendChild(xml.CreateElement(nameof(tDiep.DLieu)));

            var databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.HoaDonDienTu);
            string folderPath = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/{databaseName}/{loaiNghiepVu}");
            foreach (var item in model.DuLieuGuiHDDT.DuLieuGuiHDDTChiTiets)
            {
                string filePath = Path.Combine(folderPath, item.HoaDonDienTuId, $"xml/signed/{item.HoaDonDienTu.XMLDaKy}");

                if (File.Exists(filePath))
                {
                    XmlDocument signedXML = new XmlDocument();
                    signedXML.Load(filePath);

                    var importNode = xml.ImportNode(signedXML.DocumentElement, true);
                    xml.DocumentElement[nameof(tDiep.DLieu)].AppendChild(importNode);
                }
            }

            xml.Save(xmlFilePath);
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

        public void GenerateXML<T>(T data, string path)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            XmlSerializer serialiser = new XmlSerializer(typeof(T));

            using (TextWriter filestream = new StreamWriter(path))
            {
                serialiser.Serialize(filestream, data, ns);
            }

            // remove null value
            XDocument xd = XDocument.Load(path);
            GetRemoveElement(xd).Remove();
            xd.Save(path);
        }

        private IEnumerable<XElement> GetRemoveElement(XDocument xd)
        {
            foreach (var item in xd.Descendants())
            {
                if (item.Name.LocalName != "DSCKS" && item.Name.LocalName != "NBan" && (item.IsEmpty || string.IsNullOrWhiteSpace(item.Value) || string.IsNullOrEmpty(item.Value)))
                {
                    yield return item;
                }
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

        private async Task CreateInvoiceND51TT32Async(string xmlFilePath, HoaDonDienTuViewModel model)
        {
            string linkSearch = _configuration["Config:LinkSearchInvoice"];
            var taxCode = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.TAX_CODE)?.Value;

            LoaiTien loaiTien = await _dataContext.LoaiTiens.AsNoTracking().FirstOrDefaultAsync(x => x.LoaiTienId == model.LoaiTienId);
            var hoSoHDDT = await _hoSoHDDTService.GetDetailAsync();

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
        }

        private async Task CreateInvoiceND123TT78Async(string xmlFilePath, HoaDonDienTuViewModel model)
        {
            string pBien = "2.0.0";
            string taxCode = _configuration["Config:TaxCode"];
            var hoSoHDDT = await _hoSoHDDTService.GetDetailAsync();

            switch ((LoaiHoaDon)model.LoaiHoaDon)
            {
                case LoaiHoaDon.HoaDonGTGT:
                    HDonGTGT hDonGTGT = new HDonGTGT
                    {
                        DLHDon = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.DLHDon
                        {
                            TTChung = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTChung
                            {
                                PBan = pBien,
                                THDon = LoaiHoaDon.HoaDonGTGT.GetDescription().ToUpper(),
                                KHMSHDon = model.MauSo,
                                KHHDon = model.KyHieu,
                                SHDon = model.SoHoaDon,
                                NLap = model.NgayHoaDon.Value.ToString("yyyy-MM-dd"),
                                DVTTe = model.MaLoaiTien,
                                TGia = model.IsVND == true ? null : model.TyGia,
                                HTTToan = model.HinhThucThanhToan?.Ten ?? string.Empty,
                                MSTTCGP = taxCode,
                                TTHDLQuan = null,
                            },
                            NDHDon = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.NDHDon
                            {
                                NBan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.NBan
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
                                },
                                NMua = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.NMua
                                {
                                    Ten = model.TenKhachHang,
                                    MST = model.MaSoThue,
                                    DChi = model.DiaChi,
                                    MKHang = model.MaKhachHang,
                                    SDThoai = model.SoDienThoaiNguoiMuaHang,
                                    DCTDTu = model.EmailNguoiMuaHang,
                                    HVTNMHang = model.HoTenNguoiMuaHang,
                                    STKNHang = model.SoTaiKhoanNganHang,
                                    TNHang = model.TenNganHang
                                },
                                DSHHDVu = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.HHDVu>(),
                                TToan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TToan
                                {
                                    TgTCThue = model.TongTienHangQuyDoi ?? 0,
                                    TgTThue = model.TongTienThueGTGTQuyDoi ?? 0,
                                    TTCKTMai = model.TongTienChietKhauQuyDoi,
                                    TgTTTBSo = model.TongTienThanhToanQuyDoi ?? 0,
                                    TgTTTBChu = model.SoTienBangChu,
                                    THTTLTSuat = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.THTTLTSuat
                                    {
                                        LTSuat = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.LTSuat>()
                                    }
                                }
                            }
                        },
                        DSCKS = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.DSCKS
                        {
                            NBan = "",
                            NMua = "",
                        }
                    };

                    #region Nếu là thay thế/điều chỉnh
                    if ((model.TrangThai == (int)TrangThaiHoaDon.HoaDonThayThe) || (model.TrangThai == (int)TrangThaiHoaDon.HoaDonDieuChinh))
                    {
                        hDonGTGT.DLHDon.TTChung.TTHDLQuan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTHDLQuan
                        {
                            LHDCLQuan = LADHDDT.HinhThuc1
                        };

                        if (model.TrangThai == (int)TrangThaiHoaDon.HoaDonThayThe)
                        {
                            hDonGTGT.DLHDon.TTChung.TTHDLQuan.TCHDon = TCHDon.ThayThe;
                            hDonGTGT.DLHDon.TTChung.TTHDLQuan.KHMSHDCLQuan = model.LyDoThayTheModel.MauSo;
                            hDonGTGT.DLHDon.TTChung.TTHDLQuan.KHHDCLQuan = model.LyDoThayTheModel.KyHieu;
                            hDonGTGT.DLHDon.TTChung.TTHDLQuan.SHDCLQuan = model.LyDoThayTheModel.SoHoaDon;
                            hDonGTGT.DLHDon.TTChung.TTHDLQuan.NLHDCLQuan = model.LyDoThayTheModel.NgayHoaDon.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            hDonGTGT.DLHDon.TTChung.TTHDLQuan.TCHDon = TCHDon.DieuChinh;
                            hDonGTGT.DLHDon.TTChung.TTHDLQuan.KHMSHDCLQuan = model.LyDoDieuChinhModel.MauSo;
                            hDonGTGT.DLHDon.TTChung.TTHDLQuan.KHHDCLQuan = model.LyDoDieuChinhModel.KyHieu;
                            hDonGTGT.DLHDon.TTChung.TTHDLQuan.SHDCLQuan = model.LyDoDieuChinhModel.SoHoaDon;
                            hDonGTGT.DLHDon.TTChung.TTHDLQuan.NLHDCLQuan = model.LyDoDieuChinhModel.NgayHoaDon.ToString("yyyy-MM-dd");
                        }
                    }

                    #endregion

                    #region Hàng hóa chi tiết
                    int stt = 0;
                    foreach (var item in model.HoaDonChiTiets)
                    {
                        if (item.HangKhuyenMai != true)
                        {
                            stt += 1;
                        }

                        hDonGTGT.DLHDon.NDHDon.DSHHDVu.Add(new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.HHDVu
                        {
                            TChat = item.HangKhuyenMai == true ? TChat.KhuyenMai : TChat.HangHoaDichVu,
                            STT = stt,
                            MHHDVu = item.MaHang,
                            THHDVu = item.TenHang,
                            DVTinh = item.DonViTinh?.Ten,
                            SLuong = item.SoLuong,
                            DGia = item.DonGia,
                            TLCKhau = item.TyLeChietKhau,
                            STCKhau = item.TienChietKhau,
                            ThTien = item.ThanhTien,
                            TSuat = item.ThueGTGT
                        });
                    }
                    #endregion

                    #region tổng hợp mỗi loại thuế suất
                    var groupThueGTGT = model.HoaDonChiTiets.GroupBy(x => x.ThueGTGT)
                        .Select(x => new HoaDonDienTuChiTietViewModel
                        {
                            ThueGTGT = x.Key,
                            ThanhTien = x.Sum(y => y.ThanhTien),
                            TienThueGTGT = x.Sum(y => y.TienThueGTGT)
                        })
                        .ToList();

                    foreach (var item in groupThueGTGT)
                    {
                        hDonGTGT.DLHDon.NDHDon.TToan.THTTLTSuat.LTSuat.Add(new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.LTSuat
                        {
                            TSuat = item.ThueGTGT,
                            ThTien = item.ThanhTien ?? 0,
                            TThue = item.TienThueGTGT
                        });
                    }
                    #endregion

                    GenerateXML(hDonGTGT, xmlFilePath);
                    break;
                case LoaiHoaDon.HoaDonBanHang:

                    break;
                default:
                    break;
            }
        }

        public void CreateQuyDinhKyThuat_PhanII_II_5(string xmlFilePath, ThongDiepChungViewModel model)
        {
            ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6.TDiep tDiep = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6.TDiep
            {
                TTChung = new ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities.TTChungThongDiep
                {
                    PBan = model.PhienBan,
                    MNGui = model.MaNoiGui,
                    MNNhan = model.MaNoiNhan,
                    MLTDiep = model.MaLoaiThongDiep.ToString(),
                    MTDiep = model.MaThongDiep,
                    MTDTChieu = model.MaThongDiepThamChieu,
                    MST = model.MaSoThue,
                    SLuong = model.SoLuong,
                },
            };

            GenerateXML(tDiep, xmlFilePath);

            XmlDocument xml = new XmlDocument();
            xml.Load(xmlFilePath);
            xml.DocumentElement.AppendChild(xml.CreateElement(nameof(tDiep.DLieu)));

            var databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.HoaDonDienTu);
            string folderPath = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/{databaseName}/{loaiNghiepVu}");
            string filePath = Path.Combine(folderPath, model.DuLieuGuiHDDT.HoaDonDienTuId, $"xml/signed/{model.DuLieuGuiHDDT.HoaDonDienTu.XMLDaKy}");

            if (File.Exists(filePath))
            {
                XmlDocument signedXML = new XmlDocument();
                signedXML.Load(filePath);

                var importNode = xml.ImportNode(signedXML.DocumentElement, true);
                xml.DocumentElement[nameof(tDiep.DLieu)].AppendChild(importNode);
            }

            xml.Save(xmlFilePath);
        }

        public void CreateQuyDinhKyThuatTheoMaLoaiThongDiep(string xmlFilePath, ThongDiepChungViewModel model)
        {
            switch (model.MaLoaiThongDiep)
            {
                case (int)MLTDiep.TDGHDDTTCQTCapMa:
                    CreateQuyDinhKyThuat_PhanII_II_5(xmlFilePath, model);
                    break;
                case (int)MLTDiep.TDCDLHDKMDCQThue:
                    CreateQuyDinhKyThuat_PhanII_II_7(xmlFilePath, model);
                    break;
                default:
                    break;
            }
        }
    }
}
