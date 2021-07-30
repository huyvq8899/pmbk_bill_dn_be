using Services.ViewModels.DanhMuc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels.QuanLyHoaDonDienTu
{
    public class HoaDonDienTuChiTietViewModel : ThongTinChungViewModel
    {
        public string HoaDonDienTuChiTietId { get; set; }
        public string HoaDonDienTuId { get; set; }
        public HoaDonDienTuViewModel HoaDon { get; set; }
        public string HangHoaDichVuId { get; set; }
        public HangHoaDichVuViewModel HangHoaDichVu { get; set; }
        public string MaHang { get; set; }
        public string TenHang { get; set; }
        public bool? HangKhuyenMai { get; set; }
        public bool? DongChietKhau { get; set; }
        public bool? DongMoTa { get; set; }
        public string DonViTinhId { get; set; }
        public DonViTinhViewModel DonViTinh { get; set; }
        public decimal? SoLuong { get; set; }
        public decimal? DonGia { get; set; }
        public decimal? DonGiaSauThue { get; set; }
        public decimal? DonGiaQuyDoi { get; set; }
        public decimal? ThanhTien { get; set; }
        public decimal? ThanhTienSauThue { get; set; }
        public decimal? ThanhTienQuyDoi { get; set; }
        public decimal? ThanhTienSauThueQuyDoi { get; set; }
        public string ThueGTGT { get; set; }
        public decimal? TyLeChietKhau { get; set; }
        public decimal? TienChietKhau { get; set; }
        public decimal? TienChietKhauQuyDoi { get; set; }
        public decimal? TienThueGTGT { get; set; }
        public decimal? TienThueGTGTQuyDoi { get; set; }
        public string SoLo { get; set; }
        public DateTime? HanSuDung { get; set; }
        public string SoKhung { get; set; }
        public string SoMay { get; set; }
        public string GhiChu { get; set; }

        public decimal? TongTienThanhToan { get; set; }
        public decimal? TongTienThanhToanQuyDoi { get; set; }
        public string LoaiTienId { get; set; }
        public bool? IsVND { get; set; }
    }
}
