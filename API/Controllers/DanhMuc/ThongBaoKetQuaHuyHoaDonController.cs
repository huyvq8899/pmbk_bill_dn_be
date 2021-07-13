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
    public class ThongBaoKetQuaHuyHoaDonController : BaseController
    {
        private readonly Datacontext _db;
        private readonly IThongBaoKetQuaHuyHoaDonService _thongBaoKetQuaHuyHoaDonService;
        public ThongBaoKetQuaHuyHoaDonController(Datacontext datacontext, IThongBaoKetQuaHuyHoaDonService thongBaoKetQuaHuyHoaDonService)
        {
            _db = datacontext;
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
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _thongBaoKetQuaHuyHoaDonService.InsertAsync(model);
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
        public async Task<IActionResult> Update(ThongBaoKetQuaHuyHoaDonViewModel model)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _thongBaoKetQuaHuyHoaDonService.UpdateAsync(model);
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
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _thongBaoKetQuaHuyHoaDonService.DeleteAsync(id);
                    transaction.Commit();
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
}
