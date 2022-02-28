using DLL.Enums;

namespace DLL.Entity.DanhMuc
{
    public class MauHoaDonFile : ThongTinChung
    {
        public string MauHoaDonFileId { get; set; }
        public string MauHoaDonId { get; set; }
        public HinhThucMauHoaDon Type { get; set; }
        public string FileName { get; set; }
        public byte[] Binary { get; set; }

        public MauHoaDon MauHoaDon { get; set; }
    }
}
