﻿
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities;
using HDonGTGT = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.HDon;
using HDonBH = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.b.HDon;
using HDonPXKVanChuyenNoiBo = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.e.HDon;
using HDonPXKBanDaiLy = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.f.HDon;
using HDonTaiSanCong = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.c.HDon;
using HDonDuTruQuocGia = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.d.HDon;
using HDonKhac = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.g.HDon;
using HDonNhieuTyGia = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.k.HDon;
using HDonGTGTKiemToKhai = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.l.HDon;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._5_6
{
    //------------------ Hóa đơn GTGT ----------------------
    public partial class TDiep
    {
        public TTChungThongDiep TTChung { get; set; }
        public DLieu DLieu { get; set; }
    }

    public partial class DLieu
    {
        public HDonGTGT HDon { get; set; }
    }

    //------------------ Hóa đơn bán hàng ----------------------
    [XmlRoot(ElementName = "TDiep")]
    [XmlType(TypeName = "TDiep")]
    public partial class TDiep2
    {
        public TTChungThongDiep TTChung { get; set; }
        public DLieu2 DLieu { get; set; }
    }

    //------------------ Phiếu xuất kho vận chuyển nội bộ ----------------------
    [XmlRoot(ElementName = "TDiep")]
    [XmlType(TypeName = "TDiep")]
    public partial class TDiep7
    {
        public TTChungThongDiep TTChung { get; set; }
        public DLieu7 DLieu { get; set; }
    }

    //------------------ Phiếu xuất kho bán đại lý ----------------------
    [XmlRoot(ElementName = "TDiep")]
    [XmlType(TypeName = "TDiep")]
    public partial class TDiep8
    {
        public TTChungThongDiep TTChung { get; set; }
        public DLieu8 DLieu { get; set; }
    }

    //------------------ Phiếu xuất kho bán đại lý ----------------------
    [XmlRoot(ElementName = "TDiep")]
    [XmlType(TypeName = "TDiep")]
    public partial class TDiep3
    {
        public TTChungThongDiep TTChung { get; set; }
        public DLieu3 DLieu { get; set; }
    }

    [XmlRoot(ElementName = "TDiep")]
    [XmlType(TypeName = "TDiep")]
    public partial class TDiep4
    {
        public TTChungThongDiep TTChung { get; set; }
        public DLieu4 DLieu { get; set; }
    }

    [XmlRoot(ElementName = "TDiep")]
    [XmlType(TypeName = "TDiep")]
    public partial class TDiep5
    {
        public TTChungThongDiep TTChung { get; set; }
        public DLieu5 DLieu { get; set; }
    }

    [XmlRoot(ElementName = "TDiep")]
    [XmlType(TypeName = "TDiep")]
    public partial class TDiep11
    {
        public TTChungThongDiep TTChung { get; set; }
        public DLieu11 DLieu { get; set; }
    }

    [XmlRoot(ElementName = "TDiep")]
    [XmlType(TypeName = "TDiep")]
    public partial class TDiep12
    {
        public TTChungThongDiep TTChung { get; set; }
        public DLieu12 DLieu { get; set; }
    }
    /// <summary>
    /// ///////////////////////////////////////////////////////////////////
    /// </summary>

    public partial class DLieu2
    {
        public HDonBH HDon { get; set; }
    }

    public partial class DLieu7
    {
        public HDonPXKVanChuyenNoiBo HDon { get; set; }
    }

    public partial class DLieu8
    {
        public HDonPXKBanDaiLy HDon { get; set; }
    }

    public partial class DLieu3
    {
        public HDonTaiSanCong HDon { get; set; }
    }
    public partial class DLieu4
    {
        public HDonDuTruQuocGia HDon { get; set; }
    }
    public partial class DLieu5
    {
        public HDonKhac HDon { get; set; }
    }
    public partial class DLieu11
    {
        public HDonNhieuTyGia HDon { get; set; }
    }
    public partial class DLieu12
    {
        public HDonGTGTKiemToKhai HDon { get; set; }
    }
}
