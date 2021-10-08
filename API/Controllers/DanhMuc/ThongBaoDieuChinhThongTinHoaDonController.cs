﻿using DLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Services.Helper;
using Services.Helper.Params.DanhMuc;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.FormActions;
using System;
using System.Threading.Tasks;

namespace API.Controllers.DanhMuc
{
    public class ThongBaoDieuChinhThongTinHoaDonController : BaseController
    {
        private readonly Datacontext _db;
        private readonly IThongBaoDieuChinhThongTinHoaDonService _thongBaoDieuChinhThongTinHoaDonService;
        public ThongBaoDieuChinhThongTinHoaDonController(Datacontext datacontext, IThongBaoDieuChinhThongTinHoaDonService thongBaoDieuChinhThongTinHoaDonService)
        {
            _db = datacontext;
            _thongBaoDieuChinhThongTinHoaDonService = thongBaoDieuChinhThongTinHoaDonService;
        }

        [HttpGet("GetBangKeHoaDonChuaSuDung")]
        public async Task<IActionResult> GetBangKeHoaDonChuaSuDung([FromQuery] string id)
        {
            var result = await _thongBaoDieuChinhThongTinHoaDonService.GetBangKeHoaDonChuaSuDungAsync(id);
            return Ok(result);
        }

        [HttpGet("GetTrangThaiHieuLucs")]
        public IActionResult GetTrangThaiHieuLucs()
        {
            var result = _thongBaoDieuChinhThongTinHoaDonService.GetTrangThaiHieuLucs();
            return Ok(result);
        }

        [HttpPost("GetAllPaging")]
        public async Task<IActionResult> GetAllPaging(ThongBaoDieuChinhThongTinHoaDonParams pagingParams)
        {
            var paged = await _thongBaoDieuChinhThongTinHoaDonService.GetAllPagingAsync(pagingParams);
            return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
        }

        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(string Id)
        {
            var result = await _thongBaoDieuChinhThongTinHoaDonService.GetByIdAsync(Id);
            return Ok(result);
        }

        [HttpGet("GetThongBaoDieuChinhThongTinChiTietById/{Id}")]
        public async Task<IActionResult> GetThongBaoDieuChinhThongTinChiTietById(string Id)
        {
            var result = await _thongBaoDieuChinhThongTinHoaDonService.GetThongBaoDieuChinhThongTinChiTietByIdAsync(Id);
            return Ok(result);
        }

        [HttpPost("CheckTrungMa")]
        public async Task<IActionResult> CheckTrungMa(ThongBaoDieuChinhThongTinHoaDonViewModel model)
        {
            var result = await _thongBaoDieuChinhThongTinHoaDonService.CheckTrungMaAsync(model);
            return Ok(result);
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert(ThongBaoDieuChinhThongTinHoaDonViewModel model)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _thongBaoDieuChinhThongTinHoaDonService.InsertAsync(model);
                    transaction.Commit();
                    return Ok(result);
                }
                catch (Exception)
                {
                    return Ok(null);
                }
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(ThongBaoDieuChinhThongTinHoaDonViewModel model)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _thongBaoDieuChinhThongTinHoaDonService.UpdateAsync(model);
                    transaction.Commit();
                    return Ok(result);
                }
                catch (Exception)
                {
                    return Ok(false);
                }
            }
        }

        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> Delete(string id)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _thongBaoDieuChinhThongTinHoaDonService.DeleteAsync(id);
                    transaction.Commit();
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
        }

        [HttpGet("ExportFile/{id}/{dinhDangTepMau}/{loai}")]
        public async Task<IActionResult> ExportFile(string id, DinhDangTepMau dinhDangTepMau, int loai)
        {
            var result = await _thongBaoDieuChinhThongTinHoaDonService.ExportFileAsync(id, dinhDangTepMau, loai);
            return File(result.Bytes, result.ContentType, result.FileName);
        }

        [HttpPost("TienLuiChungTu")]
        public async Task<IActionResult> TienLuiChungTu(TienLuiViewModel model)
        {
            var result = await _thongBaoDieuChinhThongTinHoaDonService.TienLuiChungTuAsync(model);
            return Ok(result);
        }
    }
}
