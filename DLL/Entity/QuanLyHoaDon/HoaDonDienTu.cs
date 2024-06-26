﻿using DLL.Entity.DanhMuc;
using DLL.Entity.QuanLy;
using DLL.Enums;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace DLL.Entity.QuanLyHoaDon
{
    public class HoaDonDienTu : ThongTinChung
    {
        public string HoaDonDienTuId { get; set; }
        public DateTime? NgayHoaDon { get; set; }
        public long? SoHoaDon { get; set; }
        public string MauHoaDonId { get; set; }
        public bool? IsNopThueTheoThongTu1032014BTC { get; set; }
        public MauHoaDon MauHoaDon { get; set; }
        public string BoKyHieuHoaDonId { get; set; }
        public BoKyHieuHoaDon BoKyHieuHoaDon { get; set; }
        public int? LoaiThue { get; set; } // 1: một thuế suất, 2: nhiều thuế suất
        public string MauSo { get; set; }
        public string KyHieu { get; set; }
        public string KhachHangId { get; set; }
        public DoiTuong KhachHang { get; set; }
        public string MaKhachHang { get; set; }
        public string TenKhachHang { get; set; }
        public string DiaChi { get; set; }
        public string MaSoThue { get; set; }
        public string CanCuocCongDan { get; set; }
        public string HoTenNguoiMuaHang { get; set; }
        public string SoDienThoaiNguoiMuaHang { get; set; }
        public string EmailNguoiMuaHang { get; set; }
        public string TenNganHang { get; set; }
        public string SoTaiKhoanNganHang { get; set; }
        public string HoTenNguoiNhanHD { get; set; }
        public string EmailNguoiNhanHD { get; set; }
        public string SoDienThoaiNguoiNhanHD { get; set; }
        public string HinhThucThanhToanId { get; set; }
        public string NhanVienBanHangId { get; set; }
        public DoiTuong NhanVienBanHang { get; set; }
        public string MaNhanVienBanHang { get; set; }
        public string TenNhanVienBanHang { get; set; }
        public string LoaiTienId { get; set; }
        public string MaLoaiTien { get; set; }
        public LoaiTien LoaiTien { get; set; }
        public decimal? TyGia { get; set; }
        public int? TrangThai { get; set; } // DLL.Enums.TrangThaiHoaDon
        public int? TrangThaiQuyTrinh { get; set; } // DLL.Enums.TrangThaiQuyTrinh
        public string MaTraCuu { get; set; }
        public string MCCQT { get; set; }
        public int? TrangThaiGuiHoaDon { get; set; } // DLL.Enums.TrangThaiGuiHoaDon
        public int? TrangThaiGuiHoaDonNhap { get; set; } // DLL.Enums.TrangThaiGuiHoaDon
        public int? TrangThaiGuiHoaDonMTT { get; set; }
        public bool? DaGuiThongBaoXoaBoHoaDon { get; set; }
        public bool? KhachHangDaNhan { get; set; }
        public int? SoLanChuyenDoi { get; set; }
        public DateTime? NgayXoaBo { get; set; }
        public string SoCTXoaBo { get; set; }
        public int TrangThaiBienBanXoaBo { get; set; }
        public string LyDoXoaBo { get; set; }
        public int LoaiHoaDon { get; set; } // DLL.Enums.LoaiHoaDon
        public int LoaiChungTu { get; set; }
        public DateTime? ThoiHanThanhToan { get; set; }
        public string DiaChiGiaoHang { get; set; }
        public List<HoaDonDienTuChiTiet> HoaDonChiTiets { get; set; }
        public string ThamChieu { get; set; }
        public string TaiLieuDinhKem { get; set; }
        public string FileChuaKy { get; set; }
        public string FileDaKy { get; set; }
        public string XMLChuaKy { get; set; }
        public string XMLDaKy { get; set; }
        public string MaCuaCQT { get; set; }
        public DateTime? NgayKy { get; set; }
        /// Thay thế, điều chỉnh
        public string ThayTheChoHoaDonId { get; set; }
        public string LyDoThayThe { get; set; }
        public string DieuChinhChoHoaDonId { get; set; }
        public int? LoaiApDungHoaDonDieuChinh { get; set; }
        public int? LoaiDieuChinh { get; set; } // DLL\Enums\LoaiDieuChinhHoaDon.cs
        public string LyDoDieuChinh { get; set; }
        //////////////////
        public string NguoiLapId { get; set; }
        public virtual DoiTuong NguoiLap { get; set; }
        public DateTime? NgayLap { get; set; }
        public bool? IsLapVanBanThoaThuan { get; set; }
        public LoaiChietKhau LoaiChietKhau { get; set; }
        public decimal? TyLeChietKhau { get; set; }
        ///////////////////////////////////
        // Phiếu xuất kho
        public string CanCuSo { get; set; }
        public DateTime? NgayCanCu { get; set; }
        public string Cua { get; set; }
        public string DienGiai { get; set; }
        public string DiaChiKhoNhanHang { get; set; }
        public string HoTenNguoiNhanHang { get; set; }
        public string DiaChiKhoXuatHang { get; set; }
        public string HoTenNguoiXuatHang { get; set; }
        public string HopDongVanChuyenSo { get; set; }
        public string TenNguoiVanChuyen { get; set; }
        public string PhuongThucVanChuyen { get; set; }
        ///////////////////////////////////
        public decimal? TongTienHang { get; set; }
        public decimal? TongTienChietKhau { get; set; }
        public decimal? TongTienThueGTGT { get; set; }
        public decimal? TongTienThanhToan { get; set; }
        public decimal? TongTienHangQuyDoi { get; set; }
        public decimal? TongTienChietKhauQuyDoi { get; set; }
        public decimal? TongTienThueGTGTQuyDoi { get; set; }
        public decimal? TongTienThanhToanQuyDoi { get; set; }

        public decimal? TongTienGiam { get; set; }
        public decimal? TongTienGiamQuyDoi { get; set; }

        //trường bổ sung
        public string TruongThongTinBoSung1 { get; set; }
        public string TruongThongTinBoSung2 { get; set; }
        public string TruongThongTinBoSung3 { get; set; }
        public string TruongThongTinBoSung4 { get; set; }
        public string TruongThongTinBoSung5 { get; set; }
        public string TruongThongTinBoSung6 { get; set; }
        public string TruongThongTinBoSung7 { get; set; }
        public string TruongThongTinBoSung8 { get; set; }
        public string TruongThongTinBoSung9 { get; set; }
        public string TruongThongTinBoSung10 { get; set; }

        public bool? IsGiamTheoNghiQuyet { get; set; }
        public decimal? TyLePhanTramDoanhThu { get; set; }
        public bool? IsNotCreateThayThe { get; set; }
        public int? HinhThucXoabo { get; set; } // DLL\Enums\HinhThucXoabo.cs

        public int? BackUpTrangThai { get; set; } // lưu lại trạng thái hóa đơn trước khi xóa bỏ

        public bool? IsDaLapThongBao04 { get; set; }
        public int? TrangThaiGui04 { get; set; }
        public int? LanGui04 { get; set; }
        public int? PhanLoaiSaiSot04 { get; set; }
        public string ChungTuLienQuan { get; set; }
        public string ThongDiepGuiCQTId { get; set; }
        public bool? IsBuyerSigned { get; set; }
        public DateTime? NgayGuiTBaoSaiSotKhongPhaiLapHD { get; set; }

        public List<NhatKyThaoTacHoaDon> NhatKyThaoTacHoaDons { get; set; }

        public string IdHoaDonSaiSotBiThayThe { get; set; }
        public string GhiChuThayTheSaiSot { get; set; }
        public string EmailTBaoSaiSotKhongPhaiLapHDId { get; set; }
        public bool? IsThongTinNguoiBanHoacNguoiMua { get; set; }
        public bool? IsTheHienLyDoTrenHoaDon { get; set; }
        public bool? IsKemGuiEmail { get; set; }

        [MaxLength(500)]
        public string PosCustomerURL { get; set; }
        public string EmailNhanKemTheo { get; set; }
        public string TenNguoiNhanKemTheo { get; set; }

        public int? IsPos { get; set; } // hóa đơn lập từ Pos

        public long? BienLaiId { get; set; }

        public int? PhatHanhNgayPos { get; set; }

        public DateTime? TimeUpdateByNSD { get; set; }
        public bool? OptionEmailProcessHdMtt { get; set; }
        public string RefIdHoaDonXoaBoLanDau { get; set; }
        public bool IsTBaoHuyKhongDuocChapNhan { get; set; } = false;

        public bool? IsXacNhanDaGuiHDChoKhachHang { get; set; } = false;

        public bool IsUpdated04 { get; set; } = false;
    }
}
