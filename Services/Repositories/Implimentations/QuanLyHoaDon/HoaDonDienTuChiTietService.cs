using AutoMapper;
using AutoMapper.QueryableExtensions;
using DLL;
using DLL.Entity.QuanLyHoaDon;
using Microsoft.EntityFrameworkCore;
using Services.Helper;
using Services.Repositories.Interfaces.Config;
using Services.Repositories.Interfaces.DanhMuc;
using Services.Repositories.Interfaces.QuanLyHoaDon;
using Services.ViewModels.Config;
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
        Datacontext _db;
        IMapper _mp;
        ITuyChonService _tuyChonService;
        ILoaiTienService _LoaiTienService;
        ITruongDuLieuMoRongService _truongDuLieuMoRongService;

        public HoaDonDienTuChiTietService(
            Datacontext datacontext,
            IMapper mapper,
            ITuyChonService tuyChonService,
            ITruongDuLieuMoRongService truongDuLieuMoRongService)
        {
            _db = datacontext;
            _mp = mapper;
            _tuyChonService = tuyChonService;
            _truongDuLieuMoRongService = truongDuLieuMoRongService;
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
            var loaiTien = _db.LoaiTiens.FirstOrDefault(x => x.LoaiTienId == hoaDonDienTuVM.LoaiTienId);
            if (list.Count > 0)
            {
                //TuyChonViewModel tuyChonVM = await _tuyChonService.GetDetailAsync("IntPPTTGXuatQuy");
                //bool isVND = tienViet.LoaiTienId == hoaDonDienTuVM.LoaiTienId;

                int count = 1;

                var range = new List<TruongDuLieuMoRongViewModel>();

                foreach (var item in list)
                {
                    TruongDuLieuMoRongViewModel truongMoRongChiTiet1 = item.TruongMoRongChiTiet1;
                    TruongDuLieuMoRongViewModel truongMoRongChiTiet2 = item.TruongMoRongChiTiet2;
                    TruongDuLieuMoRongViewModel truongMoRongChiTiet3 = item.TruongMoRongChiTiet3;
                    TruongDuLieuMoRongViewModel truongMoRongChiTiet4 = item.TruongMoRongChiTiet4;
                    TruongDuLieuMoRongViewModel truongMoRongChiTiet5 = item.TruongMoRongChiTiet5;
                    TruongDuLieuMoRongViewModel truongMoRongChiTiet6 = item.TruongMoRongChiTiet6;
                    TruongDuLieuMoRongViewModel truongMoRongChiTiet7 = item.TruongMoRongChiTiet7;
                    TruongDuLieuMoRongViewModel truongMoRongChiTiet8 = item.TruongMoRongChiTiet8;
                    TruongDuLieuMoRongViewModel truongMoRongChiTiet9 = item.TruongMoRongChiTiet9;
                    TruongDuLieuMoRongViewModel truongMoRongChiTiet10 = item.TruongMoRongChiTiet10;

                    item.TruongMoRongChiTiet1Id = truongMoRongChiTiet1.Id = Guid.NewGuid().ToString();
                    item.TruongMoRongChiTiet2Id = truongMoRongChiTiet2.Id = Guid.NewGuid().ToString();
                    item.TruongMoRongChiTiet3Id = truongMoRongChiTiet3.Id = Guid.NewGuid().ToString();
                    item.TruongMoRongChiTiet4Id = truongMoRongChiTiet4.Id = Guid.NewGuid().ToString();
                    item.TruongMoRongChiTiet5Id = truongMoRongChiTiet5.Id = Guid.NewGuid().ToString();
                    item.TruongMoRongChiTiet6Id = truongMoRongChiTiet6.Id = Guid.NewGuid().ToString();
                    item.TruongMoRongChiTiet7Id = truongMoRongChiTiet7.Id = Guid.NewGuid().ToString();
                    item.TruongMoRongChiTiet8Id = truongMoRongChiTiet8.Id = Guid.NewGuid().ToString();
                    item.TruongMoRongChiTiet9Id = truongMoRongChiTiet9.Id = Guid.NewGuid().ToString();
                    item.TruongMoRongChiTiet10Id = truongMoRongChiTiet10.Id = Guid.NewGuid().ToString();

                    item.TruongMoRongChiTiet1 = null;
                    item.TruongMoRongChiTiet2 = null;
                    item.TruongMoRongChiTiet3 = null;
                    item.TruongMoRongChiTiet4 = null;
                    item.TruongMoRongChiTiet5 = null;
                    item.TruongMoRongChiTiet6 = null;
                    item.TruongMoRongChiTiet7 = null;
                    item.TruongMoRongChiTiet8 = null;
                    item.TruongMoRongChiTiet9 = null;
                    item.TruongMoRongChiTiet10 = null;
                    item.HoaDonDienTuId = hoaDonDienTuVM.HoaDonDienTuId;
                    item.SoLuong = item.SoLuong ?? 0;
                    item.DonGia = item.DonGia ?? 0;
                    item.DonGiaQuyDoi = item.DonGiaQuyDoi ?? 0;
                    item.TienChietKhau = item.TienChietKhau ?? 0;
                    item.TienChietKhauQuyDoi = item.TienChietKhauQuyDoi ?? 0;
                    item.TienThueGTGT = item.TienThueGTGT ?? 0;
                    item.TienThueGTGTQuyDoi = item.TienThueGTGTQuyDoi ?? 0;
                    item.ThanhTien = item.ThanhTien ?? 0;
                    item.ThanhTienQuyDoi = item.ThanhTienQuyDoi ?? 0;
                    item.TongTienThanhToan = item.ThanhTien - item.TienChietKhau + item.TienThueGTGT;
                    item.TongTienThanhToanQuyDoi = item.ThanhTienQuyDoi - item.TienChietKhauQuyDoi + item.TienThueGTGTQuyDoi;
                    item.CreatedDate = DateTime.Now;
                    item.STT = count;
                    item.Status = true;
                    item.DonViTinh = null;
                    item.HangHoaDichVu = null;
                    item.HoaDon = null;
                    count++;

                    HoaDonDienTuChiTiet hoaDonDienTuChiTiet = _mp.Map<HoaDonDienTuChiTiet>(item);
                    await _db.HoaDonDienTuChiTiets.AddAsync(hoaDonDienTuChiTiet);
                    int countSave = await _db.SaveChangesAsync();
                    item.HoaDonDienTuChiTietId = hoaDonDienTuChiTiet.HoaDonDienTuChiTietId;
                    truongMoRongChiTiet1.DataId = hoaDonDienTuChiTiet.HoaDonDienTuChiTietId;
                    truongMoRongChiTiet2.DataId = hoaDonDienTuChiTiet.HoaDonDienTuChiTietId;
                    truongMoRongChiTiet3.DataId = hoaDonDienTuChiTiet.HoaDonDienTuChiTietId;
                    truongMoRongChiTiet4.DataId = hoaDonDienTuChiTiet.HoaDonDienTuChiTietId;
                    truongMoRongChiTiet5.DataId = hoaDonDienTuChiTiet.HoaDonDienTuChiTietId;
                    truongMoRongChiTiet6.DataId = hoaDonDienTuChiTiet.HoaDonDienTuChiTietId;
                    truongMoRongChiTiet7.DataId = hoaDonDienTuChiTiet.HoaDonDienTuChiTietId;
                    truongMoRongChiTiet8.DataId = hoaDonDienTuChiTiet.HoaDonDienTuChiTietId;
                    truongMoRongChiTiet9.DataId = hoaDonDienTuChiTiet.HoaDonDienTuChiTietId;
                    truongMoRongChiTiet10.DataId = hoaDonDienTuChiTiet.HoaDonDienTuChiTietId;

                    range.Add(truongMoRongChiTiet1);
                    range.Add(truongMoRongChiTiet2);
                    range.Add(truongMoRongChiTiet3);
                    range.Add(truongMoRongChiTiet4);
                    range.Add(truongMoRongChiTiet5);
                    range.Add(truongMoRongChiTiet6);
                    range.Add(truongMoRongChiTiet7);
                    range.Add(truongMoRongChiTiet8);
                    range.Add(truongMoRongChiTiet9);
                    range.Add(truongMoRongChiTiet10);
                }

                List<HoaDonDienTuChiTiet> models = _mp.Map<List<HoaDonDienTuChiTiet>>(list);
                await _db.SaveChangesAsync();
                await _truongDuLieuMoRongService.InsertRangeAsync(range);
                List<HoaDonDienTuChiTietViewModel> result = _mp.Map<List<HoaDonDienTuChiTietViewModel>>(models);
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

        public async Task<List<HoaDonDienTuChiTietViewModel>> GetChiTietHoaDonAsync(string hoaDonId)
        {
            var result = new List<HoaDonDienTuChiTietViewModel>();
            try
            {
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
                            join tmr1 in _db.TruongDuLieuMoRongs on hdct.TruongMoRongChiTiet1Id equals tmr1.Id into tmpTMR1
                            from tmr1 in tmpTMR1.DefaultIfEmpty()
                            join tmr2 in _db.TruongDuLieuMoRongs on hdct.TruongMoRongChiTiet2Id equals tmr2.Id into tmpTMR2
                            from tmr2 in tmpTMR2.DefaultIfEmpty()
                            join tmr3 in _db.TruongDuLieuMoRongs on hdct.TruongMoRongChiTiet3Id equals tmr3.Id into tmpTMR3
                            from tmr3 in tmpTMR3.DefaultIfEmpty()
                            join tmr4 in _db.TruongDuLieuMoRongs on hdct.TruongMoRongChiTiet4Id equals tmr4.Id into tmpTMR4
                            from tmr4 in tmpTMR4.DefaultIfEmpty()
                            join tmr5 in _db.TruongDuLieuMoRongs on hdct.TruongMoRongChiTiet5Id equals tmr5.Id into tmpTMR5
                            from tmr5 in tmpTMR5.DefaultIfEmpty()
                            join tmr6 in _db.TruongDuLieuMoRongs on hdct.TruongMoRongChiTiet6Id equals tmr6.Id into tmpTMR6
                            from tmr6 in tmpTMR6.DefaultIfEmpty()
                            join tmr7 in _db.TruongDuLieuMoRongs on hdct.TruongMoRongChiTiet7Id equals tmr7.Id into tmpTMR7
                            from tmr7 in tmpTMR7.DefaultIfEmpty()
                            join tmr8 in _db.TruongDuLieuMoRongs on hdct.TruongMoRongChiTiet8Id equals tmr8.Id into tmpTMR8
                            from tmr8 in tmpTMR8.DefaultIfEmpty()
                            join tmr9 in _db.TruongDuLieuMoRongs on hdct.TruongMoRongChiTiet9Id equals tmr9.Id into tmpTMR9
                            from tmr9 in tmpTMR9.DefaultIfEmpty()
                            join tmr10 in _db.TruongDuLieuMoRongs on hdct.TruongMoRongChiTiet10Id equals tmr10.Id into tmpTMR10
                            from tmr10 in tmpTMR9.DefaultIfEmpty()
                            where hdct.HoaDonDienTuId == hoaDonId
                            orderby hdct.CreatedDate
                            select new HoaDonDienTuChiTietViewModel
                            {
                                HoaDonDienTuChiTietId = hdct.HoaDonDienTuChiTietId,
                                HoaDonDienTuId = hd.HoaDonDienTuId ?? string.Empty,
                                HoaDon = hd != null ? _mp.Map<HoaDonDienTuViewModel>(hd) : null,
                                HangHoaDichVuId = vt.HangHoaDichVuId ?? string.Empty,
                                HangHoaDichVu = vt != null ? _mp.Map<HangHoaDichVuViewModel>(vt) : null,
                                MaHang = hdct.MaHang,
                                TenHang = hdct.TenHang,
                                HangKhuyenMai = hdct.HangKhuyenMai ?? false,
                                DonViTinhId = dvt.DonViTinhId ?? string.Empty,
                                DonViTinh = dvt != null ? _mp.Map<DonViTinhViewModel>(dvt) : null,
                                SoLuong = hdct.SoLuong,
                                DonGia = hdct.DonGia,
                                DonGiaQuyDoi = hdct.DonGiaQuyDoi,
                                ThanhTien = hdct.ThanhTien,
                                ThanhTienQuyDoi = hdct.ThanhTienQuyDoi,
                                TyLeChietKhau = hdct.TyLeChietKhau,
                                TienChietKhau = hdct.TienChietKhau,
                                TienChietKhauQuyDoi = hdct.TienChietKhauQuyDoi,
                                ThueGTGT = hdct.ThueGTGT,
                                TienThueGTGT = hdct.TienThueGTGT,
                                TienThueGTGTQuyDoi = hdct.TienThueGTGTQuyDoi,
                                TongTienThanhToan = hdct.TongTienThanhToan,
                                TongTienThanhToanQuyDoi = hdct.TongTienThanhToanQuyDoi,
                                SoLo = hdct.SoLo,
                                HanSuDung = hdct.HanSuDung,
                                SoKhung = hdct.SoKhung,
                                SoMay = hdct.SoMay,
                                LoaiTienId = lt != null ? lt.LoaiTienId : null,
                                IsVND = lt != null ? (lt.Ma == "VND") : true,
                                TruongMoRongChiTiet1 = new TruongDuLieuMoRongViewModel
                                {
                                    TenTruongHienThi = tmr1.TenTruongHienThi,
                                    DuLieu = tmr1.DuLieu
                                },
                                TruongMoRongChiTiet2 = new TruongDuLieuMoRongViewModel
                                {
                                    TenTruongHienThi = tmr2.TenTruongHienThi,
                                    DuLieu = tmr2.DuLieu
                                },
                                TruongMoRongChiTiet3 = new TruongDuLieuMoRongViewModel
                                {
                                    TenTruongHienThi = tmr3.TenTruongHienThi,
                                    DuLieu = tmr3.DuLieu
                                },
                                TruongMoRongChiTiet4 = new TruongDuLieuMoRongViewModel
                                {
                                    TenTruongHienThi = tmr4.TenTruongHienThi,
                                    DuLieu = tmr4.DuLieu
                                },
                                TruongMoRongChiTiet5 = new TruongDuLieuMoRongViewModel
                                {
                                    TenTruongHienThi = tmr5.TenTruongHienThi,
                                    DuLieu = tmr5.DuLieu
                                },
                                TruongMoRongChiTiet6 = new TruongDuLieuMoRongViewModel
                                {
                                    TenTruongHienThi = tmr6.TenTruongHienThi,
                                    DuLieu = tmr6.DuLieu
                                },
                                TruongMoRongChiTiet7 = new TruongDuLieuMoRongViewModel
                                {
                                    TenTruongHienThi = tmr7.TenTruongHienThi,
                                    DuLieu = tmr7.DuLieu
                                },
                                TruongMoRongChiTiet8 = new TruongDuLieuMoRongViewModel
                                {
                                    TenTruongHienThi = tmr8.TenTruongHienThi,
                                    DuLieu = tmr8.DuLieu
                                },
                                TruongMoRongChiTiet9 = new TruongDuLieuMoRongViewModel
                                {
                                    TenTruongHienThi = tmr9.TenTruongHienThi,
                                    DuLieu = tmr9.DuLieu
                                },
                                TruongMoRongChiTiet10 = new TruongDuLieuMoRongViewModel
                                {
                                    TenTruongHienThi = tmr10.TenTruongHienThi,
                                    DuLieu = tmr10.DuLieu
                                },
                                CreatedBy = hdct.CreatedBy,
                                CreatedDate = hdct.CreatedDate,
                                Status = hd.Status
                            }).ToListAsync();
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(ex.Message);
            }

            return result;
        }
    }
}
