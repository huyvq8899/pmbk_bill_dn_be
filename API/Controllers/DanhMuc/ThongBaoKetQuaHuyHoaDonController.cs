using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Helper.Params.DanhMuc;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
using System;
using System.Threading.Tasks;

namespace API.Controllers.DanhMuc
{
    public class ThongBaoKetQuaHuyHoaDonController : BaseController
    {
        private readonly IThongBaoKetQuaHuyHoaDonService _thongBaoKetQuaHuyHoaDonService;
        public ThongBaoKetQuaHuyHoaDonController(IThongBaoKetQuaHuyHoaDonService thongBaoKetQuaHuyHoaDonService)
        {
            _thongBaoKetQuaHuyHoaDonService = thongBaoKetQuaHuyHoaDonService;
        }

        [HttpPost("GetAllPaging")]
        public async Task<IActionResult> GetAllPaging(ThongBaoKetQuaHuyHoaDonParams pagingParams)
        {
            var paged = await _thongBaoKetQuaHuyHoaDonService.GetAllPagingAsync(pagingParams);
            return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
        }

        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(string Id)
        {
            var result = await _thongBaoKetQuaHuyHoaDonService.GetByIdAsync(Id);
            return Ok(result);
        }

        [HttpPost("CheckTrungMa")]
        public async Task<IActionResult> CheckTrungMa(ThongBaoKetQuaHuyHoaDonViewModel model)
        {
            var result = await _thongBaoKetQuaHuyHoaDonService.CheckTrungMaAsync(model);
            return Ok(result);
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert(ThongBaoKetQuaHuyHoaDonViewModel model)
        {
            var result = await _thongBaoKetQuaHuyHoaDonService.InsertAsync(model);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(ThongBaoKetQuaHuyHoaDonViewModel model)
        {
            var result = await _thongBaoKetQuaHuyHoaDonService.UpdateAsync(model);
            return Ok(result);
        }

        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _thongBaoKetQuaHuyHoaDonService.DeleteAsync(id);
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
