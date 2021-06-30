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
    public class DonViTinhService : IDonViTinhService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;

        public DonViTinhService(Datacontext datacontext, IMapper mapper)
        {
            _db = datacontext;
            _mp = mapper;
        }

        public async Task<bool> CheckTrungMaAsync(DonViTinhViewModel model)
        {
            bool result = await _db.DonViTinhs
                .AnyAsync(x => x.Ten.ToUpper().Trim() == model.Ten.ToUpper().Trim());

            return result;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _db.DonViTinhs.FirstOrDefaultAsync(x => x.DonViTinhId == id);
            _db.DonViTinhs.Remove(entity);
            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<List<DonViTinhViewModel>> GetAllAsync(DonViTinhParams @params = null)
        {
            var result = new List<DonViTinhViewModel>();
            try
            {
                var query = _db.DonViTinhs.AsQueryable();

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
                    .ProjectTo<DonViTinhViewModel>(_mp.ConfigurationProvider)
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

        public async Task<PagedList<DonViTinhViewModel>> GetAllPagingAsync(DonViTinhParams @params)
        {
            var query = _db.DonViTinhs
                .OrderBy(x => x.Ten)
                .Select(x => new DonViTinhViewModel
                {
                    DonViTinhId = x.DonViTinhId,
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

            return await PagedList<DonViTinhViewModel>.CreateAsync(query, @params.PageNumber, @params.PageSize);
        }

        public async Task<DonViTinhViewModel> GetByIdAsync(string id)
        {
            var entity = await _db.DonViTinhs.AsNoTracking().FirstOrDefaultAsync(x => x.DonViTinhId == id);
            var result = _mp.Map<DonViTinhViewModel>(entity);
            return result;
        }

        public async Task<DonViTinhViewModel> InsertAsync(DonViTinhViewModel model)
        {
            var entity = _mp.Map<DonViTinh>(model);
            await _db.DonViTinhs.AddAsync(entity);
            await _db.SaveChangesAsync();
            var result = _mp.Map<DonViTinhViewModel>(entity);
            return result;
        }

        public async Task<bool> UpdateAsync(DonViTinhViewModel model)
        {
            var entity = await _db.DonViTinhs.FirstOrDefaultAsync(x => x.DonViTinhId == model.DonViTinhId);
            _db.Entry(entity).CurrentValues.SetValues(model);
            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }
    }
}
