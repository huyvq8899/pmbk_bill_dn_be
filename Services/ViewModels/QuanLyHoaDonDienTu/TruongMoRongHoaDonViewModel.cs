using DLL.Enums;

namespace Services.ViewModels.QuanLyHoaDonDienTu
{
    public class TruongMoRongHoaDonViewModel : ThongTinChungViewModel
    {
        public string TruongMoRongHoaDonId { get; set; }
        public string HoaDonDienTuId { get; set; }
        public string MauHoaDonTuyChinhChiTietId { get; set; }
        public string GiaTri { get; set; }

        public string TenTruong { get; set; }
        public string TenTruongTiengAnh { get; set; }
        public KieuDuLieuThietLapTuyChinh KieuDuLieu { get; set; }
        public LoaiTuyChinhChiTiet Loai { get; set; }
        public LoaiChiTietTuyChonNoiDung LoaiChiTiet { get; set; }
        public string GiaTriMacDinh { get; set; }
        public int? DoRong { get; set; }
        public bool? IsHienThi { get; set; }
    }
}
