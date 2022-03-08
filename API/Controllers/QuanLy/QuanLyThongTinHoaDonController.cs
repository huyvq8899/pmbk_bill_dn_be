using Microsoft.AspNetCore.Mvc;
using Services.Repositories.Interfaces.QuanLy;
using System.Threading.Tasks;

namespace API.Controllers.QuanLy
{
    public class QuanLyThongTinHoaDonController : BaseController
    {
        private readonly IQuanLyThongTinHoaDonService _quanLyThongTinHoaDonService;

        public QuanLyThongTinHoaDonController(IQuanLyThongTinHoaDonService quanLyThongTinHoaDonService)
        {
            _quanLyThongTinHoaDonService = quanLyThongTinHoaDonService;
        }

        [HttpGet("GetListByLoaiThongTin")]
        public async Task<IActionResult> GetListByLoaiThongTin(int? loaiThongTin)
        {
            var result = await _quanLyThongTinHoaDonService.GetListByLoaiThongTinAsync(loaiThongTin);
            return Ok(result);
        }
    }
}
