using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.ThongDiepGuiNhanCQT
{
    public partial class HDon
    {
        [MaxLength(4)]
        public int STT { set; get; }

        [MaxLength(34)]
        public string MCQTCap { set; get; }

        [MaxLength(11)]
        public string KHMSHDon { set; get; }

        [MaxLength(8)]
        public string KHHDon { set; get; }

        [MaxLength(8)]
        public string SHDon { set; get; }

        public string Ngay { set; get; }

        [MaxLength(1)]
        public byte LADHĐĐT { set; get; }

        [MaxLength(1)]
        public byte TCTBao { set; get; }

        [MaxLength(255)]
        public string LDo { set; get; }
    }

    public partial class DSHDon
    {
        public List<HDon> HDon { set; get; }
    }
}
