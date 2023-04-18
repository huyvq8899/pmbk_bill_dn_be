using System.Collections.Generic;

namespace Services.ViewModels.XML.ThongBaoKetQuaHuyHoaDon
{
    public partial class HoaDon
    {
        public List<ChiTiet> ChiTiet { get; set; }
    }

    public partial class ChiTiet
    {
        public string TenHDon { get; set; }
        public string MauHDon { get; set; }
        public string KyHieu { get; set; }
        public string SoLuong { get; set; }
        public string TuSo { get; set; }
        public string DenSo { get; set; }
    }
}
