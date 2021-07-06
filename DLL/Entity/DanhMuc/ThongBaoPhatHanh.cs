using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Entity.DanhMuc
{
    public class ThongBaoPhatHanh
    {
        public string Id { get; set; }
        public DateTime? NgayPhatHanh { get; set; }
        public string MauHoaDonId { get; set; }
        public virtual MauHoaDon MauHoaDon { get; set; }
        public int ChoPhepPhatHanhMin { get; set; }
        public int ChoPhepPhatHanhMax { get; set; }
    }
}
