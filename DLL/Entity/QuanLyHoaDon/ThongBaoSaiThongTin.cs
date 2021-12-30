using DLL.Entity.DanhMuc;
using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Entity.QuanLyHoaDon
{
    public class ThongBaoSaiThongTin
    {
        public string Id { get; set; }
        public string HoaDonDienTuId { get; set; }
        public string DoiTuongId { get; set; }
        public string HoTenNguoiMuaHang_Sai { get; set; }
        public string HoTenNguoiMuaHang_Dung { get; set; }
        public string TenDonVi_Sai { get; set; }
        public string TenDonVi_Dung { get; set; }
        public string DiaChi_Sai { get; set; }
        public string DiaChi_Dung { get; set; }

        public string TenNguoiNhan { get; set; }
        public string EmailCuaNguoiNhan { get; set; }
        public string EmailCCCuaNguoiNhan { get; set; }
        public string EmailBCCCuaNguoiNhan { get; set; }
        public string SDTCuaNguoiNhan { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string ModifyBy { get; set; }
    }
}
