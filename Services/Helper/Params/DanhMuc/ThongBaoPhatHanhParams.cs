using ManagementServices.Helper;
using Services.ViewModels.DanhMuc;

namespace Services.Helper.Params.DanhMuc
{
    public class ThongBaoPhatHanhParams : PagingParams
    {
        public ThongBaoPhatHanhViewModel Filter { get; set; }
    }
}
