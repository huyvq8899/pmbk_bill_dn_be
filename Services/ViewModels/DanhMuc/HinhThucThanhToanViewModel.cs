using Services.Helper.LogHelper;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.DanhMuc
{
    public class HinhThucThanhToanViewModel : ThongTinChungViewModel
    {
        [IgnoreLogging]
        public string HinhThucThanhToanId { get; set; }

        [Display(Name = "Tên")]
        public string Ten { get; set; }

        [Display(Name = "Mô tả")]
        public string MoTa { get; set; }
    }
}
