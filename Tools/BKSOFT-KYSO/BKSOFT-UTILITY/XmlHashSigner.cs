using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace BKSOFT.UTILITY
{
    public class XmlHashSigner : BaseHashSigner, IHashSigner
    {
        private string _referenceId = "";

        private string _parentNode = "";

        private string _nameSpace = "";

        private string _nameSpaceRef = "";

        private string _signId = "signId";

        private string _signTimeId = "AddSigningTime";

        private DateTime _signingTime = DateTime.UtcNow;

        private bool _addSigningTime = false;

        private XmlDocument _doc;

        public XmlHashSigner()
        {
        }

        public void SetUnsignData(string base64Data)
        {
            _unsignData = Convert.FromBase64String(base64Data);
        }

        public void SetSigningTime(DateTime time)
        {
            _signingTime = time;
            _addSigningTime = true;
        }

        public void SetSigningTime(DateTime time, string id)
        {
            _signingTime = time;
            _addSigningTime = true;
            _signTimeId = id;
        }

        public string SignBase64(string signedHashBase64)
        {
            byte[] array = Sign(signedHashBase64);
            if (array != null)
            {
                return Convert.ToBase64String(array);
            }
            Console.WriteLine("Error when package signed data");
            return null;
        }

        public XmlHashSigner(byte[] unsignData, string certBase64)
            : base(unsignData, certBase64)
        {
            Init();
        }

        private void Init()
        {
            _doc = new XmlDocument();
            _signId = Guid.NewGuid().ToString();
            XmlReaderSettings settings = new XmlReaderSettings
            {
                CloseInput = true,
                IgnoreComments = false,
                IgnoreWhitespace = false,
                IgnoreProcessingInstructions = false
            };
            using (Stream input = new MemoryStream(_unsignData))
            {
                try
                {
                    XmlReader xmlReader = XmlReader.Create(input, settings);
                    _doc.Load(xmlReader);
                    xmlReader.Close();
                }
                catch (Exception ex)
                {
                    LogFile.WriteLog(string.Empty, ex);
                }
            }
        }

        public bool CheckHashSignature(string signedHashBase64)
        {
            return true;
        }

        public string GetSecondHashAsBase64()
        {
            byte[] secondHashBytes = GetSecondHashBytes();
            if (secondHashBytes == null)
            {
                return null;
            }
            return Convert.ToBase64String(secondHashBytes);
        }

        public byte[] GetSecondHashBytes()
        {
            try
            {
                XmlNodeList elementsByTagName = _doc.GetElementsByTagName("Signature");
                XmlElement xmlElement = null;
                if (string.IsNullOrEmpty(_signId))
                {
                    xmlElement = (XmlElement)elementsByTagName[0];
                }
                else
                {
                    if (_signId[0] == '#')
                    {
                        _signId = _signId.Substring(1);
                    }
                    //xmlElement = (XmlElement)elementsByTagName.Cast<XmlNode>().SingleOrDefault((XmlNode node) => node.Attributes["id"]?.Value == _signId || node.Attributes["Id"]?.Value == _signId);
                    xmlElement = (XmlElement)Utils.SearchXmlNodeAttributes(elementsByTagName, _signId);
                }
                if (xmlElement != null)
                {
                    return null;
                }
                HashAlgorithm alg = new SHA1CryptoServiceProvider();
                string hashAlg = "sha1";
                switch (_hashAlgorithm)
                {
                    case MessageDigestAlgorithm.SHA256:
                        //alg = new SHA256CryptoServiceProvider();      // Net 4.6
                        hashAlg = "sha256";
                        break;
                    case MessageDigestAlgorithm.SHA384:
                        //alg = new SHA384CryptoServiceProvider();       // Net 4.6
                        hashAlg = "sha384";
                        break;
                    case MessageDigestAlgorithm.SHA512:
                        //alg = new SHA512CryptoServiceProvider();       // Net 4.6
                        hashAlg = "sha512";
                        break;
                }
                byte[] array = null;
                if (string.IsNullOrEmpty(_referenceId))
                {
                    array = DsigSignature.GetC14NDigest(_doc, alg);
                }
                else
                {
                    string arg = _referenceId;
                    if (_referenceId[0] == '#')
                    {
                        arg = _referenceId.Substring(1);
                    }
                    string xpath = $"//*[@Id='{arg}']";
                    XmlElement xmlElement2 = (XmlElement)_doc.SelectSingleNode(xpath);
                    if (xmlElement2 == null)
                    {
                        xpath = $"//*[@id='{arg}']";
                        xmlElement2 = (XmlElement)_doc.SelectSingleNode(xpath);
                    }
                    array = DsigSignature.GetC14NDigest(xmlElement2, _doc, alg);
                }
                string base64Digest = Convert.ToBase64String(array);
                X509Certificate2 x509Certificate = new X509Certificate2(Convert.FromBase64String(_signerCert));
                string text = "";
                text = x509Certificate.PublicKey.Key.ToXmlString(includePrivateParameters: false);
                string subjectDN = x509Certificate.SubjectName.Decode(X500DistinguishedNameFlags.None);
                XmlNode signature = DsigSignature.CreateSignature(_addSigningTime ? _signingTime : DateTime.MinValue, hashAlg, base64Digest, "", subjectDN, _signerCert, text, _signId, _referenceId, _signTimeId);
                DsigSignature.AddSignatureNode(_doc, signature, _parentNode, _nameSpace, _nameSpaceRef);
                return DsigSignature.GetHash(_doc, signature, alg);
            }
            catch (Exception ex)
            {
                LogFile.WriteLog(string.Empty, ex);

                throw ex;
            }
        }

        public byte[] Sign(string signedHashBase64)
        {
            DsigSignature.AddSignatureValue(_doc.DocumentElement, signedHashBase64, _signId);
            //XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
            //{
            //    Indent = true,
            //    IndentChars = "\t",
            //    NewLineChars = "\r\n",
            //    NewLineHandling = NewLineHandling.None,
            //    OmitXmlDeclaration = true,
            //    DoNotEscapeUriAttributes = false
            //};
            return Encoding.UTF8.GetBytes(_doc.OuterXml);
        }

        public string SignHash(X509Certificate2 cert, string hashValues)
        {
            string value = string.Empty;
            byte[] hash = Convert.FromBase64String(hashValues);

            // .NET 2.0 only SHA1
            RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)cert.PrivateKey;
            byte[] signature = rsa.SignHash(hash, CryptoConfig.MapNameToOID("SHA1"));
            value = Convert.ToBase64String(signature, 0, signature.Length);

            //// .NET 4.6
            //using (HashAlgorithm hasher = SHA256.Create())
            //using (RSA rsa = cert.GetRSAPrivateKey())
            //{
            //    byte[] signature = rsa.SignHash(hash, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
            //    value = Convert.ToBase64String(signature, 0, signature.Length);
            //}

            return value;
        }

        public void SetReferenceId(string id)
        {
            _referenceId = id;
        }

        public void SetParentNodePath(string node)
        {
            _parentNode = node;
        }

        public void SetNameSpace(string nameSpace, string reference)
        {
            _nameSpace = nameSpace;
            _nameSpaceRef = reference;
        }

        public void SetSignatureID(string value)
        {
            _signId = value;
        }

        public void SetHashAlgorithm(MessageDigestAlgorithm alg)
        {
            _hashAlgorithm = alg;
        }

        public string GetSignerCertBase64()
        {
            return _signerCert;
        }

        public bool SetSignerCertchain(string pkcs7Base64)
        {
            return false;
        }

        public string GetSingleNodeValue(string path)
        {
            string value = string.Empty;
            try
            {
                XmlNode eleNode = _doc.SelectSingleNode(path);
                if (eleNode != null)
                {
                    value = eleNode.InnerText;
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteLog(string.Empty, ex);
            }
            return value;
        }
    }
}