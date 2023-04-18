using DLL.Enums;

namespace DLL.Entity.DanhMuc
{
    public class MauHoaDonThietLapMacDinh : ThongTinChung
    {
        public string MauHoaDonThietLapMacDinhId { get; set; }
        public string MauHoaDonId { get; set; }
        public LoaiThietLapMacDinh Loai { get; set; }
        public string GiaTri { get; set; }
        public string GiaTriBoSung { get; set; }

        public MauHoaDon MauHoaDon { get; set; }
    }
}
