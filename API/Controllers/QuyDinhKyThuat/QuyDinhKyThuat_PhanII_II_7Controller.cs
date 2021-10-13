using DLL;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Services.Repositories.Interfaces.QuyDinhKyThuat;
using Services.ViewModels.QuyDinhKyThuat;
using System;
using System.Threading.Tasks;

namespace API.Controllers.QuyDinhKyThuat
{
    public class QuyDinhKyThuat_PhanII_II_7Controller : BaseController
    {
        private readonly Datacontext _db;
        private readonly IThongDiepGuiHDDTKhongMaService _thongDiepGuiHDDTKhongMaService;

        public QuyDinhKyThuat_PhanII_II_7Controller(
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

        [HttpGet("ExportXML/{id}")]
        public async Task<IActionResult> ExportXML(string id)
        {
            var result = await _thongDiepGuiHDDTKhongMaService.ExportXMLAsync(id);
            return Ok(new { result });
        }
    }
}
