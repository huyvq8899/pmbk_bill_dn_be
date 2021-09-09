using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._5
{
    public partial class DLTBao
    {
        [Required]
        [MaxLength(6)]
        public string PBan { get; set; }

        [Required]
        [MaxLength(15)]
        public string MSo { get; set; }

        [Required]
        [MaxLength(255)]
        public string Ten { get; set; }

        [Required]
        [MaxLength(30)]
        public string So { get; set; }

        [Required]
        [MaxLength(50)]
        public string DDanh { get; set; }

        [Required]
        public string NTBao { get; set; }

        [Required]
        [MaxLength(100)]
        public string TCQTCTren { get; set; }

        [Required]
        [MaxLength(100)]
        public string TCQT { get; set; }

        [Required]
        [MaxLength(14)]
        public string MST { get; set; }

        [Required]
        [MaxLength(400)]
        public string TNNT { get; set; }

        [Required]
        public string Ngay { get; set; }

        [Required]
        public LDKUNhiem LUNhiem { get; set; }

        [Required]
        [MaxLength(46)]
        public string MGDDTu { get; set; }

        [Required]
        public string TGNhan { get; set; }
    }
}
