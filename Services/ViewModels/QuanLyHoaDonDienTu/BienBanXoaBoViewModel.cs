using Services.Helper.LogHelper;
using Services.ViewModels.DanhMuc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.QuanLyHoaDonDienTu
{
    public class BienBanXoaBoViewModel
    {
        [IgnoreLogging]
        public string Id { get; set; }

        [Display(Name = "Ngày biên bản")]
        public DateTime? NgayBienBan { get; set; }

        [Display(Name = "Số biên bản")]
        public string SoBienBan { get; set; }

        [IgnoreLogging]
        public string KhachHangId { get; set; }

        [Display(Name = "Thông tư")]
        public string ThongTu { get; set; }

        [IgnoreLogging]
        public DoiTuongViewModel KhachHang { get; set; }

        [Display(Name = "Tên đơn vị bên B")]
        public string TenKhachHang { get; set; }

        [Display(Name = "Địa chỉ bên B")]
        public string DiaChi { get; set; }

        [Display(Name = "Mã số thuế bên B")]
        public string MaSoThue { get; set; }

        [Display(Name = "Số điện thoại bên B")]
        public string SoDienThoai { get; set; }

        [Display(Name = "Đại diện bên B")]
        public string DaiDien { get; set; }

        [Display(Name = "Chức vụ bên B")]
        public string ChucVu { get; set; }

        [IgnoreLogging]
        public DateTime? NgayKyBenB { get; set; }

        [Display(Name = "Tên đơn vị bên A")]
        public string TenCongTyBenA { get; set; }

        [Display(Name = "Địa chỉ bên A")]
        public string DiaChiBenA { get; set; }

        [Display(Name = "Mã số thuế bên A")]
        public string MaSoThueBenA { get; set; }

        [Display(Name = "Số điện thoại bên A")]
        public string SoDienThoaiBenA { get; set; }

        [Display(Name = "Đại diện bên A")]
        public string DaiDienBenA { get; set; }

        [Display(Name = "Chức vụ bên A")]
        public string ChucVuBenA { get; set; }

        [IgnoreLogging]
        public DateTime? NgayKyBenA { get; set; }

        [IgnoreLogging]
        public string HoaDonDienTuId { get; set; }

        [IgnoreLogging]
        public virtual HoaDonDienTuViewModel HoaDonDienTu { get; set; }

        [Display(Name = "Lý do hủy hóa đơn")]
        public string LyDoXoaBo { get; set; }

        [IgnoreLogging]
        public string FileDaKy { get; set; }

        [IgnoreLogging]
        public string FileChuaKy { get; set; }

        [IgnoreLogging]
        public string XMLChuaKy { get; set; }

        [IgnoreLogging]
        public string XMLDaKy { get; set; }

        //gửi biên bản
        [IgnoreLogging]
        public string TenNguoiNhan { get; set; }

        [IgnoreLogging]
        public string EmailNguoiNhan { get; set; }

        [IgnoreLogging]
        public string SoDienThoaiNguoiNhan { get; set; }

        [IgnoreLogging]
        public string ThongTinHoaDonId { get; set; }

        [IgnoreLogging]
        public bool? IsKhachHangKy { get; set; }
    }
}
