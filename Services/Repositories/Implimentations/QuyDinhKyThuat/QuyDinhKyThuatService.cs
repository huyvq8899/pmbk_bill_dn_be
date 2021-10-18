using AutoMapper;
using DLL;
using DLL.Constants;
using DLL.Entity.DanhMuc;
using DLL.Entity.QuyDinhKyThuat;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Services.Helper;
using Services.Repositories.Interfaces;
using Services.Repositories.Interfaces.QuyDinhKyThuat;
using Services.ViewModels.QuanLyHoaDonDienTu;
using Services.ViewModels.QuyDinhKyThuat;
using Services.ViewModels.XML.HoaDonDienTu;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Services.Repositories.Implimentations.QuyDinhKyThuat
{
    public class QuyDinhKyThuatService : IQuyDinhKyThuatService
    {
        private readonly Datacontext _dataContext;
        private readonly Random _random;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mp;
        private readonly IXMLInvoiceService _xmlInvoiceService;

        public QuyDinhKyThuatService(Datacontext dataContext, IHttpContextAccessor httpContextAccessor, IHostingEnvironment hostingEnvironment, IConfiguration configuration, IMapper mp, IXMLInvoiceService xmlInvoiceService)
        {
            _dataContext = dataContext;
            _random = new Random();
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _mp = mp;
            _xmlInvoiceService = xmlInvoiceService;
        }

        public async Task<bool> LuuToKhaiDangKyThongTin(ToKhaiDangKyThongTinViewModel tKhai)
        {
            var _entity = _mp.Map<ToKhaiDangKyThongTin>(tKhai);
            string folderName = tKhai.NhanUyNhiem == true ? "QuyDinhKyThuatHDDT_PhanII_I_2" : "QuyDinhKyThuatHDDT_PhanII_I_1";
            string assetsFolder = $"FilesUpload/QuyDinhKyThuat/{folderName}/unsigned";
            var fullXmlFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);
            var fullXmlName = Path.Combine(fullXmlFolder, tKhai.FileXMLChuaKy);
            //string xmlDeCode = DataHelper.Base64Decode(fullXmlName);
            byte[] byteXML = Encoding.UTF8.GetBytes(fullXmlName);
            _entity.ContentXMLChuaKy = byteXML;
            _entity.Id = Guid.NewGuid().ToString();
            _entity.NgayTao = DateTime.Now;
            await _dataContext.ToKhaiDangKyThongTins.AddAsync(_entity);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> SuaToKhaiDangKyThongTin(ToKhaiDangKyThongTinViewModel tKhai)
        {
            var _entity = _mp.Map<ToKhaiDangKyThongTin>(tKhai);
            string folderName = tKhai.NhanUyNhiem == true ? "QuyDinhKyThuatHDDT_PhanII_I_2" : "QuyDinhKyThuatHDDT_PhanII_I_1";
            string assetsFolder = $"FilesUpload/QuyDinhKyThuat/{folderName}/unsigned";
            var fullXmlFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);
            var fullXmlName = Path.Combine(fullXmlFolder, tKhai.FileXMLChuaKy);
            //string xmlDeCode = DataHelper.Base64Decode(fullXmlName);
            byte[] byteXML = Encoding.UTF8.GetBytes(fullXmlName);
            _entity.ContentXMLChuaKy = byteXML;
            
            _dataContext.ToKhaiDangKyThongTins.Update(_entity);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> LuuDuLieuKy(DuLieuKyToKhaiViewModel kTKhai)
        {
            var _entity = _mp.Map<DuLieuKyToKhai>(kTKhai);
            _entity.Id = Guid.NewGuid().ToString();
            _entity.NgayKy = DateTime.Now;
            //string xmlDeCode = DataHelper.Base64Decode(kTKhai.NoiDungKy);
            var base64EncodedBytes = System.Convert.FromBase64String(kTKhai.Content);
            byte[] byteXML = Encoding.UTF8.GetBytes(kTKhai.Content);
            _entity.NoiDungKy = byteXML;
            var _entityTK = await _dataContext.ToKhaiDangKyThongTins.FirstOrDefaultAsync(x => x.Id == kTKhai.IdToKhai);
            var fileName = Guid.NewGuid().ToString() + ".xml";
            string assetsFolder = !_entityTK.NhanUyNhiem ? $"FilesUpload/QuyDinhKyThuat/QuyDinhKyThuatHDDT_PhanII_I_1/signed" : $"FilesUpload/QuyDinhKyThuat/QuyDinhKyThuatHDDT_PhanII_I_2/signed";
            var fullXmlFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);
            #region create folder
            if (!Directory.Exists(fullXmlFolder))
            {
                Directory.CreateDirectory(fullXmlFolder);
            }
            #endregion
            var fullXMLFile = Path.Combine(fullXmlFolder, fileName);
            File.WriteAllText(fullXMLFile, System.Text.Encoding.UTF8.GetString(base64EncodedBytes));
            _entity.FileXMLDaKy = fileName;
            await _dataContext.DuLieuKyToKhais.AddAsync(_entity);

       
            if (!_entityTK.SignedStatus)
            {
                _entityTK.SignedStatus = true;
                _dataContext.Update(_entityTK);
            }
            else
            {
                _dataContext.Update(_entityTK);
            }
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> LuuTrangThaiGuiToKhai(TrangThaiGuiToKhaiViewModel tThai)
        {
            var _entity = _mp.Map<TrangThaiGuiToKhai>(tThai);
            _entity.Id = Guid.NewGuid().ToString();
            var _entityTK = await _dataContext.ToKhaiDangKyThongTins.FirstOrDefaultAsync(x => x.Id == tThai.IdToKhai);
            string assetsFolder = !_entityTK.NhanUyNhiem ? $"FilesUpload/QuyDinhKyThuat/QuyDinhKyThuatHDDT_PhanII_I_8/unsigned" : $"FilesUpload/QuyDinhKyThuat/QuyDinhKyThuatHDDT_PhanII_I_9/unsigned";
            var fullXmlFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);
            #region create folder
            if (!Directory.Exists(fullXmlFolder))
            {
                Directory.CreateDirectory(fullXmlFolder);
            }
            #endregion
            var fullXMLFile = Path.Combine(fullXmlFolder, tThai.FileXMLGui);
            _entity.NoiDungFileGui = await File.ReadAllBytesAsync(fullXMLFile);
         
            await _dataContext.TrangThaiGuiToKhais.AddAsync(_entity);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> GuiToKhai(string XMLUrl, string idTKhai)
        {
            var entityTK = await _dataContext.ToKhaiDangKyThongTins.FirstOrDefaultAsync(x => x.Id == idTKhai);
            var assetsFolder = entityTK.NhanUyNhiem ? $"FilesUpload/QuyDinhKyThuat/QuyDinhKyThuatHDDT_PhanII_I_8/unsigned" : $"FilesUpload/QuyDinhKyThuat/QuyDinhKyThuatHDDT_PhanII_I_9/unsigned";
            var fullXmlFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);
            XmlDocument xml = new XmlDocument();
            xml.Load(Path.Combine(fullXmlFolder,XMLUrl));
            StringWriter sw = new StringWriter();
            XmlTextWriter xw = new XmlTextWriter(sw);
            xml.DocumentElement.WriteTo(xw);
            string xmlString = sw.ToString();

            XmlSerializer serialiser = !entityTK.NhanUyNhiem ? new XmlSerializer(typeof(ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._8.TDiep)) : new XmlSerializer(typeof(ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._9.TDiep));
            StringReader rdr = new StringReader(xmlString);
            if (!entityTK.NhanUyNhiem)
            {
                var model = (ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._8.TDiep)serialiser.Deserialize(rdr);
                if (model != null)
                {
                    var tBaoTiepNhan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._3.TBao
                    {
                        DLTBao = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._3.DLTBao
                        {
                            PBan = model.TTChung.PBan,
                            MSo = "01/TB-TNĐT",
                            Ten = "Về việc tiếp nhận tờ khai đăng ký/thay đổi thông tin sử dụng HDDT",
                            So = RandomString(30),
                            DDanh = "TCT",
                            NTBao = DateTime.Now.ToString("yyyy-MM-dd"),
                            MST = model.TTChung.MST,
                            TNNT = model.DLieu.TKhai.DLTKhai.TTChung.TNNT,
                            TTKhai = model.DLieu.TKhai.DLTKhai.TTChung.Ten,
                            MGDDTu = RandomString(46),
                            TGGui = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            THop = THop.TruongHop3,
                            TGNhan = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            DSLDKCNhan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._3.DSLDKCNhan()
                        },
                        DSCKS = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._3.DSCKS()
                    };

                    //convert to xml
                    var tDiepPhanHoi = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._10.TDiep
                    {
                        TTChung = new ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities.TTChungThongDiep
                        {
                            PBan = tBaoTiepNhan.DLTBao.PBan,
                            MNGui = "TCT",
                            MNNhan = model.TTChung.MNGui,
                            MLTDiep = "101",
                            MTDiep = $"TCT{Guid.NewGuid().ToString().Replace("-", "").ToUpper()}",
                            MTDTChieu = model.TTChung.MTDiep,
                            MST = model.TTChung.MST,
                            SLuong = 1,
                        },
                        DLieu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._10.DLieu
                        {
                            TBao = tBaoTiepNhan,
                        }
                    };

                    var xmlPhanHoi = _xmlInvoiceService.CreateFileXML(tDiepPhanHoi, "QuyDinhKyThuatHDDT.PhanII.I._10");
                    return true;
                }
                else
                {
                    var tBaoTiepNhan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._3.TBao
                    {
                        DLTBao = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._3.DLTBao
                        {
                            PBan = model.TTChung.PBan,
                            MSo = "01/TB-TNĐT",
                            Ten = "Về việc không tiếp nhận tờ khai đăng ký/thay đổi thông tin sử dụng HDDT",
                            So = RandomString(30),
                            DDanh = "TCT",
                            NTBao = DateTime.Now.ToString("yyyy-MM-dd"),
                            MST = model.TTChung.MST,
                            TNNT = model.DLieu.TKhai.DLTKhai.TTChung.TNNT,
                            TTKhai = model.DLieu.TKhai.DLTKhai.TTChung.Ten,
                            MGDDTu = RandomString(46),
                            TGGui = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            THop = THop.TruongHop4,
                            TGNhan = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            DSLDKCNhan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._3.DSLDKCNhan
                            {
                                LDo = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._3.LDo>
                                {
                                    new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._3.LDo
                                    {
                                        MLoi = "0001",
                                        MTa = "Không đọc được dữ liệu gửi"
                                    }
                                }
                            }
                        },
                        DSCKS = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._3.DSCKS()
                    };
                    //convert to xml
                    var tDiepPhanHoi = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._10.TDiep
                    {
                        TTChung = new ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities.TTChungThongDiep
                        {
                            PBan = tBaoTiepNhan.DLTBao.PBan,
                            MNGui = "TCT",
                            MNNhan = model.TTChung.MNGui,
                            MLTDiep = "101",
                            MTDiep = $"TCT{Guid.NewGuid().ToString().Replace("-", "").ToUpper()}",
                            MTDTChieu = model.TTChung.MTDiep,
                            MST = model.TTChung.MST,
                            SLuong = 1,
                        },
                        DLieu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._10.DLieu
                        {
                            TBao = tBaoTiepNhan,
                        }
                    };

                    var xmlPhanHoi = _xmlInvoiceService.CreateFileXML(tDiepPhanHoi, "QuyDinhKyThuatHDDT.PhanII.I._10");
                    return false;
                }
            }
            else
            {
                var model = (ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._9.TDiep)serialiser.Deserialize(rdr);
                if (model != null)
                {
                    var tBaoTiepNhan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._3.TBao
                    {
                        DLTBao = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._3.DLTBao
                        {
                            PBan = model.TTChung.PBan,
                            MSo = "01/TB-TNĐT",
                            Ten = "Về việc tiếp nhận tờ khai đăng ký/thay đổi thông tin sử dụng HDDT",
                            So = RandomString(30),
                            DDanh = "TCT",
                            NTBao = DateTime.Now.ToString("yyyy-MM-dd"),
                            MST = model.TTChung.MST,
                            TNNT = model.DLieu.TKhai.DLTKhai.TTChung.TNNT,
                            TTKhai = model.DLieu.TKhai.DLTKhai.TTChung.Ten,
                            MGDDTu = RandomString(46),
                            TGGui = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            THop = THop.TruongHop3,
                            TGNhan = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            DSLDKCNhan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._3.DSLDKCNhan()
                        },
                        DSCKS = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._3.DSCKS()
                    };

                    //convert to xml
                    var tDiepPhanHoi = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._10.TDiep
                    {
                        TTChung = new ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities.TTChungThongDiep
                        {
                            PBan = tBaoTiepNhan.DLTBao.PBan,
                            MNGui = "TCT",
                            MNNhan = model.TTChung.MNGui,
                            MLTDiep = "101",
                            MTDiep = $"TCT{Guid.NewGuid().ToString().Replace("-", "").ToUpper()}",
                            MTDTChieu = model.TTChung.MTDiep,
                            MST = model.TTChung.MST,
                            SLuong = 1,
                        },
                        DLieu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._10.DLieu
                        {
                            TBao = tBaoTiepNhan,
                        }
                    };

                    var xmlPhanHoi = _xmlInvoiceService.CreateFileXML(tBaoTiepNhan, "QuyDinhKyThuatHDDT.PhanII.I._10");
                    return true;
                }
                else
                {
                    var tBaoTiepNhan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._3.TBao
                    {
                        DLTBao = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._3.DLTBao
                        {
                            PBan = model.TTChung.PBan,
                            MSo = "01/TB-TNĐT",
                            Ten = "Về việc không tiếp nhận tờ khai đăng ký/thay đổi thông tin sử dụng HDDT",
                            So = RandomString(30),
                            DDanh = "TCT",
                            NTBao = DateTime.Now.ToString("yyyy-MM-dd"),
                            MST = model.TTChung.MST,
                            TNNT = model.DLieu.TKhai.DLTKhai.TTChung.TNNT,
                            TTKhai = model.DLieu.TKhai.DLTKhai.TTChung.Ten,
                            MGDDTu = RandomString(46),
                            TGGui = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            THop = THop.TruongHop4,
                            TGNhan = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            DSLDKCNhan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._3.DSLDKCNhan
                            {
                                LDo = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._3.LDo>
                                {
                                    new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._3.LDo
                                    {
                                        MLoi = "0001",
                                        MTa = "Không đọc được dữ liệu gửi"
                                    }
                                }
                            }
                        },
                        DSCKS = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._3.DSCKS()
                    };

                    var tDiepPhanHoi = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._10.TDiep
                    {
                        TTChung = new ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities.TTChungThongDiep
                        {
                            PBan = tBaoTiepNhan.DLTBao.PBan,
                            MNGui = "TCT",
                            MNNhan = model.TTChung.MNGui,
                            MLTDiep = "101",
                            MTDiep = $"TCT{Guid.NewGuid().ToString().Replace("-", "").ToUpper()}",
                            MTDTChieu = model.TTChung.MTDiep,
                            MST = model.TTChung.MST,
                            SLuong = 1,
                        },
                        DLieu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._10.DLieu
                        {
                            TBao = tBaoTiepNhan,
                        }
                    };

                    var xmlPhanHoi = _xmlInvoiceService.CreateFileXML(tDiepPhanHoi, "QuyDinhKyThuatHDDT.PhanII.I._10");
                    return false;
                }
            }

        }

        public async Task<bool> NhanPhanHoiCQT(string fileXML, string idTKhai)
        {
            var entityTK = await _dataContext.ToKhaiDangKyThongTins.FirstOrDefaultAsync(x => x.Id == idTKhai);
            var assetsFolder = entityTK.NhanUyNhiem ? $"FilesUpload/QuyDinhKyThuat/QuyDinhKyThuatHDDT_PhanII_I_8/unsigned" : $"FilesUpload/QuyDinhKyThuat/QuyDinhKyThuatHDDT_PhanII_I_9/unsigned";
            var fullXmlFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);
            XmlDocument xml = new XmlDocument();
            xml.Load(Path.Combine(fullXmlFolder, fileXML));
            StringWriter sw = new StringWriter();
            XmlTextWriter xw = new XmlTextWriter(sw);
            xml.DocumentElement.WriteTo(xw);
            string xmlString = sw.ToString();

            XmlSerializer serialiser = !entityTK.NhanUyNhiem ? new XmlSerializer(typeof(ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._8.TDiep)) : new XmlSerializer(typeof(ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._9.TDiep));
            StringReader rdr = new StringReader(xmlString);
            if (!entityTK.NhanUyNhiem)
            {
                var model = (ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._8.TDiep)serialiser.Deserialize(rdr);
                if (model != null)
                {
                    var tBaoTiepNhan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._11.TDiep
                    {
                        TTChung = new ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities.TTChungThongDiep
                        {
                            PBan = model.TTChung.PBan,
                            MNGui = "TCT",
                            MNNhan = model.TTChung.MNGui,
                            MLTDiep = "101",
                            MTDiep = $"TCT{Guid.NewGuid().ToString().Replace("-", "").ToUpper()}",
                            MTDTChieu = model.TTChung.MTDiep,
                            MST = model.TTChung.MST,
                            SLuong = 1,
                        },
                        DLieu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._11.DLieu
                        {
                            TBao = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._4.TBao
                            {
                                DLTBao = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._4.DLTBao
                                {
                                    PBan = model.TTChung.PBan,
                                    MSo = "01/TB-TNĐT",
                                    Ten = "Về việc tiếp nhận tờ khai đăng ký/thay đổi thông tin sử dụng HDDT",
                                    DDanh = "Hà Nội",
                                    TCQT = "TCT",
                                    TCQTCTren = "TCT",
                                    MST = model.TTChung.MST,
                                    TNNT = model.DLieu.TKhai.DLTKhai.TTChung.TNNT,
                                    Ngay = DateTime.Now.ToString("yyyy-MM-dd"),
                                    HTDKy = model.DLieu.TKhai.DLTKhai.TTChung.HThuc,
                                    TTXNCQT = (int)TTXNCQT.ChapNhan,
                                    HThuc = "Chữ ký số",
                                    CDanh = "Giám đốc",
                                    DSLDKCNhan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._4.DSLDKCNhan()
                                },
                                STBao = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._4.STBao
                                {
                                    So = RandomString(30),
                                    NTBao = DateTime.Now.ToString("yyyy-MM-dd")
                                },
                                DSCKS = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._4.DSCKS()
                            }
                        }
                    };

                    //convert to xml
                    var xmlPhanHoi = _xmlInvoiceService.CreateFileXML(tBaoTiepNhan, "QuyDinhKyThuatHDDT.PhanII.I._11");
                    return true;
                }
                else
                {
                    var tBaoTiepNhan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._11.TDiep
                    {
                        TTChung = new ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities.TTChungThongDiep
                        {
                            PBan = model.TTChung.PBan,
                            MNGui = "TCT",
                            MNNhan = model.TTChung.MNGui,
                            MLTDiep = "101",
                            MTDiep = $"TCT{Guid.NewGuid().ToString().Replace("-", "").ToUpper()}",
                            MTDTChieu = model.TTChung.MTDiep,
                            MST = model.TTChung.MST,
                            SLuong = 1,
                        },
                        DLieu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._11.DLieu
                        {
                            TBao = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._4.TBao
                            {
                                DLTBao = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._4.DLTBao
                                {
                                    PBan = model.TTChung.PBan,
                                    MSo = "01/TB-TNĐT",
                                    Ten = "Về việc tiếp nhận tờ khai đăng ký/thay đổi thông tin sử dụng HDDT",
                                    DDanh = "Hà Nội",
                                    TCQT = "TCT",
                                    TCQTCTren = "TCT",
                                    MST = model.TTChung.MST,
                                    TNNT = model.DLieu.TKhai.DLTKhai.TTChung.TNNT,
                                    Ngay = DateTime.Now.ToString("yyyy-MM-dd"),
                                    HTDKy = model.DLieu.TKhai.DLTKhai.TTChung.HThuc,
                                    TTXNCQT = (int)TTXNCQT.ChapNhan,
                                    HThuc = "Chữ ký số",
                                    CDanh = "Giám đốc",
                                    DSLDKCNhan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._4.DSLDKCNhan
                                    {
                                        LDo = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._4.LDo>
                                        {
                                            new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._4.LDo
                                            {
                                                MLoi = "0001",
                                                MTa = "Không đọc được dữ liệu gửi"
                                            }
                                        }
                                    }
                                },
                                STBao = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._4.STBao
                                {
                                    So = RandomString(30),
                                    NTBao = DateTime.Now.ToString("yyyy-MM-dd")
                                },
                                DSCKS = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._4.DSCKS()
                            }
                        }
                    };

                    //convert to xml
                    var xmlPhanHoi = _xmlInvoiceService.CreateFileXML(tBaoTiepNhan, "QuyDinhKyThuatHDDT.PhanII.I._4");
                    return false;
                }
            }
            else
            {
                var model = (ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._9.TDiep)serialiser.Deserialize(rdr);
                if (model != null)
                {
                    var tBaoTiepNhan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._12.TDiep
                    {
                        TTChung = new ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities.TTChungThongDiep
                        {
                            PBan = model.TTChung.PBan,
                            MNGui = "TCT",
                            MNNhan = model.TTChung.MNGui,
                            MLTDiep = "101",
                            MTDiep = $"TCT{Guid.NewGuid().ToString().Replace("-", "").ToUpper()}",
                            MTDTChieu = model.TTChung.MTDiep,
                            MST = model.TTChung.MST,
                            SLuong = 1,
                        },
                        DLieu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._12.DLieu
                        {
                            TBao = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._5.TBao
                            {
                                DLTBao = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._5.DLTBao
                                {
                                    PBan = model.TTChung.PBan,
                                    MSo = "01/TB-TNĐT",
                                    Ten = "Về việc tiếp nhận tờ khai đăng ký/thay đổi thông tin sử dụng HDDT",
                                    DDanh = "Hà Nội",
                                    TCQT = "TCT",
                                    TCQTCTren = "TCT",
                                    MST = model.TTChung.MST,
                                    TNNT = model.DLieu.TKhai.DLTKhai.TTChung.TNNT,
                                    Ngay = DateTime.Now.ToString("yyyy-MM-dd"),
                                    LUNhiem = model.DLieu.TKhai.DLTKhai.TTChung.LDKUNhiem,
                                    MGDDTu = RandomString(46),
                                    TGNhan = DateTime.Now.ToString("yyyy-MM-dd"),
                                    HThuc = "Chữ ký số",
                                    CDanh = "Giám đốc",
                                    DSTTUNhiem = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._5.DSTTUNhiem
                                    {
                                    }
                                },
                                STBao = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._5.STBao
                                {
                                    So = RandomString(30),
                                    NTBao = DateTime.Now.ToString("yyyy-MM-dd")
                                },
                                DSCKS = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._4.DSCKS()
                            }
                        }
                    };

                    //convert to xml
                    var xmlPhanHoi = _xmlInvoiceService.CreateFileXML(tBaoTiepNhan, "QuyDinhKyThuatHDDT.PhanII.I._12");
                    return true;
                }
                else
                {
                    var tBaoTiepNhan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._12.TDiep
                    {
                        TTChung = new ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities.TTChungThongDiep
                        {
                            PBan = model.TTChung.PBan,
                            MNGui = "TCT",
                            MNNhan = model.TTChung.MNGui,
                            MLTDiep = "101",
                            MTDiep = $"TCT{Guid.NewGuid().ToString().Replace("-", "").ToUpper()}",
                            MTDTChieu = model.TTChung.MTDiep,
                            MST = model.TTChung.MST,
                            SLuong = 1,
                        },
                        DLieu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._12.DLieu
                        {
                            TBao = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._5.TBao
                            {
                                DLTBao = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._5.DLTBao
                                {
                                    PBan = model.TTChung.PBan,
                                    MSo = "01/TB-TNĐT",
                                    Ten = "Về việc tiếp nhận tờ khai đăng ký/thay đổi thông tin sử dụng HDDT",
                                    DDanh = "Hà Nội",
                                    TCQT = "TCT",
                                    TCQTCTren = "TCT",
                                    MST = model.TTChung.MST,
                                    TNNT = model.DLieu.TKhai.DLTKhai.TTChung.TNNT,
                                    Ngay = DateTime.Now.ToString("yyyy-MM-dd"),
                                    LUNhiem = model.DLieu.TKhai.DLTKhai.TTChung.LDKUNhiem,
                                    MGDDTu = RandomString(46),
                                    TGNhan = DateTime.Now.ToString("yyyy-MM-dd"),
                                    HThuc = "Chữ ký số",
                                    CDanh = "Giám đốc",
                                    DSTTUNhiem = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._5.DSTTUNhiem
                                    {
                                        TTUNhiem = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._5.TTUNhiem>{
                                            new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._5.TTUNhiem
                                            {
                                                DSLDKCNhan = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._5.DSLDKCNhan
                                                {
                                                    LDo = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._5.LDo>
                                                    {
                                                        new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._5.LDo
                                                        {
                                                            MLoi = "0001",
                                                            MTa = "Không đọc được dữ liệu gửi"
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                },
                                STBao = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._5.STBao
                                {
                                    So = RandomString(30),
                                    NTBao = DateTime.Now.ToString("yyyy-MM-dd")
                                },
                                DSCKS = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._4.DSCKS()
                            }
                        }
                    };

                    //convert to xml
                    var xmlPhanHoi = _xmlInvoiceService.CreateFileXML(tBaoTiepNhan, "QuyDinhKyThuatHDDT.PhanII.I._12");
                    return false;
                }
            }

        }

        public async Task<bool> XoaToKhai(string Id)
        {
            var duLieuKys = await _dataContext.DuLieuKyToKhais.Where(x => x.IdToKhai == Id).ToListAsync();
            _dataContext.DuLieuKyToKhais.RemoveRange(duLieuKys);
            var duLieuGuis = await _dataContext.TrangThaiGuiToKhais.Where(x => x.IdToKhai == Id).ToListAsync();
            _dataContext.TrangThaiGuiToKhais.RemoveRange(duLieuGuis);
            var entity = await _dataContext.ToKhaiDangKyThongTins.FirstOrDefaultAsync(x => x.Id == Id);
            _dataContext.ToKhaiDangKyThongTins.Remove(entity);
            return await _dataContext.SaveChangesAsync() == duLieuGuis.Count + duLieuKys.Count + 1;
        }

        public async Task<PagedList<ToKhaiDangKyThongTinViewModel>> GetPagingAsync(PagingParams @params)
        {
            IQueryable<ToKhaiDangKyThongTinViewModel> query = from tk in _dataContext.ToKhaiDangKyThongTins
                        join dlKy in _dataContext.DuLieuKyToKhais on tk.Id equals dlKy.IdToKhai into tmpDLKy
                        from dlKy in tmpDLKy.DefaultIfEmpty()
                        join ttGui in _dataContext.TrangThaiGuiToKhais on tk.Id equals ttGui.IdToKhai into tmpTTGui
                        from ttGui in tmpTTGui.DefaultIfEmpty()
                        select new ToKhaiDangKyThongTinViewModel
                        {
                            Id = tk.Id,
                            NgayTao = tk.NgayTao,
                            NhanUyNhiem = tk.NhanUyNhiem,
                            LoaiUyNhiem = tk.NhanUyNhiem ? tk.LoaiUyNhiem : null,
                            SignedStatus = tk.SignedStatus,
                            NgayKy = dlKy != null ? dlKy.NgayKy : null,
                            NgayGui = ttGui != null ? ttGui.NgayGioGui : null,
                            TrangThaiGui = ttGui != null ? ttGui.TrangThaiGui.GetDescription() : string.Empty,
                            TrangThaiTiepNhan = ttGui != null ? ttGui.TrangThaiTiepNhan.GetDescription() : string.Empty
                        };

            query = query.GroupBy(x => new { x.Id })
                        .Select(x => new ToKhaiDangKyThongTinViewModel
                        {
                            Id = x.Key.Id,
                            NgayTao = x.First().NgayTao,
                            NhanUyNhiem = x.First().NhanUyNhiem,
                            LoaiUyNhiem = x.First().LoaiUyNhiem,
                            SignedStatus = x.First().SignedStatus,
                            NgayKy = x.OrderByDescending(y => y.NgayKy).Select(z => z.NgayKy).FirstOrDefault(),
                            NgayGui = x.OrderByDescending(y => y.NgayGui).Select(z => z.NgayGui).FirstOrDefault(),
                            TrangThaiGui = x.OrderByDescending(y => y.NgayGui).Select(z => z.TrangThaiGui).FirstOrDefault(),
                            TrangThaiTiepNhan = x.OrderByDescending(y => y.NgayGui).Select(z => z.TrangThaiTiepNhan).FirstOrDefault(),
                        });

            var _list = await query.ToListAsync();

            if(!string.IsNullOrEmpty(@params.FromDate) && !string.IsNullOrEmpty(@params.ToDate))
            {
                DateTime fromDate = DateTime.ParseExact(@params.FromDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                DateTime toDate = DateTime.ParseExact(@params.ToDate + " 23:59:59", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

                query = query.Where(x => x.NgayTao >= fromDate &&
                                        x.NgayTao <= toDate);
            }

            return await PagedList<ToKhaiDangKyThongTinViewModel>
                .CreateAsync(query, @params.PageNumber, @params.PageSize);
        }

        public async Task<ToKhaiDangKyThongTinViewModel> GetToKhaiById(string Id)
        {
            var query = from tk in _dataContext.ToKhaiDangKyThongTins
                        join dlKy in _dataContext.DuLieuKyToKhais on tk.Id equals dlKy.IdToKhai into tmpDLKy
                        from dlKy in tmpDLKy.DefaultIfEmpty()
                        join ttGui in _dataContext.TrangThaiGuiToKhais on tk.Id equals ttGui.IdToKhai into tmpTTGui
                        from ttGui in tmpTTGui.DefaultIfEmpty()
                        where tk.Id == Id
                        select new ToKhaiDangKyThongTinViewModel
                        {
                            Id = tk.Id,
                            NgayTao = tk.NgayTao,
                            IsThemMoi = tk.IsThemMoi,
                            FileXMLChuaKy = tk.FileXMLChuaKy,
                            ToKhaiKhongUyNhiem = tk.NhanUyNhiem ? null : DataHelper.ConvertObjectFromTKhai<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1.TKhai>(tk, _hostingEnvironment.WebRootPath),
                            ToKhaiUyNhiem = !tk.NhanUyNhiem ? null : DataHelper.ConvertObjectFromTKhai<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._2.TKhai>(tk, _hostingEnvironment.WebRootPath),
                            NhanUyNhiem = tk.NhanUyNhiem,
                            LoaiUyNhiem = tk.LoaiUyNhiem,
                            SignedStatus = tk.SignedStatus,
                            NgayKy = dlKy != null ? dlKy.NgayKy : null,
                            NgayGui = ttGui != null ? ttGui.NgayGioGui : null,
                            TrangThaiGui = ttGui != null ? ttGui.TrangThaiGui.GetDescription() : string.Empty,
                            TrangThaiTiepNhan = ttGui != null ? ttGui.TrangThaiTiepNhan.GetDescription() : string.Empty
                        };

            query = query.GroupBy(x => new { x.Id })
                        .Select(x => new ToKhaiDangKyThongTinViewModel
                        {
                            Id = x.Key.Id,
                            NgayTao = x.First().NgayTao,
                            IsThemMoi = x.First().IsThemMoi,
                            NhanUyNhiem = x.First().NhanUyNhiem,
                            LoaiUyNhiem = x.First().LoaiUyNhiem,
                            SignedStatus = x.First().SignedStatus,
                            FileXMLChuaKy = x.First().FileXMLChuaKy,
                            ToKhaiKhongUyNhiem = x.First().ToKhaiKhongUyNhiem,
                            ToKhaiUyNhiem = x.First().ToKhaiUyNhiem,
                            NgayKy = x.OrderByDescending(y => y.NgayKy).Select(z => z.NgayKy).FirstOrDefault(),
                            NgayGui = x.OrderByDescending(y => y.NgayGui).Select(z => z.NgayGui).FirstOrDefault(),
                            TrangThaiGui = x.OrderByDescending(y => y.NgayGui).Select(z => z.TrangThaiGui).FirstOrDefault(),
                            TrangThaiTiepNhan = x.OrderByDescending(y => y.NgayGui).Select(z => z.TrangThaiTiepNhan).FirstOrDefault(),
                        });

            var data = await query.FirstOrDefaultAsync();
            return await query.FirstOrDefaultAsync();
        }

        private string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }
}
