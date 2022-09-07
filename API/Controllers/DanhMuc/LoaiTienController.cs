using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Helper.Params.DanhMuc;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers.DanhMuc
{
    public class LoaiTienController : BaseController
    {
        private readonly ILoaiTienService _loaiTienService;
        public LoaiTienController(ILoaiTienService loaiTienService)
        {
            _loaiTienService = loaiTienService;
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll(LoaiTienParams @params)
        {
            var result = await _loaiTienService.GetAllAsync(@params);
            return Ok(result);
        }

        [HttpPost("GetAllPaging")]
        public async Task<IActionResult> GetAllPaging(LoaiTienParams pagingParams)
        {
            var paged = await _loaiTienService.GetAllPagingAsync(pagingParams);
            return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
        }

        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _loaiTienService.GetByIdAsync(id);
            return Ok(result);
        }
        /// <summary>
        /// Kiểm tra xem đã phát sinh trong hóa đơn chưa?
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("CheckPhatSinh/{Id}")]
        public async Task<IActionResult> CheckPhatSinh(string id)
        {
            var result = await _loaiTienService.CheckPhatSinhAsync(id);
            return Ok(result);
        }

        [HttpPost("CheckTrungMa")]
        public async Task<IActionResult> CheckTrungMa(LoaiTienViewModel model)
        {
            var result = await _loaiTienService.CheckTrungMaAsync(model);
            return Ok(result);
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert(LoaiTienViewModel model)
        {
            var result = await _loaiTienService.InsertAsync(model);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(LoaiTienViewModel model)
        {
            var result = await _loaiTienService.UpdateAsync(model);
            return Ok(result);
        }

        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _loaiTienService.DeleteAsync(id);
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
        public async Task<IActionResult> ExportExcel(LoaiTienParams @params)
        {
            var result = await _loaiTienService.ExportExcelAsync(@params);
            return File(result.Bytes, result.ContentType, result.FileName);
        }

        [HttpPut("UpdateRange")]
        public async Task<IActionResult> UpdateRange(List<LoaiTienViewModel> models)
        {
            var result = await _loaiTienService.UpdateRangeAsync(models);
            return Ok(result);
        }
    }
}
