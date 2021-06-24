using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Entity.DanhMuc
{
    public class MauHoaDon
    {
        public string MauHoaDonId { get; set; }
        public string MauSo { get; set; }
        public string TenMauSo { get; set; }
        public string DienGiai { get; set; }
        public bool? TuNhap { get; set; }
        public bool Status { get; set; }
        public int? STT { get; set; }
    }
}
