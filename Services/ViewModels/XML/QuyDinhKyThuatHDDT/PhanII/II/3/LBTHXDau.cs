using Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._3.BTHopLBTHXDau;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._3
{
    public partial class LBTHXDau
    {
        [XmlArray("DSBTHop")]
        [XmlArrayItem("BTHop")]
        public List<BTHop> DSBTHop { get; set; }
    }
}
