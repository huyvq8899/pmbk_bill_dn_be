using API.Extentions;
using DLL;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Services.Helper.Params.QuyDinhKyThuat;
using Services.Repositories.Interfaces.QuyDinhKyThuat;
using Services.ViewModels.QuyDinhKyThuat;
using Services.ViewModels.XML;
using System;
using System.Threading.Tasks;
using Services.Helper;
using DLL.Constants;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace API.Controllers.QuyDinhKyThuat
{
    public class ThongDiepGuiDuLieuHDDTController : BaseController
    {
        private readonly Datacontext _db;
        private readonly IDuLieuGuiHDDTService _thongDiepGuiHDDTKhongMaService;
        private readonly IQuyDinhKyThuatService _IQuyDinhKyThuatService;
        private readonly IBangTongHopService _IBangTongHopService;

        public ThongDiepGuiDuLieuHDDTController(
            Datacontext datacontext,
            IQuyDinhKyThuatService IQuyDinhKyThuatService,
            IDuLieuGuiHDDTService thongDiepGuiHDDTKhongMaService,
            IBangTongHopService IBangTongHopService)
        {
            _db = datacontext;
            _thongDiepGuiHDDTKhongMaService = thongDiepGuiHDDTKhongMaService;
            _IQuyDinhKyThuatService = IQuyDinhKyThuatService;
            _IBangTongHopService = IBangTongHopService;
        }

        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(string Id)
        {
            var result = await _thongDiepGuiHDDTKhongMaService.GetByIdAsync(Id);
            return Ok(result);
        }

        [HttpPost("GetByHoaDonDienTuId")]
        public async Task<IActionResult> GetByHoaDonDienTuId(ThongDiepChungParams pagingParams)
        {
            var paged = await _thongDiepGuiHDDTKhongMaService.GetByHoaDonDienTuIdAsync(pagingParams);
            if (paged != null)
            {
                Response.AddPagination(paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages);
                foreach (var item in paged.Items)
                {
                    if (item.ThongDiepGuiDi == false && item.TrangThaiGui == (TrangThaiGuiThongDiep.ChoPhanHoi))
                    {
                        item.TrangThaiGui = (TrangThaiGuiThongDiep)_IQuyDinhKyThuatService.GetTrangThaiPhanHoiThongDiepNhan(item);
                        item.TenTrangThaiGui = item.TrangThaiGui.GetDescription();
                    }

                    if (item.TrangThaiGui == TrangThaiGuiThongDiep.DaTiepNhan)
                    {
                        if (item.MaLoaiThongDiep == (int)MLTDiep.TBTNToKhai || item.MaLoaiThongDiep == (int)MLTDiep.TDGToKhai || item.MaLoaiThongDiep == (int)MLTDiep.TDGToKhaiUN)
                        {
                            item.TenTrangThaiGui = "CQT đã tiếp nhận";
                        }
                    }

                    if (item.TrangThaiGui == TrangThaiGuiThongDiep.TuChoiTiepNhan)
                    {
                        if (item.MaLoaiThongDiep == (int)MLTDiep.TBTNToKhai || item.MaLoaiThongDiep == (int)MLTDiep.TDGToKhai || item.MaLoaiThongDiep == (int)MLTDiep.TDGToKhaiUN)
                        {
                            item.TenTrangThaiGui = "CQT không tiếp nhận";
                        }
                    }
                }
                return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
            }
            else return Ok(null);
        }

        [HttpGet("GetAllThongDiepTraVeInTransLogs/{id}")]
        public async Task<IActionResult> GetAllThongDiepTraVeInTransLogs(string id)
        {
            var result = await _thongDiepGuiHDDTKhongMaService.GetAllThongDiepTraVeInTransLogsAsync(id);
            return Ok(new { result });
        }
        [HttpGet("GetThongDiepTraVeInTransLogs/{id}")]
        public IActionResult GetThongDiepTraVeInTransLogs(string id)
        {
            var result = _thongDiepGuiHDDTKhongMaService.GetThongDiepTraVeInTransLogsAsync(id);
            if (result != null)
            {
                return Ok(new { result });
            }
            else return Ok(null);
        }
        [AllowAnonymous]
        [HttpPost("GuiThongDiepKiemTraDuLieuHoaDon")]
        public IActionResult GuiThongDiepKiemTraDuLieuHoaDon(ThongDiepParams @params)
        {
            var result = _thongDiepGuiHDDTKhongMaService.GuiThongDiepKiemTraDuLieuHoaDon(@params);
            return Ok(new { result });
        }

        [AllowAnonymous]
        [HttpPost("GuiThongDiepKiemTraKyThuat")]
        public IActionResult GuiThongDiepKiemTraKyThuat(ThongDiepParams @params)
        {
            var result = _thongDiepGuiHDDTKhongMaService.GuiThongDiepKiemTraKyThuat(@params);
            return Ok(new { result });
        }

        [HttpPut("UpdateTrangThaiGui")]
        public async Task<IActionResult> UpdateTrangThaiGui(DuLieuGuiHDDTViewModel model)
        {
            var result = await _thongDiepGuiHDDTKhongMaService.UpdateTrangThaiGuiAsync(model);
            return Ok(result);
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert(ThongDiepChungViewModel model)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _thongDiepGuiHDDTKhongMaService.InsertAsync(model);
                    transaction.Commit();
                    return Ok(result);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return Ok(null);
                }
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(DuLieuGuiHDDTViewModel model)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _thongDiepGuiHDDTKhongMaService.UpdateAsync(model);
                    transaction.Commit();
                    return Ok(result);
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return Ok(false);
                }
            }
        }

        [HttpGet("GuiThongDiepDuLieuHDDT/{id}")]
        public async Task<IActionResult> GuiThongDiepDuLieuHDDT(string id)
        {
            var result = await _thongDiepGuiHDDTKhongMaService.GuiThongDiepDuLieuHDDTAsync(id);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("GuiThongDiepDuLieuHDDTBackground")]
        public async Task<IActionResult> GuiThongDiepDuLieuHDDTBackground([FromBody] KeyParams param)
        {
            if (!string.IsNullOrEmpty(param.KeyString))
            {
                string dbString = (param.KeyString).Base64Decode();

                User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, dbString);
                User.AddClaim(ClaimTypeConstants.DATABASE_NAME, param.DatabaseName);

                using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
                {
                    try
                    {
                        var result = await _thongDiepGuiHDDTKhongMaService.GuiThongDiepDuLieuHDDTBackgroundAsync();
                        transaction.Commit();
                        return Ok(result);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return Ok(e.ToString());
                    }
                }
            }

            return Ok(false);
        }

        [HttpPost("InsertRange")]
        public async Task<IActionResult> InsertRange(List<ThongDiepChungViewModel> models)
        {
            try
            {
                var result = await _thongDiepGuiHDDTKhongMaService.InsertRangeAsync(models);
                return Ok(result);
            }
            catch (Exception e)
            {
                Tracert.WriteLog("InsertRange", e);
                return Ok(null);
            }
        }

        #region Thông điệp 206
        /// <summary>
        /// Thêm thông điệp 206
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("InsertThongDiep206")]
        public async Task<IActionResult> InsertThongDiep206(ThongDiepChungViewModel model)
        {
            try
            {
                var result = await _thongDiepGuiHDDTKhongMaService.InsertThongDiep206(model);
                return Ok(result);
            }
            catch (Exception e)
            {
                Tracert.WriteLog("InsertThongDiep206", e);
                return Ok(null);
            }
        }

        /// <summary>
        /// Lưu dữ liệu đã ký thông điệp 206
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("LuuDuLieuDaKy")]
        public async Task<IActionResult> LuuDuLieuDaKy(ThongDiepChungViewModel model)
        {
            try
            {
                var result = await _thongDiepGuiHDDTKhongMaService.LuuDuLieuDaKy(model);
                return Ok(result);
            }
            catch (Exception e)
            {
                Tracert.WriteLog("LuuDuLieuDaKy", e);
                return Ok(null);
            }
        }

        /// <summary>
        /// Gửi thông điệp 206
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [HttpPost("GuiThongDiep206")]
        public async Task<IActionResult> GuiThongDiep206(List<ThongDiepChungViewModel> models)
        {
            try
            {
                using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
                {

                    foreach (var model in models)
                    {
                        var result = await _thongDiepGuiHDDTKhongMaService.GuiThongDiepDuLieuHDDTAsync4(model.ThongDiepChungId);
                        if (!result)
                        {
                            transaction.Rollback();
                            return Ok(result);
                        }
                    }

                    transaction.Commit();
                    return Ok(true);
                }
            }
            catch (Exception e)
            {
                Tracert.WriteLog("GuiThongDiep206", e);
                return Ok(false);
            }
        }
        #endregion
    }
}
