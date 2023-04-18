
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities;
using HDonPOSGTGT = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h.HDon;
using HDonPOSBH = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.i.HDon;
using HDonPOSKhac = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j.HDon;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._10
{
    //------------------ Hóa đơn GTGT có mã của cơ quan thuế được khởi tạo từ máy tính tiền ----------------------
    public partial class TDiep
    {
        public TTChungThongDiep TTChung { get; set; }

        [XmlArray("DLieu")]
        [XmlArrayItem("HDon")]
        public List<HDonPOSGTGT> DLieu { get; set; }

        [XmlElement]
        public string CKSNNT { get; set; }
    }

    //------------------ Hóa đơn bán hàng có mã của cơ quan thuế được khởi tạo từ máy tính tiền ----------------------
    [XmlRoot(ElementName = "TDiep")]
    [XmlType(TypeName = "TDiep")]
    public partial class TDiep2
    {
        public TTChungThongDiep TTChung { get; set; }
        [XmlArray("DLieu")]
        [XmlArrayItem("HDon")]
        public List<HDonPOSBH> DLieu { get; set; }

        [XmlElement]
        public string CKSNNT { get; set; }

    }

    //------------------ Hóa đơn khác có mã của cơ quan thuế được khởi tạo từ máy tính tiền ----------------------
    [XmlRoot(ElementName = "TDiep")]
    [XmlType(TypeName = "TDiep")]
    public partial class TDiep3
    {
        public TTChungThongDiep TTChung { get; set; }
        [XmlArray("DLieu")]
        [XmlArrayItem("HDon")]
        public List<HDonPOSKhac> DLieu { get; set; }

        [XmlElement]
        public string CKSNNT { get; set; }

    }
}
