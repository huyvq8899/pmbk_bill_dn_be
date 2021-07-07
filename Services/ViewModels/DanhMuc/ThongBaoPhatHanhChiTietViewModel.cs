using System;

namespace Services.ViewModels.DanhMuc
{
    public class ThongBaoPhatHanhChiTietViewModel : ThongTinChungViewModel
    {
        public string ThongBaoPhatHanhChiTietId { get; set; }
        public string ThongBaoPhatHanhId { get; set; }
        public string MauHoaDonId { get; set; }
        public string KyHieu { get; set; }
        public int? SoLuong { get; set; }
        public int? TuSo { get; set; }
        public int? DenSo { get; set; }
        public DateTime? NgayBatDauSuDung { get; set; }

        public bool? Checked { get; set; }
        public string TenLoaiHoaDon { get; set; }
        public string MauSoHoaDon { get; set; }

        public ThongBaoPhatHanhViewModel ThongBaoPhatHanh { get; set; }
        public MauHoaDonViewModel MauHoaDon { get; set; }
    }
}
