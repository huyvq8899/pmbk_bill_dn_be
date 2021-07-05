using ManagementServices.Helper;
using Services.ViewModels.DanhMuc;

namespace Services.Helper.Params.DanhMuc
{
    public class MauHoaDonParams : PagingParams
    {
        public MauHoaDonViewModel Filter { get; set; }
    }
}
