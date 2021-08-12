using System.Collections.Generic;

namespace Services.ViewModels.XML.ThongBaoKetQuaHuyHoaDon
{
    public partial class HoaDon
    {
        public List<ChiTiet> ChiTiet { get; set; }
    }

    public partial class ChiTiet
    {
        public string tenHDon { get; set; }
        public string mauHDon { get; set; }
        public string kyHieu { get; set; }
        public string soLuong { get; set; }
        public string tuSo { get; set; }
        public string denSo { get; set; }
    }
}
