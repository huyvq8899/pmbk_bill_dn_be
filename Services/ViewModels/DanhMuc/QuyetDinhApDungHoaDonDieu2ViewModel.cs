using Services.Helper.LogHelper;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.DanhMuc
{
    public class QuyetDinhApDungHoaDonDieu2ViewModel : ThongTinChungViewModel
    {
        [IgnoreLogging]
        [LoggingPrimaryKey]
        public string QuyetDinhApDungHoaDonDieu2Id { get; set; }

        [IgnoreLogging]
        public string QuyetDinhApDungHoaDonId { get; set; }

        [IgnoreLogging]
        public string MauHoaDonId { get; set; }

        [Display(Name = "Tên hóa đơn")]
        public string TenLoaiHoaDon { get; set; }

        [DetailKey]
        [Display(Name = "Mẫu hóa đơn")]
        public string MauSo { get; set; }

        [Display(Name = "Ký hiệu")]
        public string KyHieu { get; set; }

        [Display(Name = "Mục đích sử dụng")]
        public string MucDichSuDung { get; set; }

        [IgnoreLogging]
        public bool? Checked { get; set; }
    }
}
