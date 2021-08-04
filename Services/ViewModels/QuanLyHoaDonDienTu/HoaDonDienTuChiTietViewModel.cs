using Services.Helper.LogHelper;
using Services.ViewModels.DanhMuc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.QuanLyHoaDonDienTu
{
    public class HoaDonDienTuChiTietViewModel : ThongTinChungViewModel
    {
        [IgnoreLogging]
        [LoggingPrimaryKey]
        public string HoaDonDienTuChiTietId { get; set; }

        [IgnoreLogging]
        public string HoaDonDienTuId { get; set; }

        [IgnoreLogging]
        public HoaDonDienTuViewModel HoaDon { get; set; }

        [IgnoreLogging]
        public string HangHoaDichVuId { get; set; }

        [IgnoreLogging]
        public HangHoaDichVuViewModel HangHoaDichVu { get; set; }

        [IgnoreLogging]
        public string MaHang { get; set; }

        [DetailKey]
        [Display(Name = "Tên hàng hóa, dịch vụ")]
        public string TenHang { get; set; }

        [CheckBox]
        [Display(Name = "Hàng khuyến mại")]
        public bool? HangKhuyenMai { get; set; }

        [IgnoreLogging]
        public bool? DongChietKhau { get; set; }

        [IgnoreLogging]
        public bool? DongMoTa { get; set; }

        [IgnoreLogging]
        public string DonViTinhId { get; set; }

        [Display(Name = "Đơn vị tính")]
        public string TenDonViTinh { get; set; }

        [IgnoreLogging]
        public DonViTinhViewModel DonViTinh { get; set; }

        [Currency]
        [Display(Name = "Số lượng")]
        public decimal? SoLuong { get; set; }

        [Currency]
        [Display(Name = "Đơn giá")]
        public decimal? DonGia { get; set; }

        [Currency]
        [Display(Name = "Đơn giá sau thuế")]
        public decimal? DonGiaSauThue { get; set; }

        [Currency]
        [Display(Name = "Đơn giá quy đổi")]
        public decimal? DonGiaQuyDoi { get; set; }

        [Currency]
        [Display(Name = "Thành tiền")]
        public decimal? ThanhTien { get; set; }

        [Currency]
        [Display(Name = "Thành tiền sau thuế")]
        public decimal? ThanhTienSauThue { get; set; }

        [Currency]
        [Display(Name = "Thành tiền")]
        public decimal? ThanhTienQuyDoi { get; set; }

        [Currency]
        [Display(Name = "Thành tiền sau thuế")]
        public decimal? ThanhTienSauThueQuyDoi { get; set; }

        [Display(Name = "Thuế GTGT")]
        public string ThueGTGT { get; set; }

        [Display(Name = "Tỷ lệ chiết khấu")]
        public decimal? TyLeChietKhau { get; set; }

        [Currency]
        [Display(Name = "Tiền chiết khấu")]
        public decimal? TienChietKhau { get; set; }

        [Currency]
        [Display(Name = "Tiền chiết khấu")]
        public decimal? TienChietKhauQuyDoi { get; set; }

        [Currency]
        [Display(Name = "Tiền thuế GTGT")]
        public decimal? TienThueGTGT { get; set; }

        [Currency]
        [Display(Name = "Tiền thuế GTGT")]
        public decimal? TienThueGTGTQuyDoi { get; set; }

        [Display(Name = "Số lô")]
        public string SoLo { get; set; }

        [Display(Name = "Hạn sử dụng")]
        public DateTime? HanSuDung { get; set; }

        [Display(Name = "Số khung")]
        public string SoKhung { get; set; }

        [Display(Name = "Số máy")]
        public string SoMay { get; set; }

        [Display(Name = "Ghi chú")]
        public string GhiChu { get; set; }

        [IgnoreLogging]
        public decimal? TongTienThanhToan { get; set; }

        [IgnoreLogging]
        public decimal? TongTienThanhToanQuyDoi { get; set; }

        [IgnoreLogging]
        public string LoaiTienId { get; set; }

        [IgnoreLogging]
        public bool? IsVND { get; set; }
    }
}
