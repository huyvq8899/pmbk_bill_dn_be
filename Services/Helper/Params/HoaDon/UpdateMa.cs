namespace Services.Helper.Params.HoaDon
{
    public class UpdateMa
    {
        public LoaiUpdateMa Loai { get; set; }
        public string RefId { get; set; }
        public string Ma { get; set; }
    }

    public enum LoaiUpdateMa
    {
        KhachHang,
        NhanVien,
        HangHoaDichVu,
        LoaiTien
    }
}
