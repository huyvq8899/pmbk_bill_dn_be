using DLL;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Services.Helper;
using Services.Helper.Params.BaoCao;
using Services.Repositories.Interfaces.BaoCao;
using Services.ViewModels.BaoCao;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class BaoCaoController : BaseController
    {
        private readonly IBaoCaoService _IBaoCaoService;
        private readonly Datacontext _db;
        public BaoCaoController(IBaoCaoService IBaoCaoService
            , Datacontext db
        )
        {
            _IBaoCaoService = IBaoCaoService;
            _db = db;
        }

        [HttpPost("ThongKeSoLuongHoaDonDaPhatHanhAsync")]
        public async Task<IActionResult> ThongKeSoLuongHoaDonDaPhatHanhAsync(BaoCaoParams @params)
        {
            var result = await _IBaoCaoService.ThongKeSoLuongHoaDonDaPhatHanhAsync(@params);
            return Ok(new { Data = result, FilePath = @params.FilePath });
        }

        [HttpPost("PrintSoLuongHoaDonDaPhatHanhAsync")]
        public async Task<IActionResult> PrintSoLuongHoaDonDaPhatHanhAsync(BaoCaoParams @params)
        {
            var result = await _IBaoCaoService.PrintThongKeSoLuongHoaDonDaPhatHanh(@params);
            return Ok(new { path = result }) ;
        }


        [HttpPost("BangKeChiTietHoaDonAsync")]
        public async Task<IActionResult> BangKeChiTietHoaDonAsync(BaoCaoParams @params)
        {
            var result = await _IBaoCaoService.BangKeChiTietHoaDonAsync(@params);
            return Ok(new { Data = result, FilePath = @params.FilePath });
        }

        [HttpPost("PrintBangKeChiTietHoaDonAsync")]
        public async Task<IActionResult> PrintBangKeChiTietHoaDonAsync(BaoCaoParams @params)
        {
            var result = await _IBaoCaoService.PrintBangKeChiTietHoaDonAsync(@params);
            return Ok(new { path = result });
        }

        [HttpPost("TongHopGiaTriHoaDonDaSuDung")]
        public async Task<IActionResult> TongHopGiaTriHoaDonDaSuDung(BaoCaoParams @params)
        {
            var result = await _IBaoCaoService.TongHopGiaTriHoaDonDaSuDungAsync(@params);
            return Ok(new { Data = result, FilePath = @params.FilePath });
        }

        [HttpPost("ThemBaoCaoTinhHinhSuDungHoaDon")]
        public async Task<IActionResult> ThemBaoCaoTinhHinhSuDungHoaDon(ChonKyTinhThueParams @params)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _IBaoCaoService.ThemBaoCaoTinhHinhSuDungHoaDon(@params);
                    if (result) transaction.Commit();
                    else transaction.Rollback();
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    FileLog.WriteLog(ex.Message);
                    transaction.Rollback();
                }

            }

            return Ok(false);
        }

        [HttpPost("CapNhatBaoCaoTinhHinhSuDungHoaDon")]
        public async Task<IActionResult> CapNhatBaoCaoTinhHinhSuDungHoaDon(BaoCaoTinhHinhSuDungHoaDonViewModel baoCao)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _IBaoCaoService.CapNhatChiTietBaoCaoTinhHinhSuDungHoaDon(baoCao);
                    if (result) transaction.Commit();
                    else transaction.Rollback();
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    FileLog.WriteLog(ex.Message);
                    transaction.Rollback();
                }

                return Ok(false);
            }
        }

        [HttpDelete("XoaBaoCaoTinhHinhSuDungHoaDon/{baoCaoId}")]
        public async Task<IActionResult> XoaBaoCaoTinhHinhSuDungHoaDon(string baoCaoId)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var result = await _IBaoCaoService.XoaBaoCaoTinhHinhSuDungHoaDon(baoCaoId);
                    if (result) transaction.Commit();
                    else transaction.Rollback();
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    FileLog.WriteLog(ex.Message);
                    transaction.Rollback();
                }

                return Ok(false);
            }
        }

        [HttpPost("GetListTinhHinhSuDungHoaDon")]
        public async Task<IActionResult> GetListTinhHinhSuDungHoaDon(PagingParams @params)
        {
            var result = await _IBaoCaoService.GetListTinhHinhSuDungHoaDon(@params);
            return Ok(new { Data = result });
        }

        [HttpGet("GetById/{baoCaoId}")]
        public async Task<IActionResult> GetById(string baoCaoId)
        {
            var result = await _IBaoCaoService.GetById(baoCaoId);
            return Ok(result);
        }

        [HttpPost("CheckNgayThangBaoCaoTinhHinhSuDungHD")]
        public async Task<IActionResult> CheckNgayThangBaoCaoTinhHinhSuDungHD(ChonKyTinhThueParams @params)
        {
            var result = await _IBaoCaoService.CheckNgayThangBaoCaoTinhHinhSuDungHD(@params);
            return Ok(result);
        }

        [HttpPost("GetBaoCaoByKyTinhThue")]
        public async Task<IActionResult> GetBaoCaoByKyTinhThue(ChonKyTinhThueParams @params)
        {
            var result = await _IBaoCaoService.GetBaoCaoByKyTinhThue(@params);
            return Ok(result);
        }
    }
}
