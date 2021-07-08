using AutoMapper;
using DLL;
using DLL.Entity.DanhMuc;
using ManagementServices.Helper;
using Microsoft.EntityFrameworkCore;
using Services.Helper.Params.DanhMuc;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.DanhMuc
{
    public class ThongBaoDieuChinhThongTinHoaDonService : IThongBaoDieuChinhThongTinHoaDonService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;

        public ThongBaoDieuChinhThongTinHoaDonService(Datacontext datacontext, IMapper mapper)
        {
            _db = datacontext;
            _mp = mapper;
        }

        public async Task<bool> CheckTrungMaAsync(ThongBaoDieuChinhThongTinHoaDonViewModel model)
        {
            bool result = await _db.ThongBaoDieuChinhThongTinHoaDons
               .AnyAsync(x => x.So.ToUpper().Trim() == model.So.ToUpper().Trim());

            return result;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _db.ThongBaoDieuChinhThongTinHoaDons.FirstOrDefaultAsync(x => x.ThongBaoDieuChinhThongTinHoaDonId == id);
            _db.ThongBaoDieuChinhThongTinHoaDons.Remove(entity);
            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        public Task<List<ThongBaoDieuChinhThongTinHoaDonViewModel>> GetAllAsync(ThongBaoDieuChinhThongTinHoaDonParams @params = null)
        {
            throw new System.NotImplementedException();
        }

        public async Task<PagedList<ThongBaoDieuChinhThongTinHoaDonViewModel>> GetAllPagingAsync(ThongBaoDieuChinhThongTinHoaDonParams @params)
        {
            var query = _db.ThongBaoDieuChinhThongTinHoaDons
                .OrderByDescending(x => x.NgayThongBaoDieuChinh).OrderByDescending(x => x.So)
                .Select(x => new ThongBaoDieuChinhThongTinHoaDonViewModel
                {

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

            return await PagedList<ThongBaoDieuChinhThongTinHoaDonViewModel>.CreateAsync(query, @params.PageNumber, @params.PageSize);
        }

        public async Task<ThongBaoDieuChinhThongTinHoaDonViewModel> GetByIdAsync(string id)
        {
            var entity = await _db.ThongBaoDieuChinhThongTinHoaDons.AsNoTracking().FirstOrDefaultAsync(x => x.ThongBaoDieuChinhThongTinHoaDonId == id);
            var result = _mp.Map<ThongBaoDieuChinhThongTinHoaDonViewModel>(entity);
            return result;
        }

        public async Task<ThongBaoDieuChinhThongTinHoaDonViewModel> InsertAsync(ThongBaoDieuChinhThongTinHoaDonViewModel model)
        {
            List<ThongBaoDieuChinhThongTinHoaDonChiTietViewModel> detailVMs = model.ThongBaoDieuChinhThongTinHoaDonChiTiets;

            model.ThongBaoDieuChinhThongTinHoaDonChiTiets = null;
            model.ThongBaoDieuChinhThongTinHoaDonId = string.IsNullOrEmpty(model.ThongBaoDieuChinhThongTinHoaDonId) ? Guid.NewGuid().ToString() : model.ThongBaoDieuChinhThongTinHoaDonId;
            ThongBaoDieuChinhThongTinHoaDon entity = _mp.Map<ThongBaoDieuChinhThongTinHoaDon>(model);
            await _db.ThongBaoDieuChinhThongTinHoaDons.AddAsync(entity);

            foreach (var item in detailVMs)
            {
                item.Status = true;
                item.CreatedDate = DateTime.Now;
                item.ThongBaoDieuChinhThongTinHoaDonId = entity.ThongBaoDieuChinhThongTinHoaDonId;
                var detail = _mp.Map<ThongBaoDieuChinhThongTinHoaDonChiTiet>(item);
                await _db.ThongBaoDieuChinhThongTinHoaDonChiTiets.AddAsync(detail);
            }

            await _db.SaveChangesAsync();
            ThongBaoDieuChinhThongTinHoaDonViewModel result = _mp.Map<ThongBaoDieuChinhThongTinHoaDonViewModel>(entity);
            return result;
        }

        public async Task<bool> UpdateAsync(ThongBaoDieuChinhThongTinHoaDonViewModel model)
        {
            List<ThongBaoDieuChinhThongTinHoaDonChiTietViewModel> detailVMs = model.ThongBaoDieuChinhThongTinHoaDonChiTiets;

            model.ThongBaoDieuChinhThongTinHoaDonChiTiets = null;
            ThongBaoDieuChinhThongTinHoaDon entity = await _db.ThongBaoDieuChinhThongTinHoaDons.FirstOrDefaultAsync(x => x.ThongBaoDieuChinhThongTinHoaDonId == model.ThongBaoDieuChinhThongTinHoaDonId);
            _db.Entry(entity).CurrentValues.SetValues(model);

            List<ThongBaoDieuChinhThongTinHoaDonChiTiet> details = await _db.ThongBaoDieuChinhThongTinHoaDonChiTiets.Where(x => x.ThongBaoDieuChinhThongTinHoaDonId == model.ThongBaoDieuChinhThongTinHoaDonId).ToListAsync();
            _db.ThongBaoDieuChinhThongTinHoaDonChiTiets.RemoveRange(details);
            foreach (var item in detailVMs)
            {
                item.Status = true;
                item.CreatedDate = DateTime.Now;
                item.ThongBaoDieuChinhThongTinHoaDonId = entity.ThongBaoDieuChinhThongTinHoaDonId;
                var detail = _mp.Map<ThongBaoDieuChinhThongTinHoaDonChiTiet>(item);
                await _db.ThongBaoDieuChinhThongTinHoaDonChiTiets.AddAsync(detail);
            }

            bool result = await _db.SaveChangesAsync() > 0;
            return result;
        }
    }
}
