using Services.Helper.LogHelper;
using Services.ViewModels.DanhMuc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.QuanLyHoaDonDienTu
{
    public class BienBanDieuChinhViewModel : ThongTinChungViewModel
    {
        [Display(Name = "Số biên bản")]
        public string SoBienBan { get; set; }
        [IgnoreLogging]
        public string BienBanDieuChinhId { get; set; }

        [Display(Name = "Căn cứ")]
        public string NoiDungBienBan { get; set; }

        [Display(Name = "Ngày biên bản")]
        public DateTime? NgayBienBan { get; set; }

        [Display(Name = "Tên bên bán")]
        public string TenDonViBenA { get; set; }

        [Display(Name = "Địa chỉ bên bán")]
        public string DiaChiBenA { get; set; }

        [Display(Name = "Mã số thuế bên bán")]
        public string MaSoThueBenA { get; set; }

        [Display(Name = "Số điện thoại bên bán")]
        public string SoDienThoaiBenA { get; set; }

        [Display(Name = "Đại diện bên bán")]
        public string DaiDienBenA { get; set; }

        [Display(Name = "Chức vụ bên bán")]
        public string ChucVuBenA { get; set; }

        [IgnoreLogging]
        public DateTime? NgayKyBenA { get; set; }

        [Display(Name = "Tên bên mua")]
        public string TenDonViBenB { get; set; }

        [Display(Name = "Địa chỉ bên mua")]
        public string DiaChiBenB { get; set; }

        [Display(Name = "Mã số thuế bên mua")]
        public string MaSoThueBenB { get; set; }

        [Display(Name = "Số điện thoại bên mua")]
        public string SoDienThoaiBenB { get; set; }

        [Display(Name = "Đại diện bên mua")]
        public string DaiDienBenB { get; set; }

        [Display(Name = "Chức vụ bên mua")]
        public string ChucVuBenB { get; set; }

        [IgnoreLogging]
        public DateTime? NgayKyBenB { get; set; }

        [Display(Name = "Lý do điều chỉnh")]
        public string LyDoDieuChinh { get; set; }

        [IgnoreLogging]
        public int? TrangThaiBienBan { get; set; } // Services\Helper\Params\HoaDon\HoaDonThayTheParams.cs

        [IgnoreLogging]
        public string FileDaKy { get; set; }

        [IgnoreLogging]
        public string FileChuaKy { get; set; }

        [IgnoreLogging]
        public string XMLChuaKy { get; set; }

        [IgnoreLogging]
        public string XMLDaKy { get; set; }

        [IgnoreLogging]
        public string HoaDonBiDieuChinhId { get; set; }

        [IgnoreLogging]
        public HoaDonDienTuViewModel HoaDonBiDieuChinh { get; set; }

        [IgnoreLogging]
        public string HoaDonDieuChinhId { get; set; }

        [IgnoreLogging]
        public HoaDonDienTuViewModel HoaDonDieuChinh { get; set; }

        [IgnoreLogging]
        public List<TaiLieuDinhKemViewModel> TaiLieuDinhKems { get; set; }
        [IgnoreLogging]

        public bool IsCheckNgay { get; set; }

        public string DanhSachHoaDonLienQuan { get; set; }
    }
}
