using Microsoft.AspNetCore.Mvc;
using Services.Helper.Params.BaoCao;
using Services.Repositories.Interfaces.BaoCao;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class BaoCaoController : BaseController
    {
        private readonly IBaoCaoService _IBaoCaoService;
        public BaoCaoController(IBaoCaoService IBaoCaoService)
        {
            _IBaoCaoService = IBaoCaoService;
        }

        [HttpPost("ThongKeSoLuongHoaDonDaPhatHanhAsync")]
        public async Task<IActionResult> ThongKeSoLuongHoaDonDaPhatHanhAsync(BaoCaoParams @params)
        {
            var result = await _IBaoCaoService.ThongKeSoLuongHoaDonDaPhatHanhAsync(@params);
            return Ok(new { Data = result, FilePath = @params.FilePath });
        }

        [HttpPost("BangKeChiTietHoaDonAsync")]
        public async Task<IActionResult> BangKeChiTietHoaDonAsync(BaoCaoParams @params)
        {
            var result = await _IBaoCaoService.BangKeChiTietHoaDonAsync(@params);
            return Ok(new { Data = result, FilePath = @params.FilePath });
        }

        [HttpPost("TongHopGiaTriHoaDonDaSuDung")]
        public async Task<IActionResult> TongHopGiaTriHoaDonDaSuDung(BaoCaoParams @params)
        {
            var result = await _IBaoCaoService.TongHopGiaTriHoaDonDaSuDungAsync(@params);
            return Ok(new { Data = result, FilePath = @params.FilePath });
        }
    }
}
