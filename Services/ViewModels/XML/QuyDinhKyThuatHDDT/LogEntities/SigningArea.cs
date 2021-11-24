using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities
{
    public class SigningArea
    {
        [XmlAttribute(AttributeName = "Id")]
        public string Id { get; set; } = "SigningData";
    }
}
