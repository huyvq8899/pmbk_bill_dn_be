using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.IV._1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.IV.IV
{
    [XmlType(TypeName = "CTu")]
    public partial class CTu
    {

        public DLCTu DLCTu { get; set; }

        [XmlElement]
        public DSCKS DSCKS { get; set; }
    }
}
