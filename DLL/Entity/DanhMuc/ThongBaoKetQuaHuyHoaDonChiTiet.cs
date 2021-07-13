using DLL.Enums;

namespace DLL.Entity.DanhMuc
{
    public class ThongBaoKetQuaHuyHoaDonChiTiet : ThongTinChung
    {
        public string ThongBaoKetQuaHuyHoaDonChiTietId { get; set; }
        public string ThongBaoKetQuaHuyHoaDonId { get; set; }
        public LoaiHoaDon LoaiHoaDon { get; set; }
        public string MauHoaDonId { get; set; }
        public int? TuSo { get; set; }
        public int? DenSo { get; set; }
        public int? SoLuong { get; set; }

        public ThongBaoKetQuaHuyHoaDon ThongBaoKetQuaHuyHoaDon { get; set; }
        public MauHoaDon MauHoaDon { get; set; }
    }
}
