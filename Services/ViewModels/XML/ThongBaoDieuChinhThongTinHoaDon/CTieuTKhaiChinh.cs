using System.Collections.Generic;

namespace Services.ViewModels.XML.ThongBaoDieuChinhThongTinHoaDon
{
    public partial class CTieuTKhaiChinh
    {
        public string NgayTBaoPHanhHDon { get; set; }
        public List<ChiTietTTinThayDoi> TTinThayDoi { get; set; }
        public TTinDonViChuQuan TTinDonViChuQuan { get; set; }
        public string TenCQTTiepNhanTBao { get; set; }
        public string NgayThongBao { get; set; }
        public string NguoiDaiDien { get; set; }
    }

    public class TTinThayDoi
    {
        public List<ChiTietTTinThayDoi> ChiTietTTinThayDoi { get; set; }
    }

    public class ChiTietTTinThayDoi
    {
        public string ThongTinThayDoi { get; set; }
        public string ThongTinCu { get; set; }
        public string ThongTinMoi { get; set; }
    }

    public class TTinDonViChuQuan
    {
        public string TenDViChuQuan { get; set; }
        public string MstDViChuQuan { get; set; }
    }
}
