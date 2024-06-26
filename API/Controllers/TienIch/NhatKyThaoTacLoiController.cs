﻿using DLL.Enums;
using Microsoft.AspNetCore.Mvc;
using Services.Repositories.Interfaces.TienIch;
using Services.ViewModels.TienIch;
using System.Threading.Tasks;

namespace API.Controllers.TienIch
{
    public class NhatKyThaoTacLoiController : BaseController
    {
        private readonly INhatKyThaoTacLoiService _nhatKyThaoTacLoiService;

        public NhatKyThaoTacLoiController(INhatKyThaoTacLoiService nhatKyThaoTacLoiService)
        {
            _nhatKyThaoTacLoiService = nhatKyThaoTacLoiService;
        }

        [HttpGet("GetByDetail/{refId}/{thaoTacLoi}")]
        public async Task<IActionResult> GetByDetail(string refId, ThaoTacLoi thaoTacLoi)
        {
            var result = await _nhatKyThaoTacLoiService.GetByDetailAsync(refId, thaoTacLoi);
            return Ok(result);
        }

        [HttpGet("GetByRefId/{refId}")]
        public async Task<IActionResult> GetByRefId(string refId)
        {
            var result = await _nhatKyThaoTacLoiService.GetByRefIdAsync(refId);
            return Ok(result);
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert(NhatKyThaoTacLoiViewModel model)
        {
            var result = await _nhatKyThaoTacLoiService.InsertAsync(model);
            return Ok(result);
        }
    }
}
