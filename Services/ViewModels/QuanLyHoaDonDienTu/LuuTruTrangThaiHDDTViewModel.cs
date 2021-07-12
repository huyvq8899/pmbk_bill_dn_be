using Services.ViewModels.QuanLyHoaDonDienTu;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels.QuanLyHoaDonDienTu
{
    public class LuuTruTrangThaiFileHDDTViewModel
    {
        public string HoaDonDienTuId { get; set; }

        public byte[] PdfChuaKy { get; set; }
        public byte[] PdfDaKy { get; set; }
        public byte[] XMLChuaKy { get; set; }
        public byte[] XMLDaKy { get; set; }

        public HoaDonDienTuViewModel HoaDonDienTu { get; set; }
    }
}
