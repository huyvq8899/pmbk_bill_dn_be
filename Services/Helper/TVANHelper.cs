using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Services.Helper
{
    public class TVANHelper
    {
        public static string TVANSendData(string action, string body, Method method = Method.POST)
        {
            try
            {
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(body);
                var data = System.Convert.ToBase64String(plainTextBytes);
                var client = new RestClient("http://tvan78.softdreams.vn/");
                var request = CreateRequest(action, method);
                request.RequestFormat = DataFormat.Json;
                request.AddHeader("Content-Type", "application/json");
                request.AddBody(data);

                var response = client.Execute(request);

                return response.Content;
            }
            catch (Exception ex)
            {
                Tracert.WriteLog(string.Empty, ex);
            }

            return string.Empty;
        }

        private static string GetToken()
        {
            try
            {
                var client = new RestClient("http://tvan78.softdreams.vn/");
                var request = new RestRequest("api/authen/login", Method.POST);
                request.RequestFormat = DataFormat.Json;

                var body = JsonConvert.SerializeObject(new
                {
                    taxcode = "0200784873",
                    username = "0200784873",
                    password = "12345678",
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

        public static RestRequest CreateRequest(string action, Method method = Method.POST)
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
