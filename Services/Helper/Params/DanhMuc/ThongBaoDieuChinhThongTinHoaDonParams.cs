using ManagementServices.Helper;
using Services.ViewModels.DanhMuc;

namespace Services.Helper.Params.DanhMuc
{
    public class ThongBaoDieuChinhThongTinHoaDonParams : PagingParams
    {
        public ThongBaoDieuChinhThongTinHoaDonViewModel Filter { get; set; }
    }
}
