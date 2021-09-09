using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._2
{
    public partial class DSCTSSDung
    {
        public List<CTS> CTS { get; set; }
    }

    public partial class CTS
    {
        [MaxLength(3)]
        public int? STT { get; set; }

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
