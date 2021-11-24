using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.ThongDiepGuiNhanCQT.TDiepNhanHDonRaSoat
{
    public partial class DLTBao
    {
        /// <summary>
        /// Phiên bản XML. Trong khuyến nghị này có giá trị là 2.0.0
        /// </summary>
        [MaxLength(6)]
        public string PBan { set; get; }

        [MaxLength(15)]
        public string MSo { set; get; }

        [MaxLength(255)]
        public string Ten { set; get; }

        [MaxLength(50)]
        public string DDanh { set; get; }

        [MaxLength(100)]
        public string TCQTCTren { set; get; }

        [MaxLength(100)]
        public string TCQT { set; get; }

        [MaxLength(400)]
        public string TNNT { set; get; }

        [MaxLength(14)]
        public string MST { set; get; }

        [MaxLength(7)]
        public string MDVQHNSach { set; get; }

        [MaxLength(400)]
        public string DCNNT { set; get; }

        [MaxLength(50)]
        public string DCTDTu { set; get; }

        public byte THan { set; get; }

        public byte Lan { set; get; }

        [MaxLength(50)]
        public string HThuc { set; get; }

        [MaxLength(50)]
        public string CDanh { set; get; }

        public List<HDon> DSHDon { set; get; }
    }

    public partial class STBao
    {
        [MaxLength(30)]
        public string So { set; get; }

        public string NTBao { set; get; }
    }

    public partial class TBao
    {
        public DLTBao DLTBao { set; get; }

        public STBao STBao { set; get; }

        public DSCKS DSCKS { set; get; }
    }

    public partial class DLieu
    {
        public TBao TBao { set; get; }
    }
}
