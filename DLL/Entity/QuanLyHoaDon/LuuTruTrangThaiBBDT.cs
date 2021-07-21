namespace DLL.Entity.QuanLyHoaDon
{
    public class LuuTruTrangThaiBBDT : ThongTinChung
    {
        public string LuuTruTrangThaiBBDTId { get; set; }
        public string BienBanDieuChinhId { get; set; }
        public byte[] PdfChuaKy { get; set; }
        public byte[] PdfDaKy { get; set; }
        public byte[] XMLChuaKy { get; set; }
        public byte[] XMLDaKy { get; set; }

        public virtual BienBanDieuChinh BienBanDieuChinh { get; set; }
    }
}
