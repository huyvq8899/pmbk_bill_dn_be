using BKSOFT_KYSO.Modal;
using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;

namespace BKSOFT_KYSO
{
    public class XMLStatus
    {
        public int Status { set; get; }

        public string XMLSigned { set; get; }

        public string Exception { set; get; }
    }


    // This code contains parts of the code found at
    // http://www.wiktorzychla.com/2012/12/interoperable-xml-digital-signatures-c_20.html

    public class XMLHelper
    {
        public static bool VerifySignature(string xml)
        {
            if (xml == null) throw new ArgumentNullException("xml");

            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            doc.LoadXml(xml);

            // If there's no signature => return that we are "valid"
            XmlNode signatureNode = FindSignatureElement(doc);
            if (signatureNode == null) return true;

            SignedXml signedXml = new SignedXml(doc);
            signedXml.LoadXml((XmlElement)signatureNode);

            //var x509Certificates = signedXml.KeyInfo.OfType<KeyInfoX509Data>();
            //var certificate = x509Certificates.SelectMany(cert => cert.Certificates.Cast<X509Certificate2>()).FirstOrDefault();

            //if (certificate == null) throw new InvalidOperationException("Signature does not contain a X509 certificate public key to verify the signature");
            //return signedXml.CheckSignature(certificate, true);

            return signedXml.CheckSignature();
        }

        /// <summary>
        /// Ký số XML
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="node"></param>
        /// <param name="certificate"></param>
        /// <returns></returns>
        public static XMLStatus XMLSignWithNode(string xml, string node, X509Certificate2 certificate)
        {
            XMLStatus status = new XMLStatus
            {
                Status = 0,
                XMLSigned = string.Empty,
                Exception = string.Empty
            };

            string xmlSigned = string.Empty;
            try
            {
                if (xml == null)
                {
                    return status;
                }                    
                else if (certificate == null)
                {
                    return status;
                }                    
                else if (!certificate.HasPrivateKey)
                {
                    return status;
                }                    

                XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = true;
                doc.LoadXml(xml);

                // Signed xml
                SignedXml signedXml = new SignedXml(doc);           // Full xml
                signedXml.SigningKey = certificate.PrivateKey;

                // Attach certificate KeyInfo
                KeyInfoX509Data keyInfoData = new KeyInfoX509Data(certificate);
                KeyInfo keyInfo = new KeyInfo();
                keyInfo.AddClause(keyInfoData);
                signedXml.KeyInfo = keyInfo;

                // Attach transforms
                var reference = new Reference("");
                //reference.Uri = "#SignatureProperty";
                reference.AddTransform(new XmlDsigEnvelopedSignatureTransform(includeComments: false));
                reference.AddTransform(new XmlDsigExcC14NTransform(includeComments: false));
                signedXml.AddReference(reference);

                // Compute signature
                signedXml.ComputeSignature();
                var signatureElement = signedXml.GetXml();

                // Add signature of seller
                XmlNodeList elemList = doc.SelectNodes(node);
                if (elemList != null && elemList.Count == 1)
                {
                    elemList[0].AppendChild(doc.ImportNode(signatureElement, true));

                    // XML signed
                    status.Status = 1;
                    status.XMLSigned = doc.OuterXml;
                }
            }
            catch (Exception ex)
            {
                status.Exception = ex.ToString();

                FileLog.WriteLog(string.Empty, ex);
            }

            return status;
        }

        public static XMLStatus XMLSignWithNode2(string xml, string node, X509Certificate2 certificate)
        {
            XMLStatus status = new XMLStatus
            {
                Status = 0,
                XMLSigned = string.Empty,
                Exception = string.Empty
            };

            string xmlSigned = string.Empty;
            try
            {
                if (xml == null)
                {
                    return status;
                }
                else if (certificate == null)
                {
                    return status;
                }
                else if (!certificate.HasPrivateKey)
                {
                    return status;
                }

                XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = true;
                doc.LoadXml(xml);

                // Signed xml
                SignedXml signedXml = new SignedXml(doc);           // Full xml
                signedXml.SigningKey = certificate.PrivateKey;

                // Attach certificate KeyInfo
                KeyInfoX509Data keyInfoData = new KeyInfoX509Data(certificate);
                KeyInfo keyInfo = new KeyInfo();
                keyInfo.AddClause(keyInfoData);
                signedXml.KeyInfo = keyInfo;

                // Attach transforms
                var reference = new Reference("");
                //reference.Uri = "#SignatureProperty";
                reference.AddTransform(new XmlDsigEnvelopedSignatureTransform(includeComments: false));
                reference.AddTransform(new XmlDsigExcC14NTransform(includeComments: false));
                signedXml.AddReference(reference);

                // Compute signature
                signedXml.ComputeSignature();
                var signatureElement = signedXml.GetXml();

                // Add signature of seller
                XmlNodeList elemList = doc.SelectNodes(node);
                if (elemList != null && elemList.Count == 1)
                {
                    //// Add SigningTime
                    //XmlDocumentFragment xfrag = xdoc.CreateDocumentFragment();
                    //xfrag.InnerXml = @"<Demographic><Age/><DOB/></Demographic>";

                    elemList[0].AppendChild(doc.ImportNode(signatureElement, true));

                    // XML signed
                    status.Status = 1;
                    status.XMLSigned = doc.OuterXml;
                }
            }
            catch (Exception ex)
            {
                status.Exception = ex.ToString();

                FileLog.WriteLog(string.Empty, ex);
            }

            return status;
        }

        public static bool XMLSignWithNode3(MessageObj msg, string node, X509Certificate2 cert)
        {
            bool res = false;

            string xmlSigned = string.Empty;
            try
            {
                if (msg.DataXML == null)
                {
                    return res;
                }
                else if (cert == null)
                {
                    return res;
                }
                else if (!cert.HasPrivateKey)
                {
                    return res;
                }

                XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = true;
                doc.LoadXml(msg.DataXML);

                // Signed xml
                SignedXml signedXml = new SignedXml(doc);           // Full xml
                signedXml.SigningKey = cert.PrivateKey;

                //// Attach certificate KeyInfo
                //KeyInfoX509Data keyInfoData = new KeyInfoX509Data(certificate);
                //KeyInfo keyInfo = new KeyInfo();
                //keyInfo.AddClause(keyInfoData);
                //signedXml.KeyInfo = keyInfo;

                // Add an RSAKeyValue KeyInfo (optional; helps recipient find key to validate).
                KeyInfo keyInfo = new KeyInfo();
                KeyInfoX509Data clause = new KeyInfoX509Data();
                clause.AddSubjectName(cert.Subject);
                clause.AddCertificate(cert);
                keyInfo.AddClause(clause);
                signedXml.KeyInfo = keyInfo;

                // Attach transforms
                var reference = new Reference("");
                //reference.Uri = "#SignatureProperty";
                reference.AddTransform(new XmlDsigEnvelopedSignatureTransform(includeComments: false));
                reference.AddTransform(new XmlDsigExcC14NTransform(includeComments: false));
                signedXml.AddReference(reference);

                // Add Object
                XmlDocument docObj = new XmlDocument();
                docObj.LoadXml($"<Object><SignedProperties><SignedProperty><SigningTime>{(DateTime.Now).ToString("yyyy-MM-ddTHH:mm:ss")}</SigningTime></SignedProperty></SignedProperties></Object>");
                DataObject obj = new DataObject();
                obj.LoadXml(docObj.DocumentElement);
                signedXml.AddObject(obj);

                // Compute signature
                signedXml.ComputeSignature();
                var signatureElement = signedXml.GetXml();

                // Add signature of seller
                XmlNodeList elemList = doc.SelectNodes(node);
                if (elemList != null && elemList.Count == 1)
                {
                    elemList[0].AppendChild(doc.ImportNode(signatureElement, true));

                    // XML signed
                    msg.XMLSigned = Utils.Base64Encode(doc.OuterXml);
                    res = true;
                }
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(string.Empty, ex);
            }

            return res;
        }


        /// <summary>
        /// Ký số XML
        /// </summary>
        /// <param name="url"></param>
        /// <param name="node"></param>
        /// <param name="certificate"></param>
        /// <returns></returns>
        public static XMLStatus SignXMLFromURL(string url, string node, X509Certificate2 certificate)
        {
            XMLStatus status = new XMLStatus
            {
                Status = 0,
                XMLSigned = string.Empty,
                Exception = string.Empty
            };

            try
            {
                string strXML = string.Empty;
                using (var wc = new WebClient())
                {
                    wc.Encoding = System.Text.Encoding.UTF8;
                    strXML = wc.DownloadString(url);
                }

                // Sign
                status = XMLSignWithNode(strXML, node, certificate);
            }
            catch (Exception ex)
            {
                status.Exception = ex.ToString();

                FileLog.WriteLog(string.Empty, ex);
            }

            return status;
        }

        /// <summary>
        /// Check node element
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private static XmlNode FindSignatureElement(XmlDocument doc)
        {
            var signatureElements = doc.DocumentElement.GetElementsByTagName("Signature", "http://www.w3.org/2000/09/xmldsig#");
            if (signatureElements.Count == 1)
            {
                return signatureElements[0];
            }
            else if (signatureElements.Count == 0)
            {
                return null;
            }
            else
            {
                throw new InvalidOperationException("Document has multiple xmldsig Signature elements");
            }
        }

        //public static bool IsSigned(string xml)
        //{
        //    if (xml == null) throw new ArgumentNullException("xml");

        //    // First, a quick check, before reading the full document
        //    if (!xml.Contains("Signature")) return false;

        //    var doc = new XmlDocument();
        //    doc.LoadXml(xml);
        //    return FindSignatureElement(doc) != null;
        //}

        //public static DateTime? GetDateInvoice(string url)
        //{
        //    DateTime? dt = null;
        //    try
        //    {
        //        // Get xml from url
        //        string xml = string.Empty;
        //        using (var wc = new WebClient())
        //        {
        //            wc.Encoding = System.Text.Encoding.UTF8;
        //            xml = wc.DownloadString(url);
        //        }

        //        // Load xml
        //        XmlDocument doc = new XmlDocument();
        //        doc.PreserveWhitespace = true;
        //        doc.LoadXml(xml);

        //        // Get Date of seller
        //        XmlNode elemList = doc.SelectSingleNode("/HDon/DLHDon/TTChung/NLap");
        //        if (elemList != null )
        //        {
        //            dt = DateTime.ParseExact(elemList.InnerText, "yyyy-MM-dd", null);
        //        }  

        //    }
        //    catch (Exception)
        //    {
        //        dt = null;
        //    }

        //    return dt;
        //}


        //public static FeedBackFunction SellerSign(string xml, X509Certificate2 certificate)
        //{
        //    string xmlSigned = string.Empty;
        //    FeedBackFunction feedBackFunction = new FeedBackFunction();
        //    try
        //    {
        //        if (xml == null) throw new ArgumentNullException("xml");
        //        if (certificate == null) throw new ArgumentNullException("certificate");
        //        if (!certificate.HasPrivateKey) throw new ArgumentException("certificate", "Certificate should have a private key");

        //        XmlDocument doc = new XmlDocument();

        //        doc.PreserveWhitespace = true;
        //        doc.LoadXml(xml);

        //        SignedXml signedXml = new SignedXml(doc);
        //        signedXml.SigningKey = certificate.PrivateKey;

        //        // Attach certificate KeyInfo
        //        KeyInfoX509Data keyInfoData = new KeyInfoX509Data(certificate);
        //        KeyInfo keyInfo = new KeyInfo();
        //        keyInfo.AddClause(keyInfoData);
        //        signedXml.KeyInfo = keyInfo;

        //        //// Add object
        //        //XmlDocument objdocument = new XmlDocument();
        //        //objdocument.LoadXml(string.Format(Constants.OBJECT_DATA, DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")));
        //        //DataObject dataObject = new DataObject();
        //        //dataObject.Data = objdocument.ChildNodes;
        //        //dataObject.Id = "SignatureProperty";
        //        //signedXml.AddObject(dataObject);

        //        // Attach transforms
        //        var reference = new Reference("");
        //        //reference.Uri = "#SignatureProperty";
        //        reference.AddTransform(new XmlDsigEnvelopedSignatureTransform(includeComments: false));
        //        reference.AddTransform(new XmlDsigExcC14NTransform(includeComments: false));
        //        signedXml.AddReference(reference);

        //        // Compute signature
        //        signedXml.ComputeSignature();
        //        var signatureElement = signedXml.GetXml();

        //        //// Add signature to bundle
        //        //doc.DocumentElement.AppendChild(doc.ImportNode(signatureElement, true));

        //        // Add signature of seller
        //        XmlNodeList elemList = doc.SelectNodes("/HDon/DSCKS/NBan");
        //        if (elemList != null && elemList.Count == 1)
        //        {
        //            elemList[0].AppendChild(doc.ImportNode(signatureElement, true));
        //        }

        //        xmlSigned = doc.OuterXml;
        //        feedBackFunction.Status = Status.Success;
        //        feedBackFunction.Content = xmlSigned;
        //    }
        //    catch (Exception ex)
        //    {
        //        FileLog.WriteLog(string.Empty, ex);
        //        feedBackFunction.Status = Status.Erro;
        //        feedBackFunction.Content = ex.Message;
        //    }

        //    return feedBackFunction;
        //}
    }
}