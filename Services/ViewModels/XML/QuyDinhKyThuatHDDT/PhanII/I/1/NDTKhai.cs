using System.Collections.Generic;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1
{
    public partial class NDTKhai
    {
        [XmlElement]
        public HTHDon HTHDon { get; set; }
        [XmlElement]
        public HTGDLHDDT HTGDLHDDT { get; set; }
        [XmlElement]
        public PThuc PThuc { get; set; }
        [XmlElement]
        public LHDSDung LHDSDung { get; set; }
        [XmlArray("DSCTSSDung")]
        [XmlArrayItem("CTS")]
        public List<CTS> DSCTSSDung { get; set; }
    }
}
