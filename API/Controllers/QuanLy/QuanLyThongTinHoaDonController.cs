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
using System.Numerics;
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

        [HttpGet("GetByLoaiThongTinChiTiet/{loaiThongTinChiTiet}")]
        public async Task<IActionResult> GetByLoaiThongTinChiTiet(LoaiThongTinChiTiet loaiThongTinChiTiet)
        {
            var result = await _quanLyThongTinHoaDonService.GetByLoaiThongTinChiTietAsync(loaiThongTinChiTiet);
            return Ok(result);
        }

        [HttpGet("GetLoaiHoaDonDangSuDung")]
        public async Task<IActionResult> GetLoaiHoaDonDangSuDung()
        {
            var result = await _quanLyThongTinHoaDonService.GetLoaiHoaDonDangSuDung();
            return Ok(result);
        }

        [HttpGet("GetHinhThucHoaDonDangSuDung")]
        public async Task<IActionResult> GetHinhThucHoaDonDangSuDung()
        {
            var result = await _quanLyThongTinHoaDonService.GetHinhThucHoaDonDangSuDung();
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
        /// <summary>
        /// Get Lich Su Sinh So Hoa Don Co Ma Tu May Tinh Tien
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet("GetHistorySinhSoHoaDonCMMTTien/{year}")]
        public async Task<IActionResult> GetHistorySinhSoHoaDonCMMTTien(int year)
        {
            var result = await _quanLyThongTinHoaDonService.GetHistorySinhSoHoaDonCMMTTien(year);
            return Ok(result);
        }

        /// <summary>
        /// Get Lich Su Sinh So Hoa Don Co Ma Tu May Tinh Tien
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet("GetHistorySinhSoHoaDonCMMTTienInNhatKyTruyCap/{id}")]
        public async Task<IActionResult> GetHistorySinhSoHoaDonCMMTTienInNhatKyTruyCap(Guid id)
        {
            var result = await _quanLyThongTinHoaDonService.GetHistorySinhSoHoaDonCMMTTienInNhatKyTruyCap(id);
            return Ok(result);
        }

        /// <summary>
        /// GUpdate So Bat Dau Table SinhSoHoadon 
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet("UpdateSoBatDauSinhSoHoaDonCMMTTien/{SoBatDau}")]
        public async Task<IActionResult> UpdateSoBatDauSinhSoHoaDonCMMTTien(long SoBatDau)
        {
            var result = await _quanLyThongTinHoaDonService.UpdateSoBatDauSinhSoHoaDonCMMTTien(SoBatDau);
            return Ok(result);
        }
        /// <summary>
        /// GUpdate So Bat Dau Table SinhSoHoadon 
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet("UpdateSoDaSinhMoiNhatHoaDonCMMTTien/{SoBatDau}")]
        public async Task<IActionResult> UpdateSoDaSinhMoiNhatHoaDonCMMTTien(long SoBatDau)
        {
            var result = await _quanLyThongTinHoaDonService.UpdateSoDaSinhMoiNhatHoaDonCMMTTien(SoBatDau);
            return Ok(result);
        }

        /// <summary>
        /// GUpdate So Bat Dau Table SinhSoHoadon 
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet("UpdateSoDaSinhMoiNhatHoaDonCMMTTienByNSD/{SoBatDau}")]
        public async Task<IActionResult> UpdateSoDaSinhMoiNhatHoaDonCMMTTienByNSD(long SoBatDau)
        {
            var result = await _quanLyThongTinHoaDonService.UpdateSoDaSinhMoiNhatHoaDonCMMTTienByNSD(SoBatDau);
            return Ok(result);
        }

        [HttpGet("GetSoCuoiMaxMaCQTCapMTTAsync")]
        public async Task<IActionResult> GetSoCuoiMaxMaCQTCapMTTAsync([FromQuery] int year)
        {
            var result = await _quanLyThongTinHoaDonService.GetSoCuoiMaxMaCQTCapMTTAsync(year);
            return Ok(result);
        }
    }
}
