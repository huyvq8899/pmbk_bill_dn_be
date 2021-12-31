using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;

namespace BKSOFT_UTILITY
{
    public class XmlHashSigner
    {
        private string _referenceId = "";

        private string _parentNode = "";

        private string _nameSpace = "";

        private string _nameSpaceRef = "";

        private string _signId = "signId";

        private string _signTimeId = "AddSigningTime";

        private X509Certificate2 _signer;

        private DateTime _signingTime = DateTime.UtcNow;

        private bool _addSigningTime = false;

        private XmlDocument _doc;

        private byte[] _unsignData = null;

        private string _exception;

        public XmlHashSigner()
        {
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

        public void SetSignerCertificate(X509Certificate2 signer)
        {
            _signer = signer;
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

        public XmlHashSigner(byte[] unsignData, X509Certificate2 signer)
        {
            _signer = signer;
            _unsignData = unsignData;

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

            try
            {
                using (Stream input = new MemoryStream(_unsignData))
                {
                    XmlReader xmlReader = XmlReader.Create(input, settings);
                    _doc.Load(xmlReader);
                    xmlReader.Close();
                }
            }
            catch (Exception ex)
            {
                LogFile.WriteLog(string.Empty, ex);
            }
        }

        public string GetSingleNodeValue(string xpath)
        {
            string value = string.Empty;
            try
            {
                XmlNode elemList = _doc.SelectSingleNode(xpath);
                if (elemList != null)
                {
                    value = elemList.InnerText;
                }    
            }
            catch(Exception ex)
            {
                LogFile.WriteLog(string.Empty, ex);
            }

            return value;
        }

        public string GetException()
        {
            return _exception;
        }

        public byte[] Sign()
        {
            byte[] signData = null;

            try
            {
                // Signed xml
                SignedXml signedXml = new SignedXml(_doc);           // Full xml
                signedXml.SigningKey = _signer.PrivateKey;

                // Add an RSAKeyValue KeyInfo (optional; helps recipient find key to validate).
                KeyInfo keyInfo = new KeyInfo();
                KeyInfoX509Data clause = new KeyInfoX509Data();
                clause.AddSubjectName(_signer.Subject);
                clause.AddCertificate(_signer);
                //clause.CRL = cert.Extensions.
                keyInfo.AddClause(clause);
                signedXml.KeyInfo = keyInfo;

                // Add Object
                if (_addSigningTime)
                {
                    XmlDocument docObj = new XmlDocument();
                    docObj.LoadXml($"<SignatureProperties><SignatureProperty Target='#Signature'><SigningTime>{_signingTime.ToString("yyyy-MM-ddTHH:mm:ss")}</SigningTime></SignatureProperty></SignatureProperties>");
                    DataObject dataObject = new DataObject();
                    dataObject.Data = docObj.ChildNodes;
                    dataObject.Id = _signTimeId;
                    signedXml.AddObject(dataObject);
                }

                // Attach transforms SigningData
                var reference = new Reference();                
                if (!string.IsNullOrEmpty(_referenceId))
                {
                    reference.Uri = _referenceId;
                }
                reference.AddTransform(new XmlDsigEnvelopedSignatureTransform(includeComments: false));
                reference.AddTransform(new XmlDsigExcC14NTransform(includeComments: false));
                signedXml.AddReference(reference);

                // Attach transforms SigningTime
                if (_addSigningTime && !string.IsNullOrEmpty(_signTimeId))
                {
                    var reference2 = new Reference();
                    reference2.Uri = $"#{_signTimeId}";
                    reference2.AddTransform(new XmlDsigEnvelopedSignatureTransform(includeComments: false));
                    reference2.AddTransform(new XmlDsigExcC14NTransform(includeComments: false));
                    signedXml.AddReference(reference2);
                }

                // Compute signature
                signedXml.ComputeSignature();
                var signatureElement = signedXml.GetXml();

                // Add signature of seller
                XmlNodeList elemList = _doc.SelectNodes(_parentNode);
                if (elemList != null && elemList.Count == 1)
                {
                    elemList[0].AppendChild(_doc.ImportNode(signatureElement, true));

                    // XML signed
                    signData = Encoding.UTF8.GetBytes(_doc.OuterXml);
                }
                else if(elemList == null || elemList.Count == 0)
                {
                    var parentPath = _parentNode.Substring(0, _parentNode.LastIndexOf('/'));
                    var newNode = _parentNode.Substring(_parentNode.LastIndexOf('/') + 1);

                    // Get node parent
                    var parent = _doc.SelectSingleNode(parentPath);
                    if (parent != null)
                    {
                        parent.AppendChild(_doc.CreateElement(newNode));

                        XmlNodeList ele = _doc.SelectNodes(_parentNode);

                        // Add signed
                        if (ele != null && ele.Count == 1)
                        {
                            ele[0].AppendChild(_doc.ImportNode(signatureElement, true));

                            // XML signed
                            signData = Encoding.UTF8.GetBytes(_doc.OuterXml);
                        }
                    }
                }    
            }
            catch (Exception ex)
            {
                _exception = ex.ToString();

                LogFile.WriteLog(string.Empty, ex);
            }

            return signData;
        }
    }
}
