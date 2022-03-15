using API.Extentions;
using DLL;
using DLL.Constants;
using DLL.Enums;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Services.Helper;
using Services.Repositories.Interfaces.QuanLy;
using System;
using System.Threading.Tasks;

namespace API.Controllers.QuanLy
{
    public class QuanLyThongTinHoaDonController : BaseController
    {
        private readonly IQuanLyThongTinHoaDonService _quanLyThongTinHoaDonService;
        private readonly Datacontext _db;

        public QuanLyThongTinHoaDonController(IQuanLyThongTinHoaDonService quanLyThongTinHoaDonService, Datacontext db)
        {
            _quanLyThongTinHoaDonService = quanLyThongTinHoaDonService;
            _db = db;
        }

        [HttpGet("GetListByLoaiThongTin")]
        public async Task<IActionResult> GetListByLoaiThongTin(int? loaiThongTin)
        {
            var result = await _quanLyThongTinHoaDonService.GetListByLoaiThongTinAsync(loaiThongTin);
            return Ok(result);
        }

        [HttpGet("GetListByHinhThucVaLoaiHoaDon/{hinhThucHoaDon}/{loaiHoaDon}")]
        public async Task<IActionResult> GetListByHinhThucVaLoaiHoaDon(HinhThucHoaDon hinhThucHoaDon, LoaiHoaDon loaiHoaDon)
        {
            var result = await _quanLyThongTinHoaDonService.GetListByHinhThucVaLoaiHoaDonAsync(hinhThucHoaDon, loaiHoaDon);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("UpdateTrangThaiSuDungTruocDo")]
        public async Task<IActionResult> UpdateTrangThaiSuDungTruocDo([FromBody] KeyParams param)
        {
            if (!string.IsNullOrEmpty(param.KeyString))
            {
                string dbString = (param.KeyString).Base64Decode();

                User.AddClaim(ClaimTypeConstants.CONNECTION_STRING, dbString);

                using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
                {
                    try
                    {
                        var result = await _quanLyThongTinHoaDonService.UpdateTrangThaiSuDungTruocDoAsync();
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

            return Ok(false);
        }
    }
}
