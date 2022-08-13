using AutoMapper;
using DLL;
using DLL.Constants;
using DLL.Entity;
using DLL.Entity.QuanLyHoaDon;
using DLL.Entity.QuyDinhKyThuat;
using DLL.Enums;
using EFCore.BulkExtensions;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Services.Helper;
using Services.Helper.Constants;
using Services.Helper.Params.QuyDinhKyThuat;
using Services.Helper.XmlModel;
using Services.Repositories.Interfaces;
using Services.Repositories.Interfaces.DanhMuc;
using Services.Repositories.Interfaces.QuanLyHoaDon;
using Services.Repositories.Interfaces.QuyDinhKyThuat;
using Services.ViewModels.QuanLyHoaDonDienTu;
using Services.ViewModels.QuyDinhKyThuat;
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

namespace Services.Repositories.Implimentations.QuyDinhKyThuat
{
    public class DuLieuGuiHDDTService : IDuLieuGuiHDDTService
    {
        private readonly Datacontext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMapper _mp;
        private readonly IXMLInvoiceService _xMLInvoiceService;
        private readonly ITVanService _ITVanService;
        private readonly IHoaDonDienTuService _hoaDonDienTuService;
        private readonly IQuyDinhKyThuatService _quyDinhKyThuatService;
        private readonly IConfiguration _configuration;
        private readonly IHoSoHDDTService _hoSoHDDTService;

        public DuLieuGuiHDDTService(
            Datacontext dataContext,
            IHttpContextAccessor httpContextAccessor,
            IHostingEnvironment hostingEnvironment,
            IMapper mp,
            IXMLInvoiceService xMLInvoiceService,
            ITVanService ITVanService,
            IHoaDonDienTuService hoaDonDienTuService,
            IQuyDinhKyThuatService quyDinhKyThuatService,
            IConfiguration configuration,
            IHoSoHDDTService hoSoHDDTService)
        {
            _db = dataContext;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _mp = mp;
            _xMLInvoiceService = xMLInvoiceService;
            _ITVanService = ITVanService;
            _hoaDonDienTuService = hoaDonDienTuService;
            _quyDinhKyThuatService = quyDinhKyThuatService;
            _configuration = configuration;
            _hoSoHDDTService = hoSoHDDTService;
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

            // add thongdiep 200 | 203
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
            await _db.BulkInsertAsync(lstDuLieuGuiHDDTs);
            // await _db.DuLieuGuiHDDTs.AddRangeAsync(lstDuLieuGuiHDDTs);

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
            var hd = await _hoaDonDienTuService.GetByIdAsync(@params.Keyword);
            if (@params.LoaiThongDiep == 300)
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
                         where td.HoaDonDienTuId == @params.Keyword
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
                             CreatedBy = td.CreatedBy,
                             CreatedDate = td.CreatedDate,
                             ThongDiepGuiDi = tdc.ThongDiepGuiDi,
                             NgayGui = tdc.NgayGui,
                             NguoiThucHien = _db.Users.FirstOrDefault(x => x.UserId == td.CreatedBy).UserName ?? "Hệ thống",
                             NgayThongBao = tdc.NgayThongBao,
                             FileXML = _db.TransferLogs.FirstOrDefault(x => x.MTDiep == tdc.MaThongDiep).XMLData,
                             Status = td.Status,
                             TrangThaiGui = (TrangThaiGuiThongDiep)tdc.TrangThaiGui,
                             TenTrangThaiThongBao = ((TrangThaiGuiThongDiep)tdc.TrangThaiGui).GetDescription(),

                         });

            var query2 = (from tdCQT in _db.ThongDiepChiTietGuiCQTs
                          join tdgCQT in _db.ThongDiepGuiCQTs on tdCQT.ThongDiepGuiCQTId equals tdgCQT.Id
                          join tdc2 in _db.ThongDiepChungs on tdgCQT.Id equals tdc2.IdThamChieu
                          where tdCQT.HoaDonDienTuId == @params.Keyword
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



            var list2 = query2.OrderByDescending(x => x.NgayGui).ToList();
            var list3 = query3.OrderByDescending(x => x.NgayGui).ToList();
            var list = await query.OrderByDescending(x => x.NgayGui).ToListAsync();
            if (list2.Count > 0)
            {
                list = await query.Union(query2).OrderByDescending(x => x.NgayGui).ToListAsync();
            }

            if (list3.Count > 0)
            {
                list = await query.Union(query3).OrderByDescending(x => x.NgayGui).ToListAsync();
            }

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
                        listTranlogs = listTranlogs.OrderBy(x => x.SoLuong).ToList();
                        listThongDiepChungs = listThongDiepChungs.OrderBy(x => x.SoLuong).ToList();
                    }
                    if (@params.SortKey == "NgayGui" && @params.SortValue == "descend")
                    {
                        listTranlogs = listTranlogs.OrderByDescending(x => x.SoLuong).ToList();
                        listThongDiepChungs = listThongDiepChungs.OrderByDescending(x => x.SoLuong).ToList();
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
                    join tl in _db.TransferLogs on tdc.MaThongDiep equals tl.MTDiep
                    where tdc.MaThongDiep == maThongDiep
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
                        FileXML = tl.XMLData,
                        Status = tdc.Status,
                        TrangThaiGui = (TrangThaiGuiThongDiep)tdc.TrangThaiGui,
                        TenTrangThaiThongBao = ((TrangThaiGuiThongDiep)tdc.TrangThaiGui).GetDescription(),
                        NguoiThucHien = _db.Users.FirstOrDefault(x => x.UserId == tdc.ModifyBy).UserName,
                    };
            query2 = from tdc in _db.ThongDiepChungs
                     join tl in _db.TransferLogs on tdc.MaThongDiep equals tl.MTDiep
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
                         NgayThongBao = tl.DateTime,
                         FileXML = tl.XMLData,
                         Status = tdc.Status,
                         TrangThaiGui = (TrangThaiGuiThongDiep)tdc.TrangThaiGui,
                         TenTrangThaiThongBao = ((TrangThaiGuiThongDiep)tdc.TrangThaiGui).GetDescription(),
                         NguoiThucHien = tl.Type == 2 ? "TCT" : "TCTN",
                     };

            var listCha = query.DistinctBy(x => x.MaThongDiep).OrderByDescending(x => x.NgayGui).ToList();
            var listCon = query2.DistinctBy(x => x.MaThongDiep).OrderByDescending(x => x.NgayGui).ToList();
            foreach (var item in listCha)
            {
                List<ThongDiepChungViewModel> listconByMTD = new List<ThongDiepChungViewModel>();
                foreach (var con in listCon)
                {
                    if (item.MaThongDiep == con.MaThongDiepThamChieu)
                    {
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
            //for (int i = 0; i < 50; i++)
            //{
            //    rsInsertThongDieps.Add(new ThongDiepChungViewModel
            //    {
            //        DataXML = Encoding.UTF8.GetString(fileData.Binary)
            //    });
            //}
            #endregion

            // 
            List<Task<string>> lstTasks = new List<Task<string>>();
            foreach (var thongDiep in rsInsertThongDieps)
            {
                lstTasks.Add(GuiThongDiepDuLieuHDDTAsync2(thongDiep, token));
            }

            // send async
            var lstXml999 = await Task.WhenAll(lstTasks);

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

            Tracert.WriteLog($"lstXml999NotEmmpty {DateTime.Now:dd/MM/yyyy HH:mm:ss}: " + lstXml999.Count(x => !string.IsNullOrEmpty(x)));
            Tracert.WriteLog($"lstXml999 {DateTime.Now:dd/MM/yyyy HH:mm:ss}: " + string.Join("\n", lstXml999));

            // Handle hóa đơn
            result = await GuiThongDiepDuLieuHDDTAsync3(rsInsertThongDieps);

            return result;
        }
    }
}
