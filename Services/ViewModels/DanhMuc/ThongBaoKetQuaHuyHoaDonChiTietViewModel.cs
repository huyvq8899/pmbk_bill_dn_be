using DLL.Enums;

namespace Services.ViewModels.DanhMuc
{
    public class ThongBaoKetQuaHuyHoaDonChiTietViewModel : ThongTinChungViewModel
    {
        public string ThongBaoKetQuaHuyHoaDonChiTietId { get; set; }
        public string ThongBaoKetQuaHuyHoaDonId { get; set; }
        public LoaiHoaDon LoaiHoaDon { get; set; }
        public string MauHoaDonId { get; set; }
        public string KyHieu { get; set; }
        public int? TuSo { get; set; }
        public int? DenSo { get; set; }
        public int? SoLuong { get; set; }

        public string MauSo { get; set; }
        public string TenLoaiHoaDon { get; set; }
    }
}
