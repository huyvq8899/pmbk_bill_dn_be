namespace Services.ViewModels.XML.ThongBaoPhatHanhHoaDon
{
    public class TKhaiThue
    {
        public string MaTKhai { get; set; }
        public string TenTKhai { get; set; }
        public string MoTaBMau { get; set; }
        public string PbanTKhaiXML { get; set; }
        public string LoaiTKhai { get; set; }
        public string SoLan { get; set; }
        public KyKKhaiThue KyKKhaiThue { get; set; }
        public string MaCQTNoiNop { get; set; }
        public string TenCQTNoiNop { get; set; }
        public string NgayLapTKhai { get; set; }
        public GiaHan GiaHan { get; set; }
        public string NguoiKy { get; set; }
        public string NgayKy { get; set; }
        public string NganhNgheKD { get; set; }
    }

    public class KyKKhaiThue
    {
        public string KieuKy { get; set; }
        public string KyKKhai { get; set; }
        public string KyKKhaiTuNgay { get; set; }
        public string KyKKhaiDenNgay { get; set; }
        public string KyKKhaiTuThang { get; set; }
        public string KyKKhaiDenThang { get; set; }
    }

    public class GiaHan
    {
        public string MaLyDoGiaHan { get; set; }
        public string LyDoGiaHan { get; set; }
    }
}
