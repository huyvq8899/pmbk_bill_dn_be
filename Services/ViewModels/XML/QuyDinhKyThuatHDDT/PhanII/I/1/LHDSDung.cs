using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1
{
    public class LHDSDung
    {
        [Required]
        public SDung HDGTGT { get; set; }

        [Required]
        public SDung HDBHang { get; set; }

        [Required]
        public SDung HDBTSCong { get; set; }

        [Required]
        public SDung HDBHDTQGia { get; set; }

        [Required]
        public SDung HDKhac { get; set; }

        [Required]
        public SDung CTu { get; set; }
    }
}
