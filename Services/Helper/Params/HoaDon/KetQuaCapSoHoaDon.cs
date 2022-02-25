using System.Numerics;

namespace Services.Helper.Params.HoaDon
{
    public class KetQuaCapSoHoaDon
    {
        public KetQuaCapSoHoaDon()
        {
            IsYesNo = false;
            IsAcceptHetHieuLucTrongKhoang = false;
            IsAcceptNgayKyLonHonNgayHoaDon = false;
            IsCoHoaDonNhoHonHoaDonDangPhatHanh = false;
        }

        public bool? IsCoHoaDonNhoHonHoaDonDangPhatHanh { get; set; }
        public bool? IsCoCanhBaoChenhLech { get; set; }
        public bool? IsAcceptHetHieuLucTrongKhoang { get; set; }
        public bool? IsAcceptNgayKyLonHonNgayHoaDon { get; set; }
        public string TitleMessage { get; set; }
        public long? SoHoaDon { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public bool? IsYesNo { get; set; }
    }
}
