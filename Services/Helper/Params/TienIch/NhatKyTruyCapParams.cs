using ManagementServices.Helper;
using Services.ViewModels.TienIch;
using System.ComponentModel.DataAnnotations;

namespace Services.Helper.Params.TienIch
{
    public class NhatKyTruyCapParams : PagingParams
    {
        public NhatKyTruyCapSearch TimKiemTheo { get; set; }
        public NhatKyTruyCapViewModel Filter { get; set; }
        public string TimKiemBatKy { get; set; }
        public bool? IsExportPDF { get; set; }
    }

    public class NhatKyTruyCapSearch
    {
        [Display(Name = "Hành động")]
        public string HanhDong { get; set; }

        [Display(Name = "Địa chỉ IP")]
        public string DiaChiIP { get; set; }

        [Display(Name = "Tên máy tính")]
        public string TenMayTinh { get; set; }

        [Display(Name = "Người thực hiện")]
        public string NguoiThucHien { get; set; }

        [Display(Name = "Tham chiếu")]
        public string ThamChieu { get; set; }
    }
}
