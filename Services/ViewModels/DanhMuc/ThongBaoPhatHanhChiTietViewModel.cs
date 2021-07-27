using Services.Helper.LogHelper;
using System;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.DanhMuc
{
    public class ThongBaoPhatHanhChiTietViewModel : ThongTinChungViewModel
    {
        [IgnoreLogging]
        [LoggingPrimaryKey]
        public string ThongBaoPhatHanhChiTietId { get; set; }

        [IgnoreLogging]
        public string ThongBaoPhatHanhId { get; set; }

        [IgnoreLogging]
        public string MauHoaDonId { get; set; }

        [Display(Name = "Loại hóa đơn")]
        public string TenLoaiHoaDon { get; set; }

        [DetailKey]
        [Display(Name = "Mẫu hóa đơn")]
        public string MauSoHoaDon { get; set; }

        [Display(Name = "Ký hiệu")]
        public string KyHieu { get; set; }

        [Display(Name = "Số lượng")]
        public int? SoLuong { get; set; }

        [OrderNumber]
        [Display(Name = "Từ số")]
        public int? TuSo { get; set; }

        [OrderNumber]
        [Display(Name = "Đến số")]
        public int? DenSo { get; set; }

        [Display(Name = "Ngày bắt đầu sử dụng")]
        public DateTime? NgayBatDauSuDung { get; set; }

        [IgnoreLogging]
        public bool? Checked { get; set; }

        [IgnoreLogging]
        public DateTime? NgayTao { get; set; }

        [IgnoreLogging]
        public string TenTrangThai { get; set; }

        [IgnoreLogging]
        public string So { get; set; }

        [IgnoreLogging]
        public ThongBaoPhatHanhViewModel ThongBaoPhatHanh { get; set; }

        [IgnoreLogging]
        public MauHoaDonViewModel MauHoaDon { get; set; }
    }
}
