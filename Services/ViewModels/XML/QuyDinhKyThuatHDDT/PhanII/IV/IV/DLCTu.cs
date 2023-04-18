using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.IV.IV
{
    public partial class DLCTu
    {
        [XmlAttribute(AttributeName = "Id")]
        public string Id { get; set; } = "SigningData";

        [XmlElement]
        public TTChung TTChung { get; set; }

        [XmlElement]
        public NDCTu NDCTu { get; set; }

        [XmlElement]
        public List<TTin> TTKhac { get; set; }
    }
}
