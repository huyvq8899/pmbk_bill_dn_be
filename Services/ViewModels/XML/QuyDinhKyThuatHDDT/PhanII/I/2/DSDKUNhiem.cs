using DLL.Entity.DanhMuc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._2
{
    public partial class DSDKUNhiem
    {
        public List<DKUNhiem> DKUNhiem { get; set; }
    }

    public partial class DKUNhiem
    {
        [MaxLength(3)]
        public int? STT { get; set; }

        [Required]
        [MaxLength(100)]
        public string TLHDon { get; set; }

        [MaxLength(1)]
        public int? KHMSHDon { get; set; }

        [MaxLength(6)]
        public string KHHDon { get; set; }

        [Required]
        [MaxLength(14)]
        public string MST { get; set; }

        [Required]
        [MaxLength(400)]
        public string TTChuc { get; set; }

        [Required]
        [MaxLength(255)]
        public string MDich { get; set; }

        [Required]
        public string TNgay { get; set; }

        [Required]
        public string DNgay { get; set; }

        [Required]
        public HinhThucThanhToan PThuc { get; set; }

        [MaxLength(50)]
        public string THTTTKhac { get; set; }
    }
}
