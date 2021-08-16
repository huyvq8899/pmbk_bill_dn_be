using DLL;
using Microsoft.AspNetCore.Mvc;
using Services.Repositories.Interfaces.Config;
using Services.ViewModels.BaoCao;
using Services.ViewModels.Config;
using Services.ViewModels.QuanLyHoaDonDienTu;
using System;
using System.Collections.Generic;
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

        [HttpGet("GetAllNoiDungEmail")]
        public async Task<IActionResult> GetAllNoiDungEmail()
        {
            var result = await _tuyChonService.GetAllNoiDungEmail();
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

        [HttpPost("UpdateRangeNoiDungEmailAsync")]
        public async Task<IActionResult> UpdateRangeNoiDungEmailAsync(List<ConfigNoiDungEmailViewModel> models)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var rs = await _tuyChonService.UpdateRangeNoiDungEmailAsync(models);
                    transaction.Commit();

                    return Ok(rs);
                }
                catch (Exception ex)
                {
                    return Ok(false);
                }
            }
        }

        [HttpGet("GetThongTinHienThiTruongDL/{tenChucNang}")]
        public async Task<IActionResult> GetThongTinHienThiTruongDL(string tenChucNang)
        {
            if (string.IsNullOrEmpty(tenChucNang))
            {
                return BadRequest();
            }

            var result = await _tuyChonService.GetThongTinHienThiTruongDL(tenChucNang);

            return Ok(result);
        }

        [HttpPost("UpdateHienThiTruongDuLieu")]
        public async Task<IActionResult> UpdateHienThiTruongDuLieu(List<TruongDuLieuViewModel> models)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var rs = await _tuyChonService.UpdateHienThiTruongDuLieu(models);
                    transaction.Commit();

                    return Ok(rs);
                }
                catch (Exception ex)
                {
                    return Ok(false);
                }
            }
        }

        [HttpGet("GetThongTinHienThiTruongDLHoaDon")]
        public async Task<IActionResult> GetThongTinHienThiTruongDLHoaDon([FromQuery] bool isChiTiet)
        {
            var result = await _tuyChonService.GetThongTinHienThiTruongDLHoaDon(isChiTiet);

            return Ok(result);
        }

        [HttpPost("UpdateHienThiTruongDuLieu")]
        public async Task<IActionResult> UpdateHienThiTruongDuLieuHoaDon(List<TruongDuLieuHoaDonViewModel> models)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var rs = await _tuyChonService.UpdateHienThiTruongDuLieuHoaDon(models);
                    transaction.Commit();

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
