using Services.Helper.LogHelper;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.DanhMuc
{
    public class DoiTuongViewModel : ThongTinChungViewModel
    {
        [IgnoreLogging]
        public string DoiTuongId { get; set; }

        [CheckBox]
        [Display(Name = "Là khách hàng")]
        public bool? IsKhachHang { get; set; }

        /// Khách hàng
        [IgnoreLogging]
        public int? LoaiKhachHang { get; set; } // 1: cá nhân, 2: tổ chức

        [Display(Name = "Loại khách hàng")]
        public string TenLoaiKhachHang { get; set; }

        [Display(Name = "Mã số thuế")]
        public string MaSoThue { get; set; }

        [Display(Name = "Mã")]
        public string Ma { get; set; }

        [Display(Name = "Tên")]
        public string Ten { get; set; }

        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }

        [Display(Name = "Số tài khoản ngân hàng")]
        public string SoTaiKhoanNganHang { get; set; }

        [Display(Name = "Tên ngân hàng")]
        public string TenNganHang { get; set; }

        [Display(Name = "Chi nhánh")]
        public string ChiNhanh { get; set; }

        [Display(Name = "Người mua hàng")]
        public string HoTenNguoiMuaHang { get; set; }

        [Display(Name = "Email người mua hàng")]
        public string EmailNguoiMuaHang { get; set; }

        [Display(Name = "SĐT người mua hàng")]
        public string SoDienThoaiNguoiMuaHang { get; set; }

        [Display(Name = "Người nhận HĐ")]
        public string HoTenNguoiNhanHD { get; set; }

        [Display(Name = "Email người nhận HĐ")]
        public string EmailNguoiNhanHD { get; set; }

        [Display(Name = "SĐT người nhận HĐ")]
        public string SoDienThoaiNguoiNhanHD { get; set; }

        /// Nhân viên
        [Display(Name = "Chức danh")]
        public string ChucDanh { get; set; }

        [Display(Name = "Tên đơn vị")]
        public string TenDonVi { get; set; }

        [IgnoreLogging]
        public bool? IsNhanVien { get; set; }
    }
}
