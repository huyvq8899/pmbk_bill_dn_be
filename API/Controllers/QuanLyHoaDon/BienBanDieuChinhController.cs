using DLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Services.Helper.Params.HoaDon;
using Services.Repositories.Interfaces.QuanLyHoaDon;
using Services.ViewModels.QuanLyHoaDonDienTu;
using System;
using System.Threading.Tasks;

namespace API.Controllers.QuanLyHoaDon
{
    public class BienBanDieuChinhController : BaseController
    {
        private readonly IBienBanDieuChinhService _bienBanDieuChinhService;
        private readonly Datacontext _db;

        public BienBanDieuChinhController(IBienBanDieuChinhService bienBanDieuChinhService, Datacontext datacontext)
        {
            _bienBanDieuChinhService = bienBanDieuChinhService;
            _db = datacontext;
        }

        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _bienBanDieuChinhService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("PreviewBienBan/{id}")]
        public async Task<IActionResult> PreviewBienBan(string id)
        {
            var result = await _bienBanDieuChinhService.PreviewBienBanAsync(id);
            return Ok(new { filePath = result });
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert(BienBanDieuChinhViewModel model)
        {
            var result = await _bienBanDieuChinhService.InsertAsync(model);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(BienBanDieuChinhViewModel model)
        {
            var result = await _bienBanDieuChinhService.UpdateAsync(model);
            return Ok(result);
        }

        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _bienBanDieuChinhService.DeleteAsync(id);
                return Ok(result);
            }
            catch (DbUpdateException)
            {
                return Ok(new
                {
                    result = "DbUpdateException",
                    value = false
                });
            }
            catch (Exception)
            {
                return Ok(false);
            }
        }

        [HttpPost("GateForWebSocket")]
        public async Task<IActionResult> GateForWebSocket(ParamPhatHanhBBDC @params)
        {
            if (string.IsNullOrEmpty(@params.BienBanDieuChinhId))
            {
                return BadRequest();
            }

            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _bienBanDieuChinhService.GateForWebSocket(@params);
                    transaction.Commit();
                    return Ok(result);
                }
                catch (Exception)
                {
                    return Ok(false);
                }
            }
        }
    }
}
