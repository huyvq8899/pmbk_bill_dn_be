using System.Collections.Generic;

namespace Services.ViewModels.XML.ThongBaoKetQuaHuyHoaDon
{
    public partial class CTieuTKhaiChinh
    {
        public string KinhGui { get; set; }
        public string PhuongPhapHuy { get; set; }
        public string ThoiGian { get; set; }
        public List<ChiTiet> HoaDon { get; set; }
        public string NguoiLapBieu { get; set; }
        public string NguoiDaiDien { get; set; }
        public string NgayBCao { get; set; }
    }
}
