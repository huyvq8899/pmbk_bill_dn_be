using AutoMapper;
using AutoMapper.QueryableExtensions;
using DLL;
using DLL.Entity.NghiepVu;
using Microsoft.EntityFrameworkCore;
using Services.Enums;
using Services.Repositories.Interfaces.Config;
using Services.Repositories.Interfaces.DanhMuc;
using Services.Repositories.Interfaces.NghiepVu;
using Services.Repositories.Interfaces.QuanLyHoaDon;
using Services.ViewModels.Config;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.NghiepVu;
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
        ISoTienGuiNganHangService _soTienGuiNganHangService;
        ISoCongNoDoiTuongService _soCongNoDoiTuongService;
        ISoCaiService _soCaiService;
        ITaiKhoanKeToanService _taiKhoanKeToanService;
        ITuyChonService _tuyChonService;
        INgoaiTeService _ngoaiTeService;

        public HoaDonDienTuChiTietService(
            Datacontext datacontext,
            IMapper mapper,
            ISoTienGuiNganHangService soTienGuiNganHangService,
            ISoCongNoDoiTuongService soCongNoDoiTuongService,
            ISoCaiService soCaiService,
            ITaiKhoanKeToanService taiKhoanKeToanService,
            ITuyChonService tuyChonService,
            INgoaiTeService ngoaiTeService)
        {
            _db = datacontext;
            _mp = mapper;
            _soTienGuiNganHangService = soTienGuiNganHangService;
            _soCongNoDoiTuongService = soCongNoDoiTuongService;
            _soCaiService = soCaiService;
            _taiKhoanKeToanService = taiKhoanKeToanService;
            _tuyChonService = tuyChonService;
            _ngoaiTeService = ngoaiTeService;
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
                item.VT_HH = null;
                item.DonViTinh = null;
            }

            return main;
        }

        public async Task<List<HoaDonDienTuChiTietViewModel>> InsertRangeAsync(HoaDonDienTuViewModel hoaDonDienTuVM, List<HoaDonDienTuChiTietViewModel> list)
        {
            if (list.Count > 0)
            {
                NgoaiTeViewModel tienViet = await _ngoaiTeService.GetTienVietAsync();
                TuyChonViewModel tuyChonVM = await _tuyChonService.GetDetailAsync("IntPPTTGXuatQuy");
                bool isVND = tienViet.NgoaiTeId == hoaDonDienTuVM.LoaiTienId;

                int count = 1;
                foreach (var item in list)
                {
                    item.HoaDonDienTuId = hoaDonDienTuVM.HoaDonDienTuId;
                    item.SoLuong = item.SoLuong ?? 0;
                    item.DonGia = item.DonGia ?? 0;
                    item.DonGiaQuyDoi = item.DonGiaQuyDoi ?? 0;
                    item.TyLeChietKhau = item.TyLeChietKhau ?? 0;
                    item.TienChietKhau = item.TienChietKhau ?? 0;
                    item.TienChietKhauQuyDoi = item.TienChietKhauQuyDoi ?? 0;
                    item.ThueGTGT = item.ThueGTGT ?? 0;
                    item.TienThueGTGT = item.TienThueGTGT ?? 0;
                    item.TienThueGTGTQuyDoi = item.TienThueGTGTQuyDoi ?? 0;
                    item.ThanhTien = item.SoLuong * item.DonGia - item.TienChietKhau;
                    item.ThanhTienQuyDoi = item.SoLuong * item.DonGiaQuyDoi - item.TienChietKhauQuyDoi;
                    item.CreatedDate = DateTime.Now;
                    item.STT = count;
                    item.Status = true;
                    item.DonViTinh = null;
                    item.VT_HH = null;
                    item.HoaDon = null;
                    count++;

                    HoaDonDienTuChiTiet hoaDonDienTuChiTiet = _mp.Map<HoaDonDienTuChiTiet>(item);
                    await _db.HoaDonDienTuChiTiets.AddAsync(hoaDonDienTuChiTiet);
                    int countSave = await _db.SaveChangesAsync();
                    item.HoaDonDienTuChiTietId = hoaDonDienTuChiTiet.HoaDonDienTuChiTietId;

                //    #region Tài khoản nợ
                //    if (string.IsNullOrEmpty(item.TaiKhoanNo))
                //    {
                //        continue;
                //    }
                //    TaiKhoanKeToanViewModel taiKhoanNo = await _taiKhoanKeToanService.GetBySoTaiKhoanAsync(item.TaiKhoanNo);
                //    if (taiKhoanNo != null)
                //    {
                //        if (taiKhoanNo.LoaiDoiTuong > 0)
                //        {
                //            SoCongNoDoiTuongViewModel soCongNoDoiTuong = new SoCongNoDoiTuongViewModel
                //            {
                //                NgayHachToan = chungTuNghiepVuKhacVM.NgayHachToan.Value,
                //                NgayChungTu = chungTuNghiepVuKhacVM.NgayChungTu.Value,
                //                NgayTao = chungTuNghiepVuKhacVM.CreatedDate,
                //                SoChungTu = chungTuNghiepVuKhacVM.SoChungTu,
                //                HanThanhToan = chungTuNghiepVuKhacVM.HanThanhToan,
                //                NhanVienId = item.NhanVienId,
                //                LyDo = chungTuNghiepVuKhacVM.DienGiai,
                //                DienGiai = item.DienGiai,
                //                TKCongNo = item.TaiKhoanNo,
                //                TKDoiUng = item.TaiKhoanCo,
                //                PhatSinhNo = chungTuNghiepVuKhacVM.IsChungTuXLCLTGTuTinhTGXQ == true ? item.ChenhLech : soTien,
                //                PhatSinhCo = 0,
                //                PhatSinhNoNgoaiTe = chungTuNghiepVuKhacVM.IsChungTuXLCLTGTuTinhTGXQ == true ? 0 : (isVND == true ? soTien : (taiKhoanNo.CoHachToanNgoaiTe == true ? item.SoTienNgoaiTe : soTien)),
                //                PhatSinhCoNgoaiTe = 0,
                //                SoDuCo = 0,
                //                SoDuNo = 0,
                //                SoDuCoNgoaiTe = 0,
                //                SoDuNoNgoaiTe = 0,
                //                NgoaiTeId = chungTuNghiepVuKhacVM.IsChungTuXLCLTGTuTinhTGXQ == true ? (taiKhoanNo.CoHachToanNgoaiTe == true ? item.NgoaiTeId : tienViet.NgoaiTeId) : (isVND == true ? tienViet.NgoaiTeId : (taiKhoanNo.CoHachToanNgoaiTe == true ? chungTuNghiepVuKhacVM.NgoaiTeId : tienViet.NgoaiTeId)),
                //                TyGia = chungTuNghiepVuKhacVM.IsChungTuXLCLTGTuTinhTGXQ == true ? (taiKhoanNo.CoHachToanNgoaiTe == true ? item.TyGiaXuatQuy : tienViet.TyGiaQuyDoi) : (isVND == true ? tienViet.TyGiaQuyDoi : (taiKhoanNo.CoHachToanNgoaiTe == true ? chungTuNghiepVuKhacVM.TyGia : tienViet.TyGiaQuyDoi)),
                //                IsTonDauKy = false,
                //                ChungTuId = chungTuNghiepVuKhacVM.ChungTuNghiepVuKhacId,
                //                ChungTuChiTietId = item.ChungTuNghiepVuKhacChiTietId,
                //                Loai = (int)BusinessOfType.CHUNG_TU_NGHIEP_VU_KHAC, // Chứng từ nvk
                //                TinhChat = 1, // Dư nợ
                //                DoiTuongId = item.DoiTuongNoId, // bắt buộc
                //                CreatedDate = DateTime.Now,
                //                Status = true
                //            };

                //            await _soCongNoDoiTuongService.InsertAsync(soCongNoDoiTuong);
                //        }
                //        if (taiKhoanNo.IsTaiKhoanNganHang == true)
                //        {
                //            SoTienGuiNganHangViewModel soTienGuiNganHang = new SoTienGuiNganHangViewModel
                //            {
                //                NgayHachToan = chungTuNghiepVuKhacVM.NgayHachToan.Value,
                //                NgayChungTu = chungTuNghiepVuKhacVM.NgayChungTu.Value,
                //                NgayTao = chungTuNghiepVuKhacVM.CreatedDate,
                //                SoChungTu = chungTuNghiepVuKhacVM.SoChungTu,
                //                LyDo = chungTuNghiepVuKhacVM.DienGiai,
                //                DienGiai = item.DienGiai,
                //                TaiKhoan = item.TaiKhoanNo,
                //                TaiKhoanDoiUng = item.TaiKhoanCo,
                //                Thu = chungTuNghiepVuKhacVM.IsChungTuXLCLTGTuTinhTGXQ == true ? item.ChenhLech : soTien,
                //                Chi = 0,
                //                ThuNgoaiTe = chungTuNghiepVuKhacVM.IsChungTuXLCLTGTuTinhTGXQ == true ? 0 : (isVND == true ? soTien : (taiKhoanNo.CoHachToanNgoaiTe == true ? item.SoTienNgoaiTe : soTien)),
                //                ChiNgoaiTe = 0,
                //                Ton = 0,
                //                TonNgoaiTe = 0,
                //                NgoaiTeId = chungTuNghiepVuKhacVM.IsChungTuXLCLTGTuTinhTGXQ == true ? (taiKhoanNo.CoHachToanNgoaiTe == true ? item.NgoaiTeId : tienViet.NgoaiTeId) : (isVND == true ? tienViet.NgoaiTeId : (taiKhoanNo.CoHachToanNgoaiTe == true ? chungTuNghiepVuKhacVM.NgoaiTeId : tienViet.NgoaiTeId)),
                //                TyGia = chungTuNghiepVuKhacVM.IsChungTuXLCLTGTuTinhTGXQ == true ? (taiKhoanNo.CoHachToanNgoaiTe == true ? item.TyGiaXuatQuy : tienViet.TyGiaQuyDoi) : (isVND == true ? tienViet.TyGiaQuyDoi : (taiKhoanNo.CoHachToanNgoaiTe == true ? chungTuNghiepVuKhacVM.TyGia : tienViet.TyGiaQuyDoi)),
                //                TaiKhoanNganHangId = item.TaiKhoanNganHangId,
                //                IsTonDauKy = false,
                //                ChungTuId = chungTuNghiepVuKhacVM.ChungTuNghiepVuKhacId,
                //                ChungTuChiTietId = item.ChungTuNghiepVuKhacChiTietId,
                //                LoaiChungTu = (int)BusinessOfType.CHUNG_TU_NGHIEP_VU_KHAC, // Chứng từ nvk
                //                TinhChat = 1, // Dư nợ
                //                CreatedDate = DateTime.Now,
                //                Status = true
                //            };

                //            await _soTienGuiNganHangService.InsertAsync(soTienGuiNganHang);
                //        }
                //    }
                //    #endregion

                //    #region Tài khoản có
                //    if (string.IsNullOrEmpty(item.TaiKhoanCo))
                //    {
                //        continue;
                //    }
                //    TaiKhoanKeToanViewModel taiKhoanCo = await _taiKhoanKeToanService.GetBySoTaiKhoanAsync(item.TaiKhoanCo);
                //    if (taiKhoanCo != null)
                //    {
                //        if (taiKhoanCo.LoaiDoiTuong > 0)
                //        {
                //            SoCongNoDoiTuongViewModel soCongNoDoiTuong = new SoCongNoDoiTuongViewModel
                //            {
                //                NgayHachToan = chungTuNghiepVuKhacVM.NgayHachToan.Value,
                //                NgayChungTu = chungTuNghiepVuKhacVM.NgayChungTu.Value,
                //                NgayTao = chungTuNghiepVuKhacVM.CreatedDate,
                //                SoChungTu = chungTuNghiepVuKhacVM.SoChungTu,
                //                HanThanhToan = chungTuNghiepVuKhacVM.HanThanhToan,
                //                NhanVienId = item.NhanVienId,
                //                LyDo = chungTuNghiepVuKhacVM.DienGiai,
                //                DienGiai = item.DienGiai,
                //                TKCongNo = item.TaiKhoanCo,
                //                TKDoiUng = item.TaiKhoanNo,
                //                PhatSinhNo = 0,
                //                PhatSinhCo = chungTuNghiepVuKhacVM.IsChungTuXLCLTGTuTinhTGXQ == true ? item.ChenhLech : soTien,
                //                PhatSinhNoNgoaiTe = 0,
                //                PhatSinhCoNgoaiTe = chungTuNghiepVuKhacVM.IsChungTuXLCLTGTuTinhTGXQ == true ? 0 : (isVND == true ? soTien : (taiKhoanCo.CoHachToanNgoaiTe == true ? item.SoTienNgoaiTe : soTien)),
                //                SoDuCo = 0,
                //                SoDuNo = 0,
                //                SoDuCoNgoaiTe = 0,
                //                SoDuNoNgoaiTe = 0,
                //                NgoaiTeId = chungTuNghiepVuKhacVM.IsChungTuXLCLTGTuTinhTGXQ == true ? (taiKhoanCo.CoHachToanNgoaiTe == true ? item.NgoaiTeId : tienViet.NgoaiTeId) : (isVND == true ? tienViet.NgoaiTeId : (taiKhoanCo.CoHachToanNgoaiTe == true ? chungTuNghiepVuKhacVM.NgoaiTeId : tienViet.NgoaiTeId)),
                //                TyGia = chungTuNghiepVuKhacVM.IsChungTuXLCLTGTuTinhTGXQ == true ? (taiKhoanCo.CoHachToanNgoaiTe == true ? item.TyGiaXuatQuy : tienViet.TyGiaQuyDoi) : (isVND == true ? tienViet.TyGiaQuyDoi : (taiKhoanCo.CoHachToanNgoaiTe == true ? tyGia : tienViet.TyGiaQuyDoi)),
                //                IsTonDauKy = false,
                //                ChungTuId = chungTuNghiepVuKhacVM.ChungTuNghiepVuKhacId,
                //                ChungTuChiTietId = item.ChungTuNghiepVuKhacChiTietId,
                //                Loai = (int)BusinessOfType.CHUNG_TU_NGHIEP_VU_KHAC, // chứng từ nghiệp vụ khác
                //                TinhChat = 2, // Dư có
                //                DoiTuongId = item.DoiTuongCoId, // bắt buộc
                //                CreatedDate = DateTime.Now,
                //                Status = true
                //            };

                //            await _soCongNoDoiTuongService.InsertAsync(soCongNoDoiTuong);

                //            if (!string.IsNullOrEmpty(item.TKThueGTGT))
                //            {
                //                SoCongNoDoiTuongViewModel soCongNoDoiTuongThue = new SoCongNoDoiTuongViewModel
                //                {
                //                    NgayHachToan = chungTuNghiepVuKhacVM.NgayHachToan.Value,
                //                    NgayChungTu = chungTuNghiepVuKhacVM.NgayChungTu.Value,
                //                    SoChungTu = chungTuNghiepVuKhacVM.SoChungTu,
                //                    HanThanhToan = chungTuNghiepVuKhacVM.HanThanhToan,
                //                    NhanVienId = item.NhanVienId,
                //                    LyDo = chungTuNghiepVuKhacVM.DienGiai,
                //                    DienGiai = item.DienGiaiThue,
                //                    TKCongNo = item.TaiKhoanCo,
                //                    TKDoiUng = item.TKThueGTGT,
                //                    PhatSinhNo = 0,
                //                    PhatSinhCo = item.TienThueGTGT,
                //                    PhatSinhNoNgoaiTe = 0,
                //                    PhatSinhCoNgoaiTe = isVND == true ? item.TienThueGTGT : (taiKhoanCo.CoHachToanNgoaiTe == true ? item.TienThueGTGTNgoaiTe : item.TienThueGTGT),
                //                    SoDuCo = 0,
                //                    SoDuNo = 0,
                //                    SoDuCoNgoaiTe = 0,
                //                    SoDuNoNgoaiTe = 0,
                //                    NgoaiTeId = isVND == true ? tienViet.NgoaiTeId : (taiKhoanCo.CoHachToanNgoaiTe == true ? chungTuNghiepVuKhacVM.NgoaiTeId : tienViet.NgoaiTeId),
                //                    TyGia = isVND == true ? tienViet.TyGiaQuyDoi : (taiKhoanCo.CoHachToanNgoaiTe == true ? chungTuNghiepVuKhacVM.TyGia : tienViet.TyGiaQuyDoi),
                //                    IsTonDauKy = false,
                //                    ChungTuId = chungTuNghiepVuKhacVM.ChungTuNghiepVuKhacId,
                //                    ChungTuChiTietId = item.ChungTuNghiepVuKhacChiTietId,
                //                    Loai = (int)BusinessOfType.CHUNG_TU_NGHIEP_VU_KHAC, // chứng từ nghiệp vụ khác
                //                    TinhChat = 2, // Dư có
                //                    DoiTuongId = item.DoiTuongCoId, // bắt buộc
                //                    CreatedDate = DateTime.Now,
                //                    Status = true
                //                };

                //                await _soCongNoDoiTuongService.InsertAsync(soCongNoDoiTuongThue);
                //            }
                //        }
                //        if (taiKhoanCo.IsTaiKhoanNganHang == true)
                //        {
                //            SoTienGuiNganHangViewModel soTienGuiNganHangCoVM = new SoTienGuiNganHangViewModel
                //            {
                //                NgayHachToan = chungTuNghiepVuKhacVM.NgayHachToan.Value,
                //                NgayChungTu = chungTuNghiepVuKhacVM.NgayChungTu.Value,
                //                NgayTao = chungTuNghiepVuKhacVM.CreatedDate,
                //                SoChungTu = chungTuNghiepVuKhacVM.SoChungTu,
                //                LyDo = chungTuNghiepVuKhacVM.DienGiai,
                //                DienGiai = item.DienGiai,
                //                TaiKhoan = item.TaiKhoanCo,
                //                TaiKhoanDoiUng = item.TaiKhoanNo,
                //                Thu = 0,
                //                Chi = chungTuNghiepVuKhacVM.IsChungTuXLCLTGTuTinhTGXQ == true ? item.ChenhLech : soTien,
                //                ThuNgoaiTe = 0,
                //                ChiNgoaiTe = chungTuNghiepVuKhacVM.IsChungTuXLCLTGTuTinhTGXQ == true ? 0 : (isVND == true ? soTien : (taiKhoanCo.CoHachToanNgoaiTe == true ? item.SoTienNgoaiTe : soTien)),
                //                Ton = 0,
                //                TonNgoaiTe = 0,
                //                NgoaiTeId = chungTuNghiepVuKhacVM.IsChungTuXLCLTGTuTinhTGXQ == true ? (taiKhoanCo.CoHachToanNgoaiTe == true ? item.NgoaiTeId : tienViet.NgoaiTeId) : (isVND == true ? tienViet.NgoaiTeId : (taiKhoanCo.CoHachToanNgoaiTe == true ? chungTuNghiepVuKhacVM.NgoaiTeId : tienViet.NgoaiTeId)),
                //                TyGia = chungTuNghiepVuKhacVM.IsChungTuXLCLTGTuTinhTGXQ == true ? (taiKhoanCo.CoHachToanNgoaiTe == true ? item.TyGiaXuatQuy : tienViet.TyGiaQuyDoi) : (isVND == true ? tienViet.TyGiaQuyDoi : (taiKhoanCo.CoHachToanNgoaiTe == true ? tyGia : tienViet.TyGiaQuyDoi)),
                //                TaiKhoanNganHangId = item.TaiKhoanNganHangId,
                //                IsTonDauKy = false,
                //                ChungTuId = chungTuNghiepVuKhacVM.ChungTuNghiepVuKhacId,
                //                ChungTuChiTietId = item.ChungTuNghiepVuKhacChiTietId,
                //                LoaiChungTu = (int)BusinessOfType.CHUNG_TU_NGHIEP_VU_KHAC, // Chứng từ nvk
                //                TinhChat = 2, // Dư có
                //                CreatedDate = DateTime.Now,
                //                Status = true
                //            };

                //            await _soTienGuiNganHangService.InsertAsync(soTienGuiNganHangCoVM);
                //        }
                //    }
                //    #endregion

                //    SoCaiViewModel socaiModel = new SoCaiViewModel
                //    {
                //        RefNghiepVuID = chungTuNghiepVuKhacVM.ChungTuNghiepVuKhacId,
                //        RefNghiepVuChiTietID = item.ChungTuNghiepVuKhacChiTietId,
                //        RefOfType = BusinessOfType.CHUNG_TU_NGHIEP_VU_KHAC,
                //        NgayHachToan = chungTuNghiepVuKhacVM.NgayHachToan.Value,
                //        NgayChungTu = chungTuNghiepVuKhacVM.NgayChungTu.Value,
                //        NgayTao = chungTuNghiepVuKhacVM.CreatedDate,
                //        SoChungTu = chungTuNghiepVuKhacVM.SoChungTu,
                //        SoTaiKhoan = item.TaiKhoanNo,
                //        SoTaiKhoanDoiUng = item.TaiKhoanCo,
                //        PhatSinhNo = chungTuNghiepVuKhacVM.IsChungTuXLCLTGTuTinhTGXQ == true ? item.ChenhLech : soTien,
                //        PhatSinhCo = 0,
                //        PhatSinhNoNgoaiTe = chungTuNghiepVuKhacVM.IsChungTuXLCLTGTuTinhTGXQ == true ? 0 : item.SoTienNgoaiTe,
                //        PhatSinhCoNgoaiTe = 0,
                //        NgoaiTeId = chungTuNghiepVuKhacVM.IsChungTuXLCLTGTuTinhTGXQ == true ? item.NgoaiTeId : chungTuNghiepVuKhacVM.NgoaiTeId,
                //        TyGia = chungTuNghiepVuKhacVM.IsChungTuXLCLTGTuTinhTGXQ == true ? item.TyGiaXuatQuy : tyGia,
                //        TyGiaTrenPhieu = chungTuNghiepVuKhacVM.IsChungTuXLCLTGTuTinhTGXQ == true ? null : chungTuNghiepVuKhacVM.TyGia,
                //        LyDo = chungTuNghiepVuKhacVM.DienGiai,
                //        DienGiai = item.DienGiai
                //    };
                //    socaiModel.DTTHCPId = item.DTTHCPId;
                //    socaiModel.CongTrinhId = item.CongTrinhId;
                //    socaiModel.DonDatHangId = item.DonDatHangId;
                //    socaiModel.HopDongBanId = item.HopDongBanId;
                //    socaiModel.HopDongMuaId = item.HopDongMuaId;
                //    socaiModel.KhoanMucChiPhiId = item.KhoanMucChiPhiId;
                //    socaiModel.MaThongKeId = item.MaThongKeId;
                //    socaiModel.MucThuChiId = item.MucThuChiId;
                //    socaiModel.CoCauToChucID = item.CoCauToChucId;
                //    socaiModel.CPKhongHopLy = item.CPKhongHopLy;
                //    _soCaiService.Add(socaiModel);

                //    if (!string.IsNullOrEmpty(item.TKThueGTGT))
                //    {
                //        SoCaiViewModel socaiModelThue = (SoCaiViewModel)socaiModel.Clone();
                //        socaiModelThue.DienGiai = item.DienGiaiThue;
                //        socaiModelThue.SoTaiKhoan = item.TKThueGTGT;
                //        socaiModelThue.SoTaiKhoanDoiUng = item.TaiKhoanCo;
                //        socaiModelThue.PhatSinhNo = item.TienThueGTGT;
                //        socaiModelThue.PhatSinhCo = 0;
                //        socaiModelThue.PhatSinhNoNgoaiTe = item.TienThueGTGTNgoaiTe;
                //        socaiModelThue.PhatSinhCoNgoaiTe = 0;
                //        _soCaiService.Add(socaiModelThue);
                //    }

                //    #region Ghi sổ khi có chênh lệch
                //    if (!string.IsNullOrEmpty(item.TaiKhoanXuLyChenhLech))
                //    {
                //        if (tuyChonVM != null && tuyChonVM.GiaTri == "1")
                //        {
                //            string dienGiaiChenhLech = "Xử lý chênh lệch tỷ giá";
                //            if (item.ChenhLech < 0 && taiKhoanNo != null)
                //            {
                //                if (taiKhoanNo.LoaiDoiTuong > 0)
                //                {
                //                    SoCongNoDoiTuongViewModel soCongNoDoiTuong = new SoCongNoDoiTuongViewModel
                //                    {
                //                        NgayHachToan = chungTuNghiepVuKhacVM.NgayHachToan.Value,
                //                        NgayChungTu = chungTuNghiepVuKhacVM.NgayChungTu.Value,
                //                        NgayTao = chungTuNghiepVuKhacVM.CreatedDate,
                //                        SoChungTu = chungTuNghiepVuKhacVM.SoChungTu,
                //                        HanThanhToan = chungTuNghiepVuKhacVM.HanThanhToan,
                //                        NhanVienId = item.NhanVienId,
                //                        LyDo = chungTuNghiepVuKhacVM.DienGiai,
                //                        DienGiai = dienGiaiChenhLech,
                //                        TKCongNo = item.TaiKhoanNo,
                //                        TKDoiUng = item.TaiKhoanXuLyChenhLech,
                //                        PhatSinhNo = item.ChenhLech * -1,
                //                        PhatSinhCo = 0,
                //                        PhatSinhNoNgoaiTe = taiKhoanNo.CoHachToanNgoaiTe == true ? 0 : (item.ChenhLech * -1),
                //                        PhatSinhCoNgoaiTe = 0,
                //                        SoDuCo = 0,
                //                        SoDuNo = 0,
                //                        SoDuCoNgoaiTe = 0,
                //                        SoDuNoNgoaiTe = 0,
                //                        NgoaiTeId = taiKhoanNo.CoHachToanNgoaiTe == true ? chungTuNghiepVuKhacVM.NgoaiTeId : tienViet.NgoaiTeId,
                //                        TyGia = taiKhoanNo.CoHachToanNgoaiTe == true ? chungTuNghiepVuKhacVM.TyGia : tienViet.TyGiaQuyDoi,
                //                        IsTonDauKy = false,
                //                        ChungTuId = chungTuNghiepVuKhacVM.ChungTuNghiepVuKhacId,
                //                        ChungTuChiTietId = item.ChungTuNghiepVuKhacChiTietId,
                //                        Loai = (int)BusinessOfType.CHUNG_TU_NGHIEP_VU_KHAC, // chứng từ nghiệp vụ khác
                //                        TinhChat = 1, // Dư có
                //                        DoiTuongId = item.DoiTuongNoId, // bắt buộc
                //                        CreatedDate = DateTime.Now,
                //                        Status = true
                //                    };

                //                    await _soCongNoDoiTuongService.InsertAsync(soCongNoDoiTuong);
                //                }
                //                if (taiKhoanNo.IsTaiKhoanNganHang == true)
                //                {
                //                    SoTienGuiNganHangViewModel soTienGuiNganHang = new SoTienGuiNganHangViewModel
                //                    {
                //                        NgayHachToan = chungTuNghiepVuKhacVM.NgayHachToan.Value,
                //                        NgayChungTu = chungTuNghiepVuKhacVM.NgayChungTu.Value,
                //                        NgayTao = chungTuNghiepVuKhacVM.CreatedDate,
                //                        SoChungTu = chungTuNghiepVuKhacVM.SoChungTu,
                //                        LyDo = chungTuNghiepVuKhacVM.DienGiai,
                //                        DienGiai = dienGiaiChenhLech,
                //                        TaiKhoan = item.TaiKhoanNo,
                //                        TaiKhoanDoiUng = item.TaiKhoanXuLyChenhLech,
                //                        Thu = item.ChenhLech * -1,
                //                        Chi = 0,
                //                        ThuNgoaiTe = taiKhoanNo.CoHachToanNgoaiTe == true ? 0 : (item.ChenhLech * -1),
                //                        ChiNgoaiTe = 0,
                //                        Ton = 0,
                //                        TonNgoaiTe = 0,
                //                        NgoaiTeId = taiKhoanNo.CoHachToanNgoaiTe == true ? chungTuNghiepVuKhacVM.NgoaiTeId : tienViet.NgoaiTeId,
                //                        TyGia = taiKhoanNo.CoHachToanNgoaiTe == true ? chungTuNghiepVuKhacVM.TyGia : tienViet.TyGiaQuyDoi,
                //                        TaiKhoanNganHangId = item.TaiKhoanNganHangId,
                //                        IsTonDauKy = false,
                //                        ChungTuId = chungTuNghiepVuKhacVM.ChungTuNghiepVuKhacId,
                //                        ChungTuChiTietId = item.ChungTuNghiepVuKhacChiTietId,
                //                        LoaiChungTu = (int)BusinessOfType.CHUNG_TU_NGHIEP_VU_KHAC, // Chứng từ nvk
                //                        TinhChat = 1, // Dư nợ
                //                        CreatedDate = DateTime.Now,
                //                        Status = true
                //                    };

                //                    await _soTienGuiNganHangService.InsertAsync(soTienGuiNganHang);
                //                }

                //                SoCaiViewModel socaiModelChenhLech = (SoCaiViewModel)socaiModel.Clone();
                //                socaiModelChenhLech.DienGiai = dienGiaiChenhLech;
                //                socaiModelChenhLech.SoTaiKhoan = item.TaiKhoanNo;
                //                socaiModelChenhLech.SoTaiKhoanDoiUng = item.TaiKhoanXuLyChenhLech;
                //                socaiModelChenhLech.PhatSinhNo = item.ChenhLech * -1;
                //                socaiModelChenhLech.PhatSinhCo = 0;
                //                socaiModelChenhLech.PhatSinhNoNgoaiTe = 0;
                //                socaiModelChenhLech.PhatSinhCoNgoaiTe = 0;
                //                socaiModelChenhLech.NgoaiTeId = chungTuNghiepVuKhacVM.NgoaiTeId;
                //                socaiModelChenhLech.TyGia = chungTuNghiepVuKhacVM.TyGia;
                //                _soCaiService.Add(socaiModelChenhLech);
                //            }
                //            if (item.ChenhLech > 0 && taiKhoanCo != null)
                //            {
                //                if (taiKhoanCo.LoaiDoiTuong > 0)
                //                {
                //                    SoCongNoDoiTuongViewModel soCongNoDoiTuong = new SoCongNoDoiTuongViewModel
                //                    {
                //                        NgayHachToan = chungTuNghiepVuKhacVM.NgayHachToan.Value,
                //                        NgayChungTu = chungTuNghiepVuKhacVM.NgayChungTu.Value,
                //                        NgayTao = chungTuNghiepVuKhacVM.CreatedDate,
                //                        SoChungTu = chungTuNghiepVuKhacVM.SoChungTu,
                //                        HanThanhToan = chungTuNghiepVuKhacVM.HanThanhToan,
                //                        NhanVienId = item.NhanVienId,
                //                        LyDo = chungTuNghiepVuKhacVM.DienGiai,
                //                        DienGiai = dienGiaiChenhLech,
                //                        TKCongNo = item.TaiKhoanCo,
                //                        TKDoiUng = item.TaiKhoanXuLyChenhLech,
                //                        PhatSinhNo = 0,
                //                        PhatSinhCo = item.ChenhLech,
                //                        PhatSinhNoNgoaiTe = 0,
                //                        PhatSinhCoNgoaiTe = taiKhoanCo.CoHachToanNgoaiTe == true ? 0 : item.ChenhLech,
                //                        SoDuCo = 0,
                //                        SoDuNo = 0,
                //                        SoDuCoNgoaiTe = 0,
                //                        SoDuNoNgoaiTe = 0,
                //                        NgoaiTeId = taiKhoanCo.CoHachToanNgoaiTe == true ? chungTuNghiepVuKhacVM.NgoaiTeId : tienViet.NgoaiTeId,
                //                        TyGia = taiKhoanCo.CoHachToanNgoaiTe == true ? item.TyGiaXuatQuy : tienViet.TyGiaQuyDoi,
                //                        IsTonDauKy = false,
                //                        ChungTuId = chungTuNghiepVuKhacVM.ChungTuNghiepVuKhacId,
                //                        ChungTuChiTietId = item.ChungTuNghiepVuKhacChiTietId,
                //                        Loai = (int)BusinessOfType.CHUNG_TU_NGHIEP_VU_KHAC, // chứng từ nghiệp vụ khác
                //                        TinhChat = 2, // Dư có
                //                        DoiTuongId = item.DoiTuongCoId, // bắt buộc
                //                        CreatedDate = DateTime.Now,
                //                        Status = true
                //                    };

                //                    await _soCongNoDoiTuongService.InsertAsync(soCongNoDoiTuong);
                //                }
                //                if (taiKhoanCo.IsTaiKhoanNganHang == true)
                //                {
                //                    SoTienGuiNganHangViewModel soTienGuiNganHangCoVM = new SoTienGuiNganHangViewModel
                //                    {
                //                        NgayHachToan = chungTuNghiepVuKhacVM.NgayHachToan.Value,
                //                        NgayChungTu = chungTuNghiepVuKhacVM.NgayChungTu.Value,
                //                        NgayTao = chungTuNghiepVuKhacVM.CreatedDate,
                //                        SoChungTu = chungTuNghiepVuKhacVM.SoChungTu,
                //                        LyDo = chungTuNghiepVuKhacVM.DienGiai,
                //                        DienGiai = dienGiaiChenhLech,
                //                        TaiKhoan = item.TaiKhoanCo,
                //                        TaiKhoanDoiUng = item.TaiKhoanXuLyChenhLech,
                //                        Thu = 0,
                //                        Chi = item.ChenhLech,
                //                        ThuNgoaiTe = 0,
                //                        ChiNgoaiTe = taiKhoanCo.CoHachToanNgoaiTe == true ? 0 : item.ChenhLech,
                //                        Ton = 0,
                //                        TonNgoaiTe = 0,
                //                        NgoaiTeId = taiKhoanCo.CoHachToanNgoaiTe == true ? chungTuNghiepVuKhacVM.NgoaiTeId : tienViet.NgoaiTeId,
                //                        TyGia = taiKhoanCo.CoHachToanNgoaiTe == true ? item.TyGiaXuatQuy : tienViet.TyGiaQuyDoi,
                //                        TaiKhoanNganHangId = item.TaiKhoanNganHangId,
                //                        IsTonDauKy = false,
                //                        ChungTuId = chungTuNghiepVuKhacVM.ChungTuNghiepVuKhacId,
                //                        ChungTuChiTietId = item.ChungTuNghiepVuKhacChiTietId,
                //                        LoaiChungTu = (int)BusinessOfType.CHUNG_TU_NGHIEP_VU_KHAC, // Chứng từ nvk
                //                        TinhChat = 2, // Dư có
                //                        CreatedDate = DateTime.Now,
                //                        Status = true
                //                    };

                //                    await _soTienGuiNganHangService.InsertAsync(soTienGuiNganHangCoVM);
                //                }

                //                SoCaiViewModel socaiModelChenhLech = (SoCaiViewModel)socaiModel.Clone();
                //                socaiModelChenhLech.DienGiai = dienGiaiChenhLech;
                //                socaiModelChenhLech.SoTaiKhoan = item.TaiKhoanCo;
                //                socaiModelChenhLech.SoTaiKhoanDoiUng = item.TaiKhoanXuLyChenhLech;
                //                socaiModelChenhLech.PhatSinhNo = 0;
                //                socaiModelChenhLech.PhatSinhCo = item.ChenhLech;
                //                socaiModelChenhLech.PhatSinhNoNgoaiTe = 0;
                //                socaiModelChenhLech.PhatSinhCoNgoaiTe = 0;
                //                socaiModelChenhLech.NgoaiTeId = chungTuNghiepVuKhacVM.NgoaiTeId;
                //                socaiModelChenhLech.TyGia = item.TyGiaXuatQuy;
                //                _soCaiService.Add(socaiModelChenhLech);
                //            }
                //        }
                //    }
                //    #endregion
                //}

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
    }
}
