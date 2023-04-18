using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Text;

namespace DLL.Entity.QuanLy
{
    public class SinhSoHDMayTinhTien : ThongTinChung
    {
        [Key]
        public Guid SinhSoHDMayTinhTienId { get; set; }
        public int NamPhatHanh { get; set; }
        public long SoBatDau { get; set; }
        public long SoKetThuc { get; set; }
        public bool IsUpdateSoBatDau { get; set; }
        public bool IsUpdateSoKetThuc { get; set; }
    }
}
