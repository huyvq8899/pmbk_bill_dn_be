using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._3
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
        [MaxLength(14)]
        public string MST { get; set; }

        [Required]
        [MaxLength(400)]
        public string TNNT { get; set; }

        [Required]
        [MaxLength(100)]
        public string TTKhai { get; set; }

        [Required]
        [MaxLength(46)]
        public string MGDDTu { get; set; }

        [Required]
        public string TGGui { get; set; }

        [Required]
        public THop THop { get; set; }

        public string TGNhan { get; set; }
    }
}
