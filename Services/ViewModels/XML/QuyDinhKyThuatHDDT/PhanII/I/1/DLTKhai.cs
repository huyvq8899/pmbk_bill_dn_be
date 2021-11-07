﻿using DocumentFormat.OpenXml.Presentation;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1
{
    public partial class DLTKhai
    {
        [XmlAttribute(AttributeName = "Id")]
        public string Id { get; set; } = "SigningData";
        public TTChung TTChung { get; set; }
        public NDTKhai NDTKhai { get; set; }
    }
}
