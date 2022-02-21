using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories.Interfaces
{
    public interface ITVanService
    {
        Task<string> TVANSendData(string action, string body, Method method = Method.POST);
        Task<bool> LCSSendData(string DataXML);
    }
}
