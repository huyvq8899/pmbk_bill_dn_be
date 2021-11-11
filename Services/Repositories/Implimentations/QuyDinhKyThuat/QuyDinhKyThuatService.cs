using AutoMapper;
using DLL;
using DLL.Constants;
using DLL.Entity.QuyDinhKyThuat;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using OfficeOpenXml;
using Services.Helper;
using Services.Helper.Params.Filter;
using Services.Helper.Params.HoaDon;
using Services.Helper.Params.QuyDinhKyThuat;
using Services.Helper.XmlModel;
using Services.Repositories.Interfaces;
using Services.Repositories.Interfaces.DanhMuc;
using Services.Repositories.Interfaces.QuyDinhKyThuat;
using Services.ViewModels.Params;
using Services.ViewModels.QuyDinhKyThuat;
using Services.ViewModels.XML;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
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
        private readonly IMapper _mp;
        private readonly IXMLInvoiceService _xmlInvoiceService;
        private readonly IHoSoHDDTService _hoSoHDDTService;

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

        private readonly List<LoaiThongDiep> TreeThongDiepGui = new List<LoaiThongDiep>()
        {
            new LoaiThongDiep(){ LoaiThongDiepId = -1, MaLoaiThongDiep = -1, Ten = "Tất cả", LoaiThongDiepChaId = null, Level = 0, IsParent = true },
            new LoaiThongDiep(){ LoaiThongDiepId = 0, Ten = "Nhóm thông điệp đáp ứng nghiệp vụ đăng ký, thay đổi thông tin sử dụng hóa đơn điện tử, đề nghị cấp hóa đơn điện tử có mã theo từng lần phát sinh", LoaiThongDiepChaId = null, Level = 0, IsParent = true },
            new LoaiThongDiep(){ LoaiThongDiepId = 1, MaLoaiThongDiep = 100, Ten = "100 - Thông điệp gửi tờ khai đăng ký/thay đổi thông tin sử dụng hóa đơn điện tử", LoaiThongDiepChaId = 0, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 2, MaLoaiThongDiep = 101, Ten = "101 - Thông điệp gửi tờ khai đăng ký thay đổi thông tin đăng ký sử dụng HĐĐT khi ủy nhiệm/nhận ủy nhiệm lập hóa đơn", LoaiThongDiepChaId = 0, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 3, MaLoaiThongDiep = 106, Ten = "106 - Thông điệp gửi Đơn đề nghị cấp hóa đơn điện tử có mã của CQT theo từng lần phát sinh", LoaiThongDiepChaId = 0, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 4, Ten = "Nhóm thông điệp đáp ứng nghiệp vụ lập và gửi hóa đơn điện tử đến cơ quan thuế", LoaiThongDiepChaId = null, Level = 0, IsParent = true },
            new LoaiThongDiep(){ LoaiThongDiepId = 5, MaLoaiThongDiep = 200, Ten = "200 - Thông điệp gửi hóa đơn điện tử tới cơ quan thuế để cấp mã", LoaiThongDiepChaId = 4, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 6, MaLoaiThongDiep = 201, Ten = "201 - Thông điệp gửi hóa đơn điện tử tới cơ quan thuế để cấp mã theo từng lần phát sinh", LoaiThongDiepChaId = 4, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 7, MaLoaiThongDiep = 203, Ten = "203 - Thông điệp chuyển dữ liệu hóa đơn điện tử không mã đến cơ quan thuế", LoaiThongDiepChaId = 4, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 8, Ten = "Nhóm thông điệp đáp ứng nghiệp vụ xử lý hóa đơn có sai sót", LoaiThongDiepChaId = null, Level = 0, IsParent = true },
            new LoaiThongDiep(){ LoaiThongDiepId = 9, MaLoaiThongDiep = 300, Ten = "300 - Thông điệp thông báo về hóa đơn điện tử đã lập có sai sót", LoaiThongDiepChaId = 8, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 10, Ten = "Nhóm thông điệp chuyển bảng tổng hợp dữ liệu hóa đơn điện tử đến cơ quan thuế", LoaiThongDiepChaId = null, Level = 0, IsParent = true },
            new LoaiThongDiep(){ LoaiThongDiepId = 11, MaLoaiThongDiep = 400, Ten = "400 - Thông điệp chuyển bảng tổng hợp dữ liệu hóa đơn điện tử đến cơ quan thuế", LoaiThongDiepChaId = 9, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 12, Ten = "Nhóm thông điệp chuyển dữ liệu hóa đơn điện tử do TCTN ủy quyền cấp mã đến cơ quan thuế", LoaiThongDiepChaId = null, Level = 0, IsParent = true },
            new LoaiThongDiep(){ LoaiThongDiepId = 13, MaLoaiThongDiep = 500, Ten = "500 - Thông điệp chuyển dữ liệu hóa đơn điện tử do TCTN ủy quyền cấp mã đến cơ quan thuế", LoaiThongDiepChaId = 12, Level = 1 },
            new LoaiThongDiep(){ LoaiThongDiepId = 14, Ten = "Nhóm thông điệp khác", LoaiThongDiepChaId = null, Level = 0, IsParent = true },
            new LoaiThongDiep(){ LoaiThongDiepId = 15, MaLoaiThongDiep = 999, Ten = "999 - Thông điệp phản hồi kỹ thuật", LoaiThongDiepChaId = 14, Level = 1 },
        };

        public QuyDinhKyThuatService(
            Datacontext dataContext,
            IHttpContextAccessor httpContextAccessor,
            IHostingEnvironment hostingEnvironment,
            IMapper mp,
            IXMLInvoiceService xmlInvoiceService,
            IHoSoHDDTService hoSoHDDTService)
        {
            _dataContext = dataContext;
            _random = new Random();
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _mp = mp;
            _xmlInvoiceService = xmlInvoiceService;
            _hoSoHDDTService = hoSoHDDTService;
        }

        public async Task<ToKhaiDangKyThongTinViewModel> LuuToKhaiDangKyThongTin(ToKhaiDangKyThongTinViewModel tKhai)
        {
            var _entity = _mp.Map<ToKhaiDangKyThongTin>(tKhai);
            string folderName = tKhai.NhanUyNhiem == true ? "QuyDinhKyThuatHDDT_PhanII_I_2" : "QuyDinhKyThuatHDDT_PhanII_I_1";
            string assetsFolder = $"FilesUpload/QuyDinhKyThuat/{folderName}/unsigned";
            var fullXmlFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);
            var fullXmlName = Path.Combine(fullXmlFolder, tKhai.FileXMLChuaKy);
            //string xmlDeCode = DataHelper.Base64Decode(fullXmlName);
            byte[] byteXML = File.ReadAllBytes(fullXmlName);
            _entity.ContentXMLChuaKy = byteXML;
            _entity.Id = Guid.NewGuid().ToString();
            _entity.NgayTao = DateTime.Now;
            _entity.ModifyDate = DateTime.Now;
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
            var _entityTDiep = _mp.Map<ThongDiepChungViewModel>(await _dataContext.ThongDiepChungs.FirstOrDefaultAsync(x => x.IdThamChieu == kTKhai.IdToKhai));
            _entity.Id = Guid.NewGuid().ToString();
            _entity.NgayKy = DateTime.Now;
            //string xmlDeCode = DataHelper.Base64Decode(kTKhai.NoiDungKy);
            var base64EncodedBytes = System.Convert.FromBase64String(kTKhai.Content);
            byte[] byteXML = Encoding.UTF8.GetBytes(kTKhai.Content);
            _entity.NoiDungKy = byteXML;
            var _entityTK = await _dataContext.ToKhaiDangKyThongTins.FirstOrDefaultAsync(x => x.Id == kTKhai.IdToKhai);
            var fileName = Guid.NewGuid().ToString() + ".xml";
            var databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.ThongDiepToKhai);
            string assetsFolder = $"FilesUpload/{databaseName}/{loaiNghiepVu}/{_entityTDiep.ThongDiepChungId}/xml/signed";
            var fullFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);
            if (!Directory.Exists(fullFolder))
            {
                Directory.CreateDirectory(fullFolder);
            }

            var fullXMLFile = Path.Combine(fullFolder, fileName);
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

        public async Task<string> GetXMLDaKy(string ToKhaiId)
        {
            return await _dataContext.DuLieuKyToKhais.Where(x => x.IdToKhai == ToKhaiId).Select(x => x.FileXMLDaKy).FirstOrDefaultAsync();
        }

        public async Task<ThongDiepChungViewModel> GetThongDiepThemMoiToKhai()
        {
            IQueryable<ThongDiepChungViewModel> query = from tdc in _dataContext.ThongDiepChungs
                                                        join tk in _dataContext.ToKhaiDangKyThongTins on tdc.IdThamChieu equals tk.Id into tmpToKhai
                                                        from tk in tmpToKhai.DefaultIfEmpty()
                                                        join dlk in _dataContext.DuLieuKyToKhais on tk.Id equals dlk.IdToKhai into tmpDuLieuKy
                                                        from dlk in tmpDuLieuKy.DefaultIfEmpty()
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
                                                            NgayGui = tdc.NgayGui ?? null,
                                                            IdThongDiepGoc = tdc.IdThongDiepGoc,
                                                            IdThamChieu = tk.Id,
                                                            CreatedDate = tdc.CreatedDate,
                                                            ModifyDate = tdc.ModifyDate
                                                        };

            return await query.FirstOrDefaultAsync(x => x.MaLoaiThongDiep == 100 && x.HinhThuc == (int)HThuc.DangKyMoi);
        }

        public async Task<bool> GuiToKhai(string XMLUrl, string idThongDiep, string maThongDiep, string mst)
        {
            var entity = await _dataContext.ThongDiepChungs.FirstOrDefaultAsync(x => x.ThongDiepChungId == idThongDiep);
            var entityTK = await _dataContext.ToKhaiDangKyThongTins.FirstOrDefaultAsync(x => x.Id == entity.IdThamChieu);
            var databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.ThongDiepToKhai);
            string assetsFolder = $"FilesUpload/{databaseName}/{loaiNghiepVu}/{idThongDiep}/xml/signed";
            var fullFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);
            string ipAddress = "";
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipAddress = ip.ToString();
                }
            }
            var data = new GuiThongDiepData
            {
                MST = mst,
                MTDiep = maThongDiep,
                DataXML = Path.Combine(fullFolder, XMLUrl).EncodeFile()
            };

            // Send to TVAN
            TVANHelper.TVANSendData("api/invoice/send", data.DataXML);

            // Write log send
            await _dataContext.AddTransferLogSendAsync(
                                    new ThongDiepPhanHoiParams
                                    {
                                        MLTDiep = 100,
                                        MTDiep = data.MTDiep,
                                        MTDTChieu = string.Empty,
                                        DataXML = data.DataXML
                                    });

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
                                    HTDKy = (HThuc)model.DLieu.TKhai.DLTKhai.TTChung.HThuc,
                                    TTXNCQT = TTXNCQT.ChapNhan,
                                    HThuc = "Chữ ký số",
                                    CDanh = "Giám đốc",
                                    DSLDKCNhan = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._3.LDo>()
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
                                    HTDKy = (HThuc)model.DLieu.TKhai.DLTKhai.TTChung.HThuc,
                                    TTXNCQT = TTXNCQT.ChapNhan,
                                    HThuc = "Chữ ký số",
                                    CDanh = "Giám đốc",
                                    DSLDKCNhan = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._3.LDo>
                                    {
                                        new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._3.LDo
                                        {
                                            MLoi = "0001",
                                            MTa = "Không đọc được dữ liệu gửi"
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
                                    //                                    LUNhiem = model.DLieu.TKhai.DLTKhai.TTChung.LDKUNhiem,
                                    MGDDTu = RandomString(46),
                                    TGNhan = DateTime.Now.ToString("yyyy-MM-dd"),
                                    HThuc = "Chữ ký số",
                                    CDanh = "Giám đốc",
                                    DSTTUNhiem = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._5.TTUNhiem>()
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
                                    //                                   LUNhiem = model.DLieu.TKhai.DLTKhai.TTChung.LDKUNhiem,
                                    MGDDTu = RandomString(46),
                                    TGNhan = DateTime.Now.ToString("yyyy-MM-dd"),
                                    HThuc = "Chữ ký số",
                                    CDanh = "Giám đốc",
                                    DSTTUNhiem = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._5.TTUNhiem>
                                    {
                                        new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._5.TTUNhiem
                                        {
                                            DSLDKCNhan = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._3.LDo>
                                            {
                                                new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._3.LDo
                                                {
                                                    MLoi = "0001",
                                                    MTa = "Không đọc được dữ liệu gửi"
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
            if (duLieuKys.Any()) _dataContext.DuLieuKyToKhais.RemoveRange(duLieuKys);
            var entity = await _dataContext.ToKhaiDangKyThongTins.FirstOrDefaultAsync(x => x.Id == Id);
            _dataContext.ToKhaiDangKyThongTins.Remove(entity);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<string> GetLinkFileXml(ThongDiepChungViewModel model, bool signed = false)
        {
            var databaseName = "";
            var loaiNghiepVu = "";
            switch (model.MaLoaiThongDiep)
            {
                case (int)MLTDiep.TDGToKhai:
                    if (!string.IsNullOrEmpty(model.MaThongDiep) && signed == true)
                    {
                        var xmlDaKy = await GetXMLDaKy(model.IdThamChieu);
                        databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                        loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.ThongDiepToKhai);
                        string assetsFolder = $"FilesUpload/{databaseName}/{loaiNghiepVu}/{model.ThongDiepChungId}/xml/signed";
                        return $"{_httpContextAccessor.HttpContext.Request.PathBase}/{assetsFolder}/{xmlDaKy}";
                    }
                    else
                    {
                        var entityTK = _dataContext.ToKhaiDangKyThongTins.FirstOrDefault(x => x.Id == model.IdThamChieu);
                        string assetsFolder = $"FilesUpload//QuyDinhKyThuat/QuyDinhKyThuatHDDT_PhanII_I_1/unsigned/";
                        return $"{_httpContextAccessor.HttpContext.Request.PathBase}/{assetsFolder}/{entityTK.FileXMLChuaKy}";
                    }
                    break;
                case (int)MLTDiep.TDGToKhaiUN:
                    if (!string.IsNullOrEmpty(model.MaThongDiep) && signed == true)
                    {
                        var xmlDaKy = await GetXMLDaKy(model.IdThamChieu);
                        databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                        loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.ThongDiepToKhai);
                        string assetsFolder = $"FilesUpload/{databaseName}/{loaiNghiepVu}/{model.ThongDiepChungId}/xml/signed";
                        return $"{_httpContextAccessor.HttpContext.Request.PathBase}/{assetsFolder}/{xmlDaKy}";
                    }
                    else
                    {
                        var entityTK = _dataContext.ToKhaiDangKyThongTins.FirstOrDefault(x => x.Id == model.IdThamChieu);
                        string assetsFolder = $"FilesUpload//QuyDinhKyThuat/QuyDinhKyThuatHDDT_PhanII_I_2/unsigned/";
                        return $"{_httpContextAccessor.HttpContext.Request.PathBase}/{assetsFolder}/{entityTK.FileXMLChuaKy}";
                    }
                    break;
                case (int)MLTDiep.TDTBHDDLSSot:
                    databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                    loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.ThongDiepGuiNhanCQT);
                    var entityTC = _dataContext.ThongDiepGuiCQTs.FirstOrDefault(x => x.Id == model.IdThamChieu);
                    string folderPath;
                    if (entityTC.DaKyGuiCQT == true && signed == true)
                    {
                        folderPath = $"FilesUpload/{databaseName}/{loaiNghiepVu}/xml/signed/{model.IdThamChieu}/{entityTC.FileXMLDaKy}";
                    }
                    else
                    {
                        var files = entityTC.FileDinhKem.Split(';');
                        var file = files.FirstOrDefault(x => x.Contains(".xml"));
                        folderPath = $"FilesUpload/{databaseName}/{loaiNghiepVu}/xml/unsigned/{model.IdThamChieu}/{file}";
                    }

                    return $"{_httpContextAccessor.HttpContext.Request.PathBase}/{folderPath}";
                    break;
                case (int)MLTDiep.TDGHDDTTCQTCapMa:
                case (int)MLTDiep.TDCDLHDKMDCQThue:
                    return null;
                default:
                    return null;
            }
        }

        public async Task<bool> AddRangeChungThuSo(List<ChungThuSoSuDungViewModel> models)
        {
            var listValidToAdd = models.Where(x => !_dataContext.ChungThuSoSuDungs.Any(o => o.Seri == x.Seri && o.HThuc == x.HThuc)).ToList();
            var listValidToEdit = models.Where(x => _dataContext.ChungThuSoSuDungs.Any(o => o.Seri == x.Seri && o.HThuc == x.HThuc)).ToList();
            var entities = _mp.Map<List<ChungThuSoSuDung>>(listValidToAdd);
            foreach (var item in entities)
            {
                item.Id = Guid.NewGuid().ToString();
            }
            await _dataContext.ChungThuSoSuDungs.AddRangeAsync(entities);
            var entitiesEdit = _mp.Map<List<ChungThuSoSuDung>>(listValidToEdit);
            foreach (var item in entitiesEdit)
            {
                item.Id = _dataContext.ChungThuSoSuDungs.Where(x => x.Seri == item.Seri && x.HThuc == item.HThuc).Select(x => x.Id).FirstOrDefault();
            }
            _dataContext.UpdateRange(entitiesEdit);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<ToKhaiDangKyThongTinViewModel> GetToKhaiById(string Id)
        {
            var query = from tk in _dataContext.ToKhaiDangKyThongTins
                        join dlKy in _dataContext.DuLieuKyToKhais on tk.Id equals dlKy.IdToKhai into tmpDLKy
                        from dlKy in tmpDLKy.DefaultIfEmpty()
                        join tdc in _dataContext.ThongDiepChungs on tk.Id equals tdc.IdThamChieu into tmpTDC
                        from tdc in tmpTDC.DefaultIfEmpty()
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
                            NgayGui = tdc != null ? tdc.NgayGui : null,
                            ModifyDate = tk.ModifyDate
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
                            ModifyDate = x.First().ModifyDate
                        });

            var data = await query.FirstOrDefaultAsync();
            return await query.FirstOrDefaultAsync();
        }

        public async Task<PagedList<ThongDiepChungViewModel>> GetPagingThongDiepChungAsync(ThongDiepChungParams @params)
        {
            try
            {

                IQueryable<ThongDiepChungViewModel> queryToKhai = from tdc in _dataContext.ThongDiepChungs
                                                                  where tdc.ThongDiepGuiDi == @params.IsThongDiepGui
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
                                                                      TenHinhThuc = tdc.HinhThuc.HasValue ? ((HThuc)tdc.HinhThuc).GetDescription() : string.Empty,
                                                                      //TrangThaiGui = ttg.TrangThaiGui,
                                                                      //TenTrangThaiThongBao = ttg.TrangThaiGui.GetDescription(),
                                                                      //TrangThaiTiepNhan = ttg.TrangThaiTiepNhan,
                                                                      //TenTrangThaiXacNhanCQT = ttg.TrangThaiTiepNhan.GetDescription(),
                                                                      NgayGui = tdc.NgayGui,
                                                                      NgayThongBao = tdc.NgayThongBao,
                                                                      // TaiLieuDinhKem = _mp.Map<List<TaiLieuDinhKemViewModel>>(_dataContext.TaiLieuDinhKems.Where(x => x.NghiepVuId == ttg.Id).ToList()),
                                                                      // IdThongDiepGoc = ttg.Id,
                                                                      IdThamChieu = tdc.IdThamChieu,
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
                if (@params.IsThongDiepGui != true && @params.LoaiThongDiep != -1 && @params.LoaiThongDiep != null)
                {
                    var loaiThongDiep = TreeThongDiepNhan.FirstOrDefault(x => x.LoaiThongDiepId == @params.LoaiThongDiep);
                    if (loaiThongDiep.IsParent == true)
                    {
                        var maLoaiThongDieps = TreeThongDiepNhan.Where(x => x.LoaiThongDiepChaId == @params.LoaiThongDiep).Select(x => x.MaLoaiThongDiep).ToList();
                        query = query.Where(x => maLoaiThongDieps.Contains(x.MaLoaiThongDiep));
                    }
                    else
                    {
                        query = query.Where(x => x.MaLoaiThongDiep == loaiThongDiep.MaLoaiThongDiep);
                    }
                }

                if (@params.IsThongDiepGui == true && @params.LoaiThongDiep != -1)
                {
                    var loaiThongDiep = TreeThongDiepGui.FirstOrDefault(x => x.MaLoaiThongDiep == @params.LoaiThongDiep);
                    if (loaiThongDiep.IsParent == true)
                    {
                        var maLoaiThongDieps = TreeThongDiepNhan.Where(x => x.LoaiThongDiepChaId == @params.LoaiThongDiep).Select(x => x.MaLoaiThongDiep).ToList();
                        query = query.Where(x => maLoaiThongDieps.Contains(x.MaLoaiThongDiep));
                    }
                    else
                    {
                        query = query.Where(x => x.MaLoaiThongDiep == loaiThongDiep.MaLoaiThongDiep);
                    }
                }

                if (@params.TimKiemTheo != null)
                {
                    var timKiemTheo = @params.TimKiemTheo;
                    if (!string.IsNullOrEmpty(timKiemTheo.PhienBan))
                    {
                        var keyword = timKiemTheo.PhienBan.ToUpper().ToTrim();
                        query = query.Where(x => !string.IsNullOrEmpty(x.PhienBan) && x.PhienBan.ToUpper().ToTrim().Contains(keyword));
                    }
                    if (!string.IsNullOrEmpty(timKiemTheo.MaNoiGui))
                    {
                        var keyword = timKiemTheo.MaNoiGui.ToUpper().ToTrim();
                        query = query.Where(x => !string.IsNullOrEmpty(x.MaNoiGui) && x.MaNoiGui.ToUpper().ToTrim().Contains(keyword));
                    }
                    if (!string.IsNullOrEmpty(timKiemTheo.MaNoiNhan))
                    {
                        var keyword = timKiemTheo.MaNoiNhan.ToUpper().ToTrim();
                        query = query.Where(x => !string.IsNullOrEmpty(x.MaNoiNhan) && x.MaNoiNhan.ToUpper().ToTrim().Contains(keyword));
                    }
                    if (timKiemTheo.MaLoaiThongDiep != null)
                    {
                        var keyword = timKiemTheo.MaLoaiThongDiep;
                        query = query.Where(x => x.MaLoaiThongDiep == keyword.Value);
                    }
                    if (!string.IsNullOrEmpty(timKiemTheo.MaThongDiep))
                    {
                        var keyword = timKiemTheo.MaThongDiep.ToUpper().ToTrim();
                        query = query.Where(x => !string.IsNullOrEmpty(x.MaThongDiep) && x.MaThongDiep.ToUpper().ToTrim().Contains(keyword));
                    }
                    if (!string.IsNullOrEmpty(timKiemTheo.MaThongDiepThamChieu))
                    {
                        var keyword = timKiemTheo.MaThongDiepThamChieu.ToUpper().ToTrim();
                        query = query.Where(x => !string.IsNullOrEmpty(x.MaThongDiepThamChieu) && x.MaThongDiepThamChieu.ToUpper().ToTrim().Contains(keyword));
                    }
                    if (!string.IsNullOrEmpty(timKiemTheo.MaSoThue))
                    {
                        var keyword = timKiemTheo.MaSoThue.ToUpper().ToTrim();
                        query = query.Where(x => !string.IsNullOrEmpty(x.MaSoThue) && x.MaSoThue.ToUpper().ToTrim().Contains(keyword));
                    }
                    if (timKiemTheo.SoLuong != null)
                    {
                        var keyword = timKiemTheo.SoLuong;
                        query = query.Where(x => x.SoLuong == keyword.Value);
                    }
                }
                #region Filter and Sort
                if (@params.FilterColumns != null && @params.FilterColumns.Any())
                {
                    @params.FilterColumns = @params.FilterColumns.Where(x => x.IsFilter == true).ToList();

                    foreach (var filterCol in @params.FilterColumns)
                    {
                        switch (filterCol.ColKey)
                        {
                            case nameof(@params.Filter.PhienBan):
                                query = GenericFilterColumn<ThongDiepChungViewModel>.Query(query, x => x.PhienBan, filterCol, FilterValueType.String);
                                break;
                            case nameof(@params.Filter.MaNoiGui):
                                query = GenericFilterColumn<ThongDiepChungViewModel>.Query(query, x => x.MaNoiGui, filterCol, FilterValueType.String);
                                break;
                            case nameof(@params.Filter.MaNoiNhan):
                                query = GenericFilterColumn<ThongDiepChungViewModel>.Query(query, x => x.MaNoiNhan, filterCol, FilterValueType.String);
                                break;
                            case nameof(@params.Filter.MaLoaiThongDiep):
                                query = GenericFilterColumn<ThongDiepChungViewModel>.Query(query, x => x.MaLoaiThongDiep, filterCol, FilterValueType.Decimal);
                                break;
                            case nameof(@params.Filter.MaThongDiep):
                                query = GenericFilterColumn<ThongDiepChungViewModel>.Query(query, x => x.MaThongDiep, filterCol, FilterValueType.String);
                                break;
                            case nameof(@params.Filter.MaThongDiepThamChieu):
                                query = GenericFilterColumn<ThongDiepChungViewModel>.Query(query, x => x.MaThongDiepThamChieu, filterCol, FilterValueType.String);
                                break;
                            case nameof(@params.Filter.MaSoThue):
                                query = GenericFilterColumn<ThongDiepChungViewModel>.Query(query, x => x.MaSoThue, filterCol, FilterValueType.String);
                                break;
                            case nameof(@params.Filter.SoLuong):
                                query = GenericFilterColumn<ThongDiepChungViewModel>.Query(query, x => x.SoLuong, filterCol, FilterValueType.Decimal);
                                break;
                            default:
                                break;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(@params.SortKey))
                {
                    if (@params.SortKey == "PhienBan" && @params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.PhienBan);
                    }
                    if (@params.SortKey == "PhienBan" && @params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.PhienBan);
                    }

                    if (@params.SortKey == "MaNoiGui" && @params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.MaNoiGui);
                    }
                    if (@params.SortKey == "MaNoiGui" && @params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.MaNoiGui);
                    }


                    if (@params.SortKey == "MaNoiNhan" && @params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.MaNoiNhan);
                    }
                    if (@params.SortKey == "MaNoiNhan" && @params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.MaNoiNhan);
                    }

                    if (@params.SortKey == "MaLoaiThongDiep" && @params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.MaLoaiThongDiep);
                    }
                    if (@params.SortKey == "MaLoaiThongDiep" && @params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.MaLoaiThongDiep);
                    }

                    if (@params.SortKey == "MaThongDiep" && @params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.MaThongDiep);
                    }
                    if (@params.SortKey == "MaThongDiep" && @params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.MaThongDiep);
                    }

                    if (@params.SortKey == "MaThongDiepThamChieu" && @params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.MaThongDiepThamChieu);
                    }
                    if (@params.SortKey == "MaThongDiepThamChieu" && @params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.MaThongDiepThamChieu);
                    }

                    if (@params.SortKey == "MaSoThue" && @params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.MaSoThue);
                    }
                    if (@params.SortKey == "MaSoThue" && @params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.MaSoThue);
                    }

                    if (@params.SortKey == "SoLuong" && @params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.SoLuong);
                    }
                    if (@params.SortKey == "SoLuong" && @params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.SoLuong);
                    }

                }
                else
                {
                    query = query.OrderByDescending(x => x.CreatedDate);
                }
                #endregion

                var list = await query.ToListAsync();

                if (@params.PageSize == -1)
                {
                    @params.PageSize = await query.CountAsync();
                }

                return await PagedList<ThongDiepChungViewModel>
                     .CreateAsync(query, @params.PageNumber, @params.PageSize);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<EnumModel> GetListTimKiemTheoThongDiep()
        {
            ThongDiepSearch search = new ThongDiepSearch();
            var result = search.GetType().GetProperties()
                .Select(x => new EnumModel
                {
                    Value = x.Name,
                    Name = (x.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute).Name
                })
                .ToList();

            return result;
        }

        public async Task<ThongDiepChungViewModel> InsertThongDiepChung(ThongDiepChungViewModel model)
        {
            var _entity = _mp.Map<ThongDiepChung>(model);
            _entity.ThongDiepChungId = Guid.NewGuid().ToString();
            _entity.CreatedDate = DateTime.Now;
            _entity.ModifyDate = DateTime.Now;
            await _dataContext.ThongDiepChungs.AddAsync(_entity);
            if (await _dataContext.SaveChangesAsync() > 0)
            {
                return _mp.Map<ThongDiepChungViewModel>(_entity);
            }
            else return null;
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

        public async Task<bool> AddRangeDangKyUyNhiem(List<DangKyUyNhiemViewModel> listDangKyUyNhiems)
        {
            var toKhaiId = listDangKyUyNhiems.FirstOrDefault()?.IdToKhai;
            if (!string.IsNullOrEmpty(toKhaiId))
            {
                var oldEntities = await _dataContext.DangKyUyNhiems.Where(x => x.IdToKhai == toKhaiId).ToListAsync();
                if (oldEntities.Any())
                {
                    _dataContext.DangKyUyNhiems.RemoveRange(oldEntities);
                }

                var entities = _mp.Map<List<DangKyUyNhiem>>(listDangKyUyNhiems);
                var idx = 1;
                foreach (var entity in entities)
                {
                    entity.Id = Guid.NewGuid().ToString();
                    entity.STT = idx;
                    idx++;
                }
                await _dataContext.DangKyUyNhiems.AddRangeAsync(entities);
                return await _dataContext.SaveChangesAsync() == listDangKyUyNhiems.Count;
            }

            return false;
        }

        public async Task<ThongDiepChungViewModel> GetThongDiepChungById(string Id)
        {
            IQueryable<ThongDiepChungViewModel> queryToKhai = from tdc in _dataContext.ThongDiepChungs
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
                                                                  TenHinhThuc = tdc.HinhThuc.HasValue ? ((HThuc)tdc.HinhThuc).GetDescription() : null,
                                                                  TrangThaiGui = (TrangThaiGuiToKhaiDenCQT)tdc.TrangThaiGui,
                                                                  TenTrangThaiThongBao = ((TrangThaiGuiToKhaiDenCQT)tdc.TrangThaiGui).GetDescription(),
                                                                  NgayGui = tdc.NgayGui ?? null,
                                                                  IdThongDiepGoc = tdc.IdThongDiepGoc,
                                                                  IdThamChieu = tdc.IdThamChieu,
                                                                  CreatedDate = tdc.CreatedDate,
                                                                  ModifyDate = tdc.ModifyDate
                                                              };

            IQueryable<ThongDiepChungViewModel> query = queryToKhai;
            return await query.FirstOrDefaultAsync(x => x.ThongDiepChungId == Id);
        }

        public async Task<List<ThongDiepChungViewModel>> GetAllThongDiepTraVe(string MaThongDiep)
        {
            return _mp.Map<List<ThongDiepChungViewModel>>(await _dataContext.ThongDiepChungs.Where(x => x.ThongDiepGuiDi == false && x.MaThongDiepThamChieu == MaThongDiep).ToListAsync());
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
            var entity = await _dataContext.ThongDiepChungs.AsNoTracking().FirstOrDefaultAsync(x => x.IdThamChieu == ThamChieuId);
            var result = _mp.Map<ThongDiepChungViewModel>(entity);
            return result;
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

        public List<LoaiThongDiep> GetListLoaiThongDiepGui()
        {
            return TreeThongDiepGui;
        }

        public async Task<bool> InsertThongDiepNhanAsync(ThongDiepPhanHoiParams @params)
        {
            string id = Guid.NewGuid().ToString();

            // save file
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.ThongDiepChung);
            string folderPath = $"FilesUpload/{databaseName}/{loaiNghiepVu}/{id}";
            string fullFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, folderPath);
            if (!Directory.Exists(fullFolderPath))
            {
                Directory.CreateDirectory(fullFolderPath);
            }
            else
            {
                Directory.Delete(fullFolderPath, true);
                Directory.CreateDirectory(fullFolderPath);
            }

            string fileName = $"{Guid.NewGuid()}.xml";
            string filePath = Path.Combine(fullFolderPath, fileName);
            File.WriteAllBytes(filePath, Convert.FromBase64String(@params.DataXML));

            switch (@params.MLTDiep)
            {
                case (int)MLTDiep.TBTNToKhai: // 102
                    var tDiep102 = DataHelper.ConvertBase64ToObject<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._10.TDiep>(@params.DataXML);
                    var tdc102 = new ThongDiepChung
                    {
                        ThongDiepChungId = id,
                        PhienBan = tDiep102.TTChung.PBan,
                        MaNoiGui = tDiep102.TTChung.MNGui,
                        MaNoiNhan = tDiep102.TTChung.MNNhan,
                        MaLoaiThongDiep = int.Parse(tDiep102.TTChung.MLTDiep),
                        MaThongDiep = tDiep102.TTChung.MTDiep,
                        MaThongDiepThamChieu = tDiep102.TTChung.MTDTChieu,
                        MaSoThue = tDiep102.TTChung.MST,
                        SoLuong = tDiep102.TTChung.SLuong,
                        ThongDiepGuiDi = false,
                        HinhThuc = 0,
                        NgayThongBao = DateTime.Now,
                        FileXML = fileName
                    };
                    await _dataContext.ThongDiepChungs.AddAsync(tdc102);
                    break;
                case (int)MLTDiep.TBCNToKhai: // 103
                    var tDiep103 = DataHelper.ConvertBase64ToObject<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._11.TDiep>(@params.DataXML);
                    var tdc103 = new ThongDiepChung
                    {
                        ThongDiepChungId = id,
                        PhienBan = tDiep103.TTChung.PBan,
                        MaNoiGui = tDiep103.TTChung.MNGui,
                        MaNoiNhan = tDiep103.TTChung.MNNhan,
                        TrangThaiGui = tDiep103.DLieu.TBao.DLTBao.TTXNCQT == TTXNCQT.ChapNhan ? 5 : 6,
                        MaLoaiThongDiep = int.Parse(tDiep103.TTChung.MLTDiep),
                        MaThongDiep = tDiep103.TTChung.MTDiep,
                        MaThongDiepThamChieu = tDiep103.TTChung.MTDTChieu,
                        MaSoThue = tDiep103.TTChung.MST,
                        SoLuong = tDiep103.TTChung.SLuong,
                        ThongDiepGuiDi = false,
                        HinhThuc = 0,
                        NgayThongBao = DateTime.Now,
                        FileXML = fileName
                    };
                    await _dataContext.ThongDiepChungs.AddAsync(tdc103);
                    break;
                case (int)MLTDiep.TBCNToKhaiUN: // 104
                    var tDiep104 = DataHelper.ConvertBase64ToObject<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._12.TDiep>(@params.DataXML);
                    var tdc104 = new ThongDiepChung
                    {
                        ThongDiepChungId = id,
                        PhienBan = tDiep104.TTChung.PBan,
                        MaNoiGui = tDiep104.TTChung.MNGui,
                        MaNoiNhan = tDiep104.TTChung.MNNhan,
                        MaLoaiThongDiep = int.Parse(tDiep104.TTChung.MLTDiep),
                        MaThongDiep = tDiep104.TTChung.MTDiep,
                        MaThongDiepThamChieu = tDiep104.TTChung.MTDTChieu,
                        MaSoThue = tDiep104.TTChung.MST,
                        SoLuong = tDiep104.TTChung.SLuong,
                        ThongDiepGuiDi = false,
                        HinhThuc = 0,
                        NgayThongBao = DateTime.Now,
                        FileXML = fileName
                    };
                    await _dataContext.ThongDiepChungs.AddAsync(tdc104);
                    break;
                case (int)MLTDiep.TBKQCMHDon: // 202
                    var tDiep202 = DataHelper.ConvertBase64ToObject<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6.TDiep>(@params.DataXML);
                    ThongDiepChung tdc202 = new ThongDiepChung
                    {
                        ThongDiepChungId = id,
                        PhienBan = tDiep202.TTChung.PBan,
                        MaNoiGui = tDiep202.TTChung.MNGui,
                        MaNoiNhan = tDiep202.TTChung.MNNhan,
                        MaLoaiThongDiep = int.Parse(tDiep202.TTChung.MLTDiep),
                        MaThongDiep = tDiep202.TTChung.MTDiep,
                        MaThongDiepThamChieu = tDiep202.TTChung.MTDTChieu,
                        MaSoThue = tDiep202.TTChung.MST,
                        SoLuong = tDiep202.TTChung.SLuong,
                        ThongDiepGuiDi = false,
                        HinhThuc = 0,
                        NgayThongBao = DateTime.Now,
                        FileXML = fileName
                    };
                    await _dataContext.ThongDiepChungs.AddAsync(tdc202);
                    break;
                case (int)MLTDiep.TDTBKQKTDLHDon: // 204
                    var tDiep204 = DataHelper.ConvertBase64ToObject<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._8.TDiep>(@params.DataXML);
                    ThongDiepChung tdc204 = new ThongDiepChung
                    {
                        ThongDiepChungId = id,
                        PhienBan = tDiep204.TTChung.PBan,
                        MaNoiGui = tDiep204.TTChung.MNGui,
                        MaNoiNhan = tDiep204.TTChung.MNNhan,
                        MaLoaiThongDiep = int.Parse(tDiep204.TTChung.MLTDiep),
                        MaThongDiep = tDiep204.TTChung.MTDiep,
                        MaThongDiepThamChieu = tDiep204.TTChung.MTDTChieu,
                        MaSoThue = tDiep204.TTChung.MST,
                        SoLuong = tDiep204.TTChung.SLuong,
                        ThongDiepGuiDi = false,
                        HinhThuc = 0,
                        NgayThongBao = DateTime.Now,
                        FileXML = fileName
                    };
                    await _dataContext.ThongDiepChungs.AddAsync(tdc204);
                    break;
                case (int)MLTDiep.TDCDLTVANUQCTQThue: // 999
                    var tDiep999 = DataHelper.ConvertBase64ToObject<ViewModels.XML.QuyDinhKyThuatHDDT.PhanI.IV._6.TDiep>(@params.DataXML);
                    ThongDiepChung tdc999 = new ThongDiepChung
                    {
                        ThongDiepChungId = id,
                        PhienBan = tDiep999.TTChung.PBan,
                        MaNoiGui = tDiep999.TTChung.MNGui,
                        MaNoiNhan = tDiep999.TTChung.MNNhan,
                        MaLoaiThongDiep = int.Parse(tDiep999.TTChung.MLTDiep),
                        MaThongDiep = tDiep999.TTChung.MTDiep,
                        MaThongDiepThamChieu = tDiep999.TTChung.MTDTChieu,
                        MaSoThue = tDiep999.TTChung.MST,
                        SoLuong = tDiep999.TTChung.SLuong,
                        ThongDiepGuiDi = false,
                        HinhThuc = 0,
                        NgayThongBao = DateTime.Now,
                        FileXML = fileName
                    };
                    await _dataContext.ThongDiepChungs.AddAsync(tdc999);
                    break;
                case (int)MLTDiep.TBTNVKQXLHDDTSSot: // 301
                    var tDiep301 = DataHelper.ConvertBase64ToObject<ViewModels.XML.ThongDiepGuiNhanCQT.TDiepNhanHDonSaiSot.TDiep>(@params.DataXML);
                    ThongDiepChung tdc301 = new ThongDiepChung
                    {
                        ThongDiepChungId = id,
                        PhienBan = tDiep301.TTChung.PBan,
                        MaNoiGui = tDiep301.TTChung.MNGui,
                        MaNoiNhan = tDiep301.TTChung.MNNhan,
                        MaLoaiThongDiep = tDiep301.TTChung.MLTDiep,
                        MaThongDiep = tDiep301.TTChung.MTDiep,
                        MaThongDiepThamChieu = tDiep301.TTChung.MTDTChieu,
                        MaSoThue = tDiep301.TTChung.MST,
                        SoLuong = tDiep301.TTChung.SLuong,
                        ThongDiepGuiDi = false,
                        HinhThuc = (int)HThuc.ChinhThuc,
                        NgayThongBao = DateTime.Now,
                        FileXML = fileName
                    };
                    await _dataContext.ThongDiepChungs.AddAsync(tdc301);
                    break;
                case (int)MLTDiep.TDTBHDDTCRSoat: // 302
                    var tDiep302 = DataHelper.ConvertBase64ToObject<ViewModels.XML.ThongDiepGuiNhanCQT.TDiepNhanHDonRaSoat.TDiep>(@params.DataXML);
                    ThongDiepChung tdc302 = new ThongDiepChung
                    {
                        ThongDiepChungId = id,
                        PhienBan = tDiep302.TTChung.PBan,
                        MaNoiGui = tDiep302.TTChung.MNGui,
                        MaNoiNhan = tDiep302.TTChung.MNNhan,
                        MaLoaiThongDiep = tDiep302.TTChung.MLTDiep,
                        MaThongDiep = tDiep302.TTChung.MTDiep,
                        MaThongDiepThamChieu = tDiep302.TTChung.MTDTChieu,
                        MaSoThue = tDiep302.TTChung.MST,
                        SoLuong = tDiep302.TTChung.SLuong,
                        ThongDiepGuiDi = false,
                        HinhThuc = (int)HThuc.ChinhThuc,
                        NgayThongBao = DateTime.Now,
                        FileXML = fileName
                    };
                    await _dataContext.ThongDiepChungs.AddAsync(tdc302);
                    break;
                default:
                    break;
            }

            var result = await _dataContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<ThongDiepChiTiet> ShowThongDiepFromFileByIdAsync(string id)
        {
            try
            {
                ThongDiepChiTiet result = new ThongDiepChiTiet
                {
                    ThongDiepChiTiet1s = new List<ThongDiepChiTiet1>(),
                    ThongDiepChiTiet2s = new List<ThongDiepChiTiet2>()
                };

                var entity = await _dataContext.ThongDiepChungs
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ThongDiepChungId == id);

                string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string loaiNghiepVu;
                if (entity.MaLoaiThongDiep != (int)MLTDiep.TDTBHDDLSSot)
                {
                    loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.ThongDiepChung);
                }
                else
                {
                    loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.ThongDiepGuiNhanCQT);
                }

                string folderPath;
                if (entity.MaLoaiThongDiep != (int)MLTDiep.TDTBHDDLSSot)
                {
                    if (entity.ThongDiepGuiDi == true)
                    {
                        folderPath = $"FilesUpload/{databaseName}/{loaiNghiepVu}/{id}/{entity.FileXML}";
                    }
                    else
                    {
                        folderPath = $"FilesUpload/{loaiNghiepVu}/{id}/{entity.FileXML}";
                    }
                }
                else
                {
                    var entityTC = _dataContext.ThongDiepGuiCQTs.FirstOrDefault(x => x.Id == entity.IdThamChieu);
                    if (entityTC.DaKyGuiCQT == true)
                    {
                        folderPath = $"FilesUpload/{databaseName}/{loaiNghiepVu}/xml/signed/{entity.IdThamChieu}/{entityTC.FileXMLDaKy}";
                    }
                    else
                    {
                        var files = entityTC.FileDinhKem.Split(';');
                        var file = files.FirstOrDefault(x => x.Contains(".xml"));
                        folderPath = $"FilesUpload/{databaseName}/{loaiNghiepVu}/xml/unsigned/{entity.IdThamChieu}/{file}";
                    }
                }
                string fullFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, folderPath);

                string moTaLoi = string.Empty;
                int length = 0;

                switch (entity.MaLoaiThongDiep)
                {
                    case (int)MLTDiep.TBTNToKhai: // 102
                        var tDiep102 = DataHelper.ConvertFileToObject<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._10.TDiep>(fullFolderPath);

                        var lstLoi102 = tDiep102.DLieu.TBao.DLTBao.DSLDKCNhan ?? new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._3.LDo>();
                        length = lstLoi102.Count;
                        for (int i = 0; i < length; i++)
                        {
                            var item = lstLoi102[i];
                            moTaLoi += $"- {i + 1}. Mã lỗi: {item.MLoi}; Mô tả: {item.MTa}\n";
                        }

                        result.ThongDiepChiTiet1s.Add(new ThongDiepChiTiet1
                        {
                            PhienBan = tDiep102.DLieu.TBao.DLTBao.PBan,
                            MauSo = tDiep102.DLieu.TBao.DLTBao.MSo,
                            TenThongBao = tDiep102.DLieu.TBao.DLTBao.Ten,
                            SoThongBao = tDiep102.DLieu.TBao.DLTBao.So,
                            NgayThongBao = DateTime.Parse(tDiep102.DLieu.TBao.DLTBao.NTBao),
                            DiaDanh = tDiep102.DLieu.TBao.DLTBao.DDanh,
                            MaSoThue = tDiep102.DLieu.TBao.DLTBao.MST,
                            TenNguoiNopThue = tDiep102.DLieu.TBao.DLTBao.TNNT,
                            TenToKhai = tDiep102.DLieu.TBao.DLTBao.TTKhai,
                            MaGiaoDichDienTu = tDiep102.DLieu.TBao.DLTBao.MGDDTu,
                            ThoiGianGui = DateTime.Parse(tDiep102.DLieu.TBao.DLTBao.TGGui),
                            ThoiGianNhan = DateTime.Parse(tDiep102.DLieu.TBao.DLTBao.TGNhan),
                            TruongHop = tDiep102.DLieu.TBao.DLTBao.THop.GetDescription(),
                            MoTaLoi = moTaLoi
                        });
                        break;
                    case (int)MLTDiep.TBCNToKhai: // 103
                        var tDiep103 = DataHelper.ConvertFileToObject<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._11.TDiep>(fullFolderPath);

                        var lstLoi103 = tDiep103.DLieu.TBao.DLTBao.DSLDKCNhan ?? new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._3.LDo>();
                        length = lstLoi103.Count;
                        for (int i = 0; i < length; i++)
                        {
                            var item = lstLoi103[i];
                            moTaLoi += $"- {i + 1}. Mã lỗi: {item.MLoi}; Mô tả: {item.MTa}\n";
                        }

                        result.ThongDiepChiTiet1s.Add(new ThongDiepChiTiet1
                        {
                            PhienBan = tDiep103.DLieu.TBao.DLTBao.PBan,
                            MauSo = tDiep103.DLieu.TBao.DLTBao.MSo,
                            TenThongBao = tDiep103.DLieu.TBao.DLTBao.Ten,
                            DiaDanh = tDiep103.DLieu.TBao.DLTBao.DDanh,
                            TenCQTCapTren = tDiep103.DLieu.TBao.DLTBao.TCQTCTren,
                            TenCQTRaThongBao = tDiep103.DLieu.TBao.DLTBao.TCQT,
                            MaSoThue = tDiep103.DLieu.TBao.DLTBao.MST,
                            TenNguoiNopThue = tDiep103.DLieu.TBao.DLTBao.TNNT,
                            NgayDangKy_ThayDoi = DateTime.Parse(tDiep103.DLieu.TBao.DLTBao.Ngay),
                            HinhThucDanhKy_ThayDoi = tDiep103.DLieu.TBao.DLTBao.HTDKy.GetDescription(),
                            TrangThaiXacNhanCuaCQT = tDiep103.DLieu.TBao.DLTBao.TTXNCQT.GetDescription(),
                            MoTaLoi = moTaLoi
                        });
                        break;
                    case (int)MLTDiep.TBCNToKhaiUN: // 104
                        var tDiep104 = DataHelper.ConvertFileToObject<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._12.TDiep>(fullFolderPath);

                        result.ThongDiepChiTiet1s.Add(new ThongDiepChiTiet1
                        {
                            PhienBan = tDiep104.DLieu.TBao.DLTBao.PBan,
                            MauSo = tDiep104.DLieu.TBao.DLTBao.MSo,
                            TenThongBao = tDiep104.DLieu.TBao.DLTBao.Ten,
                            SoThongBao = tDiep104.DLieu.TBao.DLTBao.So,
                            NgayThongBao = DateTime.Parse(tDiep104.DLieu.TBao.DLTBao.Ngay),
                            DiaDanh = tDiep104.DLieu.TBao.DLTBao.DDanh,
                            TenCQTCapTren = tDiep104.DLieu.TBao.DLTBao.TCQTCTren,
                            TenCQTRaThongBao = tDiep104.DLieu.TBao.DLTBao.TCQT,
                            MaSoThue = tDiep104.DLieu.TBao.DLTBao.MST,
                            TenNguoiNopThue = tDiep104.DLieu.TBao.DLTBao.TNNT,
                            NgayDangKy_ThayDoi = DateTime.Parse(tDiep104.DLieu.TBao.DLTBao.Ngay),
                            LoaiUyNhiem = tDiep104.DLieu.TBao.DLTBao.LUNhiem.GetDescription(),
                            MaGiaoDichDienTu = tDiep104.DLieu.TBao.DLTBao.MGDDTu,
                            ThoiGianNhan = DateTime.Parse(tDiep104.DLieu.TBao.DLTBao.TGNhan),
                        });

                        var dSTTUNhiem = tDiep104.DLieu.TBao.DLTBao.DSTTUNhiem ?? new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._5.TTUNhiem>();
                        for (int i = 0; i < dSTTUNhiem.Count; i++)
                        {
                            var dSTTUNhiemItem = dSTTUNhiem[i];
                            dSTTUNhiemItem.DSHDUNhiem = dSTTUNhiemItem.DSHDUNhiem ?? new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._5.HDUNhiem>();

                            for (int j = 0; j < dSTTUNhiemItem.DSLDKCNhan.Count; j++)
                            {
                                var dSLDKCNhanItem = dSTTUNhiemItem.DSLDKCNhan[i];
                                moTaLoi += $"- {j + 1}. Mã lỗi: {dSLDKCNhanItem.MLoi}; Mô tả: {dSLDKCNhanItem.MTa}\n";
                            }

                            for (int k = 0; k < dSTTUNhiemItem.DSHDUNhiem.Count; k++)
                            {
                                var dSHDUNhiemItem = dSTTUNhiemItem.DSHDUNhiem[k];
                                result.ThongDiepChiTiet2s.Add(new ThongDiepChiTiet2
                                {
                                    STT = i + 1,
                                    MaSoThue = dSTTUNhiemItem.MST,
                                    TenToChuc = dSTTUNhiemItem.TTChuc,
                                    NgayTiepNhan = !string.IsNullOrEmpty(dSTTUNhiemItem.NTNhan) ? DateTime.Parse(dSTTUNhiemItem.NTNhan) : (DateTime?)null,
                                    TenLoaiHoaDon = dSHDUNhiemItem.TLHDon,
                                    KyHieuMauSoHoaDon = dSHDUNhiemItem.KHMSHDon,
                                    KyHieuHoaDon = dSHDUNhiemItem.KHHDon,
                                    MucDich = dSHDUNhiemItem.MDich,
                                    TuNgay = DateTime.Parse(dSHDUNhiemItem.TNgay),
                                    DenNgay = DateTime.Parse(dSHDUNhiemItem.DNgay),
                                    MoTaLoi = moTaLoi
                                });
                            }
                        }
                        break;
                    case (int)MLTDiep.TDCDLHDKMDCQThue:
                        var tDiep203 = DataHelper.ConvertFileToObject<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._7.TDiep>(fullFolderPath);
                        foreach (var item1 in tDiep203.DLieu)
                        {
                            result.ThongDiepChiTiet1s.Add(new ThongDiepChiTiet1
                            {
                                PhienBan = item1.DLHDon.TTChung.PBan,
                                MaCuaCoQuanThue = item1.MCCQT,
                                TenHoaDon = item1.DLHDon.TTChung.THDon,
                                KyHieuMauSoHoaDon = item1.DLHDon.TTChung.KHMSHDon,
                                KyHieuHoaDon = item1.DLHDon.TTChung.KHHDon,
                                SoHoaDon = item1.DLHDon.TTChung.SHDon,
                                MaHoSo = item1.DLHDon.TTChung.MHSo,
                                NgayLap = DateTime.Parse(item1.DLHDon.TTChung.NLap),
                                SoBangKe = item1.DLHDon.TTChung.SBKe,
                                NgayBangKe = !string.IsNullOrEmpty(item1.DLHDon.TTChung.NBKe) ? DateTime.Parse(item1.DLHDon.TTChung.NBKe) : (DateTime?)null,
                                DonViTienTe = item1.DLHDon.TTChung.DVTTe,
                                TyGia = item1.DLHDon.TTChung.TGia,
                                HinhThucThanhToan = item1.DLHDon.TTChung.HTTToan,
                                MaSoThueDVNUNLHD = item1.DLHDon.TTChung.MSTDVNUNLHDon,
                                TenDVNhanUNLHD = item1.DLHDon.TTChung.TDVNUNLHDon,
                                DiaChiDVNhanUNLHD = item1.DLHDon.TTChung.DCDVNUNLHDon,
                                TinhChatHoaDon = item1.DLHDon.TTChung.TTHDLQuan?.TCHDon.GetDescription(),
                                LoaiHoaDonCoLienQuan = item1.DLHDon.TTChung.TTHDLQuan?.LHDCLQuan.GetDescription(),
                                KyHieuMauSoHoaDonCoLienQuan = item1.DLHDon.TTChung.TTHDLQuan?.KHMSHDCLQuan,
                                KyHieuHoaDonCoLienQuan = item1.DLHDon.TTChung.TTHDLQuan?.KHHDCLQuan,
                                SoHoaDonCoLienQuan = item1.DLHDon.TTChung.TTHDLQuan?.SHDCLQuan,
                                NgayLapHoaDonCoLienQuan = !string.IsNullOrEmpty(item1.DLHDon.TTChung.TTHDLQuan?.NLHDCLQuan) ? DateTime.Parse(item1.DLHDon.TTChung.TTHDLQuan?.NLHDCLQuan) : (DateTime?)null,
                                TenNguoiBan = item1.DLHDon.NDHDon.NBan.Ten,
                                MaSoThueNguoiBan = item1.DLHDon.NDHDon.NBan.MST,
                                DiaChiNguoiBan = item1.DLHDon.NDHDon.NBan.DChi,
                                TenNguoiMua = item1.DLHDon.NDHDon.NMua.Ten,
                                MaSoThueNguoiMua = item1.DLHDon.NDHDon.NMua.MST,
                                DiaChiNguoiMua = item1.DLHDon.NDHDon.NMua.DChi,
                                TongTienChuaThue = item1.DLHDon.NDHDon.TToan.TgTCThue,
                                TongTienThue = item1.DLHDon.NDHDon.TToan.TgTThue,
                                LoaiPhis = item1.DLHDon.NDHDon.TToan.DSLPhi
                                .Select(x => new LoaiPhi
                                {
                                    TenLoaiPhi = x.TLPhi,
                                    TienPhi = x.TPhi
                                }).ToList(),
                                TongTienChietKhauThuongMai = item1.DLHDon.NDHDon.TToan.TTCKTMai,
                                TongTienThanhToan = item1.DLHDon.NDHDon.TToan.TgTTTBSo
                            });
                        }
                        break;
                    case (int)MLTDiep.TDGHDDTTCQTCapMa:
                        var tDiep200 = DataHelper.ConvertFileToObject<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6.TDiep>(fullFolderPath);
                        var item2 = tDiep200.DLieu.HDon;
                        result.ThongDiepChiTiet1s.Add(new ThongDiepChiTiet1
                        {
                            PhienBan = item2.DLHDon.TTChung.PBan,
                            MaCuaCoQuanThue = item2.MCCQT,
                            TenHoaDon = item2.DLHDon.TTChung.THDon,
                            KyHieuMauSoHoaDon = item2.DLHDon.TTChung.KHMSHDon,
                            KyHieuHoaDon = item2.DLHDon.TTChung.KHHDon,
                            SoHoaDon = item2.DLHDon.TTChung.SHDon,
                            MaHoSo = item2.DLHDon.TTChung.MHSo,
                            NgayLap = DateTime.Parse(item2.DLHDon.TTChung.NLap),
                            SoBangKe = item2.DLHDon.TTChung.SBKe,
                            NgayBangKe = !string.IsNullOrEmpty(item2.DLHDon.TTChung.NBKe) ? DateTime.Parse(item2.DLHDon.TTChung.NBKe) : (DateTime?)null,
                            DonViTienTe = item2.DLHDon.TTChung.DVTTe,
                            TyGia = item2.DLHDon.TTChung.TGia,
                            HinhThucThanhToan = item2.DLHDon.TTChung.HTTToan,
                            MaSoThueDVNUNLHD = item2.DLHDon.TTChung.MSTDVNUNLHDon,
                            TenDVNhanUNLHD = item2.DLHDon.TTChung.TDVNUNLHDon,
                            DiaChiDVNhanUNLHD = item2.DLHDon.TTChung.DCDVNUNLHDon,
                            TinhChatHoaDon = item2.DLHDon.TTChung.TTHDLQuan?.TCHDon.GetDescription(),
                            LoaiHoaDonCoLienQuan = item2.DLHDon.TTChung.TTHDLQuan?.LHDCLQuan.GetDescription(),
                            KyHieuMauSoHoaDonCoLienQuan = item2.DLHDon.TTChung.TTHDLQuan?.KHMSHDCLQuan,
                            KyHieuHoaDonCoLienQuan = item2.DLHDon.TTChung.TTHDLQuan?.KHHDCLQuan,
                            SoHoaDonCoLienQuan = item2.DLHDon.TTChung.TTHDLQuan?.SHDCLQuan,
                            NgayLapHoaDonCoLienQuan = !string.IsNullOrEmpty(item2.DLHDon.TTChung.TTHDLQuan?.NLHDCLQuan) ? DateTime.Parse(item2.DLHDon.TTChung.TTHDLQuan?.NLHDCLQuan) : (DateTime?)null,
                            TenNguoiBan = item2.DLHDon.NDHDon.NBan.Ten,
                            MaSoThueNguoiBan = item2.DLHDon.NDHDon.NBan.MST,
                            DiaChiNguoiBan = item2.DLHDon.NDHDon.NBan.DChi,
                            TenNguoiMua = item2.DLHDon.NDHDon.NMua.Ten,
                            MaSoThueNguoiMua = item2.DLHDon.NDHDon.NMua.MST,
                            DiaChiNguoiMua = item2.DLHDon.NDHDon.NMua.DChi,
                            TongTienChuaThue = item2.DLHDon.NDHDon.TToan.TgTCThue,
                            TongTienThue = item2.DLHDon.NDHDon.TToan.TgTThue,
                            LoaiPhis = item2.DLHDon.NDHDon.TToan.DSLPhi
                            .Select(x => new LoaiPhi
                            {
                                TenLoaiPhi = x.TLPhi,
                                TienPhi = x.TPhi
                            }).ToList(),
                            TongTienChietKhauThuongMai = item2.DLHDon.NDHDon.TToan.TTCKTMai,
                            TongTienThanhToan = item2.DLHDon.NDHDon.TToan.TgTTTBSo
                        });
                        break;
                    case (int)MLTDiep.TBKQCMHDon: // 202
                        var tDiep202 = DataHelper.ConvertFileToObject<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6.TDiep>(fullFolderPath);

                        result.ThongDiepChiTiet1s.Add(new ThongDiepChiTiet1
                        {
                            PhienBan = tDiep202.DLieu.HDon.DLHDon.TTChung.PBan,
                            MaCuaCoQuanThue = tDiep202.DLieu.HDon.MCCQT,
                            TenHoaDon = tDiep202.DLieu.HDon.DLHDon.TTChung.THDon,
                            KyHieuMauSoHoaDon = tDiep202.DLieu.HDon.DLHDon.TTChung.KHMSHDon,
                            KyHieuHoaDon = tDiep202.DLieu.HDon.DLHDon.TTChung.KHHDon,
                            SoHoaDon = tDiep202.DLieu.HDon.DLHDon.TTChung.SHDon,
                            MaHoSo = tDiep202.DLieu.HDon.DLHDon.TTChung.MHSo,
                            NgayLap = DateTime.Parse(tDiep202.DLieu.HDon.DLHDon.TTChung.NLap),
                            SoBangKe = tDiep202.DLieu.HDon.DLHDon.TTChung.SBKe,
                            NgayBangKe = !string.IsNullOrEmpty(tDiep202.DLieu.HDon.DLHDon.TTChung.NBKe) ? DateTime.Parse(tDiep202.DLieu.HDon.DLHDon.TTChung.NBKe) : (DateTime?)null,
                            DonViTienTe = tDiep202.DLieu.HDon.DLHDon.TTChung.DVTTe,
                            TyGia = tDiep202.DLieu.HDon.DLHDon.TTChung.TGia,
                            HinhThucThanhToan = tDiep202.DLieu.HDon.DLHDon.TTChung.HTTToan,
                            MaSoThueDVNUNLHD = tDiep202.DLieu.HDon.DLHDon.TTChung.MSTDVNUNLHDon,
                            TenDVNhanUNLHD = tDiep202.DLieu.HDon.DLHDon.TTChung.TDVNUNLHDon,
                            DiaChiDVNhanUNLHD = tDiep202.DLieu.HDon.DLHDon.TTChung.DCDVNUNLHDon,
                            TinhChatHoaDon = tDiep202.DLieu.HDon.DLHDon.TTChung.TTHDLQuan?.TCHDon.GetDescription(),
                            LoaiHoaDonCoLienQuan = tDiep202.DLieu.HDon.DLHDon.TTChung.TTHDLQuan?.LHDCLQuan.GetDescription(),
                            KyHieuMauSoHoaDonCoLienQuan = tDiep202.DLieu.HDon.DLHDon.TTChung.TTHDLQuan?.KHMSHDCLQuan,
                            KyHieuHoaDonCoLienQuan = tDiep202.DLieu.HDon.DLHDon.TTChung.TTHDLQuan?.KHHDCLQuan,
                            SoHoaDonCoLienQuan = tDiep202.DLieu.HDon.DLHDon.TTChung.TTHDLQuan?.SHDCLQuan,
                            NgayLapHoaDonCoLienQuan = !string.IsNullOrEmpty(tDiep202.DLieu.HDon.DLHDon.TTChung.TTHDLQuan?.NLHDCLQuan) ? DateTime.Parse(tDiep202.DLieu.HDon.DLHDon.TTChung.TTHDLQuan?.NLHDCLQuan) : (DateTime?)null,
                            TenNguoiBan = tDiep202.DLieu.HDon.DLHDon.NDHDon.NBan.Ten,
                            MaSoThueNguoiBan = tDiep202.DLieu.HDon.DLHDon.NDHDon.NBan.MST,
                            DiaChiNguoiBan = tDiep202.DLieu.HDon.DLHDon.NDHDon.NBan.DChi,
                            TenNguoiMua = tDiep202.DLieu.HDon.DLHDon.NDHDon.NMua.Ten,
                            MaSoThueNguoiMua = tDiep202.DLieu.HDon.DLHDon.NDHDon.NMua.MST,
                            DiaChiNguoiMua = tDiep202.DLieu.HDon.DLHDon.NDHDon.NMua.DChi,
                            TongTienChuaThue = tDiep202.DLieu.HDon.DLHDon.NDHDon.TToan.TgTCThue,
                            TongTienThue = tDiep202.DLieu.HDon.DLHDon.NDHDon.TToan.TgTThue,
                            LoaiPhis = tDiep202.DLieu.HDon.DLHDon.NDHDon.TToan.DSLPhi
                                .Select(x => new LoaiPhi
                                {
                                    TenLoaiPhi = x.TLPhi,
                                    TienPhi = x.TPhi
                                }).ToList(),
                            TongTienChietKhauThuongMai = tDiep202.DLieu.HDon.DLHDon.NDHDon.TToan.TTCKTMai,
                            TongTienThanhToan = tDiep202.DLieu.HDon.DLHDon.NDHDon.TToan.TgTTTBSo
                        });
                        break;
                    case (int)MLTDiep.TDTBKQKTDLHDon: // 204
                        var tDiep204 = DataHelper.ConvertFileToObject<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._8.TDiep>(fullFolderPath);

                        result.ThongDiepChiTiet1s.Add(new ThongDiepChiTiet1
                        {
                            PhienBan = tDiep204.DLieu.TBao.DLTBao.PBan,
                            MauSo = tDiep204.DLieu.TBao.DLTBao.MSo,
                            TenThongBao = tDiep204.DLieu.TBao.DLTBao.Ten,
                            SoThongBao = tDiep204.DLieu.TBao.DLTBao.So,
                            NgayThongBao = DateTime.Parse(tDiep204.DLieu.TBao.DLTBao.NTBao),
                            DiaDanh = tDiep204.DLieu.TBao.DLTBao.DDanh,
                            MaSoThue = tDiep204.DLieu.TBao.DLTBao.MST,
                            TenNguoiNopThue = tDiep204.DLieu.TBao.DLTBao.TNNT,
                            MaDonViQuanHeNganSach = tDiep204.DLieu.TBao.DLTBao.MDVQHNSach,
                            ThoiGianGui = !string.IsNullOrEmpty(tDiep204.DLieu.TBao.DLTBao.TGGui) ? DateTime.Parse(tDiep204.DLieu.TBao.DLTBao.TGGui) : (DateTime?)null,
                            LoaiThongBao = tDiep204.DLieu.TBao.DLTBao.LTBao.GetDescription(),
                            CanCu = tDiep204.DLieu.TBao.DLTBao.CCu,
                            SoLuong = tDiep204.DLieu.TBao.DLTBao.SLuong
                        });

                        if (tDiep204.DLieu.TBao.DLTBao.LCMa != null)
                        {
                            var lCMa = tDiep204.DLieu.TBao.DLTBao.LCMa;

                            lCMa.DSLDo = lCMa.DSLDo ?? new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._3.LDo>();
                            for (int i = 0; i < lCMa.DSLDo.Count; i++)
                            {
                                var dSLDoItem = lCMa.DSLDo[i];
                                moTaLoi += $"- {i + 1}. Mã lỗi: {dSLDoItem.MLoi}; Mô tả: {dSLDoItem.MLoi}; Hướng dẫn xử lý (nếu có): {dSLDoItem.HDXLy}; Ghi chú (nếu có): {dSLDoItem.GChu}\n";
                            }

                            result.ThongDiepChiTiet1s.Add(new ThongDiepChiTiet1
                            {
                                KyHieuMauSoHoaDon = lCMa.KHMSHDon,
                                KyHieuHoaDon = lCMa.KHHDon,
                                SoHoaDon = lCMa.SHDon,
                                NgayLap = DateTime.Parse(lCMa.NLap),
                                MoTaLoi = moTaLoi
                            });
                        }
                        break;
                    case (int)MLTDiep.TBTNVKQXLHDDTSSot: // 301
                        var tDiep301 = DataHelper.ConvertFileToObject<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.III._5.TDiep>(fullFolderPath);

                        var dSLDKCNHan = tDiep301.DLieu.TBao.DLTBao.DSLDKCNhan;
                        dSLDKCNHan = dSLDKCNHan ?? new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.III._2.DSLDKTNhanDLTBao.LDo>();
                        for (int i = 0; i < dSLDKCNHan.Count; i++)
                        {
                            var dSLDKCNhanItem = dSLDKCNHan[i];
                            moTaLoi += $"- {i + 1}. Mã lỗi: {dSLDKCNhanItem.MLoi}; Mô tả: {dSLDKCNhanItem.MLoi}; Hướng dẫn xử lý (nếu có): {dSLDKCNhanItem.HDXLy}; Ghi chú (nếu có): {dSLDKCNhanItem.GChu}\n";
                        }

                        result.ThongDiepChiTiet1s.Add(new ThongDiepChiTiet1
                        {
                            PhienBan = tDiep301.DLieu.TBao.DLTBao.PBan,
                            MauSo = tDiep301.DLieu.TBao.DLTBao.MSo,
                            TenThongBao = tDiep301.DLieu.TBao.DLTBao.Ten,
                            DiaDanh = tDiep301.DLieu.TBao.DLTBao.DDanh,
                            TenCQTCapTren = tDiep301.DLieu.TBao.DLTBao.TCQTCTren,
                            TenCQTRaThongBao = tDiep301.DLieu.TBao.DLTBao.TCQT,
                            TenNguoiNopThue = tDiep301.DLieu.TBao.DLTBao.TNNT,
                            MaSoThue = tDiep301.DLieu.TBao.DLTBao.MST,
                            MaDonViQuanHeNganSach = tDiep301.DLieu.TBao.DLTBao.MDVQHNSach,
                            MaGiaoDichDienTu = tDiep301.DLieu.TBao.DLTBao.MGDDTu,
                            ThoiGianNhan = DateTime.Parse(tDiep301.DLieu.TBao.DLTBao.TGNhan),
                            SoThuTuThe = tDiep301.DLieu.TBao.DLTBao.STTThe,
                            MoTaLoi = moTaLoi
                        });

                        if (tDiep301.DLieu.TBao.DLTBao.DSHDon != null)
                        {
                            moTaLoi = string.Empty;
                            var dSHDon = tDiep301.DLieu.TBao.DLTBao.DSHDon;
                            for (int i = 0; i < length; i++)
                            {
                                var dSHDonItem = dSHDon[i];

                                dSHDonItem.DSLDKTNhan = dSHDonItem.DSLDKTNhan ?? new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.III._2.DSLDKTNhanHDon.LDo>();

                                for (int j = 0; j < dSHDonItem.DSLDKTNhan.Count; j++)
                                {
                                    var dSLDKTNhanItem = dSHDonItem.DSLDKTNhan[i];
                                    moTaLoi += $"- {j + 1}. Mã lỗi: {dSLDKTNhanItem.MLoi}; Mô tả: {dSLDKTNhanItem.MTa}\n";
                                }

                                result.ThongDiepChiTiet2s.Add(new ThongDiepChiTiet2
                                {
                                    STT = i + 1,
                                    MaCQTCap = dSHDonItem.MCQTCap,
                                    KyHieuMauSoHoaDon = dSHDonItem.KHMSHDon,
                                    KyHieuHoaDon = dSHDonItem.KHHDon,
                                    SoHoaDon = dSHDonItem.SHDon,
                                    NgayLap = DateTime.Parse(dSHDonItem.NLap),
                                    LoaiHoaDonDienTuApDung = dSHDonItem.LADHDDT.GetDescription(),
                                    TinhChatThongBao = dSHDonItem.TCTBao.GetDescription(),
                                    TrangThaiTiepNhanCuaCQT = dSHDonItem.TTTNCCQT.GetDescription(),
                                    MoTaLoi = moTaLoi
                                });
                            }
                        }

                        break;
                    case (int)MLTDiep.TDTBHDDTCRSoat: // 302
                        var tDiep302 = DataHelper.ConvertFileToObject<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.III._6.TDiep>(fullFolderPath);

                        result.ThongDiepChiTiet1s.Add(new ThongDiepChiTiet1
                        {
                            PhienBan = tDiep302.DLieu.TBao.DLTBao.PBan,
                            MauSo = tDiep302.DLieu.TBao.DLTBao.MSo,
                            TenThongBao = tDiep302.DLieu.TBao.DLTBao.Ten,
                            DiaDanh = tDiep302.DLieu.TBao.DLTBao.DDanh,
                            TenCQTCapTren = tDiep302.DLieu.TBao.DLTBao.TCQTCTren,
                            TenCQTRaThongBao = tDiep302.DLieu.TBao.DLTBao.TCQT,
                            TenNguoiNopThue = tDiep302.DLieu.TBao.DLTBao.TNNT,
                            MaSoThue = tDiep302.DLieu.TBao.DLTBao.MST,
                            MaDonViQuanHeNganSach = tDiep302.DLieu.TBao.DLTBao.MDVQHNSach,
                            DiaChiNguoiNopThue = tDiep302.DLieu.TBao.DLTBao.DCNNT,
                            DiaChiThuDienTu = tDiep302.DLieu.TBao.DLTBao.DCTDTu,
                            ThoiHan = tDiep302.DLieu.TBao.DLTBao.THan,
                            Lan = tDiep302.DLieu.TBao.DLTBao.Lan,
                        });

                        if (tDiep302.DLieu.TBao.DLTBao.DSHDon != null)
                        {
                            var dSHDon = tDiep302.DLieu.TBao.DLTBao.DSHDon;
                            for (int i = 0; i < length; i++)
                            {
                                var dSHDonItem = dSHDon[i];

                                result.ThongDiepChiTiet2s.Add(new ThongDiepChiTiet2
                                {
                                    STT = i + 1,
                                    KyHieuMauSoHoaDon = dSHDonItem.KHMSHDon,
                                    KyHieuHoaDon = dSHDonItem.KHHDon,
                                    SoHoaDon = dSHDonItem.SHDon,
                                    NgayLap = DateTime.Parse(dSHDonItem.NLap),
                                    LoaiHoaDonDienTuApDung = dSHDonItem.LADHDDT.GetDescription(),
                                    LyDoCanRaSoat = dSHDonItem.LDo
                                });
                            }
                        }
                        break;
                    case (int)MLTDiep.TDTBHDDLSSot: //300
                        var tDiep300 = DataHelper.ConvertFileToObject<ViewModels.XML.ThongDiepGuiNhanCQT.TDiep>(fullFolderPath);
                        result.ThongDiepChiTiet1s.Add(new ThongDiepChiTiet1
                        {
                            PhienBan = tDiep300.DLieu.TBao.DLTBao.PBan,
                            MauSo = tDiep300.DLieu.TBao.DLTBao.MSo,
                            TenThongBao = tDiep300.DLieu.TBao.DLTBao.Ten,
                            LoaiThongBao = tDiep300.DLieu.TBao.DLTBao.Loai == 1 ? "Thông báo hủy/giải trình của NNT" : "Thông báo hủy/giải trình của NNT theo thông báo của CQT",
                            SoThongBao = tDiep300.DLieu.TBao.DLTBao.So,
                            NgayThongBaoCuaCQT = !string.IsNullOrEmpty(tDiep300.DLieu.TBao.DLTBao.NTBCCQT) ? DateTime.Parse(tDiep300.DLieu.TBao.DLTBao.NTBCCQT) : (DateTime?)null,
                            MaCuaCoQuanThue = tDiep300.DLieu.TBao.DLTBao.MCQT,
                            DiaDanh = tDiep300.DLieu.TBao.DLTBao.DDanh,
                            TenCQTRaThongBao = tDiep300.DLieu.TBao.DLTBao.TCQT,
                            TenNguoiNopThue = tDiep300.DLieu.TBao.DLTBao.TNNT,
                            MaSoThue = tDiep300.DLieu.TBao.DLTBao.MST,
                            NgayThongBao = !string.IsNullOrEmpty(tDiep300.DLieu.TBao.DLTBao.NTBao) ? DateTime.Parse(tDiep300.DLieu.TBao.DLTBao.NTBao) : (DateTime?)null,
                            MaDonViQuanHeNganSach = tDiep300.DLieu.TBao.DLTBao.MDVQHNSach,
                            ThoiGianGui = entity.NgayGui,
                            NgayCapNhat = entity.ModifyDate
                        });

                        if (tDiep300.DLieu.TBao.DLTBao.DSHDon != null)
                        {
                            var dSHDon = tDiep300.DLieu.TBao.DLTBao.DSHDon;
                            for (int i = 0; i < dSHDon.Count; i++)
                            {
                                var dSHDonItem = dSHDon[i];

                                result.ThongDiepChiTiet2s.Add(new ThongDiepChiTiet2
                                {
                                    STT = i + 1,
                                    KyHieuMauSoHoaDon = dSHDonItem.KHMSHDon,
                                    KyHieuHoaDon = dSHDonItem.KHHDon,
                                    MaCQTCap = dSHDonItem.MCQTCap,
                                    SoHoaDon = dSHDonItem.SHDon,
                                    NgayLap = DateTime.Parse(dSHDonItem.Ngay),
                                    TinhChatThongBao = ((TCTBao)dSHDonItem.TCTBao).GetDescription(),
                                    LoaiHoaDonDienTuApDung = ((LADHDDT)dSHDonItem.LADHDDT).GetDescription(),
                                    LyDoCanRaSoat = dSHDonItem.LDo
                                });
                            }
                        }
                        break;
                    case (int)MLTDiep.TDCBTHDLHDDDTDCQThue:
                        var tDiep400 = DataHelper.ConvertFileToObject<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.IV._2.TDiep>(fullFolderPath);
                        foreach (var it in tDiep400.DLieu)
                        {
                            result.ThongDiepChiTiet1s.Add(new ThongDiepChiTiet1
                            {
                                PhienBan = tDiep400.TTChung.PBan,
                                MauSo = it.DLBTHop.TTChung.MSo,
                                TenThongBao = it.DLBTHop.TTChung.Ten,
                                SoThongBao = it.DLBTHop.TTChung.SBTHDLieu.ToString(),
                                LoaiKyDuLieu = it.DLBTHop.TTChung.LKDLieu,
                                KyDuLieu = it.DLBTHop.TTChung.KDLieu,
                                LanDau = it.DLBTHop.TTChung.LDau == LDau.LanDau ? true : false,
                                BoSungLanThu = it.DLBTHop.TTChung.LDau == LDau.BoSung ? it.DLBTHop.TTChung.BSLThu : (int?)null,
                                NgayLap = !string.IsNullOrEmpty(it.DLBTHop.TTChung.NLap) ? DateTime.Parse(it.DLBTHop.TTChung.NLap) : (DateTime?)null,
                                TenNguoiNopThue = it.DLBTHop.TTChung.TNNT,
                                MaSoThue = it.DLBTHop.TTChung.MST,
                                HoaDonDatIn = it.DLBTHop.TTChung.HDDIn == HDDIn.HoaDonDienTu ? false : true,
                                LoaiHangHoa = it.DLBTHop.TTChung.LHHoa.GetDescription(),
                                ThoiGianGui = entity.NgayGui,
                                NgayCapNhat = entity.ModifyDate
                            });
                        }

                        break;
                    case (int)MLTDiep.TDCDLTVANUQCTQThue:
                        var tDiep999 = DataHelper.ConvertFileToObject<ViewModels.XML.QuyDinhKyThuatHDDT.PhanI.IV._6.TDiep>(fullFolderPath);

                        moTaLoi = string.Empty;
                        if (tDiep999.DLieu.TBao.DSLDo != null)
                        {
                            for (int j = 0; j < tDiep999.DLieu.TBao.DSLDo.Count; j++)
                            {
                                var dSLDKTNhanItem = tDiep999.DLieu.TBao.DSLDo[j];
                                moTaLoi += $"- {j + 1}. Mã lỗi: {dSLDKTNhanItem.MLoi}; Mô tả: {dSLDKTNhanItem.MTa}\n";
                            }

                        }
                        result.ThongDiepChiTiet1s.Add(new ThongDiepChiTiet1
                        {
                            PhienBan = tDiep999.TTChung.PBan,
                            TrangThaiXacNhanCuaCQT = tDiep999.DLieu.TBao.TTTNhan.GetDescription(),
                            MoTaLoi = moTaLoi,
                            ThoiGianGui = entity.NgayGui,
                            NgayCapNhat = entity.ModifyDate
                        });

                        break;
                    default:
                        break;
                }

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<FileReturn> ExportBangKeAsync(ThongDiepChungParams @params)
        {
            @params.PageSize = -1;
            var getAllPaging = await GetPagingThongDiepChungAsync(@params);
            var list = getAllPaging.Items;

            var hoSoHDDTVM = await _hoSoHDDTService.GetDetailAsync();

            string destPath = Path.Combine(_hostingEnvironment.WebRootPath, $"temp");
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }

            // Excel
            string _sample = $"docs/ThongDiep/BANG_KE_THONG_DIEP.xlsx";
            string _path_sample = Path.Combine(_hostingEnvironment.WebRootPath, _sample);

            FileInfo file = new FileInfo(_path_sample);
            using (ExcelPackage package = new ExcelPackage(file))
            {
                // Open sheet1
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int totalRows = list.Count;
                int begin_row = 7;

                worksheet.Cells[1, 1].Value = hoSoHDDTVM.TenDonVi;
                worksheet.Cells[2, 1].Value = hoSoHDDTVM.DiaChi;
                worksheet.Cells[4, 1].Value = @params.IsThongDiepGui == true ? "BẢNG KÊ THÔNG ĐIỆP GỬI" : "BẢNG KÊ THÔNG ĐIỆP NHẬN";

                // Add Row
                if (totalRows != 0)
                {
                    worksheet.InsertRow(begin_row + 1, totalRows - 1, begin_row);
                }
                // Fill data
                int idx = begin_row + (totalRows == 0 ? 1 : 0);
                for (int i = 0; i < list.Count; i++)
                {
                    var _it = list[i];

                    worksheet.Cells[idx, 1].Value = i + 1;
                    worksheet.Cells[idx, 2].Value = _it.PhienBan;
                    worksheet.Cells[idx, 3].Value = _it.MaNoiGui;
                    worksheet.Cells[idx, 4].Value = _it.MaNoiNhan;
                    worksheet.Cells[idx, 5].Value = _it.MaLoaiThongDiep;
                    worksheet.Cells[idx, 6].Value = _it.MaThongDiep;
                    worksheet.Cells[idx, 7].Value = _it.MaThongDiepThamChieu;
                    worksheet.Cells[idx, 8].Value = _it.MaSoThue;
                    worksheet.Cells[idx, 9].Value = _it.SoLuong;

                    idx += 1;
                }

                worksheet.Cells[idx, 2].Value = string.Format("Số dòng = {0}", totalRows);

                string fileName = $"BANG_KE_THONG_DIEP_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                string filePath = Path.Combine(destPath, fileName);
                package.SaveAs(new FileInfo(filePath));
                byte[] fileByte;

                if (@params.IsPrint == true)
                {
                    string pdfPath = Path.Combine(destPath, $"print-{DateTime.Now:yyyyMMddHHmmss}.pdf");
                    FileHelper.ConvertExcelToPDF(_hostingEnvironment.WebRootPath, filePath, pdfPath);
                    File.Delete(filePath);
                    fileByte = File.ReadAllBytes(pdfPath);
                    filePath = pdfPath;
                    File.Delete(filePath);
                }
                else
                {
                    fileByte = File.ReadAllBytes(filePath);
                    File.Delete(filePath);
                }

                return new FileReturn
                {
                    Bytes = fileByte,
                    ContentType = MimeTypes.GetMimeType(filePath),
                    FileName = Path.GetFileName(filePath)
                };
            }
        }

        public async Task<List<ToKhaiForBoKyHieuHoaDonViewModel>> GetListToKhaiFromBoKyHieuHoaDonAsync(ToKhaiParams toKhaiParams)
        {
            DateTime fromDate = DateTime.Parse(toKhaiParams.FromDate);
            DateTime toDate = DateTime.Parse(toKhaiParams.ToDate);

            var query = (from tk in _dataContext.ToKhaiDangKyThongTins
                         join tdg in _dataContext.ThongDiepChungs on tk.Id equals tdg.IdThamChieu
                         join tdn in _dataContext.ThongDiepChungs on tdg.MaThongDiep equals tdn.MaThongDiepThamChieu into tmpThongDiepNhans
                         from tdn in tmpThongDiepNhans.DefaultIfEmpty()
                         where tk.NhanUyNhiem == (toKhaiParams.UyNhiemLapHoaDon == UyNhiemLapHoaDon.DangKy) &&
                         tdg.NgayGui.Value.Date >= fromDate && tdg.NgayGui.Value.Date <= toDate && /*((tdg.TrangThaiGui == (int)TrangThaiGuiToKhaiDenCQT.ChoPhanHoi) || (tdg.TrangThaiGui == (int)TrangThaiGuiToKhaiDenCQT.ChapNhan)) &&*/
                         (tdg.TrangThaiGui != (int)TrangThaiGuiToKhaiDenCQT.ChuaGui) && (tdg.TrangThaiGui != (int)TrangThaiGuiToKhaiDenCQT.TuChoiTiepNhan) && (tdg.TrangThaiGui != (int)TrangThaiGuiToKhaiDenCQT.GuiLoi) && (tdg.TrangThaiGui != (int)TrangThaiGuiToKhaiDenCQT.KhongChapNhan)
                         orderby tdg.NgayGui descending
                         select new ToKhaiForBoKyHieuHoaDonViewModel
                         {
                             ToKhaiId = tk.Id,
                             ThongDiepId = tdg.ThongDiepChungId,
                             MaThongDiepGui = tdg.MaThongDiep,
                             ThoiGianGui = tdg.NgayGui,
                             MaThongDiepNhan = tdn != null ? tdn.MaThongDiep : string.Empty,
                             TrangThaiGui = tdg.TrangThaiGui,
                             TenTrangThaiGui = ((TrangThaiGuiToKhaiDenCQT)tdg.TrangThaiGui).GetDescription(),
                             ToKhaiKhongUyNhiem = tk.NhanUyNhiem ? null : DataHelper.ConvertObjectFromTKhai<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1.TKhai>(tk, _hostingEnvironment.WebRootPath),
                             ToKhaiUyNhiem = !tk.NhanUyNhiem ? null : DataHelper.ConvertObjectFromTKhai<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._2.TKhai>(tk, _hostingEnvironment.WebRootPath),
                         });

            if (toKhaiParams.UyNhiemLapHoaDon == UyNhiemLapHoaDon.KhongDangKy)
            {
                query = query.Where(x => x.ToKhaiKhongUyNhiem != null && (toKhaiParams.HinhThucHoaDon == (HinhThucHoaDon)x.ToKhaiKhongUyNhiem.DLTKhai.NDTKhai.HTHDon.CMa));

                switch (toKhaiParams.LoaiHoaDon)
                {
                    case LoaiHoaDon.HoaDonGTGT:
                        query = query.Where(x => x.ToKhaiKhongUyNhiem != null && x.ToKhaiKhongUyNhiem.DLTKhai.NDTKhai.LHDSDung.HDGTGT == 1);
                        break;
                    case LoaiHoaDon.HoaDonBanHang:
                        query = query.Where(x => x.ToKhaiKhongUyNhiem != null && x.ToKhaiKhongUyNhiem.DLTKhai.NDTKhai.LHDSDung.HDBHang == 1);
                        break;
                    case LoaiHoaDon.HoaDonBanTaiSanCong:
                        break;
                    case LoaiHoaDon.HoaDonBanHangDuTruQuocGia:
                        break;
                    case LoaiHoaDon.CacLoaiHoaDonKhac:
                        break;
                    case LoaiHoaDon.CacCTDuocInPhatHanhSuDungVaQuanLyNhuHD:
                        break;
                    default:
                        break;
                }
            }

            var result = await query.ToListAsync();

            foreach (var item in result)
            {
                if (item.ToKhaiUyNhiem != null)
                {
                    var dkun = item.ToKhaiUyNhiem.DLTKhai.NDTKhai.DSDKUNhiem.FirstOrDefault(x => x.KHMSHDon == toKhaiParams.kyHieuMauSoHoaDon && x.KHHDon == toKhaiParams.KyHieuHoaDon);
                    if (dkun != null)
                    {
                        item.STT = dkun.STT;
                        item.TenLoaiHoaDonUyNhiem = dkun.TLHDon;
                        item.KyHieuMauHoaDon = dkun.KHMSHDon;
                        item.KyHieuHoaDonUyNhiem = dkun.KHHDon;
                        item.TenToChucDuocUyNhiem = dkun.TTChuc;
                        item.MucDichUyNhiem = dkun.MDich;
                        item.ThoiGianUyNhiem = DateTime.Parse(dkun.DNgay);
                        item.PhuongThucThanhToan = (HTTToan)dkun.PThuc;
                        item.TenPhuongThucThanhToan = (((HTTToan)dkun.PThuc).GetDescription());
                    }
                }
            }

            return result;
        }
    }
}
