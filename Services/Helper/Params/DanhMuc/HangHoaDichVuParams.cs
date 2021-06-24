using ManagementServices.Helper;
using Services.ViewModels.DanhMuc;

namespace Services.Helper.Params.DanhMuc
{
    public class HangHoaDichVuParams : PagingParams
    {
        public HangHoaDichVuViewModel Filter { get; set; }
    }
}
