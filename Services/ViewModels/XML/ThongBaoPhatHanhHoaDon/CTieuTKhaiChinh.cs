using System.Collections.Generic;

namespace Services.ViewModels.XML.ThongBaoPhatHanhHoaDon
{
    public partial class CTieuTKhaiChinh
    {
        public List<ChiTiet> HoaDon { get; set; }
        public DonViChuQuan DonViChuQuan { get; set; }
        public string tenCQTTiepNhan { get; set; }
        public string nguoiDaiDien { get; set; }
        public string ngayBCao { get; set; }
    }
}
