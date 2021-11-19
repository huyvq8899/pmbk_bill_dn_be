using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.ThongDiepGuiNhanCQT
{
    public partial class TTChung
    {
        /// <summary>
        /// Phiên bản XML. Trong khuyến nghị này có giá trị là 2.0.0
        /// </summary>
        [MaxLength(6)]
        public string PBan { set; get; } = "2.0.0";

        [MaxLength(14)]
        public string MNGui { set; get; }

        [MaxLength(14)]
        public string MNNhan { set; get; }

        public int MLTDiep { set; get; }

        [MaxLength(46)]
        public string MTDiep { set; get; }

        [MaxLength(46)]
        public string MTDTChieu { set; get; }

        [MaxLength(14)]
        public string MST { set; get; }

        public int SLuong { set; get; }
    }
}
