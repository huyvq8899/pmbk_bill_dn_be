using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._3
{
    public partial class TBao
    {
        [XmlElement]
        public DLTBao DLTBao { get; set; }
        [XmlElement]
        public DSCKS DSCKS { get; set; }
    }
}
