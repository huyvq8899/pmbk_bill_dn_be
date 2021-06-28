using API.Extentions;
using DLL;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Services.Enums;
using Services.Repositories.Interfaces;
using Services.Repositories.Interfaces.QuanLyHoaDon;
using Services.ViewModels.QuanLyHoaDonDienTu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.QuanLyHoaDon
{
    public class HoaDonDienTuController : BaseController
    {
        IHoaDonDienTuService _hoaDonDienTuService;
        IHoaDonDienTuChiTietService _hoaDonDienTuChiTietService;
        //IThamChieuService _thamChieuService;
        Datacontext _db;

        public HoaDonDienTuController(
            IHoaDonDienTuService hoaDonDienTuService,
            IHoaDonDienTuChiTietService hoaDonDienTuChiTietService,
            //IThamChieuService thamChieuService,
            Datacontext db
        )
        {
            _hoaDonDienTuService = hoaDonDienTuService;
            _hoaDonDienTuChiTietService = hoaDonDienTuChiTietService;
            //_thamChieuService = thamChieuService;
            _db = db;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _hoaDonDienTuService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("GetAllPaging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] HoaDonParams pagingParams)
        {
            var paged = await _hoaDonDienTuService.GetAllPagingAsync(pagingParams);
            Response.AddPagination(paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages);
            return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
        }

        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _hoaDonDienTuService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("CheckSoHoaDon")]
        public async Task<IActionResult> CheckSoHoaDon(string soHoaDon)
        {
            var result = await _hoaDonDienTuService.CheckSoHoaDonAsync(soHoaDon);
            return Ok(result);
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert(HoaDonDienTuViewModel model)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    List<HoaDonDienTuChiTietViewModel> hoaDonDienTuChiTiets = model.HoaDonChiTiets;
                    model.TongTienThanhToan = hoaDonDienTuChiTiets.Sum(x => x.ThanhTien + x.TienThueGTGT);

                    HoaDonDienTuViewModel result = await _hoaDonDienTuService.InsertAsync(model);
                    await _hoaDonDienTuChiTietService.InsertRangeAsync(result, hoaDonDienTuChiTiets);

                    //tham chiếu
                    //if (model.LoaiHoaDon == (int)LoaiHoaDonDienTu.HOA_DON_GIA_TRI_GIA_TANG)
                    //    await _thamChieuService.UpdateRangeAsync(result.HoaDonDienTuId, result.SoHoaDon, BusinessOfType.HOA_DON_GIA_TRI_GIA_TANG, model.ThamChieus);
                    //else await _thamChieuService.UpdateRangeAsync(result.HoaDonDienTuId, result.SoHoaDon, BusinessOfType.HOA_DON_BAN_HANG, model.ThamChieus);
                    
                    //
                    transaction.Commit();
                    return Ok(true);
                }
                catch (Exception ex)
                {
                    return Ok(false);
                }
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(HoaDonDienTuViewModel model)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    await _hoaDonDienTuChiTietService.RemoveRangeAsync(model.HoaDonDienTuId);
                    await _hoaDonDienTuChiTietService.InsertRangeAsync(model, model.HoaDonChiTiets);


                    //tham chiếu
                    //if (model.LoaiHoaDon == (int)LoaiHoaDonDienTu.HOA_DON_GIA_TRI_GIA_TANG)
                    //    await _thamChieuService.UpdateRangeAsync(model.HoaDonDienTuId, model.SoHoaDon, BusinessOfType.HOA_DON_GIA_TRI_GIA_TANG, model.ThamChieus);
                    //else await _thamChieuService.UpdateRangeAsync(model.HoaDonDienTuId, model.SoHoaDon, BusinessOfType.HOA_DON_BAN_HANG, model.ThamChieus);

                    //

                    bool result = await _hoaDonDienTuService.UpdateAsync(model);
                    transaction.Commit();
                    return Ok(result);
                }
                catch (Exception)
                {
                    throw;
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
                    await _hoaDonDienTuChiTietService.RemoveRangeAsync(id);
                    //await _thamChieuService.DeleteRangeAsync(id);

                    bool result = await _hoaDonDienTuService.DeleteAsync(id);
                    transaction.Commit();
                    return Ok(result);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        //[HttpGet("CreateSoChungTu")]
        //public async Task<IActionResult> CreateSoChungTu()
        //{
        //    string result = await _hoaDonDienTuService.CreateSoChungTuAsync();
        //    return Ok(result);
        //}

        [HttpGet("GetTrangThaiHoaDon")]
        public async Task<IActionResult> GetTrangThaiHoaDon()
        {
            var result = await _hoaDonDienTuService.GetTrangThaiHoaDon();
            return Ok(result);
        }

        [HttpGet("GetTrangThaiGuiHoaDon")]
        public async Task<IActionResult> GetTrangThaiGuiHoaDon()
        {
            var result = await _hoaDonDienTuService.GetTrangThaiGuiHoaDon();
            return Ok(result);
        }

        [HttpGet("GetTreeTrangThai")]
        public async Task<IActionResult> GetTreeTrangThai(int LoaiHoaDon, DateTime fromDate, DateTime toDate)
        {
            var result = await _hoaDonDienTuService.GetTreeTrangThai(LoaiHoaDon, fromDate, toDate);
            return Ok(result);
        }

        [HttpPost("DeleteRangeHoaDonDienTu")]
        public async Task<IActionResult> DeleteRangeHoaDonDienTu(List<HoaDonDienTuViewModel> list)
        {
            var result = await _hoaDonDienTuService.DeleteRangeHoaDonDienTuAsync(list);
            return Ok(result);
        }
    }
}
