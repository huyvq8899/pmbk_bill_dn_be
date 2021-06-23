﻿using ManagementServices.Helper;
using System.Collections.Generic;

namespace Services.ViewModels.DanhMuc
{
    public class HoSoHDDTViewModel : ThongTinChungViewModel
    {
        public string HoSoHDDTId { get; set; }
        public string MaSoThue { get; set; }
        public string TenDonVi { get; set; }
        public string DiaChi { get; set; }
        public string NganhNgheKinhDoanhChinh { get; set; }
        public string HoTenNguoiDaiDienPhapLuat { get; set; }
        public string EmailNguoiDaiDienPhapLuat { get; set; }
        public string SoDienThoaiNguoiDaiDienPhapLuat { get; set; }
        public string SoTaiKhoanNganHang { get; set; }
        public string TenNganHang { get; set; }
        public string ChiNhanh { get; set; }
        public string EmailLienHe { get; set; }
        public string SoDienThoaiLienHe { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        public string CoQuanThueCapCuc { get; set; }
        public string CoQuanThueQuanLy { get; set; }

        public List<CityParam> CoQuanThueCapCucs { get; set; }
        public List<DistrictsParam> CoQuanThueQuanLys { get; set; }
    }
}
