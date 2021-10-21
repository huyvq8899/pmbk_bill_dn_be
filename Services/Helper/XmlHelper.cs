using Services.Helper;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Services.Helper
{
    public static class XmlHelper
    {
        public static TTChungThongDiep GetTTChungFromBase64(string base64)
        {
            TTChungThongDiep result = new TTChungThongDiep();

            var xmlContent = DataHelper.Base64Decode(base64);
            byte[] encodedString = Encoding.UTF8.GetBytes(xmlContent);
            MemoryStream ms = new MemoryStream(encodedString);
            ms.Flush();
            ms.Position = 0;
            using (StreamReader reader = new StreamReader(ms))
            {
                XDocument xDoc = XDocument.Load(reader);
                result = xDoc.Descendants("TTChung")
                   .Select(x => new TTChungThongDiep
                   {
                       PBan = x.Element(nameof(result.PBan)).Value,
                       MNGui = x.Element(nameof(result.MNGui)).Value,
                       MNNhan = x.Element(nameof(result.MNNhan)).Value,
                       MLTDiep = x.Element(nameof(result.MLTDiep)).Value,
                       MTDiep = x.Element(nameof(result.MTDiep)).Value,
                       MST = x.Element(nameof(result.MST)).Value,
                       MTDTChieu = x.Element(nameof(result.MTDTChieu)).Value,
                   })
                   .FirstOrDefault();
            }
            return result;
        }


    }
}
