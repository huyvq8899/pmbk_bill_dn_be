using AutoMapper;
using AutoMapper.QueryableExtensions;
using DLL;
using DLL.Entity.Config;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.EntityFrameworkCore;
using Services.Repositories.Interfaces.Config;
using Services.ViewModels.Config;
using Services.ViewModels.QuanLyHoaDonDienTu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.Config
{
    public class ThietLapTruongDuLieuService : IThietLapTruongDuLieuService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;

        public ThietLapTruongDuLieuService(Datacontext datacontext, IMapper mapper)
        {
            _db = datacontext;
            _mp = mapper;
        }

        public async Task<bool> CheckDaPhatSinhThongBaoPhatHanhAsync(ThietLapTruongDuLieuViewModel model)
        {
            var result = await (from mhdtcct in _db.MauHoaDonTuyChinhChiTiets
                                join mhd in _db.MauHoaDons on mhdtcct.MauHoaDonId equals mhd.MauHoaDonId
                                join tbphct in _db.ThongBaoPhatHanhChiTiets on mhd.MauHoaDonId equals tbphct.MauHoaDonId
                                join tbph in _db.ThongBaoPhatHanhs on tbphct.ThongBaoPhatHanhId equals tbph.ThongBaoPhatHanhId
                                where tbph.TrangThaiNop == TrangThaiNop.DaDuocChapNhan &&
                                      mhdtcct.LoaiChiTiet.NameOfEmum() == model.TenCot &&
                                      mhd.LoaiHoaDon == model.LoaiHoaDon
                                select new
                                {
                                    Id = mhdtcct.MauHoaDonTuyChinhChiTietId
                                })
                                .AnyAsync();

            return result;
        }

        public List<ThietLapTruongDuLieuViewModel> GetListThietLapMacDinh(LoaiTruongDuLieu loaiTruong, LoaiHoaDon loaiHoaDon)
        {
            ThietLapTruongDuLieu entity = new ThietLapTruongDuLieu();
            var result = _mp.Map<List<ThietLapTruongDuLieuViewModel>>(entity.InitData());
            result = result.Where(x => x.LoaiTruongDuLieu == loaiTruong && x.LoaiHoaDon == loaiHoaDon).ToList();
            return result;
        }

        public async Task<List<ThietLapTruongDuLieuViewModel>> GetListTruongDuLieuByLoaiTruongAsync(LoaiTruongDuLieu loaiTruong, LoaiHoaDon loaiHoaDon)
        {
            var result = await _db.ThietLapTruongDuLieus
                .Where(x => x.LoaiTruongDuLieu == loaiTruong && x.LoaiHoaDon == loaiHoaDon)
                .OrderBy(x => x.STT)
                .ProjectTo<ThietLapTruongDuLieuViewModel>(_mp.ConfigurationProvider)
                .ToListAsync();

            HoaDonDienTuViewModel hoaDonDienTu = new HoaDonDienTuViewModel();
            result = result.Where(x => x.TenCot != nameof(hoaDonDienTu.MauSo)).ToList();

            //ThietLapTruongDuLieu entity = new ThietLapTruongDuLieu();
            //var result = _mp.Map<List<ThietLapTruongDuLieuViewModel>>(entity.InitData());
            //result = result.Where(x => x.LoaiTruongDuLieu == loaiTruong && x.LoaiHoaDon == loaiHoaDon).ToList();

            return result;
        }

        public async Task UpdateAsync(ThietLapTruongDuLieuViewModel model)
        {
            var entities = await _db.ThietLapTruongDuLieus
                .Where(x => x.TenCot == model.TenCot && x.LoaiHoaDon == model.LoaiHoaDon)
                .ToListAsync();

            foreach (var item in entities)
            {
                item.TenTruongHienThi = model.TenTruongHienThi;
                item.KieuDuLieu = model.KieuDuLieu;
            }

            await _db.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(List<ThietLapTruongDuLieuViewModel> models)
        {
            var entities = await _db.ThietLapTruongDuLieus
                .Where(x => models.Select(y => y.ThietLapTruongDuLieuId).Contains(x.ThietLapTruongDuLieuId))
                .ToListAsync();

            foreach (var item in entities)
            {
                var model = models.FirstOrDefault(x => x.ThietLapTruongDuLieuId == item.ThietLapTruongDuLieuId);
                item.TenTruongHienThi = model.TenTruongHienThi;
                item.GhiChu = model.GhiChu;
                item.DoRong = model.DoRong;
                item.STT = model.STT;
                item.HienThi = model.HienThi;
            }

            await _db.SaveChangesAsync();
        }
    }
}
