using ManagementServices.Helper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Helper.SmartCA;
using Services.Repositories.Interfaces;
using Services.ViewModels.Config;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmartCAController : BaseController
    {
        public readonly ISmartCAService iSmartCAService;

        public SmartCAController(ISmartCAService ISmartCAService)
        {
            iSmartCAService = ISmartCAService;
        }

        [HttpPost("DangNhapVaoMayChu")]
        public async Task<IActionResult> DangNhapVaoMayChu(TaiKhoanSmartCAViewModel model)
        {
            bool res = true;

            string json = JsonConvert.SerializeObject(
                                                new
                                                {
                                                    UID = model.UserNameSmartCA,
                                                    Password = model.PasswordSmartCA
                                                }, Newtonsoft.Json.Formatting.Indented);

            string encodeJson = TextHelper.Base64Encode(json);

            var result = await iSmartCAService.AccessSmartCA("-token", encodeJson);
            if (string.IsNullOrEmpty(result) || result == "\r\n\r\n" || result == "\r\n")
            {
                res = false;
            }
            else
            {
                _ = await iSmartCAService.InsertAsync(model);
            }

            return Ok(res);
        }

        [HttpPost("SignSmartCAXML")]
        public async Task<IActionResult> SignSmartCAXML(TaiKhoanSmartCAViewModel model)
        {
            //string dataXML = await iSmartCAService.GetDataXMLUnsign(model.DataUnsign);

            //var taiKhoanKiMem = await iSmartCAService.GetChuKiMemMoiNhat();

            //string json = JsonConvert.SerializeObject(
            //               new
            //               {
            //                   UID = taiKhoanKiMem.UserNameSmartCA,
            //                   Password = taiKhoanKiMem.PasswordSmartCA,
            //                   MLTDiep = (int)MLTDiep.TDGToKhai,
            //                   Description = model.Description,
            //                   DataXML = dataXML,

            //               }, Newtonsoft.Json.Formatting.Indented);

            //string encodeJson = TextHelper.Base64Encode(json);

            //string result = await iSmartCAService.AccessSmartCA("-sign", encodeJson);
            //if (!string.IsNullOrEmpty(result))
            //{
            //    return Ok(new { result });
            //}
            //else
            //{
            //    return null;
            //}
            //var result = TextHelper.Base64Encode(xmlData);

            // Dữ liệu nguyên thủy xml đã ký chưa nén
            string xmlData = await iSmartCAService.SignSmartCAXMLToKhai(model);

            // Base 64
            string result = TextHelper.Base64Encode(xmlData);

            // Nén trả lại font-end
            if (!string.IsNullOrEmpty(result))
            {
                return Ok(new { result });
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        ///  Kí hóa đơn mềm
        /// </summary>
        /// <returns></returns>
        ///         [HttpPost("SignSmartCAXML")]
        [HttpPost("SignSmartCAHoaDonXML")]
        public async Task<IActionResult> SignSmartCAHoaDonXML(TaiKhoanSmartCAViewModel model)
        {
            // Dữ liệu nguyên thủy xml đã ký chưa nén
            string dataXMLSigned = await iSmartCAService.SignSmartCAXML(model);

            // Nén trả lại font-end
            var result = TextHelper.Compress(dataXMLSigned);
            if (!string.IsNullOrEmpty(result))
            {
                return Ok(new { result });
            }
            else
            {
                return null;
            }
        }

        [HttpPost("SignSmartCAHoaDonThayTheXML")]
        public async Task<IActionResult> SignSmartCAHoaDonThayTheXML(TaiKhoanSmartCAViewModel model)
        {
            // Dữ liệu nguyên thủy xml đã ký chưa nén
            string xmlData = await iSmartCAService.SignSmartCAXMLSaiSot(model);

            // Base 64
            string result = TextHelper.Base64Encode(xmlData);

            if (!string.IsNullOrEmpty(result))
            {
                return Ok(new { result });
            }
            else
            {
                return null;
            }
            //var result = TextHelper.Base64Encode(xmlData);
        }

        [HttpGet("GetCredentialSmartCAData")]
        public async Task<IActionResult> GetCredentialSmartCAData()
        {
            CertRes cert = null;

            var taiKhoanKiMem = await iSmartCAService.GetChuKiMemMoiNhat();

            string json = JsonConvert.SerializeObject(
                                                new
                                                {
                                                    UID = taiKhoanKiMem.UserNameSmartCA,
                                                    Password = taiKhoanKiMem.PasswordSmartCA
                                                }, Formatting.Indented);

            string encodeJson = TextHelper.Base64Encode(json);

            var result = await iSmartCAService.AccessSmartCA("-info", encodeJson);
            if (!string.IsNullOrEmpty(result))
            {
                result = TextHelper.Base64Decode(result);
                cert = JsonConvert.DeserializeObject<CertRes>(result);

                string Str1 = cert.validFrom.Substring(0, 8);
                string Str2 = cert.validTo.Substring(0, 8);

                cert.validFrom = ForMatDateTime(Str1);
                cert.validTo = ForMatDateTime(Str2);
            }

            return Ok(cert);
        }

        private string ForMatDateTime(string dateInput)
        {
            string nam = dateInput.Substring(0, 4);
            string thang = dateInput.Substring(4, 2);
            string ngay = dateInput.Substring(6, 2);
            string DateFormatOK = nam + "/" + thang + "/" + ngay;
            DateTime dateFrom = DateTime.Parse(DateFormatOK);
            return dateFrom.ToString("yyyy-MM-dd"); ;
        }
    }
}
