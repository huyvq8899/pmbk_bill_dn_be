using System;

namespace DLL.Entity.QuanLyHoaDon
{
    public class BienBanDieuChinh : ThongTinChung
    {
        public string BienBanDieuChinhId { get; set; }
        public string NoiDungBienBan { get; set; }
        public DateTime? NgayBienBan { get; set; }
        public string TenDonViBenA { get; set; }
        public string DiaChiBenA { get; set; }
        public string MaSoThueBenA { get; set; }
        public string SoDienThoaiBenA { get; set; }
        public string DaiDienBenA { get; set; }
        public string ChucVuBenA { get; set; }
        public DateTime? NgayKyBenA { get; set; }
        public string TenDonViBenB { get; set; }
        public string DiaChiBenB { get; set; }
        public string MaSoThueBenB { get; set; }
        public string SoDienThoaiBenB { get; set; }
        public string DaiDienBenB { get; set; }
        public string ChucVuBenB { get; set; }
        public DateTime? NgayKyBenB { get; set; }
        public string LyDoDieuChinh { get; set; }
        public string FileDaKy { get; set; }
        public string FileChuaKy { get; set; }
        public string XMLChuaKy { get; set; }
        public string XMLDaKy { get; set; }

        public string HoaDonDienTuId { get; set; }
        public virtual HoaDonDienTu HoaDonDienTu { get; set; }
    }
}
