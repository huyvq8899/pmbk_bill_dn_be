using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels.BaoCao
{
    public class BangKeHangHoaBanRaViewModel
    {
        public string KyHieu { get; set; }
        public string SoHoaDon { get; set; }
        public DateTime NgayHoaDon { get; set; }
        public string TenKhachHang { get; set; }
        public string MaSoThue { get; set; }
        public decimal TongTienChuaThue { get; set; }
        public string ThueSuat { get; set; }
        public decimal TongTienThueGTGT { get; set; }
    }
}
