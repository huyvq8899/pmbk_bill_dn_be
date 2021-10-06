using System.Collections.Generic;

namespace Services.ViewModels.XML.ThongBaoPhatHanhHoaDon
{
    public partial class HoaDon
    {
        public List<ChiTiet> ChiTiet { get; set; }
    }

    public partial class ChiTiet
    {
        public string TenLoaiHDon { get; set; }
        public string MauSo { get; set; }
        public string KyHieu { get; set; }
        public string SoLuong { get; set; }
        public string TuSo { get; set; }
        public string DenSo { get; set; }
        public string NgayBDauSDung { get; set; }
        public DoanhNghiepIn DoanhNghiepIn { get; set; }
        public HopDongDatIn HopDongDatIn { get; set; }
    }

    public class DoanhNghiepIn
    {
        public string Ten { get; set; }
        public string Mst { get; set; }
    }

    public class HopDongDatIn
    {
        public string So { get; set; }
        public string Ngay { get; set; }
    }
}
