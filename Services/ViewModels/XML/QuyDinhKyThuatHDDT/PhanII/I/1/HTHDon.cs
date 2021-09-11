using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Attributes;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1
{
    public partial class HTHDon
    {
        [Required]
        [MaxLength(1)]
        [Display(Name = "Có mã (Hình thức hóa đơn có mã của CQT)")]
        [CustomDataType(CustomDataType.Number)]
        public ADung CMa { get; set; }

        [Required]
        [MaxLength(1)]
        [Display(Name = "Không có mã (Hình thức hóa đơn không có mã của CQT)")]
        [CustomDataType(CustomDataType.Number)]
        public ADung KCMa { get; set; }
    }
}
