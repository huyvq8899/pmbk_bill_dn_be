using DLL.Entity.QuanLyHoaDon;

namespace DLL.Entity.QuyDinhKyThuat
{
    public class ThongDiepGuiDuLieuHDDTChiTiet : ThongTinChung
    {
        public string ThongDiepGuiDuLieuHDDTChiTietId { get; set; }
        public string ThongDiepGuiDuLieuHDDTId { get; set; }
        public string HoaDonDienTuId { get; set; }
        //
        public ThongDiepGuiDuLieuHDDT ThongDiepGuiDuLieuHDDT { get; set; }
        public HoaDonDienTu HoaDonDienTu { get; set; }
    }
}
