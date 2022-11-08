using DLL.Entity.DanhMuc;
using System;
using System.Collections.Generic;

namespace DLL.Entity.Ticket
{
    public class TuyenDuong : ThongTinChung
    {
        public Guid TuyenDuongId { get; set; }
        public string TenTuyenDuong { get; set; }
        public string BenDi { get; set; }
        public string BenDen { get; set; }
        public string ThoiGianKhoiHanh { get; set; }
        public string SoXe { get; set; }
        public string SoTuyen { get; set; }

        public List<MauHoaDon> MauHoaDons { get; set; }
    }
}
