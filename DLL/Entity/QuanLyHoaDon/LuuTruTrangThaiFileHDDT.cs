using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Entity.QuanLyHoaDon
{
    public class LuuTruTrangThaiFileHDDT
    {
        public string HoaDonDienTuId { get; set; }
        public string Id { get; set; }
        public string PdfChuaKy { get; set; }
        public string PdfDaKy { get; set; }
        public string XMLChuaKy { get; set; }
        public string XMLDaKy { get; set; }

        public virtual HoaDonDienTu HoaDonDienTu { get; set; }
    }
}
