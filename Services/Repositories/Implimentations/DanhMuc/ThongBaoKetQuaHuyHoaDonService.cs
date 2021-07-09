using AutoMapper;
using DLL;
using DLL.Constants;
using DLL.Entity.DanhMuc;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services.Helper;
using Services.Helper.Params.DanhMuc;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.DanhMuc
{
    public class ThongBaoKetQuaHuyHoaDonService : IThongBaoKetQuaHuyHoaDonService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ThongBaoKetQuaHuyHoaDonService(Datacontext datacontext, IMapper mapper, IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _db = datacontext;
            _mp = mapper;
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> CheckTrungMaAsync(ThongBaoKetQuaHuyHoaDonViewModel model)
        {
            bool result = await _db.ThongBaoKetQuaHuyHoaDons
               .AnyAsync(x => x.So.ToUpper().Trim() == model.So.ToUpper().Trim());

            return result;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            UploadFile uploadFile = new UploadFile(_hostingEnvironment, _httpContextAccessor);
            await uploadFile.DeleteAllFileAttaches(new TaiLieuDinhKemViewModel
            {
                NghiepVuId = id,
                LoaiNghiepVu = LoaiNghiepVu.ThongBaoKetQuaHuyHoaDon
            }, _db);

            var entity = await _db.ThongBaoKetQuaHuyHoaDons.FirstOrDefaultAsync(x => x.ThongBaoKetQuaHuyHoaDonId == id);
            _db.ThongBaoKetQuaHuyHoaDons.Remove(entity);
            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        public Task<List<ThongBaoKetQuaHuyHoaDonViewModel>> GetAllAsync(ThongBaoKetQuaHuyHoaDonParams @params = null)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedList<ThongBaoKetQuaHuyHoaDonViewModel>> GetAllPagingAsync(ThongBaoKetQuaHuyHoaDonParams @params)
        {
            var query = _db.ThongBaoKetQuaHuyHoaDons
                .OrderByDescending(x => x.NgayThongBao).OrderByDescending(x => x.So)
                .Select(x => new ThongBaoKetQuaHuyHoaDonViewModel
                {
                    ThongBaoKetQuaHuyHoaDonId = x.ThongBaoKetQuaHuyHoaDonId,
                    NgayThongBao = x.NgayThongBao,
                    So = x.So,
                    PhuongPhapHuy = x.PhuongPhapHuy,
                    TrangThaiNop = x.TrangThaiNop,
                    TenTrangThaiNop = x.TrangThaiNop.GetDescription()
                });

            if (@params.Filter != null)
            {

            }

            if (!string.IsNullOrEmpty(@params.SortKey))
            {

            }

            if (@params.PageSize == -1)
            {
                @params.PageSize = await query.CountAsync();
            }

            return await PagedList<ThongBaoKetQuaHuyHoaDonViewModel>.CreateAsync(query, @params.PageNumber, @params.PageSize);
        }

        public async Task<ThongBaoKetQuaHuyHoaDonViewModel> GetByIdAsync(string id)
        {
            string databaseName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;
            string loaiNghiepVu = Enum.GetName(typeof(LoaiNghiepVu), LoaiNghiepVu.ThongBaoKetQuaHuyHoaDon);
            string rootFolder = $@"\FilesUpload\{databaseName}\FileAttach\{loaiNghiepVu}\{id}";
            string folder = _hostingEnvironment.WebRootPath + rootFolder;

            var query = from tb in _db.ThongBaoKetQuaHuyHoaDons
                        where tb.ThongBaoKetQuaHuyHoaDonId == id
                        select new ThongBaoKetQuaHuyHoaDonViewModel
                        {
                            ThongBaoKetQuaHuyHoaDonId = tb.ThongBaoKetQuaHuyHoaDonId,
                            CoQuanThue = tb.CoQuanThue,
                            NgayGioHuy = tb.NgayGioHuy,
                            PhuongPhapHuy = tb.PhuongPhapHuy,
                            So = tb.So,
                            NgayThongBao = tb.NgayThongBao,
                            TrangThaiNop = tb.TrangThaiNop,
                            ThongBaoKetQuaHuyHoaDonChiTiets = (from tbct in _db.ThongBaoKetQuaHuyHoaDonChiTiets
                                                               join mhd in _db.MauHoaDons on tbct.MauHoaDonId equals mhd.MauHoaDonId
                                                               where tbct.ThongBaoKetQuaHuyHoaDonId == tb.ThongBaoKetQuaHuyHoaDonId
                                                               orderby tbct.CreatedDate
                                                               select new ThongBaoKetQuaHuyHoaDonChiTietViewModel
                                                               {
                                                                   ThongBaoKetQuaHuyHoaDonChiTietId = tbct.ThongBaoKetQuaHuyHoaDonChiTietId,
                                                                   ThongBaoKetQuaHuyHoaDonId = tbct.ThongBaoKetQuaHuyHoaDonId,
                                                                   LoaiHoaDon = tbct.LoaiHoaDon,
                                                                   MauHoaDonId = tbct.MauHoaDonId,
                                                                   MauSo = mhd.MauSo,
                                                                   KyHieu = mhd.KyHieu,
                                                                   TuSo = tbct.TuSo,
                                                                   DenSo = tbct.DenSo,
                                                                   SoLuong = tbct.SoLuong
                                                               })
                                                               .ToList(),
                            TaiLieuDinhKems = (from tldk in _db.TaiLieuDinhKems
                                               where tldk.NghiepVuId == tb.ThongBaoKetQuaHuyHoaDonId
                                               orderby tldk.CreatedDate
                                               select new TaiLieuDinhKemViewModel
                                               {
                                                   TaiLieuDinhKemId = tldk.TaiLieuDinhKemId,
                                                   NghiepVuId = tldk.NghiepVuId,
                                                   LoaiNghiepVu = tldk.LoaiNghiepVu,
                                                   TenGoc = tldk.TenGoc,
                                                   TenGuid = tldk.TenGuid,
                                                   CreatedDate = tldk.CreatedDate,
                                                   Link = Path.Combine(_hostingEnvironment.WebRootPath, folder, tldk.TenGuid).ToByteArray(),
                                                   Status = tldk.Status
                                               })
                                               .ToList(),
                            CreatedBy = tb.CreatedBy,
                            CreatedDate = tb.CreatedDate,
                            Status = tb.Status,
                        };

            var result = await query.FirstOrDefaultAsync();
            return result;
        }

        public async Task<ThongBaoKetQuaHuyHoaDonViewModel> InsertAsync(ThongBaoKetQuaHuyHoaDonViewModel model)
        {
            List<ThongBaoKetQuaHuyHoaDonChiTietViewModel> detailVMs = model.ThongBaoKetQuaHuyHoaDonChiTiets;

            model.ThongBaoKetQuaHuyHoaDonChiTiets = null;
            model.ThongBaoKetQuaHuyHoaDonId = string.IsNullOrEmpty(model.ThongBaoKetQuaHuyHoaDonId) ? Guid.NewGuid().ToString() : model.ThongBaoKetQuaHuyHoaDonId;
            ThongBaoKetQuaHuyHoaDon entity = _mp.Map<ThongBaoKetQuaHuyHoaDon>(model);
            await _db.ThongBaoKetQuaHuyHoaDons.AddAsync(entity);

            foreach (var item in detailVMs)
            {
                item.Status = true;
                item.CreatedDate = DateTime.Now;
                item.ThongBaoKetQuaHuyHoaDonId = entity.ThongBaoKetQuaHuyHoaDonId;
                var detail = _mp.Map<ThongBaoKetQuaHuyHoaDonChiTiet>(item);
                await _db.ThongBaoKetQuaHuyHoaDonChiTiets.AddAsync(detail);
            }

            await _db.SaveChangesAsync();
            ThongBaoKetQuaHuyHoaDonViewModel result = _mp.Map<ThongBaoKetQuaHuyHoaDonViewModel>(entity);
            return result;
        }

        public async Task<bool> UpdateAsync(ThongBaoKetQuaHuyHoaDonViewModel model)
        {
            List<ThongBaoKetQuaHuyHoaDonChiTietViewModel> detailVMs = model.ThongBaoKetQuaHuyHoaDonChiTiets;

            model.ThongBaoKetQuaHuyHoaDonChiTiets = null;
            ThongBaoKetQuaHuyHoaDon entity = await _db.ThongBaoKetQuaHuyHoaDons.FirstOrDefaultAsync(x => x.ThongBaoKetQuaHuyHoaDonId == model.ThongBaoKetQuaHuyHoaDonId);
            _db.Entry(entity).CurrentValues.SetValues(model);

            List<ThongBaoKetQuaHuyHoaDonChiTiet> details = await _db.ThongBaoKetQuaHuyHoaDonChiTiets.Where(x => x.ThongBaoKetQuaHuyHoaDonId == model.ThongBaoKetQuaHuyHoaDonId).ToListAsync();
            _db.ThongBaoKetQuaHuyHoaDonChiTiets.RemoveRange(details);
            foreach (var item in detailVMs)
            {
                item.Status = true;
                item.CreatedDate = DateTime.Now;
                item.ThongBaoKetQuaHuyHoaDonId = entity.ThongBaoKetQuaHuyHoaDonId;
                var detail = _mp.Map<ThongBaoKetQuaHuyHoaDonChiTiet>(item);
                await _db.ThongBaoKetQuaHuyHoaDonChiTiets.AddAsync(detail);
            }

            bool result = await _db.SaveChangesAsync() > 0;
            return result;
        }
    }
}
