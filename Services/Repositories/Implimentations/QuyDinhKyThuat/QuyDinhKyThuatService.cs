using AutoMapper;
using DLL;
using DLL.Entity.QuyDinhKyThuat;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Services.Helper;
using Services.Helper.XmlModel;
using Services.Repositories.Interfaces;
using Services.Repositories.Interfaces.QuyDinhKyThuat;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.Params;
using Services.ViewModels.QuyDinhKyThuat;
using Services.ViewModels.XML;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using System;
using System.Collections.Generic;
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

        private readonly List<LoaiThongDiep> TreeThongDiepNhan = new List<LoaiThongDiep>()
        {
            new LoaiThongDiep(){ LoaiThongDiepId = -1, Ten = "Tất cả", LoaiThongDiepChaId = null, Level = 0, IsParent = true },
            new LoaiThongDiep(){ LoaiThongDiepId = 0, Ten = "Nhóm thông điệp đáp ứng nghiệp vụ đăng ký, thay đổi thông tin sử dụng hóa đơn điện tử, đề nghị cấp hóa đơn điện tử có mã theo từng lần phát sinh", LoaiThongDiepChaId = null, Level = 0, IsParent = true },
            new LoaiThongDiep(){ LoaiThongDiepId = 1, MaLoaiThongDiep = 102, Ten = "102 - Thông điệp thông báo về việc tiếp nhận/không tiếp nhận tờ khai đăng ký/thay đổi thông tin sử dụng HĐĐT, tờ khai đăng ký thay đổi thông tin đăng ký sử dụng HĐĐT khi ủy nhiệm/nhận ủy nhiệm lập hóa đơn", LoaiThongDiepChaId = 0, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 2, MaLoaiThongDiep = 103, Ten = "103 - Thông điệp thông báo về việc chấp nhận/không chấp nhận đăng ký/thay đổi thông tin sử dụng hóa đơn điện tử", LoaiThongDiepChaId = 0, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 3, MaLoaiThongDiep = 104, Ten = "104 - Thông điệp thông báo về việc chấp nhận/không chấp nhận đăng ký thay đổi thông tin đăng ký sử dụng HĐĐT khi ủy nhiệm/nhận ủy nhiệm lập hóa đơn", LoaiThongDiepChaId = 0, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 4, MaLoaiThongDiep = 105, Ten = "105 - Thông điệp thông báo về việc hết thời gian sử dụng hóa đơn điện tử có mã qua cổng thông tin điện tử Tổng cục Thuế/qua ủy thác tổ chức cung cấp dịch vụ về hóa đơn điện tử; không thuộc trường hợp sử dụng hóa đơn điện tử không có mã", LoaiThongDiepChaId = 0, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 5, Ten = "Nhóm thông điệp đáp ứng nghiệp vụ lập và gửi hóa đơn điện tử đến cơ quan thuế", LoaiThongDiepChaId = null, Level = 0, IsParent = true },
            new LoaiThongDiep(){ LoaiThongDiepId = 6, MaLoaiThongDiep = 202, Ten = "202 - Thông điệp thông báo kết quả cấp mã hóa đơn điện tử của cơ quan thuế", LoaiThongDiepChaId = 5, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 7, MaLoaiThongDiep = 204, Ten = "204 - Thông điệp thông báo mẫu số 01/TB-KTDL về việc kết quả kiểm tra dữ liệu hóa đơn điện tử", LoaiThongDiepChaId = 5, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 8, MaLoaiThongDiep = 205, Ten = "205 - Thông điệp phản hồi về hồ sơ đề nghị cấp hóa đơn điện tử có mã của cơ quan thuế theo từng lần pháp sinh", LoaiThongDiepChaId = 5, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 9, Ten = "Nhóm thông điệp đáp ứng nghiệp vụ xử lý hóa đơn có sai sót", LoaiThongDiepChaId = null, Level = 0, IsParent = true },
            new LoaiThongDiep(){ LoaiThongDiepId = 10, MaLoaiThongDiep = 301, Ten = "301 - Thông điệp gửi thông báo về việc tiếp nhận và kết quả xử lý về việc hóa đơn điện tử đã lập có sai sót", LoaiThongDiepChaId = 9, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 11, MaLoaiThongDiep = 302, Ten = "302 - Thông điệp thông báo về hóa đơn điện tử cần rà soát", LoaiThongDiepChaId = 9, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 12, Ten = "Nhóm thông điệp khác", LoaiThongDiepChaId = null, Level = 0, IsParent = true },
            new LoaiThongDiep(){ LoaiThongDiepId = 13, MaLoaiThongDiep = 999, Ten = "999 - Thông điệp phản hồi kỹ thuật", LoaiThongDiepChaId = 12, Level = 1 },
        };

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

        public async Task<ToKhaiDangKyThongTinViewModel> LuuToKhaiDangKyThongTin(ToKhaiDangKyThongTinViewModel tKhai)
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
            if (await _dataContext.SaveChangesAsync() > 0)
            {
                return _mp.Map<ToKhaiDangKyThongTinViewModel>(_entity);
            }
            else return null;
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

        public async Task<bool> GuiToKhai(string XMLUrl, string idThongDiep, string maThongDiep, string mst)
        {
            var entity = await _dataContext.ThongDiepChungs.FirstOrDefaultAsync(x => x.ThongDiepChungId == idThongDiep);
            var entityTK = await _dataContext.ToKhaiDangKyThongTins.FirstOrDefaultAsync(x => x.Id == entity.IdThamChieu);
            var assetsFolder = !entityTK.NhanUyNhiem ? $"FilesUpload/QuyDinhKyThuat/QuyDinhKyThuatHDDT_PhanII_I_8/unsigned" : $"FilesUpload/QuyDinhKyThuat/QuyDinhKyThuatHDDT_PhanII_I_9/unsigned";
            var fullXmlFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);
            var data = new GuiThongDiepData
            {
                MST = "010920560816",
                MTDiep = "V010920560816d402b13a4849d4a8daee185f7df6fd",
                DataXML = Path.Combine(fullXmlFolder, XMLUrl).EncodeFile()
            };
            TextHelper.SendViaSocketConvert("192.168.2.86", 35000, JsonConvert.SerializeObject(data).EncodeString());
            return true;
        }


        public async Task<string> NhanPhanHoiCQT(string fileXML, string idTKhai)
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
            string xmlPhanHoi = string.Empty;
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
                    xmlPhanHoi = _xmlInvoiceService.CreateFileXML(tBaoTiepNhan, "QuyDinhKyThuatHDDT.PhanII_I_11");
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
                    xmlPhanHoi = _xmlInvoiceService.CreateFileXML(tBaoTiepNhan, "QuyDinhKyThuatHDDT.PhanII_I_11");
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
                    xmlPhanHoi = _xmlInvoiceService.CreateFileXML(tBaoTiepNhan, "QuyDinhKyThuatHDDT.PhanII_I_12");
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
                    xmlPhanHoi = _xmlInvoiceService.CreateFileXML(tBaoTiepNhan, "QuyDinhKyThuatHDDT.PhanII_I_12");
                }
            }

            if (!string.IsNullOrEmpty(xmlPhanHoi))
            {
                var fullXmlPath = !entityTK.NhanUyNhiem ? $"FilesUpload/QuyDinhKyThuat/QuyDinhKyThuatHDDT_PhanII_I_11/unsigned/{xmlPhanHoi}" : $"FilesUpload/QuyDinhKyThuat/QuyDinhKyThuatHDDT_PhanII_I_12/unsigned/{xmlPhanHoi}";
                string xmlContent = File.ReadAllText(fullXmlPath);
                var bytesContent = Encoding.UTF8.GetBytes(xmlContent);
                return Convert.ToBase64String(bytesContent);
            }
            else return null;
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
                                                                  NgayKy = dlKy == null ? null : dlKy.NgayKy,
                                                                  NgayGui = ttGui == null ? null : ttGui.NgayGioGui,
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

            if (!string.IsNullOrEmpty(@params.FromDate) && !string.IsNullOrEmpty(@params.ToDate))
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

        public async Task<PagedList<ThongDiepChungViewModel>> GetPagingThongDiepChungAsync(ThongDiepChungParams @params)
        {
            IQueryable<ThongDiepChungViewModel> queryToKhai = from tdc in _dataContext.ThongDiepChungs
                                                              join tk in _dataContext.ToKhaiDangKyThongTins on tdc.IdThamChieu equals tk.Id into tmpToKhai
                                                              from tk in tmpToKhai.DefaultIfEmpty()
                                                              join dlk in _dataContext.DuLieuKyToKhais on tk.Id equals dlk.IdToKhai into tmpDuLieuKy
                                                              from dlk in tmpDuLieuKy.DefaultIfEmpty()
                                                              join ttg in _dataContext.TrangThaiGuiToKhais on tk.Id equals ttg.IdToKhai into tmpTrangThaiGui
                                                              from ttg in tmpTrangThaiGui.DefaultIfEmpty()
                                                              where tdc.ThongDiepGuiDi == true
                                                              select new ThongDiepChungViewModel
                                                              {
                                                                  ThongDiepChungId = tdc.ThongDiepChungId,
                                                                  PhienBan = tdc.PhienBan,
                                                                  MaNoiGui = tdc.MaNoiGui,
                                                                  MaNoiNhan = tdc.MaNoiNhan,
                                                                  MaLoaiThongDiep = tdc.MaLoaiThongDiep,
                                                                  MaThongDiep = tdc.MaThongDiep,
                                                                  MaThongDiepThamChieu = tdc.MaThongDiepThamChieu,
                                                                  MaSoThue = tdc.MaSoThue,
                                                                  SoLuong = tdc.SoLuong,
                                                                  TenLoaiThongDiep = ((MLTDiep)tdc.MaLoaiThongDiep).GetDescription(),
                                                                  ThongDiepGuiDi = tdc.ThongDiepGuiDi,
                                                                  HinhThuc = tdc.HinhThuc,
                                                                  TenHinhThuc = ((HThuc)tdc.HinhThuc).GetDescription(),
                                                                  //TrangThaiGui = ttg.TrangThaiGui,
                                                                  //TenTrangThaiThongBao = ttg.TrangThaiGui.GetDescription(),
                                                                  //TrangThaiTiepNhan = ttg.TrangThaiTiepNhan,
                                                                  //TenTrangThaiXacNhanCQT = ttg.TrangThaiTiepNhan.GetDescription(),
                                                                  NgayGui = tdc.NgayGui,
                                                                  // TaiLieuDinhKem = _mp.Map<List<TaiLieuDinhKemViewModel>>(_dataContext.TaiLieuDinhKems.Where(x => x.NghiepVuId == ttg.Id).ToList()),
                                                                  // IdThongDiepGoc = ttg.Id,
                                                                  IdThamChieu = tk.Id,
                                                                  CreatedDate = tdc.CreatedDate,
                                                                  ModifyDate = tdc.ModifyDate
                                                              };

            IQueryable<ThongDiepChungViewModel> query = queryToKhai;

            if (!string.IsNullOrEmpty(@params.FromDate) && !string.IsNullOrEmpty(@params.ToDate))
            {
                DateTime fromDate = DateTime.ParseExact(@params.FromDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                DateTime toDate = DateTime.ParseExact(@params.ToDate + " 23:59:59", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

                query = query.Where(x => (@params.IsThongDiepGui == true ? (x.NgayGui.HasValue ? x.NgayGui : x.CreatedDate) : x.NgayThongBao) >= fromDate &&
                                        (@params.IsThongDiepGui == true ? (x.NgayGui.HasValue ? x.NgayGui : x.CreatedDate) : x.NgayThongBao) <= toDate);
            }

            // thông điệp nhận
            if (@params.IsThongDiepGui != true)
            {
                var maLoaiThongDieps = TreeThongDiepNhan.Where(x => x.MaLoaiThongDiep.HasValue).Select(x => x.MaLoaiThongDiep).ToList();
                query = query.Where(x => maLoaiThongDieps.Contains(x.MaLoaiThongDiep));

                if (@params.LoaiThongDiep != -1)
                {
                    var loaiThongDiep = TreeThongDiepNhan.FirstOrDefault(x => x.LoaiThongDiepId == @params.LoaiThongDiep);
                    if (loaiThongDiep.IsParent == true)
                    {
                        maLoaiThongDieps = TreeThongDiepNhan.Where(x => x.LoaiThongDiepChaId == @params.LoaiThongDiep).Select(x => x.MaLoaiThongDiep).ToList();
                        query = query.Where(x => maLoaiThongDieps.Contains(x.MaLoaiThongDiep));
                    }
                    else
                    {
                        query = query.Where(x => x.MaLoaiThongDiep == loaiThongDiep.MaLoaiThongDiep);
                    }

                }
            }

            query = query.OrderByDescending(x => x.CreatedDate);
            var list = await query.ToListAsync();

            return await PagedList<ThongDiepChungViewModel>
                .CreateAsync(query, @params.PageNumber, @params.PageSize);
        }

        public async Task<bool> InsertThongDiepChung(ThongDiepChungViewModel model)
        {
            var _entity = _mp.Map<ThongDiepChung>(model);
            _entity.ThongDiepChungId = Guid.NewGuid().ToString();
            _entity.CreatedDate = DateTime.Now;
            _entity.ModifyDate = DateTime.Now;
            await _dataContext.ThongDiepChungs.AddAsync(_entity);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateThongDiepChung(ThongDiepChungViewModel model)
        {
            var _entity = _mp.Map<ThongDiepChung>(model);
            _entity.ModifyDate = DateTime.Now;
            _dataContext.Update(_entity);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteThongDiepChung(string Id)
        {
            var entity = await _dataContext.ThongDiepChungs.FirstOrDefaultAsync(x => x.ThongDiepChungId == Id);
            _dataContext.ThongDiepChungs.Remove(entity);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<ThongDiepChungViewModel> GetThongDiepChungById(string Id)
        {
            IQueryable<ThongDiepChungViewModel> queryToKhai = from tdc in _dataContext.ThongDiepChungs
                                                              join tk in _dataContext.ToKhaiDangKyThongTins on tdc.IdThamChieu equals tk.Id into tmpToKhai
                                                              from tk in tmpToKhai.DefaultIfEmpty()
                                                              join dlk in _dataContext.DuLieuKyToKhais on tk.Id equals dlk.IdToKhai into tmpDuLieuKy
                                                              from dlk in tmpDuLieuKy.DefaultIfEmpty()
                                                              join ttg in _dataContext.TrangThaiGuiToKhais on tk.Id equals ttg.IdToKhai into tmpTrangThaiGui
                                                              from ttg in tmpTrangThaiGui.DefaultIfEmpty()
                                                              select new ThongDiepChungViewModel
                                                              {
                                                                  ThongDiepChungId = tdc.ThongDiepChungId,
                                                                  MaLoaiThongDiep = tdc.MaLoaiThongDiep,
                                                                  MaThongDiep = tdc.MaThongDiep,
                                                                  TenLoaiThongDiep = ((MLTDiep)tdc.MaLoaiThongDiep).GetDescription(),
                                                                  MaNoiGui = tdc.MaNoiGui,
                                                                  MaNoiNhan = tdc.MaNoiNhan,
                                                                  MaThongDiepThamChieu = tdc.MaThongDiepThamChieu,
                                                                  MaSoThue = tdc.MaSoThue,
                                                                  ThongDiepGuiDi = tdc.ThongDiepGuiDi,
                                                                  HinhThuc = tdc.HinhThuc,
                                                                  TenHinhThuc = ((HThuc)tdc.HinhThuc).GetDescription(),
                                                                  TrangThaiGui = (TrangThaiGuiToKhaiDenCQT)tdc.TrangThaiGui,
                                                                  TenTrangThaiThongBao = ((TrangThaiGuiToKhaiDenCQT)tdc.TrangThaiGui).GetDescription(),
                                                                  TrangThaiTiepNhan = (TrangThaiTiepNhanCuaCoQuanThue)tdc.TrangThaiTiepNhan,
                                                                  TenTrangThaiXacNhanCQT = ((TrangThaiTiepNhanCuaCoQuanThue)tdc.TrangThaiTiepNhan).GetDescription(),
                                                                  NgayGui = tdc.NgayGui ?? null,
                                                                  IdThongDiepGoc = tdc.IdThongDiepGoc,
                                                                  IdThamChieu = tk.Id,
                                                                  CreatedDate = tdc.CreatedDate,
                                                                  ModifyDate = tdc.ModifyDate
                                                              };

            IQueryable<ThongDiepChungViewModel> query = queryToKhai;
            return await query.FirstOrDefaultAsync(x => x.ThongDiepChungId == Id);
        }

        public async Task<List<ThongDiepChungViewModel>> GetAllThongDiepTraVe(string ThongDiepGocId)
        {
            return _mp.Map<List<ThongDiepChungViewModel>>(await _dataContext.ThongDiepChungs.Where(x => x.ThongDiepGuiDi == false && x.IdThongDiepGoc == ThongDiepGocId).ToListAsync());
        }

        public async Task<int> GetLanThuMax(int MaLoaiThongDiep)
        {
            var result = await _dataContext.ThongDiepChungs.Where(x => x.MaLoaiThongDiep == MaLoaiThongDiep && x.HinhThuc == (int)HThuc.ThayDoiThongTin).OrderByDescending(x => x.CreatedDate).Select(x => x.LanThu).FirstOrDefaultAsync();
            if (result == null) result = 0;
            return result;
        }

        public async Task<int> GetLanGuiMax(ThongDiepChungViewModel td)
        {
            var result = await _dataContext.ThongDiepChungs.Where(x => x.MaLoaiThongDiep == td.MaLoaiThongDiep && x.HinhThuc == td.HinhThuc && x.LanThu == td.LanThu).OrderByDescending(x => x.CreatedDate).Select(x => x.LanThu).FirstOrDefaultAsync();
            if (result == null) result = 0;
            return result;
        }

        public async Task<ThongDiepChungViewModel> GetThongDiepByThamChieu(string ThamChieuId)
        {
            return _mp.Map<ThongDiepChungViewModel>(await _dataContext.ThongDiepChungs.FirstOrDefaultAsync(x => x.IdThamChieu == ThamChieuId));
        }

        public ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._10.TDiep ConvertToThongDiepTiepNhan(string encodedContent)
        {
            return DataHelper.ConvertObjectFromStringContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._10.TDiep>(encodedContent);
        }

        public ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._11.TDiep ConvertToThongDiepKUNCQT(string encodedContent)
        {
            return DataHelper.ConvertObjectFromStringContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._11.TDiep>(encodedContent);
        }

        public ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._12.TDiep ConvertToThongDiepUNCQT(string encodedContent)
        {
            return DataHelper.ConvertObjectFromStringContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._12.TDiep>(encodedContent);
        }

        public async Task<bool> ThongDiepDaGui(ThongDiepChungViewModel td)
        {
            return (!string.IsNullOrEmpty(td.MaThongDiep) || await _dataContext.ThongDiepChungs.AnyAsync(x => x.IdThongDiepGoc == td.ThongDiepChungId && !string.IsNullOrEmpty(x.MaThongDiep)));
        }

        private string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        public List<LoaiThongDiep> GetListLoaiThongDiepNhan()
        {
            return TreeThongDiepNhan;
        }
    }
}
