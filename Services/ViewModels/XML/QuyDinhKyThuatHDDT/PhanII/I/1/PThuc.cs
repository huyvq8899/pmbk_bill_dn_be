using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1
{
    public class PThuc
    {
        [Required]
        public ADung CDDu { get; set; }

        [Required]
        public ADung CBTHop { get; set; }
    }
}
