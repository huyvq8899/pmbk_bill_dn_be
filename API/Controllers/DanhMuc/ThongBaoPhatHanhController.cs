using DLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Services.Helper.Params.DanhMuc;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
using System;
using System.Threading.Tasks;

namespace API.Controllers.DanhMuc
{
    public class ThongBaoPhatHanhController : BaseController
    {
        private readonly Datacontext _db;
        private readonly IThongBaoPhatHanhService _thongBaoPhatHanhService;
        public ThongBaoPhatHanhController(Datacontext datacontext, IThongBaoPhatHanhService thongBaoPhatHanhService)
        {
            _db = datacontext;
            _thongBaoPhatHanhService = thongBaoPhatHanhService;
        }

        [HttpGet("GetTrangThaiNops")]
        public IActionResult GetTrangThaiNops()
        {
            var result = _thongBaoPhatHanhService.GetTrangThaiNops();
            return Ok(result);
        }

        [HttpPost("GetAllPaging")]
        public async Task<IActionResult> GetAllPaging(ThongBaoPhatHanhParams pagingParams)
        {
            var paged = await _thongBaoPhatHanhService.GetAllPagingAsync(pagingParams);
            return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
        }

        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(string Id)
        {
            var result = await _thongBaoPhatHanhService.GetByIdAsync(Id);
            return Ok(result);
        }

        [HttpGet("GetListChiTietThongBaoPhatHanhByMauHoaDonId/{Id}")]
        public async Task<IActionResult> GetListChiTietThongBaoPhatHanhByMauHoaDonId(string Id)
        {
            var result = await _thongBaoPhatHanhService.GetListChiTietThongBaoPhatHanhByMauHoaDonIdAsync(Id);
            return Ok(result);
        }

        [HttpGet("GetThongBaoPhatHanhChiTietById/{Id}")]
        public async Task<IActionResult> GetThongBaoPhatHanhChiTietById(string Id)
        {
            var result = await _thongBaoPhatHanhService.GetThongBaoPhatHanhChiTietByIdAsync(Id);
            return Ok(result);
        }

        [HttpGet("GetCacLoaiHoaDonPhatHanhs")]
        public async Task<IActionResult> GetCacLoaiHoaDonPhatHanhs([FromQuery] string Id)
        {
            var result = await _thongBaoPhatHanhService.GetCacLoaiHoaDonPhatHanhsAsync(Id);
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
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _thongBaoPhatHanhService.InsertAsync(model);
                    transaction.Commit();
                    return Ok(result);
                }
                catch (Exception e)
                {
                    return Ok(null);
                }
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(ThongBaoPhatHanhViewModel model)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _thongBaoPhatHanhService.UpdateAsync(model);
                    transaction.Commit();
                    return Ok(result);
                }
                catch (Exception e)
                {
                    return Ok(false);
                }
            }
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
