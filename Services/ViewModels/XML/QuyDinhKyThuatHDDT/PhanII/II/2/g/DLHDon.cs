﻿using System.Xml.Serialization;
using TTChung = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.f.TTChung;
using TTKhac = Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a.TTKhac;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.g
{
    public partial class DLHDon
    {
        [XmlAttribute(AttributeName = "Id")]
        public string Id { get; set; } = "SigningData";

        public TTChung TTChung { get; set; }
        public NDHDon NDHDon { get; set; }
        public TTKhac TTKhac { get; set; }
    }
}
