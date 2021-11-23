using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace BKSOFT.EMU
{
    public class GPHDHelper
    {
        public static void SendXML(string path)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                string xML = doc.InnerXml;

                if (string.IsNullOrEmpty(xML))
                {
                    return;
                }

                xML = xML.Trim();
                TTChung info = new TTChung();

                // Get Thông tin chung
                byte[] encodedString = Encoding.UTF8.GetBytes(xML);
                MemoryStream ms = new MemoryStream(encodedString);
                using (StreamReader reader = new StreamReader(ms))
                {
                    XDocument xDoc = XDocument.Load(reader);
                    info = xDoc.Descendants("TTChung")
                                   .Select(x => new TTChung
                                   {
                                       PBan = x.Element(nameof(info.PBan)).Value,
                                       MNGui = x.Element(nameof(info.MNGui)).Value,
                                       MNNhan = x.Element(nameof(info.MNNhan)).Value,
                                       MLTDiep = x.Element(nameof(info.MLTDiep)).Value,
                                       MTDiep = x.Element(nameof(info.MTDiep)).Value,
                                       MTDTChieu = x.Element(nameof(info.MTDTChieu)).Value,
                                       MST = x.Element(nameof(info.MST)).Value
                                   }).FirstOrDefault();
                }

                // Push to server
                string strXMLEncode = Utilities.Base64Encode(xML);
                Task<bool> task = HTTPHelper.TCTPostData(ConfigurationManager.AppSettings["UrlAPI"], strXMLEncode, info.MTDTChieu, info.MST);
                task.Wait();
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(string.Empty, ex);
            }
        }
    }
}
