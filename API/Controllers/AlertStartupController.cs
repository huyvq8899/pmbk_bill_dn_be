using System;
using System.Threading.Tasks;
using API.Extentions;
using DLL;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Mvc;
using Services.Helper.Params.HeThong;
using Services.Repositories.Interfaces;
using Services.ViewModels;

namespace API.Controllers
{
    public class AlertStartupController : BaseController
    {
        private readonly IAlertStartupService _IAlertStartupService;

        public AlertStartupController(IAlertStartupService IAlertStartupService)
        {
            _IAlertStartupService = IAlertStartupService;
        }


        [HttpGet("GetAlertStartupActive")]
        public async Task<IActionResult> GetAlertStartupActive()
        {
            var result = await _IAlertStartupService.GetByStatus();
            return Ok(result);
        }


        [HttpPost("Insert")]
        public async Task<IActionResult> Insert(AlertStartupViewModel model)
        {
            var result = await _IAlertStartupService.Insert(model);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(AlertStartupViewModel model)
        {
            var result = await _IAlertStartupService.Update(model);
            return Ok(result);
        }

    }
}