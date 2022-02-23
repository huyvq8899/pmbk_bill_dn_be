using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace BKSOFT.UTILITY
{
    public class DsigSignature
    {
        public enum DsigSignatureMode
        {
            Client,
            Server
        }

        public static XmlNode CreateSignature(DateTime signTime, string hashAlg, string base64Digest, string base64SignatureValue, string subjectDN, string base64Cert, string rsaKeyValue, string signatureId = "sigid", string referenceUri = "", string signTimeId = "AddSigningTime")
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlNode xmlNode = xmlDocument.CreateElement("Signature");
            XmlAttribute xmlAttribute = xmlDocument.CreateAttribute("Id");
            xmlAttribute.Value = signatureId;
            xmlNode.Attributes.Append(xmlAttribute);
            XmlAttribute xmlAttribute2 = xmlDocument.CreateAttribute("xmlns");
            xmlAttribute2.Value = "http://www.w3.org/2000/09/xmldsig#";
            xmlNode.Attributes.Append(xmlAttribute2);
            XmlNode xmlNode2 = xmlNode.AppendChild(xmlDocument.CreateElement("SignedInfo"));
            XmlNode xmlNode3 = xmlNode2.AppendChild(xmlDocument.CreateElement("CanonicalizationMethod"));
            XmlAttribute xmlAttribute3 = xmlDocument.CreateAttribute("Algorithm");
            xmlAttribute3.Value = "http://www.w3.org/TR/2001/REC-xml-c14n-20010315";
            xmlNode3.Attributes.Append(xmlAttribute3);
            XmlNode xmlNode4 = xmlNode2.AppendChild(xmlDocument.CreateElement("SignatureMethod"));
            XmlAttribute xmlAttribute4 = xmlDocument.CreateAttribute("Algorithm");
            string value = null;
            string value2 = null;
            if (!(hashAlg == "sha1"))
            {
                if (hashAlg == "sha256")
                {
                    value = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";
                    value2 = "http://www.w3.org/2001/04/xmlenc#sha256";
                }
            }
            else
            {
                value2 = "http://www.w3.org/2000/09/xmldsig#sha1";
                value = "http://www.w3.org/2000/09/xmldsig#rsa-sha1";
            }
            xmlAttribute4.Value = value;
            xmlNode4.Attributes.Append(xmlAttribute4);
            XmlNode xmlNode5 = xmlNode2.AppendChild(xmlDocument.CreateElement("Reference"));
            XmlAttribute xmlAttribute5 = xmlDocument.CreateAttribute("URI");
            if (referenceUri == null)
            {
                referenceUri = "";
            }
            if (!string.IsNullOrEmpty(referenceUri) && !referenceUri.StartsWith("#"))
            {
                referenceUri = "#" + referenceUri;
            }
            xmlAttribute5.Value = referenceUri;
            xmlNode5.Attributes.Append(xmlAttribute5);
            XmlNode xmlNode6 = xmlNode5.AppendChild(xmlDocument.CreateElement("Transforms"));
            XmlNode xmlNode7 = xmlNode6.AppendChild(xmlDocument.CreateElement("Transform"));
            XmlAttribute xmlAttribute6 = xmlDocument.CreateAttribute("Algorithm");
            xmlAttribute6.Value = "http://www.w3.org/2000/09/xmldsig#enveloped-signature";
            xmlNode7.Attributes.Append(xmlAttribute6);
            XmlNode xmlNode8 = xmlNode5.AppendChild(xmlDocument.CreateElement("DigestMethod"));
            XmlAttribute xmlAttribute7 = xmlDocument.CreateAttribute("Algorithm");
            xmlAttribute7.Value = value2;
            xmlNode8.Attributes.Append(xmlAttribute7);
            XmlNode xmlNode9 = xmlNode5.AppendChild(xmlDocument.CreateElement("DigestValue"));
            xmlNode9.InnerText = base64Digest;
            XmlNode xmlNode10 = xmlNode.AppendChild(xmlDocument.CreateElement("SignatureValue"));
            xmlNode10.InnerText = ((base64SignatureValue == null) ? "" : base64SignatureValue);
            XmlNode xmlNode11 = xmlNode.AppendChild(xmlDocument.CreateElement("KeyInfo"));
            XmlNode xmlNode12 = xmlNode11.AppendChild(xmlDocument.CreateElement("KeyValue"));
            xmlNode12.InnerXml = rsaKeyValue;
            XmlNode xmlNode13 = xmlNode11.AppendChild(xmlDocument.CreateElement("X509Data"));
            XmlNode xmlNode14 = xmlNode13.AppendChild(xmlDocument.CreateElement("X509SubjectName"));
            xmlNode14.InnerText = subjectDN;
            XmlNode xmlNode15 = xmlNode13.AppendChild(xmlDocument.CreateElement("X509Certificate"));
            xmlNode15.InnerText = base64Cert.Replace("\r", "").Replace("\n", "");
            if (DateTime.MinValue != signTime)
            {
                xmlDocument.AppendChild(xmlNode);
                XmlDocument xmlDocument2 = CreateSigningTime(DateTime.Now, signTimeId, signatureId);
                byte[] inArray = PerformGetDigest(xmlDocument2.DocumentElement);
                XmlNode newChild = xmlDocument.ImportNode(xmlDocument2.DocumentElement, deep: true);
                xmlDocument.GetElementsByTagName("Signature")[0].AppendChild(newChild);
                XmlDocument xmlDocument3 = new XmlDocument();
                xmlDocument3.LoadXml("<Reference URI=\"#" + signTimeId + "\" ><DigestMethod Algorithm=\"http://www.w3.org/2001/04/xmlenc#sha256\"/><DigestValue>" + Convert.ToBase64String(inArray) + "</DigestValue></Reference>");
                XmlNode newChild2 = xmlDocument.ImportNode(xmlDocument3.DocumentElement, deep: true);
                xmlDocument.GetElementsByTagName("SignedInfo")[0].AppendChild(newChild2);
            }
            return xmlDocument.AppendChild(xmlNode);
        }

        private static XmlDocument CreateSigningTime(DateTime signDate, string id, string targetId)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml($"<Object Id=\"{id}\" xmlns=\"http://www.w3.org/2000/09/xmldsig#\"><SignatureProperties xmlns=\"\"><SignatureProperty Target=\"#{targetId}\"><SigningTime>{signDate:yyyy-MM-dd}T{DateTime.Now:HH:mm:ss}</SigningTime></SignatureProperty></SignatureProperties></Object>");
            return xmlDocument;
        }

        public static void AddSignatureValue(XmlElement signature, string base64Signed)
        {
            XmlNodeList elementsByTagName = signature.GetElementsByTagName("SignatureValue");
            if (elementsByTagName.Count != 1)
            {
                throw new Exception("SignatureValue tag invalid");
            }
            elementsByTagName[0].InnerText = base64Signed;
        }

        public static void AddSignatureValue(XmlElement doc, string base64Signed, string signatureId)
        {
            XmlNodeList elementsByTagName = doc.GetElementsByTagName("Signature");
            if (elementsByTagName.Count < 1)
            {
                throw new Exception("No signature tag found");
            }
            XmlElement xmlElement = null;
            if (string.IsNullOrEmpty(signatureId))
            {
                xmlElement = (XmlElement)elementsByTagName[0];
            }
            else
            {
                if (signatureId[0] == '#')
                {
                    signatureId = signatureId.Substring(1);
                }
                xmlElement = (XmlElement)elementsByTagName.Cast<XmlNode>().SingleOrDefault((XmlNode node) => node.Attributes["id"]?.Value == signatureId || node.Attributes["Id"]?.Value == signatureId);
            }
            XmlNodeList elementsByTagName2 = xmlElement.GetElementsByTagName("SignatureValue");
            if (elementsByTagName2.Count != 1)
            {
                throw new Exception("SignatureValue tag invalid");
            }
            elementsByTagName2[0].InnerText = base64Signed;
        }

        public static void AddSignatureNode(XmlDocument doc, XmlNode signature, string parentNodePath, string nameSpace, string nameSpaceRef)
        {
            XmlNode newChild = doc.ImportNode(signature, deep: true);
            if (string.IsNullOrEmpty(parentNodePath))
            {
                doc.DocumentElement.AppendChild(newChild);
                return;
            }
            XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(doc.NameTable);
            if (!string.IsNullOrEmpty(nameSpace) && !string.IsNullOrEmpty(nameSpaceRef))
            {
                xmlNamespaceManager.AddNamespace(nameSpace, nameSpaceRef);
            }
            XmlElement xmlElement = (XmlElement)doc.SelectSingleNode(parentNodePath, xmlNamespaceManager);
            if (xmlElement == null)
            {
                throw new Exception("No parent node in document. node name=" + parentNodePath);
            }
            xmlElement.AppendChild(newChild);
        }

        public static bool VerifySignature(string XmlSignedFilePath, string idSignature)
        {
            List<bool> list = new List<bool>();
            XmlDocument xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.Load(XmlSignedFilePath);
                SignedXmlCustom signedXmlCustom = new SignedXmlCustom(xmlDocument);
                XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("Signature");
                XmlElement xmlElement = null;
                if (string.IsNullOrEmpty(idSignature))
                {
                    xmlElement = (XmlElement)elementsByTagName[0];
                }
                else
                {
                    if (idSignature[0] == '#')
                    {
                        idSignature = idSignature.Substring(1);
                    }
                    xmlElement = (XmlElement)elementsByTagName.Cast<XmlNode>().SingleOrDefault((XmlNode node) => node.Attributes["id"].Value == idSignature);
                }
                signedXmlCustom.LoadXml((XmlElement)elementsByTagName[0]);
                return signedXmlCustom.CheckSignature();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static byte[] GetHash(XmlDocument xdoc, XmlNode signature, HashAlgorithm alg)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(signature.OuterXml);
            XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("SignedInfo");
            XmlDocument xmlDocument2 = new XmlDocument();
            xmlDocument2.LoadXml(elementsByTagName[0].OuterXml);
            CanonicalXmlNodeList propagatedAttributes = Utils.GetPropagatedAttributes(xdoc.DocumentElement);
            Utils.AddNamespaces(xmlDocument2.DocumentElement, propagatedAttributes);
            return GetC14NDigest(xmlDocument2, alg);
        }

        public static byte[] GetC14NDigest(XmlNode xn, XmlDocument doc, HashAlgorithm alg)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xn.OuterXml);
            CanonicalXmlNodeList propagatedAttributes = Utils.GetPropagatedAttributes(doc.DocumentElement);
            Utils.AddNamespaces(xmlDocument.DocumentElement, propagatedAttributes);
            return GetC14NDigest(xmlDocument, alg);
        }

        private static byte[] PerformGetDigest(XmlNode xn)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xn.OuterXml);
            XmlDsigC14NTransformCustom xmlDsigC14NTransformCustom = new XmlDsigC14NTransformCustom();
            xmlDsigC14NTransformCustom.LoadInput(xmlDocument);
            Stream inputStream = (Stream)xmlDsigC14NTransformCustom.GetOutput(typeof(Stream));
            SHA256 sHA = new SHA256CryptoServiceProvider();
            byte[] array = sHA.ComputeHash(inputStream);
            string text = Convert.ToBase64String(array);
            return array;
        }

        public static byte[] GetC14NDigest(XmlDocument xdoc, HashAlgorithm alg)
        {
            XmlDsigC14NTransformCustom xmlDsigC14NTransformCustom = new XmlDsigC14NTransformCustom();
            xmlDsigC14NTransformCustom.LoadInput(xdoc);
            return xmlDsigC14NTransformCustom.GetDigestedOutput(alg);
        }
    }
}