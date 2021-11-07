using BKSOFT_KYSO.Modal;
using Newtonsoft.Json;
using Spire.Pdf.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace BKSOFT_KYSO
{
    public class Handler
    {
        public static string ProcessData(string encode)
        {
            MessageObj msg = new MessageObj();

            try
            {
                msg = JsonConvert.DeserializeObject<MessageObj>(encode);
                msg.TypeOfError = TypeOfError.NONE;
                msg.Exception = string.Empty;

                // Get certificate
                X509Certificate2 cert = CertificateUtil.GetAllCertificateFromStore(msg.MST);
                if (cert == null)
                {
                    msg.TypeOfError = TypeOfError.CERT_NOT_FOUND;
                    msg.Exception = TypeOfError.CERT_NOT_FOUND.GetEnumDescription();

                    MessageBox.Show(Constants.MSG_NOT_SELECT_CERT, Constants.MSG_TITLE_DIALOG, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    return JsonConvert.SerializeObject(msg);
                }

                // Checking taxcode
                string mstToken = Utils.GetMaSoThueFromSubject(cert.Subject);
                if (string.IsNullOrEmpty(msg.MST) || 
                    string.IsNullOrEmpty(mstToken) || 
                    !(msg.MST).Equals(mstToken))
                {
                    msg.TypeOfError = TypeOfError.TAXCODE_SALLER_DIFF;
                    msg.Exception = TypeOfError.TAXCODE_SALLER_DIFF.GetEnumDescription();

                    MessageBox.Show(Constants.MSG_MST_INVAILD, Constants.MSG_TITLE_DIALOG, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    return JsonConvert.SerializeObject(msg);
                }

                // Checking serial
                string serail = cert.SerialNumber.ToUpper();
                msg.Serials = (msg.Serials).Select(x => x.ToUpper()).ToList();
                if (!(msg.Serials).Contains(serail))
                {
                    msg.TypeOfError = TypeOfError.SERIAL_SALLER_DIFF;
                    msg.Exception = TypeOfError.SERIAL_SALLER_DIFF.GetEnumDescription();

                    MessageBox.Show(Constants.MSG_SERIAL_INVAILD, Constants.MSG_TITLE_DIALOG, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    return JsonConvert.SerializeObject(msg);
                }

                // Serial number
                msg.SerialSigned = serail;

                // Checking datetime
                DateTime curDate = DateTime.Now;
                if (curDate < cert.NotBefore || curDate > cert.NotAfter)
                {
                    msg.TypeOfError = TypeOfError.SIGN_DATE_INVAILD;
                    msg.Exception = string.Format(TypeOfError.SIGN_DATE_INVAILD.GetEnumDescription(), cert.NotBefore.ToString("dd/MM/yyyy"), cert.NotAfter.ToString("dd/MM/yyyy"));

                    string sTemp = string.Format(Constants.MSG_DATE_INVAILD, cert.NotBefore.ToString("dd/MM/yyyy"), cert.NotAfter.ToString("dd/MM/yyyy"));
                    MessageBox.Show(sTemp, Constants.MSG_TITLE_DIALOG, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    return JsonConvert.SerializeObject(msg);
                }

                // Ký số XML
                switch (msg.MLTDiep)
                {
                    case MLTDiep.TDGToKhai:                     // I.1 Định dạng dữ liệu tờ khai đăng ký/thay đổi thông tin sử dụng hóa đơn điện tử
                    case MLTDiep.TDGToKhaiUN:                   // I.2 Định dạng dữ liệu tờ khai đăng ký thay đổi thông tin đăng k‎ý sử dụng HĐĐT khi ủy nhiệm/nhận ủy nhiệm lập hoá đơn
                    case MLTDiep.TDDNCHDDT:                     // I.7 Định dạng dữ liệu đề nghị cấp hóa đơn điện tử có mã theo từng lần phát sinh
                        ToKhaiSigning(msg, cert);                        
                        break;
                    case MLTDiep.TDCDLHDKMDCQThue:              // II.1 Định dạng chung của hóa đơn điện tử
                        HoaDonSigning(msg, cert);
                        break;
                    case MLTDiep.TDTBHDDLSSot:
                        HoaDonSaiSotSigning(msg, cert);         // III.3 Định dạng dữ liệu thông báo hóa đơn điện tử có sai sót
                        break;
                    case MLTDiep.TDCBTHDLHDDDTDCQThue:          // 4. Bảng tổng hợp dữ liệu
                        BangTongHopDuLieuHoaDoan(msg, cert);
                        break;
                    default:
                        break;
                }

                FileLog.WriteLog(JsonConvert.SerializeObject(msg));
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(string.Empty, ex);
            }

            return JsonConvert.SerializeObject(msg);
        }

        private static bool ToKhaiSigning(MessageObj msg, X509Certificate2 cert)
        {
            bool res = true;

            try
            {
                DateTime? dt = null;

                // Reading XML from URL
                using (var wc = new WebClient())
                {
                    wc.Encoding = System.Text.Encoding.UTF8;
                    msg.DataXML = wc.DownloadString(msg.UrlXML);
                }

                // Load xml
                XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = true;
                doc.LoadXml(msg.DataXML);

                // Get Date of seller
                XmlNode elemList = doc.SelectSingleNode("/TDiep/DLieu/TKhai/DLTKhai/TTChung/NLap");
                if (elemList != null)
                {
                    dt = DateTime.ParseExact(elemList.InnerText, "yyyy-MM-dd", null);
                    if(dt?.Year != DateTime.Now.Year || dt?.Month != DateTime.Now.Month || dt?.Day != DateTime.Now.Day)
                    {
                        res = false;
                        msg.TypeOfError = TypeOfError.DATE_TKHAI_INVAILD;
                        msg.Exception = TypeOfError.DATE_TKHAI_INVAILD.GetEnumDescription();
                    }    
                    else
                    {
                        // Signing XML
                        XMLHelper.XMLSignWithNodeEx(msg, "/TDiep/DLieu/TKhai/DSCKS/NNT", cert);
                    }
                }
                else
                {
                    msg.TypeOfError = TypeOfError.DATE_TKHAI_INVAILD;
                    msg.Exception = TypeOfError.DATE_TKHAI_INVAILD.GetEnumDescription();
                }   
                
            }
            catch (Exception)
            {
                res = false;
                msg.TypeOfError = TypeOfError.DATE_TKHAI_INVAILD;
                msg.Exception = TypeOfError.DATE_TKHAI_INVAILD.GetEnumDescription();
            }

            return res;
        }

        private static bool HoaDonSigning(MessageObj msg, X509Certificate2 cert)
        {
            bool res = true;

            try
            {
                DateTime? dt = null;

                // Reading XML from URL
                using (var wc = new WebClient())
                {
                    wc.Encoding = System.Text.Encoding.UTF8;
                    msg.DataXML = wc.DownloadString(msg.UrlXML);
                }

                // Load xml
                XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = true;
                doc.LoadXml(msg.DataXML);

                // Get Date of seller
                XmlNode elemList = doc.SelectSingleNode("/HDon/DLHDon/TTChung/NLap");
                if (elemList != null)
                {
                    dt = DateTime.ParseExact(elemList.InnerText, "yyyy-MM-dd", null);
                    if (dt?.Year != DateTime.Now.Year || dt?.Month != DateTime.Now.Month || dt?.Day != DateTime.Now.Day)
                    {
                        res = false;
                        msg.TypeOfError = TypeOfError.DATE_INVOICE_INVAILD;
                        msg.Exception = TypeOfError.DATE_INVOICE_INVAILD.GetEnumDescription();
                    }
                    else
                    {
                        // Signing XML
                        XMLHelper.XMLSignWithNodeEx(msg, "/HDon/DSCKS/NBan", cert);
                    }
                }
                else
                {
                    msg.TypeOfError = TypeOfError.DATE_INVOICE_INVAILD;
                    msg.Exception = TypeOfError.DATE_INVOICE_INVAILD.GetEnumDescription();
                }

            }
            catch (Exception)
            {
                res = false;
                msg.TypeOfError = TypeOfError.SIGN_XML_ERROR;
                msg.Exception = TypeOfError.SIGN_XML_ERROR.GetEnumDescription();
            }

            return res;
        }

        private static bool HoaDonSaiSotSigning(MessageObj msg, X509Certificate2 cert)
        {
            bool res = true;

            try
            {
                // Reading XML from URL
                using (var wc = new WebClient())
                {
                    wc.Encoding = System.Text.Encoding.UTF8;
                    msg.DataXML = wc.DownloadString(msg.UrlXML);
                }

                // Load xml
                XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = true;
                doc.LoadXml(msg.DataXML);

                // Ký số thông báo
                XMLHelper.XMLSignWithNodeEx(msg, "/TDiep/DLieu/TBao/DSCKS/NNT", cert);
            }
            catch (Exception)
            {
                res = false;
                msg.TypeOfError = TypeOfError.SIGN_XML_ERROR;
                msg.Exception = TypeOfError.SIGN_XML_ERROR.GetEnumDescription();
            }

            return res;
        }

        private static bool BangTongHopDuLieuHoaDoan(MessageObj msg, X509Certificate2 cert)
        {
            bool res = true;

            try
            {
                // Reading XML from URL
                using (var wc = new WebClient())
                {
                    wc.Encoding = System.Text.Encoding.UTF8;
                    msg.DataXML = wc.DownloadString(msg.UrlXML);
                }

                // Load xml
                XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = true;
                doc.LoadXml(msg.DataXML);

                // Ký số thông báo
                XMLHelper.XMLSignWithNodeEx(msg, "/TDiep/DLieu/BTHDLieu/DSCKS/NNT", cert);
            }
            catch (Exception)
            {
                res = false;
                msg.TypeOfError = TypeOfError.SIGN_XML_ERROR;
                msg.Exception = TypeOfError.SIGN_XML_ERROR.GetEnumDescription();
            }

            return res;
        }
    }
}
