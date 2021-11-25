using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BKSOFT.EMU
{
    public class XMLHelper
    {
        public static void XMLSignFromFile(string path)
        {
            try
            {
                // Load XML
                string dataXML = File.ReadAllText(path);

                // Get X509 Certificate
                X509Certificate2 cert = GetAllCertificateFromStore();

                // Sign XML
                XMLSign(dataXML, "/TDiep/DLieu/TKhai/DSCKS/NNT", cert);
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(string.Empty, ex);
            }
        }


        public static bool XMLSign(string dataXML, string node, X509Certificate2 cert)
        {
            bool res = false;

            string xmlSigned = string.Empty;
            try
            {
                if (dataXML == null)
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
                doc.LoadXml(dataXML);

                // Signed xml
                SignedXml signedXml = new SignedXml(doc);           // Full xml
                signedXml.SigningKey = cert.PrivateKey;

                // Add an RSAKeyValue KeyInfo (optional; helps recipient find key to validate).
                KeyInfo keyInfo = new KeyInfo();
                KeyInfoX509Data clause = new KeyInfoX509Data();
                clause.AddSubjectName(cert.Subject);
                clause.AddCertificate(cert);
                keyInfo.AddClause(clause);
                signedXml.KeyInfo = keyInfo;

                // Add Object
                XmlDocument docObj = new XmlDocument();
                docObj.LoadXml($"<SignedProperties><SignedProperty><SigningTime>{(DateTime.Now).ToString("yyyy-MM-ddTHH:mm:ss")}</SigningTime></SignedProperty></SignedProperties>");
                DataObject dataObject = new DataObject();
                dataObject.Data = docObj.ChildNodes;
                dataObject.Id = "SigningTime";
                signedXml.AddObject(dataObject);

                // Attach transforms SigningData
                var reference = new Reference();
                reference.Uri = "#SigningData";
                reference.AddTransform(new XmlDsigEnvelopedSignatureTransform(includeComments: false));
                reference.AddTransform(new XmlDsigExcC14NTransform(includeComments: false));
                signedXml.AddReference(reference);

                // Attach transforms SigningTime
                var reference2 = new Reference();
                reference2.Uri = "#SigningTime";
                reference2.AddTransform(new XmlDsigEnvelopedSignatureTransform(includeComments: false));
                reference2.AddTransform(new XmlDsigExcC14NTransform(includeComments: false));
                signedXml.AddReference(reference2);

                // Compute signature
                signedXml.ComputeSignature();
                var signatureElement = signedXml.GetXml();

                // Add signature of seller
                XmlNodeList elemList = doc.SelectNodes(node);
                if (elemList != null && elemList.Count == 1)
                {
                    elemList[0].AppendChild(doc.ImportNode(signatureElement, true));

                    // XML signed
                    string strXMLSigned = doc.OuterXml;

                    File.WriteAllText(@"D:\SRC\02-Phan-Mem-Bach-Khoa\SRC\bill-back-end\Tools\BKSOFT-SEND-TVAN\BKSOFT.TCT.EMU\bin\Debug\XML\tmp.xml", strXMLSigned);

                    res = true;
                }
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(string.Empty, ex);
            }

            return res;
        }

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetForegroundWindow();

        public static X509Certificate2 GetAllCertificateFromStore()
        {
            X509Certificate2 cert = null;
            try
            {
                X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;

                //X509Certificate2Collection fcollection = (X509Certificate2Collection)collection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
                X509Certificate2Collection scollection = X509Certificate2UI.SelectFromCollection(collection, "Hóa đơn Bách Khoa", "Chọn chứng thư số", X509SelectionFlag.SingleSelection, GetForegroundWindow());

                foreach (X509Certificate2 x509 in scollection)
                {
                    cert = x509;
                    break;
                }

                store.Close();
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(string.Empty, ex);
            }

            return cert;
        }

    }
}
