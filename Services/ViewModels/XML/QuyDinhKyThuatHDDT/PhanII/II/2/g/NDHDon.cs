using Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.g
{
    public partial class NDHDon
    {
        public NBan NBan { get; set; }
        public NMua NMua { get; set; }
        [XmlArray("DSHHDVu")]
        [XmlArrayItem("HHDVu")]
        public List<HHDVu> DSHHDVu { get; set; }
        public TToan TToan { get; set; }
    }
}
