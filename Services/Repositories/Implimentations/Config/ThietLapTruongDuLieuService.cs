using AutoMapper;
using DLL;
using DLL.Entity.Config;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.EntityFrameworkCore;
using Services.Repositories.Interfaces.Config;
using Services.ViewModels.Config;
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
            //var result = await _db.ThietLapTruongDuLieus
            //    .Where(x => x.LoaiTruongDuLieu == loaiTruong && x.LoaiHoaDon == loaiHoaDon)
            //    .OrderBy(x => x.STT)
            //    .ProjectTo<ThietLapTruongDuLieuViewModel>(_mp.ConfigurationProvider)
            //    .ToListAsync();

            //HoaDonDienTuViewModel hoaDonDienTu = new HoaDonDienTuViewModel();
            //result = result.Where(x => x.TenCot != nameof(hoaDonDienTu.MauSo)).ToList();

            ThietLapTruongDuLieu entity = new ThietLapTruongDuLieu();
            var result = _mp.Map<List<ThietLapTruongDuLieuViewModel>>(entity.InitData());
            result = result.Where(x => x.LoaiTruongDuLieu == loaiTruong && x.LoaiHoaDon == loaiHoaDon).ToList();

            return result;
        }

        public async Task<List<ThietLapTruongDuLieuViewModel>> GetListTruongMoRongByMauHoaDonIdAsync(string mauHoaDonId)
        {
            var query = from tcct in _db.MauHoaDonTuyChinhChiTiets.Include(x => x.MauHoaDon)
                        where tcct.MauHoaDonId == mauHoaDonId &&
                        ((tcct.LoaiChiTiet >= LoaiChiTietTuyChonNoiDung.TruongThongTinBoSung1 && tcct.LoaiChiTiet <= LoaiChiTietTuyChonNoiDung.TruongThongTinBoSung10) ||
                        (tcct.LoaiChiTiet >= LoaiChiTietTuyChonNoiDung.TruongMoRongChiTiet1 && tcct.LoaiChiTiet <= LoaiChiTietTuyChonNoiDung.TruongMoRongChiTiet10))
                        orderby tcct.STT
                        group tcct by tcct.LoaiChiTiet into g
                        select new ThietLapTruongDuLieuViewModel
                        {
                            STT = g.First(x => x.IsParent == true).STT ?? 0,
                            TenCot = g.Key.NameOfEmum(),
                            TenTruong = GetTenTruongDuLieu(g.Key),
                            TenTruongHienThi = g.First(x => x.LoaiContainer == LoaiContainerTuyChinh.TieuDe).GiaTri,
                            LoaiTruongDuLieu = GetLoaiTruongDuLieu(g.Key),
                            LoaiHoaDon = g.First().MauHoaDon.LoaiHoaDon,
                            KieuDuLieu = g.First().KieuDuLieuThietLap,
                            DoRong = 180,
                            HienThi = g.First(x => x.IsParent == true).Checked ?? false,
                        };

            var result = await query.ToListAsync();
            return result;
        }

        public async Task<bool> InsertRangeAsync(string boKyHieuHoaDonId, List<ThietLapTruongDuLieuViewModel> models)
        {
            var listToAdd = new List<ThietLapTruongDuLieu>();

            foreach (var item in models)
            {
                var entity = _mp.Map<ThietLapTruongDuLieu>(item);
                entity.BoKyHieuHoaDonId = boKyHieuHoaDonId;
                listToAdd.Add(entity);
            }

            await _db.ThietLapTruongDuLieus.AddRangeAsync(listToAdd);
            var result = await _db.SaveChangesAsync();
            return result > 0;
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

        private string GetTenTruongDuLieu(LoaiChiTietTuyChonNoiDung loaiChiTiet)
        {
            bool isThongTinBoSung = false;
            string result = string.Empty;
            int start;
            int end;
            if (loaiChiTiet >= LoaiChiTietTuyChonNoiDung.TruongThongTinBoSung1 && loaiChiTiet <= LoaiChiTietTuyChonNoiDung.TruongThongTinBoSung10)
            {
                isThongTinBoSung = true;
                start = (int)LoaiChiTietTuyChonNoiDung.TruongThongTinBoSung1;
                end = (int)LoaiChiTietTuyChonNoiDung.TruongThongTinBoSung10;
            }
            else
            {
                start = (int)LoaiChiTietTuyChonNoiDung.TruongMoRongChiTiet1;
                end = (int)LoaiChiTietTuyChonNoiDung.TruongMoRongChiTiet10;
            }

            int count = 0;
            string text = isThongTinBoSung ? "Trường thông tin bổ sung" : "Trường thông tin mở rộng";
            for (int i = start; i <= end; i++)
            {
                count += 1;

                if (i == (int)loaiChiTiet)
                {
                    result = $"{text} {count}";
                    break;
                }
            }

            return result;
        }

        private LoaiTruongDuLieu GetLoaiTruongDuLieu(LoaiChiTietTuyChonNoiDung loaiChiTiet)
        {
            if (loaiChiTiet >= LoaiChiTietTuyChonNoiDung.TruongThongTinBoSung1 && loaiChiTiet <= LoaiChiTietTuyChonNoiDung.TruongThongTinBoSung10)
            {
                return LoaiTruongDuLieu.NhomThongTinNguoiMua;
            }

            return LoaiTruongDuLieu.NhomHangHoaDichVu;
        }
    }
}
