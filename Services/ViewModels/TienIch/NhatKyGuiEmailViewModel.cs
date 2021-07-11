using DLL.Enums;

namespace Services.ViewModels.TienIch
{
    public class NhatKyGuiEmailViewModel : ThongTinChungViewModel
    {
        public string NhatKyGuiEmailId { get; set; }
        public string MauSo { get; set; }
        public string KyHieu { get; set; }
        public string So { get; set; }
        public string Ngay { get; set; }
        public TrangThaiGuiEmail TrangThaiGuiEmail { get; set; }
        public string TenNguoiNhan { get; set; }
        public string EmailNguoiNhan { get; set; }
        public string RefId { get; set; }
        public RefType RefType { get; set; }
    }
}
