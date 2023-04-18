using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Enums;
using Services.Helper.Params.HoaDon;
using Services.Repositories.Implimentations.ESignCloud;
using Services.Repositories.Interfaces.ESignCloud;
using Services.ViewModels.XML;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace API.Controllers.ESignCloud
{
    public class ESignCloudController : BaseController
    {
        private readonly IESignCloudService _eSignCloudService;
        public ESignCloudController(IESignCloudService eSignCloudService)
        {
            _eSignCloudService = eSignCloudService;
        }
        /// <summary>
        /// Lấy thông tin cks thông qua mã định danh
        /// </summary>
        /// <param name="agreementUUID"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetInfoSignCloud/{agreementUUID}")]
        public async Task<IActionResult> GetInfoSignCloud(string agreementUUID)
        {
            var result = await _eSignCloudService.GetInfoSignCloud(agreementUUID);
            return Ok(result);
        }
        /// <summary>
        /// Ký xml
        /// </summary>
        /// <param name="agreementUUID"></param>
        /// <returns></returns>
        [HttpPost("SignCloudFile")]
        public async Task<IActionResult> SignCloudFile(MessageObj data)
        {
            var result = await _eSignCloudService.SignCloudFile(data);
            string jsonReq = JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented);
            return Ok(jsonReq);
        }

    }
}
