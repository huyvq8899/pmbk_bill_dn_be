using AutoMapper;
using DLL;
using DLL.Constants;
using DLL.Entity;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Services.Helper;
using Services.Helper.Constants;
using Services.Helper.Params.QuyDinhKyThuat;
using Services.Repositories.Interfaces;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.QuanLy;
using Services.ViewModels.QuanLyHoaDonDienTu;
using Services.ViewModels.QuyDinhKyThuat;
using Services.ViewModels.XML;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Formatting = System.Xml.Formatting;
/// Hóa đơn giá trị gia tăng
using HDonGTGT = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.HDon;
using HDonBanHang = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.b.HDon;
using HDonTaiSanCong = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.c.HDon;
using HDonDuTruQuocGia = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.d.HDon;
using HDonXuatKhoVanChuyenNoiBo = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.e.HDon;
using HDonXuatKhoBanDaiLy = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.f.HDon;
using HDonKhac = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.g.HDon;
using HDonNhieuTyGia = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.k.HDon;
using HDonGTGTKiemToKhai = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.l.HDon;
using TDiep200GTGT = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6.TDiep;
using TDiep200BH = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6.TDiep2;
using TDiep200PXKVanChuyenNoiBo = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6.TDiep7;
using TDiep200PXKBanDaiLy = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6.TDiep8;
using TDiep200GTGTPos = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._10.TDiep;
using TDiep200BHPos = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._10.TDiep2;
using TDiep200KhacPos = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._10.TDiep3;

using HoaDonGTGTPos = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.HDon;
using HoaDonBHPos = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.i.HDon;
using HoaDonKhacPos = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.HDon;
using TDiep200PTaiSanCong = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6.TDiep3;
using TDiep200DuTruQuocGia = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6.TDiep4;
using TDiep200HDonKhac = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6.TDiep5;
using TDiep200NhieuTyGia = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6.TDiep11;
using TDiep200GTGTKiemToKhai = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6.TDiep12;

using Services.Repositories.Interfaces.TienIch;
using DLL.Entity.QuyDinhKyThuat;
using DLL.Entity.QuanLyHoaDon;
using Services.Repositories.Interfaces.Config;
using Services.Helper.Params.HoaDon;
using System.Drawing;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._7;
using System.Runtime.InteropServices;

namespace Services.Repositories.Implimentations
{
    public class XMLInvoiceService : IXMLInvoiceService
    {
        private readonly Datacontext _dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IHoSoHDDTService _hoSoHDDTService;
        private readonly ITuyChonService _ITuyChonService;
        private readonly IDigitalSignerNameReaderService _digitalSignerNameReaderService;
        private readonly IMapper _mp;

        public XMLInvoiceService(
            Datacontext dataContext,
            IHttpContextAccessor httpContextAccessor,
            IHostingEnvironment hostingEnvironment,
            IConfiguration configuration,
            IHoSoHDDTService hoSoHDDTService,
            ITuyChonService ITuyChonService,
            IDigitalSignerNameReaderService digitalSignerNameReaderService,
            IMapper mp)
        {
            _dataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _hoSoHDDTService = hoSoHDDTService;
            _digitalSignerNameReaderService = digitalSignerNameReaderService;
            _ITuyChonService = ITuyChonService;
            _mp = mp;
        }

        public async Task<bool> CreateXMLInvoice(string xmlFilePath, HoaDonDienTuViewModel model)
        {
            try
            {
                if (model.MauHoaDon != null)
                {
                    await CreateInvoiceQD1450Async(xmlFilePath, model);
                    return true;
                }

                return false;
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
                        //entityData.Binary = File.ReadAllBytes(fullXMLFile);
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
                            //Binary = File.ReadAllBytes(fullXMLFile)
                        };
                        _dataContext.FileDatas.AddAsync(entityData);
                    }

                    _dataContext.SaveChanges();
                }
            }

            return Path.Combine(assetsFolder, fileName);
        }

        public async Task CreateQuyDinhKyThuat_PhanII_II_7(string xmlFilePath, ThongDiepChungViewModel model)
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
                    MTDTChieu = model.MaThongDiepThamChieu ?? string.Empty,
                    MST = model.MaSoThue,
                    SLuong = model.SoLuong,
                },
            };

            GenerateXML(tDiep, xmlFilePath);

            XmlDocument xml = new XmlDocument();
            xml.Load(xmlFilePath);
            xml.DocumentElement.AppendChild(xml.CreateElement(nameof(tDiep.DLieu)));

            if (model.DuLieuGuiHDDT.DuLieuGuiHDDTChiTiets != null && model.DuLieuGuiHDDT.DuLieuGuiHDDTChiTiets.Any())
            {
                ///////////// gửi nhiều hddt vào 1 thông điệp
            }
            else
            {
                var fileData = await _dataContext.FileDatas
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Type == 1 && x.RefId == model.DuLieuGuiHDDT.HoaDonDienTuId);

                if (fileData != null)
                {
                    XmlDocument signedXML = new XmlDocument();
                    string xmlContent = Encoding.UTF8.GetString(fileData.Binary);
                    signedXML.LoadXml(xmlContent);

                    var importNode = xml.ImportNode(signedXML.DocumentElement.SelectSingleNode("/TDiep/DLieu/HDon"), true);
                    xml.DocumentElement[nameof(tDiep.DLieu)].AppendChild(importNode);
                }
            }

            xml.Save(xmlFilePath);
        }

        public async Task<FileData> CreateQuyDinhKyThuat_PhanII_IV_2(string xmlFilePath, BangTongHopDuLieuParams @params)
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
                    SLuong = @params.TTChung1.SoLuong,
                },
                DLieu = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.IV._1.BTHDLieu>()
            };

            @params.DuLieu = @params.DuLieu.OrderBy(x => x.SoBTHDLieu).ToList();
            foreach (var item in @params.DuLieu)
            {
                tDiep.DLieu.Add(new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.IV._1.BTHDLieu
                {
                    DLBTHop = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.IV._1.DLBTHop
                    {
                        TTChung = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.IV._1.TTChung
                        {
                            PBan = item.PhienBan,
                            MSo = item.MauSo,
                            Ten = item.Ten,
                            SBTHDLieu = item.SoBTHDLieu,
                            LKDLieu = item.LoaiKyDuLieu,
                            KDLieu = item.KyDuLieu,
                            LDau = item.LanDau == true ? LDau.LanDau : LDau.BoSung,
                            BSLThu = item.BoSungLanThu,
                            NLap = item.NgayLap.ToString("yyyy-MM-ddThh:mm:ss"),
                            MST = item.MaSoThue,
                            TNNT = item.TenNNT,
                            HDDIn = item.HDDIn == true ? HDDIn.HoaDonIn : HDDIn.HoaDonDienTu,
                            LHHoa = (LHHoa)item.LHHoa,
                            DVTTe = "VND",
                        },
                        NDBTHDLieu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.IV._1.NDBTHDLieu
                        {
                            DSDLieu = item.ChiTiets.Select(x => new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.IV._1.DLieu
                            {
                                STT = item.ChiTiets.IndexOf(x) + 1,
                                KHMSHDon = x.MauSo.ToString(),
                                KHHDon = x.KyHieu,
                                SHDon = x.SoHoaDon,
                                NLap = x.NgayHoaDon.Value.ToString("yyyy-MM-ddThh:mm:ss"),
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
                                TThai = (TCTBao)x.TrangThaiHoaDon,
                                LHDCLQuan = x.LoaiHoaDonLienQuan,
                                KHMSHDCLQuan = !string.IsNullOrEmpty(x.MauSoHoaDonLienQuan) ? x.MauSoHoaDonLienQuan : "",
                                KHHDCLQuan = !string.IsNullOrEmpty(x.KyHieuHoaDonLienQuan) ? x.KyHieuHoaDonLienQuan : "",
                                SHDCLQuan = !string.IsNullOrEmpty(x.SoHoaDonLienQuan) ? x.SoHoaDonLienQuan : "",
                                GChu = x.GhiChu,
                                TGia = 1,
                                TTPhi = 0,
                                TGTKhac = 0
                            })
                                .ToList()
                        }
                    },
                    DSCKS = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.IV._1.DSCKS
                    {
                        NNT = "  "
                    }
                });

            }

            GenerateXML(tDiep, xmlFilePath);
            var fileData = new FileData
            {
                FileDataId = Guid.NewGuid().ToString(),
                RefId = @params.ThongDiepChungId,
                Content = File.ReadAllText(xmlFilePath),
                Binary = File.ReadAllBytes(xmlFilePath),
                DateTime = DateTime.Now,
                IsSigned = false
            };
            await _dataContext.FileDatas.AddAsync(fileData);
            await _dataContext.SaveChangesAsync();

            return fileData;
        }

        public void GenerateXML<T>(T data, string path)
        {
            try
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");

                data = data.RemoveTrailingZeros();

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

        public void CreateQuyDinhKyThuat_PhanII_II_5(string xmlFilePath, ThongDiepChungViewModel model)
        {
            TDiep200GTGT tDiep = new TDiep200GTGT
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
                    SLuong = model.SoLuong,
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

        public async Task CreateQuyDinhKyThuatTheoMaLoaiThongDiep(string xmlFilePath, ThongDiepChungViewModel model)
        {
            switch (model.MaLoaiThongDiep)
            {
                case (int)MLTDiep.TDGHDDTTCQTCapMa:
                    CreateQuyDinhKyThuat_PhanII_II_5(xmlFilePath, model);
                    break;
                case (int)MLTDiep.TDCDLHDKMDCQThue:
                    await CreateQuyDinhKyThuat_PhanII_II_7(xmlFilePath, model);
                    break;
                default:
                    break;
            }
        }

        public async Task<FileData> CreateBangTongHopDuLieu(string xmlPath, BangTongHopDuLieuParams @params)
        {
            return await CreateQuyDinhKyThuat_PhanII_IV_2(xmlPath, @params);
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

        private IEnumerable<XElement> GetRemoveElement(XDocument xd)
        {
            foreach (var item in xd.Descendants())
            {
                if (item.Name.LocalName != "TSuat" && item.Name.LocalName != "MTDTChieu" && item.Name.LocalName != "DSCKS" && item.Name.LocalName != "NBan" && item.Name.LocalName != "NNT" && item.Name.LocalName != "CKSNNT" && (item.IsEmpty || string.IsNullOrWhiteSpace(item.Value) || string.IsNullOrEmpty(item.Value)))
                {
                    yield return item;
                }
            }
        }

        private async Task CreateInvoiceQD1450Async(string xmlFilePath, HoaDonDienTuViewModel model)
        {
            try
            {
                string pBien = "2.0.0";
                string pBien1 = "2.0.1";
                string taxCode = _configuration["Config:TaxCode"];
                var hoSoHDDT = model.HoSoHDDT;
                if (hoSoHDDT == null) hoSoHDDT = await _hoSoHDDTService.GetDetailAsync();
                int stt = 0;
                var ttkhac_ThongTinSaiSot = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin>();
                var thongTinNguoiBan = model.ThongTinNguoiBan;
                if (thongTinNguoiBan == null)
                {

                    // Gọi API đọc thông tin người bán trên mẫu hóa đơn của hóa đơn
                    var listThongTinNguoiBan = await _digitalSignerNameReaderService.GetThongTinNguoiBanTuHoaDonAsync(new string[] { model.HoaDonDienTuId });
                    thongTinNguoiBan = listThongTinNguoiBan?.FirstOrDefault();
                }
                var sdtNBan = thongTinNguoiBan != null ? thongTinNguoiBan.DienThoaiNguoiBan != null ? thongTinNguoiBan.DienThoaiNguoiBan.Trim() : hoSoHDDT.SoDienThoaiLienHe.Trim() : hoSoHDDT.SoDienThoaiLienHe.Trim();
                if (sdtNBan.Length > 20) sdtNBan = !string.IsNullOrEmpty(hoSoHDDT.SoDienThoaiLienHe) ? hoSoHDDT.SoDienThoaiLienHe.Trim() : "";
                var stkNBan = thongTinNguoiBan != null ? thongTinNguoiBan.SoTaiKhoanNguoiBan != null ? thongTinNguoiBan.SoTaiKhoanNguoiBan.Trim() : hoSoHDDT.SoTaiKhoanNganHang.Trim() : hoSoHDDT.SoTaiKhoanNganHang.Trim();
                if (stkNBan.Length > 30) stkNBan = !string.IsNullOrEmpty(hoSoHDDT.SoTaiKhoanNganHang) ? hoSoHDDT.SoTaiKhoanNganHang.Trim() : "";
                switch ((LoaiHoaDon)model.LoaiHoaDon)
                {
                    case LoaiHoaDon.HoaDonGTGT:
                        if (!string.IsNullOrWhiteSpace(model.IdHoaDonSaiSotBiThayThe))
                        {
                            ttkhac_ThongTinSaiSot.Add(new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin { TTruong = "Hóa đơn liên quan", KDLieu = "string", DLieu = model.GhiChuThayTheSaiSot?.Replace("<b>", "").Replace("</b>", "") });
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
                                        Ten = thongTinNguoiBan != null ? (thongTinNguoiBan.TenDonViNguoiBan ?? String.Empty) : (hoSoHDDT.TenDonVi ?? String.Empty),
                                        MST = thongTinNguoiBan != null ? thongTinNguoiBan.MaSoThueNguoiBan.Trim().Replace(" ", string.Empty) ?? hoSoHDDT.MaSoThue ?? string.Empty : hoSoHDDT.MaSoThue ?? string.Empty,
                                        DChi = thongTinNguoiBan != null ? (thongTinNguoiBan.DiaChiNguoiBan ?? String.Empty) : (hoSoHDDT.DiaChi ?? string.Empty),
                                        SDThoai = sdtNBan,
                                        DCTDTu = thongTinNguoiBan != null ? (thongTinNguoiBan.EmailNguoiBan ?? String.Empty) : string.Empty,
                                        STKNHang = stkNBan,
                                        TNHang = thongTinNguoiBan != null ? (thongTinNguoiBan.TenNganHangNguoiBan ?? String.Empty) : string.Empty,
                                        Fax = thongTinNguoiBan != null ? (thongTinNguoiBan.FaxNguoiBan ?? String.Empty) : string.Empty,
                                        Website = thongTinNguoiBan != null ? (thongTinNguoiBan.WebsiteNguoiBan ?? String.Empty) : string.Empty,
                                        TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin>(),
                                    },
                                    NMua = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.NMua
                                    {
                                        Ten = model.TenKhachHang ?? string.Empty,
                                        MST = model.MaSoThue ?? string.Empty,
                                        DChi = model.DiaChi ?? string.Empty,
                                        MKHang = string.Empty,
                                        SDThoai = model.SoDienThoaiNguoiMuaHang.Trim() ?? string.Empty,
                                        DCTDTu = string.Empty,
                                        HVTNMHang = model.HoTenNguoiMuaHang ?? string.Empty,
                                        STKNHang = model.SoTaiKhoanNganHang ?? string.Empty,
                                        TNHang = model.TenNganHang ?? string.Empty,
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
                                    if (model.LyDoThayTheModel != null)
                                    {
                                        hDonGTGT.DLHDon.TTChung.TTHDLQuan.TCHDon = TCHDon.ThayThe;
                                        hDonGTGT.DLHDon.TTChung.TTHDLQuan.KHMSHDCLQuan = model.LyDoThayTheModel.MauSo;
                                        hDonGTGT.DLHDon.TTChung.TTHDLQuan.KHHDCLQuan = model.LyDoThayTheModel.KyHieu;
                                        hDonGTGT.DLHDon.TTChung.TTHDLQuan.SHDCLQuan = model.LyDoThayTheModel.SoHoaDon;
                                        hDonGTGT.DLHDon.TTChung.TTHDLQuan.NLHDCLQuan = model.LyDoThayTheModel.NgayHoaDon.ToString("yyyy-MM-dd");
                                        hDonGTGT.DLHDon.TTChung.TTHDLQuan.GChu = model.LyDoThayTheModel.LyDo;
                                    }
                                }
                                else
                                {
                                    if (model.LyDoDieuChinhModel != null)
                                    {
                                        hDonGTGT.DLHDon.TTChung.TTHDLQuan.TCHDon = TCHDon.DieuChinh;
                                        hDonGTGT.DLHDon.TTChung.TTHDLQuan.KHMSHDCLQuan = model.LyDoDieuChinhModel.MauSo;
                                        hDonGTGT.DLHDon.TTChung.TTHDLQuan.KHHDCLQuan = model.LyDoDieuChinhModel.KyHieu;
                                        hDonGTGT.DLHDon.TTChung.TTHDLQuan.SHDCLQuan = model.LyDoDieuChinhModel.SoHoaDon;
                                        hDonGTGT.DLHDon.TTChung.TTHDLQuan.NLHDCLQuan = model.LyDoDieuChinhModel.NgayHoaDon.ToString("yyyy-MM-dd");
                                        hDonGTGT.DLHDon.TTChung.TTHDLQuan.GChu = model.LyDoDieuChinhModel.LyDo;
                                    }
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
                                DVTinh = item.TenDonViTinh ?? string.Empty,
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
                                ThanhTien = x.Sum(y => y.TinhChat == 3 ? -y.ThanhTien : y.ThanhTien),
                                TienThueGTGT = x.Sum(y => y.TinhChat == 3 ? -y.TienThueGTGT : y.TienThueGTGT)
                            })
                            .ToList();

                        foreach (var item in groupThueGTGT)
                        {
                            if (!string.IsNullOrEmpty(item.ThueGTGT))
                            {
                                hDonGTGT.DLHDon.NDHDon.TToan.THTTLTSuat.Add(new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.LTSuat
                                {
                                    TSuat = item.ThueGTGT,
                                    ThTien = item.ThanhTien ?? 0,
                                    TThue = item.TienThueGTGT,
                                });
                            }
                        }
                        #endregion

                        if ((model.TrangThai == (int)TrangThaiHoaDon.HoaDonDieuChinh) && (model.LoaiDieuChinh == (int)LoaiDieuChinhHoaDon.DieuChinhThongTin) && (model.IsThongTinNguoiBanHoacNguoiMua == true))
                        {
                            hDonGTGT.DLHDon.NDHDon.DSHHDVu = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.HHDVu>();
                            hDonGTGT.DLHDon.NDHDon.TToan = null;
                        }

                        if (model.TTChungThongDiep != null)
                        {
                            string mNGui = _configuration["TTChung:MNGui"];
                            string mNNhan = _configuration["TTChung:MNNhan"];
                            model.TTChungThongDiep.PBan = pBien;
                            model.TTChungThongDiep.MNGui = mNGui;
                            model.TTChungThongDiep.MNNhan = mNNhan;
                            model.TTChungThongDiep.MST = thongTinNguoiBan != null ? thongTinNguoiBan.MaSoThueNguoiBan : hoSoHDDT.MaSoThue;

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
                                        Ten = thongTinNguoiBan != null ? (thongTinNguoiBan.TenDonViNguoiBan ?? String.Empty) : (hoSoHDDT.TenDonVi ?? String.Empty),
                                        MST = thongTinNguoiBan != null ? thongTinNguoiBan.MaSoThueNguoiBan.Trim().Replace(" ", string.Empty) ?? hoSoHDDT.MaSoThue ?? string.Empty : hoSoHDDT.MaSoThue ?? string.Empty,
                                        DChi = thongTinNguoiBan != null ? (thongTinNguoiBan.DiaChiNguoiBan ?? String.Empty) : (hoSoHDDT.DiaChi ?? string.Empty),
                                        SDThoai = sdtNBan,
                                        DCTDTu = thongTinNguoiBan != null ? (thongTinNguoiBan.EmailNguoiBan ?? String.Empty) : string.Empty,
                                        STKNHang = thongTinNguoiBan != null ? (thongTinNguoiBan.SoTaiKhoanNguoiBan ?? String.Empty) : string.Empty,
                                        TNHang = thongTinNguoiBan != null ? (thongTinNguoiBan.TenNganHangNguoiBan ?? String.Empty) : string.Empty,
                                        Fax = thongTinNguoiBan != null ? (thongTinNguoiBan.FaxNguoiBan ?? String.Empty) : string.Empty,
                                        Website = thongTinNguoiBan != null ? (thongTinNguoiBan.WebsiteNguoiBan ?? String.Empty) : string.Empty,
                                        TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin>()
                                    },
                                    NMua = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.NMua
                                    {
                                        Ten = model.TenKhachHang ?? string.Empty,
                                        MST = model.MaSoThue ?? string.Empty,
                                        DChi = model.DiaChi ?? string.Empty,
                                        MKHang = string.Empty,
                                        SDThoai = model.SoDienThoaiNguoiMuaHang ?? string.Empty,
                                        DCTDTu = string.Empty,
                                        HVTNMHang = model.HoTenNguoiMuaHang ?? string.Empty,
                                        STKNHang = model.SoTaiKhoanNganHang ?? string.Empty,
                                        TNHang = model.TenNganHang ?? string.Empty,
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
                                    hDonBanHang.DLHDon.TTChung.TTHDLQuan.KHMSHDCLQuan = hdlq.MauSo;
                                    hDonBanHang.DLHDon.TTChung.TTHDLQuan.KHHDCLQuan = hdlq.KyHieu;
                                    hDonBanHang.DLHDon.TTChung.TTHDLQuan.SHDCLQuan = hdlq.StrSoHoaDon;
                                    hDonBanHang.DLHDon.TTChung.TTHDLQuan.NLHDCLQuan = hdlq.NgayHoaDon.Value.ToString("yyyy-MM-dd");
                                }
                                else
                                {
                                    hDonBanHang.DLHDon.TTChung.TTHDLQuan.TCHDon = TCHDon.DieuChinh;
                                    hDonBanHang.DLHDon.TTChung.TTHDLQuan.KHMSHDCLQuan = hdlq.MauSo;
                                    hDonBanHang.DLHDon.TTChung.TTHDLQuan.KHHDCLQuan = hdlq.KyHieu;
                                    hDonBanHang.DLHDon.TTChung.TTHDLQuan.SHDCLQuan = hdlq.StrSoHoaDon;
                                    hDonBanHang.DLHDon.TTChung.TTHDLQuan.NLHDCLQuan = hdlq.NgayHoaDon.Value.ToString("yyyy-MM-dd");
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
                                DVTinh = item.TenDonViTinh ?? string.Empty,
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
                            string mNGui = _configuration["TTChung:MNGui"];
                            string mNNhan = _configuration["TTChung:MNNhan"];
                            model.TTChungThongDiep.PBan = pBien;
                            model.TTChungThongDiep.MNGui = mNGui;
                            model.TTChungThongDiep.MNNhan = mNNhan;
                            model.TTChungThongDiep.MST = thongTinNguoiBan != null ? thongTinNguoiBan.MaSoThueNguoiBan : hoSoHDDT.MaSoThue;

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
                    case LoaiHoaDon.PXKKiemVanChuyenNoiBo:
                        if (!string.IsNullOrWhiteSpace(model.IdHoaDonSaiSotBiThayThe))
                        {
                            ttkhac_ThongTinSaiSot.Add(new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin { TTruong = "Hóa đơn liên quan", KDLieu = "string", DLieu = model.GhiChuThayTheSaiSot?.Replace("<b>", "").Replace("</b>", "") });
                        }

                        HDonXuatKhoVanChuyenNoiBo hDonXuatKhoVanChuyenNoiBo = new HDonXuatKhoVanChuyenNoiBo
                        {
                            DLHDon = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.e.DLHDon
                            {
                                TTChung = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.e.TTChung
                                {
                                    PBan = pBien,
                                    THDon = LoaiHoaDon.PXKKiemVanChuyenNoiBo.GetDescription(),
                                    KHMSHDon = model.MauSo,
                                    KHHDon = model.KyHieu,
                                    SHDon = model.SoHoaDon,
                                    NLap = model.NgayHoaDon.Value.ToString("yyyy-MM-dd"),
                                    DVTTe = model.MaLoaiTien,
                                    TGia = model.TyGia,
                                    MSTTCGP = taxCode,
                                    TTHDLQuan = null,
                                    TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin>()
                                },
                                NDHDon = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.e.NDHDon
                                {
                                    NBan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.e.NBan
                                    {
                                        Ten = thongTinNguoiBan.TenDonViNguoiBan ?? hoSoHDDT.TenDonVi ?? string.Empty,
                                        MST = thongTinNguoiBan != null ? thongTinNguoiBan.MaSoThueNguoiBan.Trim() ?? hoSoHDDT.MaSoThue ?? string.Empty : hoSoHDDT.MaSoThue ?? string.Empty,
                                        LDDNBo = model.CanCuSo ?? string.Empty,
                                        DChi = model.DiaChiKhoXuatHang ?? string.Empty,
                                        HDSo = model.HopDongVanChuyenSo ?? string.Empty,
                                        HVTNXHang = model.HoTenNguoiXuatHang ?? string.Empty,
                                        TNVChuyen = model.TenNguoiVanChuyen ?? string.Empty,
                                        PTVChuyen = model.PhuongThucVanChuyen ?? string.Empty,
                                    },
                                    NMua = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.e.NMua
                                    {
                                        Ten = model.TenKhachHang ?? string.Empty,
                                        MST = model.MaSoThue ?? string.Empty,
                                        DChi = model.DiaChiKhoNhanHang ?? string.Empty,
                                        HVTNNHang = model.HoTenNguoiNhanHang
                                    },
                                    DSHHDVu = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.e.HHDVu>()
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
                                hDonXuatKhoVanChuyenNoiBo.DLHDon.TTChung.TTHDLQuan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTHDLQuan
                                {
                                    LHDCLQuan = model.TrangThai == (int)TrangThaiHoaDon.HoaDonThayThe ? (LADHDDT)hdlq.LoaiApDungHoaDonCanThayThe : (LADHDDT)hdlq.LoaiApDungHoaDonDieuChinh
                                };

                                if (model.TrangThai == (int)TrangThaiHoaDon.HoaDonThayThe)
                                {
                                    hDonXuatKhoVanChuyenNoiBo.DLHDon.TTChung.TTHDLQuan.TCHDon = TCHDon.ThayThe;
                                    hDonXuatKhoVanChuyenNoiBo.DLHDon.TTChung.TTHDLQuan.KHMSHDCLQuan = model.LyDoThayTheModel.MauSo;
                                    hDonXuatKhoVanChuyenNoiBo.DLHDon.TTChung.TTHDLQuan.KHHDCLQuan = model.LyDoThayTheModel.KyHieu;
                                    hDonXuatKhoVanChuyenNoiBo.DLHDon.TTChung.TTHDLQuan.SHDCLQuan = model.LyDoThayTheModel.SoHoaDon;
                                    hDonXuatKhoVanChuyenNoiBo.DLHDon.TTChung.TTHDLQuan.NLHDCLQuan = model.LyDoThayTheModel.NgayHoaDon.ToString("yyyy-MM-dd");
                                    hDonXuatKhoVanChuyenNoiBo.DLHDon.TTChung.TTHDLQuan.GChu = model.LyDoThayTheModel.LyDo;
                                }
                                else
                                {
                                    hDonXuatKhoVanChuyenNoiBo.DLHDon.TTChung.TTHDLQuan.TCHDon = TCHDon.DieuChinh;
                                    hDonXuatKhoVanChuyenNoiBo.DLHDon.TTChung.TTHDLQuan.KHMSHDCLQuan = model.LyDoDieuChinhModel.MauSo;
                                    hDonXuatKhoVanChuyenNoiBo.DLHDon.TTChung.TTHDLQuan.KHHDCLQuan = model.LyDoDieuChinhModel.KyHieu;
                                    hDonXuatKhoVanChuyenNoiBo.DLHDon.TTChung.TTHDLQuan.SHDCLQuan = model.LyDoDieuChinhModel.SoHoaDon;
                                    hDonXuatKhoVanChuyenNoiBo.DLHDon.TTChung.TTHDLQuan.NLHDCLQuan = model.LyDoDieuChinhModel.NgayHoaDon.ToString("yyyy-MM-dd");
                                    hDonXuatKhoVanChuyenNoiBo.DLHDon.TTChung.TTHDLQuan.GChu = model.LyDoDieuChinhModel.LyDo;
                                }
                            }
                        }
                        #endregion

                        #region Hàng hóa chi tiết
                        stt = 0;
                        foreach (var item in model.HoaDonChiTiets)
                        {
                            var hhdvu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.e.HHDVu
                            {
                                TChat = (TChat)item.TinhChat,
                                STT = item.STT,
                                MHHDVu = item.MaHang ?? string.Empty,
                                THHDVu = item.TenHang ?? string.Empty,
                                DVTinh = item.DonViTinh?.Ten ?? string.Empty,
                                SLuong = item.SoLuong,
                                DGia = item.DonGia,
                                ThTien = item.ThanhTien,
                                TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin>()
                            };

                            hDonXuatKhoVanChuyenNoiBo.DLHDon.NDHDon.DSHHDVu.Add(hhdvu);
                        }
                        #endregion

                        if (model.TTChungThongDiep != null)
                        {
                            string mNGui = _configuration["TTChung:MNGui"];
                            string mNNhan = _configuration["TTChung:MNNhan"];
                            model.TTChungThongDiep.PBan = pBien;
                            model.TTChungThongDiep.MNGui = mNGui;
                            model.TTChungThongDiep.MNNhan = mNNhan;
                            model.TTChungThongDiep.MST = thongTinNguoiBan != null ? thongTinNguoiBan.MaSoThueNguoiBan : hoSoHDDT.MaSoThue;

                            TDiep200PXKVanChuyenNoiBo tDiep = new TDiep200PXKVanChuyenNoiBo
                            {
                                TTChung = model.TTChungThongDiep,
                                DLieu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6.DLieu7
                                {
                                    HDon = hDonXuatKhoVanChuyenNoiBo
                                }
                            };

                            GenerateXML(tDiep, xmlFilePath);
                        }
                        else
                        {
                            GenerateXML(hDonXuatKhoVanChuyenNoiBo, xmlFilePath);
                        }
                        break;
                    case LoaiHoaDon.PXKHangGuiBanDaiLy:
                        if (!string.IsNullOrWhiteSpace(model.IdHoaDonSaiSotBiThayThe))
                        {
                            ttkhac_ThongTinSaiSot.Add(new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin { TTruong = "Hóa đơn liên quan", KDLieu = "string", DLieu = model.GhiChuThayTheSaiSot?.Replace("<b>", "").Replace("</b>", "") });
                        }

                        HDonXuatKhoBanDaiLy hDonXuatKhoBanDaiLy = new HDonXuatKhoBanDaiLy
                        {
                            DLHDon = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.f.DLHDon
                            {
                                TTChung = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.e.TTChung
                                {
                                    PBan = pBien,
                                    THDon = LoaiHoaDon.PXKHangGuiBanDaiLy.GetDescription(),
                                    KHMSHDon = model.MauSo,
                                    KHHDon = model.KyHieu,
                                    SHDon = model.SoHoaDon,
                                    NLap = model.NgayHoaDon.Value.ToString("yyyy-MM-dd"),
                                    DVTTe = model.MaLoaiTien,
                                    TGia = model.TyGia,
                                    MSTTCGP = taxCode,
                                    TTHDLQuan = null,
                                    TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin>()
                                },
                                NDHDon = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.f.NDHDon
                                {
                                    NBan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.f.NBan
                                    {
                                        Ten = thongTinNguoiBan != null ? (thongTinNguoiBan.TenDonViNguoiBan ?? String.Empty) : (hoSoHDDT.TenDonVi ?? String.Empty),
                                        MST = thongTinNguoiBan != null ? thongTinNguoiBan.MaSoThueNguoiBan.Trim().Replace(" ", string.Empty) ?? hoSoHDDT.MaSoThue ?? string.Empty : hoSoHDDT.MaSoThue ?? string.Empty,
                                        HDKTSo = model.CanCuSo,
                                        HDKTNgay = model.NgayCanCu.Value.ToString("yyyy-MM-dd"),
                                        DChi = model.DiaChiKhoXuatHang,
                                        HVTNXHang = model.HoTenNguoiXuatHang,
                                        TNVChuyen = model.TenNguoiVanChuyen,
                                        HDSo = model.HopDongVanChuyenSo,
                                        PTVChuyen = model.PhuongThucVanChuyen,
                                    },
                                    NMua = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.e.NMua
                                    {
                                        Ten = model.TenKhachHang ?? string.Empty,
                                        MST = model.MaSoThue ?? string.Empty,
                                        DChi = model.DiaChiKhoNhanHang ?? string.Empty,
                                        HVTNNHang = model.HoTenNguoiNhanHang
                                    },
                                    DSHHDVu = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.f.HHDVu>()
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
                                hDonXuatKhoBanDaiLy.DLHDon.TTChung.TTHDLQuan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTHDLQuan
                                {
                                    LHDCLQuan = model.TrangThai == (int)TrangThaiHoaDon.HoaDonThayThe ? (LADHDDT)hdlq.LoaiApDungHoaDonCanThayThe : (LADHDDT)hdlq.LoaiApDungHoaDonDieuChinh
                                };

                                if (model.TrangThai == (int)TrangThaiHoaDon.HoaDonThayThe)
                                {
                                    hDonXuatKhoBanDaiLy.DLHDon.TTChung.TTHDLQuan.TCHDon = TCHDon.ThayThe;
                                    hDonXuatKhoBanDaiLy.DLHDon.TTChung.TTHDLQuan.KHMSHDCLQuan = model.LyDoThayTheModel.MauSo;
                                    hDonXuatKhoBanDaiLy.DLHDon.TTChung.TTHDLQuan.KHHDCLQuan = model.LyDoThayTheModel.KyHieu;
                                    hDonXuatKhoBanDaiLy.DLHDon.TTChung.TTHDLQuan.SHDCLQuan = model.LyDoThayTheModel.SoHoaDon;
                                    hDonXuatKhoBanDaiLy.DLHDon.TTChung.TTHDLQuan.NLHDCLQuan = model.LyDoThayTheModel.NgayHoaDon.ToString("yyyy-MM-dd");
                                    hDonXuatKhoBanDaiLy.DLHDon.TTChung.TTHDLQuan.GChu = model.LyDoThayTheModel.LyDo;
                                }
                                else
                                {
                                    hDonXuatKhoBanDaiLy.DLHDon.TTChung.TTHDLQuan.TCHDon = TCHDon.DieuChinh;
                                    hDonXuatKhoBanDaiLy.DLHDon.TTChung.TTHDLQuan.KHMSHDCLQuan = model.LyDoDieuChinhModel.MauSo;
                                    hDonXuatKhoBanDaiLy.DLHDon.TTChung.TTHDLQuan.KHHDCLQuan = model.LyDoDieuChinhModel.KyHieu;
                                    hDonXuatKhoBanDaiLy.DLHDon.TTChung.TTHDLQuan.SHDCLQuan = model.LyDoDieuChinhModel.SoHoaDon;
                                    hDonXuatKhoBanDaiLy.DLHDon.TTChung.TTHDLQuan.NLHDCLQuan = model.LyDoDieuChinhModel.NgayHoaDon.ToString("yyyy-MM-dd");
                                    hDonXuatKhoBanDaiLy.DLHDon.TTChung.TTHDLQuan.GChu = model.LyDoDieuChinhModel.LyDo;
                                }
                            }
                        }
                        #endregion

                        #region Hàng hóa chi tiết
                        stt = 0;
                        foreach (var item in model.HoaDonChiTiets)
                        {
                            var hhdvu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.f.HHDVu
                            {
                                TChat = (TChat)item.TinhChat,
                                STT = item.STT,
                                MHHDVu = item.MaHang ?? string.Empty,
                                THHDVu = item.TenHang ?? string.Empty,
                                DVTinh = item.DonViTinh?.Ten ?? string.Empty,
                                SLuong = item.SoLuong,
                                DGia = item.DonGia,
                                ThTien = item.ThanhTien,
                                TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin>()
                            };

                            hDonXuatKhoBanDaiLy.DLHDon.NDHDon.DSHHDVu.Add(hhdvu);
                        }
                        #endregion

                        if (model.TTChungThongDiep != null)
                        {
                            string mNGui = _configuration["TTChung:MNGui"];
                            string mNNhan = _configuration["TTChung:MNNhan"];
                            model.TTChungThongDiep.PBan = pBien;
                            model.TTChungThongDiep.MNGui = mNGui;
                            model.TTChungThongDiep.MNNhan = mNNhan;
                            model.TTChungThongDiep.MST = thongTinNguoiBan != null ? thongTinNguoiBan.MaSoThueNguoiBan : hoSoHDDT.MaSoThue;

                            TDiep200PXKBanDaiLy tDiep = new TDiep200PXKBanDaiLy
                            {
                                TTChung = model.TTChungThongDiep,
                                DLieu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6.DLieu8
                                {
                                    HDon = hDonXuatKhoBanDaiLy
                                }
                            };

                            GenerateXML(tDiep, xmlFilePath);
                        }
                        else
                        {
                            GenerateXML(hDonXuatKhoBanDaiLy, xmlFilePath);
                        }
                        break;
                    case LoaiHoaDon.HoaDonBanTaiSanCong:
                        if (!string.IsNullOrWhiteSpace(model.IdHoaDonSaiSotBiThayThe))
                        {
                            ttkhac_ThongTinSaiSot.Add(new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin { TTruong = "Hóa đơn liên quan", KDLieu = "string", DLieu = model.GhiChuThayTheSaiSot?.Replace("<b>", "").Replace("</b>", "") });
                        }
                        HDonTaiSanCong hDonTaiSanCong = new HDonTaiSanCong
                        {
                            DLHDon = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.c.DLHDon
                            {
                                TTChung = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.c.TTChung
                                {
                                    PBan = pBien,
                                    THDon = LoaiHoaDon.HoaDonBanTaiSanCong.GetDescription(),
                                    KHMSHDon = model.MauSo,
                                    KHHDon = model.KyHieu,
                                    SHDon = (int?)model.SoHoaDon,
                                    NLap = model.NgayHoaDon.Value.ToString("yyyy-MM-dd"),
                                    DVTTe = model.MaLoaiTien,
                                    TGia = model.TyGia,
                                    MSTTCGP = taxCode,
                                    TTHDLQuan = null,
                                    HTTToan = model.TenHinhThucThanhToan,
                                    TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin>()
                                },
                                NDHDon = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.c.NDHDon
                                {
                                    NBan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.c.NBan
                                    {
                                        Ten = thongTinNguoiBan != null ? (thongTinNguoiBan.TenDonViNguoiBan ?? String.Empty) : (hoSoHDDT.TenDonVi ?? String.Empty),
                                        MST = thongTinNguoiBan != null ? thongTinNguoiBan.MaSoThueNguoiBan.Trim().Replace(" ", string.Empty) ?? hoSoHDDT.MaSoThue ?? string.Empty : hoSoHDDT.MaSoThue ?? string.Empty,
                                        DChi = thongTinNguoiBan != null ? (thongTinNguoiBan.DiaChiNguoiBan ?? String.Empty) : (hoSoHDDT.DiaChi ?? string.Empty),
                                        SDThoai = sdtNBan,
                                        DCTDTu = thongTinNguoiBan != null ? (thongTinNguoiBan.EmailNguoiBan ?? String.Empty) : string.Empty,
                                        STKNHang = thongTinNguoiBan != null ? (thongTinNguoiBan.SoTaiKhoanNguoiBan ?? String.Empty) : string.Empty,
                                        TNHang = thongTinNguoiBan != null ? (thongTinNguoiBan.TenNganHangNguoiBan ?? String.Empty) : string.Empty,
                                        Fax = thongTinNguoiBan != null ? (thongTinNguoiBan.FaxNguoiBan ?? String.Empty) : string.Empty,
                                        Website = thongTinNguoiBan != null ? (thongTinNguoiBan.WebsiteNguoiBan ?? String.Empty) : string.Empty,
                                        SQDinh = string.Empty,
                                        NQDinh = string.Empty,
                                        CQBHQDinh = string.Empty,
                                        HTBan = String.Empty,
                                        TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin>()

                                    },
                                    NMua = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.c.NMua
                                    {
                                        Ten = model.TenKhachHang ?? string.Empty,
                                        MST = model.MaSoThue ?? string.Empty,
                                        DChi = model.DiaChi ?? string.Empty,
                                        MDVQHNSach = string.Empty,
                                        SDThoai = model.SoDienThoaiNguoiMuaHang ?? string.Empty,
                                        DCTDTu = string.Empty,
                                        HVTNMHang = model.HoTenNguoiMuaHang ?? string.Empty,
                                        STKNHang = model.SoTaiKhoanNganHang ?? string.Empty,
                                        TNHang = model.TenNganHang ?? string.Empty,
                                        DDVCHDen = string.Empty,
                                        TGVCHDTu = string.Empty,
                                        TGVCHDDen = string.Empty,
                                        TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin>()

                                    },
                                    DSHHDVu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.c.DSHHDVu(),
                                    TToan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.c.TToan
                                    {

                                        DSLPhi = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.LPhi>()
                                    {
                                        new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.LPhi()
                                    },

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
                                hDonTaiSanCong.DLHDon.TTChung.TTHDLQuan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTHDLQuan
                                {
                                    LHDCLQuan = model.TrangThai == (int)TrangThaiHoaDon.HoaDonThayThe ? (LADHDDT)hdlq.LoaiApDungHoaDonCanThayThe : (LADHDDT)hdlq.LoaiApDungHoaDonDieuChinh
                                };

                                if (model.TrangThai == (int)TrangThaiHoaDon.HoaDonThayThe)
                                {
                                    hDonTaiSanCong.DLHDon.TTChung.TTHDLQuan.TCHDon = TCHDon.ThayThe;
                                    hDonTaiSanCong.DLHDon.TTChung.TTHDLQuan.KHMSHDCLQuan = model.LyDoThayTheModel.MauSo;
                                    hDonTaiSanCong.DLHDon.TTChung.TTHDLQuan.KHHDCLQuan = model.LyDoThayTheModel.KyHieu;
                                    hDonTaiSanCong.DLHDon.TTChung.TTHDLQuan.SHDCLQuan = model.LyDoThayTheModel.SoHoaDon;
                                    hDonTaiSanCong.DLHDon.TTChung.TTHDLQuan.NLHDCLQuan = model.LyDoThayTheModel.NgayHoaDon.ToString("yyyy-MM-dd");
                                    hDonTaiSanCong.DLHDon.TTChung.TTHDLQuan.GChu = model.LyDoThayTheModel.LyDo;
                                }
                                else
                                {
                                    hDonTaiSanCong.DLHDon.TTChung.TTHDLQuan.TCHDon = TCHDon.DieuChinh;
                                    hDonTaiSanCong.DLHDon.TTChung.TTHDLQuan.KHMSHDCLQuan = model.LyDoDieuChinhModel.MauSo;
                                    hDonTaiSanCong.DLHDon.TTChung.TTHDLQuan.KHHDCLQuan = model.LyDoDieuChinhModel.KyHieu;
                                    hDonTaiSanCong.DLHDon.TTChung.TTHDLQuan.SHDCLQuan = model.LyDoDieuChinhModel.SoHoaDon;
                                    hDonTaiSanCong.DLHDon.TTChung.TTHDLQuan.NLHDCLQuan = model.LyDoDieuChinhModel.NgayHoaDon.ToString("yyyy-MM-dd");
                                    hDonTaiSanCong.DLHDon.TTChung.TTHDLQuan.GChu = model.LyDoDieuChinhModel.LyDo;
                                }
                            }
                        }
                        #endregion

                        #region Hàng hóa chi tiết
                        stt = 0;
                        foreach (var item in model.HoaDonChiTiets)
                        {
                            var hhdvu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.c.HHDVu
                            {
                                TChat = (TChat)item.TinhChat,
                                STT = item.STT,
                                MHHDVu = item.MaHang ?? string.Empty,
                                THHDVu = item.TenHang ?? string.Empty,
                                DVTinh = item.DonViTinh?.Ten ?? string.Empty,
                                SLuong = item.SoLuong,
                                DGia = item.DonGia,
                                ThTien = item.ThanhTien,
                                TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin>()
                            };

                            hDonTaiSanCong.DLHDon.NDHDon.DSHHDVu.HHDVu.Add(hhdvu);
                        }
                        #endregion

                        if (model.TTChungThongDiep != null)
                        {
                            string mNGui = _configuration["TTChung:MNGui"];
                            string mNNhan = _configuration["TTChung:MNNhan"];
                            model.TTChungThongDiep.PBan = pBien;
                            model.TTChungThongDiep.MNGui = mNGui;
                            model.TTChungThongDiep.MNNhan = mNNhan;
                            model.TTChungThongDiep.MST = thongTinNguoiBan != null ? thongTinNguoiBan.MaSoThueNguoiBan : hoSoHDDT.MaSoThue;

                            TDiep200PTaiSanCong tDiep = new TDiep200PTaiSanCong
                            {
                                TTChung = model.TTChungThongDiep,
                                DLieu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6.DLieu3
                                {
                                    HDon = hDonTaiSanCong
                                }
                            };

                            GenerateXML(tDiep, xmlFilePath);
                        }
                        else
                        {
                            GenerateXML(hDonTaiSanCong, xmlFilePath);
                        }
                        break;
                    case LoaiHoaDon.HoaDonBanHangDuTruQuocGia:
                        if (!string.IsNullOrWhiteSpace(model.IdHoaDonSaiSotBiThayThe))
                        {
                            ttkhac_ThongTinSaiSot.Add(new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin { TTruong = "Hóa đơn liên quan", KDLieu = "string", DLieu = model.GhiChuThayTheSaiSot?.Replace("<b>", "").Replace("</b>", "") });
                        }
                        HDonDuTruQuocGia hDonDuTruQuocGia = new HDonDuTruQuocGia
                        {
                            DLHDon = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.d.DLHDon
                            {
                                TTChung = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.d.TTChung
                                {
                                    PBan = pBien,
                                    THDon = LoaiHoaDon.HoaDonBanHangDuTruQuocGia.GetDescription(),
                                    KHMSHDon = model.MauSo,
                                    KHHDon = model.KyHieu,
                                    SHDon = (int?)model.SoHoaDon,
                                    NLap = model.NgayHoaDon.Value.ToString("yyyy-MM-dd"),
                                    DVTTe = model.MaLoaiTien,
                                    TGia = model.TyGia,
                                    MSTTCGP = taxCode,
                                    TTHDLQuan = null,
                                    HTTToan = model.TenHinhThucThanhToan,
                                    TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin>()
                                },
                                NDHDon = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.d.NDHDon
                                {
                                    NBan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.NBan
                                    {
                                        Ten = thongTinNguoiBan != null ? (thongTinNguoiBan.TenDonViNguoiBan ?? String.Empty) : (hoSoHDDT.TenDonVi ?? String.Empty),
                                        MST = thongTinNguoiBan != null ? thongTinNguoiBan.MaSoThueNguoiBan.Trim().Replace(" ", string.Empty) ?? hoSoHDDT.MaSoThue ?? string.Empty : hoSoHDDT.MaSoThue ?? string.Empty,
                                        DChi = thongTinNguoiBan != null ? (thongTinNguoiBan.DiaChiNguoiBan ?? String.Empty) : (hoSoHDDT.DiaChi ?? string.Empty),
                                        SDThoai = sdtNBan,
                                        DCTDTu = thongTinNguoiBan != null ? (thongTinNguoiBan.EmailNguoiBan ?? String.Empty) : string.Empty,
                                        STKNHang = thongTinNguoiBan != null ? (thongTinNguoiBan.SoTaiKhoanNguoiBan ?? String.Empty) : string.Empty,
                                        TNHang = thongTinNguoiBan != null ? (thongTinNguoiBan.TenNganHangNguoiBan ?? String.Empty) : string.Empty,
                                        Fax = thongTinNguoiBan != null ? (thongTinNguoiBan.FaxNguoiBan ?? String.Empty) : string.Empty,
                                        Website = thongTinNguoiBan != null ? (thongTinNguoiBan.WebsiteNguoiBan ?? String.Empty) : string.Empty,
                                        TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin>()

                                    },
                                    NMua = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.d.NMua
                                    {
                                        Ten = model.TenKhachHang ?? string.Empty,
                                        MST = model.MaSoThue ?? string.Empty,
                                        DChi = model.DiaChi ?? string.Empty,
                                        CMND = model.CMND ?? string.Empty,
                                        SDThoai = model.SoDienThoaiNguoiMuaHang ?? string.Empty,
                                        DCTDTu = string.Empty,
                                        HVTNMHang = model.HoTenNguoiMuaHang ?? string.Empty,
                                        STKNHang = model.SoTaiKhoanNganHang ?? string.Empty,
                                        TNHang = model.TenNganHang ?? string.Empty,
                                        TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin>()

                                    },
                                    DSHHDVu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.c.DSHHDVu(),
                                    TToan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.c.TToan
                                    {

                                        DSLPhi = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.LPhi>()
                                    {
                                        new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.LPhi()
                                    },

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
                                hDonDuTruQuocGia.DLHDon.TTChung.TTHDLQuan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTHDLQuan
                                {
                                    LHDCLQuan = model.TrangThai == (int)TrangThaiHoaDon.HoaDonThayThe ? (LADHDDT)hdlq.LoaiApDungHoaDonCanThayThe : (LADHDDT)hdlq.LoaiApDungHoaDonDieuChinh
                                };

                                if (model.TrangThai == (int)TrangThaiHoaDon.HoaDonThayThe)
                                {
                                    hDonDuTruQuocGia.DLHDon.TTChung.TTHDLQuan.TCHDon = TCHDon.ThayThe;
                                    hDonDuTruQuocGia.DLHDon.TTChung.TTHDLQuan.KHMSHDCLQuan = model.LyDoThayTheModel.MauSo;
                                    hDonDuTruQuocGia.DLHDon.TTChung.TTHDLQuan.KHHDCLQuan = model.LyDoThayTheModel.KyHieu;
                                    hDonDuTruQuocGia.DLHDon.TTChung.TTHDLQuan.SHDCLQuan = model.LyDoThayTheModel.SoHoaDon;
                                    hDonDuTruQuocGia.DLHDon.TTChung.TTHDLQuan.NLHDCLQuan = model.LyDoThayTheModel.NgayHoaDon.ToString("yyyy-MM-dd");
                                    hDonDuTruQuocGia.DLHDon.TTChung.TTHDLQuan.GChu = model.LyDoThayTheModel.LyDo;
                                }
                                else
                                {
                                    hDonDuTruQuocGia.DLHDon.TTChung.TTHDLQuan.TCHDon = TCHDon.DieuChinh;
                                    hDonDuTruQuocGia.DLHDon.TTChung.TTHDLQuan.KHMSHDCLQuan = model.LyDoDieuChinhModel.MauSo;
                                    hDonDuTruQuocGia.DLHDon.TTChung.TTHDLQuan.KHHDCLQuan = model.LyDoDieuChinhModel.KyHieu;
                                    hDonDuTruQuocGia.DLHDon.TTChung.TTHDLQuan.SHDCLQuan = model.LyDoDieuChinhModel.SoHoaDon;
                                    hDonDuTruQuocGia.DLHDon.TTChung.TTHDLQuan.NLHDCLQuan = model.LyDoDieuChinhModel.NgayHoaDon.ToString("yyyy-MM-dd");
                                    hDonDuTruQuocGia.DLHDon.TTChung.TTHDLQuan.GChu = model.LyDoDieuChinhModel.LyDo;
                                }
                            }
                        }
                        #endregion

                        #region Hàng hóa chi tiết
                        stt = 0;
                        foreach (var item in model.HoaDonChiTiets)
                        {
                            var hhdvu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.c.HHDVu
                            {
                                TChat = (TChat)item.TinhChat,
                                STT = item.STT,
                                MHHDVu = item.MaHang ?? string.Empty,
                                THHDVu = item.TenHang ?? string.Empty,
                                DVTinh = item.DonViTinh?.Ten ?? string.Empty,
                                SLuong = item.SoLuong,
                                DGia = item.DonGia,
                                ThTien = item.ThanhTien,
                                TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin>()
                            };

                            hDonDuTruQuocGia.DLHDon.NDHDon.DSHHDVu.HHDVu.Add(hhdvu);
                        }
                        #endregion

                        if (model.TTChungThongDiep != null)
                        {
                            string mNGui = _configuration["TTChung:MNGui"];
                            string mNNhan = _configuration["TTChung:MNNhan"];
                            model.TTChungThongDiep.PBan = pBien;
                            model.TTChungThongDiep.MNGui = mNGui;
                            model.TTChungThongDiep.MNNhan = mNNhan;
                            model.TTChungThongDiep.MST = thongTinNguoiBan != null ? thongTinNguoiBan.MaSoThueNguoiBan : hoSoHDDT.MaSoThue;

                            TDiep200DuTruQuocGia tDiep = new TDiep200DuTruQuocGia
                            {
                                TTChung = model.TTChungThongDiep,
                                DLieu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6.DLieu4
                                {
                                    HDon = hDonDuTruQuocGia
                                }
                            };

                            GenerateXML(tDiep, xmlFilePath);
                        }
                        else
                        {
                            GenerateXML(hDonDuTruQuocGia, xmlFilePath);
                        }
                        break;
                    case LoaiHoaDon.CacLoaiHoaDonKhac:
                        if (!string.IsNullOrWhiteSpace(model.IdHoaDonSaiSotBiThayThe))
                        {
                            ttkhac_ThongTinSaiSot.Add(new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin { TTruong = "Hóa đơn liên quan", KDLieu = "string", DLieu = model.GhiChuThayTheSaiSot?.Replace("<b>", "").Replace("</b>", "") });
                        }
                        HDonKhac hDonKhac = new HDonKhac
                        {
                            DLHDon = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.g.DLHDon
                            {
                                TTChung = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.g.TTChung
                                {
                                    PBan = pBien,
                                    THDon = LoaiHoaDon.CacLoaiHoaDonKhac.GetDescription(),
                                    KHMSHDon = model.MauSo,
                                    KHHDon = model.KyHieu,
                                    SHDon = (int?)model.SoHoaDon,
                                    NLap = model.NgayHoaDon.Value.ToString("yyyy-MM-dd"),
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
                                NDHDon = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.g.NDHDon
                                {
                                    NBan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.NBan
                                    {
                                        Ten = thongTinNguoiBan != null ? (thongTinNguoiBan.TenDonViNguoiBan ?? String.Empty) : (hoSoHDDT.TenDonVi ?? String.Empty),
                                        MST = thongTinNguoiBan != null ? thongTinNguoiBan.MaSoThueNguoiBan.Trim().Replace(" ", string.Empty) ?? hoSoHDDT.MaSoThue ?? string.Empty : hoSoHDDT.MaSoThue ?? string.Empty,
                                        DChi = thongTinNguoiBan != null ? (thongTinNguoiBan.DiaChiNguoiBan ?? String.Empty) : (hoSoHDDT.DiaChi ?? string.Empty),
                                        SDThoai = sdtNBan,
                                        DCTDTu = thongTinNguoiBan != null ? (thongTinNguoiBan.EmailNguoiBan ?? String.Empty) : string.Empty,
                                        STKNHang = thongTinNguoiBan != null ? (thongTinNguoiBan.SoTaiKhoanNguoiBan ?? String.Empty) : string.Empty,
                                        TNHang = thongTinNguoiBan != null ? (thongTinNguoiBan.TenNganHangNguoiBan ?? String.Empty) : string.Empty,
                                        Fax = thongTinNguoiBan != null ? (thongTinNguoiBan.FaxNguoiBan ?? String.Empty) : string.Empty,
                                        Website = thongTinNguoiBan != null ? (thongTinNguoiBan.WebsiteNguoiBan ?? String.Empty) : string.Empty,
                                        TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin>()

                                    },
                                    NMua = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.g.NMua
                                    {
                                        Ten = model.TenKhachHang ?? string.Empty,
                                        MST = model.MaSoThue ?? string.Empty,
                                        DChi = model.DiaChi ?? string.Empty,
                                        MKHang = model.MaKhachHang ?? string.Empty,
                                        SDThoai = model.SoDienThoaiNguoiMuaHang ?? string.Empty,
                                        DCTDTu = string.Empty,
                                        HVTNMHang = model.HoTenNguoiMuaHang ?? string.Empty,
                                        STKNHang = model.SoTaiKhoanNganHang ?? string.Empty,
                                        TNHang = model.TenNganHang ?? string.Empty,
                                        TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin>()

                                    },
                                    DSHHDVu = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.g.HHDVu>(),
                                    TToan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.g.TToan
                                    {
                                        THTTLTSuat = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.g.LTSuat>(),
                                        TgTCThue = model.TongTienHang ?? 0,
                                        TgTThue = model.TongTienThueGTGT ?? 0,
                                        TGTKCThue = model.TGTKCThue ?? 0,
                                        TGTKhac = model.TGTKhac ?? 0,
                                        DSLPhi = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.g.LPhi>()
                                    {
                                        new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.g.LPhi()
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
                                hDonKhac.DLHDon.TTChung.TTHDLQuan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTHDLQuan
                                {
                                    LHDCLQuan = model.TrangThai == (int)TrangThaiHoaDon.HoaDonThayThe ? (LADHDDT)hdlq.LoaiApDungHoaDonCanThayThe : (LADHDDT)hdlq.LoaiApDungHoaDonDieuChinh
                                };

                                if (model.TrangThai == (int)TrangThaiHoaDon.HoaDonThayThe)
                                {
                                    hDonKhac.DLHDon.TTChung.TTHDLQuan.TCHDon = TCHDon.ThayThe;
                                    hDonKhac.DLHDon.TTChung.TTHDLQuan.KHMSHDCLQuan = model.LyDoThayTheModel.MauSo;
                                    hDonKhac.DLHDon.TTChung.TTHDLQuan.KHHDCLQuan = model.LyDoThayTheModel.KyHieu;
                                    hDonKhac.DLHDon.TTChung.TTHDLQuan.SHDCLQuan = model.LyDoThayTheModel.SoHoaDon;
                                    hDonKhac.DLHDon.TTChung.TTHDLQuan.NLHDCLQuan = model.LyDoThayTheModel.NgayHoaDon.ToString("yyyy-MM-dd");
                                    hDonKhac.DLHDon.TTChung.TTHDLQuan.GChu = model.LyDoThayTheModel.LyDo;
                                }
                                else
                                {
                                    hDonKhac.DLHDon.TTChung.TTHDLQuan.TCHDon = TCHDon.DieuChinh;
                                    hDonKhac.DLHDon.TTChung.TTHDLQuan.KHMSHDCLQuan = model.LyDoDieuChinhModel.MauSo;
                                    hDonKhac.DLHDon.TTChung.TTHDLQuan.KHHDCLQuan = model.LyDoDieuChinhModel.KyHieu;
                                    hDonKhac.DLHDon.TTChung.TTHDLQuan.SHDCLQuan = model.LyDoDieuChinhModel.SoHoaDon;
                                    hDonKhac.DLHDon.TTChung.TTHDLQuan.NLHDCLQuan = model.LyDoDieuChinhModel.NgayHoaDon.ToString("yyyy-MM-dd");
                                    hDonKhac.DLHDon.TTChung.TTHDLQuan.GChu = model.LyDoDieuChinhModel.LyDo;
                                }
                            }
                        }
                        #endregion

                        #region Hàng hóa chi tiết
                        stt = 0;
                        foreach (var item in model.HoaDonChiTiets)
                        {
                            var hhdvu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.g.HHDVu
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

                            hDonKhac.DLHDon.NDHDon.DSHHDVu.Add(hhdvu);
                        }
                        #endregion

                        if (model.TTChungThongDiep != null)
                        {
                            string mNGui = _configuration["TTChung:MNGui"];
                            string mNNhan = _configuration["TTChung:MNNhan"];

                            model.TTChungThongDiep.PBan = pBien;
                            model.TTChungThongDiep.MNGui = mNGui;
                            model.TTChungThongDiep.MNNhan = mNNhan;
                            model.TTChungThongDiep.MST = thongTinNguoiBan != null ? thongTinNguoiBan.MaSoThueNguoiBan : hoSoHDDT.MaSoThue;

                            TDiep200HDonKhac tDiep = new TDiep200HDonKhac
                            {

                                TTChung = model.TTChungThongDiep,
                                DLieu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6.DLieu5
                                {
                                    HDon = hDonKhac
                                }
                            };

                            GenerateXML(tDiep, xmlFilePath);
                        }
                        else
                        {
                            GenerateXML(hDonKhac, xmlFilePath);
                        }
                        break;
                    case LoaiHoaDon.HoaDonNhieuTyGia:
                        if (!string.IsNullOrWhiteSpace(model.IdHoaDonSaiSotBiThayThe))
                        {
                            ttkhac_ThongTinSaiSot.Add(new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin { TTruong = "Hóa đơn liên quan", KDLieu = "string", DLieu = model.GhiChuThayTheSaiSot?.Replace("<b>", "").Replace("</b>", "") });
                        }
                        HDonNhieuTyGia hDonNhieuTyGia = new HDonNhieuTyGia
                        {
                            DLHDon = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.k.DLHDon
                            {
                                TTChung = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.k.TTChung
                                {
                                    PBan = pBien,
                                    THDon = LoaiHoaDon.PXKHangGuiBanDaiLy.GetDescription(),
                                    KHMSHDon = model.MauSo,
                                    KHHDon = model.KyHieu,
                                    SHDon = (int?)model.SoHoaDon,
                                    NLap = model.NgayHoaDon.Value.ToString("yyyy-MM-dd"),
                                    SBKe = string.Empty,
                                    NBKe = string.Empty,
                                    HTTToan = model.TenHinhThucThanhToan,
                                    MSTTCGP = taxCode,
                                    HDNTGia = model.HDNTGia.Value,
                                    MSTDVNUNLHDon = string.Empty,
                                    TDVNUNLHDon = string.Empty,
                                    DCDVNUNLHDon = string.Empty,
                                    TTHDLQuan = null,
                                    TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin>()
                                },
                                NDHDon = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.k.NDHDon
                                {
                                    NBan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.k.NBan
                                    {
                                        Ten = thongTinNguoiBan != null ? (thongTinNguoiBan.TenDonViNguoiBan ?? String.Empty) : (hoSoHDDT.TenDonVi ?? String.Empty),
                                        MST = thongTinNguoiBan != null ? thongTinNguoiBan.MaSoThueNguoiBan.Trim().Replace(" ", string.Empty) ?? hoSoHDDT.MaSoThue ?? string.Empty : hoSoHDDT.MaSoThue ?? string.Empty,
                                        DChi = thongTinNguoiBan != null ? (thongTinNguoiBan.DiaChiNguoiBan ?? String.Empty) : (hoSoHDDT.DiaChi ?? string.Empty),
                                        SDThoai = sdtNBan,
                                        DCTDTu = thongTinNguoiBan != null ? (thongTinNguoiBan.EmailNguoiBan ?? String.Empty) : string.Empty,
                                        STKNHang = thongTinNguoiBan != null ? (thongTinNguoiBan.SoTaiKhoanNguoiBan ?? String.Empty) : string.Empty,
                                        TNHang = thongTinNguoiBan != null ? (thongTinNguoiBan.TenNganHangNguoiBan ?? String.Empty) : string.Empty,
                                        Fax = thongTinNguoiBan != null ? (thongTinNguoiBan.FaxNguoiBan ?? String.Empty) : string.Empty,
                                        Website = thongTinNguoiBan != null ? (thongTinNguoiBan.WebsiteNguoiBan ?? String.Empty) : string.Empty,
                                        TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin>()

                                    },
                                    NMua = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.k.NMua
                                    {
                                        Ten = model.TenKhachHang ?? string.Empty,
                                        MST = model.MaSoThue ?? string.Empty,
                                        DChi = model.DiaChi ?? string.Empty,
                                        MKHang = model.MaKhachHang ?? string.Empty,
                                        SDThoai = model.SoDienThoaiNguoiMuaHang ?? string.Empty,
                                        DCTDTu = string.Empty,
                                        HVTNMHang = model.HoTenNguoiMuaHang ?? string.Empty,
                                        STKNHang = model.SoTaiKhoanNganHang ?? string.Empty,
                                        TNHang = model.TenNganHang ?? string.Empty,
                                        TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin>()

                                    },
                                    DSHHDVu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.k.DSHHDVu(),
                                    TToan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.k.TToan
                                    {
                                        THTTLTSuat = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.k.LTSuat>(),
                                        TgTCThue = model.TongTienHang ?? 0,
                                        TgTThue = model.TongTienThueGTGT ?? 0,
                                        TGTKCThue = model.TGTKCThue ?? 0,
                                        TGTKhac = model.TGTKhac ?? 0,
                                        DSLPhi = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.k.LPhi>()
                                    {
                                        new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.k.LPhi()
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
                                //hDonNhieuTyGia.DLHDon.TTChung.TTHDLQuan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTHDLQuan
                                //{
                                //    LHDCLQuan = model.TrangThai == (int)TrangThaiHoaDon.HoaDonThayThe ? (LADHDDT)hdlq.LoaiApDungHoaDonCanThayThe : (LADHDDT)hdlq.LoaiApDungHoaDonDieuChinh
                                //};

                                if (model.TrangThai == (int)TrangThaiHoaDon.HoaDonThayThe)
                                {
                                    hDonNhieuTyGia.DLHDon.TTChung.TTHDLQuan.TCHDon = TCHDon.ThayThe;
                                    hDonNhieuTyGia.DLHDon.TTChung.TTHDLQuan.KHMSHDCLQuan = model.LyDoThayTheModel.MauSo;
                                    hDonNhieuTyGia.DLHDon.TTChung.TTHDLQuan.KHHDCLQuan = model.LyDoThayTheModel.KyHieu;
                                    hDonNhieuTyGia.DLHDon.TTChung.TTHDLQuan.SHDCLQuan = model.LyDoThayTheModel.SoHoaDon;
                                    hDonNhieuTyGia.DLHDon.TTChung.TTHDLQuan.NLHDCLQuan = model.LyDoThayTheModel.NgayHoaDon.ToString("yyyy-MM-dd");
                                    hDonNhieuTyGia.DLHDon.TTChung.TTHDLQuan.GChu = model.LyDoThayTheModel.LyDo;
                                }
                                else
                                {
                                    hDonNhieuTyGia.DLHDon.TTChung.TTHDLQuan.TCHDon = TCHDon.DieuChinh;
                                    hDonNhieuTyGia.DLHDon.TTChung.TTHDLQuan.KHMSHDCLQuan = model.LyDoDieuChinhModel.MauSo;
                                    hDonNhieuTyGia.DLHDon.TTChung.TTHDLQuan.KHHDCLQuan = model.LyDoDieuChinhModel.KyHieu;
                                    hDonNhieuTyGia.DLHDon.TTChung.TTHDLQuan.SHDCLQuan = model.LyDoDieuChinhModel.SoHoaDon;
                                    hDonNhieuTyGia.DLHDon.TTChung.TTHDLQuan.NLHDCLQuan = model.LyDoDieuChinhModel.NgayHoaDon.ToString("yyyy-MM-dd");
                                    hDonNhieuTyGia.DLHDon.TTChung.TTHDLQuan.GChu = model.LyDoDieuChinhModel.LyDo;
                                }
                            }
                        }
                        #endregion

                        #region Hàng hóa chi tiết
                        stt = 0;
                        foreach (var item in model.HoaDonChiTiets)
                        {
                            var hhdvu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.k.HHDVu
                            {
                                TChat = (TChat)item.TinhChat,
                                STT = item.STT,
                                MHHDVu = item.MaHang ?? string.Empty,
                                THHDVu = item.TenHang ?? string.Empty,
                                DVTinh = item.DonViTinh?.Ten ?? string.Empty,
                                SLuong = item.SoLuong,
                                DGia = item.DonGia,
                                ThTien = item.ThanhTien,
                                TLCKhau = item.TyLeChietKhau,
                                STCKhau = item.TienChietKhau,

                                TSuat = item.ThueGTGT.GetThueHasPer(),
                                TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin>()
                            };

                            hDonNhieuTyGia.DLHDon.NDHDon.DSHHDVu.HHDVu.Add(hhdvu);
                        }
                        #endregion

                        if (model.TTChungThongDiep != null)
                        {
                            string mNGui = _configuration["TTChung:MNGui"];
                            string mNNhan = _configuration["TTChung:MNNhan"];
                            model.TTChungThongDiep.PBan = pBien;
                            model.TTChungThongDiep.MNGui = mNGui;
                            model.TTChungThongDiep.MNNhan = mNNhan;
                            model.TTChungThongDiep.MST = thongTinNguoiBan != null ? thongTinNguoiBan.MaSoThueNguoiBan : hoSoHDDT.MaSoThue;
                            TDiep200NhieuTyGia tDiep = new TDiep200NhieuTyGia
                            {
                                TTChung = model.TTChungThongDiep,
                                DLieu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6.DLieu11
                                {
                                    HDon = hDonNhieuTyGia
                                }
                            };

                            GenerateXML(tDiep, xmlFilePath);
                        }
                        else
                        {
                            GenerateXML(hDonNhieuTyGia, xmlFilePath);
                        }
                        break;
                    case LoaiHoaDon.HoaDonGTGTKiemToKhaiHoanThue:
                        if (!string.IsNullOrWhiteSpace(model.IdHoaDonSaiSotBiThayThe))
                        {
                            ttkhac_ThongTinSaiSot.Add(new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin { TTruong = "Hóa đơn liên quan", KDLieu = "string", DLieu = model.GhiChuThayTheSaiSot?.Replace("<b>", "").Replace("</b>", "") });
                        }
                        HDonGTGTKiemToKhai hDonGTGTKiemToKhai = new HDonGTGTKiemToKhai
                        {
                            DLHDon = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.l.DLHDon
                            {
                                TTChung = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.l.TTChung
                                {
                                    PBan = pBien,
                                    THDon = LoaiHoaDon.PXKHangGuiBanDaiLy.GetDescription(),
                                    KHMSHDon = model.MauSo,
                                    KHHDon = model.KyHieu,
                                    SHDon = (int?)model.SoHoaDon,
                                    NLap = model.NgayHoaDon.Value.ToString("yyyy-MM-dd"),
                                    DVTTe = model.MaLoaiTien,
                                    TGia = model.TyGia,

                                    TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin>()
                                },
                                NDHDon = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.l.NDHDon
                                {
                                    NBan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.l.NBan
                                    {
                                        Ten = thongTinNguoiBan != null ? (thongTinNguoiBan.TenDonViNguoiBan ?? String.Empty) : (hoSoHDDT.TenDonVi ?? String.Empty),
                                        MST = thongTinNguoiBan != null ? thongTinNguoiBan.MaSoThueNguoiBan.Trim().Replace(" ", string.Empty) ?? hoSoHDDT.MaSoThue ?? string.Empty : hoSoHDDT.MaSoThue ?? string.Empty,
                                        DChi = thongTinNguoiBan != null ? (thongTinNguoiBan.DiaChiNguoiBan ?? String.Empty) : (hoSoHDDT.DiaChi ?? string.Empty),

                                        TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin>()

                                    },
                                    NMua = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.l.NMua
                                    {
                                        Ten = model.TenKhachHang ?? string.Empty,
                                        SHChieu = model.SHChieu ?? string.Empty,
                                        NCHChieu = model.NCHChieu.ToString(),
                                        NHHHChieu = model.NHHHChieu.ToString(),
                                        QTich = model.QTich ?? string.Empty,

                                    },
                                    DSHHDVu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.k.DSHHDVu(),
                                    TToan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.l.TToan
                                    {
                                        THTTLTSuat = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.l.LTSuat>(),
                                        TgTCThue = model.TongTienHang ?? 0,
                                        TgTThue = model.TongTienThueGTGT ?? 0,

                                        DSLPhi = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.l.LPhi>()
                                    {
                                        new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.l.LPhi()
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
                                //hDonGTGTKiemToKhai.DLHDon.TTChung.TTHDLQuan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTHDLQuan
                                //{
                                //    LHDCLQuan = model.TrangThai == (int)TrangThaiHoaDon.HoaDonThayThe ? (LADHDDT)hdlq.LoaiApDungHoaDonCanThayThe : (LADHDDT)hdlq.LoaiApDungHoaDonDieuChinh
                                //};

                                //if (model.TrangThai == (int)TrangThaiHoaDon.HoaDonThayThe)
                                //{
                                //    hDonGTGTKiemToKhai.DLHDon.TTChung.TTHDLQuan.TCHDon = TCHDon.ThayThe;
                                //    hDonGTGTKiemToKhai.DLHDon.TTChung.TTHDLQuan.KHMSHDCLQuan = model.LyDoThayTheModel.MauSo;
                                //    hDonGTGTKiemToKhai.DLHDon.TTChung.TTHDLQuan.KHHDCLQuan = model.LyDoThayTheModel.KyHieu;
                                //    hDonGTGTKiemToKhai.DLHDon.TTChung.TTHDLQuan.SHDCLQuan = model.LyDoThayTheModel.SoHoaDon;
                                //    hDonGTGTKiemToKhai.DLHDon.TTChung.TTHDLQuan.NLHDCLQuan = model.LyDoThayTheModel.NgayHoaDon.ToString("yyyy-MM-dd");
                                //    hDonGTGTKiemToKhai.DLHDon.TTChung.TTHDLQuan.GChu = model.LyDoThayTheModel.LyDo;
                                //}
                                //else
                                //{
                                //    hDonGTGTKiemToKhai.DLHDon.TTChung.TTHDLQuan.TCHDon = TCHDon.DieuChinh;
                                //    hDonGTGTKiemToKhai.DLHDon.TTChung.TTHDLQuan.KHMSHDCLQuan = model.LyDoDieuChinhModel.MauSo;
                                //    hDonGTGTKiemToKhai.DLHDon.TTChung.TTHDLQuan.KHHDCLQuan = model.LyDoDieuChinhModel.KyHieu;
                                //    hDonGTGTKiemToKhai.DLHDon.TTChung.TTHDLQuan.SHDCLQuan = model.LyDoDieuChinhModel.SoHoaDon;
                                //    hDonGTGTKiemToKhai.DLHDon.TTChung.TTHDLQuan.NLHDCLQuan = model.LyDoDieuChinhModel.NgayHoaDon.ToString("yyyy-MM-dd");
                                //    hDonGTGTKiemToKhai.DLHDon.TTChung.TTHDLQuan.GChu = model.LyDoDieuChinhModel.LyDo;
                                //}
                            }
                        }
                        #endregion

                        #region Hàng hóa chi tiết
                        stt = 0;
                        foreach (var item in model.HoaDonChiTiets)
                        {
                            var hhdvu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.k.HHDVu
                            {
                                TChat = (TChat)item.TinhChat,
                                STT = item.STT,
                                MHHDVu = item.MaHang ?? string.Empty,
                                THHDVu = item.TenHang ?? string.Empty,
                                DVTinh = item.DonViTinh?.Ten ?? string.Empty,
                                SLuong = item.SoLuong,
                                DGia = item.DonGia,
                                ThTien = item.ThanhTien,
                                TLCKhau = item.TyLeChietKhau,
                                STCKhau = item.TienChietKhau,

                                TSuat = item.ThueGTGT.GetThueHasPer(),
                                TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTin>()
                            };

                            hDonGTGTKiemToKhai.DLHDon.NDHDon.DSHHDVu.HHDVu.Add(hhdvu);
                        }
                        #endregion

                        if (model.TTChungThongDiep != null)
                        {
                            string mNGui = _configuration["TTChung:MNGui"];
                            string mNNhan = _configuration["TTChung:MNNhan"];
                            model.TTChungThongDiep.PBan = pBien;
                            model.TTChungThongDiep.MNGui = mNGui;
                            model.TTChungThongDiep.MNNhan = mNNhan;
                            model.TTChungThongDiep.MST = thongTinNguoiBan != null ? thongTinNguoiBan.MaSoThueNguoiBan : hoSoHDDT.MaSoThue;

                            TDiep200GTGTKiemToKhai tDiep = new TDiep200GTGTKiemToKhai
                            {
                                TTChung = model.TTChungThongDiep,
                                DLieu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6.DLieu12
                                {
                                    HDon = hDonGTGTKiemToKhai
                                }
                            };

                            GenerateXML(tDiep, xmlFilePath);
                        }
                        else
                        {
                            GenerateXML(hDonGTGTKiemToKhai, xmlFilePath);
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Tracert.WriteLog(ex.Message);
            }
        }

        public async Task CreateThongDiepGuiHoaDonCMTMTT(string xmlFilePathGTGT, string xmlFilePathBanHang, string xmlFilePathKhac, List<HoaDonDienTuViewModel> lstHoaDon, TTChungThongDiep ttChungGTGT, TTChungThongDiep ttChungBanHang, TTChungThongDiep ttChungKhac)
        {
            try
            {
                if (ttChungGTGT.SLuong == 0) ttChungGTGT.SLuong = lstHoaDon.Count(x => x.LoaiHoaDon == (int)LoaiHoaDon.HoaDonGTGTCMTMTT);
                if (ttChungBanHang.SLuong == 0) ttChungBanHang.SLuong = lstHoaDon.Count(x => x.LoaiHoaDon == (int)LoaiHoaDon.HoaDonBanHangCMTMTT);
                if (ttChungKhac.SLuong == 0) ttChungKhac.SLuong = lstHoaDon.Count(x => x.LoaiHoaDon == (int)LoaiHoaDon.HoaDonKhacCMTMTT);
                var pBien1 = "2.0.1";
                string mNGui = _configuration["TTChung:MNGui"];
                string mNNhan = _configuration["TTChung:MNNhan"];


                ttChungGTGT.MNGui = mNGui;
                ttChungGTGT.MNNhan = mNNhan;

                var gtgtOver = false;
                var bhOver = false;
                var khacOver = false;

                var thongDiepGTGT = new TDiep200GTGTPos
                {
                    TTChung = ttChungGTGT,
                    DLieu = new List<HoaDonGTGTPos>(),
                    CKSNNT = string.Empty
                };

                ttChungBanHang.MNGui = mNGui;
                ttChungBanHang.MNNhan = mNNhan;

                var thongDiepBanHang = new TDiep200BHPos
                {
                    TTChung = ttChungBanHang,
                    DLieu = new List<HoaDonBHPos>(),
                    CKSNNT = string.Empty
                };

                ttChungKhac.MNGui = mNGui;
                ttChungKhac.MNNhan = mNNhan;

                var thongDiepKhac = new TDiep200KhacPos
                {
                    TTChung = ttChungKhac,
                    DLieu = new List<HoaDonKhacPos>(),
                    CKSNNT = String.Empty
                };

                int stt = 0;

                foreach (var model in lstHoaDon)
                {
                    //var model = await GetHDByIdAsync(it.HoaDonDienTuId);
                    // Gọi API đọc thông tin người bán trên mẫu hóa đơn của hóa đơn
                    //var listThongTinNguoiBan = await _digitalSignerNameReaderService.GetThongTinNguoiBanTuHoaDonAsync(new string[] { model.HoaDonDienTuId });
                    //var thongTinNguoiBan = listThongTinNguoiBan?.FirstOrDefault();
                    var thongTinNguoiBan = model.ThongTinNguoiBan;

                    var hoSoHDDT = await _hoSoHDDTService.GetDetailAsync();

                    if (model.BoKyHieuHoaDon.HinhThucHoaDon != HinhThucHoaDon.CoMaTuMayTinhTien) return;
                    var sdtNBan = thongTinNguoiBan != null ? thongTinNguoiBan.DienThoaiNguoiBan != null ? thongTinNguoiBan.DienThoaiNguoiBan.Trim() : hoSoHDDT.SoDienThoaiLienHe.Trim() : hoSoHDDT.SoDienThoaiLienHe.Trim();
                    if (sdtNBan.Length > 20) sdtNBan = !string.IsNullOrEmpty(hoSoHDDT.SoDienThoaiLienHe) ? hoSoHDDT.SoDienThoaiLienHe.Trim() : "";
                    var stkNBan = thongTinNguoiBan != null ? thongTinNguoiBan.SoTaiKhoanNguoiBan != null ? thongTinNguoiBan.SoTaiKhoanNguoiBan.Trim() : hoSoHDDT.SoTaiKhoanNganHang.Trim() : hoSoHDDT.SoTaiKhoanNganHang.Trim();
                    if (stkNBan.Length > 30) stkNBan = !string.IsNullOrEmpty(hoSoHDDT.SoTaiKhoanNganHang) ? hoSoHDDT.SoTaiKhoanNganHang.Trim() : "";

                    switch (model.LoaiHoaDon)
                    {
                        case (int)LoaiHoaDon.HoaDonGTGTCMTMTT:
                            if (string.IsNullOrEmpty(model.TenKhachHang))
                            {
                                model.TenKhachHang = model.HoTenNguoiMuaHang;
                            }
                            var ttKhac_ThongTinSaiSot_PostGTGT = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.TTin>();

                            if (!string.IsNullOrWhiteSpace(model.IdHoaDonSaiSotBiThayThe))
                            {
                                ttKhac_ThongTinSaiSot_PostGTGT.Add(new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.TTin { TTruong = "Hóa đơn liên quan", KDLieu = "string", DLieu = model.GhiChuThayTheSaiSot?.Replace("<b>", "").Replace("</b>", "") });
                            }

                            HoaDonGTGTPos hDonGTGTPos = new HoaDonGTGTPos
                            {
                                DLHDon = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.DLHDon
                                {
                                    TTChung = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.TTChung
                                    {
                                        PBan = pBien1,
                                        THDon = LoaiHoaDon.HoaDonGTGTCMTMTT.GetDescription(),
                                        KHMSHDon = model.MauSo,
                                        KHHDon = model.KyHieu,
                                        SHDon = model.SoHoaDon,
                                        NLap = model.NgayHoaDon.Value.ToString("yyyy-MM-dd"),
                                        TTHDLQuan = null,
                                        TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.TTin>()
                                    },
                                    NDHDon = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.NDHDon
                                    {
                                        NBan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.NBan
                                        {
                                            Ten = thongTinNguoiBan != null ? (thongTinNguoiBan.TenDonViNguoiBan ?? String.Empty) : (hoSoHDDT.TenDonVi ?? String.Empty),
                                            MST = thongTinNguoiBan != null ? thongTinNguoiBan.MaSoThueNguoiBan.Trim().Replace(" ", string.Empty) ?? hoSoHDDT.MaSoThue ?? string.Empty : hoSoHDDT.MaSoThue ?? string.Empty,
                                            DChi = thongTinNguoiBan != null ? (thongTinNguoiBan.DiaChiNguoiBan ?? String.Empty) : (hoSoHDDT.DiaChi ?? string.Empty),
                                            SDThoai = sdtNBan,
                                        },
                                        NMua = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.NMua
                                        {
                                            Ten = model.TenKhachHang ?? string.Empty,
                                            MST = model.MaSoThue ?? string.Empty,
                                            DChi = model.DiaChi ?? string.Empty,
                                            SDThoai = model.SoDienThoaiNguoiMuaHang ?? string.Empty,
                                            HVTNMHang = model.HoTenNguoiMuaHang ?? string.Empty,
                                            CCCDan = model.CanCuocCongDan ?? String.Empty,
                                            TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.TTin>()
                                        },
                                        DSHHDVu = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.HHDVu>(),
                                        TToan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.TToan
                                        {
                                            THTTLTSuat = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.LTSuat>(),
                                            TgTCThue = model.TongTienHang ?? 0,
                                            TgTThue = model.TongTienThueGTGT ?? 0,
                                            TTCKTMai = model.TongTienChietKhau ?? 0,
                                            TgTTTBSo = model.TongTienThanhToan ?? 0,
                                            TgTTTBChu = model.SoTienBangChu,
                                            TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.TTin>()
                                        }
                                    },
                                    TTKhac = ttKhac_ThongTinSaiSot_PostGTGT
                                },
                                DSCKS = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.DSCKS
                                {
                                    NBan = string.Empty
                                },
                                MCCQT = model.MaCuaCQT
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
                                    hDonGTGTPos.DLHDon.TTChung.TTHDLQuan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.TTHDLQuan
                                    {
                                        LHDCLQuan = model.TrangThai == (int)TrangThaiHoaDon.HoaDonThayThe ? (LADHDDT)hdlq.LoaiApDungHoaDonCanThayThe : (LADHDDT)hdlq.LoaiApDungHoaDonDieuChinh
                                    };

                                    if (model.TrangThai == (int)TrangThaiHoaDon.HoaDonThayThe)
                                    {
                                        hDonGTGTPos.DLHDon.TTChung.TTHDLQuan.TCHDon = TCHDon.ThayThe;
                                        hDonGTGTPos.DLHDon.TTChung.TTHDLQuan.KHMSHDCLQuan = model.LyDoThayTheModel.MauSo;
                                        hDonGTGTPos.DLHDon.TTChung.TTHDLQuan.KHHDCLQuan = model.LyDoThayTheModel.KyHieu;
                                        hDonGTGTPos.DLHDon.TTChung.TTHDLQuan.SHDCLQuan = model.LyDoThayTheModel.SoHoaDon;
                                        hDonGTGTPos.DLHDon.TTChung.TTHDLQuan.NLHDCLQuan = model.LyDoThayTheModel.NgayHoaDon.ToString("yyyy-MM-dd");
                                    }
                                    else
                                    {
                                        hDonGTGTPos.DLHDon.TTChung.TTHDLQuan.TCHDon = TCHDon.DieuChinh;
                                        hDonGTGTPos.DLHDon.TTChung.TTHDLQuan.KHMSHDCLQuan = model.LyDoDieuChinhModel.MauSo;
                                        hDonGTGTPos.DLHDon.TTChung.TTHDLQuan.KHHDCLQuan = model.LyDoDieuChinhModel.KyHieu;
                                        hDonGTGTPos.DLHDon.TTChung.TTHDLQuan.SHDCLQuan = model.LyDoDieuChinhModel.SoHoaDon;
                                        hDonGTGTPos.DLHDon.TTChung.TTHDLQuan.NLHDCLQuan = model.LyDoDieuChinhModel.NgayHoaDon.ToString("yyyy-MM-dd");
                                    }
                                }
                            }
                            #endregion

                            #region Hàng hóa chi tiết
                            foreach (var item in model.HoaDonChiTiets)
                            {
                                var hhdvu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.HHDVu
                                {
                                    TChat = (TChat)item.TinhChat,
                                    STT = item.STT,
                                    MHHDVu = item.MaHang ?? string.Empty,
                                    THHDVu = item.TenHang ?? string.Empty,
                                    DVTinh = item.TenDonViTinh ?? string.Empty,
                                    SLuong = item.SoLuong,
                                    DGia = item.DonGia,
                                    TLCKhau = item.TyLeChietKhau,
                                    STCKhau = item.TienChietKhau,
                                    ThTien = item.ThanhTien,
                                    TSuat = item.ThueGTGT.GetThueHasPer(),
                                    TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.TTin>()
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

                                hDonGTGTPos.DLHDon.NDHDon.DSHHDVu.Add(hhdvu);
                            }

                            #endregion

                            #region tổng hợp mỗi loại thuế suất
                            var groupThueGTGTPos = model.HoaDonChiTiets.GroupBy(x => x.ThueGTGT.GetThueHasPer())
                                .Select(x => new HoaDonDienTuChiTietViewModel
                                {
                                    ThueGTGT = x.Key,
                                    ThanhTien = x.Sum(y => y.TinhChat == 3 ? -y.ThanhTien : y.ThanhTien),
                                    TienThueGTGT = x.Sum(y => y.TinhChat == 3 ? -y.TienThueGTGT : y.TienThueGTGT)
                                })
                                .ToList();

                            foreach (var item in groupThueGTGTPos)
                            {
                                hDonGTGTPos.DLHDon.NDHDon.TToan.THTTLTSuat.Add(new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.LTSuat
                                {
                                    TSuat = item.ThueGTGT.GetThueHasPer(),
                                    ThTien = item.ThanhTien ?? 0,
                                    TThue = item.TienThueGTGT,
                                });
                            }
                            #endregion

                            if ((model.TrangThai == (int)TrangThaiHoaDon.HoaDonDieuChinh) && (model.LoaiDieuChinh == (int)LoaiDieuChinhHoaDon.DieuChinhThongTin) && (model.IsThongTinNguoiBanHoacNguoiMua == true))
                            {
                                hDonGTGTPos.DLHDon.NDHDon.DSHHDVu = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.HHDVu>();
                                hDonGTGTPos.DLHDon.NDHDon.TToan = null;
                            }

                            thongDiepGTGT.TTChung.MST = thongTinNguoiBan != null ? thongTinNguoiBan.MaSoThueNguoiBan : hoSoHDDT.MaSoThue;
                            thongDiepGTGT.DLieu.Add(hDonGTGTPos);

                            break;
                        case (int)LoaiHoaDon.HoaDonBanHangCMTMTT:
                            if (string.IsNullOrEmpty(model.TenKhachHang))
                            {
                                model.TenKhachHang = model.HoTenNguoiMuaHang;
                            }
                            var ttKhac_ThongTinSaiSot_PostBH = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.i.TTin>();
                            if (!string.IsNullOrWhiteSpace(model.IdHoaDonSaiSotBiThayThe))
                            {
                                ttKhac_ThongTinSaiSot_PostBH.Add(new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.i.TTin { TTruong = "Hóa đơn liên quan", KDLieu = "string", DLieu = model.GhiChuThayTheSaiSot?.Replace("<b>", "").Replace("</b>", "") });
                            }

                            HoaDonBHPos hDonBanHangPos = new HoaDonBHPos
                            {
                                DLHDon = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.i.DLHDon
                                {
                                    TTChung = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.i.TTChung
                                    {
                                        PBan = pBien1,
                                        THDon = LoaiHoaDon.HoaDonBanHangCMTMTT.GetDescription(),
                                        KHMSHDon = model.MauSo,
                                        KHHDon = model.KyHieu,
                                        SHDon = model.SoHoaDon,
                                        NLap = model.NgayHoaDon.Value.ToString("yyyy-MM-dd"),
                                        TTHDLQuan = null,
                                        TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.i.TTin>()
                                    },
                                    NDHDon = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.i.NDHDon
                                    {
                                        NBan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.i.NBan
                                        {
                                            Ten = thongTinNguoiBan != null ? (thongTinNguoiBan.TenDonViNguoiBan ?? String.Empty) : (hoSoHDDT.TenDonVi ?? String.Empty),
                                            MST = thongTinNguoiBan != null ? thongTinNguoiBan.MaSoThueNguoiBan.Trim().Replace(" ", string.Empty) ?? hoSoHDDT.MaSoThue ?? string.Empty : hoSoHDDT.MaSoThue ?? string.Empty,
                                            DChi = thongTinNguoiBan != null ? (thongTinNguoiBan.DiaChiNguoiBan ?? String.Empty) : (hoSoHDDT.DiaChi ?? string.Empty),
                                            SDThoai = sdtNBan,
                                            TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.i.TTin>()
                                        },
                                        NMua = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.i.NMua
                                        {
                                            Ten = model.TenKhachHang ?? string.Empty,
                                            MST = model.MaSoThue ?? string.Empty,
                                            DChi = model.DiaChi ?? string.Empty,
                                            SDThoai = model.SoDienThoaiNguoiMuaHang ?? string.Empty,
                                            HVTNMHang = model.HoTenNguoiMuaHang ?? string.Empty,
                                            CCCDan = model.CanCuocCongDan ?? String.Empty,
                                            TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.i.TTin>()
                                        },
                                        DSHHDVu = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.i.HHDVu>(),
                                        TToan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.i.TToan
                                        {
                                            TTCKTMai = model.TongTienChietKhau,
                                            TgTTTBSo = model.TongTienThanhToan ?? 0,
                                            TgTTTBChu = model.SoTienBangChu,
                                            TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.i.TTin>()
                                        }
                                    },
                                    TTKhac = ttKhac_ThongTinSaiSot_PostBH
                                },
                                DSCKS = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.i.DSCKS
                                {
                                    NBan = string.Empty
                                },
                                MCCQT = model.MaCuaCQT
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
                                    hDonBanHangPos.DLHDon.TTChung.TTHDLQuan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.i.TTHDLQuan
                                    {
                                        LHDCLQuan = model.TrangThai == (int)TrangThaiHoaDon.HoaDonThayThe ? (LADHDDT)hdlq.LoaiApDungHoaDonCanThayThe : (LADHDDT)hdlq.LoaiApDungHoaDonDieuChinh
                                    };

                                    if (model.TrangThai == (int)TrangThaiHoaDon.HoaDonThayThe)
                                    {
                                        hDonBanHangPos.DLHDon.TTChung.TTHDLQuan.TCHDon = TCHDon.ThayThe;
                                        hDonBanHangPos.DLHDon.TTChung.TTHDLQuan.KHMSHDCLQuan = hdlq.MauSo;
                                        hDonBanHangPos.DLHDon.TTChung.TTHDLQuan.KHHDCLQuan = hdlq.KyHieu;
                                        hDonBanHangPos.DLHDon.TTChung.TTHDLQuan.SHDCLQuan = hdlq.StrSoHoaDon;
                                        hDonBanHangPos.DLHDon.TTChung.TTHDLQuan.NLHDCLQuan = hdlq.NgayHoaDon.Value.ToString("yyyy-MM-dd");
                                    }
                                    else
                                    {
                                        hDonBanHangPos.DLHDon.TTChung.TTHDLQuan.TCHDon = TCHDon.DieuChinh;
                                        hDonBanHangPos.DLHDon.TTChung.TTHDLQuan.KHMSHDCLQuan = hdlq.MauSo;
                                        hDonBanHangPos.DLHDon.TTChung.TTHDLQuan.KHHDCLQuan = hdlq.KyHieu;
                                        hDonBanHangPos.DLHDon.TTChung.TTHDLQuan.SHDCLQuan = hdlq.StrSoHoaDon;
                                        hDonBanHangPos.DLHDon.TTChung.TTHDLQuan.NLHDCLQuan = hdlq.NgayHoaDon.Value.ToString("yyyy-MM-dd");
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

                                var hhdvu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.i.HHDVu
                                {
                                    TChat = (TChat)item.TinhChat,
                                    STT = stt,
                                    MHHDVu = item.MaHang ?? string.Empty,
                                    THHDVu = item.TenHang ?? string.Empty,
                                    DVTinh = item.TenDonViTinh ?? string.Empty,
                                    SLuong = item.SoLuong,
                                    DGia = item.DonGia,
                                    TLCKhau = item.TyLeChietKhau,
                                    STCKhau = item.TienChietKhau,
                                    ThTien = item.ThanhTien,
                                    TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.i.TTin>()
                                };

                                hDonBanHangPos.DLHDon.NDHDon.DSHHDVu.Add(hhdvu);
                            }
                            #endregion

                            thongDiepBanHang.TTChung.MST = thongTinNguoiBan != null ? thongTinNguoiBan.MaSoThueNguoiBan : hoSoHDDT.MaSoThue;
                            thongDiepBanHang.DLieu.Add(hDonBanHangPos);
                            break;
                        case (int)LoaiHoaDon.HoaDonKhacCMTMTT:
                        case (int)LoaiHoaDon.TemVeGTGT:
                        case (int)LoaiHoaDon.TemVeBanHang:
                            var ttKhac_ThongTinSaiSot_PostKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.TTin>();
                            if (!string.IsNullOrWhiteSpace(model.IdHoaDonSaiSotBiThayThe))
                            {
                                ttKhac_ThongTinSaiSot_PostKhac.Add(new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.TTin { TTruong = "Hóa đơn liên quan", KDLieu = "string", DLieu = model.GhiChuThayTheSaiSot?.Replace("<b>", "").Replace("</b>", "") });
                            }

                            HoaDonKhacPos hDonKhacPos = new HoaDonKhacPos
                            {
                                DLHDon = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.DLHDon
                                {
                                    TTChung = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.TTChung
                                    {
                                        PBan = pBien1,
                                        THDon = LoaiHoaDon.HoaDonKhacCMTMTT.GetDescription(),
                                        KHMSHDon = model.MauSo,
                                        KHHDon = model.KyHieu,
                                        SHDon = model.SoHoaDon,
                                        NLap = model.NgayHoaDon.Value.ToString("yyyy-MM-dd"),
                                        TTHDLQuan = null,
                                        TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.TTin>()
                                    },
                                    NDHDon = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.NDHDon
                                    {
                                        NBan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.NBan
                                        {
                                            Ten = thongTinNguoiBan != null ? (thongTinNguoiBan.TenDonViNguoiBan ?? String.Empty) : (hoSoHDDT.TenDonVi ?? String.Empty),
                                            MST = thongTinNguoiBan != null ? thongTinNguoiBan.MaSoThueNguoiBan.Trim().Replace(" ", string.Empty) ?? hoSoHDDT.MaSoThue ?? string.Empty : hoSoHDDT.MaSoThue ?? string.Empty,
                                            DChi = thongTinNguoiBan != null ? (thongTinNguoiBan.DiaChiNguoiBan ?? String.Empty) : (hoSoHDDT.DiaChi ?? string.Empty),
                                            SDThoai = sdtNBan,
                                            TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.TTin>(),
                                        },
                                        NMua = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.NMua
                                        {
                                            Ten = model.TenKhachHang ?? string.Empty,
                                            MST = model.MaSoThue ?? string.Empty,
                                            DChi = model.DiaChi ?? string.Empty,
                                            SDThoai = model.SoDienThoaiNguoiMuaHang ?? string.Empty,
                                            CCCDan = model.CanCuocCongDan ?? String.Empty,
                                            TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.TTin>()
                                        },
                                        DSHHDVu = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.HHDVu>(),
                                        TToan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.TToan
                                        {
                                            THTTLTSuat = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.LTSuat>(),
                                            TgTCThue = model.TongTienHang ?? 0,
                                            TgTThue = model.TongTienThueGTGT ?? 0,
                                            TTCKTMai = model.TongTienChietKhau ?? 0,
                                            TgTTTBSo = model.TongTienThanhToan ?? 0,
                                            TgTTTBChu = model.SoTienBangChu,
                                            TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.TTin>()
                                        }
                                    },
                                    TTKhac = ttKhac_ThongTinSaiSot_PostKhac
                                },
                                DSCKS = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.DSCKS
                                {
                                    NBan = string.Empty
                                },
                                MCCQT = model.MaCuaCQT
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
                                    hDonKhacPos.DLHDon.TTChung.TTHDLQuan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.TTHDLQuan
                                    {
                                        LHDCLQuan = model.TrangThai == (int)TrangThaiHoaDon.HoaDonThayThe ? (LADHDDT)hdlq.LoaiApDungHoaDonCanThayThe : (LADHDDT)hdlq.LoaiApDungHoaDonDieuChinh
                                    };

                                    if (model.TrangThai == (int)TrangThaiHoaDon.HoaDonThayThe)
                                    {
                                        hDonKhacPos.DLHDon.TTChung.TTHDLQuan.TCHDon = TCHDon.ThayThe;
                                        hDonKhacPos.DLHDon.TTChung.TTHDLQuan.KHMSHDCLQuan = model.LyDoThayTheModel.MauSo;
                                        hDonKhacPos.DLHDon.TTChung.TTHDLQuan.KHHDCLQuan = model.LyDoThayTheModel.KyHieu;
                                        hDonKhacPos.DLHDon.TTChung.TTHDLQuan.SHDCLQuan = model.LyDoThayTheModel.SoHoaDon;
                                        hDonKhacPos.DLHDon.TTChung.TTHDLQuan.NLHDCLQuan = model.LyDoThayTheModel.NgayHoaDon.ToString("yyyy-MM-dd");
                                    }
                                    else
                                    {
                                        hDonKhacPos.DLHDon.TTChung.TTHDLQuan.TCHDon = TCHDon.DieuChinh;
                                        hDonKhacPos.DLHDon.TTChung.TTHDLQuan.KHMSHDCLQuan = model.LyDoDieuChinhModel.MauSo;
                                        hDonKhacPos.DLHDon.TTChung.TTHDLQuan.KHHDCLQuan = model.LyDoDieuChinhModel.KyHieu;
                                        hDonKhacPos.DLHDon.TTChung.TTHDLQuan.SHDCLQuan = model.LyDoDieuChinhModel.SoHoaDon;
                                        hDonKhacPos.DLHDon.TTChung.TTHDLQuan.NLHDCLQuan = model.LyDoDieuChinhModel.NgayHoaDon.ToString("yyyy-MM-dd");
                                    }
                                }
                            }
                            #endregion

                            #region Hàng hóa chi tiết
                            foreach (var item in model.HoaDonChiTiets)
                            {
                                var hhdvu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.HHDVu
                                {
                                    TChat = (TChat)item.TinhChat,
                                    STT = item.STT,
                                    MHHDVu = item.MaHang ?? string.Empty,
                                    THHDVu = item.TenHang ?? string.Empty,
                                    DVTinh = item.TenDonViTinh ?? string.Empty,
                                    SLuong = item.SoLuong,
                                    DGia = item.DonGia,
                                    TLCKhau = item.TyLeChietKhau,
                                    STCKhau = item.TienChietKhau,
                                    ThTien = item.ThanhTien,
                                    TSuat = item.ThueGTGT.GetThueHasPer(),
                                    TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.TTin>()
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

                                hDonKhacPos.DLHDon.NDHDon.DSHHDVu.Add(hhdvu);
                            }
                            #endregion

                            #region tổng hợp mỗi loại thuế suất
                            var groupThueKhacPos = model.HoaDonChiTiets.GroupBy(x => x.ThueGTGT.GetThueHasPer())
                                .Select(x => new HoaDonDienTuChiTietViewModel
                                {
                                    ThueGTGT = x.Key,
                                    ThanhTien = x.Sum(y => y.TinhChat == 3 ? -y.ThanhTien : y.ThanhTien),
                                    TienThueGTGT = x.Sum(y => y.TinhChat == 3 ? -y.TienThueGTGT : y.TienThueGTGT)
                                })
                                .ToList();

                            foreach (var item in groupThueKhacPos)
                            {
                                hDonKhacPos.DLHDon.NDHDon.TToan.THTTLTSuat.Add(new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.LTSuat
                                {
                                    TSuat = item.ThueGTGT.GetThueHasPer(),
                                    ThTien = item.ThanhTien ?? 0,
                                    TThue = item.TienThueGTGT,
                                });
                            }
                            #endregion

                            if ((model.TrangThai == (int)TrangThaiHoaDon.HoaDonDieuChinh) && (model.LoaiDieuChinh == (int)LoaiDieuChinhHoaDon.DieuChinhThongTin) && (model.IsThongTinNguoiBanHoacNguoiMua == true))
                            {
                                hDonKhacPos.DLHDon.NDHDon.DSHHDVu = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.HHDVu>();
                                hDonKhacPos.DLHDon.NDHDon.TToan = null;
                            }

                            thongDiepKhac.TTChung.MST = thongTinNguoiBan != null ? thongTinNguoiBan.MaSoThueNguoiBan : hoSoHDDT.MaSoThue;
                            thongDiepKhac.DLieu.Add(hDonKhacPos);

                            break;

                    }

                    if (thongDiepGTGT.DLieu.Any())
                    {
                        GenerateXML(thongDiepGTGT, xmlFilePathGTGT);
                    }

                    if (thongDiepBanHang.DLieu.Any())
                    {
                        GenerateXML(thongDiepBanHang, xmlFilePathBanHang);
                    }

                    if (thongDiepKhac.DLieu.Any())
                    {
                        GenerateXML(thongDiepKhac, xmlFilePathKhac);
                    }
                }
            }
            catch (Exception ex)
            {
                Tracert.WriteLog(ex.Message);
            }
        }
        public async Task<bool> CreateXmlCMTMTT(string xmlFilePath, HoaDonDienTuViewModel model)
        {
            try
            {
                var pBien1 = "2.0.1";
                string taxCode = _configuration["Config:TaxCode"];
                int stt = 0;
                var fileData = new FileData();
                //var model = await GetByIdAsync(model.HoaDonDienTuId);
                // Gọi API đọc thông tin người bán trên mẫu hóa đơn của hóa đơn
                var listThongTinNguoiBan = await _digitalSignerNameReaderService.GetThongTinNguoiBanTuHoaDonAsync(new string[] { model.HoaDonDienTuId });
                var thongTinNguoiBan = listThongTinNguoiBan?.FirstOrDefault();

                var hoSoHDDT = await _hoSoHDDTService.GetDetailAsync();

                if (model.BoKyHieuHoaDon.HinhThucHoaDon != HinhThucHoaDon.CoMaTuMayTinhTien) return false;
                var sdtNBan = thongTinNguoiBan != null ? thongTinNguoiBan.DienThoaiNguoiBan != null ? thongTinNguoiBan.DienThoaiNguoiBan.Trim() : hoSoHDDT.SoDienThoaiLienHe.Trim() : hoSoHDDT.SoDienThoaiLienHe.Trim();
                if (sdtNBan.Length > 20) sdtNBan = !string.IsNullOrEmpty(hoSoHDDT.SoDienThoaiLienHe) ? hoSoHDDT.SoDienThoaiLienHe.Trim() : "";
                var stkNBan = thongTinNguoiBan != null ? thongTinNguoiBan.SoTaiKhoanNguoiBan != null ? thongTinNguoiBan.SoTaiKhoanNguoiBan.Trim() : hoSoHDDT.SoTaiKhoanNganHang.Trim() : hoSoHDDT.SoTaiKhoanNganHang.Trim();
                if (stkNBan.Length > 30) stkNBan = !string.IsNullOrEmpty(hoSoHDDT.SoTaiKhoanNganHang) ? hoSoHDDT.SoTaiKhoanNganHang.Trim() : "";
                switch (model.LoaiHoaDon)
                {
                    case (int)LoaiHoaDon.HoaDonGTGTCMTMTT:
                        var ttKhac_ThongTinSaiSot_PostGTGT = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.TTin>();
                        if (!string.IsNullOrWhiteSpace(model.IdHoaDonSaiSotBiThayThe))
                        {
                            ttKhac_ThongTinSaiSot_PostGTGT.Add(new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.TTin { TTruong = "Hóa đơn liên quan", KDLieu = "string", DLieu = model.GhiChuThayTheSaiSot?.Replace("<b>", "").Replace("</b>", "") });
                        }

                        HoaDonGTGTPos hDonGTGTPos = new HoaDonGTGTPos
                        {
                            DLHDon = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.DLHDon
                            {
                                TTChung = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.TTChung
                                {
                                    PBan = pBien1,
                                    THDon = LoaiHoaDon.HoaDonGTGTCMTMTT.GetDescription(),
                                    KHMSHDon = model.MauSo,
                                    KHHDon = model.KyHieu,
                                    SHDon = model.SoHoaDon,
                                    NLap = model.NgayHoaDon.Value.ToString("yyyy-MM-dd"),
                                    TTHDLQuan = null,
                                    TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.TTin>()
                                },
                                NDHDon = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.NDHDon
                                {
                                    NBan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.NBan
                                    {
                                        Ten = thongTinNguoiBan != null ? (thongTinNguoiBan.TenDonViNguoiBan ?? String.Empty) : (hoSoHDDT.TenDonVi ?? String.Empty),
                                        MST = thongTinNguoiBan != null ? thongTinNguoiBan.MaSoThueNguoiBan.Trim().Replace(" ", string.Empty) ?? hoSoHDDT.MaSoThue ?? string.Empty : hoSoHDDT.MaSoThue ?? string.Empty,
                                        DChi = thongTinNguoiBan != null ? (thongTinNguoiBan.DiaChiNguoiBan ?? String.Empty) : (hoSoHDDT.DiaChi ?? string.Empty),
                                        SDThoai = sdtNBan,
                                        TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.TTin>(),
                                    },
                                    NMua = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.NMua
                                    {
                                        Ten = model.TenKhachHang ?? string.Empty,
                                        MST = model.MaSoThue ?? string.Empty,
                                        DChi = model.DiaChi ?? string.Empty,
                                        SDThoai = model.SoDienThoaiNguoiMuaHang ?? string.Empty,
                                        CCCDan = model.CanCuocCongDan ?? String.Empty,
                                        TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.TTin>()
                                    },
                                    DSHHDVu = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.HHDVu>(),
                                    TToan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.TToan
                                    {
                                        THTTLTSuat = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.LTSuat>(),
                                        TgTCThue = model.TongTienHang ?? 0,
                                        TgTThue = model.TongTienThueGTGT ?? 0,
                                        TTCKTMai = model.TongTienChietKhau ?? 0,
                                        TgTTTBSo = model.TongTienThanhToan ?? 0,
                                        TgTTTBChu = model.SoTienBangChu,
                                        TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.TTin>()
                                    }
                                },
                                TTKhac = ttKhac_ThongTinSaiSot_PostGTGT
                            },
                            DSCKS = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.DSCKS
                            {
                                NBan = string.Empty
                            },
                            MCCQT = model.MaCuaCQT
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
                                hDonGTGTPos.DLHDon.TTChung.TTHDLQuan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.TTHDLQuan
                                {
                                    LHDCLQuan = model.TrangThai == (int)TrangThaiHoaDon.HoaDonThayThe ? (LADHDDT)hdlq.LoaiApDungHoaDonCanThayThe : (LADHDDT)hdlq.LoaiApDungHoaDonDieuChinh
                                };

                                if (model.TrangThai == (int)TrangThaiHoaDon.HoaDonThayThe)
                                {
                                    hDonGTGTPos.DLHDon.TTChung.TTHDLQuan.TCHDon = TCHDon.ThayThe;
                                    hDonGTGTPos.DLHDon.TTChung.TTHDLQuan.KHMSHDCLQuan = model.LyDoThayTheModel.MauSo;
                                    hDonGTGTPos.DLHDon.TTChung.TTHDLQuan.KHHDCLQuan = model.LyDoThayTheModel.KyHieu;
                                    hDonGTGTPos.DLHDon.TTChung.TTHDLQuan.SHDCLQuan = model.LyDoThayTheModel.SoHoaDon;
                                    hDonGTGTPos.DLHDon.TTChung.TTHDLQuan.NLHDCLQuan = model.LyDoThayTheModel.NgayHoaDon.ToString("yyyy-MM-dd");
                                }
                                else
                                {
                                    hDonGTGTPos.DLHDon.TTChung.TTHDLQuan.TCHDon = TCHDon.DieuChinh;
                                    hDonGTGTPos.DLHDon.TTChung.TTHDLQuan.KHMSHDCLQuan = model.LyDoDieuChinhModel.MauSo;
                                    hDonGTGTPos.DLHDon.TTChung.TTHDLQuan.KHHDCLQuan = model.LyDoDieuChinhModel.KyHieu;
                                    hDonGTGTPos.DLHDon.TTChung.TTHDLQuan.SHDCLQuan = model.LyDoDieuChinhModel.SoHoaDon;
                                    hDonGTGTPos.DLHDon.TTChung.TTHDLQuan.NLHDCLQuan = model.LyDoDieuChinhModel.NgayHoaDon.ToString("yyyy-MM-dd");
                                }
                            }
                        }
                        #endregion

                        #region Hàng hóa chi tiết
                        foreach (var item in model.HoaDonChiTiets)
                        {
                            var hhdvu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.HHDVu
                            {
                                TChat = (TChat)item.TinhChat,
                                STT = item.STT,
                                MHHDVu = item.MaHang ?? string.Empty,
                                THHDVu = item.TenHang ?? string.Empty,
                                DVTinh = item.TenDonViTinh ?? string.Empty,
                                SLuong = item.SoLuong,
                                DGia = item.DonGia,
                                TLCKhau = item.TyLeChietKhau,
                                STCKhau = item.TienChietKhau,
                                ThTien = item.ThanhTien,
                                TSuat = item.ThueGTGT.GetThueHasPer(),
                                TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.TTin>()
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

                            hDonGTGTPos.DLHDon.NDHDon.DSHHDVu.Add(hhdvu);
                        }
                        #endregion

                        #region tổng hợp mỗi loại thuế suất
                        var groupThueGTGTPos = model.HoaDonChiTiets.GroupBy(x => x.ThueGTGT.GetThueHasPer())
                            .Select(x => new HoaDonDienTuChiTietViewModel
                            {
                                ThueGTGT = x.Key,
                                ThanhTien = x.Sum(y => y.TinhChat == 3 ? -y.ThanhTien : y.ThanhTien),
                                TienThueGTGT = x.Sum(y => y.TinhChat == 3 ? -y.TienThueGTGT : y.TienThueGTGT)
                            })
                            .ToList();

                        foreach (var item in groupThueGTGTPos)
                        {
                            hDonGTGTPos.DLHDon.NDHDon.TToan.THTTLTSuat.Add(new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.LTSuat
                            {
                                TSuat = item.ThueGTGT.GetThueHasPer(),
                                ThTien = item.ThanhTien ?? 0,
                                TThue = item.TienThueGTGT,
                            });
                        }
                        #endregion

                        if ((model.TrangThai == (int)TrangThaiHoaDon.HoaDonDieuChinh) && (model.LoaiDieuChinh == (int)LoaiDieuChinhHoaDon.DieuChinhThongTin) && (model.IsThongTinNguoiBanHoacNguoiMua == true))
                        {
                            hDonGTGTPos.DLHDon.NDHDon.DSHHDVu = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.HHDVu>();
                            hDonGTGTPos.DLHDon.NDHDon.TToan = null;
                        }

                        GenerateXML(hDonGTGTPos, xmlFilePath);

                        fileData = new FileData
                        {
                            FileDataId = Guid.NewGuid().ToString(),
                            RefId = model.HoaDonDienTuId,
                            Content = File.ReadAllText(xmlFilePath),
                            Binary = File.ReadAllBytes(xmlFilePath),
                            FileName = Path.GetFileName(xmlFilePath),
                            IsSigned = false,
                            Type = 1,
                            DateTime = DateTime.Now
                        };

                        await _dataContext.FileDatas.AddAsync(fileData);
                        await _dataContext.SaveChangesAsync();

                        break;
                    case (int)LoaiHoaDon.HoaDonBanHangCMTMTT:
                        var ttKhac_ThongTinSaiSot_PostBH = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.i.TTin>();
                        if (!string.IsNullOrWhiteSpace(model.IdHoaDonSaiSotBiThayThe))
                        {
                            ttKhac_ThongTinSaiSot_PostBH.Add(new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.i.TTin { TTruong = "Hóa đơn liên quan", KDLieu = "string", DLieu = model.GhiChuThayTheSaiSot?.Replace("<b>", "").Replace("</b>", "") });
                        }

                        HoaDonBHPos hDonBanHangPos = new HoaDonBHPos
                        {
                            DLHDon = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.i.DLHDon
                            {
                                TTChung = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.i.TTChung
                                {
                                    PBan = pBien1,
                                    THDon = LoaiHoaDon.HoaDonBanHangCMTMTT.GetDescription(),
                                    KHMSHDon = model.MauSo,
                                    KHHDon = model.KyHieu,
                                    SHDon = model.SoHoaDon,
                                    NLap = model.NgayHoaDon.Value.ToString("yyyy-MM-dd"),
                                    TTHDLQuan = null,
                                    TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.i.TTin>()
                                },
                                NDHDon = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.i.NDHDon
                                {
                                    NBan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.i.NBan
                                    {
                                        Ten = thongTinNguoiBan != null ? (thongTinNguoiBan.TenDonViNguoiBan ?? String.Empty) : (hoSoHDDT.TenDonVi ?? String.Empty),
                                        MST = thongTinNguoiBan != null ? thongTinNguoiBan.MaSoThueNguoiBan.Trim().Replace(" ", string.Empty) ?? hoSoHDDT.MaSoThue ?? string.Empty : hoSoHDDT.MaSoThue ?? string.Empty,
                                        DChi = thongTinNguoiBan != null ? (thongTinNguoiBan.DiaChiNguoiBan ?? String.Empty) : (hoSoHDDT.DiaChi ?? string.Empty),
                                        SDThoai = sdtNBan,
                                        TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.i.TTin>()
                                    },
                                    NMua = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.i.NMua
                                    {
                                        Ten = model.TenKhachHang ?? string.Empty,
                                        MST = model.MaSoThue ?? string.Empty,
                                        DChi = model.DiaChi ?? string.Empty,
                                        SDThoai = model.SoDienThoaiNguoiMuaHang ?? string.Empty,
                                        CCCDan = model.CanCuocCongDan ?? String.Empty,
                                        TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.i.TTin>()
                                    },
                                    DSHHDVu = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.i.HHDVu>(),
                                    TToan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.i.TToan
                                    {
                                        TTCKTMai = model.TongTienChietKhau,
                                        TgTTTBSo = model.TongTienThanhToan ?? 0,
                                        TgTTTBChu = model.SoTienBangChu,
                                        TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.i.TTin>()
                                    }
                                },
                                TTKhac = ttKhac_ThongTinSaiSot_PostBH
                            },
                            DSCKS = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.i.DSCKS
                            {
                                NBan = string.Empty
                            },
                            MCCQT = model.MaCuaCQT
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
                                hDonBanHangPos.DLHDon.TTChung.TTHDLQuan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.i.TTHDLQuan
                                {
                                    LHDCLQuan = model.TrangThai == (int)TrangThaiHoaDon.HoaDonThayThe ? (LADHDDT)hdlq.LoaiApDungHoaDonCanThayThe : (LADHDDT)hdlq.LoaiApDungHoaDonDieuChinh
                                };

                                if (model.TrangThai == (int)TrangThaiHoaDon.HoaDonThayThe)
                                {
                                    hDonBanHangPos.DLHDon.TTChung.TTHDLQuan.TCHDon = TCHDon.ThayThe;
                                    hDonBanHangPos.DLHDon.TTChung.TTHDLQuan.KHMSHDCLQuan = hdlq.MauSo;
                                    hDonBanHangPos.DLHDon.TTChung.TTHDLQuan.KHHDCLQuan = hdlq.KyHieu;
                                    hDonBanHangPos.DLHDon.TTChung.TTHDLQuan.SHDCLQuan = hdlq.StrSoHoaDon;
                                    hDonBanHangPos.DLHDon.TTChung.TTHDLQuan.NLHDCLQuan = hdlq.NgayHoaDon.Value.ToString("yyyy-MM-dd");
                                }
                                else
                                {
                                    hDonBanHangPos.DLHDon.TTChung.TTHDLQuan.TCHDon = TCHDon.DieuChinh;
                                    hDonBanHangPos.DLHDon.TTChung.TTHDLQuan.KHMSHDCLQuan = hdlq.MauSo;
                                    hDonBanHangPos.DLHDon.TTChung.TTHDLQuan.KHHDCLQuan = hdlq.KyHieu;
                                    hDonBanHangPos.DLHDon.TTChung.TTHDLQuan.SHDCLQuan = hdlq.StrSoHoaDon;
                                    hDonBanHangPos.DLHDon.TTChung.TTHDLQuan.NLHDCLQuan = hdlq.NgayHoaDon.Value.ToString("yyyy-MM-dd");
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

                            var hhdvu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.i.HHDVu
                            {
                                TChat = (TChat)item.TinhChat,
                                STT = stt,
                                MHHDVu = item.MaHang ?? string.Empty,
                                THHDVu = item.TenHang ?? string.Empty,
                                DVTinh = item.TenDonViTinh ?? string.Empty,
                                SLuong = item.SoLuong,
                                DGia = item.DonGia,
                                TLCKhau = item.TyLeChietKhau,
                                STCKhau = item.TienChietKhau,
                                ThTien = item.ThanhTien,
                                TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.i.TTin>()
                            };

                            hDonBanHangPos.DLHDon.NDHDon.DSHHDVu.Add(hhdvu);
                        }
                        #endregion

                        GenerateXML(hDonBanHangPos, xmlFilePath);

                        fileData = new FileData
                        {
                            FileDataId = Guid.NewGuid().ToString(),
                            RefId = model.HoaDonDienTuId,
                            Content = File.ReadAllText(xmlFilePath),
                            Binary = File.ReadAllBytes(xmlFilePath),
                            FileName = Path.GetFileName(xmlFilePath),
                            IsSigned = false,
                            Type = 1,
                            DateTime = DateTime.Now
                        };

                        await _dataContext.FileDatas.AddAsync(fileData);
                        await _dataContext.SaveChangesAsync();

                        break;
                    case (int)LoaiHoaDon.HoaDonKhacCMTMTT:
                    case (int)LoaiHoaDon.TemVeGTGT:
                    case (int)LoaiHoaDon.TemVeBanHang:
                        var ttKhac_ThongTinSaiSot_PostKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.TTin>();
                        if (!string.IsNullOrWhiteSpace(model.IdHoaDonSaiSotBiThayThe))
                        {
                            ttKhac_ThongTinSaiSot_PostKhac.Add(new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.TTin { TTruong = "Hóa đơn liên quan", KDLieu = "string", DLieu = model.GhiChuThayTheSaiSot?.Replace("<b>", "").Replace("</b>", "") });
                        }

                        HoaDonKhacPos hDonKhacPos = new HoaDonKhacPos
                        {
                            DLHDon = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.DLHDon
                            {
                                TTChung = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.TTChung
                                {
                                    PBan = pBien1,
                                    THDon = LoaiHoaDon.HoaDonKhacCMTMTT.GetDescription(),
                                    KHMSHDon = model.MauSo,
                                    KHHDon = model.KyHieu,
                                    SHDon = model.SoHoaDon,
                                    NLap = model.NgayHoaDon.Value.ToString("yyyy-MM-dd"),
                                    TTHDLQuan = null,
                                    TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.TTin>()
                                },
                                NDHDon = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.NDHDon
                                {
                                    NBan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.NBan
                                    {
                                        Ten = thongTinNguoiBan != null ? (thongTinNguoiBan.TenDonViNguoiBan ?? String.Empty) : (hoSoHDDT.TenDonVi ?? String.Empty),
                                        MST = thongTinNguoiBan != null ? thongTinNguoiBan.MaSoThueNguoiBan.Trim().Replace(" ", string.Empty) ?? hoSoHDDT.MaSoThue ?? string.Empty : hoSoHDDT.MaSoThue ?? string.Empty,
                                        DChi = thongTinNguoiBan != null ? (thongTinNguoiBan.DiaChiNguoiBan ?? String.Empty) : (hoSoHDDT.DiaChi ?? string.Empty),
                                        SDThoai = sdtNBan,
                                        TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.TTin>(),
                                    },
                                    NMua = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.NMua
                                    {
                                        Ten = model.TenKhachHang ?? string.Empty,
                                        MST = model.MaSoThue ?? string.Empty,
                                        DChi = model.DiaChi ?? string.Empty,
                                        SDThoai = model.SoDienThoaiNguoiMuaHang ?? string.Empty,
                                        CCCDan = model.CanCuocCongDan ?? String.Empty,
                                        TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.TTin>()
                                    },
                                    DSHHDVu = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.HHDVu>(),
                                    TToan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.TToan
                                    {
                                        THTTLTSuat = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.LTSuat>(),
                                        TgTCThue = model.TongTienHang ?? 0,
                                        TgTThue = model.TongTienThueGTGT ?? 0,
                                        TTCKTMai = model.TongTienChietKhau ?? 0,
                                        TgTTTBSo = model.TongTienThanhToan ?? 0,
                                        TgTTTBChu = model.SoTienBangChu,
                                        TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.TTin>()
                                    }
                                },
                                TTKhac = ttKhac_ThongTinSaiSot_PostKhac
                            },
                            DSCKS = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.DSCKS
                            {
                                NBan = string.Empty
                            },
                            MCCQT = model.MaCuaCQT
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
                                hDonKhacPos.DLHDon.TTChung.TTHDLQuan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.TTHDLQuan
                                {
                                    LHDCLQuan = model.TrangThai == (int)TrangThaiHoaDon.HoaDonThayThe ? (LADHDDT)hdlq.LoaiApDungHoaDonCanThayThe : (LADHDDT)hdlq.LoaiApDungHoaDonDieuChinh
                                };

                                if (model.TrangThai == (int)TrangThaiHoaDon.HoaDonThayThe)
                                {
                                    hDonKhacPos.DLHDon.TTChung.TTHDLQuan.TCHDon = TCHDon.ThayThe;
                                    hDonKhacPos.DLHDon.TTChung.TTHDLQuan.KHMSHDCLQuan = model.LyDoThayTheModel.MauSo;
                                    hDonKhacPos.DLHDon.TTChung.TTHDLQuan.KHHDCLQuan = model.LyDoThayTheModel.KyHieu;
                                    hDonKhacPos.DLHDon.TTChung.TTHDLQuan.SHDCLQuan = model.LyDoThayTheModel.SoHoaDon;
                                    hDonKhacPos.DLHDon.TTChung.TTHDLQuan.NLHDCLQuan = model.LyDoThayTheModel.NgayHoaDon.ToString("yyyy-MM-dd");
                                }
                                else
                                {
                                    hDonKhacPos.DLHDon.TTChung.TTHDLQuan.TCHDon = TCHDon.DieuChinh;
                                    hDonKhacPos.DLHDon.TTChung.TTHDLQuan.KHMSHDCLQuan = model.LyDoDieuChinhModel.MauSo;
                                    hDonKhacPos.DLHDon.TTChung.TTHDLQuan.KHHDCLQuan = model.LyDoDieuChinhModel.KyHieu;
                                    hDonKhacPos.DLHDon.TTChung.TTHDLQuan.SHDCLQuan = model.LyDoDieuChinhModel.SoHoaDon;
                                    hDonKhacPos.DLHDon.TTChung.TTHDLQuan.NLHDCLQuan = model.LyDoDieuChinhModel.NgayHoaDon.ToString("yyyy-MM-dd");
                                }
                            }
                        }
                        #endregion

                        #region Hàng hóa chi tiết
                        foreach (var item in model.HoaDonChiTiets)
                        {
                            var hhdvu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.HHDVu
                            {
                                TChat = (TChat)item.TinhChat,
                                STT = item.STT,
                                MHHDVu = item.MaHang ?? string.Empty,
                                THHDVu = item.TenHang ?? string.Empty,
                                DVTinh = item.TenDonViTinh ?? string.Empty,
                                SLuong = item.SoLuong,
                                DGia = item.DonGia,
                                TLCKhau = item.TyLeChietKhau,
                                STCKhau = item.TienChietKhau,
                                ThTien = item.ThanhTien,
                                TSuat = item.ThueGTGT.GetThueHasPer(),
                                TTKhac = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.TTin>()
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

                            hDonKhacPos.DLHDon.NDHDon.DSHHDVu.Add(hhdvu);
                        }
                        #endregion

                        #region tổng hợp mỗi loại thuế suất
                        var groupThueKhacPos = model.HoaDonChiTiets.GroupBy(x => x.ThueGTGT.GetThueHasPer())
                            .Select(x => new HoaDonDienTuChiTietViewModel
                            {
                                ThueGTGT = x.Key,
                                ThanhTien = x.Sum(y => y.TinhChat == 3 ? -y.ThanhTien : y.ThanhTien),
                                TienThueGTGT = x.Sum(y => y.TinhChat == 3 ? -y.TienThueGTGT : y.TienThueGTGT)
                            })
                            .ToList();

                        foreach (var item in groupThueKhacPos)
                        {
                            hDonKhacPos.DLHDon.NDHDon.TToan.THTTLTSuat.Add(new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.LTSuat
                            {
                                TSuat = item.ThueGTGT.GetThueHasPer(),
                                ThTien = item.ThanhTien ?? 0,
                                TThue = item.TienThueGTGT,
                            });
                        }
                        #endregion

                        if ((model.TrangThai == (int)TrangThaiHoaDon.HoaDonDieuChinh) && (model.LoaiDieuChinh == (int)LoaiDieuChinhHoaDon.DieuChinhThongTin) && (model.IsThongTinNguoiBanHoacNguoiMua == true))
                        {
                            hDonKhacPos.DLHDon.NDHDon.DSHHDVu = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.HHDVu>();
                            hDonKhacPos.DLHDon.NDHDon.TToan = null;
                        }

                        GenerateXML(hDonKhacPos, xmlFilePath);

                        fileData = new FileData
                        {
                            FileDataId = Guid.NewGuid().ToString(),
                            RefId = model.HoaDonDienTuId,
                            Content = File.ReadAllText(xmlFilePath),
                            Binary = File.ReadAllBytes(xmlFilePath),
                            FileName = Path.GetFileName(xmlFilePath),
                            IsSigned = false,
                            Type = 1,
                            DateTime = DateTime.Now
                        };

                        await _dataContext.FileDatas.AddAsync(fileData);
                        await _dataContext.SaveChangesAsync();

                        break;
                }
            }
            catch (Exception ex)
            {
                Tracert.WriteLog(ex.Message);
            }
            return File.Exists(xmlFilePath);
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
                            //join httt in _dataContext.HinhThucThanhToans on hd.HinhThucThanhToanId equals httt.HinhThucThanhToanId into tmpHinhThucThanhToans
                            //from httt in tmpHinhThucThanhToans.DefaultIfEmpty()
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
                            StrSoHoaDon = hd.SoHoaDon + "",
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
                            // pxk
                            CanCuSo = hd.CanCuSo,
                            NgayCanCu = hd.NgayCanCu,
                            Cua = hd.Cua,
                            DienGiai = hd.DienGiai,
                            DiaChiKhoNhanHang = hd.DiaChiKhoNhanHang,
                            HoTenNguoiNhanHang = hd.HoTenNguoiNhanHang,
                            DiaChiKhoXuatHang = hd.DiaChiKhoXuatHang,
                            HoTenNguoiXuatHang = hd.HoTenNguoiXuatHang,
                            HopDongVanChuyenSo = hd.HopDongVanChuyenSo,
                            TenNguoiVanChuyen = hd.TenNguoiVanChuyen,
                            PhuongThucVanChuyen = hd.PhuongThucVanChuyen,
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
                                                   SoLuongNhap = hdct.SoLuongNhap,
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
                                                StrSoHoaDon = x.SoHoaDon,
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

        //private void GenerateBillXML2(BBHuy data, string path)
        //{
        //    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
        //    ns.Add("", "");

        //    XmlSerializer serialiser = new XmlSerializer(typeof(BBHuy));

        //    using (TextWriter filestream = new StreamWriter(path))
        //    {
        //        serialiser.Serialize(filestream, data, ns);
        //    }
        //}

        //private async Task CreateInvoiceND51TT32Async(string xmlFilePath, HoaDonDienTuViewModel model)
        //{
        //    string linkSearch = _configuration["Config:LinkSearchInvoice"];
        //    var taxCode = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.TAX_CODE)?.Value;

        //    LoaiTien loaiTien = await _dataContext.LoaiTiens.AsNoTracking().FirstOrDefaultAsync(x => x.LoaiTienId == model.LoaiTienId);
        //    var hoSoHDDT = await _hoSoHDDTService.GetDetailAsync();

        //    //TTChung
        //    TTChung _ttChung = new TTChung
        //    {
        //        PBan = "1.1.0",
        //        THDon = ((LoaiHoaDon)model.LoaiHoaDon).GetDescription(),
        //        KHMSHDon = model.MauSo,
        //        KHHDon = model.KyHieu,
        //        SHDon = model.SoHoaDon,
        //        NLap = model.NgayHoaDon.Value.ToString("yyyy-MM-dd"),
        //        DVTTe = loaiTien != null ? loaiTien.Ma : string.Empty,
        //        TGia = model.TyGia.Value,
        //        TTNCC = "",
        //        DDTCuu = linkSearch,
        //        MTCuu = model.MaTraCuu,
        //        HTTToan = 1,
        //        THTTTKhac = string.Empty
        //    };
        //    #region NDHDon
        //    //NBan

        //    NBan _nBan = new NBan
        //    {
        //        Ten = hoSoHDDT.TenDonVi,
        //        MST = hoSoHDDT.MaSoThue,
        //        DChi = hoSoHDDT.DiaChi,
        //        SDThoai = hoSoHDDT.SoDienThoaiLienHe,
        //        DCTDTu = hoSoHDDT.EmailLienHe,
        //        STKNHang = hoSoHDDT.SoTaiKhoanNganHang,
        //        TNHang = hoSoHDDT.TenNganHang,
        //        Fax = hoSoHDDT.Fax,
        //        Website = hoSoHDDT.Website,
        //    };
        //    //NMua
        //    NMua _nMua = new NMua
        //    {
        //        Ten = model.TenKhachHang,
        //        MST = model.MaSoThue,
        //        DChi = model.DiaChi,
        //        SDThoai = model.SoDienThoaiNguoiMuaHang,
        //        DCTDTu = model.EmailNguoiMuaHang,
        //        HVTNMHang = model.HoTenNguoiMuaHang,
        //        STKNHang = model.SoTaiKhoanNganHang,
        //    };

        //    //HHDVus
        //    List<HHDVu> _dSHHDVu = new List<HHDVu>();
        //    int _STT = 1;
        //    foreach (var item in model.HoaDonChiTiets)
        //    {
        //        HHDVu _hhdv = new HHDVu
        //        {
        //            STT = _STT,
        //            TChat = 1,
        //            THHDVu = item.TenHang,
        //            DVTinh = item.DonViTinh != null ? item.DonViTinh.Ten : string.Empty,
        //            SLuong = item.SoLuong.Value,
        //            DGia = item.DonGia.Value,
        //            ThTien = item.ThanhTien.Value,
        //            TSuat = item.ThueGTGT,
        //        };
        //        _STT += 1;
        //        _dSHHDVu.Add(_hhdv);
        //    }

        //    //TToan
        //    List<LTSuat> listTLSuat = new List<LTSuat>();
        //    LTSuat tLSuat = new LTSuat
        //    {
        //        TSuat = model.HoaDonChiTiets.Count == 0 ? "" : model.HoaDonChiTiets.FirstOrDefault().ThueGTGT,
        //        ThTien = model.TongTienHangQuyDoi.Value,
        //        TThue = model.TongTienThueGTGTQuyDoi.Value
        //    };
        //    listTLSuat.Add(tLSuat);

        //    TToan _TToan = new TToan
        //    {
        //        TgTCThue = model.TongTienHangQuyDoi.Value,
        //        TgTThue = model.TongTienThueGTGTQuyDoi.Value,
        //        TgTTTBSo = model.TongTienThanhToanQuyDoi.Value,
        //        TgTTTBChu = model.SoTienBangChu,
        //        THTTLTSuat = listTLSuat
        //    };

        //    NDHDon _nDHDon = new NDHDon
        //    {
        //        NBan = _nBan,
        //        NMua = _nMua,
        //        DSHHDVu = _dSHHDVu,
        //        TToan = _TToan,
        //    };

        //    ///

        //    HDon _hDon = new HDon
        //    {
        //        DLHDon = new DLHDon
        //        {
        //            TTChung = _ttChung,
        //            NDHDon = _nDHDon,
        //        },
        //        DSCKS = new DSCKS
        //        {
        //            NBan = "",
        //            NMua = "",
        //        }
        //    };
        //    #endregion

        //    GenerateBillXML2(_hDon, xmlFilePath);
        //}

        //public async Task<bool> CreateXMLBienBan(string xmlFilePath, BienBanXoaBoViewModel model)
        //{
        //    try
        //    {
        //        string linkSearch = _configuration["Config:LinkSearchInvoice"];
        //        var taxCode = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.TAX_CODE)?.Value;

        //        LoaiTien loaiTien = await _dataContext.LoaiTiens.AsNoTracking().FirstOrDefaultAsync(x => x.LoaiTienId == model.HoaDonDienTu.LoaiTienId);
        //        var hoSoHDDT = await _dataContext.HoSoHDDTs.AsNoTracking().FirstOrDefaultAsync();
        //        if (hoSoHDDT == null)
        //        {
        //            hoSoHDDT = new HoSoHDDT { MaSoThue = taxCode };
        //        }

        //        //TTChung
        //        TTChung _ttChung = new TTChung
        //        {
        //            PBan = "1.1.0",
        //            THDon = ((LoaiHoaDon)model.HoaDonDienTu.LoaiHoaDon).GetDescription(),
        //            KHMSHDon = model.HoaDonDienTu.MauSo,
        //            KHHDon = model.HoaDonDienTu.KyHieu,
        //            SHDon = model.HoaDonDienTu.SoHoaDon,
        //            NLap = model.HoaDonDienTu.NgayHoaDon.Value.ToString("yyyy-MM-dd"),
        //            DVTTe = loaiTien != null ? loaiTien.Ma : string.Empty,
        //            TGia = model.HoaDonDienTu.TyGia.Value,
        //            TTNCC = "",
        //            DDTCuu = linkSearch,
        //            MTCuu = model.HoaDonDienTu.MaTraCuu,
        //            HTTToan = 1,
        //            THTTTKhac = string.Empty
        //        };
        //        #region NDHDon
        //        //NBan

        //        NBan _nBan = new NBan
        //        {
        //            Ten = hoSoHDDT.TenDonVi,
        //            MST = hoSoHDDT.MaSoThue,
        //            DChi = hoSoHDDT.DiaChi,
        //            SDThoai = hoSoHDDT.SoDienThoaiLienHe,
        //            DCTDTu = hoSoHDDT.EmailLienHe,
        //            STKNHang = hoSoHDDT.SoTaiKhoanNganHang,
        //            TNHang = hoSoHDDT.TenNganHang,
        //            Fax = hoSoHDDT.Fax,
        //            Website = hoSoHDDT.Website,
        //        };
        //        //NMua
        //        NMua _nMua = new NMua
        //        {
        //            Ten = model.HoaDonDienTu.TenKhachHang,
        //            MST = model.HoaDonDienTu.MaSoThue,
        //            DChi = model.HoaDonDienTu.DiaChi,
        //            SDThoai = model.HoaDonDienTu.SoDienThoaiNguoiMuaHang,
        //            DCTDTu = model.HoaDonDienTu.EmailNguoiMuaHang,
        //            HVTNMHang = model.HoaDonDienTu.HoTenNguoiMuaHang,
        //            STKNHang = model.HoaDonDienTu.SoTaiKhoanNganHang,
        //        };

        //        //HHDVus
        //        List<HHDVu> _dSHHDVu = new List<HHDVu>();
        //        int _STT = 1;
        //        foreach (var item in model.HoaDonDienTu.HoaDonChiTiets)
        //        {
        //            HHDVu _hhdv = new HHDVu
        //            {
        //                STT = _STT,
        //                TChat = 1,
        //                THHDVu = item.TenHang,
        //                DVTinh = item.DonViTinh != null ? item.DonViTinh.Ten : string.Empty,
        //                SLuong = item.SoLuong.Value,
        //                DGia = item.DonGia.Value,
        //                ThTien = item.ThanhTien.Value,
        //                TSuat = item.ThueGTGT,
        //            };
        //            _STT += 1;
        //            _dSHHDVu.Add(_hhdv);
        //        }

        //        //TToan
        //        List<LTSuat> listTLSuat = new List<LTSuat>();
        //        LTSuat tLSuat = new LTSuat
        //        {
        //            TSuat = model.HoaDonDienTu.HoaDonChiTiets.FirstOrDefault().ThueGTGT,
        //            ThTien = model.HoaDonDienTu.TongTienHangQuyDoi.Value,
        //            TThue = model.HoaDonDienTu.TongTienThueGTGTQuyDoi.Value
        //        };
        //        listTLSuat.Add(tLSuat);

        //        TToan _TToan = new TToan
        //        {
        //            TgTCThue = model.HoaDonDienTu.TongTienHangQuyDoi.Value,
        //            TgTThue = model.HoaDonDienTu.TongTienThueGTGTQuyDoi.Value,
        //            TgTTTBSo = model.HoaDonDienTu.TongTienThanhToanQuyDoi.Value,
        //            TgTTTBChu = model.HoaDonDienTu.SoTienBangChu,
        //            THTTLTSuat = listTLSuat
        //        };

        //        NDHDon _nDHDon = new NDHDon
        //        {
        //            NBan = _nBan,
        //            NMua = _nMua,
        //            DSHHDVu = _dSHHDVu,
        //            TToan = _TToan,
        //        };

        //        ///
        //        TTBienBan _ttBienBan = new TTBienBan
        //        {
        //            PBan = "1.1.0",
        //            NgayBienBan = model.NgayBienBan,
        //            SoBienBan = model.SoBienBan,
        //            ThongTu = model.ThongTu,
        //            MaSoThueBenA = model.MaSoThueBenA,
        //            SoDienThoaiBenA = model.SoDienThoaiBenA,
        //            TenCongTyBenA = model.TenCongTyBenA,
        //            DiaChiBenA = model.DiaChiBenA,
        //            DaiDienBenA = model.DaiDienBenA,
        //            ChucVuBenA = model.ChucVuBenA,
        //            TenKhachHang = model.TenKhachHang,
        //            MaSoThue = model.MaSoThue,
        //            DiaChi = model.DiaChi,
        //            ChucVu = model.ChucVu,
        //            DaiDien = model.DaiDien,
        //            SoDienThoai = model.SoDienThoai
        //        };

        //        BBHuy _hDon = new BBHuy
        //        {
        //            DLTTBienBan = _ttBienBan,
        //            DLHDon = new DLHDon
        //            {
        //                TTChung = _ttChung,
        //                NDHDon = _nDHDon,
        //            },
        //            DSCKS = new DSCKS
        //            {
        //                NBan = "",
        //                NMua = "",
        //            }
        //        };
        //        #endregion

        //        GenerateBillXML2(_hDon, xmlFilePath);
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}


        //private void GenerateBillXML2(HDon data, string path)
        //{
        //    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
        //    ns.Add("", "");

        //    XmlSerializer serialiser = new XmlSerializer(typeof(HDon));


        //    using (TextWriter filestream = new StreamWriter(path))
        //    {
        //        serialiser.Serialize(filestream, data, ns);
        //    }
        //}

        private string GenerateMaCQT(HoaDonDienTuViewModel hDon)
        {
            string result = "M";
            //C2
            result += hDon.LoaiHoaDon == (int)LoaiHoaDon.HoaDonGTGTCMTMTT ? ((int)LoaiHoaDon.HoaDonGTGT).ToString()
                    : hDon.LoaiHoaDon == (int)LoaiHoaDon.HoaDonBanHangCMTMTT ? ((int)LoaiHoaDon.HoaDonBanHang).ToString() : ((int)LoaiHoaDon.CacLoaiHoaDonKhac).ToString();
            result += "-";
            result += hDon.BoKyHieuHoaDon.KyHieu23 + "-";
            result += hDon.BoKyHieuHoaDon.MaCuaCQTToKhaiChapNhan.Substring(6, 5);
            result += "-";
            result += string.Format("0:00000000000", hDon.SoHoaDon.ToString());

            return result;
        }

        public async Task<HoaDonDienTuViewModel> GetHDByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id) || id == "null" || id == "undefined") return null;

            var _tuyChons = await _ITuyChonService.GetAllAsync();

            var _cachDocSo0HangChuc = _tuyChons.FirstOrDefault(x => x.Ma == "CachDocSo0OHangChuc").GiaTri;
            var _cachDocHangNghin = _tuyChons.FirstOrDefault(x => x.Ma == "CachDocSoTienOHangNghin").GiaTri;
            var _cachTheHienSoTienBangChu = int.Parse(_tuyChons.FirstOrDefault(x => x.Ma == "CachTheHienSoTienBangChu").GiaTri);
            var _cachTheHienSoTienThueLaKCT = _tuyChons.FirstOrDefault(x => x.Ma == "CachTheHienSoTienThueLaKCT").GiaTri;
            var _cachTheHienSoTienThueLaKKKNT = _tuyChons.FirstOrDefault(x => x.Ma == "CachTheHienSoTienThueLaKKKNT").GiaTri;
            var _hienThiSoChan = bool.Parse(_tuyChons.FirstOrDefault(x => x.Ma == "BoolHienThiTuChanKhiDocSoTien").GiaTri);
            var _hienThiDonViTienNgoaiTe = bool.Parse(_tuyChons.FirstOrDefault(x => x.Ma == "BoolHienThiDonViTienNgoaiTeTrenHoaDon").GiaTri);

            var thongDiepChiTiets = _dataContext.ThongDiepChiTietGuiCQTs.AsNoTracking().ToList();
            var thongDiepChungs = _dataContext.ThongDiepChungs
                .Select(x => new ThongDiepChung
                {
                    ThongDiepChungId = x.ThongDiepChungId,
                    IdThamChieu = x.IdThamChieu,
                    TrangThaiGui = x.TrangThaiGui
                })
                .ToList();
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string folder = $@"\FilesUpload\{databaseName}\{ManageFolderPath.FILE_ATTACH}";

            var tuyChonKyKeKhai = (await _dataContext.TuyChons.FirstOrDefaultAsync(x => x.Ma == "KyKeKhaiThueGTGT"))?.GiaTri;

            //cột này phải duyệt các trạng thái hóa đơn, tình trạng gửi nhận thông báo 04, v.v..
            List<HoaDonDienTu> listHoaDonDienTu = await (from hoaDon in _dataContext.HoaDonDienTus
                                                         where hoaDon.HinhThucXoabo != null || hoaDon.ThayTheChoHoaDonId != null || hoaDon.DieuChinhChoHoaDonId != null
                                                         select new HoaDonDienTu
                                                         {
                                                             HoaDonDienTuId = hoaDon.HoaDonDienTuId,
                                                             SoHoaDon = hoaDon.SoHoaDon,
                                                             ThayTheChoHoaDonId = hoaDon.ThayTheChoHoaDonId,
                                                             DieuChinhChoHoaDonId = hoaDon.DieuChinhChoHoaDonId,
                                                             NgayHoaDon = hoaDon.NgayHoaDon,
                                                             TrangThaiQuyTrinh = hoaDon.TrangThaiQuyTrinh,
                                                             MaCuaCQT = hoaDon.MaCuaCQT,
                                                             ThongDiepGuiCQTId = hoaDon.ThongDiepGuiCQTId,
                                                             TrangThaiGui04 = hoaDon.TrangThaiGui04,
                                                             LanGui04 = hoaDon.LanGui04,
                                                             IsDaLapThongBao04 = hoaDon.IsDaLapThongBao04,
                                                             CreatedDate = hoaDon.CreatedDate
                                                         }).ToListAsync();

            //đọc ra thông tin hóa đơn được nhập từ phần mềm khác, (được dùng để hiển thị cột thông tin sai sót ở hóa đơn điều chỉnh); việc đọc ra bảng này vì phải truy vấn thông tin với các hóa đơn được nhập từ phần mềm khác
            List<ThongTinHoaDon> listThongTinHoaDon = await (from hoaDon in _dataContext.ThongTinHoaDons
                                                             join hddt in _dataContext.HoaDonDienTus on hoaDon.Id equals hddt.DieuChinhChoHoaDonId
                                                             //where listHoaDonDienTu.Count(x => x.DieuChinhChoHoaDonId == hoaDon.Id) > 0
                                                             select new ThongTinHoaDon
                                                             {
                                                                 Id = hoaDon.Id,
                                                                 TrangThaiHoaDon = hoaDon.TrangThaiHoaDon,
                                                                 IsDaLapThongBao04 = hoaDon.IsDaLapThongBao04,
                                                                 LanGui04 = hoaDon.LanGui04,
                                                                 ThongDiepGuiCQTId = hoaDon.ThongDiepGuiCQTId,
                                                                 TrangThaiGui04 = hoaDon.TrangThaiGui04
                                                             }).ToListAsync();

            var query = from hd in _dataContext.HoaDonDienTus
                        join bkhhd in _dataContext.BoKyHieuHoaDons on hd.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId into tmpBoKyHieus
                        from bkhhd in tmpBoKyHieus.DefaultIfEmpty()
                        join kh in _dataContext.DoiTuongs on hd.KhachHangId equals kh.DoiTuongId into tmpKhachHangs
                        from kh in tmpKhachHangs.DefaultIfEmpty()
                        join nv in _dataContext.DoiTuongs on hd.NhanVienBanHangId equals nv.DoiTuongId into tmpNhanViens
                        from nv in tmpNhanViens.DefaultIfEmpty()
                        join nl in _dataContext.DoiTuongs on hd.CreatedBy equals nl.DoiTuongId into tmpNguoiLaps
                        from nl in tmpNguoiLaps.DefaultIfEmpty()
                        join lt in _dataContext.LoaiTiens on hd.LoaiTienId equals lt.LoaiTienId into tmpLoaiTiens
                        from lt in tmpLoaiTiens.DefaultIfEmpty()
                        join bbdc in _dataContext.BienBanDieuChinhs on hd.HoaDonDienTuId equals bbdc.HoaDonBiDieuChinhId into tmpBienBanDieuChinhs
                        from bbdc in tmpBienBanDieuChinhs.DefaultIfEmpty()
                        join bbdc_dc in _dataContext.BienBanDieuChinhs on hd.HoaDonDienTuId equals bbdc_dc.HoaDonDieuChinhId into tmpBienBanDieuChinh_DCs
                        from bbdc_dc in tmpBienBanDieuChinh_DCs.DefaultIfEmpty()
                        let mhd = _dataContext.MauHoaDons.FirstOrDefault(x => bkhhd.MauHoaDonId.Contains(x.MauHoaDonId) && ((int)x.LoaiThueGTGT == hd.LoaiThue))
                        where hd.HoaDonDienTuId == id
                        select new HoaDonDienTuViewModel
                        {
                            HoaDonDienTuId = hd.HoaDonDienTuId,
                            IsNopThueTheoThongTu1032014BTC = hd.IsNopThueTheoThongTu1032014BTC,
                            BoKyHieuHoaDonId = hd.BoKyHieuHoaDonId,
                            HoaDonThayTheDaDuocCapMa = bkhhd.HinhThucHoaDon != HinhThucHoaDon.CoMa || (!string.IsNullOrWhiteSpace(
                                                              listHoaDonDienTu.FirstOrDefault(x => x.ThayTheChoHoaDonId == hd.HoaDonDienTuId).MaCuaCQT)),
                            HoaDonDieuChinhDaDuocCapMa = bkhhd.HinhThucHoaDon != HinhThucHoaDon.CoMa || (!string.IsNullOrWhiteSpace(
                                                              listHoaDonDienTu.FirstOrDefault(x => x.DieuChinhChoHoaDonId == hd.HoaDonDienTuId).MaCuaCQT)),

                            DaDieuChinh = _dataContext.HoaDonDienTus.Any(x => x.DieuChinhChoHoaDonId == hd.HoaDonDienTuId),
                            BoKyHieuHoaDon = new BoKyHieuHoaDonViewModel
                            {
                                BoKyHieuHoaDonId = bkhhd.BoKyHieuHoaDonId,
                                KyHieu = bkhhd.KyHieu,
                                KyHieuMauSoHoaDon = bkhhd.KyHieuMauSoHoaDon,
                                KyHieuHoaDon = bkhhd.KyHieuHoaDon,
                                MauHoaDonId = bkhhd.MauHoaDonId,
                                HinhThucHoaDon = bkhhd.HinhThucHoaDon,
                                TenHinhThucHoaDon = bkhhd.HinhThucHoaDon.GetDescription(),
                                UyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon,
                                TenUyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon.GetDescription(),
                                TrangThaiSuDung = bkhhd.TrangThaiSuDung,
                                PhuongThucChuyenDL = bkhhd.PhuongThucChuyenDL
                            },
                            NgayHoaDon = hd.NgayHoaDon,
                            NgayLap = hd.CreatedDate,
                            SoHoaDon = hd.SoHoaDon,
                            MauHoaDonId = mhd != null ? mhd.MauHoaDonId : null,
                            MauHoaDon = mhd != null ? new MauHoaDonViewModel
                            {
                                MauHoaDonId = mhd.MauHoaDonId,
                                LoaiHoaDon = mhd.LoaiHoaDon,
                                LoaiThueGTGT = mhd.LoaiThueGTGT,
                            } : null,
                            LoaiThue = hd.LoaiThue,
                            KyHieuHoaDon = bkhhd.KyHieu,
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
                                            DoiTuongId = kh.DoiTuongId,
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
                            EmailNguoiNhanHD = hd.EmailNguoiNhanHD ?? string.Empty,
                            SoDienThoaiNguoiNhanHD = hd.SoDienThoaiNguoiNhanHD ?? string.Empty,
                            CanCuocCongDan = hd.CanCuocCongDan ?? string.Empty,
                            LoaiTienId = lt.LoaiTienId ?? string.Empty,
                            MaLoaiTien = hd.MaLoaiTien,
                            LoaiTien = lt != null ? new LoaiTienViewModel
                            {
                                Ma = lt.Ma,
                                Ten = lt.Ten
                            } : null,
                            TyGia = hd.TyGia ?? 1,
                            IsVND = lt == null || (lt.Ma == "VND"),
                            TrangThai = hd.TrangThai,
                            TrangThaiQuyTrinh = hd.TrangThaiQuyTrinh,
                            TenTrangThaiQuyTrinh = ((TrangThaiQuyTrinh)hd.TrangThaiQuyTrinh).GetDescription(),
                            MaTraCuu = hd.MaTraCuu,
                            IsBuyerSigned = hd.IsBuyerSigned,
                            TrangThaiGuiHoaDon = hd.TrangThaiGuiHoaDon,
                            TrangThaiGuiHoaDonNhap = hd.TrangThaiGuiHoaDonNhap,
                            KhachHangDaNhan = hd.KhachHangDaNhan ?? false,
                            SoLanChuyenDoi = hd.SoLanChuyenDoi ?? 0,
                            LyDoXoaBo = hd.LyDoXoaBo,
                            NgayXoaBo = hd.NgayXoaBo,
                            SoCTXoaBo = hd.SoCTXoaBo,
                            IsNotCreateThayThe = hd.IsNotCreateThayThe,
                            HinhThucXoabo = hd.HinhThucXoabo,
                            BackUpTrangThai = hd.BackUpTrangThai,
                            IdHoaDonSaiSotBiThayThe = hd.IdHoaDonSaiSotBiThayThe,
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
                            LyDoBiDieuChinh = bbdc != null ? bbdc.LyDoDieuChinh : null,
                            NhanVienBanHangId = hd.NhanVienBanHangId,
                            IsLapVanBanThoaThuan = hd.IsLapVanBanThoaThuan,
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
                            TrangThaiBienBanDieuChinh = bbdc_dc != null ? bbdc_dc.TrangThaiBienBan : (bbdc != null ? bbdc.TrangThaiBienBan : (int)LoaiTrangThaiBienBanDieuChinhHoaDon.ChuaLapBienBan),
                            ThoiHanThanhToan = hd.ThoiHanThanhToan,
                            DiaChiGiaoHang = hd.DiaChiGiaoHang,
                            BienBanDieuChinhId = bbdc_dc != null ? bbdc_dc.BienBanDieuChinhId : (bbdc != null ? bbdc.BienBanDieuChinhId : null),
                            //LyDoDieuChinhModel = string.IsNullOrEmpty(hd.LyDoDieuChinh) ? null : JsonConvert.DeserializeObject<LyDoDieuChinhModel>(hd.LyDoDieuChinh),
                            LyDoThayTheModel = string.IsNullOrEmpty(hd.LyDoThayThe) ? null : JsonConvert.DeserializeObject<LyDoThayTheModel>(hd.LyDoThayThe),
                            GhiChuThayTheSaiSot = hd.GhiChuThayTheSaiSot,
                            // pxk
                            CanCuSo = hd.CanCuSo,
                            NgayCanCu = hd.NgayCanCu,
                            Cua = hd.Cua,
                            DienGiai = hd.DienGiai,
                            DiaChiKhoNhanHang = hd.DiaChiKhoNhanHang,
                            HoTenNguoiNhanHang = hd.HoTenNguoiNhanHang,
                            DiaChiKhoXuatHang = hd.DiaChiKhoXuatHang,
                            HoTenNguoiXuatHang = hd.HoTenNguoiXuatHang,
                            HopDongVanChuyenSo = hd.HopDongVanChuyenSo,
                            TenNguoiVanChuyen = hd.TenNguoiVanChuyen,
                            PhuongThucVanChuyen = hd.PhuongThucVanChuyen,
                            HoaDonChiTiets = (
                                               from hdct in _dataContext.HoaDonDienTuChiTiets
                                               join hd in _dataContext.HoaDonDienTus on hdct.HoaDonDienTuId equals hd.HoaDonDienTuId into tmpHoaDons
                                               from hd in tmpHoaDons.DefaultIfEmpty()
                                               join vt in _dataContext.HangHoaDichVus on hdct.HangHoaDichVuId equals vt.HangHoaDichVuId into tmpHangHoas
                                               from vt in tmpHangHoas.DefaultIfEmpty()
                                               join dvt in _dataContext.DonViTinhs on hdct.DonViTinhId equals dvt.DonViTinhId into tmpDonViTinhs
                                               from dvt in tmpDonViTinhs.DefaultIfEmpty()
                                               join nv in _dataContext.DoiTuongs on hdct.NhanVienBanHangId equals nv.DoiTuongId into tmpNhanViens
                                               from nv in tmpNhanViens.DefaultIfEmpty()
                                               where hdct.HoaDonDienTuId == id
                                               orderby hdct.CreatedDate
                                               select new HoaDonDienTuChiTietViewModel
                                               {
                                                   STT = hdct.STT,
                                                   HoaDonDienTuChiTietId = hdct.HoaDonDienTuChiTietId,
                                                   HoaDonDienTuId = hd.HoaDonDienTuId,
                                                   HangHoaDichVuId = vt.HangHoaDichVuId,
                                                   HangHoaDichVu = new HangHoaDichVuViewModel
                                                   {
                                                       HangHoaDichVuId = vt.HangHoaDichVuId,
                                                       Ma = vt.Ma,
                                                       Ten = vt.Ten,
                                                       DonGiaBan = vt.DonGiaBan,
                                                       ThueGTGT = vt.ThueGTGT,
                                                       IsGiaBanLaDonGiaSauThue = vt.IsGiaBanLaDonGiaSauThue,
                                                       TyLeChietKhau = vt.TyLeChietKhau
                                                   },
                                                   MaHang = hdct.MaHang,
                                                   TenHang = hdct.TenHang,
                                                   TinhChat = hdct.TinhChat,
                                                   TenTinhChat = ((TChat)(hdct.TinhChat)).GetDescription(),
                                                   DonViTinhId = dvt.DonViTinhId,
                                                   TenDonViTinh = hdct.TenDonViTinh,
                                                   DonViTinh = dvt != null ? new DonViTinhViewModel
                                                   {
                                                       Ten = dvt.Ten
                                                   } : null,
                                                   SoLuong = hdct.SoLuong,
                                                   SoLuongNhap = hdct.SoLuongNhap,
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
                                                   IsMatHangDuocGiam = hdct.IsMatHangDuocGiam,
                                                   TyLePhanTramDoanhThu = hdct.TyLePhanTramDoanhThu,
                                                   TienGiam = hdct.TienGiam ?? 0,
                                                   TienGiamQuyDoi = hdct.TienGiamQuyDoi ?? 0,
                                                   SoLo = hdct.SoLo,
                                                   HanSuDung = hdct.HanSuDung,
                                                   SoKhung = hdct.SoKhung,
                                                   SoMay = hdct.SoMay,
                                                   NhanVienBanHangId = hdct.NhanVienBanHangId,
                                                   MaNhanVien = hdct.MaNhanVien,
                                                   TenNhanVien = hdct.TenNhanVien,
                                                   NhanVien = nv != null ? new DoiTuongViewModel
                                                   {
                                                       Ma = nv.Ma,
                                                       Ten = nv.Ten
                                                   } : null,
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
                            TongTienHang = hd.TongTienHang,
                            TongTienHangQuyDoi = hd.TongTienHangQuyDoi,
                            TongTienChietKhau = hd.TongTienChietKhau,
                            TongTienChietKhauQuyDoi = hd.TongTienChietKhauQuyDoi,
                            TongTienThueGTGT = hd.TongTienThueGTGT,
                            TongTienThueGTGTQuyDoi = hd.TongTienThueGTGTQuyDoi,
                            TongTienThanhToan = hd.TongTienThanhToan,
                            TongTienThanhToanQuyDoi = hd.TongTienThanhToanQuyDoi,
                            TongTienGiam = hd.TongTienGiam ?? 0,
                            TongTienGiamQuyDoi = hd.TongTienGiamQuyDoi ?? 0,
                            CreatedBy = hd.CreatedBy,
                            CreatedDate = hd.CreatedDate,
                            Status = hd.Status,
                            TrangThaiThoaThuan = hd.IsLapVanBanThoaThuan == true ? "Có thỏa thuận" : "Không thỏa thuận",
                            MaCuaCQT = hd.MaCuaCQT,
                            MCCQT = hd.MCCQT,
                            NgayKy = hd.NgayKy,
                            IsPos = hd.IsPos,
                            LoaiChietKhau = hd.LoaiChietKhau,
                            TyLeChietKhau = hd.TyLeChietKhau,
                            TrangThaiBienBanXoaBo = hd.TrangThaiBienBanXoaBo,
                            DaGuiThongBaoXoaBoHoaDon = hd.DaGuiThongBaoXoaBoHoaDon,
                            UyNhiemLapHoaDon = (int)bkhhd.UyNhiemLapHoaDon,
                            TenUyNhiemLapHoaDon = bkhhd.UyNhiemLapHoaDon.GetDescription(),
                            LoaiApDungHoaDonDieuChinh = 1,
                            IsGiamTheoNghiQuyet = hd.IsGiamTheoNghiQuyet,
                            TyLePhanTramDoanhThu = hd.TyLePhanTramDoanhThu ?? 0,
                            IsThongTinNguoiBanHoacNguoiMua = hd.IsThongTinNguoiBanHoacNguoiMua,
                            IsTheHienLyDoTrenHoaDon = hd.IsTheHienLyDoTrenHoaDon,
                            IsKemGuiEmail = hd.IsKemGuiEmail,
                            EmailNhanKemTheo = hd.EmailNhanKemTheo,
                            TenNguoiNhanKemTheo = hd.TenNguoiNhanKemTheo
                        };

            var result = await query.FirstOrDefaultAsync();
            if (result == null)
            {
                return null;
            }

            result.SoTienBangChu = result.TongTienThanhToan.Value
                                                .MathRoundNumberByTuyChon(_tuyChons, result.IsVND == true ? LoaiDinhDangSo.TIEN_QUY_DOI : LoaiDinhDangSo.TIEN_NGOAI_TE)
                                                .ConvertToInWord(_cachDocSo0HangChuc.ToLower(), _cachDocHangNghin.ToLower(), _hienThiSoChan, result.LoaiTien.Ma, _cachTheHienSoTienBangChu, result);

            #region xử lý trạng thái khác
            var hoaDonDieuChinh_ThayThes = await _dataContext.HoaDonDienTus
            .Where(x => x.DieuChinhChoHoaDonId == result.HoaDonDienTuId || x.ThayTheChoHoaDonId == result.HoaDonDienTuId)
            .AsNoTracking()
            .ToListAsync();

            var bienBanDieuChinhs = await _dataContext.BienBanDieuChinhs
                .Where(x => x.HoaDonBiDieuChinhId == result.HoaDonDienTuId)
                .AsNoTracking()
                .ToListAsync();

            result.DaLapHoaDonThayThe = hoaDonDieuChinh_ThayThes.Any(x => x.ThayTheChoHoaDonId == result.HoaDonDienTuId);
            result.HinhThucDieuChinh = GetHinhThucDieuChinh(result, hoaDonDieuChinh_ThayThes.Any(x => x.ThayTheChoHoaDonId == result.HoaDonDienTuId), hoaDonDieuChinh_ThayThes.Any(x => x.DieuChinhChoHoaDonId == result.HoaDonDienTuId) || bienBanDieuChinhs.Any(x => x.HoaDonBiDieuChinhId == result.HoaDonDienTuId));
            result.IsLapHoaDonThayThe = (result.TrangThai == (int)TrangThaiHoaDon.HoaDonGoc) && (result.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa) && result.DaLapHoaDonThayThe != true;
            result.IsLapHoaDonDieuChinh = (result.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.CQTDaCapMa) && (result.TrangThai == (int)TrangThaiHoaDon.HoaDonGoc) && (result.TrangThaiGuiHoaDon >= (int)TrangThaiGuiHoaDon.DaGui) && !hoaDonDieuChinh_ThayThes.Any(x => x.DieuChinhChoHoaDonId == result.HoaDonDienTuId);
            #endregion

            if (!string.IsNullOrEmpty(result.LyDoDieuChinh) && result.LyDoDieuChinh.StartsWith("{"))
            {
                result.LyDoDieuChinhModel = JsonConvert.DeserializeObject<LyDoDieuChinhModel>(result.LyDoDieuChinh);
            }
            else
            {
                if (string.IsNullOrEmpty(result.LyDoDieuChinh))
                {
                    if (!string.IsNullOrEmpty(result.BienBanDieuChinhId))
                    {
                        var bbdc = _dataContext.BienBanDieuChinhs.FirstOrDefault(x => x.BienBanDieuChinhId == result.BienBanDieuChinhId);
                        result.LyDoDieuChinhModel = new LyDoDieuChinhModel
                        {
                            LyDo = bbdc.LyDoDieuChinh
                        };
                    }
                    else result.LyDoDieuChinhModel = null;
                }
                else result.LyDoDieuChinhModel = new LyDoDieuChinhModel { LyDo = result.LyDoDieuChinh };
            }

            if (result.LyDoDieuChinhModel != null)
            {
                result.LyDoDieuChinhModel.DieuChinhChoHoaDonId = result.DieuChinhChoHoaDonId;
            }
            result.TenTrangThaiLanDieuChinhGanNhat = result.TrangThaiLanDieuChinhGanNhat.HasValue ? ((TrangThaiQuyTrinh)result.TrangThaiLanDieuChinhGanNhat.Value).GetDescription() : string.Empty;
            return result;
        }

        private string GetHinhThucDieuChinh(HoaDonDienTuViewModel model, bool isHoaDonXoaBoDaBiThayThe, bool isHoaDonBiDieuChinh)
        {
            TrangThaiQuyTrinh trangThaiQuyTrinh = (TrangThaiQuyTrinh)model.TrangThaiQuyTrinh;
            TrangThaiHoaDon trangThaiHoaDon = (TrangThaiHoaDon)model.TrangThai;

            if ((trangThaiQuyTrinh == TrangThaiQuyTrinh.ChuaKyDienTu ||
                    trangThaiQuyTrinh == TrangThaiQuyTrinh.KyDienTuLoi ||
                    trangThaiQuyTrinh == TrangThaiQuyTrinh.DangKyDienTu) &&
                (trangThaiHoaDon == TrangThaiHoaDon.HoaDonGoc ||
                    trangThaiHoaDon == TrangThaiHoaDon.HoaDonThayThe))
            {
                return string.Empty;
            }

            if (trangThaiQuyTrinh != TrangThaiQuyTrinh.ChuaKyDienTu &&
                trangThaiQuyTrinh != TrangThaiQuyTrinh.KyDienTuLoi &&
                trangThaiQuyTrinh != TrangThaiQuyTrinh.DangKyDienTu &&
                (trangThaiHoaDon == TrangThaiHoaDon.HoaDonGoc || trangThaiHoaDon == TrangThaiHoaDon.HoaDonThayThe) &&
                !isHoaDonBiDieuChinh)
            {
                return "Chưa điều chỉnh";
            }

            if (trangThaiHoaDon == TrangThaiHoaDon.HoaDonDieuChinh)
            {
                return "Không điều chỉnh";
            }

            if ((trangThaiHoaDon == TrangThaiHoaDon.HoaDonXoaBo && !isHoaDonXoaBoDaBiThayThe))
            {
                return "Hủy";
            }

            if (trangThaiQuyTrinh != TrangThaiQuyTrinh.ChuaKyDienTu &&
                trangThaiQuyTrinh != TrangThaiQuyTrinh.KyDienTuLoi &&
                trangThaiQuyTrinh != TrangThaiQuyTrinh.DangKyDienTu &&
                (trangThaiHoaDon == TrangThaiHoaDon.HoaDonGoc || trangThaiHoaDon == TrangThaiHoaDon.HoaDonThayThe) &&
                isHoaDonBiDieuChinh)
            {
                return "Điều chỉnh";
            }

            if (trangThaiHoaDon == TrangThaiHoaDon.HoaDonXoaBo) // thay thế
            {
                return "Thay thế";
            }

            return string.Empty;
        }

    }
}