using Services.ViewModels.DanhMuc;
using System;

namespace Services.ViewModels.QuanLyHoaDonDienTu
{
    public class HoaDonSaiSotViewModel
    {
        public string HoaDonDienTuId { get; set; }
        public string MaCQTCap { get; set; }
        public string MauHoaDon { get; set; }
        public string KyHieuHoaDon { get; set; }
        public string SoHoaDon { get; set; }
        public DateTime? NgayLapHoaDon { get; set; }
        public byte LoaiApDungHDDT { get; set; }
        public byte PhanLoaiHDSaiSot { get; set; }
        public string LyDo { get; set; }
    }
}
