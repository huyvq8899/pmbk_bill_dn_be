using DLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Services.Helper.Params.QuanLy;
using Services.Repositories.Interfaces.QuanLy;
using Services.ViewModels.QuanLy;
using System;
using System.Threading.Tasks;

namespace API.Controllers.QuanLy
{
    public class BoKyHieuHoaDonController : BaseController
    {
        private readonly IBoKyHieuHoaDonService _boKyHieuHoaDonService;
        private readonly Datacontext _db;
        public BoKyHieuHoaDonController(IBoKyHieuHoaDonService boKyHieuHoaDonService, Datacontext db)
        {
            _boKyHieuHoaDonService = boKyHieuHoaDonService;
            _db = db;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _boKyHieuHoaDonService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost("GetAllPaging")]
        public async Task<IActionResult> GetAllPaging(BoKyHieuHoaDonParams pagingParams)
        {
            var paged = await _boKyHieuHoaDonService.GetAllPagingAsync(pagingParams);
            return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _boKyHieuHoaDonService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("GetListByMauHoaDonId/{id}")]
        public async Task<IActionResult> GetListByMauHoaDonId(string id)
        {
            var result = await _boKyHieuHoaDonService.GetListByMauHoaDonIdAsync(id);
            return Ok(result);
        }

        [HttpGet("GetListNhatKyXacThucById/{id}")]
        public async Task<IActionResult> GetListNhatKyXacThucById(string id)
        {
            var result = await _boKyHieuHoaDonService.GetListNhatKyXacThucByIdAsync(id);
            return Ok(result);
        }

        [HttpPost("CheckTrungKyHieu")]
        public async Task<IActionResult> CheckTrungKyHieu(BoKyHieuHoaDonViewModel model)
        {
            var result = await _boKyHieuHoaDonService.CheckTrungKyHieuAsync(model);
            return Ok(result);
        }

        [HttpPost("GetListForHoaDon")]
        public async Task<IActionResult> GetListForHoaDon(BoKyHieuHoaDonViewModel model)
        {
            var result = await _boKyHieuHoaDonService.GetListForHoaDonAsync(model);
            return Ok(result);
        }

        [HttpGet("KiemTraXacThuc/{boKyHieuId}")]
        public async Task<IActionResult> KiemTraXacThuc(string boKyHieuId)
        {
            var result = await _boKyHieuHoaDonService.KiemTraHieuLucBoKyHieu(boKyHieuId);
            return Ok(result);
        }


        [HttpGet("GetSoSeriChungThuById/{id}")]
        public async Task<IActionResult> GetSoSeriChungThuById(string id)
        {
            var result = await _boKyHieuHoaDonService.GetSoSeriChungThuByIdAsync(id);
            return Ok(new { result });
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert(BoKyHieuHoaDonViewModel model)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _boKyHieuHoaDonService.InsertAsync(model);
                    transaction.Commit();
                    return Ok(result);
                }
                catch (Exception)
                {
                    return Ok(false);
                }
            }
        }

        [HttpPost("XacThucBoKyHieuHoaDon")]
        public async Task<IActionResult> XacThucBoKyHieuHoaDon(NhatKyXacThucBoKyHieuViewModel model)
        {
            var result = await _boKyHieuHoaDonService.XacThucBoKyHieuHoaDonAsync(model);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(BoKyHieuHoaDonViewModel model)
        {
            var result = await _boKyHieuHoaDonService.UpdateAsync(model);
            return Ok(result);
        }

        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _boKyHieuHoaDonService.DeleteAsync(id);
                return Ok(result);
            }
            catch (DbUpdateException)
            {
                return Ok(new
                {
                    result = "DbUpdateException",
                    value = false
                });
            }
            catch (Exception)
            {
                return Ok(false);
            }
        }

        [HttpPost("CheckSoSeriChungThu")]
        public IActionResult CheckSoSeriChungThu(BoKyHieuHoaDonViewModel model)
        {
            var result = _boKyHieuHoaDonService.CheckSoSeriChungThu(model);
            return Ok(result);
        }

        [HttpPost("CheckThoiHanChungThuSo")]
        public async Task<IActionResult> CheckThoiHanChungThuSo(BoKyHieuHoaDonViewModel model)
        {
            var result = await _boKyHieuHoaDonService.CheckThoiHanChungThuSoAsync(model);
            return Ok(result);
        }

        [HttpGet("GetChungThuSoById/{id}")]
        public async Task<IActionResult> GetChungThuSoById(string id)
        {
            var result = await _boKyHieuHoaDonService.GetChungThuSoByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("CheckDaKySoBatDau/{id}")]
        public async Task<IActionResult> CheckDaKySoBatDau(string id)
        {
            var result = await _boKyHieuHoaDonService.CheckDaKySoBatDauAsync(id);
            return Ok(result);
        }
    }
}
