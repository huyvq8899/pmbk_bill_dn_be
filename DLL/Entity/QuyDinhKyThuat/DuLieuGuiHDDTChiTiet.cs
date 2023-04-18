using DLL.Entity.QuanLyHoaDon;

namespace DLL.Entity.QuyDinhKyThuat
{
    public class DuLieuGuiHDDTChiTiet : ThongTinChung
    {
        public string DuLieuGuiHDDTChiTietId { get; set; }
        public string DuLieuGuiHDDTId { get; set; }
        public string HoaDonDienTuId { get; set; }
        //
        public DuLieuGuiHDDT DuLieuGuiHDDT { get; set; }
        public HoaDonDienTu HoaDonDienTu { get; set; }
    }
}
