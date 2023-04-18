using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Repositories.Interfaces.TienIch;
using Services.ViewModels.Params;

namespace API.Controllers
{
    /// <summary>
    /// Lớp này thực hiện các nhiệm vụ:
    /// - Đọc thông tin người bán từ mẫu hóa đơn theo các trường hợp khác nhau.
    /// - Đọc ra tên người ký theo các điều kiện/trường hợp khác nhau.
    /// - Gửi lệnh socket để đọc thông tin người ký từ chữ ký số.
    /// </summary>
    public class DigitalSignerNameReaderController : BaseController
    {
        private readonly IDigitalSignerNameReaderService _digitalSignerNameReaderService;

        public DigitalSignerNameReaderController(IDigitalSignerNameReaderService digitalSignerNameReaderService)
        {
            _digitalSignerNameReaderService = digitalSignerNameReaderService;
        }

        /// <summary>
        /// Đọc ra tên người ký theo các điều kiện/trường hợp khác nhau.
        /// </summary>
        /// <param name="signerNameParams">Tham số điều kiện.</param>
        /// <returns>Tên người ký.</returns>
        [HttpPost("GetUnifiedSignerName")]
        public async Task<IActionResult> GetUnifiedSignerName(UnifiedSignerNameParams signerNameParams)
        {
            var result = await _digitalSignerNameReaderService.GetUnifiedSignerNameAsync(signerNameParams);
            return Ok(new { UnifiedSignerName = result });
        }

        /// <summary>
        /// Đọc ra thông tin của người bán trên mẫu hóa đơn của hóa đơn điện tử.
        /// </summary>
        /// <param name="hoaDonDienTuIds">Là mảng các id của hóa đơn điện tử cần đọc thông tin.</param>
        /// <returns>Thông tin của người bán trên mẫu hóa đơn.</returns>
        [HttpPost("GetThongTinNguoiBanTuHoaDon")]
        public async Task<IActionResult> GetThongTinNguoiBanTuHoaDon(string[] hoaDonDienTuIds)
        {
            var result = await _digitalSignerNameReaderService.GetThongTinNguoiBanTuHoaDonAsync(hoaDonDienTuIds);
            return Ok(result);
        }
    }
}