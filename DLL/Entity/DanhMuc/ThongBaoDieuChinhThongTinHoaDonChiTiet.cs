namespace DLL.Entity.DanhMuc
{
    public class ThongBaoDieuChinhThongTinHoaDonChiTiet : ThongTinChung
    {
        public string ThongBaoDieuChinhThongTinHoaDonChiTietId { get; set; }
        public string ThongBaoDieuChinhThongTinHoaDonId { get; set; }
        public string MauHoaDonId { get; set; }

        public ThongBaoDieuChinhThongTinHoaDon ThongBaoDieuChinhThongTinHoaDon { get; set; }
        public MauHoaDon MauHoaDon { get; set; }
    }
}
