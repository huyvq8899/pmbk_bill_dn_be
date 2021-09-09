using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._5
{
    public partial class DSTTUNhiem
    {
        public List<TTUNhiem> TTUNhiem { get; set; }
    }

    public partial class TTUNhiem
    {
        [Required]
        [MaxLength(14)]
        public string MST { get; set; }

        [Required]
        [MaxLength(400)]
        public string TTChuc { get; set; }

        [Required]
        public string NTNhan { get; set; }

        [Required]
        public KQua KQua { get; set; }

        public DSLDKCNhan DSLDKCNhan { get; set; }

        public DSHDUNhiem DSHDUNhiem { get; set; }
    }

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

        [MaxLength(255)]
        public string HDXLy { get; set; }

        [MaxLength(255)]
        public string GChu { get; set; }
    }

    public partial class DSHDUNhiem
    {
        public List<HDUNhiem> HDUNhiem { get; set; }
    }

    public partial class HDUNhiem
    {
        [Required]
        [MaxLength(100)]
        public string TLHDon { get; set; }

        [MaxLength(1)]
        public int? KHMSHDon { get; set; }

        [MaxLength(6)]
        public string KHHDon { get; set; }

        [Required]
        [MaxLength(255)]
        public string MDich { get; set; }

        [Required]
        public string TNgay { get; set; }

        [Required]
        public string DNgay { get; set; }
    }
}
