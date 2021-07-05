using DLL.Entity.DanhMuc;
using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Entity.QuanLyHoaDon
{
    public class NhatKyThaoTacHoaDon : ThongTinChung
    {
        public string Id { get; set; }
        public DateTime NgayGio { get; set; }
        public string KhachHangId { get; set; }
        public string MoTa { get; set; }
        public DoiTuong KhachHang { get; set; }
        public string ErrorMessage { get; set; }
        public string HasError { get; set; }
        public int LoaiThaoTac { get; set; }
    }
}
