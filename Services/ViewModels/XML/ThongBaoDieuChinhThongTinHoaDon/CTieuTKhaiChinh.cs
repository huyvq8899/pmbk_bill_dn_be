namespace Services.ViewModels.XML.ThongBaoDieuChinhThongTinHoaDon
{
    public partial class CTieuTKhaiChinh
    {
        public string ngayTBaoPHanhHDon { get; set; }
        public TTinThayDoi TTinThayDoi { get; set; }
        public TTinDonViChuQuan TTinDonViChuQuan { get; set; }
        public string tenCQTTiepNhanTBao { get; set; }
        public string ngayThongBao { get; set; }
        public string nguoiDaiDien { get; set; }
    }

    public class TTinThayDoi
    {
        public ChiTietTTinThayDoi ChiTietTTinThayDoi { get; set; }
    }

    public class ChiTietTTinThayDoi
    {
        public string thongTinThayDoi { get; set; }
        public string thongTinCu { get; set; }
        public string thongTinMoi { get; set; }
    }

    public class TTinDonViChuQuan
    {
        public string tenDViChuQuan { get; set; }
        public string mstDViChuQuan { get; set; }
    }
}
