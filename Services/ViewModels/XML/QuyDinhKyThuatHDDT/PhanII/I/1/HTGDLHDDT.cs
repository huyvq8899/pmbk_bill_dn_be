using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1
{
    public class HTGDLHDDT
    {
        [Required]
        public ADung NNTDBKKhan { get; set; }

        [Required]
        public ADung NNTKTDNUBND { get; set; }

        [Required]
        public ADung CDLTTDCQT { get; set; }

        [Required]
        public ADung CDLQTVAN { get; set; }
    }
}
