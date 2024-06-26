﻿using ManagementServices.Helper;
using Services.ViewModels.QuanLyHoaDonDienTu;
using System.Collections.Generic;

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
        public bool? LocHoaDonCoSaiSotChuaLapChuaGui { get; set; }
        public int? TrangThaiLapVaGuiThongBao { get; set; }
        public string DienGiaiChiTietTrangThai { get; set; }
        public List<string> HoaDonDienTuIds { get; set; }
        public List<HoaDonDienTuViewModel> HoaDonDienTus { get; set; }
        public string BoKyHieuHoaDonId { get; set; }
        public bool? IsBanNhap { get; set; }
        public decimal? TongTienThanhToan { get; set; }
        public int? IsPos { get; set; }
        public bool? ChuaHuy { get; set; }
        public bool? ChuaLapThayThe { get; set; }
        public bool? GuiCQT { get; set; }
        public bool? LocHoaDonYeuCauHuy { get; set; }
    }
}
