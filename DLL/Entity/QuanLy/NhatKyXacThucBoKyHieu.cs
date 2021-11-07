using DLL.Enums;

namespace DLL.Entity.QuanLy
{
    public class NhatKyXacThucBoKyHieu : ThongTinChung
    {
        public string NhatKyXacThucBoKyHieuId { get; set; }
        public string BoKyHieuHoaDonId { get; set; }
        public TrangThaiSuDung TrangThaiSuDung { get; set; }
        public string NoiDung { get; set; }
        public string MauHoaDonId { get; set; }
        public string ToKhaiId { get; set; }

        public BoKyHieuHoaDon BoKyHieuHoaDon { get; set; }
    }
}
