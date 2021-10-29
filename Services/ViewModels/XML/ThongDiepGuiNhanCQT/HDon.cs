using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.ThongDiepGuiNhanCQT
{
    public partial class HDon
    {
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

        public byte LADHĐĐT { set; get; }

        public byte TCTBao { set; get; }

        [MaxLength(255)]
        public string LDo { set; get; }
    }

    public partial class DSHDon
    {
        public List<HDon> HDon { set; get; }
    }
}
