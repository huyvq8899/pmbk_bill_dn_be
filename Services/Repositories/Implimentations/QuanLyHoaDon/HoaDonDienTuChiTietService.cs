﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using DLL;
using DLL.Entity.QuanLyHoaDon;
using Microsoft.EntityFrameworkCore;
using Services.Repositories.Interfaces.QuanLyHoaDon;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.QuanLyHoaDonDienTu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.QuanLyHoaDon
{
    public class HoaDonDienTuChiTietService : IHoaDonDienTuChiTietService
    {
        private readonly Datacontext _db;
        private readonly IMapper _mp;

        public HoaDonDienTuChiTietService(
            Datacontext datacontext,
            IMapper mapper)
        {
            _db = datacontext;
            _mp = mapper;
        }

        public async Task<HoaDonDienTuViewModel> GetMainAndDetailByPhieuIdAsync(string phieuId)
        {
            HoaDonDienTuViewModel main = await _db.HoaDonDienTus
                .Include(x => x.HoaDonChiTiets)
                .Where(x => x.HoaDonDienTuId == phieuId)
                .ProjectTo<HoaDonDienTuViewModel>(_mp.ConfigurationProvider)
                .FirstOrDefaultAsync();

            main.HoaDonChiTiets = main.HoaDonChiTiets.OrderBy(x => x.CreatedDate).ToList();
            foreach (HoaDonDienTuChiTietViewModel item in main.HoaDonChiTiets)
            {
                item.HoaDon = null;
                item.HangHoaDichVu = null;
                item.DonViTinh = null;
            }

            return main;
        }

        public async Task<List<HoaDonDienTuChiTietViewModel>> InsertRangeAsync(HoaDonDienTuViewModel hoaDonDienTuVM, List<HoaDonDienTuChiTietViewModel> list)
        {
            if (list.Count > 0)
            {
                int count = 1;
                List<HoaDonDienTuChiTiet> listToAdd = new List<HoaDonDienTuChiTiet>();

                foreach (var item in list)
                {
                    item.HoaDonDienTuId = hoaDonDienTuVM.HoaDonDienTuId;
                    item.SoLuong = item.SoLuong ?? 0;
                    item.DonGia = item.DonGia ?? 0;
                    item.DonGiaQuyDoi = item.DonGiaQuyDoi ?? 0;
                    item.TyLeChietKhau = item.TyLeChietKhau ?? 0;
                    item.TienChietKhau = item.TienChietKhau ?? 0;
                    item.TienChietKhauQuyDoi = item.TienChietKhauQuyDoi ?? 0;
                    item.TienThueGTGT = item.TienThueGTGT ?? 0;
                    item.TienThueGTGTQuyDoi = item.TienThueGTGTQuyDoi ?? 0;
                    item.ThanhTien = item.ThanhTien ?? 0;
                    item.ThanhTienQuyDoi = item.ThanhTienQuyDoi ?? 0;
                    item.TyLePhanTramDoanhThu = item.TyLePhanTramDoanhThu ?? 0;
                    item.TienGiam = item.TienGiam ?? 0;
                    item.TienGiamQuyDoi = item.TienGiamQuyDoi ?? 0;
                    item.TongTienThanhToan = item.ThanhTien - item.TienChietKhau + item.TienThueGTGT - item.TienGiam;
                    item.TongTienThanhToanQuyDoi = item.ThanhTienQuyDoi - item.TienChietKhauQuyDoi + item.TienThueGTGTQuyDoi - item.TienGiamQuyDoi;
                    item.CreatedDate = DateTime.Now;
                    item.Status = true;
                    item.DonViTinh = null;
                    item.HangHoaDichVu = null;
                    item.HoaDon = null;
                    count++;

                    HoaDonDienTuChiTiet hoaDonDienTuChiTiet = _mp.Map<HoaDonDienTuChiTiet>(item);
                    listToAdd.Add(hoaDonDienTuChiTiet);
                }

                //List<HoaDonDienTuChiTiet> models = _mp.Map<List<HoaDonDienTuChiTiet>>(list);

                await _db.HoaDonDienTuChiTiets.AddRangeAsync(listToAdd);
                await _db.SaveChangesAsync();
                List<HoaDonDienTuChiTietViewModel> result = _mp.Map<List<HoaDonDienTuChiTietViewModel>>(listToAdd);
                return result;
            }

            return null;
        }

        public async Task RemoveRangeAsync(string HoaDonDienTuId)
        {
            IQueryable<HoaDonDienTuChiTiet> list = _db.HoaDonDienTuChiTiets
                .Where(x => x.HoaDonDienTuId == HoaDonDienTuId);
            _db.HoaDonDienTuChiTiets.RemoveRange(list);
            await _db.SaveChangesAsync();
        }

        public async Task<List<HoaDonDienTuChiTietViewModel>> GetChiTietHoaDonAsync(string hoaDonId, bool displayMauHoaDon)
        {
            var result = new List<HoaDonDienTuChiTietViewModel>();

            result = await (
                        from hdct in _db.HoaDonDienTuChiTiets
                        join hd in _db.HoaDonDienTus on hdct.HoaDonDienTuId equals hd.HoaDonDienTuId into tmpHoaDons
                        from hd in tmpHoaDons.DefaultIfEmpty()
                        join vt in _db.HangHoaDichVus on hdct.HangHoaDichVuId equals vt.HangHoaDichVuId into tmpHangHoas
                        from vt in tmpHangHoas.DefaultIfEmpty()
                        join dvt in _db.DonViTinhs on hdct.DonViTinhId equals dvt.DonViTinhId into tmpDonViTinhs
                        from dvt in tmpDonViTinhs.DefaultIfEmpty()
                        join lt in _db.LoaiTiens on hd.LoaiTienId equals lt.LoaiTienId into tmpLoaiTiens
                        from lt in tmpLoaiTiens.DefaultIfEmpty()
                        where hdct.HoaDonDienTuId == hoaDonId
                        orderby hdct.CreatedDate
                        select new HoaDonDienTuChiTietViewModel
                        {
                            HoaDonDienTuChiTietId = hdct.HoaDonDienTuChiTietId,
                            HoaDonDienTuId = hd.HoaDonDienTuId ?? string.Empty,
                            HoaDon = hd != null ? _mp.Map<HoaDonDienTuViewModel>(hd) : null,
                            HangHoaDichVuId = vt.HangHoaDichVuId ?? string.Empty,
                            HangHoaDichVu = vt != null ? _mp.Map<HangHoaDichVuViewModel>(vt) : null,
                            MaHang = vt != null ? vt.Ma : null,
                            TenHang = hdct.TenHang,
                            TinhChat = hdct.TinhChat,
                            DonViTinhId = dvt.DonViTinhId ?? string.Empty,
                            DonViTinh = dvt != null ? _mp.Map<DonViTinhViewModel>(dvt) : null,
                            TenDonViTinh = hdct.TenDonViTinh,
                            SoLuong = hdct.SoLuong,
                            SoLuongNhap = hdct.SoLuongNhap,
                            DonGia = hdct.DonGia,
                            DonGiaQuyDoi = hdct.DonGiaQuyDoi,
                            ThanhTien = hdct.ThanhTien,
                            ThanhTienQuyDoi = hdct.ThanhTienQuyDoi,
                            TyLeChietKhau = hdct.TyLeChietKhau,
                            TienChietKhau = hdct.TienChietKhau,
                            TienChietKhauQuyDoi = hdct.TienChietKhauQuyDoi,
                            ThueGTGT = hdct.ThueGTGT,
                            IsThueKhac = hdct.ThueGTGT != "0" && hdct.ThueGTGT != "5" && hdct.ThueGTGT != "8" && hdct.ThueGTGT != "10" && hdct.ThueGTGT != "KKKNT" && hdct.ThueGTGT != "KCT",
                            IsHangKhongTinhTien = hdct.TinhChat == 2 || hdct.TinhChat == 4,
                            TienThueGTGT = hdct.TienThueGTGT,
                            TienThueGTGTQuyDoi = hdct.TienThueGTGTQuyDoi,
                            TongTienThanhToan = hdct.TongTienThanhToan ?? (hdct.ThanhTien - hdct.TienChietKhau + hdct.TienThueGTGT),
                            TongTienThanhToanQuyDoi = hdct.TongTienThanhToanQuyDoi ?? (hdct.ThanhTienQuyDoi - hdct.TienChietKhauQuyDoi + hdct.TienThueGTGTQuyDoi),
                            IsMatHangDuocGiam = hdct.IsMatHangDuocGiam,
                            TyLePhanTramDoanhThu = hdct.TyLePhanTramDoanhThu,
                            TienGiam = hdct.TienGiam,
                            TienGiamQuyDoi = hdct.TienGiamQuyDoi,
                            SoLo = hdct.SoLo,
                            HanSuDung = hdct.HanSuDung,
                            SoKhung = hdct.SoKhung,
                            SoMay = hdct.SoMay,
                            LoaiTienId = lt != null ? lt.LoaiTienId : null,
                            IsVND = lt == null || (lt.Ma == "VND"),
                            GhiChu = hdct.GhiChu,
                            XuatBanPhi = hdct.XuatBanPhi,
                            MaNhanVien = hdct.MaNhanVien,
                            TenNhanVien = hdct.TenNhanVien,
                            NhanVienBanHangId = hdct.NhanVienBanHangId,
                            ThanhTienSauThue = hdct.ThanhTienSauThue,
                            TruongMoRongChiTiet1 = hdct.TruongMoRongChiTiet1,
                            TruongMoRongChiTiet2 = hdct.TruongMoRongChiTiet2,
                            TruongMoRongChiTiet3 = hdct.TruongMoRongChiTiet3,
                            TruongMoRongChiTiet4 = hdct.TruongMoRongChiTiet4,
                            TruongMoRongChiTiet5 = hdct.TruongMoRongChiTiet5,
                            TruongMoRongChiTiet6 = hdct.TruongMoRongChiTiet6,
                            TruongMoRongChiTiet7 = hdct.TruongMoRongChiTiet7,
                            TruongMoRongChiTiet8 = hdct.TruongMoRongChiTiet8,
                            TruongMoRongChiTiet9 = hdct.TruongMoRongChiTiet9,
                            TruongMoRongChiTiet10 = hdct.TruongMoRongChiTiet10,
                            CreatedBy = hdct.CreatedBy,
                            CreatedDate = hdct.CreatedDate,
                            Status = hd.Status,
                            STT = hdct.STT
                        }).ToListAsync();

            if (displayMauHoaDon && result.Any())
            {
                var isAllKM = result.All(x => x.TinhChat == 2); // hàng khuyến mãi 
                if (isAllKM)
                {
                    var firstItem = result[0];

                    result.Add(new HoaDonDienTuChiTietViewModel
                    {
                        HoaDonDienTuChiTietId = Guid.NewGuid().ToString(),
                        HoaDonDienTuId = hoaDonId,
                        MaHang = string.Empty,
                        TenHang = "(Hàng khuyến mại không thu tiền)",
                        TinhChat = firstItem.TinhChat,
                        DonViTinh = new DonViTinhViewModel
                        {
                            Ten = string.Empty
                        },
                        SoLuong = 0,
                        DonGia = 0,
                        DonGiaQuyDoi = 0,
                        DonGiaSauThue = 0,
                        ThanhTien = 0,
                        ThanhTienQuyDoi = 0,
                        ThanhTienSauThue = 0,
                        ThanhTienSauThueQuyDoi = 0,
                        ThueGTGT = firstItem.ThueGTGT,
                        TienThueGTGT = 0,
                        TienThueGTGTQuyDoi = 0,
                        TyLeChietKhau = 0,
                        TienChietKhau = 0,
                        TienChietKhauQuyDoi = 0,
                        TongTienThanhToan = 0,
                        TongTienThanhToanQuyDoi = 0,
                        IsAllKhuyenMai = true
                    });
                }
                else
                {
                    foreach (var item in result)
                    {
                        if (item.TinhChat == 2)
                        {
                            item.TenHang += " (Hàng khuyến mãi không thu tiền)";
                        }
                    }
                }
            }

            return result;
        }
    }
}
