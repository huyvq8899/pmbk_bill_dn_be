using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels.QuanLyHoaDonDienTu
{
    public class ThongTinHoaDonViewModel
    {
        public string MauSoHoaDon { get; set; }
        public string KyHieuHoaDon { get; set; }
        public string SoHoaDon { get; set; }
        public string NgayHoaDon { get; set; }

        //LoaiHinhThuc = 1 hình thức hóa đơn thay thế, = 2 hình thức hóa đơn bị điều chỉnh
        public byte LoaiHinhThuc { get; set; }
    }
}
