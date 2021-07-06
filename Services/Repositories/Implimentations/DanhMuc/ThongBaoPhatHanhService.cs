using AutoMapper;
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
    public class ThongBaoPhatHanhService : IThongBaoPhatHanhService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;

        public ThongBaoPhatHanhService(Datacontext datacontext, IMapper mapper)
        {
            _db = datacontext;
            _mp = mapper;
        }

        public async Task<bool> CheckTrungMaAsync(ThongBaoPhatHanhViewModel model)
        {
            bool result = await _db.ThongBaoPhatHanhs
                .AnyAsync(x => x.So.ToUpper().Trim() == model.So.ToUpper().Trim());

            return result;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _db.ThongBaoPhatHanhs.FirstOrDefaultAsync(x => x.ThongBaoPhatHanhId == id);
            _db.ThongBaoPhatHanhs.Remove(entity);
            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        public Task<List<ThongBaoPhatHanhViewModel>> GetAllAsync(ThongBaoPhatHanhParams @params = null)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedList<ThongBaoPhatHanhViewModel>> GetAllPagingAsync(ThongBaoPhatHanhParams @params)
        {
            var query = _db.ThongBaoPhatHanhs
                .OrderByDescending(x => x.Ngay).OrderByDescending(x => x.So)
                .Select(x => new ThongBaoPhatHanhViewModel
                {
                    Ngay = x.Ngay,
                    So = x.So,
                    TrangThaiNop = x.TrangThaiNop,
                    TenTrangThaiNop = x.TrangThaiNop.GetDescription()
                });

            if (@params.Filter != null)
            {
                //if (!string.IsNullOrEmpty(@params.Filter.Ma))
                //{
                //    var keyword = @params.Filter.Ma.ToUpper().ToTrim();
                //    query = query.Where(x => x.Ma.ToUpper().ToTrim().Contains(keyword));
                //}
                //if (!string.IsNullOrEmpty(@params.Filter.Ten))
                //{
                //    var keyword = @params.Filter.Ten.ToUpper().ToTrim();
                //    query = query.Where(x => x.Ten.ToUpper().ToTrim().Contains(keyword) || x.Ten.ToUpper().ToTrim().ToUnSign().Contains(keyword.ToUnSign()));
                //}
                //if (!string.IsNullOrEmpty(@params.Filter.MaSoThue))
                //{
                //    var keyword = @params.Filter.MaSoThue.ToUpper().ToTrim();
                //    query = query.Where(x => x.MaSoThue.ToUpper().ToTrim().Contains(keyword));
                //}
            }

            if (!string.IsNullOrEmpty(@params.SortKey))
            {
                if (@params.SortKey == nameof(@params.Filter.Ngay))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.Ngay);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.Ngay);
                    }
                }

                if (@params.SortKey == nameof(@params.Filter.So))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.So);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.So);
                    }
                }
            }

            if (@params.PageSize == -1)
            {
                @params.PageSize = await query.CountAsync();
            }

            return await PagedList<ThongBaoPhatHanhViewModel>.CreateAsync(query, @params.PageNumber, @params.PageSize);
        }

        public async Task<ThongBaoPhatHanhViewModel> GetByIdAsync(string id)
        {
            var entity = await _db.ThongBaoPhatHanhs.AsNoTracking().FirstOrDefaultAsync(x => x.ThongBaoPhatHanhId == id);
            var result = _mp.Map<ThongBaoPhatHanhViewModel>(entity);
            return result;
        }

        public async Task<ThongBaoPhatHanhViewModel> InsertAsync(ThongBaoPhatHanhViewModel model)
        {
            var entity = _mp.Map<ThongBaoPhatHanh>(model);
            await _db.ThongBaoPhatHanhs.AddAsync(entity);
            await _db.SaveChangesAsync();
            var result = _mp.Map<ThongBaoPhatHanhViewModel>(entity);
            return result;
        }

        public async Task<bool> UpdateAsync(ThongBaoPhatHanhViewModel model)
        {
            var entity = await _db.ThongBaoPhatHanhs.FirstOrDefaultAsync(x => x.ThongBaoPhatHanhId == model.ThongBaoPhatHanhId);
            _db.Entry(entity).CurrentValues.SetValues(model);
            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }
    }
}
