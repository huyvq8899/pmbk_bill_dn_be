using Services.Helper.Params.Filter;
using System;
using System.Collections.Generic;
using System.Text;

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
    }

    public class FileXMLThongDiepGuiParams
    {
        public string ThongDiepGuiCQTId { get; set; }
        public string DataXML { get; set; }
        public bool? AutoCapNhatNgayLap { get; set; }
    }

    public class DuLieuXMLGuiCQTParams
    {
        public string ThongDiepGuiCQTId { get; set; }
        public string XMLFileName { get; set; }
        public string MaSoThue { get; set; }
        public string MaThongDiep { get; set; }
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
        public string SoHoaDon { get; set; }
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
        public string SoHoaDonThayThe { get; set; }
        public string NgayLapHoaDonThayThe { get; set; }

        public string MauHoaDonDieuChinh { get; set; }
        public string KyHieuHoaDonDieuChinh { get; set; }
        public string SoHoaDonDieuChinh { get; set; }
        public string NgayLapHoaDonDieuChinh { get; set; }
    }

    public class DataByIdParams
    {
        public string ThongDiepGuiCQTId { get; set; }
        public bool IsTraVeThongDiepChung { get; set; }
    }
}
