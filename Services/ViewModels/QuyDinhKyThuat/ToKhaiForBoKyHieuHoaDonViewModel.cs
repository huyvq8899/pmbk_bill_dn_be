using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using System;

namespace Services.ViewModels.QuyDinhKyThuat
{
    public class ToKhaiForBoKyHieuHoaDonViewModel
    {
        public string ToKhaiId { get; set; }
        public string ThongDiepId { get; set; }
        public string MaThongDiepGui { get; set; }
        public DateTime? ThoiGianGui { get; set; }
        public string MaThongDiepNhan { get; set; }
        public int? TrangThaiGui { get; set; }
        public string TenTrangThaiGui { get; set; }
        public int? STT { get; set; }
        public string TenLoaiHoaDonUyNhiem { get; set; }
        public int? KyHieuMauHoaDon { get; set; }
        public string KyHieuHoaDonUyNhiem { get; set; }
        public string TenToChucDuocUyNhiem { get; set; }
        public string MaSoThueDuocUyNhiem { get; set; }
        public string MucDichUyNhiem { get; set; }
        public DateTime? ThoiGianUyNhiem { get; set; }
        public HTTToan PhuongThucThanhToan { get; set; }
        public string TenPhuongThucThanhToan { get; set; }
        public DateTime? ThoiDiemChapNhan { get; set; }
        //////////////////////////////////////////////
        public string TenToChucChungThuc { get; set; }
        public string SoSeriChungThu { get; set; }
        public DateTime? ThoiGianSuDungTu { get; set; }
        public DateTime? ThoiGianSuDungDen { get; set; }
        //////////////////////////////////////////////
        public XML.QuyDinhKyThuatHDDT.PhanII.I._1.TKhai ToKhaiKhongUyNhiem { get; set; }
        public XML.QuyDinhKyThuatHDDT.PhanII.I._2.TKhai ToKhaiUyNhiem { get; set; }
        public ThongDiepChungViewModel ThongDiepNhan { get; set; }
    }
}
