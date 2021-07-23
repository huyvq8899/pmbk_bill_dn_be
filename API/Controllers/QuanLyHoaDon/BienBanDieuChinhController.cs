using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Repositories.Interfaces.QuanLyHoaDon;
using Services.ViewModels.QuanLyHoaDonDienTu;
using System;
using System.Threading.Tasks;

namespace API.Controllers.QuanLyHoaDon
{
    public class BienBanDieuChinhController : BaseController
    {
        private readonly IBienBanDieuChinhService _bienBanDieuChinhService;

        public BienBanDieuChinhController(IBienBanDieuChinhService bienBanDieuChinhService)
        {
            _bienBanDieuChinhService = bienBanDieuChinhService;
        }

        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _bienBanDieuChinhService.GetByIdAsync(id);
            return Ok(result);
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
            catch (DbUpdateException ex)
            {
                return Ok(new
                {
                    result = "DbUpdateException",
                    value = false
                });
            }
            catch (Exception ex)
            {
                return Ok(false);
            }
        }

        [HttpGet("PreviewBienBan/{id}")]
        public IActionResult PreviewBienBan(string id)
        {
            var result = _bienBanDieuChinhService.PreviewBienBan(id);
            return File(result.Bytes, result.ContentType, result.FileName);
        }
    }
}
