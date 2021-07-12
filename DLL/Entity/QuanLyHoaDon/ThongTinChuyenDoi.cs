using DLL.Entity.DanhMuc;
using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Entity.QuanLyHoaDon
{
    public class ThongTinChuyenDoi
    {
        public string Id { get; set; }
        public string HoaDonDienTuId { get; set; }
        public virtual HoaDonDienTu HoaDonDienTu { get; set; }
        public DateTime NgayChuyenDoi { get; set; }
        public string NguoiChuyenDoiId { get; set; }
        public virtual DoiTuong NguoiChuyenDoi { get; set; }
    }
}
