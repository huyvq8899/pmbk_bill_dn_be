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

        public TTChung TTChung { get; set;}
        public NDBTHDLieu NDBTHDLieu { get; set;}
    }
}
