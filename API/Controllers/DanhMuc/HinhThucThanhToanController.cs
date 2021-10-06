using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Helper.Params.DanhMuc;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
using System;
using System.Threading.Tasks;

namespace API.Controllers.DanhMuc
{
    public class HinhThucThanhToanController : BaseController
    {
        private readonly IHinhThucThanhToanService _HinhThucThanhToanService;
        public HinhThucThanhToanController(IHinhThucThanhToanService HinhThucThanhToanService)
        {
            _HinhThucThanhToanService = HinhThucThanhToanService;
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll(HinhThucThanhToanParams @params)
        {
            var result = await _HinhThucThanhToanService.GetAllAsync(@params);
            return Ok(result);
        }

        [HttpPost("GetAllPaging")]
        public async Task<IActionResult> GetAllPaging(HinhThucThanhToanParams pagingParams)
        {
            var paged = await _HinhThucThanhToanService.GetAllPagingAsync(pagingParams);
            return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
        }

        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _HinhThucThanhToanService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("CheckTrungMa")]
        public async Task<IActionResult> CheckTrungMa(HinhThucThanhToanViewModel model)
        {
            var result = await _HinhThucThanhToanService.CheckTrungMaAsync(model);
            return Ok(result);
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert(HinhThucThanhToanViewModel model)
        {
            var result = await _HinhThucThanhToanService.InsertAsync(model);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(HinhThucThanhToanViewModel model)
        {
            var result = await _HinhThucThanhToanService.UpdateAsync(model);
            return Ok(result);
        }

        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _HinhThucThanhToanService.DeleteAsync(id);
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

        [HttpPost("ExportExcel")]
        public async Task<IActionResult> ExportExcel(HinhThucThanhToanParams @params)
        {
            var result = await _HinhThucThanhToanService.ExportExcelAsync(@params);
            return File(result.Bytes, result.ContentType, result.FileName);
        }
    }
}
