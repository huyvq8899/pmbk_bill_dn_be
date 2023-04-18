using Services.Helper.Params.Filter;
using Services.ViewModels.QuanLyHoaDonDienTu;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Services.Helper
{
    public class HoaDonSaiSotParams
    {
        public string LapTuHoaDonDienTuId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int HinhThucHoaDon { get; set; }
        public int TrangThaiGuiHoaDon { get; set; }
        public int? LoaiSaiSot { get; set; }

        public List<FilterColumn> FilterColumns { get; set; }
        public string SortKey { get; set; }
        public string SortValue { get; set; }

        public ThongBaoSaiSotSearch TimKiemTheo { get; set; }
        public string TimKiemBatKy { get; set; }
        public bool? IsTBaoHuyGiaiTrinhKhacCuaNNT { get; set; }
        public string HoaDonDienTuIdLienQuan { get; set; }
    }

    public class FileXMLThongDiepGuiParams
    {
        public string ThongDiepGuiCQTId { get; set; }
        public string DataXML { get; set; }
        public bool? AutoCapNhatNgayLap { get; set; }
        public int MaLoaiThongDiep { get; set; }
    }

    public class DuLieuXMLGuiCQTParams
    {
        public string ThongDiepGuiCQTId { get; set; }
        public string XMLFileName { get; set; }
        public string MaSoThue { get; set; }
        public string MaThongDiep { get; set; }
        public int MaLoaiThongDiep { get; set; }
    }

    public class MauKyHieuHoaDonParams
    {
        public string LoaiHoaDon { get; set; }
        public string HinhThucHoaDon { get; set; }
        public string MauKyHieuHoaDon { get; set; }
    }

    public class ThongBaoSaiSotSearch
    {
        public string MauHoaDon { get; set; }
        public string KyHieuHoaDon { get; set; }
        public string SoHoaDon { get; set; }
        public string NgayLapHoaDon { get; set; }
    }

    public class HoaDonHeThongViewModel
    {
        public string HoaDonDienTuId { get; set; }
        public string MauHoaDon { get; set; }
        public string KyHieuHoaDon { get; set; }
        public long? SoHoaDon { get; set; }
        public string NgayLapHoaDon { get; set; }
        public string ThayTheChoHoaDonId { get; set; }
        public string DieuChinhChoHoaDonId { get; set; }
        //PhanLoaiTrungHoaDon: 
        // 1 = trùng chung với hóa đơn hệ thống, 2 = trùng với với hóa đơn bị thay thế, 3 = trùng với hóa đơn bị điều chỉnh
        public int? PhanLoaiTrungHoaDon { get; set; }
        //RowIndex là index của dòng hóa đơn bị trùng với hóa đơn hệ thống
        public int? RowIndex { get; set; }

        public string MauHoaDonThayThe { get; set; }
        public string KyHieuHoaDonThayThe { get; set; }
        public long? SoHoaDonThayThe { get; set; }
        public string NgayLapHoaDonThayThe { get; set; }

        public string MauHoaDonDieuChinh { get; set; }
        public string KyHieuHoaDonDieuChinh { get; set; }
        public long? SoHoaDonDieuChinh { get; set; }
        public string NgayLapHoaDonDieuChinh { get; set; }
    }

    public class DataByIdParams
    {
        public string ThongDiepGuiCQTId { get; set; }
        public bool IsTraVeThongDiepChung { get; set; }
    }

    public class ThongKeHoaDonSaiSotParams
    {
        public byte LoaiThongke { get; set; } //LoaiThongke = 1: thống kê theo hóa đơn, 2: thống kê theo thông báo
        public string TuNgay { get; set; }
        public string DenNgay { get; set; }
        public byte LoaiThongBaoSaiSot { get; set; }
        public bool KhongHienThiThongTinGiongNhau { get; set; }
        public ThongKeHoaDonSaiSotSearch TimKiemTheo { get; set; }
        public string TimKiemBatKy { get; set; }
        public List<FilterColumn> FilterColumns { get; set; }
        public string SortValue { get; set; }
        public string SortKey { get; set; }
        public BangKeHoaDonSaiSot_ViewModel Filter { get; set; }
    }

    public class ThongKeHoaDonSaiSotSearch
    {
        public string KyHieuHoaDon { get; set; }
        public string SoHoaDon { get; set; }
        public string NgayHoaDon { get; set; }
    }

    public class ExportExcelBangKeSaiSotParams
    {
        public ThongKeHoaDonSaiSotParams Params { get; set; }
        public List<BangKeHoaDonSaiSot_ViewModel> ListBangKeSaiSot { get; set; }
    }

    public class LoaiThongBaoSaiSotViewModel
    {
        public byte LoaiThongBaoSaiSot { get; set; }
        public string TenLoaiThongBaoSaiSot { get; set; }
    }
}
