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
            var result = new List<HangHoaDichVuViewModel>();
            try
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

                result = await query
                    .ProjectTo<HangHoaDichVuViewModel>(_mp.ConfigurationProvider)
                    .AsNoTracking()
                    .OrderBy(x => x.Ma)
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(ex.Message);
            }

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
                    query = query.Where(x => x.Ma.ToUpper().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(@params.Filter.Ten))
                {
                    var keyword = @params.Filter.Ten.ToUpper().ToTrim();
                    query = query.Where(x => x.Ten.ToUpper().ToTrim().Contains(keyword) || x.Ten.ToUpper().ToTrim().ToUnSign().Contains(keyword.ToUnSign()));
                }
                if (!string.IsNullOrEmpty(@params.Filter.TenDonViTinh))
                {
                    var keyword = @params.Filter.TenDonViTinh.ToUpper().ToTrim();
                    query = query.Where(x => x.TenDonViTinh.ToUpper().ToTrim().Contains(keyword) || x.TenDonViTinh.ToUpper().ToTrim().ToUnSign().Contains(keyword.ToUnSign()));
                }
                if (@params.Filter.DonGiaBan.HasValue)
                {
                    var keyword = @params.Filter.DonGiaBan.ToString().ToTrim();
                    query = query.Where(x => x.DonGiaBan.ToString().ToTrim().Contains(keyword));
                }
                if (!string.IsNullOrEmpty(@params.Filter.TenThueGTGT))
                {
                    var keyword = @params.Filter.TenThueGTGT.ToUpper().ToTrim();
                    query = query.Where(x => x.TenThueGTGT.ToUpper().ToTrim().Contains(keyword));
                }
                if (@params.Filter.TyLeChietKhau.HasValue)
                {
                    var keyword = @params.Filter.TyLeChietKhau.ToString().ToTrim();
                    query = query.Where(x => x.TyLeChietKhau.ToString().ToTrim().Contains(keyword));
                }
            }

            if (!string.IsNullOrEmpty(@params.SortKey))
            {
                if (@params.SortKey == nameof(@params.Filter.Ma))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.Ma);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.Ma);
                    }
                }

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

                if (@params.SortKey == nameof(@params.Filter.TenDonViTinh))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.TenDonViTinh);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.TenDonViTinh);
                    }
                }

                if (@params.SortKey == nameof(@params.Filter.DonGiaBan))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.DonGiaBan);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.DonGiaBan);
                    }
                }

                if (@params.SortKey == nameof(@params.Filter.TenThueGTGT))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.TenThueGTGT);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.TenThueGTGT);
                    }
                }

                if (@params.SortKey == nameof(@params.Filter.TyLeChietKhau))
                {
                    if (@params.SortValue == "ascend")
                    {
                        query = query.OrderBy(x => x.TyLeChietKhau);
                    }
                    if (@params.SortValue == "descend")
                    {
                        query = query.OrderByDescending(x => x.TyLeChietKhau);
                    }
                }
            }

            if (@params.PageSize == -1)
            {
                @params.PageSize = await query.CountAsync();
            }

            return await PagedList<HangHoaDichVuViewModel>.CreateAsync(query, @params.PageNumber, @params.PageSize);
        }

        public async Task<HangHoaDichVuViewModel> GetByIdAsync(string id)
        {
            var query = from hhdv in _db.HangHoaDichVus
                        join dvt in _db.DonViTinhs on hhdv.DonViTinhId equals dvt.DonViTinhId into tmpDonViTinhs
                        from dvt in tmpDonViTinhs.DefaultIfEmpty()
                        where hhdv.HangHoaDichVuId == id
                        select new HangHoaDichVuViewModel
                        {
                            HangHoaDichVuId = hhdv.HangHoaDichVuId,
                            Ma = hhdv.Ma,
                            Ten = hhdv.Ten,
                            DonGiaBan = hhdv.DonGiaBan,
                            IsGiaBanLaDonGiaSauThue = hhdv.IsGiaBanLaDonGiaSauThue,
                            ThueGTGT = hhdv.ThueGTGT,
                            TyLeChietKhau = hhdv.TyLeChietKhau,
                            MoTa = hhdv.MoTa,
                            DonViTinhId = hhdv.DonViTinhId,
                            TenDonViTinh = dvt != null ? dvt.Ten : string.Empty,
                            CreatedBy = hhdv.CreatedBy,
                            CreatedDate = hhdv.CreatedDate,
                            Status = hhdv.Status
                        };

            var result = await query.FirstOrDefaultAsync();
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
