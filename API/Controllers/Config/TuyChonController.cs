using DLL;
using Microsoft.AspNetCore.Mvc;
using Services.Repositories.Interfaces.Config;
using Services.ViewModels.Config;
using System;
using System.Threading.Tasks;

namespace API.Controllers.Config
{
    public class TuyChonController : BaseController
    {
        private ITuyChonService _tuyChonService;
        private Datacontext _db;

        public TuyChonController(ITuyChonService tuyChonService, Datacontext datacontext)
        {
            _tuyChonService = tuyChonService;
            _db = datacontext;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(string keyword)
        {
            var result = await _tuyChonService.GetAllAsync(keyword);
            return Ok(result);
        }

        [HttpGet("GetDetail/{ma}")]
        public async Task<IActionResult> GetDetail(string ma)
        {
            if (string.IsNullOrEmpty(ma))
            {
                return BadRequest();
            }

            var result = await _tuyChonService.GetDetailAsync(ma);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] TuyChonViewModel model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var rs = await _tuyChonService.UpdateAsync(model);
                    transaction.Commit();

                    foreach (var item in model.NewList)
                    {
                        Response.Cookies.Append(item.Ma, item.GiaTri);
                    }

                    return Ok(rs);
                }
                catch (Exception ex)
                {
                    return Ok(false);
                }
            }
        }
    }
}
