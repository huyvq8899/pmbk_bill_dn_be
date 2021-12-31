using DLL;
using ManagementServices.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Services.Helper;
using Services.Helper.Params.BaoCao;
using Services.Repositories.Interfaces.BaoCao;
using Services.ViewModels.BaoCao;
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
            return Ok(new { Data = result, @params.FilePath });
        }

        [HttpPost("PrintSoLuongHoaDonDaPhatHanhAsync")]
        public IActionResult PrintSoLuongHoaDonDaPhatHanhAsync(BaoCaoParams @params)
        {
            var result = _IBaoCaoService.PrintThongKeSoLuongHoaDonDaPhatHanh(@params);
            return Ok(new { path = result });
        }

        [HttpPost("ExportExcelBangKeHangHoaBanRa")]
        public async Task<IActionResult> ExportExcelBangKeHangHoaBanRa(PagingParams @params)
        {
            var result = await _IBaoCaoService.ExportExcelBangKeHangHoaBanRa(@params);
            return Ok(new { Path = result });
        }

        [HttpPost("BangKeChiTietHoaDonAsync")]
        public async Task<IActionResult> BangKeChiTietHoaDonAsync(BaoCaoParams @params)
        {
            var result = await _IBaoCaoService.BangKeChiTietHoaDonAsync(@params);
            return Ok(new { Data = result, @params.FilePath });
        }

        [HttpPost("PrintBangKeChiTietHoaDonAsync")]
        public IActionResult PrintBangKeChiTietHoaDonAsync(BaoCaoParams @params)
        {
            var result = _IBaoCaoService.PrintBangKeChiTietHoaDonAsync(@params);
            return Ok(new { path = result });
        }

        [HttpPost("TongHopGiaTriHoaDonDaSuDung")]
        public async Task<IActionResult> TongHopGiaTriHoaDonDaSuDung(BaoCaoParams @params)
        {
            var result = await _IBaoCaoService.TongHopGiaTriHoaDonDaSuDungAsync(@params);
            return Ok(new { Data = result, @params.FilePath });
        }

        [HttpPost("ThemBaoCaoTinhHinhSuDungHoaDon")]
        public async Task<IActionResult> ThemBaoCaoTinhHinhSuDungHoaDon(ChonKyTinhThueParams @params)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                var result = await _IBaoCaoService.ThemBaoCaoTinhHinhSuDungHoaDon(@params);
                if (result) transaction.Commit();
                else transaction.Rollback();
                return Ok(result);
            }
        }

        [HttpPost("CapNhatBaoCaoTinhHinhSuDungHoaDon")]
        public async Task<IActionResult> CapNhatBaoCaoTinhHinhSuDungHoaDon(BaoCaoTinhHinhSuDungHoaDonViewModel baoCao)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                var result = await _IBaoCaoService.CapNhatChiTietBaoCaoTinhHinhSuDungHoaDon(baoCao);
                if (result) transaction.Commit();
                else transaction.Rollback();
                return Ok(result);
            }
        }

        [HttpDelete("XoaBaoCaoTinhHinhSuDungHoaDon/{baoCaoId}")]
        public async Task<IActionResult> XoaBaoCaoTinhHinhSuDungHoaDon(string baoCaoId)
        {
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                var result = await _IBaoCaoService.XoaBaoCaoTinhHinhSuDungHoaDon(baoCaoId);
                if (result) transaction.Commit();
                else transaction.Rollback();
                return Ok(result);
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

        [HttpPost("PrintBaoCaoTinhHinhSuDungHoaDonAsync")]
        public async Task<IActionResult> PrintBaoCaoTinhHinhSuDungHoaDonAsync(BaoCaoTinhHinhSuDungHoaDonViewModel @params)
        {
            var result = await _IBaoCaoService.PrintChiTietBaoCaoTinhHinhSuDungHoaDonAsync(@params);
            return Ok(new { path = result });
        }


        [HttpPost("ExportExcelTongHopGiaTriHoaDonDaSuDung")]
        public async Task<IActionResult> ExportExcelTongHopGiaTriHoaDonDaSuDung(BaoCaoParams @params)
        {
            var result = await _IBaoCaoService.ExportExcelTongHopGiaTriHoaDonDaSuDungAsync(@params);
            return File(result.Bytes, result.ContentType, result.FileName);
        }
    }
}
