using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Entity.QuanLyHoaDon
{
    public class LuuTruTrangThaiFileHDDT
    {
        public string HoaDonDienTuId { get; set; }
        public string Id { get; set; }
        public byte[] PdfDaKy { get; set; }
        public byte[] XMLDaKy { get; set; }
        public virtual HoaDonDienTu HoaDonDienTu { get; set; }
    }
}
