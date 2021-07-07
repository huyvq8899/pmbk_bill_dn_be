using DLL.Enums;

namespace Services.ViewModels.DanhMuc
{
    public class MauHoaDonViewModel : ThongTinChungViewModel
    {
        public string MauHoaDonId { get; set; }
        public string Ten { get; set; }
        public int? SoThuTu { get; set; }
        public string MauSo { get; set; }
        public string KyHieu { get; set; }
        public string TenBoMau { get; set; }
        public string AnhMau { get; set; }
        public LoaiHoaDon LoaiHoaDon { get; set; }
        public LoaiMauHoaDon LoaiMauHoaDon { get; set; }
        public LoaiThueGTGT LoaiThueGTGT { get; set; }
        public LoaiNgonNgu LoaiNgonNgu { get; set; }
        public LoaiKhoGiay LoaiKhoGiay { get; set; }

        public string TenLoaiHoaDon { get; set; }
    }
}
