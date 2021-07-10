using DLL.Enums;
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

        [HttpPost("GetListMauHoaDon")]
        public IActionResult GetListMauHoaDon(MauHoaDonParams pagingParams)
        {
            var result = _mauHoaDonService.GetListMauHoaDon(pagingParams);
            return Ok(result);
        }

        [HttpGet("GetListMauDaDuocChapNhan")]
        public async Task<IActionResult> GetListMauDaDuocChapNhan()
        {
            var result = await _mauHoaDonService.GetListMauDaDuocChapNhanAsync();
            return Ok(result);
        }

        [HttpGet("GetListQuyDinhApDung")]
        public IActionResult GetListQuyDinhApDung()
        {
            var result = _mauHoaDonService.GetListQuyDinhApDung();
            return Ok(result);
        }

        [HttpGet("GetListLoaiHoaDon")]
        public IActionResult GetListLoaiHoaDon()
        {
            var result = _mauHoaDonService.GetListLoaiHoaDon();
            return Ok(result);
        }

        [HttpGet("GetListLoaiMau")]
        public IActionResult GetListLoaiMau()
        {
            var result = _mauHoaDonService.GetListLoaiMau();
            return Ok(result);
        }

        [HttpGet("GetListLoaiThueGTGT")]
        public IActionResult GetListLoaiThueGTGT()
        {
            var result = _mauHoaDonService.GetListLoaiThueGTGT();
            return Ok(result);
        }

        [HttpGet("GetListLoaiNgonNgu")]
        public IActionResult GetListLoaiNgonNgu()
        {
            var result = _mauHoaDonService.GetListLoaiNgonNgu();
            return Ok(result);
        }

        [HttpGet("GetListLoaiKhoGiay")]
        public IActionResult GetListLoaiKhoGiay()
        {
            var result = _mauHoaDonService.GetListLoaiKhoGiay();
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
