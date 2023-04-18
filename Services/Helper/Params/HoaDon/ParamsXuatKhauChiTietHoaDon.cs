using Services.ViewModels.QuanLyHoaDonDienTu;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helper.Params.HoaDon
{
    public class ParamsXuatKhauChiTietHoaDon
    {
        public int Mode { get; set; }
        public List<string> KhachHangId { get; set; }
        public List<int> LoaiHoaDon { get; set; }
        public int? HinhThucHoaDon { get; set; }
        public int? UyNhiemLapHoaDon { get; set; }
        public string TuNgay { get; set; }
        public string DenNgay { get; set; }
        public List<string> BoKyHieuHoaDonId { get; set; }
        public List<int> TrangThaiHoaDon { get; set; }
        public List<int> TrangThaiQuyTrinh { get; set; }
        public List<int> TrangThaiGuiHoaDon { get; set; }
        public int? TrangThaiChuyenDoi { get; set; }
        public string HoaDonDienTuId { get; set; }
        public List<string> HoaDonDienTuIds { get; set; }
        public int? LoaiNghiepVu { get; set; } // 1 hoa don, 2 phieu xuat kho
    }
}
