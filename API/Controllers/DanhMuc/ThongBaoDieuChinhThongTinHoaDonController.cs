using Services.Repositories.Interfaces.DanhMuc;

namespace API.Controllers.DanhMuc
{
    public class ThongBaoDieuChinhThongTinHoaDonController : BaseController
    {
        private readonly IThongBaoDieuChinhThongTinHoaDonService _thongBaoDieuChinhThongTinHoaDonService;
        public ThongBaoDieuChinhThongTinHoaDonController(IThongBaoDieuChinhThongTinHoaDonService thongBaoDieuChinhThongTinHoaDonService)
        {
            _thongBaoDieuChinhThongTinHoaDonService = thongBaoDieuChinhThongTinHoaDonService;
        }
    }
}
