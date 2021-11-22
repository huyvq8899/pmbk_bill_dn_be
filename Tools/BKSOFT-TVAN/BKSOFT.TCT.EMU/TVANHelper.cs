using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BKSOFT.TCT.EMU
{
    public class TVANHelper
    {
        public TVanInfo TVanInfo { set; get; }

        public TVANHelper(TVanInfo info)
        {
            TVanInfo = info;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action">
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
        public string TVANSendData(string action, string body, Method method = Method.POST)
        {
            string strContent = string.Empty;
            try
            {
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(body);
                var data = System.Convert.ToBase64String(plainTextBytes);
                var client = new RestClient(TVanInfo.ApiUrl);
                var request = CreateRequest(action, method);
                request.RequestFormat = DataFormat.Json;
                request.AddHeader("Content-Type", "application/json");
                request.AddBody(data);

                var response = client.Execute(request);

                strContent = response.Content;

                Console.WriteLine(strContent);
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(string.Empty, ex);
            }

            return strContent;
        }

        private string GetToken()
        {
            try
            {
                var client = new RestClient(TVanInfo.ApiUrl);
                var request = new RestRequest("api/authen/login", Method.POST);
                request.RequestFormat = DataFormat.Json;

                var body = JsonConvert.SerializeObject(new
                {
                    taxcode = TVanInfo.ApiTaxCode,
                    username = TVanInfo.ApiUserName,
                    password = TVanInfo.ApiPassword
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
                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public RestRequest CreateRequest(string action, Method method = Method.POST)
        {
            var request = new RestRequest(action, method);

            request.Timeout = 5000; //

            var token = GetToken();

            if (string.IsNullOrWhiteSpace(token))
                throw new Exception("TVANRestApi-GetToken is null");

            request.AddHeader("Authorization", "Bearer " + GetToken());

            return request;
        }
    }
}
