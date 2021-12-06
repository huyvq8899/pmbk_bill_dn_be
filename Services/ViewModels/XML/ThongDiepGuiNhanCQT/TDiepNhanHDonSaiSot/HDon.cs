using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.ThongDiepGuiNhanCQT.TDiepNhanHDonSaiSot
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

        public string NLap { set; get; }

        public byte LADHDDT { set; get; }

        public byte TCTBao { set; get; }

        public byte TTTNCCQT { set; get; }

        public List<LDo> DSLDKTNhan { set; get; }
    }
}
