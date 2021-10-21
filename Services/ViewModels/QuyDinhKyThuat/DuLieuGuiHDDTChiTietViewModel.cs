using Services.ViewModels.QuanLyHoaDonDienTu;

namespace Services.ViewModels.QuyDinhKyThuat
{
    public class DuLieuGuiHDDTChiTietViewModel : ThongTinChungViewModel
    {
        public string ThongDiepGuiDuLieuHDDTChiTietId { get; set; }
        public string ThongDiepGuiDuLieuHDDTId { get; set; }
        public string HoaDonDienTuId { get; set; }
        //
        public HoaDonDienTuViewModel HoaDonDienTu { get; set; }
        //
        public string MaLoi { get; set; }
        public string MoTaLoi { get; set; }
        public string HuongDanXuLy { get; set; }
    }
}
