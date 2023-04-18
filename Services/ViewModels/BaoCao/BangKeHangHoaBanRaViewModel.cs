using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Services.ViewModels.BaoCao
{
    public class BangKeHangHoaBanRaViewModel
    {
        public string KyHieu { get; set; }
        public long? SoHoaDon { get; set; }
        public DateTime NgayHoaDon { get; set; }
        public string TenKhachHang { get; set; }
        public string MaSoThue { get; set; }
        public decimal TongTienChuaThue { get; set; }
        public string ThueSuat { get; set; }
        public decimal TongTienThueGTGT { get; set; }
        public string GhiChu { get; set; }
        public int TrangThai { get; set; }
        public string HoaDonDienTuId { get; set; }
    }
}
