using DLL.Enums;

namespace Services.ViewModels.DanhMuc
{
    public class MauHoaDonViewModel : ThongTinChungViewModel
    {
        public MauHoaDonViewModel()
        {

        }

        public string MauHoaDonId { get; set; }
        public string Ten { get; set; }
        public int? SoThuTu { get; set; }
        public string MauSo { get; set; }
        public string KyHieu { get; set; }
        public string TenBoMau { get; set; }
    }
}
