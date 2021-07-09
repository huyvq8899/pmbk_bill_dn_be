﻿using AutoMapper;
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
    public class ThongBaoKetQuaHuyHoaDonService : IThongBaoKetQuaHuyHoaDonService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;

        public ThongBaoKetQuaHuyHoaDonService(Datacontext datacontext, IMapper mapper)
        {
            _db = datacontext;
            _mp = mapper;
        }

        public async Task<bool> CheckTrungMaAsync(ThongBaoKetQuaHuyHoaDonViewModel model)
        {
            bool result = await _db.ThongBaoKetQuaHuyHoaDons
               .AnyAsync(x => x.So.ToUpper().Trim() == model.So.ToUpper().Trim());

            return result;
        }

        public async Task<bool> DeleteAsync(string id)
        {
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
                            CreatedBy = tb.CreatedBy,
                            CreatedDate = tb.CreatedDate,
                            Status = tb.Status
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
