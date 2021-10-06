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
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1;
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
            var loaiTien = _db.LoaiTiens.FirstOrDefault(x => x.LoaiTienId == hoaDonDienTuVM.LoaiTienId);
            if (list.Count > 0)
            {
                //TuyChonViewModel tuyChonVM = await _tuyChonService.GetDetailAsync("IntPPTTGXuatQuy");
                //bool isVND = tienViet.LoaiTienId == hoaDonDienTuVM.LoaiTienId;

                int count = 1;


                foreach (var item in list)
                {
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
                    await _db.SaveChangesAsync();
                }

                List<HoaDonDienTuChiTiet> models = _mp.Map<List<HoaDonDienTuChiTiet>>(list);
                await _db.SaveChangesAsync();
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
                            Status = hd.Status
                        }).ToListAsync();


            return result;
        }
    }
}
