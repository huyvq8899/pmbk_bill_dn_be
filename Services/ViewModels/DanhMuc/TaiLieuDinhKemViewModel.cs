using DLL.Enums;

namespace Services.ViewModels.DanhMuc
{
    public class TaiLieuDinhKemViewModel : ThongTinChungViewModel
    {
        public string TaiLieuDinhKemId { get; set; }
        public string NghiepVuId { get; set; }
        public LoaiNghiepVu LoaiNghiepVu { get; set; }
        public string TenGoc { get; set; }
        public string TenGuid { get; set; }
    }
}
