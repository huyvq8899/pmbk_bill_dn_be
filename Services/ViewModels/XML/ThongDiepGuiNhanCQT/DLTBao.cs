using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels.XML.ThongDiepGuiNhanCQT
{
    public partial class DLTBao
    {
        public string PBan { set; get; }
        public string MSo { set; get; }
        public string Ten { set; get; }
        public string Loai { set; get; }
        public string So { set; get; }
        public string NTBCCQT { set; get; }
        public string MCQT { set; get; }
        public string TCQT { set; get; }
        public string TNNT { set; get; }

        public string MST { set; get; }
        public string MDVQHNSach { set; get; }
        public string DDanh { set; get; }
        public string NTBao { set; get; }

        public DSHDon DSHDon { set; get; }
    }

    public partial class TBao
    {
        public DLTBao DLTBao { set; get; }

        public DSCKS DSCKS { set; get; }
    }

    public partial class DLieu
    {
        public List<TBao> TBao { set; get; }
    }
}
