using DLL;
using DLL.Constants;
using DLL.Entity.DanhMuc;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Services.Enums;
using Services.Helper;
using Services.Helper.Params.QuyDinhKyThuat;
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
using HDonBanHang = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.b.HDon;
using TDiep200GTGT = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6.TDiep;
using TDiep200BH = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6.TDiep2;
using Services.Repositories.Interfaces.QuyDinhKyThuat;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities;
using Services.Helper.Constants;
using DLL.Entity;
using Services.Repositories.Interfaces.QuanLyHoaDon;
using Services.ViewModels.QuanLy;
using Services.ViewModels.DanhMuc;
using AutoMapper;
using Newtonsoft.Json;
using System.Text;
using Formatting = System.Xml.Formatting;

namespace Services.Repositories.Implimentations
{
    public class XMLInvoiceService : IXMLInvoiceService
    {
        private readonly Datacontext _dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IHoSoHDDTService _hoSoHDDTService;
        private readonly IMapper _mp;

        public XMLInvoiceService(
            Datacontext dataContext,
            IHttpContextAccessor httpContextAccessor,
            IHostingEnvironment hostingEnvironment,
            IConfiguration configuration,
            IHoSoHDDTService hoSoHDDTService,
            IMapper mp)
        {
            _dataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _hoSoHDDTService = hoSoHDDTService;
            _mp = mp;
        }

        public async Task<bool> CreateXMLInvoice(string xmlFilePath, HoaDonDienTuViewModel model)
        {
            try
            {
                if (model.MauHoaDon != null)
                {
                    await CreateInvoiceND123TT78Async(xmlFilePath, model);
                    return true;
                }

                return false;
            }
            catch (Exception e)
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

        public string CreateFileXML<T>(T obj, string folderName, string fileName, string ThongDiepId)
        {
            var databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string assetsFolder = $"FilesUpload/{databaseName}/{folderName}";
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

            if (!string.IsNullOrEmpty(ThongDiepId))
            {
                var entityTD = _dataContext.ThongDiepChungs.FirstOrDefault(x => x.ThongDiepChungId == ThongDiepId);
                if (entityTD != null)
                {
                    var entityData = new FileData();
                    if (_dataContext.FileDatas.Any(x => x.RefId == entityTD.ThongDiepChungId && x.IsSigned == false))
                    {
                        entityData = _dataContext.FileDatas.FirstOrDefault(x => x.RefId == entityTD.ThongDiepChungId && x.IsSigned == false);
                        entityData.Content = File.ReadAllText(fullXMLFile);
                        entityData.Binary = File.ReadAllBytes(fullXMLFile);
                        _dataContext.FileDatas.Update(entityData);
                    }
                    else
                    {
                        entityData = new FileData
                        {
                            RefId = entityTD.ThongDiepChungId,
                            DateTime = DateTime.Now,
                            IsSigned = false,
                            Content = File.ReadAllText(fullXMLFile),
                            Binary = File.ReadAllBytes(fullXMLFile)
                        };
                        _dataContext.FileDatas.AddAsync(entityData);
                    }

                    _dataContext.SaveChanges();
                }
            }
            return Path.Combine(assetsFolder, fileName);
        }

        public void CreateQuyDinhKyThuat_PhanII_II_7(string xmlFilePath, ThongDiepChungViewModel model)
        {
            ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._7.TDiep tDiep = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._7.TDiep
            {
                TTChung = new TTChungThongDiep
                {
                    PBan = model.PhienBan,
                    MNGui = model.MaNoiGui,
                    MNNhan = model.MaNoiNhan,
                    MLTDiep = model.MaLoaiThongDiep.ToString(),
                    MTDiep = model.MaThongDiep,
                    MTDTChieu = model.MaThongDiepThamChieu,
                    MST = model.MaSoThue,
                    SLuong = model.SoLuong.Value,
                },
            };

            GenerateXML(tDiep, xmlFilePath);

            XmlDocument xml = new XmlDocument();
            xml.Load(xmlFilePath);
            xml.DocumentElement.AppendChild(xml.CreateElement(nameof(tDiep.DLieu)));

            var databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string folderPath = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/{databaseName}");
            foreach (var item in model.DuLieuGuiHDDT.DuLieuGuiHDDTChiTiets)
            {
                string filePath = Path.Combine(folderPath, $"{ManageFolderPath.XML_SIGNED}/{item.HoaDonDienTu.XMLDaKy}");

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

        public void CreateQuyDinhKyThuat_PhanII_IV_2(string xmlFilePath, BangTongHopDuLieuParams @params)
        {
            ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.IV._2.TDiep tDiep = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.IV._2.TDiep
            {
                TTChung = new ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities.TTChungThongDiep
                {
                    PBan = @params.TTChung1.PhienBan,
                    MNGui = @params.TTChung1.MaNoiGui,
                    MNNhan = @params.TTChung1.MaNoiNhan,
                    MLTDiep = @params.TTChung1.MaLoaiThongDiep.ToString(),
                    MTDiep = @params.TTChung1.MaThongDiep,
                    MTDTChieu = !string.IsNullOrEmpty(@params.TTChung1.MaThongDiepThamChieu) ? @params.TTChung1.MaThongDiepThamChieu : "",
                    MST = @params.TTChung1.MaSoThue,
                    SLuong = @params.TTChung1.SoLuong.Value,
                },
                DLieu = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.IV._1.BTHDLieu>()
            };


            tDiep.DLieu.Add(new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.IV._1.BTHDLieu
            {
                DLBTHop = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.IV._1.DLBTHop
                {
                    TTChung = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.IV._1.TTChung
                    {
                        PBan = @params.TTChung2.PBan,
                        MSo = @params.TTChung2.MSo,
                        Ten = @params.TTChung2.Ten,
                        SBTHDLieu = @params.TTChung2.SBTHDLieu,
                        LKDLieu = @params.TTChung2.LKDLieu,
                        KDLieu = @params.TTChung2.KDLieu,
                        LDau = @params.TTChung2.LDau,
                        BSLThu = @params.TTChung2.BSLThu,
                        NLap = @params.TTChung2.NLap,
                        MST = @params.TTChung2.MST,
                        TNNT = @params.TTChung2.TNNT,
                        HDDIn = HDDIn.HoaDonDienTu,
                        LHHoa = @params.TTChung2.LHHoa
                    },
                    NDBTHDLieu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.IV._1.NDBTHDLieu
                    {
                        DSDLieu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.IV._1.DSDLieu
                        {
                            DLieu = @params.DuLieu.Select(x => new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.IV._1.DLieu
                            {
                                STT = @params.DuLieu.IndexOf(x) + 1,
                                KHMSHDon = x.MauSo,
                                KHHDon = x.KyHieu,
                                SHDon = int.Parse(x.SoHoaDon),
                                NLap = x.NgayHoaDon.Value.ToString("yyyy-MM-dd"),
                                TNMua = x.HoTenNguoiMuaHang,
                                MKHang = x.MaKhachHang,
                                MSTNMua = x.MaSoThue,
                                MHHoa = x.MaHang,
                                THHDVu = x.TenHang,
                                DVTinh = x.DonViTinh,
                                SLuong = x.SoLuong,
                                TTCThue = x.ThanhTien,
                                TSuat = x.ThueGTGT,
                                TgTThue = x.TienThueGTGT,
                                TgTTToan = x.TongTienThanhToan,
                                TThai = x.TrangThaiHoaDon == (int)TrangThaiHoaDon.HoaDonGoc ? TCTBao.TCTBao0 :
                                        x.TrangThaiHoaDon == (int)TrangThaiHoaDon.HoaDonXoaBo ? TCTBao.TCTBao1 :
                                        x.TrangThaiHoaDon == (int)TrangThaiHoaDon.HoaDonThayThe ? TCTBao.TCTBao2 : TCTBao.TCTBao3,
                                LHDCLQuan = LADHDDT.HinhThuc1,
                                KHMSHDCLQuan = !string.IsNullOrEmpty(x.MauSoHoaDonLienQuan) ? x.MauSoHoaDonLienQuan : "",
                                KHHDCLQuan = !string.IsNullOrEmpty(x.KyHieuHoaDonLienQuan) ? x.KyHieuHoaDonLienQuan : "",
                                SHDCLQuan = !string.IsNullOrEmpty(x.SoHoaDonLienQuan) ? x.SoHoaDonLienQuan : "",
                            })
                            .ToList()
                        }
                    }
                },
                DSCKS = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.IV._1.DSCKS
                {
                    NNT = "  "
                }
            });

            GenerateXML(tDiep, xmlFilePath);
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
            try
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
            catch (Exception ex)
            {
                return;
            }
        }

        private IEnumerable<XElement> GetRemoveElement(XDocument xd)
        {
            foreach (var item in xd.Descendants())
            {
                if (item.Name.LocalName != "MTDTChieu" && item.Name.LocalName != "DSCKS" && item.Name.LocalName != "NBan" && item.Name.LocalName != "NNT" && (item.IsEmpty || string.IsNullOrWhiteSpace(item.Value) || string.IsNullOrEmpty(item.Value)))
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
            int stt = 0;
            var ttkhac_ThongTinSaiSot = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin>();

            switch ((LoaiHoaDon)model.LoaiHoaDon)
            {
                case LoaiHoaDon.HoaDonGTGT:
                    if (!string.IsNullOrWhiteSpace(model.IdHoaDonSaiSotBiThayThe))
                    {
                        ttkhac_ThongTinSaiSot.Add(new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin { TTruong = "Hóa đơn liên quan", KDLieu = "string", DLieu = model.GhiChuThayTheSaiSot?.Replace("<b>","").Replace("</b>","") });
                    }

                    HDonGTGT hDonGTGT = new HDonGTGT
                    {
                        DLHDon = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.DLHDon
                        {
                            TTChung = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTChung
                            {
                                PBan = pBien,
                                THDon = model.MauSo == "1" ? LoaiHoaDon.HoaDonGTGT.GetDescription() : "Tem , Vé , Thẻ, Phiếu thu điện tử",
                                KHMSHDon = model.MauSo,
                                KHHDon = model.KyHieu,
                                SHDon = model.SoHoaDon,
                                MHSo = model.MauSo == "1" ? string.Empty : null,
                                NLap = model.NgayHoaDon.Value.ToString("yyyy-MM-dd"),
                                SBKe = model.MauSo == "1" ? string.Empty : null,
                                NBKe = model.MauSo == "1" ? string.Empty : null,
                                DVTTe = model.MaLoaiTien,
                                TGia = model.TyGia,
                                HTTToan = model.TenHinhThucThanhToan,
                                MSTTCGP = taxCode,
                                MSTDVNUNLHDon = string.Empty,
                                TDVNUNLHDon = string.Empty,
                                DCDVNUNLHDon = string.Empty,
                                TTHDLQuan = null,
                                TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin>()
                            },
                            NDHDon = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.NDHDon
                            {
                                NBan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.NBan
                                {
                                    Ten = hoSoHDDT.TenDonVi ?? string.Empty,
                                    MST = hoSoHDDT.MaSoThue ?? string.Empty,
                                    DChi = hoSoHDDT.DiaChi ?? string.Empty,
                                    SDThoai = string.Empty,
                                    DCTDTu = string.Empty,
                                    STKNHang = string.Empty,
                                    TNHang = string.Empty,
                                    Fax = string.Empty,
                                    Website = string.Empty,
                                    TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin>(),
                                },
                                NMua = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.NMua
                                {
                                    Ten = model.TenKhachHang ?? string.Empty,
                                    MST = model.MaSoThue ?? string.Empty,
                                    DChi = model.DiaChi ?? string.Empty,
                                    MKHang = string.Empty,
                                    SDThoai = string.Empty,
                                    DCTDTu = string.Empty,
                                    HVTNMHang = string.Empty,
                                    STKNHang = string.Empty,
                                    TNHang = string.Empty,
                                    TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin>()
                                },
                                DSHHDVu = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.HHDVu>(),
                                TToan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TToan
                                {
                                    THTTLTSuat = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.LTSuat>(),
                                    TgTCThue = model.TongTienHang ?? 0,
                                    TgTThue = model.TongTienThueGTGT ?? 0,
                                    DSLPhi = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.LPhi>()
                                    {
                                        new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.LPhi()
                                    },
                                    TTCKTMai = model.TongTienChietKhau ?? 0,
                                    TgTTTBSo = model.TongTienThanhToan ?? 0,
                                    TgTTTBChu = model.SoTienBangChu,
                                    TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin>()
                                }
                            },
                            TTKhac = ttkhac_ThongTinSaiSot
                        },
                        DSCKS = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.DSCKS
                        {
                            NBan = string.Empty,
                            NMua = string.Empty,
                            CCKSKhac = string.Empty,
                        }
                    };

                    #region Nếu là thay thế/điều chỉnh
                    if ((model.TrangThai == (int)TrangThaiHoaDon.HoaDonThayThe) || (model.TrangThai == (int)TrangThaiHoaDon.HoaDonDieuChinh))
                    {
                        HoaDonDienTuViewModel hdlq = null;
                        if (model.TrangThai == (int)TrangThaiHoaDon.HoaDonThayThe)
                        {
                            hdlq = await GetHoaDonByIdAsync(model.ThayTheChoHoaDonId);
                            if (hdlq == null)
                            {
                                hdlq = await GetThongTinHoaDonById(model.ThayTheChoHoaDonId);
                            }
                        }
                        else if (model.TrangThai == (int)TrangThaiHoaDon.HoaDonDieuChinh)
                        {
                            hdlq = await GetHoaDonByIdAsync(model.DieuChinhChoHoaDonId);
                            if (hdlq == null)
                            {
                                hdlq = await GetThongTinHoaDonById(model.DieuChinhChoHoaDonId);
                            }
                        }

                        if (hdlq != null)
                        {
                            hDonGTGT.DLHDon.TTChung.TTHDLQuan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTHDLQuan
                            {
                                LHDCLQuan = model.TrangThai == (int)TrangThaiHoaDon.HoaDonThayThe ? (LADHDDT)hdlq.LoaiApDungHoaDonCanThayThe : (LADHDDT)hdlq.LoaiApDungHoaDonDieuChinh
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
                    }

                    #endregion

                    #region Hàng hóa chi tiết
                    stt = 0;
                    foreach (var item in model.HoaDonChiTiets)
                    {
                        var hhdvu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.HHDVu
                        {
                            TChat = (TChat)item.TinhChat,
                            STT = item.STT,
                            MHHDVu = item.MaHang ?? string.Empty,
                            THHDVu = item.TenHang ?? string.Empty,
                            DVTinh = item.DonViTinh?.Ten ?? string.Empty,
                            SLuong = item.SoLuong,
                            DGia = item.DonGia,
                            TLCKhau = item.TyLeChietKhau,
                            STCKhau = item.TienChietKhau,
                            ThTien = item.ThanhTien,
                            TSuat = item.ThueGTGT.GetThueHasPer(),
                            TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin>()
                        };

                        //if (item.TienThueGTGT != 0)
                        //{
                        //    hhdvu.TTKhac.Add(new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin
                        //    {
                        //        TTruong = TDLieu.VAT_AMOUNT,
                        //        KDLieu = KieuDuLieu.NUMERIC,
                        //        DLieu = item.TienThueGTGT.Value.ToString("G29")
                        //    });
                        //}

                        //hhdvu.TTKhac.Add(new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin
                        //{
                        //    TTruong = TDLieu.AMOUNT,
                        //    KDLieu = KieuDuLieu.NUMERIC,
                        //    DLieu = (item.ThanhTien - item.TienChietKhau + item.TienThueGTGT).Value.ToString("G29")
                        //});

                        hDonGTGT.DLHDon.NDHDon.DSHHDVu.Add(hhdvu);
                    }
                    #endregion

                    #region tổng hợp mỗi loại thuế suất
                    var groupThueGTGT = model.HoaDonChiTiets.GroupBy(x => x.ThueGTGT.GetThueHasPer())
                        .Select(x => new HoaDonDienTuChiTietViewModel
                        {
                            ThueGTGT = x.Key,
                            ThanhTien = x.Sum(y => y.ThanhTien),
                            TienThueGTGT = x.Sum(y => y.TienThueGTGT)
                        })
                        .ToList();

                    foreach (var item in groupThueGTGT)
                    {
                        hDonGTGT.DLHDon.NDHDon.TToan.THTTLTSuat.Add(new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.LTSuat
                        {
                            TSuat = item.ThueGTGT,
                            ThTien = item.ThanhTien ?? 0,
                            TThue = item.TienThueGTGT,
                        });
                    }
                    #endregion

                    if (model.TTChungThongDiep != null)
                    {
                        TDiep200GTGT tDiep = new TDiep200GTGT
                        {
                            TTChung = model.TTChungThongDiep,
                            DLieu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6.DLieu
                            {
                                HDon = hDonGTGT
                            }
                        };

                        GenerateXML(tDiep, xmlFilePath);
                    }
                    else
                    {
                        GenerateXML(hDonGTGT, xmlFilePath);
                    }

                    break;
                case LoaiHoaDon.HoaDonBanHang:
                    if (!string.IsNullOrWhiteSpace(model.IdHoaDonSaiSotBiThayThe))
                    {
                        ttkhac_ThongTinSaiSot.Add(new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin { TTruong = "Hóa đơn liên quan", KDLieu = "string", DLieu = model.GhiChuThayTheSaiSot?.Replace("<b>", "").Replace("</b>", "") });
                    }

                    HDonBanHang hDonBanHang = new HDonBanHang
                    {
                        DLHDon = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.b.DLHDon
                        {
                            TTChung = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.b.TTChung
                            {
                                PBan = pBien,
                                THDon = LoaiHoaDon.HoaDonBanHang.GetDescription(),
                                KHMSHDon = model.MauSo,
                                KHHDon = model.KyHieu,
                                SHDon = model.SoHoaDon,
                                MHSo = string.Empty,
                                NLap = model.NgayHoaDon.Value.ToString("yyyy-MM-dd"),
                                HDDCKPTQuan = model.IsHoaDonChoTCCNTKPTQ == true ? HDDCKPTQuan.HoaDonDanhChoToChucTrongKhuPhiThueQuan : HDDCKPTQuan.HoaDonKhongDanhChoToChucTrongKhuPhiThueQuan,
                                SBKe = string.Empty,
                                NBKe = string.Empty,
                                DVTTe = model.MaLoaiTien,
                                TGia = model.TyGia,
                                HTTToan = model.TenHinhThucThanhToan,
                                MSTTCGP = taxCode,
                                MSTDVNUNLHDon = string.Empty,
                                TDVNUNLHDon = string.Empty,
                                DCDVNUNLHDon = string.Empty,
                                TTHDLQuan = null,
                                TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin>()
                            },
                            NDHDon = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.b.NDHDon
                            {
                                NBan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.NBan
                                {
                                    Ten = hoSoHDDT.TenDonVi ?? string.Empty,
                                    MST = hoSoHDDT.MaSoThue ?? string.Empty,
                                    DChi = hoSoHDDT.DiaChi ?? string.Empty,
                                    SDThoai = string.Empty,
                                    DCTDTu = string.Empty,
                                    STKNHang = string.Empty,
                                    TNHang = string.Empty,
                                    Fax = string.Empty,
                                    Website = string.Empty,
                                    TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin>()
                                },
                                NMua = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.NMua
                                {
                                    Ten = model.TenKhachHang ?? string.Empty,
                                    MST = model.MaSoThue ?? string.Empty,
                                    DChi = model.DiaChi ?? string.Empty,
                                    MKHang = string.Empty,
                                    SDThoai = string.Empty,
                                    DCTDTu = string.Empty,
                                    HVTNMHang = string.Empty,
                                    STKNHang = string.Empty,
                                    TNHang = string.Empty,
                                    TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin>()
                                },
                                DSHHDVu = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.b.HHDVu>(),
                                TToan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.b.TToan
                                {
                                    DSLPhi = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.b.LPhi>()
                                    {
                                        new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.b.LPhi()
                                    },
                                    TTCKTMai = model.TongTienChietKhau,
                                    TgTTTBSo = model.TongTienThanhToan ?? 0,
                                    TgTTTBChu = model.SoTienBangChu,
                                    TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin>()
                                }
                            },
                            TTKhac = ttkhac_ThongTinSaiSot
                        },
                        DSCKS = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.DSCKS
                        {
                            NBan = string.Empty,
                            NMua = string.Empty,
                            CCKSKhac = string.Empty
                        }
                    };

                    #region Nếu là thay thế/điều chỉnh
                    if ((model.TrangThai == (int)TrangThaiHoaDon.HoaDonThayThe) || (model.TrangThai == (int)TrangThaiHoaDon.HoaDonDieuChinh))
                    {
                        HoaDonDienTuViewModel hdlq = null;
                        if (model.TrangThai == (int)TrangThaiHoaDon.HoaDonThayThe)
                        {
                            hdlq = await GetHoaDonByIdAsync(model.ThayTheChoHoaDonId);
                            if (hdlq == null)
                            {
                                hdlq = await GetThongTinHoaDonById(model.ThayTheChoHoaDonId);
                            }
                        }
                        else if (model.TrangThai == (int)TrangThaiHoaDon.HoaDonDieuChinh)
                        {
                            hdlq = await GetHoaDonByIdAsync(model.DieuChinhChoHoaDonId);
                            if (hdlq == null)
                            {
                                hdlq = await GetThongTinHoaDonById(model.DieuChinhChoHoaDonId);
                            }
                        }

                        if (hdlq != null)
                        {
                            hDonBanHang.DLHDon.TTChung.TTHDLQuan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTHDLQuan
                            {
                                LHDCLQuan = model.TrangThai == (int)TrangThaiHoaDon.HoaDonThayThe ? (LADHDDT)hdlq.LoaiApDungHoaDonCanThayThe : (LADHDDT)hdlq.LoaiApDungHoaDonDieuChinh
                            };

                            if (model.TrangThai == (int)TrangThaiHoaDon.HoaDonThayThe)
                            {
                                hDonBanHang.DLHDon.TTChung.TTHDLQuan.TCHDon = TCHDon.ThayThe;
                                hDonBanHang.DLHDon.TTChung.TTHDLQuan.KHMSHDCLQuan = model.LyDoThayTheModel.MauSo;
                                hDonBanHang.DLHDon.TTChung.TTHDLQuan.KHHDCLQuan = model.LyDoThayTheModel.KyHieu;
                                hDonBanHang.DLHDon.TTChung.TTHDLQuan.SHDCLQuan = model.LyDoThayTheModel.SoHoaDon;
                                hDonBanHang.DLHDon.TTChung.TTHDLQuan.NLHDCLQuan = model.LyDoThayTheModel.NgayHoaDon.ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                hDonBanHang.DLHDon.TTChung.TTHDLQuan.TCHDon = TCHDon.DieuChinh;
                                hDonBanHang.DLHDon.TTChung.TTHDLQuan.KHMSHDCLQuan = model.LyDoDieuChinhModel.MauSo;
                                hDonBanHang.DLHDon.TTChung.TTHDLQuan.KHHDCLQuan = model.LyDoDieuChinhModel.KyHieu;
                                hDonBanHang.DLHDon.TTChung.TTHDLQuan.SHDCLQuan = model.LyDoDieuChinhModel.SoHoaDon;
                                hDonBanHang.DLHDon.TTChung.TTHDLQuan.NLHDCLQuan = model.LyDoDieuChinhModel.NgayHoaDon.ToString("yyyy-MM-dd");
                            }
                        }
                    }

                    #endregion

                    #region Hàng hóa chi tiết
                    stt = 0;
                    foreach (var item in model.HoaDonChiTiets)
                    {
                        if (item.TinhChat != (int)TChat.KhuyenMai)
                        {
                            stt += 1;
                        }

                        var hhdvu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.b.HHDVu
                        {
                            TChat = (TChat)item.TinhChat,
                            STT = stt,
                            MHHDVu = item.MaHang ?? string.Empty,
                            THHDVu = item.TenHang ?? string.Empty,
                            DVTinh = item.DonViTinh?.Ten ?? string.Empty,
                            SLuong = item.SoLuong,
                            DGia = item.DonGia,
                            TLCKhau = item.TyLeChietKhau,
                            STCKhau = item.TienChietKhau,
                            ThTien = item.ThanhTien,
                            TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin>()
                        };

                        hDonBanHang.DLHDon.NDHDon.DSHHDVu.Add(hhdvu);
                    }
                    #endregion

                    if (model.TTChungThongDiep != null)
                    {
                        TDiep200BH tDiep = new TDiep200BH
                        {
                            TTChung = model.TTChungThongDiep,
                            DLieu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6.DLieu2
                            {
                                HDon = hDonBanHang
                            }
                        };

                        GenerateXML(tDiep, xmlFilePath);
                    }
                    else
                    {
                        GenerateXML(hDonBanHang, xmlFilePath);
                    }
                    break;
                default:
                    break;
            }
        }

        private async Task<HoaDonDienTuViewModel> GetHoaDonByIdAsync(string id)
        {
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string folder = $@"\FilesUpload\{databaseName}\{ManageFolderPath.FILE_ATTACH}";

            var query = from hd in _dataContext.HoaDonDienTus
                        join mhd in _dataContext.MauHoaDons on hd.MauHoaDonId equals mhd.MauHoaDonId into tmpMauHoaDons
                        from mhd in tmpMauHoaDons.DefaultIfEmpty()
                        join bkhhd in _dataContext.BoKyHieuHoaDons on hd.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId into tmpBoKyHieus
                        from bkhhd in tmpBoKyHieus.DefaultIfEmpty()
                        join kh in _dataContext.DoiTuongs on hd.KhachHangId equals kh.DoiTuongId into tmpKhachHangs
                        from kh in tmpKhachHangs.DefaultIfEmpty()
                        join httt in _dataContext.HinhThucThanhToans on hd.HinhThucThanhToanId equals httt.HinhThucThanhToanId into tmpHinhThucThanhToans
                        from httt in tmpHinhThucThanhToans.DefaultIfEmpty()
                        join nv in _dataContext.DoiTuongs on hd.NhanVienBanHangId equals nv.DoiTuongId into tmpNhanViens
                        from nv in tmpNhanViens.DefaultIfEmpty()
                        join nl in _dataContext.DoiTuongs on hd.CreatedBy equals nl.DoiTuongId into tmpNguoiLaps
                        from nl in tmpNguoiLaps.DefaultIfEmpty()
                        join lt in _dataContext.LoaiTiens on hd.LoaiTienId equals lt.LoaiTienId into tmpLoaiTiens
                        from lt in tmpLoaiTiens.DefaultIfEmpty()
                        join bbdc in _dataContext.BienBanDieuChinhs on hd.HoaDonDienTuId equals bbdc.HoaDonDieuChinhId into tmpBienBanDieuChinhs
                        from bbdc in tmpBienBanDieuChinhs.DefaultIfEmpty()
                        where hd.HoaDonDienTuId == id
                        select new HoaDonDienTuViewModel
                        {
                            HoaDonDienTuId = hd.HoaDonDienTuId,
                            BoKyHieuHoaDonId = hd.BoKyHieuHoaDonId,

                            BoKyHieuHoaDon = new BoKyHieuHoaDonViewModel
                            {
                                BoKyHieuHoaDonId = bkhhd.BoKyHieuHoaDonId,
                                KyHieu = bkhhd.KyHieu,
                                MauHoaDonId = bkhhd.MauHoaDonId,
                                HinhThucHoaDon = bkhhd.HinhThucHoaDon,
                                TenHinhThucHoaDon = bkhhd.HinhThucHoaDon.GetDescription(),
                                UyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon,
                                TenUyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon.GetDescription()
                            },
                            NgayHoaDon = hd.NgayHoaDon,
                            NgayLap = hd.CreatedDate,
                            SoHoaDon = hd.SoHoaDon,
                            MauHoaDonId = mhd.MauHoaDonId ?? string.Empty,
                            MauHoaDon = mhd != null ? _mp.Map<MauHoaDonViewModel>(mhd) : null,
                            MauSo = bkhhd.KyHieuMauSoHoaDon + "",
                            KyHieu = bkhhd.KyHieuHoaDon ?? string.Empty,
                            KhachHangId = kh.DoiTuongId,
                            MaKhachHang = hd.MaKhachHang,
                            TenKhachHang = hd.TenKhachHang,
                            DiaChi = hd.DiaChi,
                            MaNhanVienBanHang = hd.MaNhanVienBanHang,
                            TenNhanVienBanHang = hd.TenNhanVienBanHang,
                            KhachHang = kh != null ?
                                        new DoiTuongViewModel
                                        {
                                            Ma = kh.Ma,
                                            Ten = kh.Ten,
                                            MaSoThue = kh.MaSoThue,
                                            HoTenNguoiMuaHang = kh.HoTenNguoiMuaHang,
                                            SoDienThoaiNguoiMuaHang = kh.SoDienThoaiNguoiMuaHang,
                                            EmailNguoiMuaHang = kh.EmailNguoiMuaHang,
                                            HoTenNguoiNhanHD = kh.HoTenNguoiNhanHD,
                                            SoDienThoaiNguoiNhanHD = kh.SoDienThoaiNguoiNhanHD,
                                            EmailNguoiNhanHD = kh.EmailNguoiNhanHD,
                                            SoTaiKhoanNganHang = kh.SoTaiKhoanNganHang
                                        }
                                        : null,
                            MaSoThue = hd.MaSoThue ?? (kh != null ? kh.MaSoThue : string.Empty),
                            HinhThucThanhToanId = hd.HinhThucThanhToanId,
                            TenHinhThucThanhToan = TextHelper.GetTenHinhThucThanhToan(hd.HinhThucThanhToanId),
                            HoTenNguoiMuaHang = hd.HoTenNguoiMuaHang ?? string.Empty,
                            SoDienThoaiNguoiMuaHang = hd.SoDienThoaiNguoiMuaHang ?? string.Empty,
                            EmailNguoiMuaHang = hd.EmailNguoiMuaHang ?? string.Empty,
                            TenNganHang = hd.TenNganHang ?? string.Empty,
                            SoTaiKhoanNganHang = hd.SoTaiKhoanNganHang ?? string.Empty,
                            HoTenNguoiNhanHD = hd.HoTenNguoiNhanHD ?? string.Empty,
                            LoaiApDungHoaDonCanThayThe = (int)LADHDDT.HinhThuc1,
                            LoaiApDungHoaDonDieuChinh = (int)LADHDDT.HinhThuc1,
                            EmailNguoiNhanHD = hd.EmailNguoiNhanHD ?? string.Empty,
                            SoDienThoaiNguoiNhanHD = hd.SoDienThoaiNguoiNhanHD ?? string.Empty,
                            LoaiTienId = lt.LoaiTienId ?? string.Empty,
                            LoaiTien = lt != null ? new LoaiTienViewModel
                            {
                                Ma = lt.Ma,
                                Ten = lt.Ten
                            } : null,
                            TyGia = hd.TyGia ?? 1,
                            MaLoaiTien = lt != null ? lt.Ma : "VND",
                            IsVND = lt == null || (lt.Ma == "VND"),
                            TrangThai = hd.TrangThai,
                            TrangThaiQuyTrinh = hd.TrangThaiQuyTrinh,
                            TenTrangThaiQuyTrinh = ((TrangThaiQuyTrinh)hd.TrangThaiQuyTrinh).GetDescription(),
                            MaTraCuu = hd.MaTraCuu,
                            TrangThaiGuiHoaDon = hd.TrangThaiGuiHoaDon,
                            KhachHangDaNhan = hd.KhachHangDaNhan ?? false,
                            SoLanChuyenDoi = hd.SoLanChuyenDoi,
                            LyDoXoaBo = hd.LyDoXoaBo,
                            NgayXoaBo = hd.NgayXoaBo,
                            SoCTXoaBo = hd.SoCTXoaBo,
                            IsNotCreateThayThe = hd.IsNotCreateThayThe,
                            FileChuaKy = hd.FileChuaKy,
                            FileDaKy = hd.FileDaKy,
                            XMLChuaKy = hd.XMLChuaKy,
                            XMLDaKy = hd.XMLDaKy,
                            LoaiHoaDon = hd.LoaiHoaDon,
                            TenLoaiHoaDon = ((LoaiHoaDon)hd.LoaiHoaDon).GetDescription(),
                            LoaiChungTu = hd.LoaiChungTu,
                            ThayTheChoHoaDonId = hd.ThayTheChoHoaDonId,
                            LyDoThayThe = hd.LyDoThayThe,
                            DieuChinhChoHoaDonId = hd.DieuChinhChoHoaDonId,
                            LyDoDieuChinh = hd.LyDoDieuChinh,
                            LoaiDieuChinh = hd.LoaiDieuChinh,
                            NhanVienBanHangId = hd.NhanVienBanHangId,
                            NhanVienBanHang = nv != null ? new DoiTuongViewModel
                            {
                                Ma = nv.Ma,
                                Ten = nv.Ten
                            } : null,
                            TruongThongTinBoSung1 = hd.TruongThongTinBoSung1,
                            TruongThongTinBoSung2 = hd.TruongThongTinBoSung2,
                            TruongThongTinBoSung3 = hd.TruongThongTinBoSung3,
                            TruongThongTinBoSung4 = hd.TruongThongTinBoSung4,
                            TruongThongTinBoSung5 = hd.TruongThongTinBoSung5,
                            TruongThongTinBoSung6 = hd.TruongThongTinBoSung6,
                            TruongThongTinBoSung7 = hd.TruongThongTinBoSung7,
                            TruongThongTinBoSung8 = hd.TruongThongTinBoSung8,
                            TruongThongTinBoSung9 = hd.TruongThongTinBoSung9,
                            TruongThongTinBoSung10 = hd.TruongThongTinBoSung10,
                            ThoiHanThanhToan = hd.ThoiHanThanhToan,
                            DiaChiGiaoHang = hd.DiaChiGiaoHang,
                            BienBanDieuChinhId = bbdc != null ? bbdc.BienBanDieuChinhId : null,
                            LyDoDieuChinhModel = string.IsNullOrEmpty(hd.LyDoDieuChinh) ? null : JsonConvert.DeserializeObject<LyDoDieuChinhModel>(hd.LyDoDieuChinh),
                            LyDoThayTheModel = string.IsNullOrEmpty(hd.LyDoThayThe) ? null : JsonConvert.DeserializeObject<LyDoThayTheModel>(hd.LyDoThayThe),
                            HoaDonChiTiets = (
                                               from hdct in _dataContext.HoaDonDienTuChiTiets
                                               join hd in _dataContext.HoaDonDienTus on hdct.HoaDonDienTuId equals hd.HoaDonDienTuId into tmpHoaDons
                                               from hd in tmpHoaDons.DefaultIfEmpty()
                                               join vt in _dataContext.HangHoaDichVus on hdct.HangHoaDichVuId equals vt.HangHoaDichVuId into tmpHangHoas
                                               from vt in tmpHangHoas.DefaultIfEmpty()
                                               join dvt in _dataContext.DonViTinhs on hdct.DonViTinhId equals dvt.DonViTinhId into tmpDonViTinhs
                                               from dvt in tmpDonViTinhs.DefaultIfEmpty()
                                               where hdct.HoaDonDienTuId == id
                                               orderby hdct.CreatedDate
                                               select new HoaDonDienTuChiTietViewModel
                                               {
                                                   HoaDonDienTuChiTietId = hdct.HoaDonDienTuChiTietId,
                                                   HoaDonDienTuId = hd.HoaDonDienTuId,
                                                   HangHoaDichVuId = vt.HangHoaDichVuId,
                                                   MaHang = hdct.MaHang,
                                                   TenHang = hdct.TenHang,
                                                   TinhChat = hdct.TinhChat,
                                                   TenTinhChat = ((TChat)(hdct.TinhChat)).GetDescription(),
                                                   DonViTinhId = dvt.DonViTinhId,
                                                   DonViTinh = dvt != null ? new DonViTinhViewModel
                                                   {
                                                       Ten = dvt.Ten
                                                   } : null,
                                                   SoLuong = hdct.SoLuong,
                                                   DonGia = hdct.DonGia,
                                                   DonGiaSauThue = hdct.DonGiaSauThue,
                                                   DonGiaQuyDoi = hdct.DonGiaQuyDoi,
                                                   ThanhTien = hdct.ThanhTien,
                                                   ThanhTienSauThue = hdct.ThanhTienSauThue,
                                                   ThanhTienQuyDoi = hdct.ThanhTienQuyDoi,
                                                   ThanhTienSauThueQuyDoi = hdct.ThanhTienSauThueQuyDoi,
                                                   TyLeChietKhau = hdct.TyLeChietKhau,
                                                   TienChietKhau = hdct.TienChietKhau,
                                                   TienChietKhauQuyDoi = hdct.TienChietKhauQuyDoi,
                                                   ThueGTGT = hdct.ThueGTGT,
                                                   TienThueGTGT = hdct.TienThueGTGT,
                                                   TienThueGTGTQuyDoi = hdct.TienThueGTGTQuyDoi,
                                                   TongTienThanhToan = hdct.TongTienThanhToan,
                                                   TongTienThanhToanQuyDoi = hdct.TongTienThanhToanQuyDoi,
                                                   SoLo = hdct.SoLo,
                                                   HanSuDung = hdct.HanSuDung,
                                                   SoKhung = hdct.SoKhung,
                                                   SoMay = hdct.SoMay,
                                                   NhanVienBanHangId = hdct.NhanVienBanHangId,
                                                   MaNhanVien = hdct.MaNhanVien,
                                                   TenNhanVien = hdct.TenNhanVien,
                                                   XuatBanPhi = hdct.XuatBanPhi,
                                                   GhiChu = hdct.GhiChu,
                                                   TruongMoRongChiTiet1 = hdct.TruongMoRongChiTiet1,
                                                   TruongMoRongChiTiet2 = hdct.TruongMoRongChiTiet2,
                                                   TruongMoRongChiTiet3 = hdct.TruongMoRongChiTiet3,
                                                   TruongMoRongChiTiet4 = hdct.TruongMoRongChiTiet4,
                                                   TruongMoRongChiTiet5 = hdct.TruongMoRongChiTiet5,
                                                   TruongMoRongChiTiet6 = hdct.TruongMoRongChiTiet6,
                                                   TruongMoRongChiTiet7 = hdct.TruongMoRongChiTiet7,
                                                   TruongMoRongChiTiet8 = hdct.TruongMoRongChiTiet8,
                                                   TruongMoRongChiTiet9 = hdct.TruongMoRongChiTiet9,
                                                   TruongMoRongChiTiet10 = hdct.TruongMoRongChiTiet10
                                               }).ToList(),
                            TaiLieuDinhKems = (from tldk in _dataContext.TaiLieuDinhKems
                                               where tldk.NghiepVuId == hd.HoaDonDienTuId
                                               orderby tldk.CreatedDate
                                               select new TaiLieuDinhKemViewModel
                                               {
                                                   TaiLieuDinhKemId = tldk.TaiLieuDinhKemId,
                                                   NghiepVuId = tldk.NghiepVuId,
                                                   LoaiNghiepVu = tldk.LoaiNghiepVu,
                                                   TenGoc = tldk.TenGoc,
                                                   TenGuid = tldk.TenGuid,
                                                   CreatedDate = tldk.CreatedDate,
                                                   Link = _httpContextAccessor.GetDomain() + Path.Combine(folder, tldk.TenGuid),
                                                   Status = tldk.Status
                                               })
                                               .ToList(),
                            TaiLieuDinhKem = hd.TaiLieuDinhKem,
                            TongTienHang = hd.TongTienHang,
                            TongTienHangQuyDoi = hd.TongTienHangQuyDoi,
                            TongTienChietKhau = hd.TongTienChietKhau,
                            TongTienChietKhauQuyDoi = hd.TongTienChietKhauQuyDoi,
                            TongTienThueGTGT = hd.TongTienThueGTGT,
                            TongTienThueGTGTQuyDoi = hd.TongTienThueGTGTQuyDoi,
                            TongTienThanhToan = hd.TongTienThanhToan,
                            TongTienThanhToanQuyDoi = hd.TongTienThanhToanQuyDoi,
                            CreatedBy = hd.CreatedBy,
                            CreatedDate = hd.CreatedDate,
                            Status = hd.Status,
                            MaCuaCQT = hd.MaCuaCQT,
                            NgayKy = hd.NgayKy,
                            TrangThaiBienBanXoaBo = hd.TrangThaiBienBanXoaBo,
                            DaGuiThongBaoXoaBoHoaDon = hd.DaGuiThongBaoXoaBoHoaDon
                        };

            var result = await query.FirstOrDefaultAsync();
            if (result != null)
            {
                result.TongTienThanhToan = result.HoaDonChiTiets.Sum(x => x.TongTienThanhToan ?? 0);
                result.TongTienThanhToanQuyDoi = result.HoaDonChiTiets.Sum(x => x.TongTienThanhToanQuyDoi ?? 0);
                result.IsSentCQT = await (from dlghd in _dataContext.DuLieuGuiHDDTs
                                          join dlghdct in _dataContext.DuLieuGuiHDDTChiTiets on dlghd.DuLieuGuiHDDTId equals dlghdct.DuLieuGuiHDDTId into tmpCT
                                          from dlghdct in tmpCT.DefaultIfEmpty()
                                          select new
                                          {
                                              HoaDonDienTuId = dlghdct != null ? dlghdct.HoaDonDienTuId : dlghd.HoaDonDienTuId
                                          })
                                        .Where(x => x.HoaDonDienTuId == result.HoaDonDienTuId)
                                        .AnyAsync();
            }

            return result;
        }

        private async Task<HoaDonDienTuViewModel> GetThongTinHoaDonById(string Id)
        {
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;

            return await _dataContext.ThongTinHoaDons.Where(x => x.Id == Id)
                                            .Select(x => new HoaDonDienTuViewModel
                                            {
                                                HoaDonDienTuId = x.Id,
                                                MauSo = x.MauSoHoaDon,
                                                KyHieu = x.KyHieuHoaDon,
                                                MaCuaCQT = x.MaCQTCap,
                                                NgayHoaDon = x.NgayHoaDon,
                                                SoHoaDon = x.SoHoaDon,
                                                LoaiApDungHoaDonDieuChinh = x.HinhThucApDung,
                                                LoaiApDungHoaDonCanThayThe = x.HinhThucApDung,
                                                BienBanDieuChinhId = _dataContext.BienBanDieuChinhs.Where(o => o.HoaDonBiDieuChinhId == Id).Select(o => o.BienBanDieuChinhId).FirstOrDefault(),
                                                TaiLieuDinhKems = (from tldk in _dataContext.TaiLieuDinhKems
                                                                   where tldk.NghiepVuId == x.Id
                                                                   orderby tldk.CreatedDate
                                                                   select new TaiLieuDinhKemViewModel
                                                                   {
                                                                       TaiLieuDinhKemId = tldk.TaiLieuDinhKemId,
                                                                       NghiepVuId = tldk.NghiepVuId,
                                                                       LoaiNghiepVu = tldk.LoaiNghiepVu,
                                                                       TenGoc = tldk.TenGoc,
                                                                       TenGuid = tldk.TenGuid,
                                                                       CreatedDate = tldk.CreatedDate,
                                                                       Link = _httpContextAccessor.GetDomain() + Path.Combine($@"\FilesUpload\{databaseName}\{ManageFolderPath.FILE_ATTACH}", tldk.TenGuid),
                                                                       Status = tldk.Status
                                                                   })
                                                   .ToList(),
                                            })
                                            .FirstOrDefaultAsync();
        }

        public void CreateQuyDinhKyThuat_PhanII_II_5(string xmlFilePath, ThongDiepChungViewModel model)
        {
            ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6.TDiep tDiep = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6.TDiep
            {
                TTChung = new TTChungThongDiep
                {
                    PBan = model.PhienBan,
                    MNGui = model.MaNoiGui,
                    MNNhan = model.MaNoiNhan,
                    MLTDiep = model.MaLoaiThongDiep.ToString(),
                    MTDiep = model.MaThongDiep,
                    MTDTChieu = model.MaThongDiepThamChieu ?? string.Empty,
                    MST = model.MaSoThue,
                    SLuong = model.SoLuong.Value,
                },
            };

            GenerateXML(tDiep, xmlFilePath);

            XmlDocument xml = new XmlDocument();
            xml.Load(xmlFilePath);
            xml.DocumentElement.AppendChild(xml.CreateElement(nameof(tDiep.DLieu)));

            var databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string folderPath = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/{databaseName}");
            string filePath = Path.Combine(folderPath, $"{ManageFolderPath.XML_SIGNED}/{model.DuLieuGuiHDDT.HoaDonDienTu.XMLDaKy}");

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

        public void CreateBangTongHopDuLieu(string xmlPath, BangTongHopDuLieuParams @params)
        {
            CreateQuyDinhKyThuat_PhanII_IV_2(xmlPath, @params);
        }

        public string PrintXML(string xml)
        {
            string result = "";

            MemoryStream mStream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(mStream, Encoding.Unicode);
            XmlDocument document = new XmlDocument();

            try
            {
                // Load the XmlDocument with the XML.
                document.LoadXml(xml);

                writer.Formatting = Formatting.Indented;

                // Write the XML into a formatting XmlTextWriter
                document.WriteContentTo(writer);
                writer.Flush();
                mStream.Flush();

                // Have to rewind the MemoryStream in order to read
                // its contents.
                mStream.Position = 0;

                // Read MemoryStream contents into a StreamReader.
                StreamReader sReader = new StreamReader(mStream);

                // Extract the text from the StreamReader.
                string formattedXml = sReader.ReadToEnd();

                result = formattedXml;
            }
            catch (XmlException)
            {
                // Handle the exception
            }

            mStream.Close();
            writer.Close();

            return result;
        }
    }
}
