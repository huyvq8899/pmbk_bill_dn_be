using DLL;
using DLL.Entity.DanhMuc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Helper;
using Services.Repositories.Interfaces.DanhMuc;
using Services.ViewModels.DanhMuc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.DanhMuc
{
    public class MauHoaDonController : BaseController
    {
        IMauHoaDonService _IMauHoaDonService;
        public MauHoaDonController(Datacontext datacontext, IMauHoaDonService IMauHoaDonService)
        {
            _IMauHoaDonService = IMauHoaDonService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _IMauHoaDonService.GetAll();
            return Ok(result);
        }

        [HttpGet("GetAllActive")]
        public async Task<IActionResult> GetAllActive()
        {
            var result = await _IMauHoaDonService.GetAllActive();
            return Ok(result);
        }

        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _IMauHoaDonService.GetById(id);
            return Ok(result);
        }


        [HttpGet("GetAllActiveTuyChinh")]
        public async Task<IActionResult> GetAllActiveTuyChinh()
        {
            var result = await _IMauHoaDonService.GetAllTuyChinh();
            return Ok(result);
        }

        [HttpPost("DeleteByMauSo")]
        public async Task<IActionResult> DeleteByMauSo(MauHoaDonViewModel MauSo)
        {
            try
            {
                var result = await _IMauHoaDonService.DeleteByMauSo(MauSo);
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
                throw;
            }
        }

        [HttpPost("CheckTrungMauSo")]
        public async Task<IActionResult> CheckTrungMauSo(MauHoaDonViewModel mauHoaDon)
        {
            var result = await _IMauHoaDonService.CheckTrungMauSo(mauHoaDon.MauSo);
            return Ok(result);
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert(MauHoaDon model)
        {
            var result = await _IMauHoaDonService.Insert(model);
            return Ok(result);
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update(MauHoaDon model)
        {
            var result = await _IMauHoaDonService.Update(model);
            return Ok(result);
        }
        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _IMauHoaDonService.Delete(id);
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
                throw;
            }
        }

        [HttpGet("CheckTrungMa/{Ma}")]              
        public async Task<IActionResult> CheckTrungMa(string Ma)
        {
            var rs = await _IMauHoaDonService.CheckTrungMa(Ma);
            return Ok(rs);
        }

        [HttpPost("CheckTrungMaSo")]
        public async Task<IActionResult> CheckTrungMaSo(MauHoaDonViewModel ma)
        {
            var rs = await _IMauHoaDonService.CheckTrungMa(ma.MauSo);
            return Ok(rs);
        }
    }
}
