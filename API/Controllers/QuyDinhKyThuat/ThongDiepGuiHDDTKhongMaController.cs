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
    public class ThongDiepGuiHDDTKhongMaController : BaseController
    {
        private readonly Datacontext _db;
        private readonly IThongDiepGuiHDDTKhongMaService _thongDiepGuiHDDTKhongMaService;

        public ThongDiepGuiHDDTKhongMaController(
            Datacontext datacontext,
            IThongDiepGuiHDDTKhongMaService thongDiepGuiHDDTKhongMaService)
        {
            _db = datacontext;
            _thongDiepGuiHDDTKhongMaService = thongDiepGuiHDDTKhongMaService;
        }

        [HttpPost("GetAllPaging")]
        public async Task<IActionResult> GetAllPaging(PagingParams pagingParams)
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
        [HttpPost("GuiThongDiep")]
        public IActionResult GuiThongDiep(ThongDiepParams @params)
        {
            try
            {
                var result = _thongDiepGuiHDDTKhongMaService.GuiThongDiep(@params);
                return Ok(new { result });
            }
            catch (Exception e)
            {

                throw;
            }
        }

        [HttpPost("NhanPhanHoi")]
        public async Task<IActionResult> NhanPhanHoi(ThongDiepParams @params)
        {
            var result = await _thongDiepGuiHDDTKhongMaService.NhanPhanHoiAsync(@params);
            return Ok(result);
        }

        [HttpGet("XemKetQuaTuCQT/{id}")]
        public async Task<IActionResult> XemKetQuaTuCQT(string id)
        {
            var result = await _thongDiepGuiHDDTKhongMaService.XemKetQuaTuCQTAsync(id);
            return Ok(result);
        }

        [HttpPut("UpdateTrangThaiGui")]
        public async Task<IActionResult> UpdateTrangThaiGui(ThongDiepGuiHDDTKhongMaViewModel model)
        {
            var result = await _thongDiepGuiHDDTKhongMaService.UpdateTrangThaiGuiAsync(model);
            return Ok(result);
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert(ThongDiepGuiHDDTKhongMaViewModel model)
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
        public async Task<IActionResult> Update(ThongDiepGuiHDDTKhongMaViewModel model)
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
