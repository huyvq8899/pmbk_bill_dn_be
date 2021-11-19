using DLL.Entity.DanhMuc;
using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Entity.QuanLyHoaDon
{
    public class ThongTinHoaDon
    {
        public string Id { get; set; }
        public byte HinhThucApDung { get; set; }
        public int LoaiHoaDon { get; set; }
        public string MaCQTCap { get; set; }
        public string MauSoHoaDon { get; set; }
        public string KyHieuHoaDon { get; set; }
        public string SoHoaDon { get; set; }
        public DateTime? NgayHoaDon { get; set; }
        public string FileDinhKem { get; set; }
    }
}
