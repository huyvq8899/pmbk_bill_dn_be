﻿using BKSOFT_KYSO.Modal;
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
        //private static Dictionary<string, string> dict = new Dictionary<string, string>();

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
                    case MLTDiep.TBTNVKQXLHDDTSSot:
                        HoaDonSaiSotSigning(msg, cert);         // III.3 Định dạng dữ liệu thông báo hóa đơn điện tử có sai sót
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
                XmlNode elemList = doc.SelectSingleNode("/TKhai/DLTKhai/TTChung/NLap");
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
                        XMLHelper.XMLSignWithNode3(msg, "/TKhai/DSCKS/NNT", cert);
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
                        XMLHelper.XMLSignWithNode3(msg, "/HDon/DSCKS/NBan", cert);
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
                XMLHelper.XMLSignWithNode3(msg, "/TBao/DSCKS/NBan", cert);
            }
            catch (Exception)
            {
                res = false;
                msg.TypeOfError = TypeOfError.SIGN_XML_ERROR;
                msg.Exception = TypeOfError.SIGN_XML_ERROR.GetEnumDescription();
            }

            return res;
        }

        //public static MessageObj SignInvoice(MessageObj msg)
        //{
        //    MessageObj resp = new MessageObj();

        //    try
        //    {
        //        // GET CERT
        //        X509Certificate2 cert = CertificateUtil.GetAllCertificateFromStore((msg.NBan).MST);
        //        if (cert == null)
        //        {
        //            resp.Type = (int)TYPE_MESSAGE.REP_SIGN_ERR;
        //            resp.ErrorType = (int)TYPE_ERROR.CERT_NOT_FOUND;
        //            resp.ContentError = TYPE_ERROR.CERT_NOT_FOUND.GetEnumDescription();

        //            MessageBox.Show(Constants.MSG_NOT_SELECT_CERT, Constants.MSG_TITLE_DIALOG, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

        //            return resp;
        //        }

        //        // CHECK MST
        //        //string pattern = "MST:(.*),";
        //        //string mstToken = Regex.Match(cert.Subject, pattern).Groups[1].Value;
        //        string mstToken = Utils.GetMaSoThueFromSubject(cert.Subject);
        //        if (msg.NBan == null)
        //        {
        //            resp.Type = (int)TYPE_MESSAGE.REP_SIGN_ERR;
        //            resp.ErrorType = (int)TYPE_ERROR.SALLER_EMPTY;
        //            resp.ContentError = TYPE_ERROR.SALLER_EMPTY.GetEnumDescription();

        //            FileLog.WriteLog("Thông tin người bán NULL");
        //            return resp;
        //        }
        //        else if (string.IsNullOrEmpty(msg.NBan.MST))
        //        {
        //            resp.Type = (int)TYPE_MESSAGE.REP_SIGN_ERR;
        //            resp.ErrorType = (int)TYPE_ERROR.TAXCODE_SALLER_EMPTY;
        //            resp.ContentError = TYPE_ERROR.TAXCODE_SALLER_EMPTY.GetEnumDescription();

        //            FileLog.WriteLog("MST người bán trống hoặc NULL");
        //            return resp;
        //        }
        //        else if (!(msg.NBan.MST).Equals(mstToken))
        //        {
        //            resp.Type = (int)TYPE_MESSAGE.REP_SIGN_ERR;
        //            resp.ErrorType = (int)TYPE_ERROR.TAXCODE_SALLER_DIFF;
        //            resp.ContentError = TYPE_ERROR.TAXCODE_SALLER_DIFF.GetEnumDescription();

        //            MessageBox.Show(Constants.MSG_MST_INVAILD, Constants.MSG_TITLE_DIALOG, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        //            return resp;
        //        }

        //        // CHECK INVAILD TIME
        //        DateTime curDate = DateTime.Now;
        //        if (curDate < cert.NotBefore || curDate > cert.NotAfter)
        //        {
        //            resp.Type = (int)TYPE_MESSAGE.REP_SIGN_ERR;
        //            resp.ErrorType = (int)TYPE_ERROR.SIGN_DATE_INVAILD;
        //            resp.ContentError = string.Format(TYPE_ERROR.SIGN_DATE_INVAILD.GetEnumDescription(), cert.NotBefore.ToString("dd/MM/yyyy"), cert.NotAfter.ToString("dd/MM/yyyy"));

        //            string temp = string.Format(Constants.MSG_DATE_INVAILD, cert.NotBefore.ToString("dd/MM/yyyy"), cert.NotAfter.ToString("dd/MM/yyyy"));
        //            MessageBox.Show(temp, Constants.MSG_TITLE_DIALOG, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

        //            return resp;
        //        }

        //        // Check invaild time
        //        if (msg.Type == (int)TYPE_MESSAGE.SIGN_INVOICE)
        //        {
        //            DateTime? invDate = XMLHelper.GetDateInvoice(msg.DataXML);

        //            if (invDate == null)
        //            {
        //                resp.Type = (int)TYPE_MESSAGE.REP_SIGN_ERR;
        //                resp.ErrorType = (int)TYPE_ERROR.DATE_INVOICE_INVAILD;
        //                resp.ContentError = TYPE_ERROR.DATE_INVOICE_INVAILD.GetEnumDescription();

        //                string temp = "Ngày lập hóa đơn không hợp lệ";
        //                MessageBox.Show(temp, Constants.MSG_TITLE_DIALOG, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        //                return resp;
        //            }
        //            else if ((invDate.Value.Year != curDate.Year) || (invDate.Value.Month != curDate.Month) || (invDate.Value.Day != curDate.Day))
        //            {
        //                resp.Type = (int)TYPE_MESSAGE.REP_SIGN_ERR;
        //                resp.ErrorType = (int)TYPE_ERROR.DATE_INVOICE_OTHER;
        //                resp.ContentError = string.Format(TYPE_ERROR.DATE_INVOICE_OTHER.GetEnumDescription(), curDate.ToString("dd/MM/yyyy"), invDate.Value.ToString("dd/MM/yyyy"));

        //                string temp = string.Format(Constants.MSG_DATE_INV_SIGN_INVAILD, curDate.ToString("dd/MM/yyyy"), invDate.Value.ToString("dd/MM/yyyy"));
        //                MessageBox.Show(temp, Constants.MSG_TITLE_DIALOG, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        //                return resp;
        //            }
        //        }

        //        // Sign Invoice PDF
        //        PDFSignatureHelper pdf = new PDFSignatureHelper(msg.DataPDF, new PdfCertificate(cert));
        //        pdf.TypeFindPositionSign = (TYPE_MESSAGE)msg.Type;
        //        pdf.NBan = msg.NBan;
        //        FeedBackFunction res = pdf.Sign();
        //        if (res.Status == Status.Success)
        //        {
        //            // PDF Signed
        //            resp.DataPDF = Utils.BytesToHexStr((pdf.Ms).ToArray());

        //            // Type message success.
        //            resp.Type = (int)TYPE_MESSAGE.REP_SIGN_SUC;

        //            // Sign Invoice XML
        //            if (msg.Type == (int)TYPE_MESSAGE.SIGN_INVOICE)
        //            {
        //                var _signXML = XMLHelper.SignFromURL(msg.DataXML, cert);
        //                if (_signXML.Status == Status.Success)
        //                {
        //                    string xml = _signXML.Content;
        //                    if (!string.IsNullOrEmpty(xml))
        //                    {
        //                        resp.DataXML = Utils.Base64Encode(xml);             // XML Signed
        //                                                                            //resp.DataXML = xml;             // XML Signed
        //                    }
        //                    else
        //                    {
        //                        resp.Type = (int)TYPE_MESSAGE.REP_SIGN_ERR;         // XML Signed Error
        //                        //resp.ContentErro = "Không ký được file XML";
        //                        resp.ErrorType = (int)TYPE_ERROR.SIGN_XML_ERROR;
        //                        resp.ContentError = TYPE_ERROR.SIGN_XML_ERROR.GetEnumDescription();
        //                    }
        //                }
        //                else
        //                {
        //                    resp.Type = (int)TYPE_MESSAGE.REP_SIGN_ERR;
        //                    //resp.ContentErro = _signXML.Content;
        //                    resp.ErrorType = (int)TYPE_ERROR.SIGN_XML_ERROR;
        //                    resp.ContentError = TYPE_ERROR.SIGN_XML_ERROR.GetEnumDescription();
        //                }
        //            }

        //            resp.NBan = msg.NBan;
        //        }
        //        else
        //        {
        //            resp.Type = (int)TYPE_MESSAGE.REP_SIGN_ERR;
        //            //resp.ContentErro = res.Content;
        //            resp.ErrorType = (int)TYPE_ERROR.SIGN_PDF_ERROR;
        //            resp.ContentError = TYPE_ERROR.SIGN_PDF_ERROR.GetEnumDescription();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        FileLog.WriteLog(string.Empty, ex);
        //    }

        //    return resp;
        //}

        //public static MessageObj SignMultipleInvoice(MessageObj msg)
        //{
        //    MessageObj resp = new MessageObj();

        //    try
        //    {
        //        // Check type multiple invoice
        //        if (msg.Type != (int)TYPE_MESSAGE.SIGN_MULTIPLE_INVOICE)
        //        {
        //            return resp;
        //        }

        //        X509Certificate2 cert = null;

        //        // Check token in list dictionary
        //        if (dict.ContainsKey((msg.NBan).MST))
        //        {
        //            string serial = dict[(msg.NBan).MST];

        //            byte[] sbytes = Utils.HexStringToByteArray(serial);
        //            Array.Reverse(sbytes);

        //            cert = PdfCertificate.FindBySerialId(StoreType.MY, sbytes);
        //            if (cert == null)
        //            {
        //                FileLog.WriteLog($"PDF_CERTIFICATE NOT FIND WITH SERIAL {serial}");

        //                resp.Type = (int)TYPE_MESSAGE.REP_SIGN_ERR;
        //                resp.ContentError = "Không tìm thấy chữ ký số";
        //                MessageBox.Show(Constants.MSG_NOT_SELECT_CERT, Constants.MSG_TITLE_DIALOG, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

        //                return resp;
        //            }
        //        }
        //        else
        //        {
        //            // GET CERT
        //            cert = CertificateUtil.GetAllCertificateFromStore((msg.NBan).MST);
        //            if (cert == null)
        //            {
        //                resp.Type = (int)TYPE_MESSAGE.REP_SIGN_ERR;
        //                resp.ContentError = "Không tìm thấy chữ ký số";
        //                MessageBox.Show(Constants.MSG_NOT_SELECT_CERT, Constants.MSG_TITLE_DIALOG, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

        //                return resp;
        //            }

        //            // Push to diction store
        //            dict.Add((msg.NBan).MST, cert.SerialNumber);
        //        }

        //        // CHECK MST
        //        string mstToken = Utils.GetMaSoThueFromSubject(cert.Subject);
        //        if (msg.NBan == null)
        //        {
        //            resp.Type = (int)TYPE_MESSAGE.REP_SIGN_ERR;

        //            FileLog.WriteLog("Thông tin người bán NULL");

        //            return resp;
        //        }
        //        else if (string.IsNullOrEmpty(msg.NBan.MST))
        //        {
        //            resp.Type = (int)TYPE_MESSAGE.REP_SIGN_ERR;

        //            FileLog.WriteLog("MST người bán trống hoặc NULL");

        //            return resp;
        //        }
        //        else if (!(msg.NBan.MST).Equals(mstToken))
        //        {
        //            resp.Type = (int)TYPE_MESSAGE.REP_SIGN_ERR;

        //            MessageBox.Show(Constants.MSG_MST_INVAILD, Constants.MSG_TITLE_DIALOG, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

        //            return resp;
        //        }

        //        // CHECK INVAILD TIME
        //        DateTime curDate = DateTime.Now;
        //        if (curDate < cert.NotBefore || curDate > cert.NotAfter)
        //        {
        //            resp.Type = (int)TYPE_MESSAGE.REP_SIGN_ERR;
        //            string temp = string.Format(Constants.MSG_DATE_INVAILD, cert.NotBefore.ToString("dd/MM/yyyy"), cert.NotAfter.ToString("dd/MM/yyyy"));
        //            MessageBox.Show(temp, Constants.MSG_TITLE_DIALOG, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

        //            return resp; ;
        //        }

        //        // Check invaild time
        //        if (msg.Type == (int)TYPE_MESSAGE.SIGN_INVOICE)
        //        {
        //            DateTime? invDate = XMLHelper.GetDateInvoice(msg.DataXML);

        //            if (invDate == null)
        //            {
        //                resp.Type = (int)TYPE_MESSAGE.REP_SIGN_ERR;
        //                string temp = "Ngày lập hóa đơn không hợp lệ";
        //                MessageBox.Show(temp, Constants.MSG_TITLE_DIALOG, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

        //                return resp; ;
        //            }
        //            else if ((invDate.Value.Year != curDate.Year) || (invDate.Value.Month != curDate.Month) || (invDate.Value.Day != curDate.Day))
        //            {
        //                FileLog.WriteLog(invDate.Value.ToString("yyyy-MM-dd"));
        //                resp.Type = (int)TYPE_MESSAGE.REP_SIGN_ERR;
        //                string temp = string.Format(Constants.MSG_DATE_INV_SIGN_INVAILD, curDate.ToString("dd/MM/yyyy"), invDate.Value.ToString("dd/MM/yyyy"));
        //                MessageBox.Show(temp, Constants.MSG_TITLE_DIALOG, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

        //                return resp; ;
        //            }
        //        }

        //        // Start begin sign invoice
        //        PDFSignatureHelper pdf = new PDFSignatureHelper(msg.DataPDF, new PdfCertificate(cert));
        //        pdf.TypeFindPositionSign = (TYPE_MESSAGE)msg.Type;
        //        pdf.NBan = msg.NBan;
        //        FeedBackFunction res = pdf.Sign();
        //        if (res.Status == Status.Success)
        //        {
        //            // PDF Signed
        //            resp.DataPDF = Utils.BytesToHexStr((pdf.Ms).ToArray());

        //            // Type message success.
        //            resp.Type = (int)TYPE_MESSAGE.REP_SIGN_SUC;

        //            // Sign Invoice XML
        //            var _signXML = XMLHelper.SignFromURL(msg.DataXML, cert);
        //            if (_signXML.Status == Status.Success)
        //            {
        //                string xml = _signXML.Content;
        //                if (!string.IsNullOrEmpty(xml))
        //                {
        //                    resp.DataXML = Utils.Base64Encode(xml);              // XML Signed
        //                                                                         // resp.DataXML = xml;             // XML Signed
        //                }
        //                else
        //                {
        //                    resp.Type = (int)TYPE_MESSAGE.REP_SIGN_ERR;         // XML Signed Error
        //                    resp.ContentError = "Không ký được file XML";
        //                }
        //            }
        //            else
        //            {
        //                resp.Type = (int)TYPE_MESSAGE.REP_SIGN_ERR;
        //                resp.ContentError = _signXML.Content;
        //            }

        //            resp.NBan = msg.NBan;
        //        }
        //        else
        //        {
        //            resp.Type = (int)TYPE_MESSAGE.REP_SIGN_ERR;
        //            resp.ContentError = res.Content;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        FileLog.WriteLog(string.Empty, ex);
        //    }

        //    return resp;
        //}
    }
}