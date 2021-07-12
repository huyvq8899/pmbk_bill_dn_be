using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helper.Params.HoaDon
{
    public class ParamsXuatKhauChiTietHoaDon
    {
        public string KhachHangId { get; set; }
        public int LoaiHoaDon { get; set; }
        public string TuNgay { get; set; }
        public string DenNgay { get; set; }
        public string TenMauSo { get; set; }
        public string MauSo { get; set; }
        public int TrangThaiHoaDon { get; set; }
        public int TrangThaiPhatHanh { get; set; }
        public int TrangThaiGuiHoaDon { get; set; }
        public int TrangThaiChuyenDoi { get; set; }
        public string KhongDuocChon { get; set; }
    }
}
