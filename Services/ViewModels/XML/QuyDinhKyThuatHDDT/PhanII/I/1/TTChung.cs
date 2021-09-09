using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1
{
    public partial class TTChung
    {
        [Required]
        [MaxLength(6)]
        public string PBan { get; set; }

        [Required]
        [MaxLength(15)]
        public string MSo { get; set; }

        [Required]
        [MaxLength(100)]
        public string Ten { get; set; }

        [Required]
        public HThuc HThuc { get; set; }

        [Required]
        [MaxLength(400)]
        public string TNNT { get; set; }

        [Required]
        [MaxLength(14)]
        public string MST { get; set; }

        [Required]
        [MaxLength(100)]
        public string CQTQLy { get; set; }

        [Required]
        [MaxLength(5)]
        public string MCQTQLy { get; set; }

        [Required]
        [MaxLength(50)]
        public string NLHe { get; set; }

        [Required]
        [MaxLength(400)]
        public string DCLHe { get; set; }

        [Required]
        [MaxLength(50)]
        public string DCTDTu { get; set; }

        [Required]
        [MaxLength(20)]
        public string DTLHe { get; set; }

        [Required]
        [MaxLength(50)]
        public string DDanh { get; set; }

        [Required]
        public string NLap { get; set; }
    }
}
