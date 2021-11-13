using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.ThongDiepGuiNhanCQT.TDiepNhan999
{
    public partial class TBao
    {
        [MaxLength(46)]
        public string MTDiep { set; get; }

        [MaxLength(14)]
        public string MNGui { set; get; }

        [MaxLength(14)]
        public string MNNhan { set; get; }

        public byte TTTNhan { set; get; }

        public List<LDo> DSLDo { set; get; }
    }

    public partial class LDo
    {
        public string MLoi { set; get; }
        public string MTa { set; get; }
    }

    public partial class DLieu
    {
        public TBao TBao { set; get; }
    }
}
