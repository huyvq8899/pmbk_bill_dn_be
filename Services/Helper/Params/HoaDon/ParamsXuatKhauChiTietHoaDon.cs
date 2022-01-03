using Services.ViewModels.QuanLyHoaDonDienTu;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helper.Params.HoaDon
{
    public class ParamsXuatKhauChiTietHoaDon
    {
        public int Mode { get; set; }
        public string KhachHangId { get; set; }
        public string LoaiHoaDon { get; set; }
        public int? HinhThucHoaDon { get; set; }
        public int? UyNhiemLapHoaDon { get; set; }
        public string TuNgay { get; set; }
        public string DenNgay { get; set; }
        public string BoKyHieuHoaDonId { get; set; }
        public string TrangThaiHoaDon { get; set; }
        public string TrangThaiQuyTrinh { get; set; }
        public string TrangThaiGuiHoaDon { get; set; }
        public int? TrangThaiChuyenDoi { get; set; }
        public string HoaDonDienTuId { get; set; }
        public List<string> HoaDonDienTuIds { get; set; }
    }
}
