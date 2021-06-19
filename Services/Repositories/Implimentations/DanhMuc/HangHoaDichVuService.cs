﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using DLL;
using DLL.Entity.DanhMuc;
using ManagementServices.Helper;
using Microsoft.EntityFrameworkCore;
using Services.Helper;
using Services.Helper.Params.DanhMuc;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.DanhMuc
{
    public class HangHoaDichVuService : IHangHoaDichVuService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;

        public HangHoaDichVuService(Datacontext datacontext, IMapper mapper)
        {
            _db = datacontext;
            _mp = mapper;
        }

        public async Task<bool> CheckTrungMaAsync(HangHoaDichVuViewModel model)
        {
            bool result = await _db.HangHoaDichVus
                .AnyAsync(x => x.Ma.ToUpper().Trim() == model.Ma.ToUpper().Trim());

            return result;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _db.HangHoaDichVus.FirstOrDefaultAsync(x => x.HangHoaDichVuId == id);
            _db.HangHoaDichVus.Remove(entity);
            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }

        public async Task<List<HangHoaDichVuViewModel>> GetAllAsync(HangHoaDichVuParams @params = null)
        {
            var query = _db.HangHoaDichVus.AsQueryable();

            if (@params != null)
            {
                if (!string.IsNullOrEmpty(@params.Keyword))
                {
                    string keyword = @params.Keyword.ToUpper().ToTrim();
                    query = query.Where(x => x.Ma.ToUpper().ToTrim().Contains(keyword) || x.Ma.ToUpper().ToTrim().ToUnSign().Contains(keyword.ToUpper()) ||
                                            x.Ten.ToUpper().ToTrim().Contains(keyword) || x.Ten.ToUpper().ToTrim().ToUpper().Contains(keyword.ToUpper()));
                }
            }

            var result = await query
                .ProjectTo<HangHoaDichVuViewModel>(_mp.ConfigurationProvider)
                .AsNoTracking()
                .OrderBy(x => x.Ma)
                .ToListAsync();

            return result;
        }

        public async Task<PagedList<HangHoaDichVuViewModel>> GetAllPagingAsync(HangHoaDichVuParams @params)
        {
            var query = from hhdv in _db.HangHoaDichVus
                        join dvt in _db.DonViTinhs on hhdv.DonViTinhId equals dvt.DonViTinhId into tmpDonViTinhs
                        from dvt in tmpDonViTinhs.DefaultIfEmpty()
                        orderby hhdv.Ma
                        select new HangHoaDichVuViewModel
                        {
                            HangHoaDichVuId = hhdv.HangHoaDichVuId,
                            Ma = hhdv.Ma ?? string.Empty,
                            Ten = hhdv.Ten ?? string.Empty,
                            DonGiaBan = hhdv.DonGiaBan,
                            IsGiaBanLaDonGiaSauThue = hhdv.IsGiaBanLaDonGiaSauThue,
                            ThueGTGT = hhdv.ThueGTGT,
                            TenThueGTGT = hhdv.ThueGTGT.GetDescription(),
                            TyLeChietKhau = hhdv.TyLeChietKhau,
                            MoTa = hhdv.MoTa,
                            TenDonViTinh = dvt != null ? dvt.Ten : string.Empty,
                            Status = hhdv.Status
                        };

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

            return await PagedList<HangHoaDichVuViewModel>.CreateAsync(query, @params.PageNumber, @params.PageSize);
        }

        public async Task<HangHoaDichVuViewModel> GetByIdAsync(string id)
        {
            var entity = await _db.HangHoaDichVus.AsNoTracking().FirstOrDefaultAsync(x => x.HangHoaDichVuId == id);
            var result = _mp.Map<HangHoaDichVuViewModel>(entity);
            return result;
        }

        public async Task<HangHoaDichVuViewModel> InsertAsync(HangHoaDichVuViewModel model)
        {
            var entity = _mp.Map<HangHoaDichVu>(model);
            await _db.HangHoaDichVus.AddAsync(entity);
            await _db.SaveChangesAsync();
            var result = _mp.Map<HangHoaDichVuViewModel>(entity);
            return result;
        }

        public async Task<bool> UpdateAsync(HangHoaDichVuViewModel model)
        {
            var entity = await _db.HangHoaDichVus.FirstOrDefaultAsync(x => x.HangHoaDichVuId == model.HangHoaDichVuId);
            _db.Entry(entity).CurrentValues.SetValues(model);
            var result = await _db.SaveChangesAsync() > 0;
            return result;
        }
    }
}
