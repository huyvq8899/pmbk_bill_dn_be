using DLL.Entity.DanhMuc;
using DLL.Entity.QuanLy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DLL.Entity.QuanLyHoaDon
{
    public class HoaDonDienTu : ThongTinChung
    {
        public string HoaDonDienTuId { get; set; }
        public DateTime? NgayHoaDon { get; set; }
        public string SoHoaDon { get; set; }
        public string MauHoaDonId { get; set; }
        public MauHoaDon MauHoaDon { get; set; }
        public string BoKyHieuHoaDonId { get; set; }
        public BoKyHieuHoaDon BoKyHieuHoaDon { get; set; }
        public string MauSo { get; set; }
        public string KyHieu { get; set; }
        public string KhachHangId { get; set; }
        public DoiTuong KhachHang { get; set; }
        public string MaKhachHang { get; set; }
        public string TenKhachHang { get; set; }
        public string DiaChi { get; set; }
        public string MaSoThue { get; set; }
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
        public LoaiTien LoaiTien { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal? TyGia { get; set; }
        public int? TrangThai { get; set; } // DLL.Enums.TrangThaiHoaDon
        public int? TrangThaiQuyTrinh { get; set; } // DLL.Enums.TrangThaiQuyTrinh
        public string MaTraCuu { get; set; }
        public int? TrangThaiGuiHoaDon { get; set; } // DLL.Enums.TrangThaiGuiHoaDon
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
        ///////////////////////////////////
        [Column(TypeName = "decimal(18,4)")]
        public decimal? TongTienHang { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? TongTienChietKhau { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? TongTienThueGTGT { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? TongTienThanhToan { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? TongTienHangQuyDoi { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? TongTienChietKhauQuyDoi { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? TongTienThueGTGTQuyDoi { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? TongTienThanhToanQuyDoi { get; set; }

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

        public bool? IsNotCreateBienBan { get; set; }

        public List<NhatKyThaoTacHoaDon> NhatKyThaoTacHoaDons { get; set; }
    }
}
