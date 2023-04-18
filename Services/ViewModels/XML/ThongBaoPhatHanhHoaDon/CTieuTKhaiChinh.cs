using System.Collections.Generic;

namespace Services.ViewModels.XML.ThongBaoPhatHanhHoaDon
{
    public partial class CTieuTKhaiChinh
    {
        public List<ChiTiet> HoaDon { get; set; }
        public DonViChuQuan DonViChuQuan { get; set; }
        public string TenCQTTiepNhan { get; set; }
        public string NguoiDaiDien { get; set; }
        public string NgayBCao { get; set; }
    }
}
