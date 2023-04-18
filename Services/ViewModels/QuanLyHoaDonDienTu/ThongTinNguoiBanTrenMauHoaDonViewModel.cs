using DLL.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels.QuanLyHoaDonDienTu
{
    /// <summary>
    /// Lớp này thể hiện thông tin người bán lấy từ mẫu hóa đơn.
    /// </summary>
    public class ThongTinNguoiBanTrenMauHoaDonViewModel
    {
        /// <summary>
        /// Tên đơn vị/người bán.
        /// </summary>
        public string TenDonViNguoiBan { get; set; }

        /// <summary>
        /// Mã số thuế của người bán.
        /// </summary>
        public string MaSoThueNguoiBan { get; set; }

        /// <summary>
        /// Địa chỉ của người bán.
        /// </summary>
        public string DiaChiNguoiBan { get; set; }

        /// <summary>
        /// Tên bộ ký hiệu.
        /// </summary>
        public string TenBoKyHieu { get; set; }
        /// <summary>
        /// Mã số thuế giải pháp
        /// </summary>
        public string MaSoThueGiaiPhap { get; set; }
        public string SoTaiKhoanNguoiBan { get; set; }
        public string TenNganHangNguoiBan { get; set; }
        public string FaxNguoiBan { get; set; }
        public string WebsiteNguoiBan { get; set; }
        public string EmailNguoiBan { get; set; }
        public string DienThoaiNguoiBan { get; set; }

    }
}
