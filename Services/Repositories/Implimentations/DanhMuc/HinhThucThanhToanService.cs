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
    public class HinhThucThanhToanService : IHinhThucThanhToanService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;

        public HinhThucThanhToanService(Datacontext datacontext, IMapper mapper)
        {
            _db = datacontext;
            _mp = mapper;
        }

        public async Task<bool> CheckTrungMaAsync(HinhThucThanhToanViewModel model)
        {
            bool result = await _db.HinhThucThanhToans
                .AnyAsync(x => x.Ten.ToUpper().Trim() == model.Ten.ToUpper().Trim());

            return result;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _db.HinhThucThanhToans.FirstOrDefaultAsync(x => x.HinhThucThanhToanId == id);
            _db.HinhThucThanhToans.Remove(entity);
            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<List<HinhThucThanhToanViewModel>> GetAllAsync(HinhThucThanhToanParams @params = null)
        {
            var result = new List<HinhThucThanhToanViewModel>();
            try
            {
                var query = _db.HinhThucThanhToans.AsQueryable();

                if (@params != null)
                {
                    if (!string.IsNullOrEmpty(@params.Keyword))
                    {
                        string keyword = @params.Keyword.ToUpper().ToTrim();
                        query = query.Where(x => x.Ten.ToUpper().ToTrim().Contains(keyword) || x.Ten.ToUpper().ToTrim().ToUnSign().Contains(keyword.ToUpper()) ||
                                                x.MoTa.ToUpper().ToTrim().Contains(keyword) || x.MoTa.ToUpper().ToTrim().ToUpper().Contains(keyword.ToUpper()));
                    }

                    if (@params.IsActive.HasValue)
                    {
                        query = query.Where(x => x.Status == @params.IsActive);
                    }
                }

                result = await query
                    .ProjectTo<HinhThucThanhToanViewModel>(_mp.ConfigurationProvider)
                    .AsNoTracking()
                    .OrderBy(x => x.Ten)
                    .ToListAsync();
            }
            catch(Exception ex)
            {
                FileLog.WriteLog(ex.Message);
            }
                          
            return result;
        }

        public async Task<PagedList<HinhThucThanhToanViewModel>> GetAllPagingAsync(HinhThucThanhToanParams @params)
        {
            var query = _db.HinhThucThanhToans
                .OrderBy(x => x.Ten)
                .Select(x => new HinhThucThanhToanViewModel
                {
                    HinhThucThanhToanId = x.HinhThucThanhToanId,
                    Ten = x.Ten ?? string.Empty,
                    MoTa = x.MoTa ?? string.Empty,
                    Status = true
                });

            if (@params.Filter != null)
            {
                if (!string.IsNullOrEmpty(@params.Filter.Ten))
                {
                    var keyword = @params.Filter.Ten.ToUpper().ToTrim();
                    query = query.Where(x => x.Ten.ToUpper().ToTrim().Contains(keyword) || x.Ten.ToUpper().ToTrim().ToUnSign().Contains(keyword.ToUnSign()));
                }
                if (!string.IsNullOrEmpty(@params.Filter.MoTa))
                {
                    var keyword = @params.Filter.MoTa.ToUpper().ToTrim();
                    query = query.Where(x => x.MoTa.ToUpper().ToTrim().Contains(keyword) || x.MoTa.ToUpper().ToTrim().ToUnSign().Contains(keyword.ToUnSign()));
                }
            }

            if (!string.IsNullOrEmpty(@params.SortKey))
            {
                if (@params.SortKey == nameof(@params.Filter.Ten))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.Ten);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.Ten);
                    }
                }

                if (@params.SortKey == nameof(@params.Filter.MoTa))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.MoTa);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.MoTa);
                    }
                }
            }

            if (@params.PageSize == -1)
            {
                @params.PageSize = await query.CountAsync();
            }

            return await PagedList<HinhThucThanhToanViewModel>.CreateAsync(query, @params.PageNumber, @params.PageSize);
        }

        public async Task<HinhThucThanhToanViewModel> GetByIdAsync(string id)
        {
            var entity = await _db.HinhThucThanhToans.AsNoTracking().FirstOrDefaultAsync(x => x.HinhThucThanhToanId == id);
            var result = _mp.Map<HinhThucThanhToanViewModel>(entity);
            return result;
        }

        public async Task<HinhThucThanhToanViewModel> InsertAsync(HinhThucThanhToanViewModel model)
        {
            var entity = _mp.Map<HinhThucThanhToan>(model);
            await _db.HinhThucThanhToans.AddAsync(entity);
            await _db.SaveChangesAsync();
            var result = _mp.Map<HinhThucThanhToanViewModel>(entity);
            return result;
        }

        public async Task<bool> UpdateAsync(HinhThucThanhToanViewModel model)
        {
            var entity = await _db.HinhThucThanhToans.FirstOrDefaultAsync(x => x.HinhThucThanhToanId == model.HinhThucThanhToanId);
            _db.Entry(entity).CurrentValues.SetValues(model);
            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }
    }
}
