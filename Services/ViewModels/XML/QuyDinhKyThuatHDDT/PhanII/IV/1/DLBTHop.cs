using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.IV._1
{
    public partial class DLBTHop
    {
        [XmlAttribute(AttributeName = "Id")]
        public string Id { get; set; } = "SigningData";
        [XmlElement(Namespace = "DLBTHop")]
        public TTChung TTChung { get; set;}

        [XmlElement]
        public NDBTHDLieu NDBTHDLieu { get; set;}
    }
}
