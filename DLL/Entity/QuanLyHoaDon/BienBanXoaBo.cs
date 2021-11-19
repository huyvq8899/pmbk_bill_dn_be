using DLL.Entity.DanhMuc;
using System;
using System.Collections.Generic;
using System.Text;

namespace DLL.Entity.QuanLyHoaDon
{
    public class BienBanXoaBo
    {
        public string Id { get; set; }
        public DateTime? NgayBienBan { get; set; }
        public string ThongTu { get; set; }
        public string SoBienBan { get; set; }
        public string KhachHangId { get; set; }
        public DoiTuong KhachHang { get; set; }
        public string TenKhachHang { get; set; }
        public string DiaChi { get; set; }
        public string MaSoThue { get; set; }
        public string SoDienThoai { get; set; }
        public string DaiDien { get; set; }
        public string ChucVu { get; set; }
        public DateTime? NgayKyBenB { get; set; }
        public string TenCongTyBenA { get; set; }
        public string DiaChiBenA { get; set; }
        public string MaSoThueBenA { get; set; }
        public string SoDienThoaiBenA { get; set; }
        public string DaiDienBenA { get; set; }
        public string ChucVuBenA { get; set; }
        public DateTime? NgayKyBenA { get; set; }
        public string HoaDonDienTuId { get; set; }
        public virtual HoaDonDienTu HoaDonDienTu { get; set; }
        public string LyDoXoaBo { get; set; }
        public string FileDaKy { get; set; }
        public string FileChuaKy { get; set; }
        public string XMLChuaKy { get; set; }
        public string XMLDaKy { get; set; }

        //gửi biên bản
        public string TenNguoiNhan { get; set; }
        public string EmailNguoiNhan { get; set; }
        public string SoDienThoaiNguoiNhan { get; set; }

        //tham chiếu đến bảng ThongTinHoaDons
        //(các hóa đơn nhập từ chức năng nhập thông tin hóa đơn)
        public string ThongTinHoaDonId { get; set; }
    }
}
