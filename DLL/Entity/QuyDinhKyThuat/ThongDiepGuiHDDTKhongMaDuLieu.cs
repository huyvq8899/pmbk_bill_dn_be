using DLL.Entity.QuanLyHoaDon;

namespace DLL.Entity.QuyDinhKyThuat
{
    public class ThongDiepGuiHDDTKhongMaDuLieu : ThongTinChung
    {
        public string ThongDiepGuiHDDTKhongMaDuLieuId { get; set; }
        public string ThongDiepGuiHDDTKhongMaId { get; set; }
        public string HoaDonDienTuId { get; set; }
        //
        public ThongDiepGuiHDDTKhongMa ThongDiepGuiHDDTKhongMa { get; set; }
        public HoaDonDienTu HoaDonDienTu { get; set; }
    }
}
