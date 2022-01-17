using BKSOFT.TVAN.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace BKSOFT.TVAN
{
    public static class XMLHelper
    {
        public static TTChung GetTTChungFromXML(string strXMl)
        {
            TTChung info = new TTChung();
            try
            {
                strXMl = strXMl.Trim();

                // Get Thông tin chung
                byte[] bytes = Encoding.UTF8.GetBytes(strXMl);
                using (MemoryStream ms = new MemoryStream(bytes))
                {
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
                }
            }
            catch (Exception ex)
            {
                GPSFileLog.WriteLog(string.Empty, ex);
            }

            return info;
        }

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

        public static bool HandlMessageFromTCT(string xML)
        {
            bool res = true;
            try
            {
                if (string.IsNullOrEmpty(xML))
                {
                    return false;
                }
                xML = xML.Trim();

                // Get API
                string api = ConfigurationManager.AppSettings["UrlAPI"];

                // Get Thông tin chung.
                TTChung info = GetTTChungFromXML(xML);
                if (string.IsNullOrEmpty(info.MTDiep))
                {
                    return false;
                }

                // Task call api
                Task<bool> task = null;

                // Get type invoice 
                List<int> lstTypeDetails = SQLHelper.GetInvoiceDetailType(ConfigurationManager.AppSettings["ConCus"], info.MST);
                if (lstTypeDetails.IndexOf(2) >= 0)
                {
                    // Push To hdbk.pmbk.vn
                    task = HTTPHelper.TCTPostData(string.Format("https://hdbk.pmbk.vn/{0}", api), Utilities.Base64Encode(xML), info.MTDTChieu, info.MST);
                    task.Wait();
                }
                else if (lstTypeDetails.IndexOf(3) >= 0)
                {
                    // Push To hkd.pmbk.vn
                    task = HTTPHelper.TCTPostData(string.Format("https://hkd.pmbk.vn/{0}", api), Utilities.Base64Encode(xML), info.MTDTChieu, info.MST);
                    task.Wait();
                }
                else
                {
                    // Push To hdbk.pmbk.vn
                    task = HTTPHelper.TCTPostData(string.Format("https://hdbk.pmbk.vn/{0}", api), Utilities.Base64Encode(xML), info.MTDTChieu, info.MST);
                    task.Wait();
                }    

                // Get status
                res = task.Result;

                // Add to log
                AddToLogTIVan(info, xML, res);

                // Push to web test
                if (!string.IsNullOrWhiteSpace(info.MST) && (info.MST.Contains("0105987432-999") || info.MST.Contains("0105987432-998")))
                {
                    task = HTTPHelper.TCTPostData(string.Format("https://hoadon-da.pmbk.vn/{0}", api), Utilities.Base64Encode(xML), info.MTDTChieu, info.MST);
                    task.Wait();
                }
            }
            catch (Exception ex)
            {
                res = false;

                GPSFileLog.WriteLog(xML, ex);
            }

            return res;
        }

        public static bool HandlMessageError(string xML)
        {
            bool res = true;
            try
            {
                if (string.IsNullOrEmpty(xML))
                {
                    return false;
                }
                xML = xML.Trim();

                // Get API
                string api = ConfigurationManager.AppSettings["UrlAPI"];

                // Get Thông tin chung.
                TTChung info = GetTTChungFromXML(xML);
                if (string.IsNullOrEmpty(info.MTDiep))
                {
                    return false;
                }

                // Task call api
                Task<bool> task = null;

                // Get type invoice 
                List<int> lstTypeDetails = SQLHelper.GetInvoiceDetailType(ConfigurationManager.AppSettings["ConCus"], info.MST);
                if (lstTypeDetails.IndexOf(2) >= 0)
                {
                    // Push To hdbk.pmbk.vn
                    task = HTTPHelper.TCTPostData(string.Format("https://hdbk.pmbk.vn/{0}", api), Utilities.Base64Encode(xML), info.MTDTChieu, info.MST);
                    task.Wait();
                }

                if (lstTypeDetails.IndexOf(3) >= 0)
                {
                    // Push To hkd.pmbk.vn
                    task = HTTPHelper.TCTPostData(string.Format("https://hkd.pmbk.vn/{0}", api), Utilities.Base64Encode(xML), info.MTDTChieu, info.MST);
                    task.Wait();
                }

                // Get status
                res = task.Result;

                // Push to web test
                if (!string.IsNullOrWhiteSpace(info.MST) && (info.MST.Contains("0105987432-999") || info.MST.Contains("0105987432-998")))
                {
                    task = HTTPHelper.TCTPostData(string.Format("https://hoadon-da.pmbk.vn/{0}", api), Utilities.Base64Encode(xML), info.MTDTChieu, info.MST);
                    task.Wait();
                }
            }
            catch (Exception ex)
            {
                res = false;

                GPSFileLog.WriteLog(xML, ex);
            }

            return res;
        }

        public static bool AddToLogTIVan(TTChung info, string Xml, bool status)
        {
            bool res = false;
            try
            {
                // Write log
                using (var db = new TCTTranferEntities())
                {
                    db.TIVans.Add(new TIVan
                    {
                        Id = Guid.NewGuid(),
                        DateTime = DateTime.Now,
                        MNGui = info.MNGui,
                        MNNhan = info.MNNhan,
                        MLTDiep = Convert.ToInt32(info.MLTDiep),
                        MTDiep = info.MTDiep,
                        MTDTChieu = info.MTDTChieu,
                        MST = info.MST,
                        DataXML = Xml,
                        Status = status
                    });

                    db.SaveChanges();
                }
                res = true;
            }
            catch (Exception ex)
            {
                GPSFileLog.WriteLog(string.Empty, ex);
            }

            return res;
        }

        //public static bool AddToLogSingeTIVan(TTChung info, string Xml, bool status)
        //{
        //    bool res = false;

        //    try
        //    {
        //        using (var db = new TCTTranferEntities())
        //        {
        //            db.usp_InsertMessage(DateTime.Now, info.MNGui, info.MNNhan, Convert.ToInt32(info.MLTDiep), info.MTDiep, info.MTDTChieu, info.MST, Xml, status);
        //        }

        //        res = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        GPSFileLog.WriteLog(string.Empty, ex);
        //    }

        //    return res;
        //}

    }
}
