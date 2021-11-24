using DLL.Entity;
using DLL.Enums;

namespace Services.ViewModels.DanhMuc
{
    public class MauHoaDonThietLapMacDinhViewModel : ThongTinChung
    {
        public string MauHoaDonThietLapMacDinhId { get; set; }
        public string MauHoaDonId { get; set; }
        public LoaiThietLapMacDinh Loai { get; set; }
        public string GiaTri { get; set; }
        public string GiaTriBoSung { get; set; }
        public string ImgBase64 { get; set; }
    }
}
