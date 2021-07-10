using DLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Helper.Params.DanhMuc;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
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

        [HttpPost("CheckTrungMa")]
        public async Task<IActionResult> CheckTrungMa(ThongBaoDieuChinhThongTinHoaDonViewModel model)
        {
            var result = await _thongBaoDieuChinhThongTinHoaDonService.CheckTrungMaAsync(model);
            return Ok(result);
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert(ThongBaoDieuChinhThongTinHoaDonViewModel model)
        {
            var result = await _thongBaoDieuChinhThongTinHoaDonService.InsertAsync(model);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(ThongBaoDieuChinhThongTinHoaDonViewModel model)
        {
            var result = await _thongBaoDieuChinhThongTinHoaDonService.UpdateAsync(model);
            return Ok(result);
        }

        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _thongBaoDieuChinhThongTinHoaDonService.DeleteAsync(id);
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
