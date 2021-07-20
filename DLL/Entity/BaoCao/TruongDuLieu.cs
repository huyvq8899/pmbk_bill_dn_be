using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Entity.BaoCao
{
    public class TruongDuLieu
    {
        public string Id { get; set; }
        public int STT { get; set; }
        public string MaTruong { get; set; }
        public string TenTruong { get; set; }
        public string TenHienThi { get; set; }
        public bool Status { get; set; }
        public bool Default { get; set; }
        public bool HienThiKhiCongGop { get; set; } = true;
        public string NghiepVuId { get; set; }
        public NghiepVu NghiepVu { get; set; }
    }
}
