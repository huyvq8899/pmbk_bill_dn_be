using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Helper.Params.DanhMuc;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
using System;
using System.Threading.Tasks;

namespace API.Controllers.DanhMuc
{
    public class HangHoaDichVuController : BaseController
    {
        private readonly IHangHoaDichVuService _hangHoaDichVuService;
        public HangHoaDichVuController(IHangHoaDichVuService hangHoaDichVuService)
        {
            _hangHoaDichVuService = hangHoaDichVuService;
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll(HangHoaDichVuParams @params)
        {
            var result = await _hangHoaDichVuService.GetAllAsync(@params);
            return Ok(result);
        }

        [HttpPost("GetAllPaging")]
        public async Task<IActionResult> GetAllPaging(HangHoaDichVuParams pagingParams)
        {
            var paged = await _hangHoaDichVuService.GetAllPagingAsync(pagingParams);
            return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
        }

        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _hangHoaDichVuService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("CheckTrungMa")]
        public async Task<IActionResult> CheckTrungMa(HangHoaDichVuViewModel model)
        {
            var result = await _hangHoaDichVuService.CheckTrungMaAsync(model);
            return Ok(result);
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert(HangHoaDichVuViewModel model)
        {
            var result = await _hangHoaDichVuService.InsertAsync(model);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(HangHoaDichVuViewModel model)
        {
            var result = await _hangHoaDichVuService.UpdateAsync(model);
            return Ok(result);
        }

        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _hangHoaDichVuService.DeleteAsync(id);
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
    }
}
