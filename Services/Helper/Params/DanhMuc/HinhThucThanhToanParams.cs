using ManagementServices.Helper;
using Services.ViewModels.DanhMuc;

namespace Services.Helper.Params.DanhMuc
{
    public class HinhThucThanhToanParams : PagingParams
    {
        public HinhThucThanhToanViewModel Filter { get; set; }
    }
}
