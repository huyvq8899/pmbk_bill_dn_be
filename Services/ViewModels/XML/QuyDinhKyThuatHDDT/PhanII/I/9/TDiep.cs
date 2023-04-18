using Services.ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._9
{
    public partial class TDiep
    {
        [XmlElement]
        public TTChungThongDiep TTChung { get; set; }
        [XmlElement]
        public DLieu DLieu { get; set; }
    }
}
