using DLL.Entity.QuanLyHoaDon;
using System;
using System.Collections.Generic;

namespace Services.ViewModels.QuanLyHoaDonDienTu
{
    /// <summary>
    /// Class này thể hiện thông tin chung của hóa đơn điện tử sẽ được kết chuyển vào phần mềm Kế Toán Bách Khoa.
    /// </summary>
    public class HoaDonChoKeToanBachKhoaViewModel : ThongTinChungViewModel
    {
        public string HoaDonDienTuId { get; set; }

        public DateTime? NgayHoaDon { get; set; }

        public long? SoHoaDon { get; set; }

        public string MauSo { get; set; }

        public string KyHieu { get; set; }

        public string MaKhachHang { get; set; }

        public string TenKhachHang { get; set; }

        public string HoTenNguoiMuaHang { get; set; }

        public string MaSoThue { get; set; }

        public string DiaChi { get; set; }

        public decimal? TongTienThanhToan { get; set; } // Tổng tiền thanh toán

        public decimal? TongTienThanhToanQuyDoi { get; set; } // Tổng tiền thanh toán quy đổi

        public string MaLoaiTien { get; set; } // Mã loại tiền

        public decimal? TyGia { get; set; } // Tỷ giá

        public int? TrangThai { get; set; } // Giá trị số của trạng thái hóa đơn

        public string TenTrangThaiHoaDon { get; set; } // Một chuỗi mô tả trạng thái hóa đơn

        public string HinhThucThanhToan { get; set; }

        public bool? IsHoaDonBiDieuChinh { get; set; }

        public bool? IsHoaDonBiThayThe { get; set; }

        public bool? IsHoaDonHuy { get; set; }

        public bool? IsHoaDonBiXoaBo { get; set; }

        // Nếu là hóa đơn thay thế thì sẽ có giá trị HoaDonBiThayThe, 
        // vì khi đó trường ThayTheChoHoaDonId khác rỗng
        public HoaDonChoKeToanBachKhoaViewModel HoaDonBiThayThe { get; set; }

        public List<ChiTietHoaDonChoKeToanBachKhoaViewModel> ChiTietHangHoaDichVu { get; set; } // Chi tiết hàng hóa dịch vụ
    }

    /// <summary>
    /// Class này thể hiện thông tin chi tiết của hóa đơn điện tử sẽ được kết chuyển vào phần mềm Kế Toán Bách Khoa.
    /// </summary>
    public class ChiTietHoaDonChoKeToanBachKhoaViewModel : ThongTinChungViewModel
    {
        public string HoaDonDienTuChiTietId { get; set; }

        public string HoaDonDienTuId { get; set; }

        public string HangHoaDichVuId { get; set; }

        public string MaHang { get; set; }

        public string TenHang { get; set; }

        public int? TinhChat { get; set; }

        public string TenTinhChat { get; set; }

        public string DonViTinhId { get; set; }

        public string TenDonViTinh { get; set; }

        public decimal? SoLuong { get; set; }

        public decimal? DonGia { get; set; }

        public decimal? ThanhTien { get; set; }

        public decimal? ThanhTienQuyDoi { get; set; }

        public string ThueGTGT { get; set; }

        public decimal? TienThueGTGT { get; set; }

        public decimal? TienThueGTGTQuyDoi { get; set; }

        public decimal? TyLeChietKhau { get; set; }

        public decimal? TienChietKhau { get; set; }

        public decimal? TienChietKhauQuyDoi { get; set; }
    }

    /// <summary>
    /// Class này thể hiện các tham số điều kiện khi lấy dữ liệu hóa đơn để kết chuyển vào phần mềm Kế Toán Bách Khoa.
    /// </summary>
    public class ThamSoLayDuLieuHoaDon
    {
        public string TuNgay { get; set; } // Từ ngày
        public string DenNgay { get; set; } // Đến ngày
        public bool? IsCoPhatSinhNgoaiTe { get; set; } // Nếu có phát sinh ngoại tệ
        public string HoaDonDienTuId { get; set; } // Nếu truyền vào ID của một hóa đơn cụ thể
    }

    /// <summary>
    /// Class này để truyền tham số vào vào phương thức NhanBietHoaDonTheoPhanLoai trong class HoaDonDienTuService.
    /// </summary>
    public class ThamSoNhanBietHoaDon
    {
        public HoaDonDienTu HoaDonDienTu { get; set; }
        public List<HoaDonDienTu> ListAllHoaDons { get; set; }
        public List<DLL.Entity.QuanLy.BoKyHieuHoaDon> ListAllBoKyHieuHoaDons { get; set; }
    }
}
