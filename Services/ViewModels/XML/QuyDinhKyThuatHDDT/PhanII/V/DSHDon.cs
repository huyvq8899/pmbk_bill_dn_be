using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.V
{
    public partial class DSHDon
    {
        [XmlArray("DSHDon")]
        [XmlArrayItem("BTHDLieu")]
        public List<HDon> DSHDons { get; set; }
    }
}
