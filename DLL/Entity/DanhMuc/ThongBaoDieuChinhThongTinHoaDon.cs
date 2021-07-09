using DLL.Enums;
using System;
using System.Collections.Generic;

namespace DLL.Entity.DanhMuc
{
    public class ThongBaoDieuChinhThongTinHoaDon : ThongTinChung
    {
        public string ThongBaoDieuChinhThongTinHoaDonId { get; set; }
        public DateTime? NgayThongBaoDieuChinh { get; set; }
        public DateTime? NgayThongBaoPhatHanh { get; set; }
        public string CoQuanThue { get; set; }
        public string So { get; set; }
        public TrangThaiNop TrangThaiNop { get; set; } // bỏ DaDuocChapNhan

        public string TenDonViCu { get; set; }
        public string TenDonViMoi { get; set; }
        public string DiaChiCu { get; set; }
        public string DiaChiMoi { get; set; }
        public string DienThoaiCu { get; set; }
        public string DienThoaiMoi { get; set; }

        public List<ThongBaoDieuChinhThongTinHoaDonChiTiet> ThongBaoDieuChinhThongTinHoaDonChiTiets { get; set; }
    }
}
