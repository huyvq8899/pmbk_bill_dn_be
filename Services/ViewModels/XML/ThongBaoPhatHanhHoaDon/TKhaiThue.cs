namespace Services.ViewModels.XML.ThongBaoPhatHanhHoaDon
{
    public class TKhaiThue
    {
        public string maTKhai { get; set; }
        public string tenTKhai { get; set; }
        public string moTaBMau { get; set; }
        public string pbanTKhaiXML { get; set; }
        public string loaiTKhai { get; set; }
        public string soLan { get; set; }
        public KyKKhaiThue KyKKhaiThue { get; set; }
        public string maCQTNoiNop { get; set; }
        public string tenCQTNoiNop { get; set; }
        public string ngayLapTKhai { get; set; }
        public GiaHan GiaHan { get; set; }
        public string nguoiKy { get; set; }
        public string ngayKy { get; set; }
        public string nganhNgheKD { get; set; }
    }

    public class KyKKhaiThue
    {
        public string kieuKy { get; set; }
        public string kyKKhai { get; set; }
        public string kyKKhaiTuNgay { get; set; }
        public string kyKKhaiDenNgay { get; set; }
        public string kyKKhaiTuThang { get; set; }
        public string kyKKhaiDenThang { get; set; }
    }

    public class GiaHan
    {
        public string maLyDoGiaHan { get; set; }
        public string lyDoGiaHan { get; set; }
    }
}
