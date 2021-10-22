using Services.ViewModels.DanhMuc;
using System;
using System.Collections.Generic;

namespace Services.ViewModels.QuanLyHoaDonDienTu
{
    public class HoaDonSaiSotViewModel
    {
        public string HoaDonDienTuId { get; set; }
        public string MaCQTCap { get; set; }
        public string MauHoaDon { get; set; }
        public string KyHieuHoaDon { get; set; }
        public string SoHoaDon { get; set; }
        public DateTime? NgayLapHoaDon { get; set; }
        public byte? LoaiApDungHDDT { get; set; }
        public byte? PhanLoaiHDSaiSot { get; set; }
        public string LyDo { get; set; }
    }

    public class ThongDiepGuiCQTViewModel
    {
        public string Id { get; set; }
        public string TenCoQuanThue { get; set; }
        public string MaThongDiep { get; set; }
        public byte LoaiThongBao { get; set; }
        public DateTime NgayGui { get; set; }
        public DateTime NgayLap { get; set; }
        public string FileDinhKem { get; set; }
        public string NguoiNopThue { get; set; }
        public string DaiDienNguoiNopThue { get; set; }
        public string DiaDanh { get; set; }
        public string MaSoThue { get; set; }
        public List<ThongDiepChiTietGuiCQTViewModel> ThongDiepChiTietGuiCQTs { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string ModifyBy { get; set; }
    }

    public class ThongDiepChiTietGuiCQTViewModel
    {
        public string Id { get; set; }
        public string ThongDiepGuiCQTId { get; set; }
        public string HoaDonDienTuId { get; set; }
        public string MaCQTCap { get; set; }
        public string MauHoaDon { get; set; }
        public string KyHieuHoaDon { get; set; }
        public string SoHoaDon { get; set; }
        public DateTime? NgayLapHoaDon { get; set; }
        public byte? LoaiApDungHoaDon { get; set; }
        public byte? PhanLoaiHDSaiSot { get; set; }
        public string LyDo { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string ModifyBy { get; set; }
    }
}
