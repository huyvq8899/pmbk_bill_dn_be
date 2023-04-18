using Services.ViewModels.QuanLyHoaDonDienTu;
using System.Collections.Generic;

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
            IsCoCanhBaoChenhLechThanhTien = false;
            IsCoCanhBaoChenhLechTienChietKhau = false;
            IsCoCanhBaoChenhLechTienThueGTGT = false;
        }

        public bool? IsCoHoaDonNhoHonHoaDonDangPhatHanh { get; set; }
        public bool? IsCoCanhBaoChenhLechThanhTien { get; set; }
        public bool? IsCoCanhBaoChenhLechTienChietKhau { get; set; }
        public bool? IsCoCanhBaoChenhLechTienThueGTGT { get; set; }
        public bool? IsAcceptHetHieuLucTrongKhoang { get; set; }
        public bool? IsAcceptNgayKyLonHonNgayHoaDon { get; set; }
        public string TitleMessage { get; set; }
        public long? SoHoaDon { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public bool? IsYesNo { get; set; }
        public HoaDonDienTuViewModel HoaDon { get; set; }
        public List<HoaDonDienTuViewModel> HoaDons { get; set; }
    }
}
