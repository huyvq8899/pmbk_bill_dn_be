namespace Services.ViewModels.DanhMuc
{
    public class DoiTuongViewModel : ThongTinChungViewModel
    {
        public string DoiTuongId { get; set; }
        /// Khách hàng
        public int? LoaiKhachHang { get; set; } // 1: cá nhân, 2: tổ chức
        public string MaSoThue { get; set; }
        public string Ma { get; set; }
        public string Ten { get; set; }
        public string DiaChi { get; set; }
        public string SoTaiKhoanNganHang { get; set; }
        public string TenNganHang { get; set; }
        public string ChiNhanh { get; set; }
        public string HoTenNguoiMuaHang { get; set; }
        public string EmailNguoiMuaHang { get; set; }
        public string SoDienThoaiNguoiMuaHang { get; set; }
        public string HoTenNguoiNhanHD { get; set; }
        public string EmailNguoiNhanHD { get; set; }
        public string SoDienThoaiNguoiNhanHD { get; set; }

        /// Nhân viên
        public string ChucDanh { get; set; }
        public string TenDonVi { get; set; }

        //
        public bool? IsKhachHang { get; set; }
        public bool? IsNhanVien { get; set; }
    }
}
