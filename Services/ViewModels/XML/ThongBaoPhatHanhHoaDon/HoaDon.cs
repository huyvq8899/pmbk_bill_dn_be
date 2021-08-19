using System.Collections.Generic;

namespace Services.ViewModels.XML.ThongBaoPhatHanhHoaDon
{
    public partial class HoaDon
    {
        public List<ChiTiet> ChiTiet { get; set; }
    }

    public partial class ChiTiet
    {
        public string tenLoaiHDon { get; set; }
        public string mauSo { get; set; }
        public string kyHieu { get; set; }
        public string soLuong { get; set; }
        public string tuSo { get; set; }
        public string denSo { get; set; }
        public string ngayBDauSDung { get; set; }
        public DoanhNghiepIn DoanhNghiepIn { get; set; }
        public HopDongDatIn HopDongDatIn { get; set; }
    }

    public class DoanhNghiepIn
    {
        public string ten { get; set; }
        public string mst { get; set; }
    }

    public class HopDongDatIn
    {
        public string so { get; set; }
        public string ngay { get; set; }
    }
}
