using DLL;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Services.Helper.Params.QuyDinhKyThuat;
using Services.Repositories.Interfaces.QuyDinhKyThuat;
using Services.ViewModels.QuyDinhKyThuat;
using System;
using System.Threading.Tasks;

namespace API.Controllers.QuyDinhKyThuat
{
    public class ThongDiepGuiDuLieuHDDTController : BaseController
    {
        private readonly Datacontext _db;
        private readonly IThongDiepGuiDuLieuHDDTService _thongDiepGuiHDDTKhongMaService;

        public ThongDiepGuiDuLieuHDDTController(
            Datacontext datacontext,
            IThongDiepGuiDuLieuHDDTService thongDiepGuiHDDTKhongMaService)
        {
            _db = datacontext;
            _thongDiepGuiHDDTKhongMaService = thongDiepGuiHDDTKhongMaService;
        }

        [HttpPost("GetAllPaging")]
        public async Task<IActionResult> GetAllPaging(ThongDiepParams pagingParams)
        {
            var paged = await _thongDiepGuiHDDTKhongMaService.GetAllPagingAsync(pagingParams);
            return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
        }

        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(string Id)
        {
            var result = await _thongDiepGuiHDDTKhongMaService.GetByIdAsync(Id);
            return Ok(result);
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

        [HttpPost("NhanPhanHoiThongDiepKiemTraDuLieuHoaDon")]
        public async Task<IActionResult> NhanPhanHoiThongDiepKiemTraDuLieuHoaDon(ThongDiepParams @params)
        {
            var result = await _thongDiepGuiHDDTKhongMaService.NhanPhanHoiThongDiepKiemTraDuLieuHoaDonAsync(@params);
            return Ok(result);
        }

        [HttpPost("NhanPhanHoiThongDiepKyThuat")]
        public async Task<IActionResult> NhanPhanHoiThongDiepKyThuat(ThongDiepParams @params)
        {
            var result = await _thongDiepGuiHDDTKhongMaService.NhanPhanHoiThongDiepKyThuatAsync(@params);
            return Ok(result);
        }

        [HttpGet("KetQuaKiemTraDuLieuHoaDon/{id}")]
        public async Task<IActionResult> KetQuaKiemTraDuLieuHoaDon(string id)
        {
            var result = await _thongDiepGuiHDDTKhongMaService.KetQuaKiemTraDuLieuHoaDonAsync(id);
            return Ok(result);
        }

        [HttpGet("KetQuaPhanHoiKyThuat/{id}")]
        public async Task<IActionResult> KetQuaPhanHoiKyThuat(string id)
        {
            var result = await _thongDiepGuiHDDTKhongMaService.KetQuaPhanHoiKyThuatAsync(id);
            return Ok(result);
        }

        [HttpPut("UpdateTrangThaiGui")]
        public async Task<IActionResult> UpdateTrangThaiGui(ThongDiepGuiDuLieuHDDTViewModel model)
        {
            var result = await _thongDiepGuiHDDTKhongMaService.UpdateTrangThaiGuiAsync(model);
            return Ok(result);
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert(ThongDiepGuiDuLieuHDDTViewModel model)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _thongDiepGuiHDDTKhongMaService.InsertAsync(model);
                    transaction.Commit();
                    return Ok(result);
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return Ok(null);
                }
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(ThongDiepGuiDuLieuHDDTViewModel model)
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

        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _thongDiepGuiHDDTKhongMaService.DeleteAsync(id);
            return Ok(result);
        }

        [HttpGet("ExportXMLGuiDi/{id}")]
        public async Task<IActionResult> ExportXMLGuiDi(string id)
        {
            var result = await _thongDiepGuiHDDTKhongMaService.ExportXMLGuiDiAsync(id);
            return Ok(new { result });
        }

        [HttpGet("ExportXMLKetQua/{id}")]
        public async Task<IActionResult> ExportXMLKetQua(string id)
        {
            var result = await _thongDiepGuiHDDTKhongMaService.ExportXMLKetQuaAsync(id);
            return Ok(new { result });
        }
    }
}
