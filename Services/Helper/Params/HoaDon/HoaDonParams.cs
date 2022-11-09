using ManagementServices.Helper;
using Services.ViewModels.QuanLyHoaDonDienTu;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Services.Helper.Params.HoaDon
{
    public class HoaDonParams : PagingParams
    {
        public string KhachHangId { get; set; }
        public int? HinhThucHoaDon { get; set; }
        public int? UyNhiemLapHoaDon { get; set; }
        public int? LoaiHoaDon { get; set; }
        public int? LoaiNghiepVu { get; set; }
        public int? TrangThaiHoaDonDienTu { get; set; }
        public List<int> TrangThaiHoaDonDienTus { get; set; }
        public int? TrangThaiPhatHanh { get; set; }
        public List<int> TrangThaiPhatHanhs { get; set; }
        public int? TrangThaiGuiHoaDon { get; set; }
        public int? TrangThaiChuyenDoi { get; set; }
        public int? TrangThaiXoaBo { get; set; }
        public int? TrangThaiBienBanXoaBo { get; set; }
        public HoaDonDienTuViewModel Filter { get; set; }
        public HoaDonSearch TimKiemTheo { get; set; }
        public string GiaTri { get; set; }
        public string HoaDonDienTuId { get; set; }
        public bool? IsChuyenDoi { get; set; }
        public bool? LocHoaDonCoSaiSotChuaLapTBao04 { get; set; }
        public List<string> HoaDonDienTuIds { get; set; }
        public List<HoaDonDienTuViewModel> HoaDonDienTus { get; set; }
        public string BoKyHieuHoaDonId { get; set; }
        public bool? IsBanNhap { get; set; }
        public decimal? TongTienThanhToan { get; set; }
    }

    public class MobileResult
    {
        public string HoaDonDienTuId { get; set; }
        public string BenDi { get; set; }
        public string BenDen { get; set; }
        public string SoTuyen { get; set; }
        public int? SoChuyen { get; set; }
        public Guid? TuyenDuongId { get; set; }
        public DateTime? ThoiGianKhoiHanh { get; set; }
        public string TenTuyenDuong { get; set; }
        public int? STT { get; set; }
    }

    public class MobileParamsExport
    {
        public string HoaDonDienTuId { get; set; }
        public string BenDi { get; set; }
        public int SoChuyen { get; set; }
    }

    public class QuanLyVeParams
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }

        /// <summary>
        /// Lấy theo trạng thái từ Trạng thái quy trình
        /// Trạng thái hóa đơn = -1 - Mới tạo lập ( Tất cả)
        /// Trạng thái hóa đơn = 9 - Đã đồng bộ ( Cơ quan thuế cấp mã)
        /// </summary>
        public int? TrangThaiHoaDon { get; set; }

        public int? SoChuyen { get; set; }

    }

    public class BaoCaoMobileResult
    {
        public List<BaoCaoGroupBySoChuyen> BaoCaoVes { get; set; }
        public DateTime? CreatedDate { get; set; }
        public decimal? TongTienNgay { get; set; }
        public int? TongVeNgay { get; set; }
    }

    public class BaoCaoGroupBySoChuyen
    {
        public List<BaoCaoVeResult> Ves { get; set; }
        public int? SoChuyen { get; set; }
        public int? TongSoVe { get; set; }
        public decimal? TongTienChuyen { get; set; }

    }
    public class BaoCaoVeResult
    {
        public string HoaDonDienTuId { get; set; }
        public Guid? TuyenDuongId { get; set; }
        public string TenTuyenDuong { get; set; }
        public string BenDi { get; set; }
        public string BenDen { get; set; }
        public decimal? SoLuong { get; set; }
        public decimal? DonGia { get; set; }
        public decimal? TongTien { get; set; }
        public int? SoChuyen { get; set; }
        public DateTime? ThoiGianKhoiHanh { get; set; }
        public DateTime? NgayKy { get; set; }
        public int? TrangThaiHoaDon { get; set; }
        public int? STT { get; set; }
        public DateTime? CreatedDate { get; set; }
    }

    public class QuanLyVeResult
    {
        public string BenDen { get; set; }
        public decimal? SoLuong { get; set; }
        public int? SoChuyen { get; set; }
        public int? STT { get; set; }
        public DateTime? ThoiGianKhoiHanh { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
    public class VeDienTuSearch
    {
        [Display(Name = "Loại hóa đơn")]
        public string LoaiHoaDon { get; set; }
        [Display(Name = "Ký hiệu mẫu số hóa đơn")]
        public string MauSo { get; set; }
        [Display(Name = "Ký hiệu hóa đơn")]
        public string KyHieu { get; set; }
        [Display(Name = "Số hóa đơn")]
        public string SoHoaDon { get; set; }
        [Display(Name = "Mã số thuế")]
        public string MaSoThue { get; set; }
        [Display(Name = "Mã khách hàng")]
        public string MaKhachHang { get; set; }
        [Display(Name = "Tên khách hàng")]
        public string TenKhachHang { get; set; }
        [Display(Name = "Số xe")]
        public string SoXe { get; set; }
        [Display(Name = "Số ghế")]
        public string SoGhe { get; set; }
        [Display(Name = "Số tuyến")]
        public string SoTuyen { get; set; }
        [Display(Name = "Tên tuyến đường")]
        public string TenTuyenDuong { get; set; }
        [Display(Name = "Bến đi")]
        public string BenDi { get; set; }
        [Display(Name = "Bến đến")]
        public string BenDen { get; set; }
    }
}
