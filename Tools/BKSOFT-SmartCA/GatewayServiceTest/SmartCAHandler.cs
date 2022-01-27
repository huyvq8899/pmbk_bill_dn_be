using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VnptHashSignatures.Common;
using VnptHashSignatures.Interface;
using VnptHashSignatures.Xml;

namespace GatewayServiceTest
{
    public static class SmartCAHandler
    {
        /// <summary>
        /// sign xml smart ca
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pass"></param>
        /// <param name="MLTDiep"></param>
        /// <param name="unsignData"></param>
        /// <param name="description">Mau so/ky hieu/so</param>
        /// <returns></returns>
        public static string SignSmartCAXML(string email, string pass, MLTDiep MLTDiep, string xmlData, string description)
        {
            try
            {
                string xmlSigned = string.Empty;

                byte[] unsignData = Encoding.UTF8.GetBytes(xmlData);

                // Step 1: Get access token
                var access_token = GetAccessToken(email, pass);

                // Step 2: Get Credential SmartCA
                var credentialsUri = Properties.Settings.Default["URL_CREDENTIALS_LIST"] as string;
                string credential = GetCredentialSmartCA(access_token, credentialsUri);

                // Step 3: Accout SmartCA Cert
                var caCertUri = Properties.Settings.Default["URL_CREDENTIALS_INFO"] as string;
                string certBase64 = GetAccoutSmartCACert(access_token, caCertUri, credential);

                // Step 4: Sign XML
                IHashSigner signers = HashSignerFactory.GenerateSigner(unsignData, certBase64, null, HashSignerFactory.XML);
                ((XmlHashSigner)signers).SetHashAlgorithm(MessageDigestAlgorithm.SHA256);

                // Set reference đến id
                ((XmlHashSigner)signers).SetReferenceId("#SigningData");

                // Set thời gian ký
                ((XmlHashSigner)signers).SetSigningTime(DateTime.Now, "SigningTime");

                // Check loại
                switch (MLTDiep)
                {
                    case MLTDiep.TDGToKhai:
                    case MLTDiep.TDGToKhaiUN:
                    case MLTDiep.TDDNCHDDT:
                        ((XmlHashSigner)signers).SetParentNodePath("/TDiep/DLieu/TKhai/DSCKS/NNT");
                        break;
                    case MLTDiep.TDGHDDTTCQTCapMa:
                        ((XmlHashSigner)signers).SetParentNodePath("/TDiep/DLieu/HDon/DSCKS/NBan");
                        break;
                    case MLTDiep.TDTBHDDLSSot:
                        ((XmlHashSigner)signers).SetParentNodePath("/TDiep/DLieu/TBao/DSCKS/NNT");
                        break;
                    case MLTDiep.TDCBTHDLHDDDTDCQThue:
                        ((XmlHashSigner)signers).SetParentNodePath("/TDiep/DLieu/BTHDLieu/DSCKS/NNT");
                        break;
                    default:
                        break;
                }

                // Hash data
                string hashValues = null;
                try
                {
                    hashValues = signers.GetSecondHashAsBase64();
                }
                catch (Exception ex)
                {
                    Tracert.WriteLog(string.Empty, ex);

                    return xmlSigned;
                }

                // Step 5: 
                var urlSignHash = Properties.Settings.Default["URL_SIGN_HASH"] as string;
                var tranId = SignHash(access_token, urlSignHash, hashValues, credential, description);
                if (string.IsNullOrEmpty(tranId))
                {
                    Tracert.WriteLog("Ky so that bai");
                    return xmlSigned;
                }

                // Step 6: User access mobile application
                var count = 0;
                var isConfirm = false;
                var datasigned = "";
                var urlGetTranInfo = Properties.Settings.Default["URL_GET_TRAN_ID"] as string;
                while (count < 24 && !isConfirm)
                {
                    var tranInfo = GetTranInfo(access_token, urlGetTranInfo, tranId);
                    if (tranInfo != null)
                    {
                        if (tranInfo.tranStatus != 1)
                        {
                            count = count + 1;
                            Thread.Sleep(10000);
                        }
                        else
                        {
                            isConfirm = true;
                            datasigned = tranInfo.documents[0].sig;
                        }
                    }
                    else
                    {
                        Tracert.WriteLog("Error from content");
                        return xmlSigned;
                    }
                }
                if (!isConfirm)
                {
                    Tracert.WriteLog("Signer not confirm from App");
                    return xmlSigned;
                }
                if (string.IsNullOrEmpty(datasigned))
                {
                    Tracert.WriteLog("Sign error");
                    return xmlSigned;
                }
                if (!signers.CheckHashSignature(datasigned))
                {
                    Tracert.WriteLog("Signature not match");
                    return xmlSigned;
                }

                // Step 7. Verify
                if (!signers.CheckHashSignature(datasigned))
                {
                    Tracert.WriteLog("Signature not match");
                    return xmlSigned;
                }

                // Step 8. Package external signature to signed file
                byte[] signed = signers.Sign(datasigned);
                xmlSigned = Encoding.UTF8.GetString(signed);

                // Return xml.
                return xmlSigned;
            }
            catch (Exception ex)
            {
                Tracert.WriteLog(ex.Message);
                return null;
            }
        }

        public static CertRes SmartCAInfoFull(string email, string pass)
        {
            try
            {
                // Step 1: Get access token
                var access_token = GetAccessToken(email, pass);

                // Step 2: Get Credential SmartCA
                var credentialsUri = Properties.Settings.Default["URL_CREDENTIALS_LIST"] as string;
                string credential = GetCredentialSmartCA(access_token, credentialsUri);

                // Step 3: Accout SmartCA Cert
                var caCertUri = Properties.Settings.Default["URL_CREDENTIALS_INFO"] as string;
                var cert = GetAccoutSmartCACertFullInfo(access_token, caCertUri, credential);

                // Return xml.
                return cert;
            }
            catch (Exception ex)
            {
                Tracert.WriteLog(ex.Message);
                return null;
            }

            return null;
        }

        private static string SignHash(string accessToken, String serviceUri, string data, string credentialId, string description)
        {
            var req = new SignHashSmartCAReq
            {
                credentialId = credentialId,
                refTranId = "1234-5678-90000",
                notifyUrl = "http://10.169.0.221/api/v1/smart_ca_callback",
                description = description,
                datas = new List<DataSignHash>()
            };
            var test = new DataSignHash
            {
                name = description,
                hash = data
            };
            req.datas.Add(test);

            var response = ServiceClientQuery(req, serviceUri, accessToken);
            if (response != null)
            {
                SignHashSmartCAResponse resp = JsonConvert.DeserializeObject<SignHashSmartCAResponse>(response);
                if (resp.code == 0)
                {
                    return resp.content.tranId;
                }
            }
            return string.Empty;
        }

        private static TranInfoSmartCARespContent GetTranInfo(string accessToken, String serviceUri, String tranId)
        {
            var response = ServiceClientQuery(new ContenSignHash
            {
                tranId = tranId
            }, serviceUri, accessToken);

            if (response != null)
            {
                TranInfoSmartCAResp resp = JsonConvert.DeserializeObject<TranInfoSmartCAResp>(response);
                if (resp.code == 0)
                {
                    return resp.content;
                }
            }
            return null;
        }

        public static string GetAccessToken(string email, string pass)
        {
            string refresh_token = "";
            // Verify service certificate. 
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(SslHelper.ValidateRemoteCertificate);

            // Create RestClient instance and add client certificate
            var tokenUri = Properties.Settings.Default["SERVICE_GET_TOKENURL"] as string;

            RestClient client = new RestClient(tokenUri);

            // Token request body
            RestRequest request = new RestRequest(Method.POST);
            var req = new GetTokenBody
            {
                grant_type = "password",
                username = email,
                password = pass,
                client_id = Properties.Settings.Default["APP_ID"] as string,
                client_secret = Properties.Settings.Default["APP_SECRET"] as string
            };
            var param = JsonConvert.SerializeObject(req);
            Tracert.WriteLog("access_token request:" + param);

            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", $"grant_type={req.grant_type}" +
                $"&username={req.username}&password={req.password}&client_id={req.client_id}" +
                $"&client_secret={req.client_secret}", ParameterType.RequestBody);
            // End: Token request body

            IRestResponse response = null;
            try
            {
                response = client.Execute(request);
            }
            catch (Exception ex)
            {
                Tracert.WriteLog($"Connect gateway error: {ex.Message}", ex);
            }

            if (response == null || response.ErrorException != null)
            {
                Tracert.WriteLog("Service return null response");
                return null;
            }
            Encoding encoding = Encoding.GetEncoding("ISO-8859-1");
            var result = encoding.GetString(response.RawBytes);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                Tracert.WriteLog($"Status code={response.StatusCode}. Status content: {response.Content}");
                return null;
            }

            var respContent = response.Content;
            try
            {
                var resp = JsonConvert.DeserializeObject<GetTokenResponse>(respContent);
                Tracert.WriteLog("access_token:" + resp.access_token);
                Tracert.WriteLog($"refresh_token: {resp.refresh_token}");
                refresh_token = resp.refresh_token;
                return resp.access_token;
            }
            catch (Exception ex)
            {
                Tracert.WriteLog($"Service return error: {ex.Message}", ex);
                return null;
            }
        }

        private static string GetCredentialSmartCA(String accessToken, String serviceUri)
        {
            var response = ServiceClientQuery(new ReqCredentialSmartCA(), serviceUri, accessToken);

            if (response != null)
            {
                CredentialSmartCAResponse credentials = JsonConvert.DeserializeObject<CredentialSmartCAResponse>(response);
                return credentials.content[0];
            }
            return string.Empty;
        }

        private static string GetAccoutSmartCACert(String accessToken, String serviceUri, string credentialId)
        {
            var response = ServiceClientQuery(new ReqCertificateSmartCA
            {
                credentialId = credentialId,
                certificates = "chain",
                certInfo = true,
                authInfo = true
            }, serviceUri, accessToken);
            if (response != null)
            {
                CertificateSmartCAResponse req = JsonConvert.DeserializeObject<CertificateSmartCAResponse>(response);
                String certBase64 = req.cert.certificates[0];
                return certBase64.Replace("\r\n", "");
            }
            return "";
        }

        private static CertRes GetAccoutSmartCACertFullInfo(String accessToken, String serviceUri, string credentialId)
        {
            var response = ServiceClientQuery(new ReqCertificateSmartCA
            {
                credentialId = credentialId,
                certificates = "chain",
                certInfo = true,
                authInfo = true
            }, serviceUri, accessToken);
            if (response != null)
            {
                CertificateSmartCAResponse req = JsonConvert.DeserializeObject<CertificateSmartCAResponse>(response);
                return req.cert;
            }
            return null;
        }

        private static ResponseMessage ServiceClientQuery(RequestMessage req, string accessToken)
        {
            // Verify service certificate. 
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(SslHelper.ValidateRemoteCertificate);

            // Create RestClient instance and add client certificate
            var serviceUri = Properties.Settings.Default["SERVICE_URL"] as string;
            RestClient client = new RestClient(serviceUri);

            RestRequest request = new RestRequest(Method.POST)
            {
                RequestFormat = DataFormat.Json
            };
            request.AddJsonBody(req);
            request.AddHeader("Authorization", $"Bearer {accessToken}");

            IRestResponse response = null;
            try
            {
                response = client.Execute(request);
            }
            catch (Exception ex)
            {
                Tracert.WriteLog($"Connect gateway error: {ex.Message}", ex);
            }

            if (response == null || response.ErrorException != null)
            {
                Tracert.WriteLog("Service return null response");
                return null;
            }
            if (response.StatusCode != HttpStatusCode.OK)
            {
                Tracert.WriteLog($"Status code={response.StatusCode}. Status content: {response.Content}");
                return null;
            }

            var respContent = response.Content;
            try
            {
                return JsonConvert.DeserializeObject<ResponseMessage>(respContent);
            }
            catch (Exception ex)
            {
                Tracert.WriteLog($"Service return error: {ex.Message}", ex);
                return null;
            }
        }

        private static string ServiceClientQuery(object req, string serviceUri, string accessToken)
        {
            // Verify service certificate. 
            ServicePointManager.ServerCertificateValidationCallback
                += new RemoteCertificateValidationCallback(SslHelper.ValidateRemoteCertificate);

            RestClient client = new RestClient(serviceUri);
            RestRequest request = new RestRequest(Method.POST)
            {
                RequestFormat = DataFormat.Json
            };
            request.AddJsonBody(req);
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            IRestResponse response = null;
            try
            {
                response = client.Execute(request);
            }
            catch (Exception ex)
            {
                Tracert.WriteLog($"Connect gateway error: {ex.Message}", ex);
            }

            if (response == null || response.ErrorException != null)
            {
                Tracert.WriteLog("Service return null response");
                return null;
            }
            if (response.StatusCode != HttpStatusCode.OK)
            {
                Tracert.WriteLog($"Status code={response.StatusCode}. Status content: {response.Content}");
                return null;
            }

            return response.Content;
        }
    }
}
