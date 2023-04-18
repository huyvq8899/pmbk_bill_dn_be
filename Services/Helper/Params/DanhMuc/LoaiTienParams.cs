using ManagementServices.Helper;
using Services.ViewModels.DanhMuc;

namespace Services.Helper.Params.DanhMuc
{
    public class LoaiTienParams : PagingParams
    {
        public LoaiTienViewModel Filter { get; set; }
    }
}
