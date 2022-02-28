using DLL.Enums;

namespace Services.ViewModels.DanhMuc
{
    public class MauHoaDonFileViewModel : ThongTinChungViewModel
    {
        public string MauHoaDonFileId { get; set; }
        public string MauHoaDonId { get; set; }
        public HinhThucMauHoaDon Type { get; set; }
        public string FileName { get; set; }
        public byte[] Binary { get; set; }

        public MauHoaDonViewModel MauHoaDon { get; set; }
    }
}
