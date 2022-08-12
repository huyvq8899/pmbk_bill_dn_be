using DLL;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Services.Helper;
using Services.Repositories.Interfaces;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Xml;

namespace Services.Repositories.Implimentations
{
    public class TVanService : ITVanService
    {
        //private readonly string TVAN_URL = "https://tvan.easyinvoice.com.vn";

        //private readonly string TVAN_TAX_CODE = "0200784873";

        //private readonly string TVAN_USER_NAME = "NCC0200784873";

        //private readonly string TVAN_Pass_Word = "VdgMe#cI!rkf";

        private readonly Datacontext _dataContext;

        private readonly IConfiguration iConfiguration;

        public TVanService(IConfiguration IConfiguration, Datacontext dataContext)
        {
            iConfiguration = IConfiguration;
            _dataContext = dataContext;
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
        [Obsolete]
        public async Task<string> TVANSendData(string action, string body, Method method = Method.POST)
        {
            string strContent = string.Empty;

            try
            {
                Tracert.WriteLog("body: " + body);

                // Write log to send
                await _dataContext.AddTransferLog(body);

                // Re-check MNGui, MNNhan
                // Get MST
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(body);

                // MNGui
                XmlNode eleNode = doc.SelectSingleNode("/TDiep/TTChung/MNGui");
                if (eleNode != null && eleNode.InnerText != "0200784873")
                {
                    eleNode.InnerText = $"0200784873";
                }

                // MNNhan
                eleNode = doc.SelectSingleNode("/TDiep/TTChung/MNNhan");
                if (eleNode != null && eleNode.InnerText != "V0200784873")
                {
                    eleNode.InnerText = $"V0200784873";
                }

                // Re-Get xml
                body = doc.OuterXml;

                // Send
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(body);
                var data = System.Convert.ToBase64String(plainTextBytes);

                // Get Url
                string url = iConfiguration["TVanAccount:Url"];
                var client = new RestClient(url);

                // Request
                var request = CreateRequest(action, method);
                request.RequestFormat = DataFormat.Json;
                request.AddHeader("Content-Type", "application/json");
                request.AddBody(data);

                // Get response
                var response = client.Execute(request);
                strContent = response.Content;

                // Write log response
                if (!string.IsNullOrEmpty(strContent))
                {
                    Tracert.WriteLog("strContent: " + strContent);
                    await _dataContext.AddTransferLog(strContent, 3);
                }
            }
            catch (Exception ex)
            {
                Tracert.WriteLog(string.Empty, ex);
            }

            return strContent;
        }

        private string GetToken()
        {
            try
            {
                // Get value from configuration
                string url = iConfiguration["TVanAccount:Url"];
                string taxcode = iConfiguration["TVanAccount:TaxCode"];
                string username = iConfiguration["TVanAccount:UserName"];
                string password = iConfiguration["TVanAccount:PassWord"];

                // Open client
                var client = new RestClient(url);
                var request = new RestRequest("api/authen/login", Method.POST)
                {
                    RequestFormat = DataFormat.Json
                };
                var body = JsonConvert.SerializeObject(new
                {
                    taxcode,
                    username,
                    password,
                });
                request.AddJsonBody(body);
                var response = client.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    if (!string.IsNullOrWhiteSpace(response.Content))
                    {
                        var content = JObject.Parse(response.Content);
                        return content["token"].ToString();
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private RestRequest CreateRequest(string action, Method method = Method.POST)
        {
            var request = new RestRequest(action, method)
            {
                Timeout = 5000 //
            };

            var token = GetToken();

            if (string.IsNullOrWhiteSpace(token))
                throw new Exception("TVANRestApi-GetToken is null");

            request.AddHeader("Authorization", "Bearer " + token);

            return request;
        }
    }
}
