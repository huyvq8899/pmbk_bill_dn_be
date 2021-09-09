using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1
{
    public class DSCTSSDung
    {
        public List<CTS> CTS { get; set; }
    }

    public class CTS
    {
        [Required]
        [MaxLength(400)]
        public string TTChuc { get; set; }

        [Required]
        [MaxLength(40)]
        public string Seri { get; set; }

        [Required]
        public string TNgay { get; set; }

        [Required]
        public string DNgay { get; set; }

        [Required]
        public HThuc2 HThuc { get; set; }
    }
}
