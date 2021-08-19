using DLL;
using Microsoft.AspNetCore.Http;
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
    public class HangHoaDichVuController : BaseController
    {
        private readonly IHangHoaDichVuService _hangHoaDichVuService;
        private readonly Datacontext _db;
        public HangHoaDichVuController(IHangHoaDichVuService hangHoaDichVuService, Datacontext db)
        {
            _hangHoaDichVuService = hangHoaDichVuService;
            _db = db;
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

        [HttpPost("InsertVTHHImport")]
        public async Task<IActionResult> InsertVTHHImport(List<HangHoaDichVuViewModel> model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    // convert
                    var listData = await _hangHoaDichVuService.ConvertImport(model);
                    int success = 0;
                    foreach (var item in listData)
                    {
                        HangHoaDichVuViewModel result = await _hangHoaDichVuService.InsertAsync(item);
                        if (result != null) success++;
                    }
                    transaction.Commit();
                    return Ok(new
                    {
                        status = true,
                        numDanhMuc = listData.Count,
                        numSuccess = success
                    });
                }
                catch (Exception e)
                {
                    return Ok(false);
                }
            }
        }

        [HttpPost("CreateFileImportVTHHError")]
        public async Task<IActionResult> CreateFileImportVTHHError(List<HangHoaDichVuViewModel> list)
        {
            try
            {
                var result = await _hangHoaDichVuService.CreateFileImportVTHHError(list);
                return Ok(new
                {
                    status = true,
                    link = result
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("ImportVTHH")]
        public async Task<IActionResult> ImportVT(IList<IFormFile> files)
        {
            var result = await _hangHoaDichVuService.ImportVTHH(files);
            return Ok(result);
        }

        [HttpPost("ExportExcel")]
        public async Task<IActionResult> ExportExcel(HangHoaDichVuParams @params)
        {
            var result = await _hangHoaDichVuService.ExportExcelAsync(@params);
            return File(result.Bytes, result.ContentType, result.FileName);
        }
    }
}
