using System.Collections.Generic;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._3
{
    public partial class LBTHKXDau
    {
        [XmlArray("DSBTHop")]
        [XmlArrayItem("BTHop")]
        public List<BTHopLBTHKXDau> DSBTHop { get; set; }
    }
}
