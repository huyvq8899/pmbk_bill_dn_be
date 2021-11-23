using DLL.Entity.QuanLyHoaDon;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Helper.Params.DanhMuc;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
using System;
using System.Threading.Tasks;

namespace API.Controllers.QuanLyHoaDon
{
    public class ThongTinHoaDonController : BaseController
    {
        private readonly IThongTinHoaDonService _thongTinHoaDonService;
        public ThongTinHoaDonController(IThongTinHoaDonService thongTinHoaDonService)
        {
            _thongTinHoaDonService = thongTinHoaDonService;
        }
        
        [HttpPost("Insert")]
        public async Task<IActionResult> Insert(ThongTinHoaDon model)
        {
            var result = await _thongTinHoaDonService.InsertAsync(model);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(ThongTinHoaDon model)
        {
            var result = await _thongTinHoaDonService.UpdateAsync(model);
            return Ok(result);
        }

        [HttpPost("CheckTrungThongTin")]
        public async Task<IActionResult> CheckTrungThongTin(ThongTinHoaDon model)
        {
            var result = await _thongTinHoaDonService.CheckTrungThongTinAsync(model);
            return Ok(result);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(string Id)
        {
            var result = await _thongTinHoaDonService.GetById(Id);
            return Ok(result);
        }
    }
}
