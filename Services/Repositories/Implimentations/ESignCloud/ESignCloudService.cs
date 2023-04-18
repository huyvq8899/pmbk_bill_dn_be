using Microsoft.Extensions.Configuration;
using DLL;
using DLL.Constants;
using DLL.Entity;
using DLL.Entity.QuyDinhKyThuat;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml;
using Services.Helper;
using Services.Helper.Constants;
using Services.Repositories.Interfaces.ESignCloud;
using Services.ViewModels.XML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Services.Repositories.Implimentations.ESignCloud
{
    public class ESignCloudService : IESignCloudService
    {
        private readonly Datacontext _dataContext;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _IHttpContextAccessor;
        private readonly IConfiguration iConfiguration;

        public ESignCloudService(Datacontext dataContext,
            IHostingEnvironment IHostingEnvironment,
            IHttpContextAccessor IHttpContextAccessor,
            IConfiguration IConfiguration
            )
        {
            _dataContext = dataContext;
            _hostingEnvironment = IHostingEnvironment;
            _IHttpContextAccessor = IHttpContextAccessor;
            iConfiguration = IConfiguration;
        }
        public string FUNCTION_PREPAREFILEFORSIGNCLOUD = "prepareFileForSignCloud";
        public string FUNCTION_GETCERTIFICATEDETAILFORSIGNCLOUD = "getCertificateDetailForSignCloud";
        public string FUNCTION_CHANGEPASSCODEFORSIGNCLOUD = "changePasscodeForSignCloud";
        public string FUNCTION_FORGETPASSCODEFORSIGNCLOUD = "forgetPasscodeForSignCloud";

        //public string CERTIFICATEPROFILE = "PERS.1D";
        //public string FILE_DIRECTORY = "file\\";

        public async Task<ChungThuSoSuDung> GetInfoSignCloud(string agreementUUID)
        {
            string REST_URL = iConfiguration["IcorpAccount:Url"];
            string relyingParty = iConfiguration["IcorpAccount:relyingParty"];
            string relyingPartyUser = iConfiguration["IcorpAccount:relyingPartyUser"];
            string relyingPartyPassword = iConfiguration["IcorpAccount:relyingPartyPassword"];
            string relyingPartySignature = iConfiguration["IcorpAccount:relyingPartySignature"];
            string relyingPartyKeyStore = iConfiguration["IcorpAccount:relyingPartyKeyStore"];
            string relyingPartyKeyStorePassword = iConfiguration["IcorpAccount:relyingPartyKeyStorePassword"];
            bool flag = false;
            relyingPartyKeyStore = Path.Combine(_hostingEnvironment.WebRootPath, relyingPartyKeyStore);//"file\\rssp.p12";
            string timestamp = Utils.CurrentTimeMillis().ToString();
            string data2sign = relyingPartyUser + relyingPartyPassword + relyingPartySignature + timestamp;
            string pkcs1Signature = Utils.getPKCS1Signature(data2sign, relyingPartyKeyStore, relyingPartyKeyStorePassword);

            SignCloudReq signCloudReq = new SignCloudReq();
            signCloudReq.relyingParty = relyingParty;
            signCloudReq.agreementUUID = agreementUUID;

            CredentialData credentialData = new CredentialData();
            credentialData.username = relyingPartyUser;
            credentialData.password = relyingPartyPassword;
            credentialData.timestamp = timestamp;
            credentialData.signature = relyingPartySignature;
            credentialData.pkcs1Signature = pkcs1Signature;
            signCloudReq.credentialData = credentialData;

            string jsonReq = JsonConvert.SerializeObject(signCloudReq, Newtonsoft.Json.Formatting.Indented);
            var signCloudResp = SendPost(REST_URL + FUNCTION_GETCERTIFICATEDETAILFORSIGNCLOUD, jsonReq);
            var cts = new ChungThuSoSuDung();

            if (signCloudResp.responseCode == 0)
            {
                cts.Id = Guid.NewGuid().ToString();
                cts.Seri = signCloudResp.certificateSerialNumber;
                cts.TTChuc = signCloudResp.issuerDN;
                cts.CertificateDN = signCloudResp.certificateDN;
                cts.HThuc = 1;
                cts.TypeSign = 1;
                cts.IsAddInTTNNT = true;
                cts.UserkeySign = agreementUUID;
                cts.TNgay = DateTimeOffset.FromUnixTimeMilliseconds(signCloudResp.validFrom).DateTime.ToString("s");
                cts.DNgay = DateTimeOffset.FromUnixTimeMilliseconds(signCloudResp.validTo).DateTime.ToString("s");
                //var item = await _dataContext.ChungThuSoSuDungs.Where(x => x.Seri == signCloudResp.certificateSerialNumber && x.TypeSign == 1).FirstOrDefaultAsync();
                //if (item == null)
                //{
                //    //insert
                //    var cts = new ChungThuSoSuDung();

                //    cts.Id = Guid.NewGuid().ToString();
                //    cts.Seri = signCloudResp.certificateSerialNumber;
                //    cts.TTChuc = signCloudResp.issuerDN;
                //    cts.HThuc = 1;
                //    cts.TypeSign = 1;
                //    cts.IsAddInTTNNT = true;
                //    cts.UserkeySign = agreementUUID;
                //    cts.TNgay = DateTimeOffset.FromUnixTimeMilliseconds(signCloudResp.validFrom).DateTime.ToString("s");
                //    cts.DNgay = DateTimeOffset.FromUnixTimeMilliseconds(signCloudResp.validTo).DateTime.ToString("s");

                //    await _dataContext.ChungThuSoSuDungs.AddAsync(cts);
                //    flag = await _dataContext.SaveChangesAsync() > 0;
                //}
                //else
                //{
                //    //update
                //    //item.Seri = signCloudResp.certificateSerialNumber;
                //    item.UserkeySign = agreementUUID;
                //    item.TTChuc = signCloudResp.issuerDN;
                //    item.HThuc = 1;
                //    item.TypeSign = 1;
                //    item.IsAddInTTNNT = true;
                //    item.TNgay = DateTimeOffset.FromUnixTimeMilliseconds(signCloudResp.validFrom).DateTime.ToString("s");
                //    item.DNgay = DateTimeOffset.FromUnixTimeMilliseconds(signCloudResp.validTo).DateTime.ToString("s");

                //    _dataContext.ChungThuSoSuDungs.Update(item);
                //    flag = await _dataContext.SaveChangesAsync() > 0;
                //}
            }
            else
            {
                throw new Exception("Error while calling changePasscodeForSignCloud");
            }
            return cts;
        }
        public async Task<MessageObj> SignCloudFile(MessageObj dataJson)
        {
            if (dataJson == null) return null;

            string SIGNATURELOCATION = dataJson.IsNMua ? "DSCKS/NMua" : "DSCKS/NBan";// Để mặc định Hóa đơn điện tử
            string mimeTYPE = ESignCloudConstant.MIMETYPE_XML;
            string databaseName = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.DATABASE_NAME)?.Value;

            string fullFolder = string.Empty;
            string fileName = string.Empty;
            var unsignData = new byte[0];
            //Check ký pdf hay xml
            if (!string.IsNullOrEmpty(dataJson.DataXML))
            {
                string xML;
                if (dataJson.MLTDiep != MLTDiep.TDTBHDDLSSot && dataJson.MLTDiep != MLTDiep.TDGToKhai && dataJson.MLTDiep != MLTDiep.TDGToKhaiUN && dataJson.IsSignBKH != true)
                    xML = TextHelper.Decompress(dataJson.DataXML);
                else xML = TextHelper.Base64Decode(dataJson.DataXML);

                // Load xml
                XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = true;
                doc.LoadXml(xML);

                unsignData = Encoding.UTF8.GetBytes(doc.OuterXml);
                if (dataJson.MLTDiep != MLTDiep.TDCDLHDKMDCQThue)
                {
                    XmlNode mTDiep = doc.SelectSingleNode("/TDiep/TTChung/MTDiep");
                    XmlNode mLTDiep = doc.SelectSingleNode("/TDiep/TTChung/MLTDiep");
                    switch (Int32.Parse(mLTDiep.InnerText))
                    {
                        case 100:
                        case 300:
                        case 303:
                        case 400:
                            SIGNATURELOCATION = "DSCKS/NNT";
                            break;
                        case 206:
                            SIGNATURELOCATION = "CKSNNT";
                            break;
                    }
                    fileName = string.Format("{0}-{1}.xml", mLTDiep.InnerText, mTDiep.InnerText);
                    fullFolder = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/{databaseName}/SignCloud/{ManageFolderPath.XML_SIGNED}");
                }
                //else if (dataJson.MLTDiep == MLTDiep.TDCDLHDKMDCQThue)
                //{
                //    return dataJson;
                //}


            }
            else if (!string.IsNullOrEmpty(dataJson.UrlPDF))
            {
                mimeTYPE = ESignCloudConstant.MIMETYPE_PDF;

                fullFolder = Path.Combine(_hostingEnvironment.WebRootPath, $"FilesUpload/{databaseName}/{ManageFolderPath.PDF_SIGNED}");
                fileName = Path.GetFileName(dataJson.UrlPDF);
                if (!string.IsNullOrEmpty(dataJson.UrlPDF))
                {
                    using (WebClient webClient = new WebClient())
                    {
                        unsignData = webClient.DownloadData(dataJson.UrlPDF);

                    }
                }
                else if (!string.IsNullOrEmpty(dataJson.DataPDF))
                {
                    unsignData = Convert.FromBase64String(dataJson.DataPDF);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

            string fullFilePath = Path.Combine(fullFolder, fileName);
            #region create folder
            if (!Directory.Exists(fullFolder) && !string.IsNullOrEmpty(fullFilePath))
            {
                Directory.CreateDirectory(fullFolder);
            }
            else
            {
                if (!string.IsNullOrEmpty(fullFilePath))
                {
                    if (File.Exists(fullFilePath))
                    {
                        File.Delete(fullFilePath);
                    }
                }
            }
            #endregion

            SignCloudMetaData signCloudMetaData_case2 = new SignCloudMetaData();

            Dictionary<string, string> singletonSigning_c2 = new Dictionary<string, string>();
            singletonSigning_c2["NODETOBESIGNED"] = "SigningData";
            singletonSigning_c2["SIGNATUREFORMAT"] = "TAX-211120";
            singletonSigning_c2["SIGNATURELOCATION"] = SIGNATURELOCATION;
            singletonSigning_c2["DATETIMEFORMAT"] = "yyyy-MM-dd HH:mm:ss";
            signCloudMetaData_case2.singletonSigning = singletonSigning_c2;
            //string passCode = "12345678";

            var rs = await SignFileForSignCloud(dataJson, dataJson.UserkeySign, signCloudMetaData_case2, dataJson.PassCode, mimeTYPE, fileName, unsignData, fullFilePath);


            return rs;
        }
        public async Task<MessageObj> SignFileForSignCloud(MessageObj msg,
            string agreementUUID,
            SignCloudMetaData signCloudMetaData,
            string passCode,
            string mimeType,
            string fileName,
            byte[] fileData, string fullXmlFilePath)
        {
            string REST_URL = iConfiguration["IcorpAccount:Url"];
            string relyingParty = iConfiguration["IcorpAccount:relyingParty"];
            string relyingPartyUser = iConfiguration["IcorpAccount:relyingPartyUser"];
            string relyingPartyPassword = iConfiguration["IcorpAccount:relyingPartyPassword"];
            string relyingPartySignature = iConfiguration["IcorpAccount:relyingPartySignature"];
            string relyingPartyKeyStore = iConfiguration["IcorpAccount:relyingPartyKeyStore"];
            string relyingPartyKeyStorePassword = iConfiguration["IcorpAccount:relyingPartyKeyStorePassword"];

            relyingPartyKeyStore = Path.Combine(_hostingEnvironment.WebRootPath, relyingPartyKeyStore);//"file\\rssp.p12";
            string timestamp = Utils.CurrentTimeMillis().ToString();
            string data2sign = relyingPartyUser + relyingPartyPassword + relyingPartySignature + timestamp;
            string pkcs1Signature = Utils.getPKCS1Signature(data2sign, relyingPartyKeyStore, relyingPartyKeyStorePassword);
            SignCloudReq signCloudReq = new SignCloudReq();
            signCloudReq.relyingParty = relyingParty;
            signCloudReq.agreementUUID = agreementUUID;
            signCloudReq.authorizeMethod = ESignCloudConstant.AUTHORISATION_METHOD_PASSCODE;
            signCloudReq.authorizeCode = passCode;
            signCloudReq.messagingMode = ESignCloudConstant.SYNCHRONOUS;
            signCloudReq.certificateRequired = true;
            if (mimeType == ESignCloudConstant.MIMETYPE_PDF)
            {
                signCloudReq.signingFileData = fileData;
            }
            else if (mimeType == ESignCloudConstant.MIMETYPE_XML)
            {
                char[] charXml = Encoding.UTF8.GetString(fileData).ToCharArray();
                string stringXML = new string(charXml);
                signCloudReq.xmlDocument = TextHelper.StripXmlWhitespace(stringXML);
            }
            signCloudReq.mimeType = mimeType;
            signCloudReq.signingFileName = fileName;
            signCloudReq.signCloudMetaData = signCloudMetaData;

            var credentialData = new CredentialData();
            credentialData.username = relyingPartyUser;
            credentialData.password = relyingPartyPassword;
            credentialData.timestamp = timestamp;
            credentialData.signature = relyingPartySignature;
            credentialData.pkcs1Signature = pkcs1Signature;
            signCloudReq.credentialData = credentialData;

            //var javaScriptSerializer = new JsonSerializer();
            //javaScriptSerializer.MaxJsonLength = Int32.MaxValue;
            //javaScriptSerializer.Serialize(signCloudReq);
            string jsonReq = JsonConvert.SerializeObject(signCloudReq, Newtonsoft.Json.Formatting.Indented);
            var signCloudResp = SendPost(REST_URL + FUNCTION_PREPAREFILEFORSIGNCLOUD, jsonReq);


            if (signCloudResp.responseCode == 0 || signCloudResp.responseCode == 1018)
            {
                if (signCloudResp.signedFileData != null)
                {
                    var signCloudGetInfo = SendPost(REST_URL + FUNCTION_GETCERTIFICATEDETAILFORSIGNCLOUD, jsonReq);
                    if (signCloudGetInfo.responseCode == 0)
                    {
                        msg.SerialSigned = signCloudGetInfo.certificateSerialNumber;
                    }
                    char[] charFileData = Encoding.UTF8.GetString(signCloudResp.signedFileData).ToCharArray();
                    string stringFileSigned = new string(charFileData);
                    var base64String = string.Empty;
                    if (msg.MLTDiep != MLTDiep.TDTBHDDLSSot && msg.MLTDiep != MLTDiep.TDGToKhai && msg.MLTDiep != MLTDiep.TDGToKhaiUN && msg.MLTDiep != MLTDiep.TDCDLHDKMDCQThue)
                    {
                        base64String = TextHelper.Compress(stringFileSigned);
                    }
                    else
                    {
                        base64String = TextHelper.Base64Encode(stringFileSigned);
                    }
                    if (signCloudResp.mimeType == ESignCloudConstant.MIMETYPE_XML)
                    {
                        msg.XMLSigned = base64String;
                        msg.TypeOfError = TypeOfError.NONE;
                        if (!string.IsNullOrEmpty(fullXmlFilePath))
                        {
                            File.WriteAllBytes(fullXmlFilePath, signCloudResp.signedFileData);
                        }
                    }
                    else if (signCloudResp.mimeType == ESignCloudConstant.MIMETYPE_PDF)
                    {
                        msg.DataPDF = Convert.ToBase64String(signCloudResp.signedFileData, 0, signCloudResp.signedFileData.Length);
                        msg.PDFSigned = fullXmlFilePath;
                        msg.TypeOfError = TypeOfError.NONE;
                        //File.WriteAllBytes(fullXmlFilePath, signCloudResp.signedFileData);
                    }
                }
                else
                {
                    msg.TypeOfError = signCloudResp.mimeType == ESignCloudConstant.MIMETYPE_XML ? TypeOfError.SIGN_XML_ERROR : TypeOfError.SIGN_PDF_ERROR;
                    msg.Exception = string.Format("{0} - {1}", signCloudResp.responseCode, signCloudResp.responseMessage);
                    Tracert.WriteLog(signCloudResp.responseMessage);
                }
            }
            else if (signCloudResp.responseCode == 1007)
            {
                msg.TypeOfError = signCloudResp.mimeType == ESignCloudConstant.MIMETYPE_XML ? TypeOfError.SIGN_XML_ERROR : TypeOfError.SIGN_PDF_ERROR;
                msg.Exception = string.Format("{0} - {1}", signCloudResp.responseCode, signCloudResp.responseMessage);
                Tracert.WriteLog("signCloudResp.responseCode == 1007 : " + signCloudResp.responseMessage);
            }
            else
            {
                msg.TypeOfError = signCloudResp.mimeType == ESignCloudConstant.MIMETYPE_XML ? TypeOfError.SIGN_XML_ERROR : TypeOfError.SIGN_PDF_ERROR;
                msg.Exception = string.Format("{0} - {1}", signCloudResp.responseCode, signCloudResp.responseMessage);
                Tracert.WriteLog("Error while calling prepareFileForSignCloud: " + signCloudResp.responseMessage);
            }
            return msg;
        }
        public async Task<int> PrepareFileForSignCloud(
        string agreementUUID,
        SignCloudMetaData signCloudMetaData,
        string passCode,
        string mimeType,
        string fileName,
        byte[] fileData, string fullXmlFilePath)
        {
            string REST_URL = iConfiguration["IcorpAccount:Url"];
            string relyingParty = iConfiguration["IcorpAccount:relyingParty"];
            string relyingPartyUser = iConfiguration["IcorpAccount:relyingPartyUser"]; ;
            string relyingPartyPassword = iConfiguration["IcorpAccount:relyingPartyPassword"];
            string relyingPartySignature = iConfiguration["IcorpAccount:relyingPartySignature"];
            string relyingPartyKeyStore = iConfiguration["IcorpAccount:relyingPartyKeyStore"];
            string relyingPartyKeyStorePassword = iConfiguration["IcorpAccount:relyingPartyKeyStorePassword"];

            relyingPartyKeyStore = Path.Combine(_hostingEnvironment.WebRootPath, relyingPartyKeyStore);//"file\\rssp.p12";
            string timestamp = Utils.CurrentTimeMillis().ToString();
            string data2sign = relyingPartyUser + relyingPartyPassword + relyingPartySignature + timestamp;
            string pkcs1Signature = Utils.getPKCS1Signature(data2sign, relyingPartyKeyStore, relyingPartyKeyStorePassword);

            SignCloudReq signCloudReq = new SignCloudReq();
            signCloudReq.relyingParty = relyingParty;
            signCloudReq.agreementUUID = agreementUUID;
            signCloudReq.authorizeMethod = ESignCloudConstant.AUTHORISATION_METHOD_PASSCODE;
            signCloudReq.authorizeCode = passCode;
            signCloudReq.messagingMode = ESignCloudConstant.SYNCHRONOUS;
            signCloudReq.certificateRequired = true;
            if (mimeType == ESignCloudConstant.MIMETYPE_PDF)
            {
                signCloudReq.signingFileData = fileData;
            }
            else if (mimeType == ESignCloudConstant.MIMETYPE_XML)
            {
                char[] charXml = Encoding.UTF8.GetString(fileData).ToCharArray();
                string stringXML = new string(charXml);
                signCloudReq.xmlDocument = TextHelper.StripXmlWhitespace(stringXML);
            }
            signCloudReq.mimeType = mimeType;
            signCloudReq.signingFileName = fileName;
            signCloudReq.signCloudMetaData = signCloudMetaData;

            var credentialData = new CredentialData();
            credentialData.username = relyingPartyUser;
            credentialData.password = relyingPartyPassword;
            credentialData.timestamp = timestamp;
            credentialData.signature = relyingPartySignature;
            credentialData.pkcs1Signature = pkcs1Signature;
            signCloudReq.credentialData = credentialData;

            //var javaScriptSerializer = new JsonSerializer();
            //javaScriptSerializer.MaxJsonLength = Int32.MaxValue;
            //javaScriptSerializer.Serialize(signCloudReq);
            string jsonReq = JsonConvert.SerializeObject(signCloudReq, Newtonsoft.Json.Formatting.Indented);
            var signCloudResp = SendPost(REST_URL + FUNCTION_PREPAREFILEFORSIGNCLOUD, jsonReq);


            if (signCloudResp.responseCode == 0 || signCloudResp.responseCode == 1018)
            {
                if (signCloudResp.signedFileData != null)
                {
                    if (signCloudResp.mimeType == ESignCloudConstant.MIMETYPE_XML)
                    {
                        File.WriteAllBytes(fullXmlFilePath, signCloudResp.signedFileData);
                    }
                    else if (signCloudResp.mimeType == ESignCloudConstant.MIMETYPE_PDF)
                    {

                        File.WriteAllBytes(fullXmlFilePath, signCloudResp.signedFileData);
                    }
                }
                else
                {
                    Tracert.WriteLog(signCloudResp.responseMessage);
                }
            }
            else if (signCloudResp.responseCode == 1007)
            {
                Tracert.WriteLog("signCloudResp.responseCode == 1007 : " + signCloudResp.responseMessage);
            }
            else
            {
                Tracert.WriteLog("Error while calling prepareFileForSignCloud");
            }

            return signCloudResp.responseCode;
        }

        public async Task ChangePasscodeForSignCloud(string agreementUUID, string currentPassCode, string newPassCode)
        {
            string REST_URL = iConfiguration["IcorpAccount:Url"];
            string relyingParty = iConfiguration["IcorpAccount:relyingParty"];
            string relyingPartyUser = iConfiguration["IcorpAccount:relyingPartyUser"]; ;
            string relyingPartyPassword = iConfiguration["IcorpAccount:relyingPartyPassword"]; ;
            string relyingPartySignature = iConfiguration["IcorpAccount:relyingPartySignature"];
            string relyingPartyKeyStore = iConfiguration["IcorpAccount:relyingPartyKeyStore"];
            string relyingPartyKeyStorePassword = iConfiguration["IcorpAccount:relyingPartyKeyStorePassword"];

            relyingPartyKeyStore = Path.Combine(_hostingEnvironment.WebRootPath, relyingPartyKeyStore);//"file\\rssp.p12";
            string timestamp = Utils.CurrentTimeMillis().ToString();
            string data2sign = relyingPartyUser + relyingPartyPassword + relyingPartySignature + timestamp;
            string pkcs1Signature = Utils.getPKCS1Signature(data2sign, relyingPartyKeyStore, relyingPartyKeyStorePassword);

            SignCloudReq signCloudReq = new SignCloudReq();
            signCloudReq.relyingParty = relyingParty;
            signCloudReq.agreementUUID = agreementUUID;
            signCloudReq.currentPasscode = currentPassCode;
            signCloudReq.newPasscode = newPassCode;

            CredentialData credentialData = new CredentialData();
            credentialData.username = relyingPartyUser;
            credentialData.password = relyingPartyPassword;
            credentialData.timestamp = timestamp;
            credentialData.signature = relyingPartySignature;
            credentialData.pkcs1Signature = pkcs1Signature;
            signCloudReq.credentialData = credentialData;

            string jsonReq = JsonConvert.SerializeObject(signCloudReq, Newtonsoft.Json.Formatting.Indented);
            var signCloudResp = SendPost(REST_URL + FUNCTION_CHANGEPASSCODEFORSIGNCLOUD, jsonReq);

            if (signCloudResp.responseCode == 0)
            {
                Console.WriteLine("Response Code: " + signCloudResp.responseCode);
                Console.WriteLine("Response Message: " + signCloudResp.responseMessage);
            }
            else
            {
                throw new Exception("Error while calling changePasscodeForSignCloud");
            }
        }

        public async Task<int> ForgetPasscodeForSignCloud(string agreementUUID)
        {
            string REST_URL = iConfiguration["IcorpAccount:Url"];
            string relyingParty = iConfiguration["IcorpAccount:relyingParty"];
            string relyingPartyUser = iConfiguration["IcorpAccount:relyingPartyUser"]; ;
            string relyingPartyPassword = iConfiguration["IcorpAccount:relyingPartyPassword"]; ;
            string relyingPartySignature = iConfiguration["IcorpAccount:relyingPartySignature"];
            string relyingPartyKeyStore = iConfiguration["IcorpAccount:relyingPartyKeyStore"];
            string relyingPartyKeyStorePassword = iConfiguration["IcorpAccount:relyingPartyKeyStorePassword"];

            relyingPartyKeyStore = Path.Combine(_hostingEnvironment.WebRootPath, relyingPartyKeyStore);//"file\\rssp.p12";
            string timestamp = Utils.CurrentTimeMillis().ToString();
            string data2sign = relyingPartyUser + relyingPartyPassword + relyingPartySignature + timestamp;
            string pkcs1Signature = Utils.getPKCS1Signature(data2sign, relyingPartyKeyStore, relyingPartyKeyStorePassword);

            SignCloudReq signCloudReq = new SignCloudReq();
            signCloudReq.relyingParty = relyingParty;
            signCloudReq.agreementUUID = agreementUUID;

            CredentialData credentialData = new CredentialData();
            credentialData.username = relyingPartyUser;
            credentialData.password = relyingPartyPassword;
            credentialData.timestamp = timestamp;
            credentialData.signature = relyingPartySignature;
            credentialData.pkcs1Signature = pkcs1Signature;
            signCloudReq.credentialData = credentialData;

            string jsonReq = JsonConvert.SerializeObject(signCloudReq, Newtonsoft.Json.Formatting.Indented);
            var signCloudResp = SendPost(REST_URL + FUNCTION_FORGETPASSCODEFORSIGNCLOUD, jsonReq);

            if (signCloudResp.responseCode == 0)
            {
                Console.WriteLine("Response Code: " + signCloudResp.responseCode);
                Console.WriteLine("Response Message: " + signCloudResp.responseMessage);
            }
            else
            {
                throw new Exception("Error while calling changePasscodeForSignCloud");
            }
            return signCloudResp.responseCode;
        }
        private SignCloudResp SendPost(string url, string payload)
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                Converters = new[] { new ByteArrayConverter() }
            };
            SignCloudResp signCloudResp = null;
            try
            {
                string result = string.Empty;
                string endpointUrl = url;

                ServicePointManager.CheckCertificateRevocationList = false;
                ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.DefaultConnectionLimit = 9999;

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(endpointUrl);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(payload);
                }

                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                // Write log response
                if (!string.IsNullOrEmpty(result))
                {
                    signCloudResp = JsonConvert.DeserializeObject<SignCloudResp>(result, jsonSerializerSettings);
                    //Tracert.WriteLog("strContent: " + result);
                    //if (signCloudResp.responseCode == 0 || signCloudResp.responseCode == 1018)
                    //{
                    //    await _dataContext.AddTransferLog(result, 3);
                    //}
                }
                return signCloudResp;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
