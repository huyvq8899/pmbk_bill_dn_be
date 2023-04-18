using DLL.Enums;
using Services.Helper.LogHelper;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.DanhMuc
{
    public class ThongBaoDieuChinhThongTinHoaDonChiTietViewModel : ThongTinChungViewModel
    {
        [IgnoreLogging]
        [LoggingPrimaryKey]
        public string ThongBaoDieuChinhThongTinHoaDonChiTietId { get; set; }

        [IgnoreLogging]
        public string ThongBaoDieuChinhThongTinHoaDonId { get; set; }

        [IgnoreLogging]
        public string MauHoaDonId { get; set; }

        [IgnoreLogging]
        public LoaiHoaDon LoaiHoaDon { get; set; }

        [IgnoreLogging]
        public string TenLoaiHoaDon { get; set; }

        [DetailKey]
        [Display(Name = "Mẫu hóa đơn")]
        public string MauSo { get; set; }

        [Display(Name = "Ký hiệu")]
        public string KyHieu { get; set; }

        [OrderNumber]
        [Display(Name = "Từ số")]
        public int? TuSo { get; set; }

        [OrderNumber]
        [Display(Name = "Đến số")]
        public int? DenSo { get; set; }

        [Display(Name = "Số lượng")]
        public int? SoLuong { get; set; }

        [IgnoreLogging]
        public bool? Checked { get; set; }
    }
}
