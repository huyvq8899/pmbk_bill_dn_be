using ManagementServices.Helper;
using Services.Helper.LogHelper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.DanhMuc
{
    public class HoSoHDDTViewModel : ThongTinChungViewModel
    {
        [IgnoreLogging]
        public string HoSoHDDTId { get; set; }

        [IgnoreLogging]
        public string MaSoThue { get; set; }

        [Display(Name = "Tên đơn vị")]
        public string TenDonVi { get; set; }

        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }

        [Display(Name = "Ngành nghề kinh doanh chính")]
        public string NganhNgheKinhDoanhChinh { get; set; }

        [Display(Name = "Người đại điện phát luật")]
        public string HoTenNguoiDaiDienPhapLuat { get; set; }

        [Display(Name = "Email người đại diện pháp luật")]
        public string EmailNguoiDaiDienPhapLuat { get; set; }

        [Display(Name = "SĐT người đại diện pháp luật")]
        public string SoDienThoaiNguoiDaiDienPhapLuat { get; set; }

        [Display(Name = "Số tài khoản ngân hàng")]
        public string SoTaiKhoanNganHang { get; set; }

        [Display(Name = "Tên ngân hàng")]
        public string TenNganHang { get; set; }

        [Display(Name = "Chi nhánh")]
        public string ChiNhanh { get; set; }

        [Display(Name = "Email liên hệ")]
        public string EmailLienHe { get; set; }

        [Display(Name = "SĐT liên hệ")]
        public string SoDienThoaiLienHe { get; set; }

        [Display(Name = "Fax")]
        public string Fax { get; set; }

        [Display(Name = "Website")]
        public string Website { get; set; }

        [Display(Name = "Cơ quan thuế cấp cục")]
        public string TenCoQuanThueCapCuc { get; set; }

        [Display(Name = "Cơ quan thuế quản lý")]
        public string TenCoQuanThueQuanLy { get; set; }

        [IgnoreLogging]
        public string CoQuanThueCapCuc { get; set; }
        [IgnoreLogging]
        public string CoQuanThueQuanLy { get; set; }
        [IgnoreLogging]
        public List<CityParam> CoQuanThueCapCucs { get; set; }
        [IgnoreLogging]
        public List<DistrictsParam> CoQuanThueQuanLys { get; set; }
    }
}
