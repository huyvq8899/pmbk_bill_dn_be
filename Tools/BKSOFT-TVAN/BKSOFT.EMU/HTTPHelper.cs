using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BKSOFT.EMU
{
    public class HTTPHelper
    {
        public static async Task<bool> TCTPostData(string url, string XML, string mTDTChieu, string MST)
        {
            bool res = false;

            HttpResponseMessage resp = null;

            try
            {
                ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;

                // Post to server
                string rec = await url
                                 .PostJsonAsync(new
                                 {
                                     MST = MST,
                                     DataXML = XML,
                                     MTDTChieu = mTDTChieu
                                 }).ReceiveString();

                // Check result
                if (rec == "true")
                {
                    res = true;
                }
                else
                {
                    res = false;
                }
            }
            catch (Exception ex)
            {
                FileLog.WriteLog(string.Empty, ex);
            }

            if (resp != null)
            {
                FileLog.WriteLog(resp.ToString());
            }

            return res;
        }
    }
}
