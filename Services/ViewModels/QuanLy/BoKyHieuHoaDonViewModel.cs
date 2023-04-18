using DLL.Enums;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.QuyDinhKyThuat;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Services.ViewModels.QuanLy
{
    public class BoKyHieuHoaDonViewModel : ThongTinChungViewModel
    {
        public BoKyHieuHoaDonViewModel()
        {
            Checked = false;
        }

        public string BoKyHieuHoaDonId { get; set; }
        public UyNhiemLapHoaDon UyNhiemLapHoaDon { get; set; }
        public HinhThucHoaDon HinhThucHoaDon { get; set; }
        public LoaiHoaDon LoaiHoaDon { get; set; }
        public PhuongThucChuyenDL PhuongThucChuyenDL { get; set; }
        public string KyHieu { get; set; }
        public int KyHieuMauSoHoaDon { get; set; }
        public string KyHieuHoaDon { get; set; }
        public string KyHieu1 { get; set; }
        public string KyHieu23 { get; set; }
        public string KyHieu4 { get; set; }
        public string KyHieu56 { get; set; }
        public long? SoBatDau { get; set; }
        public long? SoLonNhatDaLapDenHienTai { get; set; }
        public long? SoToiDa { get; set; }
        public bool? IsTuyChinh { get; set; } // tùy chỉnh nguyên tắc số hóa đơn
        public string MauHoaDonId { get; set; }
        public string ThongDiepId { get; set; }
        public string ThongDiepMoiNhatId { get; set; }
        public TrangThaiSuDung TrangThaiSuDung { get; set; }
        public string MaSoThueBenUyNhiem { get; set; }
        //////////////////////////////////////////
        public DateTime? ThoiGianXacThuc { get; set; }
        public DateTime? NgayHoaDon { get; set; }
        public int? KyHieu23Int { get; set; }
        public DateTime? ThoiDiemChapNhan { get; set; }
        public string TenPhuongThucTruyenDuLieu { get; set; }
        public string TenTrangThaiSuDung { get; set; }
        public string TenUyNhiemLapHoaDon { get; set; }
        public string TenHinhThucHoaDon { get; set; }
        public string TenMauHoaDon { get; set; }
        public string MaThongDiep { get; set; }
        public string ThoiDiemChapNhanFilter { get; set; }
        public string NgayCapNhatFilter { get; set; }
        public string SerialNumber { get; set; }
        public bool? Checked { get; set; }
        public bool? Actived { get; set; }
        public int? IntKyHieu23 { get; set; }
        public bool? IsHetHieuLuc { get; set; }
        /// //////////////////////
        public MauHoaDonViewModel MauHoaDon { get; set; }
        public ThongDiepChungViewModel ThongDiepChung { get; set; }
        public List<LoaiHoaDon> LoaiHoaDons { get; set; }
        public ToKhaiForBoKyHieuHoaDonViewModel ToKhaiForBoKyHieuHoaDon { get; set; }
        public ToKhaiForBoKyHieuHoaDonViewModel ToKhaiMoiNhatForBoKyHieuHoaDon { get; set; }
        public bool? IsChuyenBangTongHop { get; set; }
        public bool? IsChuyenDayDuNoiDungTungHoaDon { get; set; }
        public List<NhatKyXacThucBoKyHieuViewModel> NhatKyXacThucBoKyHieus { get; set; }
        public List<MauHoaDonViewModel> MauHoaDons { get; set; }
        public bool IsCMTMTTien { get; set; }
        public bool IsKhongCoMa { get; set; }

        public string MaCuaCQTToKhaiChapNhan { get; set; }
        public long? SoBatDauCMCQT { get; set; }

        public string RoleId { get; set; }

        public int? LoaiNghiepVu { get; set; }
    }

    public class DanhSachRutGonBoKyHieuHoaDonViewModel
    {
        public string BoKyHieuHoaDonId { get; set; }
        public UyNhiemLapHoaDon UyNhiemLapHoaDon { get; set; }
        public string KyHieu { get; set; }
    }


    public class BoKyHieuForMayTinhTienViewModels
    {
        public string BoKyHieuHoaDonId { get; set; }
        public string MauHoaDonId { get; set; }
        public string KyHieu { get; set; }
        public string MaCuaCQTToKhaiChapNhan { get; set; }

        public long? SoBatDauCMCQT { get; set; }

        public List<MauHoaDonViewModel> MauHoaDons { get; set; }
    }
}
