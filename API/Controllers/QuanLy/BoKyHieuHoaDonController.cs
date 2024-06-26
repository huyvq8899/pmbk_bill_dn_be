﻿using DLL;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Services.Helper.Params.QuanLy;
using Services.Repositories.Interfaces.QuanLy;
using Services.ViewModels.QuanLy;
using Services.ViewModels.QuanLyHoaDonDienTu;
using System;
using System.Collections.Generic;
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

        [HttpPost("GetListForPhatHanhDongLoat")]
        public async Task<IActionResult> GetListForPhatHanhDongLoat(PagingParams param)
        {
            var result = await _boKyHieuHoaDonService.GetListForPhatHanhDongLoatAsync(param);
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
        
        [HttpPost("GetMultiChungThuSoById")]
        public async Task<IActionResult> GetMultiChungThuSoById(List<string> listId)
        {
            var result = await _boKyHieuHoaDonService.GetMultiChungThuSoByIdAsync(listId);
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

        [HttpGet("HasChuyenTheoBangTongHopDuLieuHDDT/{id}")]
        public async Task<IActionResult> HasChuyenTheoBangTongHopDuLieuHDDT(string id)
        {
            var result = await _boKyHieuHoaDonService.HasChuyenTheoBangTongHopDuLieuHDDTAsync(id);
            return Ok(result);
        }

        [HttpGet("GetNhatKyXacThucBoKyHieuIdForXemMauHoaDon/{id}")]
        public async Task<IActionResult> GetNhatKyXacThucBoKyHieuIdForXemMauHoaDon(string id)
        {
            var result = await _boKyHieuHoaDonService.GetNhatKyXacThucBoKyHieuIdForXemMauHoaDonAsync(id);
            return Ok(new { result });
        }

        [HttpPost("CheckHasToKhaiMoiNhat")]
        public async Task<IActionResult> CheckHasToKhaiMoiNhat(BoKyHieuHoaDonViewModel model)
        {
            var result = await _boKyHieuHoaDonService.CheckHasToKhaiMoiNhatAsync(model);
            return Ok(new { result });
        }

        [HttpGet("GetThongTinTuToKhaiMoiNhat")]
        public async Task<IActionResult> GetThongTinTuToKhaiMoiNhat()
        {
            var result = await _boKyHieuHoaDonService.GetThongTinTuToKhaiMoiNhatAsync();
            return Ok(result);
        }

        [HttpGet("CheckTrangThaiSuDungTruocKhiSua/{id}")]
        public async Task<IActionResult> CheckTrangThaiSuDungTruocKhiSua(string id)
        {
            var result = await _boKyHieuHoaDonService.CheckTrangThaiSuDungTruocKhiSuaAsync(id);
            return Ok(result);
        }

        [HttpPost("CheckBoKyHieuDangPhatHanh")]
        public async Task<IActionResult> CheckBoKyHieuDangPhatHanh(List<string> boKyHieuHoaDonIds)
        {
            var result = await _boKyHieuHoaDonService.CheckBoKyHieuDangPhatHanhAsync(boKyHieuHoaDonIds);
            return Ok(new { message = result });
        }

        [HttpDelete("ClearBoKyHieuDaPhatHanh")]
        public async Task<IActionResult> ClearBoKyHieuDaPhatHanh()
        {
            var result = await _boKyHieuHoaDonService.ClearBoKyHieuDaPhatHanhAsync();
            return Ok(result);
        }

        [HttpPost("CheckDaHetSoLuongHoaDonVaXacThuc")]
        public async Task<IActionResult> CheckDaHetSoLuongHoaDonVaXacThuc(HoaDonDienTuViewModel hoaDon)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    await _boKyHieuHoaDonService.CheckDaHetSoLuongHoaDonVaXacThucAsync(hoaDon);
                    transaction.Commit();
                    return Ok(true);
                }
                catch (Exception)
                {
                    return Ok(false);
                }
            }
        }

        /// <summary>
        /// API Call Get List BoKyHieuHoaDon từ máy post
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("GetListBoKyHieuForHoaDonCMMTTienAsync")]
        public async Task<IActionResult> GetListBoKyHieuForHoaDonCMMTTienAsync(BoKyHieuHoaDonViewModel model)
        {
            var result = await _boKyHieuHoaDonService.GetListBoKyHieuForHoaDonCMMTTienAsync(model);
            return Ok(result);
        }
        /// <summary>
        /// Trả về danh sách BoKyHieuHoaDon với hình thức hóa đơn có mã từ MTT
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("PosGetBoKyHieu")]
        public async Task<IActionResult> PosGetBoKyHieu()
        {
            var result = await _boKyHieuHoaDonService.PosGetBoKyHieuMTT();
            return Ok(result);
        }
        /// <summary>
        /// Trả về danh sách BoKyHieuHoaDon tất cả
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("PosGetAllBoKyHieu")]
        public async Task<IActionResult> PosGetAllBoKyHieu()
        {
            var result = await _boKyHieuHoaDonService.PosGetAllBoKyHieu();
            return Ok(result);
        }

        [HttpGet("CheckUsedBoKyHieuHoaDon")]
        public IActionResult CheckUsedBoKyHieuHoaDon(string Id)
        {
            var result = _boKyHieuHoaDonService.CheckUsedBoKyHieuHoaDon(Id);
            return Ok(result);
        }
    }
}
