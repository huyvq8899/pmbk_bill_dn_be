using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Helper.Params.DanhMuc;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
using System;
using System.Threading.Tasks;

namespace API.Controllers.DanhMuc
{
    public class ThongBaoPhatHanhController : BaseController
    {
        private readonly IThongBaoPhatHanhService _thongBaoPhatHanhService;
        public ThongBaoPhatHanhController(IThongBaoPhatHanhService thongBaoPhatHanhService)
        {
            _thongBaoPhatHanhService = thongBaoPhatHanhService;
        }

        [HttpPost("GetAllPaging")]
        public async Task<IActionResult> GetAllPaging(ThongBaoPhatHanhParams pagingParams)
        {
            var paged = await _thongBaoPhatHanhService.GetAllPagingAsync(pagingParams);
            return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
        }

        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _thongBaoPhatHanhService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("CheckTrungMa")]
        public async Task<IActionResult> CheckTrungMa(ThongBaoPhatHanhViewModel model)
        {
            var result = await _thongBaoPhatHanhService.CheckTrungMaAsync(model);
            return Ok(result);
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert(ThongBaoPhatHanhViewModel model)
        {
            var result = await _thongBaoPhatHanhService.InsertAsync(model);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(ThongBaoPhatHanhViewModel model)
        {
            var result = await _thongBaoPhatHanhService.UpdateAsync(model);
            return Ok(result);
        }

        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _thongBaoPhatHanhService.DeleteAsync(id);
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
