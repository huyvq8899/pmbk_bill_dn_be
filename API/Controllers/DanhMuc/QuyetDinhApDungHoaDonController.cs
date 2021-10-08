﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Helper;
using Services.Helper.Params.DanhMuc;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.FormActions;
using System;
using System.Threading.Tasks;

namespace API.Controllers.DanhMuc
{
    public class QuyetDinhApDungHoaDonController : BaseController
    {
        private readonly IQuyetDinhApDungHoaDonService _quyetDinhApDungHoaDonService;
        public QuyetDinhApDungHoaDonController(IQuyetDinhApDungHoaDonService quyetDinhApDungHoaDonService)
        {
            _quyetDinhApDungHoaDonService = quyetDinhApDungHoaDonService;
        }

        [HttpPost("GetAllPaging")]
        public async Task<IActionResult> GetAllPaging(QuyetDinhApDungHoaDonParams pagingParams)
        {
            var paged = await _quyetDinhApDungHoaDonService.GetAllPagingAsync(pagingParams);
            return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
        }

        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _quyetDinhApDungHoaDonService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("GetMauCacLoaiHoaDon")]
        public async Task<IActionResult> GetMauCacLoaiHoaDon([FromQuery] string id)
        {
            var result = await _quyetDinhApDungHoaDonService.GetMauCacLoaiHoaDonAsync(id);
            return Ok(result);
        }

        [HttpGet("GetListMauHoaDonById/{id}")]
        public async Task<IActionResult> GetListMauHoaDonById(string id)
        {
            var result = await _quyetDinhApDungHoaDonService.GetListMauHoaDonByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("CheckTrungMa")]
        public async Task<IActionResult> CheckTrungMa(QuyetDinhApDungHoaDonViewModel model)
        {
            var result = await _quyetDinhApDungHoaDonService.CheckTrungMaAsync(model);
            return Ok(result);
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert(QuyetDinhApDungHoaDonViewModel model)
        {
            var result = await _quyetDinhApDungHoaDonService.InsertAsync(model);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(QuyetDinhApDungHoaDonViewModel model)
        {
            var result = await _quyetDinhApDungHoaDonService.UpdateAsync(model);
            return Ok(result);
        }

        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _quyetDinhApDungHoaDonService.DeleteAsync(id);
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

        [HttpGet("ExportFile/{id}/{dinhDangTepMau}")]
        public async Task<IActionResult> ExportFile(string id, DinhDangTepMau dinhDangTepMau)
        {
            var result = await _quyetDinhApDungHoaDonService.ExportFileAsync(id, dinhDangTepMau);
            return File(result.Bytes, result.ContentType, result.FileName);
        }

        [HttpPost("TienLuiChungTu")]
        public async Task<IActionResult> TienLuiChungTu(TienLuiViewModel model)
        {
            var result = await _quyetDinhApDungHoaDonService.TienLuiChungTuAsync(model);
            return Ok(result);
        }
    }
}
