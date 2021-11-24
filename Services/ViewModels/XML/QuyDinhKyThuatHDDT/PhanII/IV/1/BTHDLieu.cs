using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.IV._1
{
    /// <summary>
    /// Định dạng dữ liệu bảng tổng hợp dữ liệu hóa đơn điện tử gửi cơ quan thuế
    /// </summary>
    [XmlType(TypeName = "BTHDLieu")]
    public partial class BTHDLieu
    {
        [XmlElement]
        public DLBTHop DLBTHop { get; set; }
        [XmlElement]
        public DSCKS DSCKS { get; set; }
    }
}
