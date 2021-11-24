using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.ThongDiepGuiNhanCQT.TDiepNhanHDonRaSoat
{
    public partial class HDon
    {
        public int STT { set; get; }

        [MaxLength(11)]
        public string KHMSHDon { set; get; }

        [MaxLength(8)]
        public string KHHDon { set; get; }

        [MaxLength(8)]
        public string SHDon { set; get; }

        public string NLap { set; get; }

        public byte LADHDDT { set; get; }

        [MaxLength(255)]
        public string LDo { set; get; }
    }
}
