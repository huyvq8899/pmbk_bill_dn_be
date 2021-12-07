using DLL;
using Microsoft.AspNetCore.Mvc;
using Services.Repositories.Interfaces.Config;
using Services.ViewModels.BaoCao;
using Services.ViewModels.Config;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers.Config
{
    public class TuyChonController : BaseController
    {
        private readonly ITuyChonService _tuyChonService;
        private readonly Datacontext _db;

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

        [HttpPost("LayLaiThietLapEmailMacDinh")]
        public async Task<IActionResult> LayLaiThietLapMacDinh(int LoaiEmail)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var rs = await _tuyChonService.LayLaiThietLapEmailMacDinh(LoaiEmail);
                    transaction.Commit();

                    return Ok(rs);
                }
                catch (Exception)
                {
                    return Ok(false);
                }
            }
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
                catch (Exception)
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
                catch (Exception)
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
                catch (Exception)
                {
                    return Ok(false);
                }
            }
        }

        [HttpGet("CheckCoPhatSinhNgoaiTe")]
        public async Task<IActionResult> CheckCoPhatSinhNgoaiTe()
        {
            var result = await _tuyChonService.CheckCoPhatSinhNgoaiTeAsync();
            return Ok(result);
        }
    }
}
