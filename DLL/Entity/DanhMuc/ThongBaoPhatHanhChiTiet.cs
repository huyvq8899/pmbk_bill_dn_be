using System;

namespace DLL.Entity.DanhMuc
{
    public class ThongBaoPhatHanhChiTiet : ThongTinChung
    {
        public string ThongBaoPhatHanhChiTietId { get; set; }
        public string ThongBaoPhatHanhId { get; set; }
        public string MauHoaDonId { get; set; }
        public string KyHieu { get; set; }
        public int? SoLuong { get; set; }
        public int? TuSo { get; set; }
        public int? DenSo { get; set; }
        public DateTime? NgayBatDauSuDung { get; set; }

        public ThongBaoPhatHanh ThongBaoPhatHanh { get; set; }
        public MauHoaDon MauHoaDon { get; set; }
    }
}
