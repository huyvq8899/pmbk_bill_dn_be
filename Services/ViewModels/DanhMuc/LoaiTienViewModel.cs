using Services.Helper.LogHelper;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.DanhMuc
{
    public class LoaiTienViewModel : ThongTinChungViewModel
    {
        [IgnoreLogging]
        public string LoaiTienId { get; set; }

        [Display(Name = "Mã")]
        public string Ma { get; set; }

        [Display(Name = "Tên")]
        public string Ten { get; set; }

        [Currency]
        [Display(Name = "Tỷ giá quy đổi")]
        public decimal? TyGiaQuyDoi { get; set; }

        [Display(Name = "Sắp xếp")]
        public int? SapXep { get; set; }
    }
}
