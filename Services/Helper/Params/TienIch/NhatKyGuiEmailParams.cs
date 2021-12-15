using ManagementServices.Helper;
using Services.ViewModels.TienIch;
using System.ComponentModel.DataAnnotations;

namespace Services.Helper.Params.TienIch
{
    public class NhatKyGuiEmailParams : PagingParams
    {
        public NhatKyGuiEmailViewModel Filter { get; set; }
        public bool? IsExportPDF { get; set; }

        public NhatKyGuiEmailSearch TimKiemTheo { get; set; }
        public string TimKiemBatKy { get; set; }
        public int LoaiEmail { get; set; }
        public int TrangThaiGuiEmail { get; set; }
    }

    public class NhatKyGuiEmailSearch
    {
        [Display(Name = "Ký hiệu mẫu số hóa đơn")]
        public string MauHoaDon { get; set; }

        [Display(Name = "Ký hiệu hóa đơn")]
        public string KyHieuHoaDon { get; set; }

        [Display(Name = "Số hóa đơn")]
        public string SoHoaDon { get; set; }

        [Display(Name = "Người thực hiện")]
        public string NguoiThucHien { get; set; }
    }
}
