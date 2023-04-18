using Services.Helper.LogHelper;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.DanhMuc
{
    public class DonViTinhViewModel : ThongTinChungViewModel
    {
        [IgnoreLogging]
        public string DonViTinhId { get; set; }

        [Display(Name = "Tên")]
        public string Ten { get; set; }

        [Display(Name = "Mô tả")]
        public string MoTa { get; set; }
    }
}
