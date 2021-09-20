using DLL;
using DLL.Enums;
using Microsoft.AspNetCore.Mvc;
using Services.Repositories.Interfaces.Config;
using Services.ViewModels.Config;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers.Config
{
    public class ThietLapTruongDuLieuController : BaseController
    {
        private readonly IThietLapTruongDuLieuService _thietLapTruongDuLieuService;
        private readonly Datacontext _db;

        public ThietLapTruongDuLieuController(IThietLapTruongDuLieuService thietLapTruongDuLieuService, Datacontext db)
        {
            _thietLapTruongDuLieuService = thietLapTruongDuLieuService;
            _db = db;
        }

        [HttpGet("GetListTruongDuLieuByLoaiTruong/{loaiTruong}/{loaiHoaDon}")]
        public async Task<IActionResult> GetListTruongDuLieuByLoaiTruong(LoaiTruongDuLieu loaiTruong, LoaiHoaDon loaiHoaDon)
        {
            var result = await _thietLapTruongDuLieuService.GetListTruongDuLieuByLoaiTruongAsync(loaiTruong, loaiHoaDon);
            return Ok(result);
        }

        [HttpGet("GetListThietLapMacDinh/{loaiTruong}/{loaiHoaDon}")]
        public IActionResult GetListThietLapMacDinh(LoaiTruongDuLieu loaiTruong, LoaiHoaDon loaiHoaDon)
        {
            var result = _thietLapTruongDuLieuService.GetListThietLapMacDinh(loaiTruong, loaiHoaDon);
            return Ok(result);
        }

        [HttpPost("CheckDaPhatSinhThongBaoPhatHanh")]
        public async Task<IActionResult> CheckDaPhatSinhThongBaoPhatHanh(ThietLapTruongDuLieuViewModel model)
        {
            var result = await _thietLapTruongDuLieuService.CheckDaPhatSinhThongBaoPhatHanhAsync(model);
            return Ok(result);
        }

        [HttpPut("UpdateRange")]
        public async Task<IActionResult> UpdateRange(List<ThietLapTruongDuLieuViewModel> models)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    await _thietLapTruongDuLieuService.UpdateRangeAsync(models);
                    transaction.Commit();
                    return Ok(true);
                }
                catch (Exception e)
                {
                    return Ok(false);
                }
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(ThietLapTruongDuLieuViewModel model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    await _thietLapTruongDuLieuService.UpdateAsync(model);
                    transaction.Commit();
                    return Ok(true);
                }
                catch (Exception e)
                {
                    return Ok(false);
                }
            }
        }
    }
}
