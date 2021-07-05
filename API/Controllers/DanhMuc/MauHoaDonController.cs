using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Helper.Params.DanhMuc;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
using System;
using System.Threading.Tasks;

namespace API.Controllers.DanhMuc
{
    public class MauHoaDonController : BaseController
    {
        private readonly IMauHoaDonService _mauHoaDonService;

        public MauHoaDonController(IMauHoaDonService mauHoaDonService)
        {
            _mauHoaDonService = mauHoaDonService;
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll(MauHoaDonParams @params)
        {
            var result = await _mauHoaDonService.GetAllAsync(@params);
            return Ok(result);
        }

        [HttpPost("GetAllPaging")]
        public async Task<IActionResult> GetAllPaging(MauHoaDonParams pagingParams)
        {
            var paged = await _mauHoaDonService.GetAllPagingAsync(pagingParams);
            return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
        }

        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _mauHoaDonService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("GetMauHoaDonBackgrounds")]
        public IActionResult GetMauHoaDonBackgrounds()
        {
            var result = _mauHoaDonService.GetMauHoaDonBackgrounds();
            return Ok(result);
        }

        //[HttpPost("CheckTrungMa")]
        //public async Task<IActionResult> CheckTrungMa(DoiTuongViewModel model)
        //{
        //    //var result = await _doiTuongService.CheckTrungMaAsync(model);
        //    //return Ok(result);

        //    return Ok(true);
        //}

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert(MauHoaDonViewModel model)
        {
            var result = await _mauHoaDonService.InsertAsync(model);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(MauHoaDonViewModel model)
        {
            var result = await _mauHoaDonService.UpdateAsync(model);
            return Ok(result);
        }

        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _mauHoaDonService.DeleteAsync(id);
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
