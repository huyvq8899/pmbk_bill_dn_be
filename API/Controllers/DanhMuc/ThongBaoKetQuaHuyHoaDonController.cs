using Services.Repositories.Interfaces.DanhMuc;

namespace API.Controllers.DanhMuc
{
    public class ThongBaoKetQuaHuyHoaDonController : BaseController
    {
        private readonly IThongBaoKetQuaHuyHoaDonService _thongBaoKetQuaHuyHoaDonService;
        public ThongBaoKetQuaHuyHoaDonController(IThongBaoKetQuaHuyHoaDonService thongBaoKetQuaHuyHoaDonService)
        {
            _thongBaoKetQuaHuyHoaDonService = thongBaoKetQuaHuyHoaDonService;
        }
    }
}
