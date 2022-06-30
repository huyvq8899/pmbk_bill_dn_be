using Services.ViewModels.QuanLyHoaDonDienTu;
using Services.ViewModels.TienIch;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Services.Helper.Params.HoaDon
{
    public class ParamsSendMail
    {
        public HoaDonDienTuViewModel HoaDon { get; set; }
        public string ToMail { get; set; }
        public string TenNguoiNhan { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string SoDienThoaiNguoiNhan { get; set; }
        public int? LoaiEmail { get; set; }
        public string Link { get; set; }
        public string LinkTraCuu { get; set; }
        public string BienBanDieuChinhId { get; set; }
        public NhatKyThaoTacLoiViewModel ErrorActionModel { get; set; }
    }

    public class ParamsSendMailThongTinHoaDon
    {
        public string ThongTinHoaDonId { get; set; }
        public string ToMail { get; set; }
        public string TenNguoiNhan { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string SoDienThoaiNguoiNhan { get; set; }
        public int? LoaiEmail { get; set; }
        public string Link { get; set; }
    }

    public class ParamsSendMailThongBaoSaiThongTin
    {
        public string HoaDonDienTuId { get; set; }
        public string MauHoaDon { get; set; }
        public string KyHieuHoaDon { get; set; }
        public long? SoHoaDon { get; set; }
        public DateTime? NgayHoaDon { get; set; }
        public decimal? TongTienThanhToan { get; set; }
        public string TenKhachHang { get; set; }
        public bool? TichChon_HoTenNguoiMuaHang { get; set; }
        public bool? TichChon_TenDonVi { get; set; }
        public bool? TichChon_DiaChi { get; set; }
        public string HoTenNguoiMuaHang_Sai { get; set; }
        public string HoTenNguoiMuaHang_Dung { get; set; }
        public string TenDonVi_Sai { get; set; }
        public string TenDonVi_Dung { get; set; }
        public string DiaChi_Sai { get; set; }
        public string DiaChi_Dung { get; set; }
        public string TenNguoiNhan { get; set; }
        public string EmailCuaNguoiNhan { get; set; }
        public string EmailCCNguoiNhan { get; set; }
        public string EmailBCCNguoiNhan { get; set; }
        public string SoDienThoaiNguoiNhan { get; set; }
        public string MaLoaiTien { get; set; }
        public string UserId { get; set; }
        public string KhachHangId { get; set; }
    }
}
