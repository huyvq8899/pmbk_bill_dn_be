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
using Newtonsoft.Json;
using Services.Enums;
using Services.Helper;
using Services.Helper.Params.QuyDinhKyThuat;
using Services.Helper.XmlModel;
using Services.Repositories.Interfaces;
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
using System.Net;
using System.Net.Sockets;
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

        public DuLieuGuiHDDTService(
            Datacontext dataContext,
            IHttpContextAccessor httpContextAccessor,
            IHostingEnvironment hostingEnvironment,
            IMapper mp,
            IXMLInvoiceService xMLInvoiceService)
        {
            _db = dataContext;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _mp = mp;
            _xMLInvoiceService = xMLInvoiceService;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            //string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            //string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.QuyDinhKyThuat_PhanII_II_7);
            //string folderPath = $"FilesUpload/{databaseName}/{loaiNghiepVu}/{id}";
            //string fullFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, folderPath);
            //if (Directory.Exists(fullFolderPath))
            //{
            //    Directory.Delete(fullFolderPath, true);
            //}

            //var entity = await _db.DuLieuGuiHDDTs.FirstOrDefaultAsync(x => x.DuLieuGuiHDDTId == id);
            //_db.DuLieuGuiHDDTs.Remove(entity);
            //var result = await _db.SaveChangesAsync() > 0;
            //return result;

            return true;
        }

        public async Task<string> ExportXMLGuiDiAsync(string id)
        {
            //var model = await GetByIdAsync(id);
            //string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            //string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.QuyDinhKyThuat_PhanII_II_7);
            //string folderPath = $"FilesUpload/{databaseName}/{loaiNghiepVu}/{model.DuLieuGuiHDDTId}/Gui";
            //string fullFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, folderPath);
            //if (!Directory.Exists(fullFolderPath))
            //{
            //    Directory.CreateDirectory(fullFolderPath);
            //}
            //else
            //{
            //    Directory.Delete(fullFolderPath, true);
            //    Directory.CreateDirectory(fullFolderPath);
            //}

            //string fileName = $"{Guid.NewGuid()}.xml";
            //string filePath = Path.Combine(fullFolderPath, fileName);
            //// _xMLInvoiceService.CreateQuyDinhKyThuat_PhanII_II_7(filePath, model);

            //var entity = await _db.DuLieuGuiHDDTs.FirstOrDefaultAsync(x => x.DuLieuGuiHDDTId == id);
            //// entity.FileXMLGui = fileName;
            //await _db.SaveChangesAsync();

            //return Path.Combine(folderPath, fileName);

            return null;
        }

        public async Task<string> ExportXMLKetQuaAsync(string id)
        {
            //var entity = await _db.ThongDiepGuiDuLieuHDDTs.AsNoTracking().FirstOrDefaultAsync(x => x.DuLieuGuiHDDTId == id);
            //string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            //string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.QuyDinhKyThuat_PhanII_II_7);
            //string folderPath = $"FilesUpload/{databaseName}/{loaiNghiepVu}/{id}/Nhan/{entity.FileXMLNhan}";
            //return folderPath;
            return null;
        }

        public async Task<PagedList<DuLieuGuiHDDTViewModel>> GetAllPagingAsync(ThongDiepParams @params)
        {
            var query = _db.DuLieuGuiHDDTs
                //.Where(x => x.MaLoaiThongDiep == ((int)@params.LoaiThongDiep).ToString())
                .OrderByDescending(x => x.CreatedBy)
                .Select(x => new DuLieuGuiHDDTViewModel
                {
                    DuLieuGuiHDDTId = x.DuLieuGuiHDDTId,
                    //PhienBan = x.PhienBan,
                    //MaNoiGui = x.MaNoiGui,
                    //MaNoiNhan = x.MaNoiNhan,
                    //MaLoaiThongDiep = x.MaLoaiThongDiep,
                    //MaThongDiep = x.MaThongDiep,
                    //MaThongDiepThamChieu = x.MaThongDiepThamChieu,
                    //SoLuong = x.SoLuong,
                    //TrangThaiGui = x.TrangThaiGui,
                    //FileXMLGui = x.FileXMLGui,
                    //FileXMLNhan = x.FileXMLNhan,
                    //TrangThaiTiepNhan = x.TrangThaiTiepNhan,
                    //TenTrangThaiGui = x.TrangThaiGui.GetDescription(),
                    //TenTrangThaiTiepNhan = x.TrangThaiTiepNhan.GetDescription(),
                    CreatedDate = x.CreatedDate,
                });

            if (@params.PageSize == -1)
            {
                @params.PageSize = await query.CountAsync();
            }

            return await PagedList<DuLieuGuiHDDTViewModel>.CreateAsync(query, @params.PageNumber, @params.PageSize);
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
            File.Delete(xmlFilePath);

            return fileByte;
        }

        public async Task<ThongDiepChungViewModel> InsertAsync(ThongDiepChungViewModel model)
        {
            DuLieuGuiHDDT duLieuGuiHDDT = new DuLieuGuiHDDT
            {
                DuLieuGuiHDDTId = Guid.NewGuid().ToString(),
                HoaDonDienTuId = model.DuLieuGuiHDDT.HoaDonDienTuId,
            };

            if (model.DuLieuGuiHDDT.DuLieuGuiHDDTChiTiets != null)
            {
                duLieuGuiHDDT.DuLieuGuiHDDTChiTiets = new List<DuLieuGuiHDDTChiTiet>();

                foreach (var item in model.DuLieuGuiHDDT.DuLieuGuiHDDTChiTiets)
                {
                    duLieuGuiHDDT.DuLieuGuiHDDTChiTiets.Add(new DuLieuGuiHDDTChiTiet
                    {
                        HoaDonDienTuId = item.HoaDonDienTuId,
                        CreatedDate = DateTime.Now,
                        Status = true
                    });
                }
            }

            await _db.DuLieuGuiHDDTs.AddAsync(duLieuGuiHDDT);

            model.ThongDiepChungId = Guid.NewGuid().ToString();
            model.IdThamChieu = duLieuGuiHDDT.DuLieuGuiHDDTId;
            model.NgayGui = DateTime.Now;
            ThongDiepChung entity = _mp.Map<ThongDiepChung>(model);

            //////// create xml
            #region create xml
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.ThongDiepChung);
            string folderPath = $"FilesUpload/{databaseName}/{loaiNghiepVu}/{model.ThongDiepChungId}";
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
            _xMLInvoiceService.CreateQuyDinhKyThuatTheoMaLoaiThongDiep(filePath, model);
            entity.FileXML = fileName;
            #endregion

            await _db.ThongDiepChungs.AddAsync(entity);

            await _db.SaveChangesAsync();
            ThongDiepChungViewModel result = _mp.Map<ThongDiepChungViewModel>(entity);
            return result;
        }

        public string CreateXMLBangTongHopDuLieu(BangTongHopDuLieuParams @params)
        {
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.BangTongHopDuLieu);
            string folderPath = $"FilesUpload/{loaiNghiepVu}/unsigned";
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
            _xMLInvoiceService.CreateBangTongHopDuLieu(filePath, @params);
            return fileName;
        }


        public async Task<bool> NhanPhanHoiThongDiepKiemTraDuLieuHoaDonAsync(ThongDiepParams @params)
        {
            //var tDiep = DataHelper.ConvertByteXMLToObject<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._8.TDiep>(@params.FileByte);
            //var entity = await _db.ThongDiepChungs.FirstOrDefaultAsync(x => x.MaThongDiep == tDiep.TTChung.MTDTChieu);
            //if (entity != null)
            //{
            //    string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            //    string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.QuyDinhKyThuat_PhanII_II_7);
            //    string folderPath = $"FilesUpload/{databaseName}/{loaiNghiepVu}/{entity.ThongDiepChungId}/Nhan";
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
            //    File.WriteAllBytes(Path.Combine(fullFolderPath, fileName), @params.FileByte);

            //    //entity.TrangThaiGui = TrangThaiGuiToKhaiDenCQT.DaTiepNhan;
            //    //entity.FileXMLNhan = fileName;

            //    //if (tDiep.DLieu.TBao.DLTBao.LTBao == LTBao.ThongBao3)
            //    //{
            //    //    entity.TrangThaiTiepNhan = TrangThaiTiepNhanCuaCoQuanThue.KhongChapNhan;
            //    //}
            //    //else
            //    //{
            //    //    entity.TrangThaiTiepNhan = TrangThaiTiepNhanCuaCoQuanThue.ChapNhan;
            //    //}

            //    var result = await _db.SaveChangesAsync();
            //    return result > 0;
            //}

            //return false;

            return false;
        }

        public async Task<bool> GuiBangDuLieu(string XMLUrl, string thongDiepChungId, string maThongDiep, string mst)
        {
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.ThongDiepChung);
            string assetsFolder = $"FilesUpload/{databaseName}/{loaiNghiepVu}/{thongDiepChungId}";
            var fullXMLFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);
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
                DataXML = File.ReadAllText(Path.Combine(fullXMLFolder, XMLUrl))
            };
            await _db.TVANSendData("api/report/send", data.DataXML);
            return true;
        }

        public async Task<List<TongHopDuLieuHoaDonGuiCQTViewModel>> GetDuLieuBangTongHopGuiDenCQT(BangTongHopParams @params)
        {
            IQueryable<TongHopDuLieuHoaDonGuiCQTViewModel> query = null;
            if (@params.LoaiHangHoa == 1)
            {
                query = from hd in _db.HoaDonDienTus
                        join hdct in _db.HoaDonDienTuChiTiets on hd.HoaDonDienTuId equals hdct.HoaDonDienTuId into tmpHoaDons
                        from hdct in tmpHoaDons.DefaultIfEmpty()
                        join mhd in _db.MauHoaDons on hd.MauHoaDonId equals mhd.MauHoaDonId into tmpMauHoaDons
                        from mhd in tmpMauHoaDons.DefaultIfEmpty()
                        join dvt in _db.DonViTinhs on hdct.DonViTinhId equals dvt.DonViTinhId into tmpDonViTinhs
                        from dvt in tmpDonViTinhs.DefaultIfEmpty()
                        where hd.TrangThaiPhatHanh == (int)TrangThaiPhatHanh.DaPhatHanh && hd.TrangThai != (int)TrangThaiHoaDon.HoaDonXoaBo && mhd.QuyDinhApDung == QuyDinhApDung.ND1232020TT782021
                        select new TongHopDuLieuHoaDonGuiCQTViewModel
                        {
                            MauSo = hd.MauSo,
                            KyHieu = hd.KyHieu,
                            SoHoaDon = hd.SoHoaDon,
                            NgayHoaDon = hd.NgayHoaDon,
                            MaSoThue = hd.MaSoThue,
                            MaKhachHang = hd.MaKhachHang,
                            TenKhachHang = hd.TenKhachHang,
                            HoTenNguoiMuaHang = hd.HoTenNguoiMuaHang,
                            MaHang = hdct.MaHang,
                            TenHang = hdct.TenHang,
                            SoLuong = hdct.SoLuong,
                            DonViTinh = dvt.Ten ?? string.Empty,
                            ThanhTien = hdct.ThanhTien,
                            ThueGTGT = hdct.ThueGTGT,
                            TienThueGTGT = hdct.TienThueGTGT,
                            TongTienThanhToan = hdct.TongTienThanhToan,
                            TenTrangThaiHoaDon = ((TrangThaiHoaDon)hd.TrangThai).GetDescription(),
                            MauSoHoaDonLienQuan = !string.IsNullOrEmpty(hd.DieuChinhChoHoaDonId) ? _db.HoaDonDienTus.Where(x => x.HoaDonDienTuId == hd.DieuChinhChoHoaDonId).Select(x => x.MauSo).FirstOrDefault() :
                                             !string.IsNullOrEmpty(hd.ThayTheChoHoaDonId) ? _db.HoaDonDienTus.Where(x => x.HoaDonDienTuId == hd.ThayTheChoHoaDonId).Select(x => x.MauSo).FirstOrDefault() : null,
                            KyHieuHoaDonLienQuan = !string.IsNullOrEmpty(hd.DieuChinhChoHoaDonId) ? _db.HoaDonDienTus.Where(x => x.HoaDonDienTuId == hd.DieuChinhChoHoaDonId).Select(x => x.KyHieu).FirstOrDefault() :
                                             !string.IsNullOrEmpty(hd.ThayTheChoHoaDonId) ? _db.HoaDonDienTus.Where(x => x.HoaDonDienTuId == hd.ThayTheChoHoaDonId).Select(x => x.KyHieu).FirstOrDefault() : null
                        };
            }
            else
            {
                query = from hd in _db.HoaDonDienTus
                        join mhd in _db.MauHoaDons on hd.MauHoaDonId equals mhd.MauHoaDonId into tmpMauHoaDons
                        from mhd in tmpMauHoaDons.DefaultIfEmpty()
                        where hd.TrangThaiPhatHanh == (int)TrangThaiPhatHanh.DaPhatHanh && hd.TrangThai != (int)TrangThaiHoaDon.HoaDonXoaBo && mhd.QuyDinhApDung == QuyDinhApDung.ND1232020TT782021
                        select new TongHopDuLieuHoaDonGuiCQTViewModel
                        {
                            MauSo = hd.MauSo,
                            KyHieu = hd.KyHieu,
                            SoHoaDon = hd.SoHoaDon,
                            NgayHoaDon = hd.NgayHoaDon,
                            MaSoThue = hd.MaSoThue,
                            TenKhachHang = hd.TenKhachHang,
                            HoTenNguoiMuaHang = hd.HoTenNguoiMuaHang,
                            ThanhTien = _db.HoaDonDienTuChiTiets.Where(x => x.HoaDonDienTuId == hd.HoaDonDienTuId).Sum(x => x.ThanhTien),
                            TienThueGTGT = _db.HoaDonDienTuChiTiets.Where(x => x.HoaDonDienTuId == hd.HoaDonDienTuId).Sum(x => x.TienThueGTGT),
                            TongTienThanhToan = _db.HoaDonDienTuChiTiets.Where(x => x.HoaDonDienTuId == hd.HoaDonDienTuId).Sum(x => x.TongTienThanhToan),
                            TenTrangThaiHoaDon = ((TrangThaiHoaDon)hd.TrangThai).GetDescription(),
                            TrangThaiHoaDon = hd.TrangThai,
                            MauSoHoaDonLienQuan = !string.IsNullOrEmpty(hd.DieuChinhChoHoaDonId) ? _db.HoaDonDienTus.Where(x => x.HoaDonDienTuId == hd.DieuChinhChoHoaDonId).Select(x => x.MauSo).FirstOrDefault() :
                                             !string.IsNullOrEmpty(hd.ThayTheChoHoaDonId) ? _db.HoaDonDienTus.Where(x => x.HoaDonDienTuId == hd.ThayTheChoHoaDonId).Select(x => x.MauSo).FirstOrDefault() : null,
                            KyHieuHoaDonLienQuan = !string.IsNullOrEmpty(hd.DieuChinhChoHoaDonId) ? _db.HoaDonDienTus.Where(x => x.HoaDonDienTuId == hd.DieuChinhChoHoaDonId).Select(x => x.KyHieu).FirstOrDefault() :
                                             !string.IsNullOrEmpty(hd.ThayTheChoHoaDonId) ? _db.HoaDonDienTus.Where(x => x.HoaDonDienTuId == hd.ThayTheChoHoaDonId).Select(x => x.KyHieu).FirstOrDefault() : null
                        };
            }

            if (!string.IsNullOrEmpty(@params.TuNgay) && !string.IsNullOrEmpty(@params.DenNgay))
            {
                DateTime fromDate = DateTime.ParseExact(@params.TuNgay, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                DateTime toDate = DateTime.ParseExact(@params.DenNgay + " 23:59:59", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

                query = query.Where(x => x.NgayHoaDon >= fromDate && x.NgayHoaDon <= toDate);
            }

            return await query.ToListAsync();
        }

        public string LuuDuLieuKy(string encodedContent, string thongDiepId)
        {
            var fileName = Guid.NewGuid().ToString() + ".xml";
            var databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.ThongDiepChung);
            string assetsFolder = $"FilesUpload/{databaseName}/{loaiNghiepVu}/{thongDiepId}";
            var fullFolder = Path.Combine(_hostingEnvironment.WebRootPath, assetsFolder);
            if (!Directory.Exists(fullFolder))
            {
                Directory.CreateDirectory(fullFolder);
            }

            var fullXMLFile = Path.Combine(fullFolder, fileName);
            File.WriteAllText(fullXMLFile, encodedContent);
            return fileName;
        }
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

        public async Task<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._8.TDiep> KetQuaKiemTraDuLieuHoaDonAsync(string id)
        {
            //var entity = await _db.ThongDiepGuiDuLieuHDDTs.AsNoTracking().FirstOrDefaultAsync(x => x.DuLieuGuiHDDTId == id);
            //string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            //string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.QuyDinhKyThuat_PhanII_II_7);
            //string folderPath = $"FilesUpload/{databaseName}/{loaiNghiepVu}/{id}/Nhan";
            //string fullFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, folderPath, entity.FileXMLNhan);

            //XDocument xd = XDocument.Load(fullFolderPath);
            //XmlSerializer serialiser = new XmlSerializer(typeof(ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._8.TDiep));
            //var model = (ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._8.TDiep)serialiser.Deserialize(xd.CreateReader());
            //return model;

            return null;
        }

        public async Task<bool> NhanPhanHoiThongDiepKyThuatAsync(ThongDiepParams @params)
        {
            //var tDiep = DataHelper.ConvertByteXMLToObject<ViewModels.XML.QuyDinhKyThuatHDDT.PhanI.IV._6.TDiep>(@params.FileByte);
            //var entity = await _db.ThongDiepChungs.FirstOrDefaultAsync(x => x.MaThongDiep == tDiep.TTChung.MTDTChieu);
            //if (entity != null)
            //{
            //    string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            //    string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.QuyDinhKyThuat_PhanII_II_7);
            //    string folderPath = $"FilesUpload/{databaseName}/{loaiNghiepVu}/{entity.IdThamChieu}";
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
            //    File.WriteAllBytes(Path.Combine(fullFolderPath, fileName), @params.FileByte);

            //    //entity.TrangThaiGui = (int)TrangThaiGuiToKhaiDenCQT.DaTiepNhan;
            //    //entity.FileXMLNhan = fileName;

            //    //if (tDiep.DLieu.TBao.TTTNhan == TTTNhan.CoLoi)
            //    //{
            //    //    entity.TrangThaiTiepNhan = TrangThaiTiepNhanCuaCoQuanThue.KhongChapNhan;
            //    //}
            //    //else
            //    //{
            //    //    entity.TrangThaiTiepNhan = TrangThaiTiepNhanCuaCoQuanThue.ChapNhan;
            //    //}

            //    var result = await _db.SaveChangesAsync();
            //    return result > 0;
            //}

            return false;
        }

        public async Task<ViewModels.XML.QuyDinhKyThuatHDDT.PhanI.IV._6.TDiep> KetQuaPhanHoiKyThuatAsync(string id)
        {
            //var entity = await _db.ThongDiepGuiDuLieuHDDTs.AsNoTracking().FirstOrDefaultAsync(x => x.DuLieuGuiHDDTId == id);
            //string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            //string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.QuyDinhKyThuat_PhanII_II_7);
            //string folderPath = $"FilesUpload/{databaseName}/{loaiNghiepVu}/{id}/Nhan";
            //string fullFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, folderPath, entity.FileXMLNhan);

            //XDocument xd = XDocument.Load(fullFolderPath);
            //XmlSerializer serialiser = new XmlSerializer(typeof(ViewModels.XML.QuyDinhKyThuatHDDT.PhanI.IV._6.TDiep));
            //var model = (ViewModels.XML.QuyDinhKyThuatHDDT.PhanI.IV._6.TDiep)serialiser.Deserialize(xd.CreateReader());
            //return model;

            return null;
        }

        public async Task<bool> GuiThongDiepDuLieuHDDTAsync(string id)
        {
            var entity = await _db.ThongDiepChungs.AsNoTracking().FirstOrDefaultAsync(x => x.ThongDiepChungId == id);

            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.ThongDiepChung);
            string folderPath = $"FilesUpload/{databaseName}/{loaiNghiepVu}/{entity.ThongDiepChungId}/{entity.FileXML}";
            string filePath = Path.Combine(_hostingEnvironment.WebRootPath, folderPath);
            // TextHelper.SendViaSocketConvert("192.168.2.108", 35000, DataHelper.EncodeString(JsonConvert.SerializeObject(data)));

            string fileBody = File.ReadAllText(filePath); // relative path;

            // Send to TVAN
            await _db.TVANSendData("api/invoice/send", fileBody);

            // Write log send
            //await _dataContext.AddTransferLogSendAsync(
            //                        new ThongDiepPhanHoiParams
            //                        {
            //                            MLTDiep = 100,
            //                            MTDiep = data.MTDiep,
            //                            MTDTChieu = string.Empty,
            //                            DataXML = data.DataXML
            //                        });

            return true;
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
                    File.Delete(filePath);

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
    }
}
