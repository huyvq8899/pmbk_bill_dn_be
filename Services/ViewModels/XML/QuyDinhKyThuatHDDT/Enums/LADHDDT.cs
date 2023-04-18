using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums
{
    [Serializable]
    public enum LADHDDT
    {
        [Description("Hóa đơn điện tử theo Nghị định số 123/2020/NĐ-CP")]
        [XmlEnum("1")]
        HinhThuc1 = 1,
        [Description("Hóa đơn điện tử có mã xác thực của cơ quan thuế theo Quyết định số 1209/QĐ-BTC ngày 23 tháng 6 năm 2015 và Quyết định số 2660/QĐ-BTC ngày 14 tháng 12 năm 2016 của Bộ Tài chính (Hóa đơn có mã xác thực của CQT theo Nghị định số 51/2010/NĐ-CP và Nghị định số 04/2014/NĐ-CP)")]
        [XmlEnum("2")]
        HinhThuc2 = 2,
        [Description("Các loại hóa đơn theo Nghị định số 51/2010/NĐ-CP và Nghị định số 04/2014/NĐ-CP (Trừ hóa đơn điện tử có mã xác thực của cơ quan thuế theo Quyết định số 1209/QĐ-BTC và Quyết định số 2660/QĐ-BTC)")]
        [XmlEnum("3")]
        HinhThuc3 = 3,
        [Description("Hóa đơn đặt in theo Nghị định số 123/2020/NĐ-CP")]
        [XmlEnum("4")]
        HinhThuc4 = 4,
    }
}
