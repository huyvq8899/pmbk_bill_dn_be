using System;

namespace DLL.Entity.QuanLyHoaDon
{
    public class ThongTinHoaDon
    {
        public string Id { get; set; }
        public byte HinhThucApDung { get; set; }
        public int? TrangThaiHoaDon { get; set; }
        public int LoaiHoaDon { get; set; }
        public string MaCQTCap { get; set; }
        public string MauSoHoaDon { get; set; }
        public string KyHieuHoaDon { get; set; }
        public string SoHoaDon { get; set; }
        public DateTime? NgayHoaDon { get; set; }
        public string FileDinhKem { get; set; }

        public string MaTraCuu { get; set; }
        public decimal? ThanhTien { get; set; }
        public string LoaiTienId { get; set; }

        public bool? IsDaLapThongBao04 { get; set; }
        public int? TrangThaiGui04 { get; set; }
        public int? LanGui04 { get; set; }
        public string ThongDiepGuiCQTId { get; set; }

        public int TrangThaiBienBanXoaBo { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string ModifyBy { get; set; }
    }
}
