namespace Services.ViewModels.XML.ThongBaoPhatHanhHoaDon
{
    public class HoaDon
    {
        public ChiTiet ChiTiet { get; set; }
    }

    public class ChiTiet
    {
        public string tenLoaiHDon { get; set; }
        public string mauSo { get; set; }
        public string kyHieu { get; set; }
        public string soLuong { get; set; }
        public string tuSo { get; set; }
        public string denSo { get; set; }
        public string ngayBDauSDung { get; set; }
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
