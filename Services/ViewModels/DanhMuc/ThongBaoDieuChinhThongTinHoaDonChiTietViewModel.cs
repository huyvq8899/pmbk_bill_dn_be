using DLL.Enums;

namespace Services.ViewModels.DanhMuc
{
    public class ThongBaoDieuChinhThongTinHoaDonChiTietViewModel : ThongTinChungViewModel
    {
        public string ThongBaoDieuChinhThongTinHoaDonChiTietId { get; set; }
        public string ThongBaoDieuChinhThongTinHoaDonId { get; set; }
        public string MauHoaDonId { get; set; }

        public LoaiHoaDon LoaiHoaDon { get; set; }
        public string TenLoaiHoaDon { get; set; }
        public string MauSo { get; set; }
        public string KyHieu { get; set; }
        public int? TuSo { get; set; }
        public int? DenSo { get; set; }
        public int? SoLuong { get; set; }
        public bool? Checked { get; set; }
    }
}
