using DLL.Enums;
using Services.Helper.LogHelper;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.DanhMuc
{
    public class HangHoaDichVuViewModel : ThongTinChungViewModel
    {
        [IgnoreLogging]
        public string HangHoaDichVuId { get; set; }

        [Display(Name = "Mã")]
        public string Ma { get; set; }

        [Display(Name = "Tên")]
        public string Ten { get; set; }

        [Display(Name = "Đơn vị tính")]
        public string TenDonViTinh { get; set; }

        [Currency]
        [Display(Name = "Giá bán")]
        public decimal? DonGiaBan { get; set; }
        public string DonGiaBanText { get; set; }

        [CheckBox]
        [Display(Name = "Giá bán là đơn giá sau thuế")]
        public bool? IsGiaBanLaDonGiaSauThue { get; set; }

        [Display(Name = "Thuế GTGT")]
        public ThueGTGT ThueGTGT { get; set; } // %
        public string ThueGTGTText { get; set; }

        [Percent]
        [Display(Name = "Tỷ lệ CK")]
        public decimal? TyLeChietKhau { get; set; } // %
        public string TyLeChietKhauText { get; set; }

        [Display(Name = "Mô tả")]
        public string MoTa { get; set; }

        // Khóa ngoại
        [IgnoreLogging]
        public string DonViTinhId { get; set; }

        [IgnoreLogging]
        public string TenThueGTGT { get; set; }

        [IgnoreLogging]
        public DonViTinhViewModel DonViTinh { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
        public int Row { get; set; }
    }
}
