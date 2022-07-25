using Microsoft.AspNetCore.Mvc;
using Services.Helper.Params.TienIch;
using Services.Repositories.Interfaces.TienIch;
using System.Threading.Tasks;

namespace API.Controllers.TienIch
{
    public class NhatKyGuiEmailController : BaseController
    {
        private readonly INhatKyGuiEmailService _nhatKyGuiEmailService;

        public NhatKyGuiEmailController(INhatKyGuiEmailService nhatKyGuiEmailService)
        {
            _nhatKyGuiEmailService = nhatKyGuiEmailService;
        }

        [HttpPost("GetAllPaging")]
        public async Task<IActionResult> GetAllPaging(NhatKyGuiEmailParams pagingParams)
        {
            var paged = await _nhatKyGuiEmailService.GetAllPagingAsync(pagingParams);
            return Ok(new { paged.Items, paged.CurrentPage, paged.PageSize, paged.TotalCount, paged.TotalPages });
        }

        [HttpPost("ExportExcel")]
        public async Task<IActionResult> ExportExcel(NhatKyGuiEmailParams @params)
        {
            var result = await _nhatKyGuiEmailService.ExportExcelAsync(@params);
            return File(result.Bytes, result.ContentType, result.FileName);
        }

        [HttpGet("KiemTraDaGuiEmailChoKhachHang/{HoaDonDienTuId}")]
        public async Task<IActionResult> KiemTraDaGuiEmailChoKhachHang(string hoaDonDienTuId)
        {
            var result = await _nhatKyGuiEmailService.KiemTraDaGuiEmailChoKhachHangAsync(hoaDonDienTuId, -100);
            return Ok(result);
        }
        /// <summary>
        /// Kiểm tra đã gửi email cho khách hàng chưa với loại email là:
        /// Gửi thông báo hóa đơn có thông tin sai sót không phải lập lại hóa đơn cho khách hàng
        /// </summary>
        /// <param name="hoaDonDienTuId"></param>
        /// <returns></returns>
        [HttpGet("KiemTraDaGuiTBSSKhongLapHd/{HoaDonDienTuId}")]
        public async Task<IActionResult> KiemTraDaGuiTBSSKhongLapHd(string hoaDonDienTuId)
        {
            var result = await _nhatKyGuiEmailService.GetNhatKyGuiEmailByHoaDonDienTuIdAsync(hoaDonDienTuId, 4);
            return Ok(result);
        }

        [HttpGet("GetThongTinEmailDaGuiChoKHGanNhat")]
        public async Task<IActionResult> GetThongTinEmailDaGuiChoKHGanNhat()
        {
            var result = await _nhatKyGuiEmailService.GetThongTinEmailDaGuiChoKHGanNhatAsync();
            return Ok(result);
        }
    }
}
