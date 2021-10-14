using AutoMapper;
using AutoMapper.Configuration;
using DLL;
using DLL.Constants;
using DLL.Entity.QuyDinhKyThuat;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services.Repositories.Interfaces;
using Services.Repositories.Interfaces.QuyDinhKyThuat;
using Services.ViewModels.QuanLyHoaDonDienTu;
using Services.ViewModels.QuyDinhKyThuat;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._7;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using HDonGTGT = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.HDon;

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
                if (!string.IsNullOrEmpty(model.FileXMLGui) && File.Exists(Path.Combine(fullFolderPath, model.FileXMLGui)))
                {
                    File.Delete(Path.Combine(fullFolderPath, model.FileXMLGui));
                }
            }

            string fileName = $"{Guid.NewGuid()}.xml";
            string filePath = Path.Combine(fullFolderPath, fileName);
            _xMLInvoiceService.CreateQuyDinhKyThuat_PhanII_II_7(filePath, model);
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
                    TrangThaiTiepNhan = x.TrangThaiTiepNhan
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
                            TrangThaiTiepNhan = td.TrangThaiTiepNhan,
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
