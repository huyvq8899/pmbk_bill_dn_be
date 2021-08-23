using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Entity.Config
{
    public class ThietLapTruongDuLieuMoRong
    {
        public int STT { get; set; }
        public string Id { get; set; }
        public string TenTruong { get; set; }
        public string TenTruongHienThi { get; set; }
        public int LoaiHoaDon { get; set; } // 1: hóa đơn GTGT, 2: hóa đơn bán hàng
        public string GhiChu { get; set; }
        public bool HienThi { get; set; }
    }
}
