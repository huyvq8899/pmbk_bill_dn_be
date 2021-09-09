using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._4
{
    public partial class DSLDKCNhan
    {
        public List<LDo> LDo { get; set; }
    }

    public partial class LDo
    {
        [Required]
        [MaxLength(4)]
        public string MLoi { get; set; }

        [Required]
        [MaxLength(100)]
        public string MTa { get; set; }
    }
}
