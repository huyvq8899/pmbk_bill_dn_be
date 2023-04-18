using DLL.Enums;
using Services.Helper.LogHelper;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace Services.ViewModels.DanhMuc
{
    public class ThongBaoKetQuaHuyHoaDonChiTietViewModel : ThongTinChungViewModel
    {
        [IgnoreLogging]
        [LoggingPrimaryKey]
        public string ThongBaoKetQuaHuyHoaDonChiTietId { get; set; }

        [IgnoreLogging]
        public string ThongBaoKetQuaHuyHoaDonId { get; set; }

        [Display(Name = "Loại hóa đơn")]
        public LoaiHoaDon LoaiHoaDon { get; set; }

        [IgnoreLogging]
        public string MauHoaDonId { get; set; }

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
        public string TenLoaiHoaDon { get; set; }

        [IgnoreLogging]
        public long? SoLuongOther { get; set; }

        [IgnoreLogging]
        public bool? BlockDelete { get; set; }
    }
}
