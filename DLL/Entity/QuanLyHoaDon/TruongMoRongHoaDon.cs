using DLL.Enums;

namespace DLL.Entity.QuanLyHoaDon
{
    public class TruongMoRongHoaDon : ThongTinChung
    {
        public string TruongMoRongHoaDonId { get; set; }
        public string HoaDonDienTuId { get; set; }
        public string MauHoaDonTuyChinhChiTietId { get; set; }
        public string GiaTri { get; set; }
        public LoaiTruongMoRong Loai { get; set; }

        public HoaDonDienTu HoaDonDienTu { get; set; }
    }
}
