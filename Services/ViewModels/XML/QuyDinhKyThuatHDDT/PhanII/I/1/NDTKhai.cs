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
        [XmlElement]
        public DSCTSSDung DSCTSSDung { get; set; }
    }
}
