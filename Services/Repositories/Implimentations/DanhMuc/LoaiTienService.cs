using AutoMapper;
using AutoMapper.QueryableExtensions;
using DLL;
using DLL.Entity.DanhMuc;
using ManagementServices.Helper;
using Microsoft.EntityFrameworkCore;
using Services.Helper;
using Services.Helper.Params.DanhMuc;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.DanhMuc
{
    public class LoaiTienService : ILoaiTienService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;

        public LoaiTienService(Datacontext datacontext, IMapper mapper)
        {
            _db = datacontext;
            _mp = mapper;
        }

        public async Task<bool> CheckTrungMaAsync(LoaiTienViewModel model)
        {
            bool result = await _db.LoaiTiens
                .AnyAsync(x => x.Ma.ToUpper().Trim() == model.Ma.ToUpper().Trim());

            return result;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _db.LoaiTiens.FirstOrDefaultAsync(x => x.LoaiTienId == id);
            _db.LoaiTiens.Remove(entity);
            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<List<LoaiTienViewModel>> GetAllAsync(LoaiTienParams @params = null)
        {
            var result = new List<LoaiTienViewModel>();
            try
            {
                var query = _db.LoaiTiens.AsQueryable();

                if (@params != null)
                {
                    if (!string.IsNullOrEmpty(@params.Keyword))
                    {
                        string keyword = @params.Keyword.ToUpper().ToTrim();
                        query = query.Where(x => x.Ma.ToUpper().ToTrim().Contains(keyword) || x.Ma.ToUpper().ToTrim().ToUnSign().Contains(keyword.ToUpper()) ||
                                                x.Ten.ToUpper().ToTrim().Contains(keyword) || x.Ten.ToUpper().ToTrim().ToUpper().Contains(keyword.ToUpper()));
                    }

                    if (@params.IsActive.HasValue)
                    {
                        query = query.Where(x => x.Status == @params.IsActive);
                    }
                }

                result = await query
                    .ProjectTo<LoaiTienViewModel>(_mp.ConfigurationProvider)
                    .AsNoTracking()
                    .OrderBy(x => x.SapXep).ThenBy(x => x.Ma)
                    .ToListAsync();
            }
            catch(Exception ex)
            {
                FileLog.WriteLog(ex.Message);
            }
            return result;
        }

        public async Task<PagedList<LoaiTienViewModel>> GetAllPagingAsync(LoaiTienParams @params)
        {
            var query = _db.LoaiTiens
                .OrderBy(x => x.SapXep).ThenBy(x => x.Ma)
                .Select(x => new LoaiTienViewModel
                {
                    LoaiTienId = x.LoaiTienId,
                    Ma = x.Ma ?? string.Empty,
                    Ten = x.Ten ?? string.Empty,
                    TyGiaQuyDoi = x.TyGiaQuyDoi,
                    SapXep = x.SapXep,
                    Status = true
                });

            if (@params.Filter != null)
            {
                if (!string.IsNullOrEmpty(@params.Filter.Ma))
                {
                    var keyword = @params.Filter.Ma.ToUpper().ToTrim();
                    query = query.Where(x => x.Ma.ToUpper().ToTrim().Contains(keyword) || x.Ma.ToUpper().ToTrim().ToUnSign().Contains(keyword.ToUnSign()));
                }
                if (!string.IsNullOrEmpty(@params.Filter.Ten))
                {
                    var keyword = @params.Filter.Ten.ToUpper().ToTrim();
                    query = query.Where(x => x.Ten.ToUpper().ToTrim().Contains(keyword) || x.Ten.ToUpper().ToTrim().ToUnSign().Contains(keyword.ToUnSign()));
                }
            }

            if (@params.PageSize == -1)
            {
                @params.PageSize = await query.CountAsync();
            }

            return await PagedList<LoaiTienViewModel>.CreateAsync(query, @params.PageNumber, @params.PageSize);
        }

        public async Task<LoaiTienViewModel> GetByIdAsync(string id)
        {
            var entity = await _db.LoaiTiens.AsNoTracking().FirstOrDefaultAsync(x => x.LoaiTienId == id);
            var result = _mp.Map<LoaiTienViewModel>(entity);
            return result;
        }

        public async Task<LoaiTienViewModel> InsertAsync(LoaiTienViewModel model)
        {
            var entity = _mp.Map<LoaiTien>(model);
            await _db.LoaiTiens.AddAsync(entity);
            await _db.SaveChangesAsync();
            var result = _mp.Map<LoaiTienViewModel>(entity);
            return result;
        }

        public async Task<bool> UpdateAsync(LoaiTienViewModel model)
        {
            var entity = await _db.LoaiTiens.FirstOrDefaultAsync(x => x.LoaiTienId == model.LoaiTienId);
            _db.Entry(entity).CurrentValues.SetValues(model);
            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }
    }
}
