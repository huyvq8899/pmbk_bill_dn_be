﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Helper.Params.DanhMuc;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
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