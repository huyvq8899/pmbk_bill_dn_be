using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1
{
    public partial class HTHDon
    {
        [Required]
        public ADung CMa { get; set; }

        [Required]
        public ADung KCMa { get; set; }
    }
}
