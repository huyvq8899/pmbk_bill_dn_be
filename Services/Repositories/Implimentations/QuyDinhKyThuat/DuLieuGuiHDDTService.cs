using AutoMapper;
using DLL;
using DLL.Constants;
using DLL.Entity;
using DLL.Entity.QuanLy;
using DLL.Entity.QuanLyHoaDon;
using DLL.Entity.QuyDinhKyThuat;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Newtonsoft.Json;
using Services.Enums;
using Services.Helper;
using Services.Helper.Constants;
using Services.Helper.Params.QuyDinhKyThuat;
using Services.Helper.XmlModel;
using Services.Repositories.Interfaces;
using Services.Repositories.Interfaces.DanhMuc;
using Services.Repositories.Interfaces.Pos;
using Services.Repositories.Interfaces.QuanLyHoaDon;
using Services.Repositories.Interfaces.QuyDinhKyThuat;
using Services.Repositories.Interfaces.TienIch;
using Services.ViewModels.Pos;
using Services.ViewModels.QuanLyHoaDonDienTu;
using Services.ViewModels.QuyDinhKyThuat;
using Services.ViewModels.TienIch;
using Services.ViewModels.XML;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Services.Repositories.Implimentations.QuyDinhKyThuat
{
    public class DuLieuGuiHDDTService : IDuLieuGuiHDDTService
    {
        const int MAX_SIZE = 2097152;

        private readonly Datacontext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMapper _mp;
        private readonly IXMLInvoiceService _xMLInvoiceService;
        private readonly ITVanService _ITVanService;
        private readonly IConfiguration _configuration;
        private readonly IHoSoHDDTService _hoSoHDDTService;
        private readonly IPosTransferService _posTransferService;
        private readonly INhatKyTruyCapService _nhatKyTruyCapService;

        public DuLieuGuiHDDTService(
            Datacontext dataContext,
            IHttpContextAccessor httpContextAccessor,
            IHostingEnvironment hostingEnvironment,
            IMapper mp,
            IXMLInvoiceService xMLInvoiceService,
            ITVanService ITVanService,
            IConfiguration configuration,
            IHoSoHDDTService hoSoHDDTService,
            INhatKyTruyCapService nhatKyTruyCapService,
            IPosTransferService posTransferService)
        {
            _db = dataContext;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _mp = mp;
            _xMLInvoiceService = xMLInvoiceService;
            _ITVanService = ITVanService;
            _configuration = configuration;
            _hoSoHDDTService = hoSoHDDTService;
            _posTransferService = posTransferService;
            _nhatKyTruyCapService = nhatKyTruyCapService;
        }

        public async Task<ThongDiepChungViewModel> GetByIdAsync(string id)
        {
            var query = from td in _db.DuLieuGuiHDDTs
                        join hddt in _db.HoaDonDienTus on td.HoaDonDienTuId equals hddt.HoaDonDienTuId into tmpHoaDonDienTus
                        from hddt in tmpHoaDonDienTus.DefaultIfEmpty()
                        join tdc in _db.ThongDiepChungs on td.DuLieuGuiHDDTId equals tdc.IdThamChieu
                        where td.DuLieuGuiHDDTId == id
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
                            CreatedBy = td.CreatedBy,
                            CreatedDate = td.CreatedDate,
                            Status = td.Status,
                            DuLieuGuiHDDT = new DuLieuGuiHDDTViewModel
                            {
                                DuLieuGuiHDDTId = td.DuLieuGuiHDDTId,
                                HoaDonDienTuId = td.HoaDonDienTuId,
                                HoaDonDienTu = hddt != null ? new HoaDonDienTuViewModel
                                {
                                    HoaDonDienTuId = hddt.HoaDonDienTuId,
                                    MauHoaDonId = hddt.MauHoaDonId,
                                    MauSo = hddt.MauSo,
                                    KyHieu = hddt.KyHieu,
                                    SoHoaDon = hddt.SoHoaDon,
                                    NgayHoaDon = hddt.NgayHoaDon
                                } : null,
                                DuLieuGuiHDDTChiTiets = (from tddl in _db.DuLieuGuiHDDTChiTiets
                                                         join hddt in _db.HoaDonDienTus on tddl.HoaDonDienTuId equals hddt.HoaDonDienTuId
                                                         where tddl.DuLieuGuiHDDTId == td.DuLieuGuiHDDTId
                                                         orderby tddl.CreatedDate
                                                         select new DuLieuGuiHDDTChiTietViewModel
                                                         {
                                                             ThongDiepGuiDuLieuHDDTChiTietId = tddl.DuLieuGuiHDDTChiTietId,
                                                             ThongDiepGuiDuLieuHDDTId = tddl.DuLieuGuiHDDTId,
                                                             HoaDonDienTuId = tddl.HoaDonDienTuId,
                                                             HoaDonDienTu = new HoaDonDienTuViewModel
                                                             {
                                                                 HoaDonDienTuId = hddt.HoaDonDienTuId,
                                                                 NgayHoaDon = hddt.NgayHoaDon,
                                                                 SoHoaDon = hddt.SoHoaDon,
                                                                 MauHoaDonId = hddt.MauHoaDonId,
                                                                 MauSo = hddt.MauSo,
                                                                 KyHieu = hddt.KyHieu,
                                                                 KhachHangId = hddt.KhachHangId,
                                                                 MaKhachHang = hddt.MaKhachHang,
                                                                 TenKhachHang = hddt.TenKhachHang,
                                                                 MaSoThue = hddt.MaSoThue,
                                                                 TongTienThanhToan = hddt.TongTienThanhToanQuyDoi,
                                                                 XMLChuaKy = hddt.XMLChuaKy,
                                                                 XMLDaKy = hddt.XMLDaKy
                                                             }
                                                         })
                                                        .ToList()
                            }
                        };

            var result = await query.FirstOrDefaultAsync();
            return result;
        }

        public byte[] GuiThongDiepKiemTraDuLieuHoaDon(ThongDiepParams @params)
        {
            /* Code cũ comment lại để sửa thẻ <LHDKMa> 
             * trong đó thẻ <DSHDon> có thẻ <HDon> áp dụng vào trường hợp nhận 204 cho 300
            
            // convert url xml to  `9 content xml
            XDocument xd = XDocument.Load(@params.FileUrl);
            xd.Descendants().Where(x => x.Name.LocalName == "Signature").Remove();

            XmlSerializer serialiser = new XmlSerializer(typeof(ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._7.TDiep));
            var model = (ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._7.TDiep)serialiser.Deserialize(xd.CreateReader());

            // thông điệp kết quả kiểm tra dữ liệu
            ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._8.TDiep tDiep = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._8.TDiep
            {
                TTChung = new TTChungThongDiep
                {
                    PBan = model.TTChung.PBan,
                    MNGui = model.TTChung.MNNhan,
                    MNNhan = model.TTChung.MNGui,
                    MLTDiep = ((int)MLTDiep.TDTBKQKTDLHDon).ToString(),
                    MTDiep = $"TCT{Guid.NewGuid().ToString().Replace("-", "").ToUpper()}",
                    MTDTChieu = model.TTChung.MTDiep,
                    MST = model.TTChung.MST,
                    SLuong = model.TTChung.SLuong
                },
                DLieu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._8.DLieu
                {
                    TBao = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._3.TBao
                    {
                        DLTBao = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._3.DLTBao
                        {
                            PBan = model.TTChung.PBan,
                            MSo = MSoThongBao.ThongBao10,
                            Ten = "Thành công",
                            So = "0000001",
                            DDanh = "TCT",
                            NTBao = DateTime.Now.ToString("yyyy-MM-dd"),
                            MST = model.TTChung.MST,
                            TNNT = "test",
                            LTBao = LTBao.ThongBao3,
                            CCu = "test",
                            LHDKMa = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._3.LHDKMa
                            {
                                DSHDon = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._3.HDonDSHDon>()
                            },
                        },
                        DSCKS = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._3.DSCKS()
                    }
                }
            };

            // fake lỗi
            int lengthListHD = model.DLieu.Count();
            int stt = 0;
            for (int i = 0; i < lengthListHD; i++)
            {
                stt += 1;
                var item = model.DLieu[i].DLHDon.TTChung;

                var hDon = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._3.HDonDSHDon
                {
                    STT = stt,
                    KHMSHDon = item.KHMSHDon,
                    KHHDon = item.KHHDon,
                    SHDon = item.SHDon,
                    NLap = item.NLap,
                };

                if (i % 2 == 0)
                {
                    hDon.DSLDo = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._3.LDo>
                    {
                        new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._3.LDo
                        {
                            MLoi = $"LOI{i}",
                            MTLoi = $"Lỗi test{i}",
                            HDXLy = "Fix bug",
                        }
                    };
                }

                tDiep.DLieu.TBao.DLTBao.LHDKMa.DSHDon.Add(hDon);
            }

            // convert model to byte xml
            var xmlFileFolder = Path.Combine(_hostingEnvironment.WebRootPath, $"temp/{Guid.NewGuid()}");
            if (!Directory.Exists(xmlFileFolder))
            {
                Directory.CreateDirectory(xmlFileFolder);
            }
            var xmlFilePath = Path.Combine(xmlFileFolder, $"{Guid.NewGuid()}.xml");
            _xMLInvoiceService.GenerateXML(tDiep, xmlFilePath);

            byte[] fileByte = File.ReadAllBytes(xmlFilePath);
            File.Delete(xmlFilePath);

            return fileByte;
            */
            return null;
        }

        public byte[] GuiThongDiepKiemTraKyThuat(ThongDiepParams @params)
        {
            // convert url xml to content xml
            XDocument xd = XDocument.Load(@params.FileUrl);
            xd.Descendants().Where(x => x.Name.LocalName == "Signature").Remove();

            XmlSerializer serialiser = new XmlSerializer(typeof(ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6.TDiep));
            var model = (ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6.TDiep)serialiser.Deserialize(xd.CreateReader());

            // thông điệp kết quả kiểm tra dữ liệu
            ViewModels.XML.QuyDinhKyThuatHDDT.PhanI.IV._6.TDiep tDiep = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanI.IV._6.TDiep
            {
                TTChung = new TTChungThongDiep
                {
                    PBan = model.TTChung.PBan,
                    MNGui = model.TTChung.MNNhan,
                    MNNhan = model.TTChung.MNGui,
                    MLTDiep = ((int)MLTDiep.TDTBKQKTDLHDon).ToString(),
                    MTDiep = $"TCT{Guid.NewGuid().ToString().Replace("-", "").ToUpper()}",
                    MTDTChieu = model.TTChung.MTDiep,
                    MST = model.TTChung.MST,
                    SLuong = model.TTChung.SLuong
                },
                DLieu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanI.IV._6.DLieu
                {
                    TBao = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanI.IV._6.TBao
                    {
                        MTDiep = model.TTChung.MTDiep,
                        NNhan = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"),
                        TTTNhan = TTTNhan.KhongLoi
                    }
                }
            };

            // convert model to byte xml
            var xmlFileFolder = Path.Combine(_hostingEnvironment.WebRootPath, $"temp/{Guid.NewGuid()}");
            if (!Directory.Exists(xmlFileFolder))
            {
                Directory.CreateDirectory(xmlFileFolder);
            }
            var xmlFilePath = Path.Combine(xmlFileFolder, $"{Guid.NewGuid()}.xml");
            _xMLInvoiceService.GenerateXML(tDiep, xmlFilePath);

            byte[] fileByte = File.ReadAllBytes(xmlFilePath);
            if (File.Exists(xmlFilePath))
            {
                File.Delete(xmlFilePath);
            }

            return fileByte;
        }

        /// <summary>
        /// Insert thong diep
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ThongDiepChungViewModel> InsertAsync(ThongDiepChungViewModel model)
        {
            // add du lieu gui hddt
            DuLieuGuiHDDT duLieuGuiHDDT = new DuLieuGuiHDDT
            {
                DuLieuGuiHDDTId = Guid.NewGuid().ToString(),
                HoaDonDienTuId = model.DuLieuGuiHDDT.HoaDonDienTuId,
            };
            await _db.DuLieuGuiHDDTs.AddAsync(duLieuGuiHDDT);

            model.ThongDiepChungId = Guid.NewGuid().ToString();

            // get MaThongDiep from HoaDon
            var fileHoaDon = await _db.FileDatas.AsNoTracking().FirstOrDefaultAsync(x => x.IsSigned == true && x.Type == 1 && x.RefId == model.DuLieuGuiHDDT.HoaDonDienTuId);
            if (fileHoaDon != null)
            {
                MemoryStream stream = new MemoryStream(fileHoaDon.Binary);
                XmlDocument doc = new XmlDocument();
                doc.Load(stream);
                var mTDiep = doc.SelectSingleNode("//TDiep/TTChung/MTDiep")?.InnerText;
                if (!string.IsNullOrEmpty(mTDiep))
                {
                    model.MaThongDiep = mTDiep;
                }

                string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string folderPath = $"FilesUpload/{databaseName}/{ManageFolderPath.XML_SIGNED}";
                string fullFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, folderPath);
                if (!Directory.Exists(fullFolderPath))
                {
                    Directory.CreateDirectory(fullFolderPath);
                }

                string fileName = $"TD-{Guid.NewGuid()}.xml";
                string filePath = Path.Combine(fullFolderPath, fileName);
                await File.WriteAllBytesAsync(filePath, fileHoaDon.Binary);

                // add filedata for thongdiep
                var fileData = new FileData
                {
                    RefId = model.ThongDiepChungId,
                    Type = 1,
                    IsSigned = true,
                    DateTime = DateTime.Now,
                    Binary = fileHoaDon.Binary,
                    FileName = fileName
                };
                await _db.FileDatas.AddAsync(fileData);
            }
            else if (!string.IsNullOrEmpty(model.Base64))
            {
                var contentPlain = TextHelper.Decompress(model.Base64);
                string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string folderPath = $"FilesUpload/{databaseName}/{ManageFolderPath.XML_SIGNED}";
                string fullFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, folderPath);
                if (!Directory.Exists(fullFolderPath))
                {
                    Directory.CreateDirectory(fullFolderPath);
                }

                string fileName = $"TD-{Guid.NewGuid()}.xml";
                string filePath = Path.Combine(fullFolderPath, fileName);
                await File.WriteAllTextAsync(filePath, contentPlain);

                var fileData = new FileData
                {
                    RefId = model.ThongDiepChungId,
                    Type = 1,
                    IsSigned = true,
                    DateTime = DateTime.Now,
                    Content = contentPlain,
                    Binary = File.ReadAllBytes(filePath),
                    FileName = fileName
                };
                await _db.FileDatas.AddAsync(fileData);
                await _db.SaveChangesAsync();
            }

            // add thongdiep 200 | 203 | 206
            model.IdThamChieu = duLieuGuiHDDT.DuLieuGuiHDDTId;
            model.NgayGui = DateTime.Now;
            model.HinhThuc = 0;
            model.TrangThaiGui = (int)TrangThaiGuiThongDiep.ChoPhanHoi;
            model.ThongDiepGuiDi = true;
            ThongDiepChung entity = _mp.Map<ThongDiepChung>(model);
            await _db.ThongDiepChungs.AddAsync(entity);
            await _db.SaveChangesAsync();
            ThongDiepChungViewModel result = _mp.Map<ThongDiepChungViewModel>(entity);

            // add thongdiep 999
            var thongDiep999 = new ThongDiepChung
            {
                ThongDiepChungId = Guid.NewGuid().ToString(),
                PhienBan = model.PhienBan,
                MaNoiGui = model.MaNoiNhan,
                MaNoiNhan = model.MaNoiGui,
                MaLoaiThongDiep = 999,
                MaThongDiepThamChieu = model.MaThongDiep,
                MaSoThue = model.MaSoThue,
                SoLuong = model.SoLuong,
                ThongDiepGuiDi = false,
                TrangThaiGui = (int)TrangThaiGuiThongDiep.ChoPhanHoi,
                Status = true,
            };
            await _db.ThongDiepChungs.AddAsync(thongDiep999);

            //#region create xml 200 | 203
            //string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            //string folderPath = $"FilesUpload/{databaseName}/{ManageFolderPath.XML_SIGNED}";
            //string fullFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, folderPath);
            //if (!Directory.Exists(fullFolderPath))
            //{
            //    Directory.CreateDirectory(fullFolderPath);
            //}

            //string fileName = $"TD-{Guid.NewGuid()}.xml";
            //string filePath = Path.Combine(fullFolderPath, fileName);
            //await _xMLInvoiceService.CreateQuyDinhKyThuatTheoMaLoaiThongDiep(filePath, model);
            //string xmlContent = File.ReadAllText(filePath);
            //#endregion

            await _db.SaveChangesAsync();
            return result;
        }

        public async Task<List<ThongDiepChungViewModel>> InsertThongDiep206(ThongDiepChungViewModel model)
        {
            //IQueryable<BoKyHieuHoaDon> listBoKyHieu = _db.BoKyHieuHoaDons.AsQueryable();
            try
            {
                //Thêm dữ liệu gửi hddt
                string userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var duLieuGuiHDDTs = new List<DuLieuGuiHDDT>();
                var listResult = new List<ThongDiepChungViewModel>();

                foreach (var item in model.HoaDons)
                {
                    duLieuGuiHDDTs.Add(new DuLieuGuiHDDT
                    {
                        DuLieuGuiHDDTId = Guid.NewGuid().ToString(),
                        HoaDonDienTuId = item.HoaDonDienTuId,
                    });
                }
                await _db.DuLieuGuiHDDTs.AddRangeAsync(duLieuGuiHDDTs);
                if (await _db.SaveChangesAsync() == duLieuGuiHDDTs.Count)
                {

                    var thongDiepGTGTId = Guid.NewGuid().ToString();
                    var thongDiepBanHangId = Guid.NewGuid().ToString();
                    var thongDiepKhacId = Guid.NewGuid().ToString();

                    //create xml
                    string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                    string folderPath = $"FilesUpload/{databaseName}/{ManageFolderPath.XML_SIGNED}";
                    string fullFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, folderPath);
                    if (!Directory.Exists(fullFolderPath))
                    {
                        Directory.CreateDirectory(fullFolderPath);
                    }

                    string fileGTGTName = $"TD-{thongDiepGTGTId}.xml";
                    string fileGTGTPath = Path.Combine(fullFolderPath, fileGTGTName);


                    string fileBHName = $"TD-{thongDiepBanHangId}.xml";
                    string fileBHPath = Path.Combine(fullFolderPath, fileBHName);

                    string fileKhacName = $"TD-{thongDiepBanHangId}.xml";
                    string fileKhacPath = Path.Combine(fullFolderPath, fileBHName);

                    model.TTChung1.SLuong = model.HoaDons.Count(x => x.LoaiHoaDon == (int)LoaiHoaDon.HoaDonGTGTCMTMTT);
                    model.TTChung2.SLuong = model.HoaDons.Count(x => x.LoaiHoaDon == (int)LoaiHoaDon.HoaDonBanHangCMTMTT);
                    model.TTChung3.SLuong = model.HoaDons.Count(x => x.LoaiHoaDon == (int)LoaiHoaDon.HoaDonKhacCMTMTT || x.LoaiHoaDon == (int)LoaiHoaDon.TemVeGTGT || x.LoaiHoaDon == (int)LoaiHoaDon.TemVeBanHang);

                    await _xMLInvoiceService.CreateThongDiepGuiHoaDonCMTMTT(fileGTGTPath, fileBHPath, fileKhacPath, model.HoaDons, model.TTChung1, model.TTChung2, model.TTChung3);

                    if (model.HoaDons.Any(x => x.LoaiHoaDon == (int)LoaiHoaDon.HoaDonGTGTCMTMTT))
                    {
                        if (new FileInfo(fileGTGTPath).Length <= MAX_SIZE)
                        {
                            var hoaDonGTGTIdss = model.HoaDons.Where(x => x.LoaiHoaDon == (int)LoaiHoaDon.HoaDonGTGTCMTMTT).Select(x => x.HoaDonDienTuId).ToList();

                            var thongDiepGTGT = new ThongDiepChungViewModel
                            {
                                ThongDiepChungId = thongDiepGTGTId,
                                PhienBan = model.TTChung1.PBan,
                                MaNoiGui = model.TTChung1.MNGui,
                                MaNoiNhan = model.TTChung1.MNNhan,
                                MaLoaiThongDiep = int.Parse(model.TTChung1.MLTDiep),
                                MaThongDiep = model.TTChung1.MTDiep,
                                MaThongDiepThamChieu = String.Empty,
                                MaSoThue = model.TTChung1.MST,
                                SoLuong = hoaDonGTGTIdss.Count,
                                ThongDiepGuiDi = true,
                                TrangThaiGui = (int)TrangThaiGuiThongDiep.ChoPhanHoi,
                                CreatedBy = userId,
                                CreatedDate = DateTime.Now,
                                ModifyBy = userId,
                                ModifyDate = DateTime.Now,
                                NgayGui = DateTime.Now
                            };

                            ThongDiepChung entity = _mp.Map<ThongDiepChung>(thongDiepGTGT);
                            await _db.ThongDiepChungs.AddAsync(entity);
                            if (await _db.SaveChangesAsync() > 0)
                            {
                                var duLieuGuiHDDTGTGTs = await _db.DuLieuGuiHDDTs.Where(x => hoaDonGTGTIdss.Contains(x.HoaDonDienTuId)).ToListAsync();
                                foreach (var item in duLieuGuiHDDTGTGTs)
                                {
                                    item.ThongDiepChungId = thongDiepGTGTId;
                                }

                                _db.UpdateRange(duLieuGuiHDDTGTGTs);
                                await _db.SaveChangesAsync();

                                var fileData = new FileData
                                {
                                    Type = 1,
                                    DateTime = DateTime.Now,
                                    Content = File.ReadAllText(fileGTGTPath),
                                    Binary = File.ReadAllBytes(fileGTGTPath),
                                    RefId = thongDiepGTGTId,
                                    IsSigned = false,
                                    FileName = fileGTGTName
                                };

                                await _db.FileDatas.AddAsync(fileData);
                                await _db.SaveChangesAsync();

                                thongDiepGTGT.DataXML = TextHelper.Compress(fileData.Content);

                                listResult.Add(thongDiepGTGT);
                            }
                        }
                    }

                    if (model.HoaDons.Any(x => x.LoaiHoaDon == (int)LoaiHoaDon.HoaDonBanHangCMTMTT))
                    {
                        if (new FileInfo(fileBHPath).Length <= MAX_SIZE)
                        {
                            var hoaDonBHIdss = model.HoaDons.Where(x => x.LoaiHoaDon == (int)LoaiHoaDon.HoaDonBanHangCMTMTT).Select(x => x.HoaDonDienTuId).ToList();

                            var thongDiepBH = new ThongDiepChungViewModel
                            {
                                ThongDiepChungId = thongDiepBanHangId,
                                PhienBan = model.TTChung2.PBan,
                                MaNoiGui = model.TTChung2.MNGui,
                                MaNoiNhan = model.TTChung2.MNNhan,
                                MaLoaiThongDiep = int.Parse(model.TTChung2.MLTDiep),
                                MaThongDiep = model.TTChung2.MTDiep,
                                MaThongDiepThamChieu = String.Empty,
                                MaSoThue = model.TTChung2.MST,
                                SoLuong = hoaDonBHIdss.Count,
                                ThongDiepGuiDi = true,
                                TrangThaiGui = (int)TrangThaiGuiThongDiep.ChoPhanHoi,
                                CreatedBy = userId,
                                CreatedDate = DateTime.Now,
                                ModifyBy = userId,
                                ModifyDate = DateTime.Now,
                                NgayGui = DateTime.Now
                            };

                            ThongDiepChung entity = _mp.Map<ThongDiepChung>(thongDiepBH);
                            await _db.ThongDiepChungs.AddAsync(entity);
                            if (await _db.SaveChangesAsync() > 0)
                            {
                                var duLieuGuiHDDTBHs = await _db.DuLieuGuiHDDTs.Where(x => hoaDonBHIdss.Contains(x.HoaDonDienTuId)).ToListAsync();
                                foreach (var item in duLieuGuiHDDTBHs)
                                {
                                    item.ThongDiepChungId = thongDiepBanHangId;
                                }

                                _db.UpdateRange(duLieuGuiHDDTBHs);
                                await _db.SaveChangesAsync();

                                var fileData = new FileData
                                {
                                    Type = 1,
                                    DateTime = DateTime.Now,
                                    Content = File.ReadAllText(fileBHPath),
                                    Binary = File.ReadAllBytes(fileBHPath),
                                    RefId = thongDiepBanHangId,
                                    IsSigned = false,
                                    FileName = fileBHName
                                };

                                await _db.FileDatas.AddAsync(fileData);
                                await _db.SaveChangesAsync();

                                thongDiepBH.DataXML = TextHelper.Compress(fileData.Content);

                                listResult.Add(thongDiepBH);
                            }
                        }
                    }

                    if (model.HoaDons.Any(x => x.LoaiHoaDon == (int)LoaiHoaDon.HoaDonKhacCMTMTT || x.LoaiHoaDon == (int)LoaiHoaDon.TemVeGTGT || x.LoaiHoaDon == (int)LoaiHoaDon.TemVeBanHang))
                    {
                        if (new FileInfo(fileKhacPath).Length <= MAX_SIZE)
                        {
                            var hoaDonKhacIds = model.HoaDons.Where(x => x.LoaiHoaDon == (int)LoaiHoaDon.HoaDonKhacCMTMTT || x.LoaiHoaDon == (int)LoaiHoaDon.TemVeGTGT || x.LoaiHoaDon == (int)LoaiHoaDon.TemVeBanHang).Select(x => x.HoaDonDienTuId).ToList();

                            var thongDiepKhac = new ThongDiepChungViewModel
                            {
                                ThongDiepChungId = thongDiepKhacId,
                                PhienBan = model.TTChung3.PBan,
                                MaNoiGui = model.TTChung3.MNGui,
                                MaNoiNhan = model.TTChung3.MNNhan,
                                MaLoaiThongDiep = int.Parse(model.TTChung3.MLTDiep),
                                MaThongDiep = model.TTChung3.MTDiep,
                                MaThongDiepThamChieu = String.Empty,
                                MaSoThue = model.TTChung3.MST,
                                SoLuong = hoaDonKhacIds.Count,
                                ThongDiepGuiDi = true,
                                TrangThaiGui = (int)TrangThaiGuiThongDiep.ChoPhanHoi,
                                CreatedBy = userId,
                                CreatedDate = DateTime.Now,
                                ModifyBy = userId,
                                ModifyDate = DateTime.Now,
                                NgayGui = DateTime.Now
                            };

                            ThongDiepChung entity = _mp.Map<ThongDiepChung>(thongDiepKhac);
                            await _db.ThongDiepChungs.AddAsync(entity);
                            if (await _db.SaveChangesAsync() > 0)
                            {
                                var duLieuGuiHDDTKhacs = await _db.DuLieuGuiHDDTs.Where(x => hoaDonKhacIds.Contains(x.HoaDonDienTuId)).ToListAsync();
                                foreach (var item in duLieuGuiHDDTKhacs)
                                {
                                    item.ThongDiepChungId = thongDiepKhacId;
                                }

                                _db.UpdateRange(duLieuGuiHDDTKhacs);
                                await _db.SaveChangesAsync();

                                var fileData = new FileData
                                {
                                    Type = 1,
                                    DateTime = DateTime.Now,
                                    Content = File.ReadAllText(fileKhacPath),
                                    Binary = File.ReadAllBytes(fileKhacPath),
                                    RefId = thongDiepKhacId,
                                    IsSigned = false,
                                    FileName = fileKhacName
                                };

                                await _db.FileDatas.AddAsync(fileData);
                                await _db.SaveChangesAsync();

                                thongDiepKhac.DataXML = TextHelper.Compress(fileData.Content);

                                listResult.Add(thongDiepKhac);
                            }
                        }
                    }

                }

                return listResult.Any() ? listResult : null;

            }
            catch (Exception ex)
            {
                Tracert.WriteLog(ex.Message);
            }

            return null;
        }

        public async Task<bool> LuuDuLieuDaKy(ThongDiepChungViewModel model)
        {
            try
            {
                var plain = TextHelper.Decompress(model.DataXML);
                var fileData = new FileData
                {
                    Type = 1,
                    DateTime = DateTime.Now,
                    IsSigned = true,
                    RefId = model.ThongDiepChungId,
                    Content = plain,
                    Binary = Encoding.UTF8.GetBytes(plain)
                };

                await _db.FileDatas.AddAsync(fileData);

                //cập nhật hóa đơn tương ứng
                var hoaDonDienTuIds = await _db.DuLieuGuiHDDTs.Where(x => x.ThongDiepChungId == model.ThongDiepChungId).Select(x => x.HoaDonDienTuId).ToListAsync();

                return await _db.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Tracert.WriteLog(ex.Message);
            }

            return false;
        }

        public async Task<List<ThongDiepChungViewModel>> InsertAsync2(List<ThongDiepChungViewModel> models)
        {
            string userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // add du lieu gui hddt
            List<DuLieuGuiHDDT> lstDuLieuGuiHDDTs = new List<DuLieuGuiHDDT>();
            foreach (var model in models)
            {
                DuLieuGuiHDDT duLieuGuiHDDT = new DuLieuGuiHDDT
                {
                    DuLieuGuiHDDTId = Guid.NewGuid().ToString(),
                    HoaDonDienTuId = model.DuLieuGuiHDDT.HoaDonDienTuId,
                    CreatedBy = userId,
                    CreatedDate = DateTime.Now,
                    ModifyBy = userId,
                    ModifyDate = DateTime.Now
                };
                model.IdThamChieu = duLieuGuiHDDT.DuLieuGuiHDDTId;
                model.ThongDiepChungId = Guid.NewGuid().ToString();

                lstDuLieuGuiHDDTs.Add(duLieuGuiHDDT);
            }
            // await _db.BulkInsertAsync(lstDuLieuGuiHDDTs);
            await _db.DuLieuGuiHDDTs.AddRangeAsync(lstDuLieuGuiHDDTs);

            // get MaThongDiep from HoaDon
            // Get list FileDatas
            List<string> lstHoaDonDienTuIds = models.Select(o => o.DuLieuGuiHDDT.HoaDonDienTuId).ToList();

            var fileHoaDons = await _db.FileDatas.AsNoTracking().Where(x => x.IsSigned == true && x.Type == 1 && lstHoaDonDienTuIds.Contains(x.RefId)).ToListAsync();

            List<FileData> lstFileDatas = new List<FileData>();
            List<ThongDiepChung> lstthongDiep999s = new List<ThongDiepChung>();
            List<ThongDiepChung> lstthongDiep200s = new List<ThongDiepChung>();
            List<TransferLog> lstTransferLog200203 = new List<TransferLog>();

            foreach (var fileHoaDon in fileHoaDons)
            {
                var model = models.Where(o => o.DuLieuGuiHDDT.HoaDonDienTuId == fileHoaDon.RefId).FirstOrDefault();
                if (model == null)
                {
                    continue;
                }

                MemoryStream stream = new MemoryStream(fileHoaDon.Binary);
                XmlDocument doc = new XmlDocument();
                doc.Load(stream);
                var mTDiep = doc.SelectSingleNode("//TDiep/TTChung/MTDiep")?.InnerText;
                if (!string.IsNullOrEmpty(mTDiep))
                {
                    model.MaThongDiep = mTDiep;
                }

                string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string folderPath = $"FilesUpload/{databaseName}/{ManageFolderPath.XML_SIGNED}";
                string fullFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, folderPath);
                if (!Directory.Exists(fullFolderPath))
                {
                    Directory.CreateDirectory(fullFolderPath);
                }

                string fileName = $"TD-{Guid.NewGuid()}.xml";
                string filePath = Path.Combine(fullFolderPath, fileName);
                await File.WriteAllBytesAsync(filePath, fileHoaDon.Binary);

                // Set for models
                model.DataXML = Encoding.UTF8.GetString(fileHoaDon.Binary);

                // add filedata for thongdiep
                var fileData = new FileData
                {
                    FileDataId = Guid.NewGuid().ToString(),
                    RefId = model.ThongDiepChungId,
                    Type = 1,
                    IsSigned = true,
                    DateTime = DateTime.Now,
                    Binary = fileHoaDon.Binary,
                    FileName = fileName
                };
                lstFileDatas.Add(fileData);

                // add thongdiep 200 | 203
                //model.IdThamChieu = duLieuGuiHDDT.DuLieuGuiHDDTId;
                model.NgayGui = DateTime.Now;
                model.HinhThuc = 0;
                model.TrangThaiGui = (int)TrangThaiGuiThongDiep.ChoPhanHoi;
                model.ThongDiepGuiDi = true;
                model.CreatedBy = userId;
                model.CreatedDate = DateTime.Now;
                model.ModifyBy = userId;
                model.ModifyDate = DateTime.Now;

                ThongDiepChung entity = _mp.Map<ThongDiepChung>(model);
                lstthongDiep200s.Add(entity);
                lstTransferLog200203.Add(new TransferLog
                {
                    TransferLogId = Guid.NewGuid().ToString(),
                    DateTime = DateTime.Now,
                    Type = 1,
                    MNGui = model.MaNoiGui,
                    MNNhan = model.MaNoiNhan,
                    MLTDiep = model.MaLoaiThongDiep,
                    MTDiep = model.MaThongDiep,
                    MTDTChieu = model.MaThongDiepThamChieu,
                    XMLData = model.DataXML
                });

                // add thongdiep 999
                var thongDiep999 = new ThongDiepChung
                {
                    ThongDiepChungId = Guid.NewGuid().ToString(),
                    PhienBan = model.PhienBan,
                    MaNoiGui = model.MaNoiNhan,
                    MaNoiNhan = model.MaNoiGui,
                    MaLoaiThongDiep = 999,
                    MaThongDiepThamChieu = model.MaThongDiep,
                    MaSoThue = model.MaSoThue,
                    SoLuong = model.SoLuong,
                    ThongDiepGuiDi = false,
                    TrangThaiGui = (int)TrangThaiGuiThongDiep.ChoPhanHoi,
                    Status = true,
                    CreatedBy = userId,
                    CreatedDate = DateTime.Now,
                    ModifyBy = userId,
                    ModifyDate = DateTime.Now
                };

                lstthongDiep999s.Add(thongDiep999);
            }

            //await _db.BulkInsertAsync(lstFileDatas);

            //await _db.BulkInsertAsync(lstthongDiep200s);

            //await _db.BulkInsertAsync(lstthongDiep999s);

            //await _db.BulkInsertAsync(lstTransferLog200203);

            await _db.FileDatas.AddRangeAsync(lstFileDatas);

            await _db.ThongDiepChungs.AddRangeAsync(lstthongDiep200s);

            await _db.ThongDiepChungs.AddRangeAsync(lstthongDiep999s);

            await _db.TransferLogs.AddRangeAsync(lstTransferLog200203);

            await _db.SaveChangesAsync();

            return models;
        }

        //public string CreateXMLBangTongHopDuLieu(BangTongHopDuLieuParams @params)
        //{
        //    string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
        //    string folderPath = $"FilesUpload/{databaseName}/{ManageFolderPath.XML_SIGNED}/unsigned";
        //    string fullFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, folderPath);
        //    if (!Directory.Exists(fullFolderPath))
        //    {
        //        Directory.CreateDirectory(fullFolderPath);
        //    }
        //    else
        //    {
        //        Directory.Delete(fullFolderPath, true);
        //        Directory.CreateDirectory(fullFolderPath);
        //    }

        //    string fileName = $"{Guid.NewGuid()}.xml";
        //    string filePath = Path.Combine(fullFolderPath, fileName);
        //    _xMLInvoiceService.CreateBangTongHopDuLieu(filePath, @params);
        //    return fileName;
        //}

        //public async Task<bool> GuiBangDuLieu(string XMLUrl, string thongDiepChungId, string maThongDiep, string mst)
        //{
        //    string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
        //    string assetsFolder = $"FilesUpload/{databaseName}/{ManageFolderPath.XML_SIGNED}";
        //    var fullXMLFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);

        //    var data = new GuiThongDiepData
        //    {
        //        MST = mst,
        //        MTDiep = maThongDiep,
        //        DataXML = File.ReadAllText(Path.Combine(fullXMLFolder, XMLUrl))
        //    };
        //    await _ITVanService.TVANSendData("api/report/send", data.DataXML);
        //    return true;
        //}

        //public async Task<List<TongHopDuLieuHoaDonGuiCQTViewModel>> GetDuLieuBangTongHopGuiDenCQT(BangTongHopParams @params)
        //{
        //    IQueryable<TongHopDuLieuHoaDonGuiCQTViewModel> query = null;
        //    query = from hd in _db.HoaDonDienTus
        //            join hdct in _db.HoaDonDienTuChiTiets on hd.HoaDonDienTuId equals hdct.HoaDonDienTuId into tmpHoaDons
        //            from hdct in tmpHoaDons.DefaultIfEmpty()
        //            join mhd in _db.BoKyHieuHoaDons on hd.BoKyHieuHoaDonId equals mhd.BoKyHieuHoaDonId into tmpMauHoaDons
        //            from mhd in tmpMauHoaDons.DefaultIfEmpty()
        //            join dvt in _db.DonViTinhs on hdct.DonViTinhId equals dvt.DonViTinhId into tmpDonViTinhs
        //            from dvt in tmpDonViTinhs.DefaultIfEmpty()
        //            where hd.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.DaKyDienTu && mhd.HinhThucHoaDon == (int)HinhThucHoaDon.KhongCoMa
        //            select new TongHopDuLieuHoaDonGuiCQTViewModel
        //            {
        //                MauSo = mhd.KyHieuMauSoHoaDon.ToString(),
        //                KyHieu = mhd.KyHieuHoaDon,
        //                SoHoaDon = hd.SoHoaDon,
        //                NgayHoaDon = hd.NgayHoaDon,
        //                MaSoThue = hd.MaSoThue,
        //                MaKhachHang = hd.MaKhachHang,
        //                TenKhachHang = hd.TenKhachHang,
        //                HoTenNguoiMuaHang = hd.HoTenNguoiMuaHang,
        //                MaHang = hdct.MaHang,
        //                TenHang = hdct.TenHang,
        //                SoLuong = hdct.SoLuong,
        //                DonViTinh = dvt.Ten ?? string.Empty,
        //                ThanhTien = hdct.ThanhTien,
        //                ThueGTGT = hdct.ThueGTGT,
        //                TienThueGTGT = hdct.TienThueGTGT,
        //                TongTienThanhToan = hdct.TongTienThanhToan,
        //                TrangThaiHoaDon = hd.TrangThai,
        //                TenTrangThaiHoaDon = ((TrangThaiHoaDon)hd.TrangThai).GetDescription(),
        //                MauSoHoaDonLienQuan = !string.IsNullOrEmpty(hd.DieuChinhChoHoaDonId) ?
        //                                       (_db.HoaDonDienTus.Any(x => x.HoaDonDienTuId == hd.DieuChinhChoHoaDonId) ?
        //                                       (from hd1 in _db.HoaDonDienTus
        //                                        join bkh in _db.BoKyHieuHoaDons on hd.BoKyHieuHoaDonId equals bkh.BoKyHieuHoaDonId
        //                                        where hd1.HoaDonDienTuId == hd.DieuChinhChoHoaDonId
        //                                        select bkh.KyHieuMauSoHoaDon.ToString()).FirstOrDefault()
        //                                        : (from hd1 in _db.ThongTinHoaDons
        //                                           where hd1.Id == hd.DieuChinhChoHoaDonId
        //                                           select hd1.MauSoHoaDon).FirstOrDefault()
        //                                        ) :
        //                                    !string.IsNullOrEmpty(hd.ThayTheChoHoaDonId) ?
        //                                    (_db.HoaDonDienTus.Any(x => x.HoaDonDienTuId == hd.ThayTheChoHoaDonId) ?
        //                                    (
        //                                    from hd1 in _db.HoaDonDienTus
        //                                    join bkh in _db.BoKyHieuHoaDons on hd.BoKyHieuHoaDonId equals bkh.BoKyHieuHoaDonId
        //                                    where hd1.HoaDonDienTuId == hd.ThayTheChoHoaDonId
        //                                    select bkh.KyHieuMauSoHoaDon.ToString()).FirstOrDefault()
        //                                    : (from hd1 in _db.ThongTinHoaDons
        //                                       where hd1.Id == hd.ThayTheChoHoaDonId
        //                                       select hd1.MauSoHoaDon).FirstOrDefault()
        //                                    )
        //                                    : null,
        //                KyHieuHoaDonLienQuan = !string.IsNullOrEmpty(hd.DieuChinhChoHoaDonId) ?
        //                                       (_db.HoaDonDienTus.Any(x => x.HoaDonDienTuId == hd.DieuChinhChoHoaDonId) ?
        //                                       (from hd1 in _db.HoaDonDienTus
        //                                        join bkh in _db.BoKyHieuHoaDons on hd.BoKyHieuHoaDonId equals bkh.BoKyHieuHoaDonId
        //                                        where hd1.HoaDonDienTuId == hd.DieuChinhChoHoaDonId
        //                                        select bkh.KyHieuHoaDon.ToString()).FirstOrDefault()
        //                                        : (from hd1 in _db.ThongTinHoaDons
        //                                           where hd1.Id == hd.DieuChinhChoHoaDonId
        //                                           select hd1.KyHieuHoaDon).FirstOrDefault()
        //                                        ) :
        //                                    !string.IsNullOrEmpty(hd.ThayTheChoHoaDonId) ?
        //                                    (_db.HoaDonDienTus.Any(x => x.HoaDonDienTuId == hd.ThayTheChoHoaDonId) ?
        //                                    (
        //                                    from hd1 in _db.HoaDonDienTus
        //                                    join bkh in _db.BoKyHieuHoaDons on hd.BoKyHieuHoaDonId equals bkh.BoKyHieuHoaDonId
        //                                    where hd1.HoaDonDienTuId == hd.ThayTheChoHoaDonId
        //                                    select bkh.KyHieuHoaDon.ToString()).FirstOrDefault()
        //                                    : (from hd1 in _db.ThongTinHoaDons
        //                                       where hd1.Id == hd.ThayTheChoHoaDonId
        //                                       select hd1.KyHieuHoaDon).FirstOrDefault()
        //                                    )
        //                                    : null,
        //                SoHoaDonLienQuan = !string.IsNullOrEmpty(hd.DieuChinhChoHoaDonId) ?
        //                                        (_db.HoaDonDienTus.Any(x => x.HoaDonDienTuId == hd.DieuChinhChoHoaDonId) ?
        //                                            (from hd1 in _db.HoaDonDienTus
        //                                             where hd1.HoaDonDienTuId == hd.DieuChinhChoHoaDonId
        //                                             select hd1.SoHoaDon).FirstOrDefault() :
        //                                             (from hd1 in _db.ThongTinHoaDons
        //                                              where hd1.Id == hd.DieuChinhChoHoaDonId
        //                                              select hd1.SoHoaDon).FirstOrDefault()) :
        //                                    !string.IsNullOrEmpty(hd.ThayTheChoHoaDonId) ?
        //                                        (_db.HoaDonDienTus.Any(x => x.HoaDonDienTuId == hd.ThayTheChoHoaDonId) ?
        //                                            (from hd1 in _db.HoaDonDienTus
        //                                             where hd1.HoaDonDienTuId == hd.ThayTheChoHoaDonId
        //                                             select hd1.SoHoaDon).FirstOrDefault() :
        //                                             (from hd1 in _db.ThongTinHoaDons
        //                                              where hd1.Id == hd.ThayTheChoHoaDonId
        //                                              select hd1.SoHoaDon).FirstOrDefault()) : null,
        //                NgayHoaDonLienQuan = !string.IsNullOrEmpty(hd.DieuChinhChoHoaDonId) ?
        //                                        (_db.HoaDonDienTus.Any(x => x.HoaDonDienTuId == hd.DieuChinhChoHoaDonId) ?
        //                                            (from hd1 in _db.HoaDonDienTus
        //                                             where hd1.HoaDonDienTuId == hd.DieuChinhChoHoaDonId
        //                                             select hd1.NgayHoaDon).FirstOrDefault() :
        //                                             (from hd1 in _db.ThongTinHoaDons
        //                                              where hd1.Id == hd.DieuChinhChoHoaDonId
        //                                              select hd1.NgayHoaDon).FirstOrDefault()) :
        //                                    !string.IsNullOrEmpty(hd.ThayTheChoHoaDonId) ?
        //                                        (_db.HoaDonDienTus.Any(x => x.HoaDonDienTuId == hd.ThayTheChoHoaDonId) ?
        //                                            (from hd1 in _db.HoaDonDienTus
        //                                             where hd1.HoaDonDienTuId == hd.ThayTheChoHoaDonId
        //                                             select hd1.NgayHoaDon).FirstOrDefault() :
        //                                             (from hd1 in _db.ThongTinHoaDons
        //                                              where hd1.Id == hd.ThayTheChoHoaDonId
        //                                              select hd1.NgayHoaDon).FirstOrDefault()) : null,
        //                LoaiApDungHoaDonLienQuan = !string.IsNullOrEmpty(hd.DieuChinhChoHoaDonId) ?
        //                                            (_db.HoaDonDienTus.Any(x => x.HoaDonDienTuId == hd.DieuChinhChoHoaDonId) ?
        //                                            (int)LADHDDT.HinhThuc1 :
        //                                            (from hd1 in _db.ThongTinHoaDons
        //                                             where hd1.Id == hd.DieuChinhChoHoaDonId
        //                                             select (int)hd1.HinhThucApDung).FirstOrDefault()
        //                                            ) : (!string.IsNullOrEmpty(hd.ThayTheChoHoaDonId) ?
        //                                            (_db.HoaDonDienTus.Any(x => x.HoaDonDienTuId == hd.ThayTheChoHoaDonId) ?
        //                                            (int)LADHDDT.HinhThuc1 :
        //                                            (from hd1 in _db.ThongTinHoaDons
        //                                             where hd1.Id == hd.ThayTheChoHoaDonId
        //                                             select (int)hd1.HinhThucApDung).FirstOrDefault()) : (int?)null)
        //            };

        //    query = query.GroupBy(x => new { x.MauSo, x.KyHieu, x.SoHoaDon, x.ThueGTGT })
        //        .Select(x => new TongHopDuLieuHoaDonGuiCQTViewModel
        //        {
        //            MauSo = x.Key.MauSo,
        //            KyHieu = x.Key.KyHieu,
        //            SoHoaDon = x.Key.SoHoaDon,
        //            NgayHoaDon = x.First().NgayHoaDon,
        //            MaSoThue = x.First().MaSoThue,
        //            MaKhachHang = x.First().MaKhachHang,
        //            TenKhachHang = x.First().TenKhachHang,
        //            HoTenNguoiMuaHang = x.First().HoTenNguoiMuaHang,
        //            TrangThaiHoaDon = x.First().TrangThaiHoaDon,
        //            TenTrangThaiHoaDon = x.First().TenTrangThaiHoaDon,
        //            SoLuong = x.Sum(o => o.SoLuong),
        //            TenHang = x.Select(o => o.TenHang).Distinct().Join(", "),
        //            ThueGTGT = x.Key.ThueGTGT.CheckIsInteger() ? $"{x.Key.ThueGTGT}%" : x.Key.ThueGTGT,
        //            ThanhTien = x.Sum(o => o.ThanhTien),
        //            TienThueGTGT = x.Sum(o => o.TienThueGTGT),
        //            TongTienThanhToan = x.Sum(o => o.TongTienThanhToan),
        //            MauSoHoaDonLienQuan = x.First().MauSoHoaDonLienQuan,
        //            KyHieuHoaDonLienQuan = x.First().KyHieuHoaDonLienQuan,
        //            SoHoaDonLienQuan = x.First().SoHoaDonLienQuan,
        //            NgayHoaDonLienQuan = x.First().NgayHoaDonLienQuan,
        //            LoaiApDungHoaDonLienQuan = x.First().LoaiApDungHoaDonLienQuan
        //        });


        //    if (!string.IsNullOrEmpty(@params.TuNgay) && !string.IsNullOrEmpty(@params.DenNgay))
        //    {
        //        DateTime fromDate = DateTime.ParseExact(@params.TuNgay, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        //        DateTime toDate = DateTime.ParseExact(@params.DenNgay + " 23:59:59", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

        //        query = query.Where(x => x.NgayHoaDon >= fromDate && x.NgayHoaDon <= toDate);
        //    }

        //    query = query.OrderBy(x => x.NgayHoaDon)
        //    .ThenBy(x => x.MauSo)
        //    .ThenBy(x => x.KyHieu)
        //    .ThenBy(x => int.Parse(x.SoHoaDon));
        //    var result = await query.ToListAsync();
        //    return result;
        //}

        //public string LuuDuLieuKy(string encodedContent, string thongDiepId)
        //{
        //    var fileName = Guid.NewGuid().ToString() + ".xml";
        //    var databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
        //    string assetsFolder = $"FilesUpload/{databaseName}/{ManageFolderPath.XML_SIGNED}";
        //    var fullFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);
        //    if (!Directory.Exists(fullFolder))
        //    {
        //        Directory.CreateDirectory(fullFolder);
        //    }

        //    var fullXMLFile = Path.Combine(fullFolder, fileName);
        //    File.WriteAllText(fullXMLFile, encodedContent);
        //    return fileName;
        //}

        public async Task<bool> UpdateAsync(DuLieuGuiHDDTViewModel model)
        {
            List<DuLieuGuiHDDTChiTietViewModel> duLieus = model.DuLieuGuiHDDTChiTiets;

            model.DuLieuGuiHDDTChiTiets = null;
            var entity = await _db.DuLieuGuiHDDTs.FirstOrDefaultAsync(x => x.DuLieuGuiHDDTId == model.DuLieuGuiHDDTId);
            _db.Entry(entity).CurrentValues.SetValues(model);

            var oldDuLieus = await _db.DuLieuGuiHDDTChiTiets.Where(x => x.DuLieuGuiHDDTId == model.DuLieuGuiHDDTId).ToListAsync();
            _db.DuLieuGuiHDDTChiTiets.RemoveRange(oldDuLieus);
            if (duLieus.Any())
            {
                foreach (var item in duLieus)
                {
                    item.Status = true;
                    item.CreatedDate = DateTime.Now;
                    item.ThongDiepGuiDuLieuHDDTId = entity.DuLieuGuiHDDTId;
                    var detail = _mp.Map<DuLieuGuiHDDTChiTiet>(item);
                    await _db.DuLieuGuiHDDTChiTiets.AddAsync(detail);
                }
            }

            bool result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<bool> UpdateTrangThaiGuiAsync(DuLieuGuiHDDTViewModel model)
        {
            var entity = await _db.DuLieuGuiHDDTs.FirstOrDefaultAsync(x => x.DuLieuGuiHDDTId == model.DuLieuGuiHDDTId);
            // entity.TrangThaiGui = model.TrangThaiGui;
            await _db.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Gửi thông điệp hóa đơn tới cqt
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TrangThaiQuyTrinh> GuiThongDiepDuLieuHDDTAsync(string id)
        {
            var fileData = await _db.FileDatas.AsNoTracking().FirstOrDefaultAsync(x => x.Type == 1 && x.RefId == id && x.IsSigned == true);

            // get xml content of thongdiep
            string fileBody = Encoding.UTF8.GetString(fileData.Binary);

            var status = TrangThaiQuyTrinh.GuiLoi;

            // Send to TVAN
            string strContent = await _ITVanService.TVANSendData("api/invoice/send", fileBody);
            if (!string.IsNullOrEmpty(strContent))
            {
                var tDiep999 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanI.IV._6.TDiep>(strContent);
                if (tDiep999.DLieu.TBao.TTTNhan == TTTNhan.KhongLoi)
                {
                    status = TrangThaiQuyTrinh.GuiKhongLoi;
                }

                // update thongdiep 999
                var tDiepChung999 = await _db.ThongDiepChungs.FirstOrDefaultAsync(x => x.MaLoaiThongDiep == 999 && x.MaThongDiepThamChieu == tDiep999.TTChung.MTDTChieu);
                if (tDiepChung999 != null)
                {
                    tDiepChung999.MaThongDiep = tDiep999.TTChung.MTDiep;
                    tDiepChung999.TrangThaiGui = tDiep999.DLieu.TBao.TTTNhan == (int)TTTNhan.KhongLoi ? (int)TrangThaiGuiThongDiep.GuiKhongLoi : (int)TrangThaiGuiThongDiep.GuiLoi;
                    tDiepChung999.NgayThongBao = DateTime.Now;

                    var fileData999 = new FileData
                    {
                        RefId = tDiepChung999.ThongDiepChungId,
                        Type = 1,
                        DateTime = DateTime.Now,
                        Binary = Encoding.UTF8.GetBytes(strContent),
                        Content = strContent,
                    };
                    await _db.FileDatas.AddAsync(fileData999);
                }
            }
            else
            {
                status = TrangThaiQuyTrinh.GuiTCTNLoi;
            }
       

            // set TrangThaiQuyTrinh cho HoaDon
            var hoaDonDienTuId = await (from dlghhdt in _db.DuLieuGuiHDDTs
                                        join tdg in _db.ThongDiepChungs on dlghhdt.DuLieuGuiHDDTId equals tdg.IdThamChieu
                                        where tdg.ThongDiepChungId == id
                                        select dlghhdt.HoaDonDienTuId).FirstOrDefaultAsync();
            var hoaDon = await _db.HoaDonDienTus.FirstOrDefaultAsync(x => x.HoaDonDienTuId == hoaDonDienTuId);
            if (hoaDon.TrangThaiQuyTrinh <= (int)TrangThaiQuyTrinh.GuiKhongLoi)
            {
                hoaDon.TrangThaiQuyTrinh = (int)status;

                // set TrangThaiGui cho ThongDiepGui
                TrangThaiGuiThongDiep trangThaiGui = TrangThaiGuiThongDiep.ChoPhanHoi;
                switch (status)
                {
                    case TrangThaiQuyTrinh.GuiTCTNLoi:
                        trangThaiGui = TrangThaiGuiThongDiep.GuiTCTNLoi;

                        break;
                    case TrangThaiQuyTrinh.GuiKhongLoi:
                        trangThaiGui = TrangThaiGuiThongDiep.GuiKhongLoi;
                        break;
                    case TrangThaiQuyTrinh.GuiLoi:
                        trangThaiGui = TrangThaiGuiThongDiep.GuiLoi;
                        break;
                    default:
                        break;
                 
                }

                var thongDiepGui = await _db.ThongDiepChungs.FirstOrDefaultAsync(x => x.ThongDiepChungId == id);
                thongDiepGui.TrangThaiGui = (int)trangThaiGui;
            }

            // save db
            await _db.SaveChangesAsync();

            return status;
        }

        public async Task<string> GuiThongDiepDuLieuHDDTAsync2(ThongDiepChungViewModel model, string token)
        {
            var status = TrangThaiQuyTrinh.GuiLoi;

            // Send to TVAN
            string strContent = await _ITVanService.TVANSendData2("api/invoice/send", model.DataXML, token);

            if (!string.IsNullOrEmpty(strContent))
            {
                try
                {
                    var tDiep999 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanI.IV._6.TDiep>(strContent);
                    if (tDiep999.DLieu.TBao.TTTNhan == TTTNhan.KhongLoi)
                    {
                        status = TrangThaiQuyTrinh.GuiKhongLoi;
                    }

                    model.MaNoiGui999 = tDiep999.TTChung.MNGui;
                    model.MaNoiNhan999 = tDiep999.TTChung.MNNhan;
                    model.MaLoaiThongDiep999 = tDiep999.TTChung.MLTDiep;
                    model.MaThongDiep999 = tDiep999.TTChung.MTDiep;
                    model.MaThongDiepThamChieu999 = tDiep999.TTChung.MTDTChieu;
                    model.DataXML999 = strContent;
                }
                catch
                {
                    status = TrangThaiQuyTrinh.GuiTCTNLoi;
                }
            }
            else
            {
                status = TrangThaiQuyTrinh.GuiTCTNLoi;
            }

            model.TrangThai = status;
            return strContent;
        }

        public async Task<List<TrangThaiQuyTrinh>> GuiThongDiepDuLieuHDDTAsync3(List<ThongDiepChungViewModel> models)
        {
            List<TrangThaiQuyTrinh> result = new List<TrangThaiQuyTrinh>();

            var lstFileData = new List<FileData>();
            var lstHoaDon = new List<HoaDonDienTu>();
            var lstThongDiepChung = new List<ThongDiepChung>();

            // set TrangThaiQuyTrinh cho HoaDon
            var hoaDonDienTuId = await (from dlghhdt in _db.DuLieuGuiHDDTs
                                        join tdg in _db.ThongDiepChungs on dlghhdt.DuLieuGuiHDDTId equals tdg.IdThamChieu
                                        where models.Select(x => x.ThongDiepChungId).Contains(tdg.ThongDiepChungId)
                                        select dlghhdt.HoaDonDienTuId).ToListAsync();

            var hoaDons = await _db.HoaDonDienTus.Where(x => hoaDonDienTuId.Contains(x.HoaDonDienTuId)).ToListAsync();
            var thongDiepGuis = await _db.ThongDiepChungs
                .Where(x => models.Select(y => y.ThongDiepChungId).Contains(x.ThongDiepChungId) || (models.Select(y => y.MaThongDiep).Contains(x.MaThongDiepThamChieu) && x.MaLoaiThongDiep == 999))
                .ToListAsync();

            foreach (var model in models)
            {
                var status = TrangThaiQuyTrinh.GuiLoi;

                if (!string.IsNullOrEmpty(model.DataXML999))
                {
                    var tDiep999 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanI.IV._6.TDiep>(model.DataXML999);
                    if (tDiep999.DLieu.TBao.TTTNhan == TTTNhan.KhongLoi)
                    {
                        status = TrangThaiQuyTrinh.GuiKhongLoi;
                    }

                    // update thongdiep 999
                    var tDiepChung999 = thongDiepGuis.FirstOrDefault(x => x.MaLoaiThongDiep == 999 && x.MaThongDiepThamChieu == tDiep999.TTChung.MTDTChieu);
                    if (tDiepChung999 != null)
                    {
                        tDiepChung999.MaThongDiep = tDiep999.TTChung.MTDiep;
                        tDiepChung999.TrangThaiGui = tDiep999.DLieu.TBao.TTTNhan == (int)TTTNhan.KhongLoi ? (int)TrangThaiGuiThongDiep.GuiKhongLoi : (int)TrangThaiGuiThongDiep.GuiLoi;
                        tDiepChung999.NgayThongBao = DateTime.Now;

                        var fileData999 = new FileData
                        {
                            FileDataId = Guid.NewGuid().ToString(),
                            RefId = tDiepChung999.ThongDiepChungId,
                            Type = 1,
                            DateTime = DateTime.Now,
                            Binary = Encoding.UTF8.GetBytes(model.DataXML999),
                            Content = model.DataXML999,
                        };

                        lstFileData.Add(fileData999);
                        lstThongDiepChung.Add(tDiepChung999);
                    }
                }
                else
                {
                    status = TrangThaiQuyTrinh.GuiTCTNLoi;
                }

                var hoaDon = hoaDons.FirstOrDefault(x => x.HoaDonDienTuId == model.DuLieuGuiHDDT.HoaDonDienTuId);
                if (hoaDon.TrangThaiQuyTrinh <= (int)TrangThaiQuyTrinh.GuiKhongLoi)
                {
                    hoaDon.TrangThaiQuyTrinh = (int)status;

                    // set TrangThaiGui cho ThongDiepGui
                    TrangThaiGuiThongDiep trangThaiGui = TrangThaiGuiThongDiep.ChoPhanHoi;
                    switch (status)
                    {
                        case TrangThaiQuyTrinh.GuiTCTNLoi:
                            trangThaiGui = TrangThaiGuiThongDiep.GuiTCTNLoi;
                            break;
                        case TrangThaiQuyTrinh.GuiKhongLoi:
                            trangThaiGui = TrangThaiGuiThongDiep.GuiKhongLoi;
                            break;
                        case TrangThaiQuyTrinh.GuiLoi:
                            trangThaiGui = TrangThaiGuiThongDiep.GuiLoi;
                            break;
                        default:
                            break;
                    }

                    var thongDiepGui = thongDiepGuis.FirstOrDefault(x => x.ThongDiepChungId == model.ThongDiepChungId);
                    thongDiepGui.TrangThaiGui = (int)trangThaiGui;

                    lstHoaDon.Add(hoaDon);
                    lstThongDiepChung.Add(thongDiepGui);
                }

                result.Add(status);
            }

            //await _db.BulkInsertAsync(lstFileData);

            //await _db.BulkUpdateAsync(lstHoaDon);

            //await _db.BulkUpdateAsync(lstThongDiepChung);

            await _db.FileDatas.AddRangeAsync(lstFileData);

            await _db.SaveChangesAsync();

            return result;
        }

        /// <summary>
        /// Gửi thông điệp 206 đến cqt
        /// </summary>
        /// <param name="ThongDiepId">Id Thông điệp</param>
        /// <returns></returns>
        public async Task<bool> GuiThongDiepDuLieuHDDTAsync4(string ThongDiepId,bool isGuiVe = false)
        {
            try
            {
                List<ResultHoaDonMTTViewModels> ListHoaDonToPost = new List<ResultHoaDonMTTViewModels>();
                var fileData = await _db.FileDatas.Where(x => x.Type == 1 && x.RefId == ThongDiepId && x.IsSigned == true).FirstOrDefaultAsync();

                // get xml content of thongdiep
                string fileBody = Encoding.UTF8.GetString(fileData.Binary);

                var status = TrangThaiQuyTrinh.GuiLoi;

                // Send to TVAN
                string strContent = await _ITVanService.TVANSendData("api/invoice/send206", fileBody);
                if (!string.IsNullOrEmpty(strContent))
                {
                    var tDiep999 = DataHelper.ConvertObjectFromPlainContent<ViewModels.XML.QuyDinhKyThuatHDDT.PhanI.IV._6.TDiep>(strContent);
                    if (tDiep999.DLieu.TBao.TTTNhan == TTTNhan.KhongLoi)
                    {
                        status = TrangThaiQuyTrinh.GuiKhongLoi;
                    }

                    // update thongdiep 999
                    var tDiepChung999 = new ThongDiepChung();
                    tDiepChung999.ThongDiepChungId = Guid.NewGuid().ToString();
                    tDiepChung999.PhienBan = tDiep999.TTChung.PBan;
                    tDiepChung999.MaThongDiep = tDiep999.TTChung.MTDiep;
                    tDiepChung999.MaThongDiepThamChieu = tDiep999.TTChung.MTDTChieu;
                    tDiepChung999.TrangThaiGui = tDiep999.DLieu.TBao.TTTNhan == (int)TTTNhan.KhongLoi ? (int)TrangThaiGuiThongDiep.GuiKhongLoi : (int)TrangThaiGuiThongDiep.GuiLoi;
                    tDiepChung999.NgayThongBao = DateTime.Now;
                    tDiepChung999.MaLoaiThongDiep = int.Parse(tDiep999.TTChung.MLTDiep);
                    tDiepChung999.MaNoiGui = tDiep999.TTChung.MNGui;
                    tDiepChung999.MaNoiNhan = tDiep999.TTChung.MNNhan;
                    tDiepChung999.MaSoThue = tDiep999.TTChung.MST;
                    tDiepChung999.SoLuong = tDiep999.TTChung.SLuong;

                    await _db.ThongDiepChungs.AddAsync(tDiepChung999);
                    await _db.SaveChangesAsync();

                    var fileData999 = new FileData
                    {
                        RefId = tDiepChung999.ThongDiepChungId,
                        Type = 1,
                        DateTime = DateTime.Now,
                        Binary = Encoding.UTF8.GetBytes(strContent),
                        Content = strContent,
                    };
                    await _db.FileDatas.AddAsync(fileData999);
                }
                else
                {
                    status = TrangThaiQuyTrinh.GuiTCTNLoi;
                }
                ResultHoaDonMTTViewModels HoaDonToPost = new ResultHoaDonMTTViewModels();
                // set TrangThaiQuyTrinh cho HoaDon
                var hoaDonDienTuIds = await (from dlghhdt in _db.DuLieuGuiHDDTs
                                             join tdg in _db.ThongDiepChungs on dlghhdt.ThongDiepChungId equals tdg.ThongDiepChungId
                                             where tdg.ThongDiepChungId == ThongDiepId
                                             select dlghhdt.HoaDonDienTuId).ToListAsync();
                var hoaDons = await _db.HoaDonDienTus.Where(x => hoaDonDienTuIds.Contains(x.HoaDonDienTuId)).ToListAsync();
                TrangThaiGuiThongDiep trangThaiGui = TrangThaiGuiThongDiep.ChoPhanHoi;

                //cập nhật trạng thái quy trình cho hóa đơn
                foreach (var hoaDon in hoaDons)
                {
                    if (hoaDon.TrangThaiQuyTrinh <= (int)TrangThaiQuyTrinh.GuiKhongLoi || hoaDon.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.ChuaGuiCQT)
                    {
                        hoaDon.TrangThaiQuyTrinh = (int)status;
                        hoaDon.NgayKy = DateTime.Now;

                        HoaDonToPost.MaTraCuu = hoaDon.HoaDonDienTuId;
                        HoaDonToPost.TrangThaiQuyTrinh = (int)status;
                        HoaDonToPost.SoHoaDon = (long)hoaDon.SoHoaDon;
                        HoaDonToPost.TrangThaiHoaDon = hoaDon.TrangThai;
                        HoaDonToPost.BienLaiId = hoaDon.BienLaiId;
                        HoaDonToPost.PosCustomerURL = hoaDon.PosCustomerURL;
                        HoaDonToPost.Error = "";
                        ListHoaDonToPost.Add(HoaDonToPost);
                    }
                }

                _db.HoaDonDienTus.UpdateRange(hoaDons);

                // set TrangThaiGui cho ThongDiepGui
                switch (status)
                {
                    case TrangThaiQuyTrinh.GuiTCTNLoi:
                        trangThaiGui = TrangThaiGuiThongDiep.GuiTCTNLoi;
                        break;
                    case TrangThaiQuyTrinh.GuiKhongLoi:
                        trangThaiGui = TrangThaiGuiThongDiep.GuiKhongLoi;
                        break;
                    case TrangThaiQuyTrinh.GuiLoi:
                        trangThaiGui = TrangThaiGuiThongDiep.GuiLoi;
                        break;
                    default:
                        break;
                }

                var thongDiepGui = await _db.ThongDiepChungs.FirstOrDefaultAsync(x => x.ThongDiepChungId == ThongDiepId);
                thongDiepGui.TrangThaiGui = (int)trangThaiGui;
                thongDiepGui.NgayGui = DateTime.Now;
                _db.ThongDiepChungs.Update(thongDiepGui);

                await _nhatKyTruyCapService.InsertAsync(new NhatKyTruyCapViewModel
                {
                    LoaiHanhDong = hoaDons.Count()>1 ? LoaiHanhDong.GuiCqtDongLoat : LoaiHanhDong.GuiCQT,
                    DoiTuongThaoTac = "empty",
                    RefType = RefType.HoaDonDienTuMTT,
                    ThamChieu = string.Format("{1} Gửi CQT {0} hóa đơn", hoaDons.Count(),isGuiVe ? "HỆ THỐNG TỰ ĐỘNG" : string.Empty),
                    MoTaChiTiet = $"- Gửi CQT thành công {hoaDons.Count()} hóa đơn",
                    RefId = ThongDiepId
                });
                var rs = await _db.SaveChangesAsync() > 0;
                // save db
                if (rs)
                {
                    /// Send Trang Thai To Post
                    if (ListHoaDonToPost.Count() > 0 && isGuiVe == false)
                    {
                        await _posTransferService.SendResponseTCTToPos(ListHoaDonToPost);
                    }
                }
                return rs;

            }
            catch (Exception ex)
            {
                Tracert.WriteLog(ex.Message);
                return false;
            }
        }

        public FileReturn CreateThongDiepPhanHoi(ThongDiepPhanHoiParams model)
        {
            var xmlContent = DataHelper.Base64Decode(model.DataXML);
            byte[] encodedString = Encoding.UTF8.GetBytes(xmlContent);
            MemoryStream ms = new MemoryStream(encodedString);
            ms.Flush();
            ms.Position = 0;
            using (StreamReader reader = new StreamReader(ms))
            {
                XDocument xDoc = XDocument.Load(reader);
                xDoc.Descendants().Where(x => x.Name.LocalName == "Signature").Remove();

                XmlSerializer serialiser = null;

                var xmlFileFolder = Path.Combine(_hostingEnvironment.WebRootPath, $"temp");
                if (!Directory.Exists(xmlFileFolder))
                {
                    Directory.CreateDirectory(xmlFileFolder);
                }
                string fileName = $"{Guid.NewGuid()}.xml";
                string filePath = Path.Combine(xmlFileFolder, fileName);

                switch (model.MLTDiep)
                {
                    case (int)MLTDiep.TDGHDDTTCQTCapMa: // 200
                        serialiser = new XmlSerializer(typeof(ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6.TDiep));
                        var tDiep200 = (ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6.TDiep)serialiser.Deserialize(xDoc.CreateReader());

                        switch (model.MLTDiepPhanHoi)
                        {
                            case (int)MLTDiep.TDCDLTVANUQCTQThue: // 999
                                // create xml model
                                var tDiep999 = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanI.IV._6.TDiep
                                {
                                    TTChung = new TTChungThongDiep
                                    {
                                        PBan = tDiep200.TTChung.PBan,
                                        MNGui = tDiep200.TTChung.MNNhan,
                                        MNNhan = tDiep200.TTChung.MNGui,
                                        MLTDiep = ((int)MLTDiep.TDCDLTVANUQCTQThue).ToString(),
                                        MTDiep = $"TCT{Guid.NewGuid().ToString().Replace("-", "").ToUpper()}",
                                        MTDTChieu = tDiep200.TTChung.MTDiep,
                                        MST = tDiep200.TTChung.MST,
                                        SLuong = 0
                                    },
                                    DLieu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanI.IV._6.DLieu
                                    {
                                        TBao = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanI.IV._6.TBao
                                        {
                                            MTDiep = tDiep200.TTChung.MTDiep,
                                            NNhan = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"),
                                            TTTNhan = TTTNhan.KhongLoi
                                        }
                                    }
                                };
                                // create xml
                                _xMLInvoiceService.GenerateXML(tDiep999, filePath);
                                break;
                            case (int)MLTDiep.TDTBKQKTDLHDon: // 204
                                // create xml model
                                var tDiep204 = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._8.TDiep
                                {
                                    TTChung = new TTChungThongDiep
                                    {
                                        PBan = tDiep200.TTChung.PBan,
                                        MNGui = tDiep200.TTChung.MNNhan,
                                        MNNhan = tDiep200.TTChung.MNGui,
                                        MLTDiep = ((int)MLTDiep.TDTBKQKTDLHDon).ToString(),
                                        MTDiep = $"TCT{Guid.NewGuid().ToString().Replace("-", "").ToUpper()}",
                                        MTDTChieu = tDiep200.TTChung.MTDiep,
                                        MST = tDiep200.TTChung.MST,
                                        SLuong = tDiep200.TTChung.SLuong
                                    },
                                    DLieu = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._8.DLieu
                                    {
                                        TBao = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._3.TBao
                                        {
                                            DLTBao = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._3.DLTBao
                                            {
                                                PBan = tDiep200.TTChung.PBan,
                                                MSo = MSoThongBao.ThongBao10,
                                                Ten = "Thành công",
                                                So = "0000001",
                                                DDanh = "TCT",
                                                NTBao = DateTime.Now.ToString("yyyy-MM-dd"),
                                                MST = tDiep200.TTChung.MST,
                                                TNNT = "test",
                                                LTBao = LTBao.ThongBao2,
                                                CCu = "test",
                                            },
                                            DSCKS = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._3.DSCKS()
                                        }
                                    }
                                };
                                // create xml
                                _xMLInvoiceService.GenerateXML(tDiep204, filePath);
                                break;
                            case (int)MLTDiep.TBKQCMHDon: // 202
                                // create xml model
                                var tDiep202 = new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6.TDiep
                                {
                                    TTChung = new TTChungThongDiep
                                    {
                                        PBan = tDiep200.TTChung.PBan,
                                        MNGui = tDiep200.TTChung.MNNhan,
                                        MNNhan = tDiep200.TTChung.MNGui,
                                        MLTDiep = ((int)MLTDiep.TBKQCMHDon).ToString(),
                                        MTDiep = $"TCT{Guid.NewGuid().ToString().Replace("-", "").ToUpper()}",
                                        MTDTChieu = tDiep200.TTChung.MTDiep,
                                        MST = tDiep200.TTChung.MST,
                                        SLuong = tDiep200.TTChung.SLuong
                                    },
                                };
                                // create xml
                                _xMLInvoiceService.GenerateXML(tDiep202, filePath);

                                XmlDocument xml = new XmlDocument();
                                xml.Load(filePath);
                                xml.DocumentElement.AppendChild(xml.CreateElement(nameof(tDiep202.DLieu)));

                                XmlDocument xmlHoaDon = new XmlDocument();

                                using (StringWriter writer = new StringWriter())
                                {
                                    xDoc.Save(writer);
                                    var result = writer.ToString();

                                    xmlHoaDon.LoadXml(result);
                                    var hoaDonElement = xmlHoaDon.SelectSingleNode($"//TDiep/{nameof(tDiep202.DLieu)}/{nameof(tDiep202.DLieu.HDon)}");
                                    var importNode = xml.ImportNode(hoaDonElement, true);
                                    xml.DocumentElement[nameof(tDiep202.DLieu)].AppendChild(importNode);
                                }

                                xml.Save(filePath);
                                break;
                            default:
                                break;
                        }
                        break;
                    case (int)MLTDiep.TDCDLHDKMDCQThue: // 203
                        ///////////////////////////////////////
                        break;
                    case (int)MLTDiep.TDTBHDDLSSot: // 300
                        ///////////////////////////////////////
                        break;
                    default:
                        break;
                }

                // create bytes from path
                if (File.Exists(filePath))
                {
                    byte[] fileByte = File.ReadAllBytes(filePath);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }

                    return new FileReturn
                    {
                        Bytes = fileByte,
                        ContentType = MimeTypes.GetMimeType(filePath),
                        FileName = Path.GetFileName(filePath)
                    };
                }

                return null;
            }
        }

        public async Task<PagedList<ThongDiepChungViewModel>> GetByHoaDonDienTuIdAsync(ThongDiepChungParams @params)
        {
            var hd = await _db.HoaDonDienTus.FirstOrDefaultAsync(x => x.HoaDonDienTuId == @params.Keyword);
            var refIdXoaBos = await _db.HoaDonDienTus.Where(x => x.RefIdHoaDonXoaBoLanDau == @params.Keyword).Select(x => x.HoaDonDienTuId).ToListAsync();

            if (@params.LoaiThongDiep == 300 || @params.LoaiThongDiep == 303)
            {
                var query0 = from tdCQT in _db.ThongDiepChiTietGuiCQTs
                             where tdCQT.ThongDiepGuiCQTId == @params.Keyword
                             select new ThongDiepChungViewModel
                             {
                                 Key = tdCQT.HoaDonDienTuId,
                             };
                var list0 = await query0.ToListAsync();

                return PagedList<ThongDiepChungViewModel>
                 .CreateAsyncWithList(list0, @params.PageNumber, @params.PageSize);
            }

            var query = (from td in _db.DuLieuGuiHDDTs
                         join hddt in _db.HoaDonDienTus on td.HoaDonDienTuId equals hddt.HoaDonDienTuId into tmpHoaDonDienTus
                         from hddt in tmpHoaDonDienTus.DefaultIfEmpty()
                         join tdc in _db.ThongDiepChungs on td.DuLieuGuiHDDTId equals tdc.IdThamChieu
                         where td.HoaDonDienTuId == @params.Keyword || refIdXoaBos.Contains(td.HoaDonDienTuId)
                         select new ThongDiepChungViewModel
                         {

                             Key = Guid.NewGuid().ToString(),
                             ThongDiepChungId = tdc.ThongDiepChungId,
                             PhienBan = tdc.PhienBan,
                             MaNoiGui = tdc.MaNoiGui,
                             MaNoiNhan = tdc.MaNoiNhan,
                             MaLoaiThongDiep = tdc.MaLoaiThongDiep,
                             MaThongDiep = tdc.MaThongDiep,
                             MaThongDiepThamChieu = tdc.MaThongDiepThamChieu,
                             MaThongDiepPhanHoi = tdc.MaThongDiepPhanHoi,
                             MaSoThue = tdc.MaSoThue,
                             SoLuong = tdc.SoLuong,
                             CreatedBy = tdc.CreatedBy,
                             CreatedDate = tdc.CreatedDate,
                             ThongDiepGuiDi = tdc.ThongDiepGuiDi,
                             NgayGui = tdc.NgayGui,
                             NguoiThucHien = _db.Users.FirstOrDefault(x => x.UserId == (tdc.CreatedBy)).UserName ?? "Hệ thống",
                             NgayThongBao = tdc.NgayThongBao,
                             FileXML = _db.TransferLogs.FirstOrDefault(x => x.MTDiep == (tdc.MaThongDiep)).XMLData,
                             Status = td.Status,
                             TrangThaiGui = (TrangThaiGuiThongDiep)(tdc.TrangThaiGui),
                             TenTrangThaiThongBao = ((TrangThaiGuiThongDiep)(tdc.TrangThaiGui)).GetDescription(),

                         });

            var query1 = (from td in _db.DuLieuGuiHDDTs
                         join hddt in _db.HoaDonDienTus on td.HoaDonDienTuId equals hddt.HoaDonDienTuId into tmpHoaDonDienTus
                         from hddt in tmpHoaDonDienTus.DefaultIfEmpty()
                         join tdc in _db.ThongDiepChungs on td.ThongDiepChungId equals tdc.ThongDiepChungId
                         where td.HoaDonDienTuId == @params.Keyword || refIdXoaBos.Contains(td.HoaDonDienTuId)
                         select new ThongDiepChungViewModel
                         {

                             Key = Guid.NewGuid().ToString(),
                             ThongDiepChungId = tdc.ThongDiepChungId,
                             PhienBan = tdc.PhienBan,
                             MaNoiGui = tdc.MaNoiGui,
                             MaNoiNhan = tdc.MaNoiNhan,
                             MaLoaiThongDiep = tdc.MaLoaiThongDiep,
                             MaThongDiep = tdc.MaThongDiep,
                             MaThongDiepThamChieu = tdc.MaThongDiepThamChieu,
                             MaThongDiepPhanHoi = tdc.MaThongDiepPhanHoi,
                             MaSoThue = tdc.MaSoThue,
                             SoLuong = tdc.SoLuong,
                             CreatedBy = tdc.CreatedBy,
                             CreatedDate = tdc.CreatedDate,
                             ThongDiepGuiDi = tdc.ThongDiepGuiDi,
                             NgayGui = tdc.NgayGui,
                             NguoiThucHien = _db.Users.FirstOrDefault(x => x.UserId == (tdc.CreatedBy)).UserName ?? "Hệ thống",
                             NgayThongBao = tdc.NgayThongBao,
                             FileXML = _db.TransferLogs.FirstOrDefault(x => x.MTDiep == (tdc.MaThongDiep)).XMLData,
                             Status = td.Status,
                             TrangThaiGui = (TrangThaiGuiThongDiep)(tdc.TrangThaiGui),
                             TenTrangThaiThongBao = ((TrangThaiGuiThongDiep)(tdc.TrangThaiGui)).GetDescription(),

                         });

            var query2 = (from tdCQT in _db.ThongDiepChiTietGuiCQTs
                          join tdgCQT in _db.ThongDiepGuiCQTs on tdCQT.ThongDiepGuiCQTId equals tdgCQT.Id
                          join tdc2 in _db.ThongDiepChungs on tdgCQT.Id equals tdc2.IdThamChieu
                          where tdCQT.HoaDonDienTuId == @params.Keyword || refIdXoaBos.Contains(tdCQT.HoaDonDienTuId)
                          select new ThongDiepChungViewModel
                          {

                              Key = Guid.NewGuid().ToString(),
                              ThongDiepChungId = tdc2.ThongDiepChungId,
                              PhienBan = tdc2.PhienBan,
                              MaNoiGui = tdc2.MaNoiGui,
                              MaNoiNhan = tdc2.MaNoiNhan,
                              MaLoaiThongDiep = tdc2.MaLoaiThongDiep,
                              MaThongDiep = tdc2.MaThongDiep,
                              MaThongDiepThamChieu = tdc2.MaThongDiepThamChieu,
                              MaThongDiepPhanHoi = tdc2.MaThongDiepPhanHoi,
                              MaSoThue = tdc2.MaSoThue,
                              SoLuong = tdc2.SoLuong,
                              CreatedBy = tdCQT.CreatedBy,
                              CreatedDate = tdCQT.CreatedDate,
                              ThongDiepGuiDi = tdc2.ThongDiepGuiDi,
                              NgayGui = tdc2.CreatedDate,
                              NguoiThucHien = _db.Users.FirstOrDefault(x => x.UserId == tdc2.CreatedBy).UserName ?? "Hệ thống",
                              NgayThongBao = tdc2.NgayThongBao,
                              FileXML = _db.TransferLogs.FirstOrDefault(x => x.MTDiep == tdc2.MaThongDiep).XMLData,
                              TrangThaiGui = (TrangThaiGuiThongDiep)tdc2.TrangThaiGui,
                              TenTrangThaiThongBao = ((TrangThaiGuiThongDiep)tdc2.TrangThaiGui).GetDescription(),

                          });

            var query3 = (from bth in _db.BangTongHopDuLieuHoaDons
                          join ct in _db.BangTongHopDuLieuHoaDonChiTiets on bth.Id equals ct.BangTongHopDuLieuHoaDonId
                          join tdc in _db.ThongDiepChungs on bth.ThongDiepChungId equals tdc.ThongDiepChungId into thongDiepChungTmp
                          from tdc in thongDiepChungTmp.DefaultIfEmpty()
                          where ct.SoHoaDon == hd.SoHoaDon && ct.KyHieu == hd.KyHieu && ct.MauSo.ToString() == hd.MauSo && ct.NgayHoaDon == hd.NgayHoaDon
                          select new ThongDiepChungViewModel
                          {
                              Key = Guid.NewGuid().ToString(),
                              ThongDiepChungId = tdc.ThongDiepChungId,
                              PhienBan = tdc.PhienBan,
                              MaNoiGui = tdc.MaNoiGui,
                              MaNoiNhan = tdc.MaNoiNhan,
                              MaLoaiThongDiep = tdc.MaLoaiThongDiep,
                              MaThongDiep = tdc.MaThongDiep,
                              MaThongDiepThamChieu = tdc.MaThongDiepThamChieu,
                              MaThongDiepPhanHoi = tdc.MaThongDiepPhanHoi,
                              MaSoThue = tdc.MaSoThue,
                              SoLuong = tdc.SoLuong,
                              CreatedBy = tdc.CreatedBy,
                              CreatedDate = tdc.CreatedDate,
                              ThongDiepGuiDi = tdc.ThongDiepGuiDi,
                              NgayGui = tdc.NgayGui,
                              NguoiThucHien = _db.Users.FirstOrDefault(x => x.UserId == tdc.CreatedBy).UserName ?? "Hệ thống",
                              NgayThongBao = tdc.NgayThongBao,
                              FileXML = _db.TransferLogs.FirstOrDefault(x => x.MTDiep == tdc.MaThongDiep).XMLData,
                              Status = tdc.Status,
                              TrangThaiGui = (TrangThaiGuiThongDiep)tdc.TrangThaiGui,
                              TenTrangThaiThongBao = ((TrangThaiGuiThongDiep)tdc.TrangThaiGui).GetDescription(),

                          });



            var list = await query.OrderByDescending(x => x.NgayGui).ToListAsync();
            var list1 = await query1.OrderByDescending(x => x.NgayGui).ToListAsync();
            var list2 = await query2.OrderByDescending(x => x.NgayGui).ToListAsync();
            var list3 = await query3.OrderByDescending(x => x.NgayGui).ToListAsync();

            if (list.Count == 0) { list = list1; list1 = null; }

            if (list1 != null && list1.Count > 0)
            {
                foreach(var item in list1)
                {
                    list.Add(item);
                }
            }
            else if (list2.Count > 0)
            {
                foreach (var item in list2)
                {
                    list.Add(item);
                }
            }else if (list3.Count > 0)
            {
                foreach (var item in list3)
                {
                    list.Add(item);
                }
            }
            //else list = await query.OrderByDescending(x => x.NgayGui).ToListAsync();

            list = list.OrderByDescending(x => x.NgayGui).ToList();

            #region Filter and Sort

            if (!string.IsNullOrEmpty(@params.SortKey))
            {

                if (@params.SortKey == "NgayGui" && @params.SortValue == "ascend")
                {
                    list = list.OrderBy(x => x.SoLuong).ToList();
                }
                if (@params.SortKey == "NgayGui" && @params.SortValue == "descend")
                {
                    list = list.OrderByDescending(x => x.SoLuong).ToList();
                }

            }
            #endregion

            foreach (var item in list)
            {
                IQueryable<ThongDiepChungViewModel> queryTranslogs = from tl in _db.TransferLogs
                                                                     where tl.MTDTChieu == item.MaThongDiep
                                                                     let ac = TransferLogHelper.GetThongTinThongBao(tl.XMLData, tl.MLTDiep)
                                                                     select new ThongDiepChungViewModel
                                                                     {
                                                                         Key = Guid.NewGuid().ToString(),
                                                                         MaNoiGui = tl.MNGui,
                                                                         MaNoiNhan = tl.MNNhan,
                                                                         MaLoaiThongDiep = tl.MLTDiep,
                                                                         MaThongDiep = tl.MTDiep,
                                                                         MaThongDiepThamChieu = tl.MTDTChieu,
                                                                         NgayGui = tl.DateTime,
                                                                         NgayThongBao = tl.DateTime,
                                                                         FileXML = tl.XMLData,
                                                                         Type = tl.Type.ToString(),
                                                                         MGDDTu = ac.MGDDTu,
                                                                         SoTBao = ac.SoTBao,
                                                                         NgayTBao = ac.NgayTBao,
                                                                         MSoTBao = ac.MSoTBao,
                                                                         NguoiThucHien = tl.Type == 2 ? "TCT" : "TCTN",
                                                                     };
                IQueryable<ThongDiepChungViewModel> queryThongDiepChungs = from tdc2 in _db.ThongDiepChungs
                                                                           where tdc2.MaThongDiepThamChieu == item.MaThongDiep
                                                                           select new ThongDiepChungViewModel
                                                                           {
                                                                               Key = Guid.NewGuid().ToString(),
                                                                               MaThongDiep = tdc2.MaThongDiep,
                                                                               ThongDiepChungId = tdc2.ThongDiepChungId,
                                                                               PhienBan = tdc2.PhienBan,
                                                                               ThongDiepGuiDi = tdc2.ThongDiepGuiDi,
                                                                               TrangThaiGui = (TrangThaiGuiThongDiep)tdc2.TrangThaiGui,
                                                                               TenTrangThaiThongBao = ((TrangThaiGuiThongDiep)tdc2.TrangThaiGui).GetDescription(),
                                                                           };
                var listTranlogs = queryTranslogs.DistinctBy(x => x.MaThongDiep).OrderByDescending(x => x.NgayGui).ToList();
                var listThongDiepChungs = queryThongDiepChungs.OrderByDescending(x => x.NgayGui).ToList();
                #region Filter and Sort

                if (!string.IsNullOrEmpty(@params.SortKey))
                {

                    if (@params.SortKey == "NgayGui" && @params.SortValue == "ascend")
                    {
                        listTranlogs = listTranlogs.OrderBy(x => x.NgayGui).ToList();
                        listThongDiepChungs = listThongDiepChungs.OrderBy(x => x.NgayGui).ToList();
                    }
                    if (@params.SortKey == "NgayGui" && @params.SortValue == "descend")
                    {
                        listTranlogs = listTranlogs.OrderByDescending(x => x.NgayGui).ToList();
                        listThongDiepChungs = listThongDiepChungs.OrderByDescending(x => x.NgayGui).ToList();
                    }
                }
                #endregion
                foreach (var tl in listTranlogs)
                {
                    foreach (var tdc in listThongDiepChungs)
                    {
                        if (tl.MaThongDiep == tdc.MaThongDiep)
                        {
                            tl.ThongDiepChungId = tdc.ThongDiepChungId;
                            tl.ThongDiepGuiDi = tdc.ThongDiepGuiDi;
                            tl.TrangThaiGui = tdc.TrangThaiGui;
                            tl.TenTrangThaiThongBao = tdc.TenTrangThaiThongBao;
                        }
                    }
                }

                item.Children = listTranlogs;

            }

            return PagedList<ThongDiepChungViewModel>
                 .CreateAsyncWithList(list, @params.PageNumber, @params.PageSize);
        }
        public async Task<ThongDiepChungViewModel> GetAllThongDiepTraVeInTransLogsAsync(string maThongDiep)
        {
            IQueryable<ThongDiepChungViewModel> query = null;

            query = from tl in _db.TransferLogs
                    where tl.MTDiep == maThongDiep
                    select new ThongDiepChungViewModel
                    {
                        MaLoaiThongDiep = tl.MLTDiep,
                        NgayGui = tl.DateTime,
                        NgayThongBao = tl.DateTime,
                        FileXML = tl.XMLData,
                        Type = tl.Type.ToString(),
                    };


            return await query.OrderByDescending(x => x.NgayGui).FirstOrDefaultAsync();
        }

        public List<ThongDiepChungViewModel> GetThongDiepTraVeInTransLogsAsync(string maThongDiep)
        {
            IQueryable<ThongDiepChungViewModel> query = null;
            IQueryable<ThongDiepChungViewModel> query2 = null;

            query = from tdc in _db.ThongDiepChungs
                    join tl in _db.TransferLogs on tdc.MaThongDiep equals tl.MTDiep into tmpTrans
                    from tl in tmpTrans.DefaultIfEmpty()
                    join tl_tcn in _db.TransferLogs on tdc.ThongDiepChungId equals tl_tcn.MTDiep into tmpTrans_TCN
                    from tl_tcn in tmpTrans_TCN.DefaultIfEmpty()
                    where (tdc.MaThongDiep == maThongDiep || tdc.ThongDiepChungId == maThongDiep)
                    let ac = TransferLogHelper.GetThongTinThongBao(tl.XMLData ?? (tl_tcn.XMLData ?? string.Empty), tdc.MaLoaiThongDiep)
                    select new ThongDiepChungViewModel
                    {
                        Key = Guid.NewGuid().ToString(),
                        ThongDiepChungId = tdc.ThongDiepChungId,
                        PhienBan = tdc.PhienBan,
                        MaNoiGui = tdc.MaNoiGui,
                        MaNoiNhan = tdc.MaNoiNhan,
                        MaLoaiThongDiep = tdc.MaLoaiThongDiep,
                        MaThongDiep = tdc.MaThongDiep,
                        MaThongDiepThamChieu = tdc.MaThongDiepThamChieu,
                        MaThongDiepPhanHoi = tdc.MaThongDiepPhanHoi,
                        MaSoThue = tdc.MaSoThue,
                        SoLuong = tdc.SoLuong,
                        CreatedBy = tdc.CreatedBy,
                        CreatedDate = tdc.CreatedDate,
                        ThongDiepGuiDi = tdc.ThongDiepGuiDi,
                        NgayGui = tdc.NgayGui,
                        NgayThongBao = tdc.NgayThongBao,
                        FileXML = tl.XMLData ?? (tl_tcn.XMLData ?? string.Empty),
                        MGDDTu = ac.MGDDTu,
                        SoTBao = ac.SoTBao,
                        NgayTBao = ac.NgayTBao,
                        MSoTBao = ac.MSoTBao,
                        Status = tdc.Status,
                        TrangThaiGui = (TrangThaiGuiThongDiep)tdc.TrangThaiGui,
                        TenTrangThaiThongBao = ((TrangThaiGuiThongDiep)tdc.TrangThaiGui).GetDescription(),
                        NguoiThucHien = _db.Users.FirstOrDefault(x => x.UserId == tdc.ModifyBy).UserName,
                        IdThamChieu = tdc.IdThamChieu,
                    };
            query2 = from tdc in _db.ThongDiepChungs
                     join tl in _db.TransferLogs on tdc.MaThongDiep equals tl.MTDiep into tmpTrans
                     from tl in tmpTrans.DefaultIfEmpty()
                     join tl_tcn in _db.TransferLogs on tdc.MaThongDiepThamChieu equals tl_tcn.MTDTChieu into tmpTrans_TCN
                     from tl_tcn in tmpTrans_TCN.DefaultIfEmpty()
                     let ac = TransferLogHelper.GetThongTinThongBao(tl.XMLData ?? (tl_tcn.XMLData ?? string.Empty), tdc.MaLoaiThongDiep)
                     where tdc.MaThongDiepThamChieu == maThongDiep
                     select new ThongDiepChungViewModel
                     {
                         Key = Guid.NewGuid().ToString(),
                         ThongDiepChungId = tdc.ThongDiepChungId,
                         PhienBan = tdc.PhienBan,
                         MaNoiGui = tdc.MaNoiGui,
                         MaNoiNhan = tdc.MaNoiNhan,
                         MaLoaiThongDiep = tdc.MaLoaiThongDiep,
                         MaThongDiep = tdc.MaThongDiep,
                         MaThongDiepThamChieu = tdc.MaThongDiepThamChieu,
                         MaThongDiepPhanHoi = tdc.MaThongDiepPhanHoi,
                         MaSoThue = tdc.MaSoThue,
                         SoLuong = tdc.SoLuong,
                         CreatedBy = tdc.CreatedBy,
                         CreatedDate = tdc.CreatedDate,
                         ThongDiepGuiDi = tdc.ThongDiepGuiDi,
                         NgayGui = tl.DateTime,
                         NgayThongBao = tdc.NgayThongBao,
                         FileXML = tl.XMLData ?? (tl_tcn.XMLData),
                         MGDDTu = ac.MGDDTu,
                         SoTBao = ac.SoTBao,
                         NgayTBao = ac.NgayTBao,
                         MSoTBao = ac.MSoTBao,
                         Status = tdc.Status,
                         TrangThaiGui = (TrangThaiGuiThongDiep)tdc.TrangThaiGui,
                         TenTrangThaiThongBao = ((TrangThaiGuiThongDiep)tdc.TrangThaiGui).GetDescription(),
                         NguoiThucHien = tl.Type == 2 ? "TCT" : "TCTN",
                         IdThongBao = GetIdThongBaoFromXMLData(tl_tcn.XMLData),
                         IdThamChieu = tdc.IdThamChieu,
                     };

            var listCha = query.DistinctBy(x => x.MaThongDiep).OrderByDescending(x => x.NgayGui).ToList();
            var listCon = query2.DistinctBy(x => x.MaThongDiep).OrderByDescending(x => x.NgayGui).ToList();
            foreach (var item in listCha)
            {
                List<ThongDiepChungViewModel> listconByMTD = new List<ThongDiepChungViewModel>();
                foreach (var con in listCon)
                {
                    if (con.ThongDiepGuiDi == false && con.MaLoaiThongDiep == (int)MLTDiep.TDTBHDDTCRSoat)
                    {
                        var timeExpired = con.ThoiHan.HasValue ? con.NgayThongBao.Value.AddHours(con.ThoiHan.Value) : con.NgayThongBao.Value.AddDays(2);
                        var tDiepPhanHoi = _db.ThongDiepChungs.FirstOrDefault(x => x.MaLoaiThongDiep == (int)MLTDiep.TDTBHDDLSSot && x.MaThongDiepThamChieu == item.MaThongDiep);
                        if (tDiepPhanHoi == null || !tDiepPhanHoi.NgayGui.HasValue)
                        {
                            if (DateTime.Now <= timeExpired)
                            {
                                con.TrangThaiGui = TrangThaiGuiThongDiep.TrongHanVaChuaGiaiTrinh;
                            }
                            else
                            {
                                con.TrangThaiGui = TrangThaiGuiThongDiep.QuaHanVaChuaGiaiTrinh;
                            }
                        }
                        else
                        {
                            if (DateTime.Now <= timeExpired)
                            {
                                con.TrangThaiGui = TrangThaiGuiThongDiep.DaGiaiTrinhKhiTrongHan;
                            }
                            else
                            {
                                con.TrangThaiGui = TrangThaiGuiThongDiep.DaGiaiTrinhKhiQuaHan;
                            }
                        }


                        con.TenTrangThaiGui = con.TrangThaiGui.GetDescription();
                        var entity = _db.ThongDiepChungs.FirstOrDefault(x => x.ThongDiepChungId == con.ThongDiepChungId);
                        entity.TrangThaiGui = (int)con.TrangThaiGui;
                        _db.ThongDiepChungs.Update(entity);
                        _db.SaveChanges();
                    }

                    if (item.MaThongDiep == con.MaThongDiepThamChieu || item.ThongDiepChungId == con.MaThongDiepThamChieu)
                    {
                        con.Parent = item;
                        listconByMTD.Add(con);
                    }
                }
                item.Children = listconByMTD;
            }

            return listCha;
        }

        /// <summary>
        /// send background thong diep khong ma to CQT
        /// </summary>
        /// <returns></returns>
        public async Task<string> GuiThongDiepDuLieuHDDTBackgroundAsync()
        {
            string result = "";
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;

            var hoSoHDDT = await _hoSoHDDTService.GetDetailAsync();

            // create folder xml
            string folderPath = $"FilesUpload/{databaseName}/{ManageFolderPath.XML_SIGNED}";
            string fullFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, folderPath);
            if (!Directory.Exists(fullFolderPath))
            {
                Directory.CreateDirectory(fullFolderPath);
            }

            // get list hóa đơn id không có mã chưa gửi cqt theo phương thức từng hóa đơn
            var hoaDonKhongMaChuaGuiCQTIds = await (from hddt in _db.HoaDonDienTus
                                                    join bkhhd in _db.BoKyHieuHoaDons on hddt.BoKyHieuHoaDonId equals bkhhd.BoKyHieuHoaDonId
                                                    join dlghddt in _db.DuLieuGuiHDDTs on hddt.HoaDonDienTuId equals dlghddt.HoaDonDienTuId into tmpDuLieuGuiHDDTs
                                                    from dlghddt in tmpDuLieuGuiHDDTs.DefaultIfEmpty()
                                                    where bkhhd.PhuongThucChuyenDL == PhuongThucChuyenDL.CDDu && bkhhd.HinhThucHoaDon == HinhThucHoaDon.KhongCoMa &&
                                                    ((hddt.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.DaKyDienTu) || (hddt.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.GuiTCTNLoi) || (hddt.TrangThaiQuyTrinh == (int)TrangThaiQuyTrinh.GuiLoi)) &&
                                                    (hddt.TrangThai != (int)TrangThaiHoaDon.HoaDonXoaBo) && dlghddt == null
                                                    orderby hddt.SoHoaDon, bkhhd.KyHieu
                                                    select hddt.HoaDonDienTuId).Distinct().ToListAsync();

            if (hoaDonKhongMaChuaGuiCQTIds.Any())
            {
                #region Create thong diep
                var addedDuLieuGuiHDDTs = new List<DuLieuGuiHDDT>();
                var addedThongDieps = new List<ThongDiepChung>();
                var addedFileDatas = new List<FileData>();

                foreach (var id in hoaDonKhongMaChuaGuiCQTIds)
                {
                    DuLieuGuiHDDT duLieuGuiHDDT = new DuLieuGuiHDDT
                    {
                        DuLieuGuiHDDTId = Guid.NewGuid().ToString(),
                        HoaDonDienTuId = id,
                        CreatedDate = DateTime.Now
                    };
                    addedDuLieuGuiHDDTs.Add(duLieuGuiHDDT);

                    var thongDiep203 = new ThongDiepChung
                    {
                        ThongDiepChungId = Guid.NewGuid().ToString(),
                        PhienBan = _configuration["TTChung:PBan"],
                        MaNoiGui = _configuration["TTChung:MNGui"],
                        MaNoiNhan = _configuration["TTChung:MNNhan"],
                        MaLoaiThongDiep = 203,
                        MaThongDiep = _configuration["TTChung:MNGui"] + Guid.NewGuid().ToString().Replace("-", "").ToUpper(),
                        MaSoThue = hoSoHDDT.MaSoThue,
                        SoLuong = 1,
                        IdThamChieu = duLieuGuiHDDT.DuLieuGuiHDDTId,
                        NgayGui = DateTime.Now,
                        TrangThaiGui = (int)TrangThaiGuiThongDiep.ChoPhanHoi,
                        ThongDiepGuiDi = true,
                        CreatedDate = DateTime.Now,
                        Status = true,
                    };
                    addedThongDieps.Add(thongDiep203);

                    string fileName = $"TD-{Guid.NewGuid()}.xml";
                    string filePath = Path.Combine(fullFolderPath, fileName);
                    var thongDiepChungViewModel = _mp.Map<ThongDiepChungViewModel>(thongDiep203);
                    thongDiepChungViewModel.DuLieuGuiHDDT = new DuLieuGuiHDDTViewModel
                    {
                        HoaDonDienTuId = id
                    };
                    await _xMLInvoiceService.CreateQuyDinhKyThuatTheoMaLoaiThongDiep(filePath, thongDiepChungViewModel);
                    string xmlContent = await File.ReadAllTextAsync(filePath);

                    var fileData = new FileData
                    {
                        RefId = thongDiep203.ThongDiepChungId,
                        Type = 1,
                        IsSigned = true,
                        DateTime = DateTime.Now,
                        Binary = Encoding.UTF8.GetBytes(xmlContent),
                        Content = xmlContent
                    };
                    addedFileDatas.Add(fileData);
                }
                await _db.DuLieuGuiHDDTs.AddRangeAsync(addedDuLieuGuiHDDTs);
                await _db.ThongDiepChungs.AddRangeAsync(addedThongDieps);
                await _db.FileDatas.AddRangeAsync(addedFileDatas);
                await _db.SaveChangesAsync();
                #endregion

                #region Send thong diep
                if (addedThongDieps.Any())
                {
                    result = "Send Total: " + addedThongDieps.Count + "\n";
                    foreach (var item in addedThongDieps)
                    {
                        var status = await GuiThongDiepDuLieuHDDTAsync(item.ThongDiepChungId);
                        if (status != TrangThaiQuyTrinh.GuiKhongLoi)
                        {
                            result += $"{item.MaThongDiep}: {status.GetDescription()}\n";
                        }
                    }
                }
                #endregion
            }

            return result;
        }

        #region Tool
        /// <summary>
        /// Get Id từ trường DLieuTBao
        /// </summary>
        /// <param name="dataXML"></param>
        /// <returns></returns>
        private string GetIdThongBaoFromXMLData(string dataXML)
        {
            try
            {
                XmlDocument xd = new XmlDocument();
                xd.LoadXml(dataXML);
                var node = xd.SelectSingleNode("TBao/DLTBao");
                if (node != null)
                {
                    return node.Attributes["Id"].Value;
                }

                return string.Empty;
            }
            catch (Exception)
            {

            }

            return string.Empty;
        }

        #endregion

        /// <summary>
        /// Tạo nhiều thông điệp và gửi thông điệp tới cqt
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public async Task<List<TrangThaiQuyTrinh>> InsertRangeAsync(List<ThongDiepChungViewModel> models)
        {
            var result = new List<TrangThaiQuyTrinh>();
            var transferLog = new List<TransferLog>();

            // Log thông điệp
            var rsInsertThongDieps = await InsertAsync2(models);

            // Get token
            string token = await _ITVanService.GetToken2();
            if (string.IsNullOrEmpty(token))
            {
                return result;
            }

            #region test
            //var rsInsertThongDieps = new List<ThongDiepChungViewModel>();
            //var fileData = await _db.FileDatas.FirstOrDefaultAsync(x => x.FileDataId == "95fe6698-a53d-4705-8a78-f54cc83c4f2f");
            //for (int i = 0; i < 101; i++)
            //{
            //    rsInsertThongDieps.Add(new ThongDiepChungViewModel
            //    {
            //        DataXML = Encoding.UTF8.GetString(fileData.Binary)
            //    });
            //}
            #endregion

            // send multi task concurrently
            var lstXml999 = new List<string>();
            var lengthThongDiep = Math.Ceiling(rsInsertThongDieps.Count / 25d);
            for (int i = 0; i < lengthThongDiep; i++)
            {
                List<Task<string>> lstTasks = new List<Task<string>>();

                var rsThongDiepSlices = rsInsertThongDieps.Skip(i * 25).Take(25).ToList();
                for (int j = 0; j < rsThongDiepSlices.Count; j++)
                {
                    var thongDiep = rsThongDiepSlices[j];
                    lstTasks.Add(GuiThongDiepDuLieuHDDTAsync2(thongDiep, token));

                    if (j == (rsThongDiepSlices.Count - 1))
                    {
                        var rsXml999 = await Task.WhenAll(lstTasks);
                        lstXml999.AddRange(rsXml999);
                    }
                }
            }

            // create transferlog
            foreach (var item in rsInsertThongDieps)
            {
                transferLog.Add(new TransferLog
                {
                    TransferLogId = Guid.NewGuid().ToString(),
                    DateTime = DateTime.Now,
                    Type = 3,
                    MNGui = item.MaNoiNhan999,
                    MNNhan = item.MaNoiNhan999,
                    MLTDiep = Convert.ToInt32(item.MaLoaiThongDiep999),
                    MTDiep = item.MaThongDiep999,
                    MTDTChieu = item.MaThongDiepThamChieu999,
                    XMLData = item.DataXML999
                });

                if (!string.IsNullOrEmpty(item.DataXML999))
                {
                    transferLog.Add(new TransferLog
                    {
                        TransferLogId = Guid.NewGuid().ToString(),
                        DateTime = DateTime.Now,
                        Type = 3,
                        MNGui = item.MaNoiNhan999,
                        MNNhan = item.MaNoiNhan999,
                        MLTDiep = Convert.ToInt32(item.MaLoaiThongDiep999),
                        MTDiep = item.MaThongDiep999,
                        MTDTChieu = item.MaThongDiepThamChieu999,
                        XMLData = item.DataXML999
                    });
                }
            }

            //await _db.BulkInsertAsync(transferLog);

            await _db.TransferLogs.AddRangeAsync(transferLog);

            //Tracert.WriteLog($"lstXml999NotEmmpty {DateTime.Now:dd/MM/yyyy HH:mm:ss}: " + lstXml999.Count(x => !string.IsNullOrEmpty(x)));
            //Tracert.WriteLog($"lstXml999 {DateTime.Now:dd/MM/yyyy HH:mm:ss}: " + string.Join("\n", lstXml999));

            // Handle hóa đơn
            result = await GuiThongDiepDuLieuHDDTAsync3(rsInsertThongDieps);

            return result;
        }
    }
}
