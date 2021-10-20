using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels.XML.ThongDiepGuiNhanCQT
{
    public partial class HDon
    {
        public int STT { set; get; }

        public string MCQTCap { set; get; }

        public string KHMSHDon { set; get; }

        public string KHHDon { set; get; }

        public string SHDon { set; get; }

        public string Ngay { set; get; }

        public string LADHĐĐT { set; get; }

        public string TCTBao { set; get; }

        public string LDo { set; get; }
    }

    public partial class DSHDon
    {
        public List<HDon> HDon { set; get; }
    }
}
