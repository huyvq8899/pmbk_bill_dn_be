using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Helper.Params.DanhMuc;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
using System;
using System.Threading.Tasks;

namespace API.Controllers.DanhMuc
{
    public class DoiTuongController : BaseController
    {
        private readonly IDoiTuongService _doiTuongService;
        public DoiTuongController(IDoiTuongService doiTuongService)
        {
            _doiTuongService = doiTuongService;
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll(DoiTuongParams @params)
        {
            var result = await _doiTuongService.GetAllAsync(@params);
            return Ok(result);
        }

        [HttpPost("GetAllPaging")]
        public async Task<IActionResult> GetAllPaging(DoiTuongParams pagingParams)
        {
            var paged = await _doiTuongService.GetAllPagingAsync(pagingParams);
            return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
        }

        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _doiTuongService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("GetAllKhachHang")]
        public async Task<IActionResult> GetAllKhachHang()
        {
            var result = await _doiTuongService.GetAllKhachHang();
            return Ok(result);
        }

        [HttpGet("GetAllNhanVien")]
        public async Task<IActionResult> GetAllNhanVien()
        {
            var result = await _doiTuongService.GetAllNhanVien();
            return Ok(result);
        }

        [HttpPost("CheckTrungMa")]
        public async Task<IActionResult> CheckTrungMa(DoiTuongViewModel model)
        {
            var result = await _doiTuongService.CheckTrungMaAsync(model);
            return Ok(result);
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert(DoiTuongViewModel model)
        {
            var result = await _doiTuongService.InsertAsync(model);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(DoiTuongViewModel model)
        {
            var result = await _doiTuongService.UpdateAsync(model);
            return Ok(result);
        }

        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _doiTuongService.DeleteAsync(id);
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
