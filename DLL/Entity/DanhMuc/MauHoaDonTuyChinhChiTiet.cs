using DLL.Enums;

namespace DLL.Entity.DanhMuc
{
    public class MauHoaDonTuyChinhChiTiet : ThongTinChung
    {
        public string MauHoaDonTuyChinhChiTietId { get; set; }
        public string MauHoaDonId { get; set; }
        public string GiaTri { get; set; }
        public string TuyChinhChiTiet { get; set; }
        public string TenTiengAnh { get; set; }
        public KieuDuLieuThietLapTuyChinh KieuDuLieuThietLap { get; set; }
        public LoaiTuyChinhChiTiet Loai { get; set; }
        public LoaiChiTietTuyChonNoiDung LoaiChiTiet { get; set; }
        public LoaiContainerTuyChinh LoaiContainer { get; set; }
        public bool? IsParent { get; set; }
        public bool? Checked { get; set; }
        public bool? Disabled { get; set; }

        public MauHoaDon MauHoaDon { get; set; }
    }
}
