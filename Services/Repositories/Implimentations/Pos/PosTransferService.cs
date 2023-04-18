using DLL;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RestSharp;
using Services.Helper;
using Services.Repositories.Interfaces.Pos;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Services.ViewModels.Pos;
using System.Linq;

namespace Services.Repositories.Implimentations.Pos
{
    public class PosTransferService : IPosTransferService 
    {
        private readonly Datacontext _dataContext;

        private readonly IConfiguration iConfiguration;

        public PosTransferService(IConfiguration IConfiguration, Datacontext dataContext)
        {
            iConfiguration = IConfiguration;
            _dataContext = dataContext;
        }

        [Obsolete]
        public async Task<string> SendResponseTCTToPos(List<ResultHoaDonMTTViewModels> listResult)
        {
            string strContent = "";

            try
            {
                var listResult1 = listResult.AsQueryable();
                var query = listResult1.GroupBy(x => new { x.PosCustomerURL })
                .Select(x => new ResultHoaDonMTTViewModels
                {
                    PosCustomerURL = x.Key.PosCustomerURL,
                    ListHoaDons = x.OrderBy(y => y.SoHoaDon).ToList()
                });

                Method method = Method.POST;
                foreach (var item in query)
                {
                    if (!string.IsNullOrEmpty(item.PosCustomerURL))
                    {
                        string url = item.PosCustomerURL.Trim();

                        var token = await this.GetTokenPos(url);

                        var body = JsonConvert.SerializeObject(item.ListHoaDons);
                        // Get Url
                        var client = new RestClient(url);

                        // Request
                        //var request = CreateRequest(action, method);
                        var request = new RestRequest("api/sales/capNhapTrangThaiQuyTrinh", method)
                        {
                            Timeout = 5000,
                            RequestFormat = DataFormat.Json
                        };
                        request.AddHeader("Authorization", "Bearer " + token);
                        request.AddJsonBody(body);

                        // Get response
                        var response = await client.ExecuteAsync(request);
                        strContent = response.Content;
                    }
                }
            }
            catch (Exception ex)
            {
                 Tracert.WriteLog("Exception SendResponseTCTToPos:: " + ex.Message);
            }

            return strContent;
        }



        public async Task<string> GetTokenPos(string url)
        {
            try
            {
                // Get value from configuration
                string username = iConfiguration["PostAccount:UserName"];
                string password = iConfiguration["PostAccount:PassWord"];

                // Open client
                var client = new RestClient(url);
                var request = new RestRequest("api/auth/login", Method.POST)
                {
                    RequestFormat = DataFormat.Json
                };
                var body = JsonConvert.SerializeObject(new
                {
                    username,
                    password,
                });
                request.AddJsonBody(body);
                var response = await client.ExecuteAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    if (!string.IsNullOrWhiteSpace(response.Content))
                    {
                        var content = JObject.Parse(response.Content);
                        return content["token"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Tracert.WriteLog("GetToken2", ex);
            }

            return string.Empty;
        }

    }
}
