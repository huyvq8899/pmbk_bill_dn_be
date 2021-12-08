using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BKSOFT.TVAN
{
    public static class XMLHelper
    {
        //public static string FindMST(string strXML)
        //{
        //    string mst = string.Empty;

        //    // Create the XmlDocument.
        //    XmlDocument doc = new XmlDocument();
        //    doc.Load(strXML);

        //    //Display all the book titles.
        //    XmlNodeList elemList = doc.GetElementsByTagName("MST");
        //    for (int idx = 0; idx < elemList.Count; idx++)
        //    {
        //        mst = elemList[idx].InnerXml;
        //        break;
        //    }

        //    return mst;
        //}

        public static string GetMTDTChieu(string strXML)
        {
            string mTDTChieu = string.Empty;

            try
            {
                // Create the XmlDocument.
                XmlDocument doc = new XmlDocument();
                doc.Load(strXML);

                //Display all the book titles.
                XmlNodeList elemList = doc.GetElementsByTagName("MTDTChieu");
                for (int idx = 0; idx < elemList.Count; idx++)
                {
                    mTDTChieu = elemList[idx].InnerXml;
                    break;
                }
            }
            catch (Exception ex)
            {
                GPSFileLog.WriteLog(string.Empty, ex);
            }

            return mTDTChieu;
        }
    }
}
