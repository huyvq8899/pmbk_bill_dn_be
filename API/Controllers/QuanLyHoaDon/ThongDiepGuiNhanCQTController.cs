using DLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Services.Helper;
using Services.Helper.Params.HoaDon;
using Services.Repositories.Interfaces.QuanLyHoaDon;
using Services.ViewModels.QuanLyHoaDonDienTu;
using System;
using System.Threading.Tasks;

namespace API.Controllers.QuanLyHoaDon
{
    public class ThongDiepGuiNhanCQTController : BaseController
    {
        private readonly IThongDiepGuiNhanCQTService _IThongDiepGuiNhanCQTService;
        private readonly Datacontext _db;

        public ThongDiepGuiNhanCQTController(IThongDiepGuiNhanCQTService iThongDiepGuiNhanCQTService, Datacontext datacontext)
        {
            _IThongDiepGuiNhanCQTService = iThongDiepGuiNhanCQTService;
            _db = datacontext;
        }

        [HttpPost("GetListHoaDonSaiSot")]
        public async Task<IActionResult> GetListHoaDonSaiSot(HoaDonSaiSotParams @params)
        {
            var result = await _IThongDiepGuiNhanCQTService.GetListHoaDonSaiSotAsync(@params);
            return Ok(result);
        }

        [HttpPost("InsertThongBaoGuiHoaDonSaiSot")]
        public async Task<IActionResult> InsertThongBaoGuiHoaDonSaiSot(ThongDiepGuiCQTViewModel model)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    string result = await _IThongDiepGuiNhanCQTService.InsertThongBaoGuiHoaDonSaiSotAsync(model);
                    if (result == null)
                    {
                        transaction.Rollback();
                        return Ok(null);
                    }
                    else
                    {
                        transaction.Commit();
                        return Ok(new { id = result });
                    }
                }
                catch (Exception)
                {
                    return Ok(null);
                }
            }
        }
    }
}
