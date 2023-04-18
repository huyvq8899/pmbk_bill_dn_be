namespace Services.ViewModels.QuanLyHoaDonDienTu
{
    public class LuuTruTrangThaiBBDTViewModel : ThongTinChungViewModel
    {
        public string LuuTruTrangThaiBBDTId { get; set; }
        public string BienBanDieuChinhId { get; set; }
        public byte[] PdfDaKy { get; set; }
        public byte[] XMLDaKy { get; set; }
    }
}
