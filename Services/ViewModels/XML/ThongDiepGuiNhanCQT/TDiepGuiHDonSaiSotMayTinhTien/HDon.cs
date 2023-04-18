using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Services.ViewModels.XML.ThongDiepGuiNhanCQT.TDiepGuiHDonSaiSotMayTinhTien
{
    public partial class HDon
    {
        public int STT { set; get; }

        [MaxLength(34)]
        public string MCQTCap { set; get; }

        [MaxLength(34)]
        public string MCCQT { set; get; }

        [MaxLength(11)]
        public string KHMSHDon { set; get; }

        [MaxLength(8)]
        public string KHHDon { set; get; }

        [MaxLength(8)]
        public string SHDon { set; get; }

        public string Ngay { set; get; }

        /*
        public LADHDDT LADHDDT { set; get; }

        public TCTBao TCTBao { set; get; }
        */
        public byte LADHDDT { set; get; }

        public byte TCTBao { set; get; }

        [MaxLength(255)]
        public string LDo { set; get; }
    }

    public partial class DSHDon
    {
        public List<HDon> HDon { set; get; }
    }
}
