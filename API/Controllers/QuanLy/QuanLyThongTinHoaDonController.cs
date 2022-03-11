using DLL.Enums;
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

        [HttpGet("GetListByHinhThucVaLoaiHoaDon/{hinhThucHoaDon}/{loaiHoaDon}")]
        public async Task<IActionResult> GetListByHinhThucVaLoaiHoaDon(HinhThucHoaDon hinhThucHoaDon, LoaiHoaDon loaiHoaDon)
        {
            var result = await _quanLyThongTinHoaDonService.GetListByHinhThucVaLoaiHoaDonAsync(hinhThucHoaDon, loaiHoaDon);
            return Ok(result);
        }
    }
}
