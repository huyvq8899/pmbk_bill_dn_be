using AutoMapper;
using DLL;
using DLL.Constants;
using DLL.Entity.QuyDinhKyThuat;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services.Helper;
using Services.Helper.Params.QuyDinhKyThuat;
using Services.Repositories.Interfaces;
using Services.Repositories.Interfaces.QuyDinhKyThuat;
using Services.ViewModels.QuanLyHoaDonDienTu;
using Services.ViewModels.QuyDinhKyThuat;
using Services.ViewModels.XML;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._7;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._8;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Services.Repositories.Implimentations.QuyDinhKyThuat
{
    public class ThongDiepGuiHDDTKhongMaService : IThongDiepGuiHDDTKhongMaService
    {
        private readonly Datacontext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMapper _mp;
        private readonly IXMLInvoiceService _xMLInvoiceService;

        public ThongDiepGuiHDDTKhongMaService(
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
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.QuyDinhKyThuat_PhanII_II_7);
            string folderPath = $"FilesUpload/{databaseName}/{loaiNghiepVu}/{id}";
            string fullFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, folderPath);
            if (Directory.Exists(fullFolderPath))
            {
                Directory.Delete(fullFolderPath, true);
            }

            var entity = await _db.ThongDiepGuiHDDTKhongMas.FirstOrDefaultAsync(x => x.ThongDiepGuiHDDTKhongMaId == id);
            _db.ThongDiepGuiHDDTKhongMas.Remove(entity);
            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<string> ExportXMLAsync(string id)
        {
            var model = await GetByIdAsync(id);
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.QuyDinhKyThuat_PhanII_II_7);
            string folderPath = $"FilesUpload/{databaseName}/{loaiNghiepVu}/{model.ThongDiepGuiHDDTKhongMaId}/Gui";
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
            _xMLInvoiceService.CreateQuyDinhKyThuat_PhanII_II_7(filePath, model);

            var entity = await _db.ThongDiepGuiHDDTKhongMas.FirstOrDefaultAsync(x => x.ThongDiepGuiHDDTKhongMaId == id);
            entity.FileXMLGui = fileName;
            await _db.SaveChangesAsync();

            return Path.Combine(folderPath, fileName);
        }

        public async Task<PagedList<ThongDiepGuiHDDTKhongMaViewModel>> GetAllPagingAsync(PagingParams @params)
        {
            var query = _db.ThongDiepGuiHDDTKhongMas
                .OrderByDescending(x => x.CreatedBy)
                .Select(x => new ThongDiepGuiHDDTKhongMaViewModel
                {
                    ThongDiepGuiHDDTKhongMaId = x.ThongDiepGuiHDDTKhongMaId,
                    PhienBan = x.PhienBan,
                    MaNoiGui = x.MaNoiGui,
                    MaNoiNhan = x.MaNoiNhan,
                    MaLoaiThongDiep = x.MaLoaiThongDiep,
                    MaThongDiep = x.MaThongDiep,
                    MaThongDiepThamChieu = x.MaThongDiepThamChieu,
                    SoLuong = x.SoLuong,
                    TrangThaiGui = x.TrangThaiGui,
                    FileXMLGui = x.FileXMLGui,
                    FileXMLNhan = x.FileXMLNhan,
                    TrangThaiTiepNhan = x.TrangThaiTiepNhan,
                    CreatedDate = x.CreatedDate
                });

            if (@params.PageSize == -1)
            {
                @params.PageSize = await query.CountAsync();
            }

            return await PagedList<ThongDiepGuiHDDTKhongMaViewModel>.CreateAsync(query, @params.PageNumber, @params.PageSize);
        }

        public async Task<ThongDiepGuiHDDTKhongMaViewModel> GetByIdAsync(string id)
        {
            var query = from td in _db.ThongDiepGuiHDDTKhongMas
                        where td.ThongDiepGuiHDDTKhongMaId == id
                        select new ThongDiepGuiHDDTKhongMaViewModel
                        {
                            ThongDiepGuiHDDTKhongMaId = td.ThongDiepGuiHDDTKhongMaId,
                            PhienBan = td.PhienBan,
                            MaNoiGui = td.MaNoiGui,
                            MaNoiNhan = td.MaNoiNhan,
                            MaLoaiThongDiep = td.MaLoaiThongDiep,
                            MaThongDiep = td.MaThongDiep,
                            MaThongDiepThamChieu = td.MaThongDiepThamChieu,
                            MaSoThue = td.MaSoThue,
                            SoLuong = td.SoLuong,
                            FileXMLGui = td.FileXMLGui,
                            FileXMLNhan = td.FileXMLNhan,
                            TrangThaiGui = td.TrangThaiGui,
                            TenTrangThaiGui = td.TrangThaiGui.GetDescription(),
                            TrangThaiTiepNhan = td.TrangThaiTiepNhan,
                            TenTrangThaiTiepNhan = td.TrangThaiTiepNhan.GetDescription(),
                            CreatedBy = td.CreatedBy,
                            CreatedDate = td.CreatedDate,
                            Status = td.Status,
                            ThongDiepGuiHDDTKhongMaDuLieus = (from tddl in _db.ThongDiepGuiHDDTKhongMaDuLieus
                                                              join hddt in _db.HoaDonDienTus on tddl.HoaDonDienTuId equals hddt.HoaDonDienTuId
                                                              where tddl.ThongDiepGuiHDDTKhongMaId == td.ThongDiepGuiHDDTKhongMaId
                                                              orderby tddl.CreatedDate
                                                              select new ThongDiepGuiHDDTKhongMaDuLieuViewModel
                                                              {
                                                                  ThongDiepGuiHDDTKhongMaDuLieuId = tddl.ThongDiepGuiHDDTKhongMaDuLieuId,
                                                                  ThongDiepGuiHDDTKhongMaId = tddl.ThongDiepGuiHDDTKhongMaId,
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
                        };

            var result = await query.FirstOrDefaultAsync();
            return result;
        }

        public byte[] GuiThongDiep(ThongDiepParams @params)
        {
            // convert url xml to content xml
            XmlDocument xml = new XmlDocument();
            xml.Load(@params.FileUrl);
            StringWriter sw = new StringWriter();
            XmlTextWriter xw = new XmlTextWriter(sw);
            xml.DocumentElement.WriteTo(xw);
            string xmlString = sw.ToString();

            // convert content xml to object
            XmlSerializer serialiser = new XmlSerializer(typeof(ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._7.TDiep));
            StringReader rdr = new StringReader(xmlString);
            var model = (ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._7.TDiep)serialiser.Deserialize(rdr);

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
                DLieu = new DLieu
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
                if (i % 2 == 0)
                {
                    stt += 1;
                    var item = model.DLieu[i].DLHDon.TTChung;
                    tDiep.DLieu.TBao.DLTBao.LHDKMa.DSHDon.Add(new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._3.HDonDSHDon
                    {
                        STT = stt,
                        KHMSHDon = item.KHMSHDon,
                        KHHDon = item.KHHDon,
                        SHDon = item.SHDon,
                        NLap = item.NLap,
                        DSLDo = new List<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._3.LDo>
                        {
                            new ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._3.LDo
                            {
                                MLoi = "LOI1",
                                MTLoi = "Lỗi test",
                                HDXLy = "Fix bug",
                            }
                        }
                    });
                }
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

        public async Task<ThongDiepGuiHDDTKhongMaViewModel> InsertAsync(ThongDiepGuiHDDTKhongMaViewModel model)
        {
            List<ThongDiepGuiHDDTKhongMaDuLieuViewModel> duLieus = model.ThongDiepGuiHDDTKhongMaDuLieus;

            model.ThongDiepGuiHDDTKhongMaDuLieus = null;
            model.ThongDiepGuiHDDTKhongMaId = Guid.NewGuid().ToString();
            ThongDiepGuiHDDTKhongMa entity = _mp.Map<ThongDiepGuiHDDTKhongMa>(model);
            await _db.ThongDiepGuiHDDTKhongMas.AddAsync(entity);

            foreach (var item in duLieus)
            {
                item.Status = true;
                item.CreatedDate = DateTime.Now;
                item.ThongDiepGuiHDDTKhongMaId = entity.ThongDiepGuiHDDTKhongMaId;
                var detail = _mp.Map<ThongDiepGuiHDDTKhongMaDuLieu>(item);
                await _db.ThongDiepGuiHDDTKhongMaDuLieus.AddAsync(detail);
            }

            await _db.SaveChangesAsync();
            ThongDiepGuiHDDTKhongMaViewModel result = _mp.Map<ThongDiepGuiHDDTKhongMaViewModel>(entity);
            return result;
        }

        public async Task<bool> NhanPhanHoiAsync(ThongDiepParams @params)
        {
            var tDiep = DataHelper.ConvertByteXMLToObject<ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._8.TDiep>(@params.FileByte);
            var entity = await _db.ThongDiepGuiHDDTKhongMas.FirstOrDefaultAsync(x => x.MaThongDiep == tDiep.TTChung.MTDTChieu);
            if (entity != null)
            {
                string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
                string loaiNghiepVu = Enum.GetName(typeof(RefType), RefType.QuyDinhKyThuat_PhanII_II_7);
                string folderPath = $"FilesUpload/{databaseName}/{loaiNghiepVu}/{entity.ThongDiepGuiHDDTKhongMaId}/Nhan";
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
                File.WriteAllBytes(Path.Combine(fullFolderPath, fileName), @params.FileByte);

                entity.TrangThaiGui = TrangThaiGuiToKhaiDenCQT.DaTiepNhan;
                entity.FileXMLNhan = fileName;

                if (tDiep.DLieu.TBao.DLTBao.LTBao == LTBao.ThongBao3)
                {
                    entity.TrangThaiTiepNhan = TrangThaiTiepNhanCuaCoQuanThue.KhongChapNhan;
                }
                else
                {
                    entity.TrangThaiTiepNhan = TrangThaiTiepNhanCuaCoQuanThue.ChapNhan;
                }

                var result = await _db.SaveChangesAsync();
                return result > 0;
            }

            return false;
        }

        public async Task<bool> UpdateAsync(ThongDiepGuiHDDTKhongMaViewModel model)
        {
            List<ThongDiepGuiHDDTKhongMaDuLieuViewModel> duLieus = model.ThongDiepGuiHDDTKhongMaDuLieus;

            model.ThongDiepGuiHDDTKhongMaDuLieus = null;
            var entity = await _db.ThongDiepGuiHDDTKhongMas.FirstOrDefaultAsync(x => x.ThongDiepGuiHDDTKhongMaId == model.ThongDiepGuiHDDTKhongMaId);
            _db.Entry(entity).CurrentValues.SetValues(model);

            var oldDuLieus = await _db.ThongDiepGuiHDDTKhongMaDuLieus.Where(x => x.ThongDiepGuiHDDTKhongMaId == model.ThongDiepGuiHDDTKhongMaId).ToListAsync();
            _db.ThongDiepGuiHDDTKhongMaDuLieus.RemoveRange(oldDuLieus);
            foreach (var item in duLieus)
            {
                item.Status = true;
                item.CreatedDate = DateTime.Now;
                item.ThongDiepGuiHDDTKhongMaId = entity.ThongDiepGuiHDDTKhongMaId;
                var detail = _mp.Map<ThongDiepGuiHDDTKhongMaDuLieu>(item);
                await _db.ThongDiepGuiHDDTKhongMaDuLieus.AddAsync(detail);
            }

            bool result = await _db.SaveChangesAsync() > 0;
            return result;
        }
    }
}
