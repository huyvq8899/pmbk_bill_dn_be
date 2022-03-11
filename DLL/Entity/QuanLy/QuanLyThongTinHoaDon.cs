using DLL.Enums;
using System;

namespace DLL.Entity.QuanLy
{
    public class QuanLyThongTinHoaDon
    {
        public string QuanLyThongTinHoaDonId { get; set; }
        public double STT { get; set; }
        public int LoaiThongTin { get; set; } // 1: Hình thức hóa đơn, 2: Loại hóa đơn
        public LoaiThongTinChiTiet LoaiThongTinChiTiet { get; set; }
        public TrangThaiSuDung2 TrangThaiSuDung { get; set; }
        public DateTime? NgayBatDauSuDung { get; set; }
        public DateTime? TuNgayTamNgungSuDung { get; set; }
        public DateTime? DenNgayTamNgungSuDung { get; set; }
        public DateTime? NgayNgungSuDung { get; set; }
    }
}
