using DLL;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Services.Helper;
using Services.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using DLL.Constants;
using ManagementServices.Helper;
using Newtonsoft.Json.Serialization;
using System.Xml.Linq;
using System.Xml.XPath;
using Services.Helper.XmlModel;

namespace Services.Repositories.Implimentations
{
    public class TVanService : ITVanService
    {
        private readonly string TVAN_URL = "https://tvan.easyinvoice.com.vn";

        private readonly string TVAN_TAX_CODE = "0200784873";

        private readonly string TVAN_USER_NAME = "NCC0200784873";

        private readonly string TVAN_Pass_Word = "VdgMe#cI!rkf";

        private readonly Datacontext db;

        private readonly IConfiguration iConfiguration;
        private readonly IHttpContextAccessor _IHttpContextAccessor;

        public TVanService(IConfiguration IConfiguration, Datacontext db, IHttpContextAccessor IHttpContextAccessor)
        {
            this.iConfiguration = IConfiguration;
            this.db = db;
            _IHttpContextAccessor = IHttpContextAccessor;
        }

        /// <summary>
        /// Gửi dữ liệu tới TVAN (Softdream)
        /// </summary>
        /// <param name="action">
        ///  "api/register/send"       gửi thông báo đăng ký sử dụng hóa đơn điện tử
        ///  "api/invoice/send"        gửi thông báo dữ liệu hóa đơn lên TVan
        ///  "api/error-invoice/send"  gửi thông báo cáo hóa đơn sai sót lên TVan
        ///  "api/report/send"         gửi thông báo bảng tổng hợp hóa đơn lên TVan
        /// </param>
        /// <param name="body">
        /// chuỗi thông điệp gửi đến TVan
        /// </param>
        /// <param name="method">
        /// mặc định POST
        /// </param>
        public async Task<string> TVANSendData(string action, string body, Method method = Method.POST)
        {
            string strContent = string.Empty;

            try
            {
                // Write log to send
                await db.AddTransferLog(body);

                // Send
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(body);
                var data = System.Convert.ToBase64String(plainTextBytes);

                //
                var obj = new
                {
                    DataXML = data
                };

                // Get Url
                string url = iConfiguration["TVanAccount:Url"];
                var client = new RestClient(url);
                
                // Request
                var request = CreateRequest(action, method);
                request.RequestFormat = DataFormat.Json;
                request.AddHeader("Content-Type", "application/json;charset=UTF-8");

                //
                request.AddJsonBody(obj);

                // Get response
                var response = client.Execute(request);
                if (response.Content != null)
                {
                    var td = JsonConvert.DeserializeObject<ThongDiepPhanHoiParams>(response.Content);
                    strContent = td.DataXML;
                }

                // Write log response
                if (!string.IsNullOrEmpty(strContent))
                {
                    await db.AddTransferLog(strContent, 3);
                }
            }
            catch (Exception ex)
            {
                Tracert.WriteLog(string.Empty, ex);
            }

            return strContent;
        }

        public string GetToken()
        {
            try
            {
                // Get value from configuration
                string url = iConfiguration["TVanAccount:Url"];
                string taxcode = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.TAX_CODE)?.Value;
                string username = iConfiguration["TVanAccount:UserName"];
                string password = iConfiguration["TVanAccount:PassWord"];

                // Open client
                var client = new RestClient(url);
                var request = new RestRequest("api/Auth/Login", Method.POST);
                request.RequestFormat = DataFormat.Json;
                var body = JsonConvert.SerializeObject(new
                {
                    taxcode = taxcode,
                    username = username,
                    password = password,
                });
                request.AddJsonBody(body);
                var response = client.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    if (!string.IsNullOrWhiteSpace(response.Content))
                    {
                        var content = JObject.Parse(response.Content);
                        return content["tokenKey"].ToString();
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public RestRequest CreateRequest(string action, Method method = Method.POST)
        {
            var request = new RestRequest(action, method);

            request.Timeout = 500000; //

            var token = GetToken();

            if (string.IsNullOrWhiteSpace(token))
                throw new Exception("TVANRestApi-GetToken is null");

            request.AddHeader("Authorization", "Bearer " + token);

            return request;
        }

        public async Task<bool> LCSSendData(string DataXML)
        {
            try
            {
                XDocument doc = XDocument.Parse(DataXML);

                var obj = new
                {
                    TaxCode = _IHttpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypeConstants.TAX_CODE)?.Value,
                    MTDiep = doc.XPathSelectElement("/TDiep/TTChung/MTDiep").Value,
                };

                var json = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                // lưu thông điệp
                string ipConfig = iConfiguration["SocketTTCTransfer:ip"];
                int portConfig = Int32.Parse(iConfiguration["SocketTTCTransfer:port"]);
                string result = SocketHelper.SendViaSocketConvert(ipConfig, portConfig, json);

                // send
                var obj1 = new
                {
                    JwtToken = "eyJhbGciOiJIUzUxMiJ9.eyJzdWIiOiJkODIyNzZmMC02MzQ2LTM0MWItOTg3Mi03YmViMWY1NTFlODUiLCJhdXRoIjoiUk9MRV9UVkFOX1RDR1AyX0JBU0lDX1VTRVIiLCJ1c2VySW5mbyI6eyJ1c2VyQ29kZSI6InR2YW4udGNncDIuZDgyMjc2ZjAtNjM0Ni0zNDFiLTk4NzItN2JlYjFmNTUxZTg1IiwiZnVsbE5hbWUiOiJU4buVIGNo4bupYyBnaeG6o2kgcGjDoXAgMiIsImZpcnN0TG9naW5UaW1lIjoxNjQxOTg2ODU3MDAwLCJsYXN0TG9naW5UaW1lIjoxNjQxOTg3ODYxMDUyLCJ1c2VyTmFtZSI6ImQ4MjI3NmYwLTYzNDYtMzQxYi05ODcyLTdiZWIxZjU1MWU4NSIsInRlbmFudENvZGUiOiJUQ0dQMiIsImFwcENvZGUiOiJUVkFOIn0sImNsaWVudEluZm8iOnsiY2xpZW50Q29kZSI6ImQ4MjI3NmYwLTYzNDYtMzQxYi05ODcyLTdiZWIxZjU1MWU4NSIsImVtYWlsIjoidGNncDJAdGNncDIudm4iLCJmdWxsTmFtZSI6IlThu5UgY2jhu6ljIGdp4bqjaSBwaMOhcCAyIiwiY2xpZW50VHlwZSI6MCwiY29udGFjdEVtYWlsIjpudWxsLCJwaG9uZU51bWJlciI6IjA5MDk4NDY2NjAiLCJjcm1SZWYiOiIifSwiZXhwIjoxNjQ0NTU0NjYxfQ.20OLalZNMSLmd3S-IOjZXawzKyiWtrZtquRzGesz1psBgxI80stXJ3fS8c681fCdefrEiIc_PSQAPM2AWn_XPg",
                    Event = "EVENT_TCGP_TO_TCTN",
                    SourceServiceName = "TCGP_BACHKHOA",
                    SourceServiceNode = 1,
                    RootUuid = Guid.NewGuid().ToString(),
                    SubUuid = "",
                    TimeSubmit = 1641960912,
                    SourceIp = "",
                    Data = new
                    {
                        DLieu = TextHelper.Base64Encode(DataXML)
                    }
                };
                var json1 = JsonConvert.SerializeObject(obj1, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                json1 = json1.Replace("dLieu", "DLieu");

                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(json1);
                return QueueHelper.SendMsg(plainTextBytes);
            }
            catch (Exception ex)
            {
                Tracert.WriteLog(string.Empty, ex);
                return false;
            }
        }
    }
}
