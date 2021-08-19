using ManagementServices.Helper;
using Services.ViewModels.TienIch;

namespace Services.Helper.Params.TienIch
{
    public class NhatKyTruyCapParams : PagingParams
    {
        public NhatKyTruyCapViewModel Filter { get; set; }
        public bool? IsExportPDF { get; set; }
    }
}
