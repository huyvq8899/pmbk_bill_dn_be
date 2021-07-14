using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels.QuanLyHoaDonDienTu
{
    public class LuuTruTrangThaiBBXBViewModel
    {
        public string BienBanXoaBoId { get; set; }
        public string Id { get; set; }
        public byte[] PdfChuaKy { get; set; }
        public byte[] PdfDaKy { get; set; }
        public byte[] XMLChuaKy { get; set; }
        public byte[] XMLDaKy { get; set; }

        public BienBanXoaBoViewModel BienBanXoaBo { get; set; }
    }
}
