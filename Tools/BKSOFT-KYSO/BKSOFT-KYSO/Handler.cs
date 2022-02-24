using BKSOFT.UTILITY;
using BKSOFT_KYSO.Modal;
using Newtonsoft.Json;
using Spire.Pdf.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace BKSOFT_KYSO
{
    public class Handler
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetSystemTime(ref SYSTEMTIME st);

        private static Dictionary<string, string> dict = new Dictionary<string, string>();

        public static string ProcessData(string encode)
        {
            MessageObj msg = new MessageObj();

            try
            {
                msg = JsonConvert.DeserializeObject<MessageObj>(encode);
                msg.TypeOfError = TypeOfError.NONE;
                msg.Exception = string.Empty;

                // View Certificate
                if (msg.MLTDiep == MLTDiep.CTSInfo)
                {
                    if (msg.Cert != null)
                    {
                        X509Certificate2UI.DisplayCertificate(new X509Certificate2(msg.Cert));

                        return JsonConvert.SerializeObject(msg);
                    }
                    else
                    {
                        msg.TypeOfError = TypeOfError.CERT_NOT_FOUND;
                        msg.Exception = string.Empty;

                        return JsonConvert.SerializeObject(msg);
                    }
                }

                // Check tool signed TT32
                if (msg.Type >= 1000)
                {
                    msg.MST = (msg.NBan).MST;
                    msg.TTNKy = new TTNKy
                    {
                        Ten = (msg.NBan).Ten,
                        SDThoai = (msg.NBan).SDThoai,
                        DChi = (msg.NBan).DChi,
                        TenP1 = (msg.NBan).TenP1,
                        TenP2 = (msg.NBan).TenP2
                    };
                    msg.IsTT32 = true;
                }

                // Certificate
                X509Certificate2 cert = null;

                // Check multiple sign invoice
                if (msg.Type == 1003 && dict.ContainsKey((msg.NBan).MST))
                {
                    string serial = dict[(msg.NBan).MST];

                    byte[] sbytes = Utils.HexStringToByteArray(serial);
                    Array.Reverse(sbytes);

                    // Find
                    cert = PdfCertificate.FindBySerialId(StoreType.MY, sbytes);
                }
                else
                {
                    // TaxCode with P12
                    string path = AppDomain.CurrentDomain.BaseDirectory;
                    if (msg.MST.Contains("0105987432-999") && File.Exists(Path.Combine(path, "SDS_TVAN/0105987432-999.p12")))
                    {
                        cert = new X509Certificate2(Path.Combine(path, "SDS_TVAN/0105987432-999.p12"), "1");
                    }
                    else if (msg.MST.Contains("0105987432-998") && File.Exists(Path.Combine(path, "SDS_TVAN/0105987432-998.p12")))
                    {
                        cert = new X509Certificate2(Path.Combine(path, "SDS_TVAN/0105987432-998.p12"), "1");
                    }
                    else if (msg.MST.Contains("0200784873-999") && File.Exists(Path.Combine(path, "SDS_TVAN/0200784873-999.p12")))
                    {
                        cert = new X509Certificate2(Path.Combine(path, "SDS_TVAN/0200784873-999.p12"), "123456");
                    }
                    else if (msg.MST.Contains("0200784873-998") && File.Exists(Path.Combine(path, "SDS_TVAN/0200784873-998.p12")))
                    {
                        cert = new X509Certificate2(Path.Combine(path, "SDS_TVAN/0200784873-998.p12"), "123456");
                    }
                    else
                    {
                        cert = CertificateUtil.GetAllCertificateFromStore(msg.MST);
                    }
                }

                // Check certificate
                if (cert == null)
                {
                    msg.TypeOfError = TypeOfError.CERT_NOT_FOUND;
                    msg.Exception = TypeOfError.CERT_NOT_FOUND.GetEnumDescription();

                    MessageBox.Show(Constants.MSG_NOT_SELECT_CERT, Constants.MSG_TITLE_DIALOG, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    return JsonConvert.SerializeObject(msg);
                }
                else if (msg.MLTDiep == MLTDiep.TTCTSo)
                {
                    string strDate = cert.GetEffectiveDateString();
                    DateTime NotBefore = DateTime.Now;
                    bool beff = Utils.GetDateTimeString(strDate, ref NotBefore);

                    strDate = cert.GetExpirationDateString();
                    DateTime NotAfter = DateTime.Now;
                    bool bexp = Utils.GetDateTimeString(strDate, ref NotAfter);
                    if (beff && bexp)
                    {
                        msg.DataJson = JsonConvert.SerializeObject(
                                                   new
                                                   {
                                                       Issuer = cert.Issuer,
                                                       IssuerName = cert.IssuerName,
                                                       Subject = cert.Subject,
                                                       SerialNumber = cert.SerialNumber.ToUpper(),
                                                       NotBefore = NotBefore,
                                                       NotAfter = NotAfter
                                                   }, Newtonsoft.Json.Formatting.Indented);
                    }
                    else
                    {
                        msg.DataJson = JsonConvert.SerializeObject(
                                                   new
                                                   {
                                                       Issuer = cert.Issuer,
                                                       IssuerName = cert.IssuerName,
                                                       Subject = cert.Subject,
                                                       SerialNumber = cert.SerialNumber.ToUpper(),
                                                       NotBefore = cert.NotBefore,
                                                       NotAfter = cert.NotAfter
                                                   }, Newtonsoft.Json.Formatting.Indented);
                    }

                    FileLog.WriteLog(JsonConvert.SerializeObject(msg));

                    return JsonConvert.SerializeObject(msg);
                }
                else if (msg.MLTDiep == MLTDiep.BBCBenB)             // Ký số biên bản cho bên A.
                {
                    PDFHelper pdf = new PDFHelper(msg, new PdfCertificate(cert));
                    bool res = pdf.Sign();
                    if (res)
                    {
                        msg.DataPDF = Utils.BytesToHexStr((pdf.Ms).ToArray());
                    }
                    else
                    {
                        msg.TypeOfError = TypeOfError.SIGN_PDF_ERROR;
                        msg.Exception = TypeOfError.SIGN_PDF_ERROR.GetEnumDescription();
                    }

                    return JsonConvert.SerializeObject(msg);
                }

                // Checking taxcode
                string mstToken = Utils.GetMaSoThueFromSubject(cert.Subject);
                if (string.IsNullOrEmpty(msg.MST) ||
                    string.IsNullOrEmpty(mstToken) ||
                    !(msg.MST).Equals(mstToken))
                {
                    msg.TypeOfError = TypeOfError.MST_KHONG_HLe;
                    msg.Exception = TypeOfError.MST_KHONG_HLe.GetEnumDescription();

                    MessageBox.Show(Constants.MSG_MST_INVAILD, Constants.MSG_TITLE_DIALOG, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    return JsonConvert.SerializeObject(msg);
                }

                // Checking datetime
                DateTime curDate = DateTime.Now;
                if (curDate < cert.NotBefore || curDate > cert.NotAfter)
                {
                    msg.TypeOfError = TypeOfError.NKY_KHONG_HLe;
                    msg.Exception = $"Chứng thư chỉ ký số trong khoảng thời gian từ {cert.NotBefore.ToString("dd/MM/yyyy HH:mm:ss")} đến {cert.NotAfter.ToString("dd/MM/yyyy HH:mm:ss")}";

                    string sTemp = string.Format(Constants.MSG_DATE_INVAILD, cert.NotBefore.ToString("dd/MM/yyyy HH:mm:ss"), cert.NotAfter.ToString("dd/MM/yyyy HH:mm:ss"));
                    MessageBox.Show(sTemp, Constants.MSG_TITLE_DIALOG, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    return JsonConvert.SerializeObject(msg);
                }

                // Handler for TT32
                if (msg.IsTT32)
                {
                    // Add multiple signed
                    if (msg.Type == 1003 && !dict.ContainsKey((msg.NBan).MST))
                    {
                        dict.Add((msg.NBan).MST, cert.SerialNumber);
                    }

                    HandlSignForTT32(msg, cert);
                    // Return json.
                    return JsonConvert.SerializeObject(msg);
                }

                // Checking serial
                if (msg.Serials != null && msg.Serials.Any())
                {
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

                }

                // Get algorithm
                MessageDigestAlgorithm algo = MessageDigestAlgorithm.SHA1;
                string friendlyName = cert.SignatureAlgorithm.FriendlyName;

                // Check algorithm
                FileLog.WriteLog(string.Format("SignatureAlgorithm {0}", friendlyName));
                if (!string.IsNullOrEmpty(friendlyName) && friendlyName.Contains("256"))
                {
                    algo = MessageDigestAlgorithm.SHA256;
                }

                // Ký số XML
                switch (msg.MLTDiep)
                {
                    case MLTDiep.TDGToKhai:                     // I.1 Định dạng dữ liệu tờ khai đăng ký/thay đổi thông tin sử dụng hóa đơn điện tử
                    case MLTDiep.TDGToKhaiUN:                   // I.2 Định dạng dữ liệu tờ khai đăng ký thay đổi thông tin đăng k‎ý sử dụng HĐĐT khi ủy nhiệm/nhận ủy nhiệm lập hoá đơn
                    case MLTDiep.TDDNCHDDT:                     // I.7 Định dạng dữ liệu đề nghị cấp hóa đơn điện tử có mã theo từng lần phát sinh
                        ToKhaiSigning(msg, cert, algo);
                        break;
                    case MLTDiep.TDCDLHDKMDCQThue:              // II.1 Định dạng chung của hóa đơn điện tử
                        HoaDonSigning(msg, cert, algo);
                        break;
                    case MLTDiep.TDTBHDDLSSot:
                        HoaDonSaiSotSigning(msg, cert, algo);         // III.3 Định dạng dữ liệu thông báo hóa đơn điện tử có sai sót
                        break;
                    case MLTDiep.TDCBTHDLHDDDTDCQThue:          // 4. Bảng tổng hợp dữ liệu
                        BangTongHopDuLieuHoaDoan(msg, cert, algo);
                        break;
                    case MLTDiep.BBCBenA:                       // Ký số biên bản cho bên A người mua.
                        PDFHelper pdf = new PDFHelper(msg, new PdfCertificate(cert));
                        bool res = pdf.Sign();
                        if (res)
                        {
                            msg.DataPDF = Utils.BytesToHexStr((pdf.Ms).ToArray());
                        }
                        else
                        {
                            msg.TypeOfError = TypeOfError.SIGN_PDF_ERROR;
                            msg.Exception = TypeOfError.SIGN_PDF_ERROR.GetEnumDescription();
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(string.Empty, ex);
            }

            return JsonConvert.SerializeObject(msg);
        }

        private static bool ToKhaiSigning(MessageObj msg, X509Certificate2 cert, MessageDigestAlgorithm algo = MessageDigestAlgorithm.SHA256)
        {
            bool res = true;

            try
            {
                DateTime? dt = null;
                DateTime now = DateTime.Now;
                DateTime dtsys = new DateTime(now.Year, now.Month, now.Day);

                // Reading XML from URL
                if (!string.IsNullOrWhiteSpace(msg.DataXML))
                {
                    msg.DataXML = Utils.Base64Decode(msg.DataXML);
                }
                else
                {
                    using (var wc = new WebClient())
                    {
                        wc.Encoding = System.Text.Encoding.UTF8;
                        msg.DataXML = wc.DownloadString(msg.UrlXML);
                    }
                }

                // Check vaild datetime
                string strDateTime = XMLHelper.GetSingleNodeValue(msg.DataXML, "/TDiep/DLieu/TKhai/DLTKhai/TTChung/NLap");
                if (string.IsNullOrEmpty(strDateTime))
                {
                    res = false;
                    msg.TypeOfError = TypeOfError.NLAP_TKHAI_TRONG;
                    msg.Exception = TypeOfError.NLAP_TKHAI_TRONG.GetEnumDescription();
                }
                else
                {
                    dt = DateTime.ParseExact(strDateTime, "yyyy-MM-dd", null);
                    if (dt > dtsys)
                    {
                        res = false;
                        msg.TypeOfError = TypeOfError.NLAP_TKHAI_KHLe;
                        msg.Exception = $"Ngày lập tờ khai không hợp lệ. Ngày lập {strDateTime} > ngày hiện tại {dtsys.ToString("yyyy-MM-dd")}";
                    }
                    else
                    {
                        byte[] signData = null;

                        // Alogrithm SHA256
                        if (algo == MessageDigestAlgorithm.SHA256)
                        {
                            // Load xml & cert
                            byte[] unsignData = Encoding.UTF8.GetBytes(msg.DataXML);
                            string certBase64 = Convert.ToBase64String(cert.RawData);
                            IHashSigner signers = HashSignerFactory.GenerateSigner(unsignData, certBase64, null, HashSignerFactory.XML);
                            ((XmlHashSigner)signers).SetHashAlgorithm(MessageDigestAlgorithm.SHA256);

                            // Signing XML
                            ((XmlHashSigner)signers).SetReferenceId("#SigningData");
                            ((XmlHashSigner)signers).SetSigningTime(DateTime.Now, "SigningTime");
                            ((XmlHashSigner)signers).SetParentNodePath("/TDiep/DLieu/TKhai/DSCKS/NNT");
                            // Get hash
                            var hashValues = signers.GetSecondHashAsBase64();
                            var datasigned = signers.SignHash(cert, hashValues);
                            signData = signers.Sign(datasigned);
                            if (signData == null)
                            {
                                msg.TypeOfError = TypeOfError.KSO_XML_LOI;
                            }
                        }
                        // Alogrithm SHA1
                        else if (algo == MessageDigestAlgorithm.SHA1)
                        {
                            XmlNormalSigner xmlSigner = new XmlNormalSigner(Encoding.UTF8.GetBytes(msg.DataXML), cert);

                            // Signing XML
                            xmlSigner.SetReferenceId("#SigningData");
                            xmlSigner.SetSigningTime(DateTime.Now, "SigningTime");
                            xmlSigner.SetParentNodePath("/TDiep/DLieu/TKhai/DSCKS/NNT");

                            signData = xmlSigner.Sign();
                            if (signData == null)
                            {
                                msg.TypeOfError = TypeOfError.KSO_XML_LOI;
                                msg.Exception = xmlSigner.GetException();
                            }
                        }

                        // Set for response
                        msg.DataXML = string.Empty;         // Clear XML
                        msg.DataPDF = string.Empty;
                        if (msg.IsCompression)
                        {
                            msg.XMLSigned = Encoding.UTF8.GetString(signData);
                            msg.XMLSigned = Utils.Compress(msg.XMLSigned);
                        }
                        else
                        {
                            msg.XMLSigned = Convert.ToBase64String(signData);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                res = false;
                msg.TypeOfError = TypeOfError.KSO_XML_LOI;
                msg.Exception = ex.ToString();

                LogFile.WriteLog(string.Empty, ex);
            }

            return res;
        }

        private static bool HoaDonSigning(MessageObj msg, X509Certificate2 cert, MessageDigestAlgorithm algo = MessageDigestAlgorithm.SHA256)
        {
            bool res = true;

            try
            {
                DateTime? dt = null;
                DateTime now = DateTime.Now;
                DateTime dtsys = new DateTime(now.Year, now.Month, now.Day);

                // Reading XML from URL
                if (!string.IsNullOrWhiteSpace(msg.DataXML))
                {
                    if (msg.IsCompression)
                    {
                        msg.DataXML = Utils.Decompress(msg.DataXML);
                    }
                    else
                    {
                        msg.DataXML = Utils.Base64Decode(msg.DataXML);
                    }
                }
                else
                {
                    using (var wc = new WebClient())
                    {
                        wc.Encoding = System.Text.Encoding.UTF8;
                        msg.DataXML = wc.DownloadString(msg.UrlXML);
                    }
                }

                // Check vaild datetime
                string strDateTime = XMLHelper.GetSingleNodeValue(msg.DataXML, "/TDiep/DLieu//HDon/DLHDon/TTChung/NLap");
                if (string.IsNullOrEmpty(strDateTime))
                {
                    res = false;
                    msg.TypeOfError = TypeOfError.NLAP_HDON_TRONG;
                    msg.Exception = TypeOfError.NLAP_HDON_TRONG.GetEnumDescription();
                }
                else
                {
                    dt = DateTime.ParseExact(strDateTime, "yyyy-MM-dd", null);
                    if (dt > dtsys)
                    {
                        res = false;
                        msg.TypeOfError = TypeOfError.NLAP_HDON_KHLe;
                        msg.Exception = $"Ngày lập hóa đơn không hợp lệ. Ngày lập {strDateTime} > ngày hiện tại {dtsys.ToString("yyyy-MM-dd")}";
                    }
                    else
                    {
                        byte[] signData = null;

                        // Alogrithm SHA256
                        if (algo == MessageDigestAlgorithm.SHA256)
                        {
                            // Load xml & cert
                            byte[] unsignData = Encoding.UTF8.GetBytes(msg.DataXML);
                            string certBase64 = Convert.ToBase64String(cert.RawData);
                            IHashSigner signers = HashSignerFactory.GenerateSigner(unsignData, certBase64, null, HashSignerFactory.XML);
                            ((XmlHashSigner)signers).SetHashAlgorithm(MessageDigestAlgorithm.SHA256);

                            // Signing XML
                            ((XmlHashSigner)signers).SetReferenceId("#SigningData");
                            ((XmlHashSigner)signers).SetSigningTime(DateTime.Now, "SigningTime");

                            // Check persion sign
                            if (msg.IsNMua)
                            {
                                ((XmlHashSigner)signers).SetParentNodePath("/TDiep/DLieu/HDon/DSCKS/NMua");
                            }
                            else
                            {
                                ((XmlHashSigner)signers).SetParentNodePath("/TDiep/DLieu/HDon/DSCKS/NBan");
                            }

                            // Get hash
                            var hashValues = signers.GetSecondHashAsBase64();
                            var datasigned = signers.SignHash(cert, hashValues);
                            signData = signers.Sign(datasigned);
                            if (signData == null)
                            {
                                msg.TypeOfError = TypeOfError.KSO_XML_LOI;
                            }
                        }
                        // Alogrithm SHA1
                        else if (algo == MessageDigestAlgorithm.SHA1)
                        {
                            XmlNormalSigner xmlSigner = new XmlNormalSigner(Encoding.UTF8.GetBytes(msg.DataXML), cert);
                            // Signing XML
                            xmlSigner.SetReferenceId("#SigningData");
                            xmlSigner.SetSigningTime(DateTime.Now, "SigningTime");

                            // Check persion sign
                            if (msg.IsNMua)
                            {
                                xmlSigner.SetParentNodePath("/TDiep/DLieu/HDon/DSCKS/NMua");
                            }
                            else
                            {
                                xmlSigner.SetParentNodePath("/TDiep/DLieu/HDon/DSCKS/NBan");
                            }

                            // sign
                            signData = xmlSigner.Sign();
                            if (signData == null)
                            {
                                msg.TypeOfError = TypeOfError.KSO_XML_LOI;
                                msg.Exception = xmlSigner.GetException();
                            }
                        }

                        // Set for response
                        msg.DataXML = string.Empty;         // Clear XML
                        msg.DataPDF = string.Empty;         // Clear PDF
                        if (msg.IsCompression)
                        {
                            msg.XMLSigned = Encoding.UTF8.GetString(signData);
                            msg.XMLSigned = Utils.Compress(msg.XMLSigned);
                        }
                        else
                        {
                            msg.XMLSigned = Convert.ToBase64String(signData);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                FileLog.WriteLog(string.Empty, ex);

                res = false;
                msg.TypeOfError = TypeOfError.KSO_XML_LOI;
                msg.Exception = ex.ToString();
            }

            return res;
        }

        private static bool HoaDonSaiSotSigning(MessageObj msg, X509Certificate2 cert, MessageDigestAlgorithm algo = MessageDigestAlgorithm.SHA256)
        {
            bool res = true;

            try
            {
                // Reading XML from URL
                if (!string.IsNullOrWhiteSpace(msg.DataXML))
                {
                    msg.DataXML = Utils.Base64Decode(msg.DataXML);
                }
                else
                {
                    using (var wc = new WebClient())
                    {
                        wc.Encoding = System.Text.Encoding.UTF8;
                        msg.DataXML = wc.DownloadString(msg.UrlXML);
                    }
                }

                byte[] signData = null;

                // Alogrithm SHA256
                if (algo == MessageDigestAlgorithm.SHA256)
                {

                    // Load xml & cert
                    byte[] unsignData = Encoding.UTF8.GetBytes(msg.DataXML);
                    string certBase64 = Convert.ToBase64String(cert.RawData);
                    IHashSigner signers = HashSignerFactory.GenerateSigner(unsignData, certBase64, null, HashSignerFactory.XML);
                    ((XmlHashSigner)signers).SetHashAlgorithm(MessageDigestAlgorithm.SHA256);

                    // Signing XML
                    ((XmlHashSigner)signers).SetReferenceId("#SigningData");
                    ((XmlHashSigner)signers).SetSigningTime(DateTime.Now, "SigningTime");
                    ((XmlHashSigner)signers).SetParentNodePath("/TDiep/DLieu/TBao/DSCKS/NNT");
                    //byte[] signData = xmlSigner.Sign();
                    var hashValues = signers.GetSecondHashAsBase64();
                    var datasigned = signers.SignHash(cert, hashValues);
                    signData = signers.Sign(datasigned);
                    if (signData == null)
                    {
                        msg.TypeOfError = TypeOfError.KSO_XML_LOI;
                    }
                }
                // Alogrithm SHA1
                else if (algo == MessageDigestAlgorithm.SHA1)
                {
                    // Load xml & cert
                    XmlNormalSigner xmlSigner = new XmlNormalSigner(Encoding.UTF8.GetBytes(msg.DataXML), cert);

                    // Signing XML
                    xmlSigner.SetReferenceId("#SigningData");
                    xmlSigner.SetSigningTime(DateTime.Now, "SigningTime");
                    xmlSigner.SetParentNodePath("/TDiep/DLieu/TBao/DSCKS/NNT");
                    signData = xmlSigner.Sign();
                    if (signData == null)
                    {
                        msg.TypeOfError = TypeOfError.KSO_XML_LOI;
                        msg.Exception = xmlSigner.GetException();
                    }
                }

                // Set for response
                msg.DataXML = string.Empty;         // Clear XML
                msg.DataPDF = string.Empty;         // Clear PDF                
                if (msg.IsCompression)
                {
                    msg.XMLSigned = Encoding.UTF8.GetString(signData);
                    msg.XMLSigned = Utils.Compress(msg.XMLSigned);
                }
                else
                {
                    msg.XMLSigned = Convert.ToBase64String(signData);
                }
            }
            catch (Exception ex)
            {
                res = false;
                msg.TypeOfError = TypeOfError.KSO_XML_LOI;
                msg.Exception = ex.ToString();
            }

            return res;
        }

        private static bool BangTongHopDuLieuHoaDoan(MessageObj msg, X509Certificate2 cert, MessageDigestAlgorithm algo = MessageDigestAlgorithm.SHA256)
        {
            bool res = true;

            try
            {
                // Reading XML from URL
                if (!string.IsNullOrWhiteSpace(msg.DataXML))
                {
                    msg.DataXML = Utils.Base64Decode(msg.DataXML);
                }
                else
                {
                    using (var wc = new WebClient())
                    {
                        wc.Encoding = System.Text.Encoding.UTF8;
                        msg.DataXML = wc.DownloadString(msg.UrlXML);
                    }
                }

                byte[] signData = null;

                // Alogrithm SHA256
                if (algo == MessageDigestAlgorithm.SHA256)
                {
                    // Load xml & cert
                    byte[] unsignData = Encoding.UTF8.GetBytes(msg.DataXML);
                    string certBase64 = Convert.ToBase64String(cert.RawData);
                    IHashSigner signers = HashSignerFactory.GenerateSigner(unsignData, certBase64, null, HashSignerFactory.XML);
                    ((XmlHashSigner)signers).SetHashAlgorithm(MessageDigestAlgorithm.SHA256);

                    // Signing XML
                    ((XmlHashSigner)signers).SetReferenceId("#SigningData");
                    ((XmlHashSigner)signers).SetSigningTime(DateTime.Now, "SigningTime");
                    ((XmlHashSigner)signers).SetParentNodePath("/TDiep/DLieu/BTHDLieu/DSCKS/NNT");
                    var hashValues = signers.GetSecondHashAsBase64();
                    var datasigned = signers.SignHash(cert, hashValues);
                    signData = signers.Sign(datasigned);
                    if (signData == null)
                    {
                        msg.TypeOfError = TypeOfError.KSO_XML_LOI;
                    }
                }
                // Alogrithm SHA1
                else if (algo == MessageDigestAlgorithm.SHA1)
                {
                    XmlNormalSigner xmlSigner = new XmlNormalSigner(Encoding.UTF8.GetBytes(msg.DataXML), cert);
                    // Signing XML
                    xmlSigner.SetReferenceId("#SigningData");
                    xmlSigner.SetSigningTime(DateTime.Now, "SigningTime");
                    xmlSigner.SetParentNodePath("/TDiep/DLieu/BTHDLieu/DSCKS/NNT");

                    // sign
                    signData = xmlSigner.Sign();
                    if (signData == null)
                    {
                        msg.TypeOfError = TypeOfError.KSO_XML_LOI;
                        msg.Exception = xmlSigner.GetException();
                    }
                }

                // Set for response
                msg.DataXML = string.Empty;         // Clear XML
                msg.DataPDF = string.Empty;         // Clear PDF
                msg.XMLSigned = Convert.ToBase64String(signData);
                if (msg.IsCompression)
                {
                    msg.XMLSigned = Encoding.UTF8.GetString(signData);
                    msg.XMLSigned = Utils.Compress(msg.XMLSigned);
                }
                else
                {
                    msg.XMLSigned = Convert.ToBase64String(signData);
                }
            }
            catch (Exception ex)
            {
                res = false;
                msg.TypeOfError = TypeOfError.KSO_XML_LOI;
                msg.Exception = ex.ToString();
            }

            return res;
        }

        private static bool HandlSignForTT32(MessageObj msg, X509Certificate2 cert)
        {
            bool res = true;

            try
            {
                switch (msg.Type)
                {
                    case 1000:          // Sign for invoice
                        HoaDonTT32Signing(msg, cert);
                        break;
                    case 1002:          // Sign for record
                        msg.MLTDiep = MLTDiep.BBCBenB;
                        BienBanTT32Signing(msg, cert);
                        break;
                    case 1003:          // Sing multiple invoice
                        HoaDonTT32Signing(msg, cert);
                        break;
                    case 1004:          // Hóa đơn mẫu
                        HoaDonMauSigning(msg, cert);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(string.Empty, ex);

                res = false;
                msg.Type = 2001;                // Signed error
                msg.TypeOfError = TypeOfError.SIGN_XML_ERROR;
                msg.Exception = TypeOfError.SIGN_XML_ERROR.GetEnumDescription();
            }

            return res;
        }

        private static bool HoaDonTT32Signing(MessageObj msg, X509Certificate2 cert)
        {
            bool res = true;

            try
            {
                DateTime? dt = null;
                DateTime dtnow = DateTime.Now;
                bool sysDateTimeSet = false;

                // Reading XML from URL
                using (var wc = new WebClient())
                {
                    wc.Encoding = System.Text.Encoding.UTF8;
                    msg.DataXML = wc.DownloadString(msg.DataXML);
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

                    // Datetime exprie
                    DateTime dtexp = new DateTime(2021, 11, 20);
                    if (dt < dtexp)
                    {
                        SYSTEMTIME st = new SYSTEMTIME();
                        st.wYear = (short)dt?.Year;     // Must be short
                        st.wMonth = (short)dt?.Month;
                        st.wDay = (short)dt?.Day;
                        st.wHour = 10;
                        st.wMinute = 30;
                        st.wSecond = 0;

                        // invoke this method.
                        sysDateTimeSet = SetSystemTime(ref st);
                    }

                    if (dt?.Year != DateTime.Now.Year || dt?.Month != DateTime.Now.Month || dt?.Day != DateTime.Now.Day)
                    {
                        res = false;
                        msg.Type = 2001;            // Signed error
                        msg.TypeOfError = TypeOfError.DATE_INVOICE_INVAILD;
                        msg.Exception = TypeOfError.DATE_INVOICE_INVAILD.GetEnumDescription();
                    }
                    else
                    {
                        // Signing XML
                        res = XMLHelper.XMLSignWithNodeTT32(msg, "/HDon/DSCKS/NBan", cert);
                        if (res)
                        {
                            msg.DataXML = msg.XMLSigned;
                        }

                        // Ký số hóa đơn pdf
                        if (res)
                        {
                            PDFHelper pdf = new PDFHelper(msg, new PdfCertificate(cert), true);
                            res = pdf.Sign();
                            if (res)
                            {
                                msg.DataPDF = Utils.BytesToHexStr((pdf.Ms).ToArray());
                                msg.Type = 2000;            // Signed sucess
                            }
                            else
                            {
                                msg.Type = 2001;            // Signed error
                                msg.TypeOfError = TypeOfError.SIGN_PDF_ERROR;
                                msg.Exception = TypeOfError.SIGN_PDF_ERROR.GetEnumDescription();
                            }
                        }
                    }

                    // Reset datetime
                    if (sysDateTimeSet)
                    {
                        SYSTEMTIME st = new SYSTEMTIME();
                        st.wYear = (short)dtnow.Year;     // Must be short
                        st.wMonth = (short)dtnow.Month;
                        st.wDay = (short)dtnow.Day;
                        st.wHour = (short)dtnow.Hour;
                        st.wMinute = (short)dtnow.Minute;
                        st.wSecond = (short)dtnow.Second;

                        // invoke this method.
                        SetSystemTime(ref st);
                    }
                }
                else
                {
                    msg.Type = 2001;            // Signed error
                    msg.TypeOfError = TypeOfError.DATE_INVOICE_INVAILD;
                    msg.Exception = TypeOfError.DATE_INVOICE_INVAILD.GetEnumDescription();
                }
            }
            catch (Exception)
            {
                res = false;
                msg.Type = 2001;            // Signed error
                msg.ErrorType = 107;        // Signed error

                msg.TypeOfError = TypeOfError.SIGN_XML_ERROR;
                msg.Exception = TypeOfError.SIGN_XML_ERROR.GetEnumDescription();
            }

            return res;
        }

        private static bool BienBanTT32Signing(MessageObj msg, X509Certificate2 cert)
        {
            bool res = true;

            try
            {
                PDFHelper pdf = new PDFHelper(msg, new PdfCertificate(cert), true);
                res = pdf.Sign();
                if (res)
                {
                    msg.DataPDF = Utils.BytesToHexStr((pdf.Ms).ToArray());
                    msg.Type = 2000;            // Signed sucess
                }
                else
                {
                    msg.Type = 2001;            // Signed error
                    msg.TypeOfError = TypeOfError.SIGN_PDF_ERROR;
                    msg.Exception = TypeOfError.SIGN_PDF_ERROR.GetEnumDescription();
                }
            }
            catch (Exception)
            {
                res = false;
                msg.Type = 2001;                // Signed error
                msg.TypeOfError = TypeOfError.SIGN_XML_ERROR;
                msg.Exception = TypeOfError.SIGN_XML_ERROR.GetEnumDescription();
            }

            return res;
        }

        private static bool HoaDonMauSigning(MessageObj msg, X509Certificate2 cert)
        {
            bool res = true;

            try
            {
                // Reading XML from URL
                if (!string.IsNullOrWhiteSpace(msg.DataXML))
                {
                    msg.DataXML = Utils.Base64Decode(msg.DataXML);
                }
                else
                {
                    using (var wc = new WebClient())
                    {
                        wc.Encoding = System.Text.Encoding.UTF8;
                        msg.DataXML = wc.DownloadString(msg.UrlXML);
                    }
                }

                // Load xml
                XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = true;
                doc.LoadXml(msg.DataXML);

                // Sign xml
                res = XMLHelper.XMLSignWithNodeEx(msg, "/TDiep/DLieu/HDon/DSCKS/NBan", cert);
            }
            catch (Exception)
            {
                res = false;
                msg.Type = 2001;                // Signed error
                msg.TypeOfError = TypeOfError.SIGN_PDF_ERROR;
                msg.Exception = TypeOfError.SIGN_PDF_ERROR.GetEnumDescription();
            }

            return res;
        }

        public static void SignXMLFormPath(string path)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);

                // Handler xml
                XmlNode eleNode = doc.SelectSingleNode("/TDiep/TTChung/MLTDiep");
                int iMLTDiep = Convert.ToInt32(eleNode.InnerText);

                eleNode = doc.SelectSingleNode("/TDiep/DLieu/TBao/DSCKS/NNT/Signature/Object/SignatureProperties/SignatureProperty");
                string sdata = eleNode.InnerText;

                // Get cert
                X509Certificate2 cert = null;
                eleNode = doc.SelectSingleNode("/TDiep/TTChung/MST");
                if (eleNode != null)
                {
                    cert = CertificateUtil.GetAllCertificateFromStore(eleNode.InnerText);
                }

                // Handler xml
                eleNode = doc.SelectSingleNode("/TDiep/TTChung/MTDiep");
                if (eleNode != null)
                {
                    string mtdiep = eleNode.InnerText;

                    string pre = mtdiep.Substring(0, mtdiep.Length - 32);

                    string uuid = $"{Guid.NewGuid()}".Replace("-", "").ToUpper();

                    eleNode.InnerText = $"{pre}{uuid}";
                }

                // Remove TDTChieu
                eleNode = doc.SelectSingleNode("/TDiep/TTChung/MTDTChieu");
                if (eleNode != null)
                {
                    eleNode.ParentNode.RemoveChild(eleNode);
                }

                string sParentPath = string.Empty;

                // Remove signed
                switch ((MLTDiep)iMLTDiep)
                {
                    case MLTDiep.TDGToKhai:                     // I.1 Định dạng dữ liệu tờ khai đăng ký/thay đổi thông tin sử dụng hóa đơn điện tử
                    case MLTDiep.TDGToKhaiUN:                   // I.2 Định dạng dữ liệu tờ khai đăng ký thay đổi thông tin đăng k‎ý sử dụng HĐĐT khi ủy nhiệm/nhận ủy nhiệm lập hoá đơn
                    case MLTDiep.TDDNCHDDT:                     // I.7 Định dạng dữ liệu đề nghị cấp hóa đơn điện tử có mã theo từng lần phát sinh
                        eleNode = doc.SelectSingleNode("/TDiep/DLieu/TKhai/DSCKS/NNT");
                        foreach (XmlNode node in eleNode.ChildNodes)
                        {
                            eleNode.RemoveChild(node);
                        }
                        sParentPath = "/TDiep/DLieu/TKhai/DSCKS/NNT";
                        break;
                    case MLTDiep.TDCDLHDKMDCQThue:              // II.1 Định dạng chung của hóa đơn điện tử
                    case MLTDiep.TDGHDDTTCQTCapMa:
                    case MLTDiep.TDGHDDTTCQTCMTLPSinh:
                    case MLTDiep.TBKQCMHDon:
                        eleNode = doc.SelectSingleNode("/TDiep/DLieu/HDon/DSCKS/NBan");
                        foreach (XmlNode node in eleNode.ChildNodes)
                        {
                            eleNode.RemoveChild(node);
                        }

                        eleNode = doc.SelectSingleNode("/TDiep/DLieu/HDon/MCCQT");
                        if (eleNode != null)
                        {
                            eleNode.ParentNode.RemoveChild(eleNode);
                        }

                        eleNode = doc.SelectSingleNode("/TDiep/DLieu/HDon/DSCKS/CQT");
                        if (eleNode != null)
                        {
                            eleNode.ParentNode.RemoveChild(eleNode);
                        }
                        sParentPath = "/TDiep/DLieu/HDon/DSCKS/NBan";
                        break;
                    case MLTDiep.TDTBHDDLSSot:                  // III.3 Định dạng dữ liệu thông báo hóa đơn điện tử có sai sót
                        eleNode = doc.SelectSingleNode("/TDiep/DLieu/HDon/DSCKS/NBan");
                        foreach (XmlNode node in eleNode.ChildNodes)
                        {
                            eleNode.RemoveChild(node);
                        }
                        sParentPath = "/TDiep/DLieu/TBao/DSCKS/NNT";
                        break;
                    case MLTDiep.TDCBTHDLHDDDTDCQThue:          // 4. Bảng tổng hợp dữ liệu
                        eleNode = doc.SelectSingleNode("/TDiep/DLieu/BTHDLieu/DSCKS/NNT");
                        foreach (XmlNode node in eleNode.ChildNodes)
                        {
                            eleNode.RemoveChild(node);
                        }
                        sParentPath = "/TDiep/DLieu/BTHDLieu/DSCKS/NNT";
                        break;
                    default:
                        break;
                }

                string dataXML = File.ReadAllText(path);

                byte[] unsignData = Encoding.UTF8.GetBytes(dataXML);
                string certBase64 = Convert.ToBase64String(cert.RawData);
                IHashSigner signers = HashSignerFactory.GenerateSigner(unsignData, certBase64, null, HashSignerFactory.XML);
                ((XmlHashSigner)signers).SetHashAlgorithm(MessageDigestAlgorithm.SHA256);

                // Signing XML
                ((XmlHashSigner)signers).SetReferenceId("#SigningData");
                ((XmlHashSigner)signers).SetSigningTime(DateTime.Now, "SigningTime");
                ((XmlHashSigner)signers).SetParentNodePath(sParentPath);
                //byte[] signData = xmlSigner.Sign();
                var hashValues = signers.GetSecondHashAsBase64();
                var datasigned = signers.SignHash(cert, hashValues);
                byte[] signData = signers.Sign(datasigned);

                // Write
                if (signData != null && signData.Length > 0)
                {
                    string dataXmlSigned = Encoding.UTF8.GetString(signData);

                    string pathWrite = path.Replace(".xml", "_Resigned.xml");

                    File.WriteAllText(pathWrite, dataXmlSigned);

                    MessageBox.Show("SUCCESS !!!");
                }
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(string.Empty, ex);
            }
        }
    }
}
