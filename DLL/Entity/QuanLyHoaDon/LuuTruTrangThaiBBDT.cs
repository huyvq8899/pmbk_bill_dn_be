namespace DLL.Entity.QuanLyHoaDon
{
    public class LuuTruTrangThaiBBDT : ThongTinChung
    {
        public string LuuTruTrangThaiBBDTId { get; set; }
        public string BienBanDieuChinhId { get; set; }
        public byte[] PdfDaKy { get; set; }
        public byte[] XMLDaKy { get; set; }

        public BienBanDieuChinh BienBanDieuChinh { get; set; }
    }
}
