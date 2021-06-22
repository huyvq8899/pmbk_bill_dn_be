using ManagementServices.Helper;
using Services.ViewModels.DanhMuc;

namespace Services.Helper.Params.DanhMuc
{
    public class DonViTinhParams : PagingParams
    {
        public DonViTinhViewModel Filter { get; set; }
    }
}
