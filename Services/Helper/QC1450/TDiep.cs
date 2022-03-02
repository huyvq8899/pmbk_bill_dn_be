using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Services.Helper.QC1450
{

    [XmlRoot("TDiep")]
    public class TDiep
    {
        [XmlElement("TTChung")]
        public TTChung TTChung { set; get; }

        [XmlElement("DLieu")]
        public DLieu DLieu { set; get; }
    }

    public class DLieu
    {
        [XmlElement("HDon")]
        public HDon HDon { set; get; }

        [XmlElement("TKhai")]
        public TKhai TKhai { set; get; }
    }
}
