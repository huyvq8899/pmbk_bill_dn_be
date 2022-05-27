using System;
using System.Collections.Generic;

namespace Services.ViewModels.QuanLyHoaDonDienTu
{
    public class HoaDonSaiSotViewModel
    {
        public string HoaDonDienTuId { get; set; }
        public string ChungTuLienQuan { get; set; }
        public int TrangThaiHoaDon { get; set; }
        public string DienGiaiTrangThai { get; set; }
        public string MaCQTCap { get; set; }
        public string MauHoaDon { get; set; }
        public string KyHieuHoaDon { get; set; }
        public long? SoHoaDon { get; set; }
        public string StrSoHoaDon { get; set; }
        public DateTime? NgayLapHoaDon { get; set; }
        public byte? LoaiApDungHDDT { get; set; }
        public string TenLoaiApDungHDDT { get; set; }
        public byte? PhanLoaiHDSaiSot { get; set; } //trương này lưu vào database nên có thể thay đổi
        public byte? PhanLoaiHDSaiSotMacDinh { get; set; }
        public string LyDo { get; set; }
        public bool? LaThongTinSaiSot { get; set; } //dựa vào trường này để hiển thị chữ/màu sắc các nút
        public int LoaiSaiSotDeTimKiem { get; set; }
        public int SoLanGuiCQT { get; set; }
        public int? LoaiDieuChinh { get; set; }
        public bool? LaHoaDonNgoaiHeThong { get; set; }
        public string IdsChungTuLienQuan { get; set; } //chuỗi gồm 1 hoặc nhiều id chứng từ liên quan
        public string LyDoThayThe { get; set; }
    }

    public class ThongDiepGuiCQTViewModel
    {
        public string Id { get; set; }
        public string SoThongBaoSaiSot { get; set; }
        public string MaCoQuanThue { get; set; }
        public string TenCoQuanThue { get; set; }
        public string MaThongDiep { get; set; }
        public string MaTDiepThamChieu { get; set; }
        public byte LoaiThongBao { get; set; }
        public string SoTBCCQT { get; set; }
        public DateTime? NTBCCQT { get; set; }
        public DateTime NgayGui { get; set; }
        public DateTime NgayLap { get; set; }
        public string FileDinhKem { get; set; }
        public string FileXMLDaKy { get; set; }
        public string FileContainerPath { get; set; }

        public string ThongBaoHoaDonRaSoatId { get; set; }
        public bool? DaKyGuiCQT { get; set; }
        public string NguoiNopThue { get; set; }
        public string DaiDienNguoiNopThue { get; set; }
        public string MaDiaDanh { get; set; }
        public string DiaDanh { get; set; }
        public string MaSoThue { get; set; }
        public List<ThongDiepChiTietGuiCQTViewModel> ThongDiepChiTietGuiCQTs { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string ModifyBy { get; set; }
        public bool? IsTBaoHuyGiaiTrinhKhacCuaNNT { get; set; }
        public byte? HinhThucTBaoHuyGiaiTrinhKhac { get; set; }
        public string ThongDiepChungId { get; set; }
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
        //public LADHDDT LoaiApDungHoaDon { get; set; }
        //public TCTBao PhanLoaiHDSaiSot { get; set; }

        public byte LoaiApDungHoaDon { get; set; }
        public byte PhanLoaiHDSaiSot { get; set; }

        public string LyDo { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string ModifyBy { get; set; }
        public int? STT { get; set; }
        public string ThongBaoChiTietHDRaSoatId { get; set; }

        //2 trường này là đọc thông tin của bảng thông báo hóa đơn rà soát
        public byte LoaiApDungHD { get; set; }
        public string LyDoRaSoat { get; set; }

        public string ChungTuLienQuan { get; set; }
        public int? TrangThaiHoaDon { get; set; }
        public string DienGiaiTrangThai { get; set; }
        public int SoLanGuiCQT { get; set; }
        public byte? PhanLoaiHDSaiSotMacDinh { get; set; }
        public bool? LaHoaDonNgoaiHeThong { get; set; }
    }

    public class KetQuaLuuThongDiep
    {
        public string Id { get; set; }
        public string FileNames { get; set; }
        public string FileContainerPath { get; set; }
        public string MaThongDiep { get; set; }
        public string SoThongBaoSaiSot { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ThongDiepChungId { get; set; }
    }

    public class DSMauKyHieuHoaDonViewModel
    {
        public string MauKyHieuHoaDon { get; set; }
    }

    public class ThongTinChuKySoViewModel
    {
        public string TenNguoiKy { get; set; }
        public string NgayKy { get; set; }
    }


    public class BangKeHoaDonSaiSot_ViewModel
    {
        public string MauHoaDon { get; set; }
        public string KyHieuHoaDon { get; set; }
        public string SoHoaDon { get; set; }
        public DateTime? NgayLapHoaDon { get; set; }
        public string MaCQTCap { get; set; }
        public decimal? TongTienThanhToan { get; set; }
        public string MaLoaiTien { get; set; }

        public int LoaiApDungHoaDon { get; set; }
        public string LoaiSaiSot { get; set; }
        public string LyDo { get; set; }

        public string LoaiHoaDon { get; set; }
        public TrangThaiHoaDon_BangKeSaiSot_ViewModel TrangThaiHoaDon { get; set; }

        public string ChungTuLienQuan { get; set; }
        public DateTime? NgayThongBao { get; set; }

        public byte? LoaiThongBaoSaiSot { get; set; }
        public string TenLoaiThongBaoSaiSot { get; set; }

        public string SoTBaoCuaCQT { get; set; }
        public DateTime? NgayTBaoCuaCQT { get; set; }

        public string MaThongDiepGui { get; set; }
        public string SoTBaoPhanHoiTuCQT { get; set; }
        public DateTime? NgayTBaoPhanHoiTuCQT { get; set; }
        public int? TrangThaiGui { get; set; }
        public string TenTrangThaiGui { get; set; }

        public string ThongDiepChungId { get; set; }
        public string IdTDiepTBaoPhanHoiCuaCQT { get; set; }
        public bool? LaHoaDonNgoaiHeThong { get; set; }

        public string HoaDonDienTuId { get; set; }
        public string ColorHex { get; set; }
        public int? TrangThaiQuyTrinh { get; set; }
    }

    public class ChiTietHoaDonRaSoat301_ViewModel
    {
        public string MauHoaDon { get; set; }
        public string KyHieuHoaDon { get; set; }
        public string SoHoaDon { get; set; }
        public string SoThongBaoCuaCQT { get; set; }
        public DateTime? NgayThongBao { get; set; }
    }

    public class HoaDon_ViewModel
    {
        public string HoaDonDienTuId { get; set; }
        public string ThayTheChoHoaDonId { get; set; }
        public string DieuChinhChoHoaDonId { get; set; }
        public string MaCuaCQT { get; set; }
        public string SoHoaDon { get; set; }
        public DLL.Enums.HinhThucHoaDon HinhThucHoaDon { get; set; }
        public DateTime? NgayHoaDon { get; set; }
    }

    public class TrangThaiHoaDon_BangKeSaiSot_ViewModel
    {
        public int? TrangThai { get; set; }
        public int? LoaiDieuChinh { get; set; }
        public string DienGiaiTrangThai { get; set; }
    }

    public class SoLanGuiHoaDonToiCQT_ViewModel
    {
        public string HoaDonDienTuId { get; set; }
        public string ThongDiepGuiCQTId { get; set; }
    }
}
