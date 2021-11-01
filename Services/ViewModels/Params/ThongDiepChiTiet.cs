using System;
using System.Collections.Generic;

namespace Services.ViewModels.Params
{
    public class ThongDiepChiTiet
    {
        public List<ThongDiepChiTiet1> ThongDiepChiTiet1s { get; set; }
        public List<ThongDiepChiTiet2> ThongDiepChiTiet2s { get; set; }
    }

    public class ThongDiepChiTiet1
    {
        public string PhienBan { get; set; }
        public string MauSo { get; set; }
        public string TenThongBao { get; set; }
        public string SoThongBao { get; set; }
        public DateTime? NgayThongBao { get; set; }
        
        public DateTime? NgayThongBaoCuaCQT { get; set; }

        public string LoaiKyDuLieu { get; set; }
        public string KyDuLieu { get; set; }
        public bool LanDau { get; set; }
        public int? BoSungLanThu { get; set; }
        public bool HoaDonDatIn { get; set; }
        public string LoaiHangHoa { get; set; }
        public string DiaDanh { get; set; }
        public string TenCQTCapTren { get; set; }
        public string TenCQTRaThongBao { get; set; }
        public string MaSoThue { get; set; }
        public string TenNguoiNopThue { get; set; }
        public DateTime? NgayDangKy_ThayDoi { get; set; }
        public string HinhThucDanhKy_ThayDoi { get; set; }
        public string TenToKhai { get; set; }
        public string MaGiaoDichDienTu { get; set; }
        public DateTime? ThoiGianGui { get; set; }
        public DateTime? ThoiGianNhan { get; set; }
        public string TruongHop { get; set; }
        public string TrangThaiXacNhanCuaCQT { get; set; }
        public string MoTaLoi { get; set; }
        public string LoaiUyNhiem { get; set; }
        ////////////////////////////////////////
        public string MaCuaCoQuanThue { get; set; }
        public string TenHoaDon { get; set; }
        public string KyHieuMauSoHoaDon { get; set; }
        public string KyHieuHoaDon { get; set; }
        public string SoHoaDon { get; set; }
        public string MaHoSo { get; set; }
        public DateTime? NgayLap { get; set; }
        public string SoBangKe { get; set; }
        public DateTime? NgayBangKe { get; set; }
        public string DonViTienTe { get; set; }
        public decimal? TyGia { get; set; }
        public string HinhThucThanhToan { get; set; }
        public string MaSoThueDVNUNLHD { get; set; }
        public string TenDVNhanUNLHD { get; set; }
        public string DiaChiDVNhanUNLHD { get; set; }
        public string TinhChatHoaDon { get; set; }
        public string LoaiHoaDonCoLienQuan { get; set; }
        public string KyHieuMauSoHoaDonCoLienQuan { get; set; }
        public string KyHieuHoaDonCoLienQuan { get; set; }
        public string SoHoaDonCoLienQuan { get; set; }
        public DateTime? NgayLapHoaDonCoLienQuan { get; set; }
        public string TenNguoiBan { get; set; }
        public string MaSoThueNguoiBan { get; set; }
        public string DiaChiNguoiBan { get; set; }
        public string TenNguoiMua { get; set; }
        public string MaSoThueNguoiMua { get; set; }
        public string DiaChiNguoiMua { get; set; }
        public decimal? TongTienChuaThue { get; set; }
        public decimal? TongTienThue { get; set; }
        public List<LoaiPhi> LoaiPhis { get; set; }
        public decimal? TongTienChietKhauThuongMai { get; set; }
        public decimal? TongTienThanhToan { get; set; }
        ///////////////////////////////
        public string HoaDonDanhChoKhuPhiThueQuan { get; set; }
        public string ThanhToan { get; set; }
        /////////////////////////////
        public string MaDonViQuanHeNganSach { get; set; }
        public string LoaiThongBao { get; set; }
        public string CanCu { get; set; }
        public decimal? SoLuong { get; set; }
        /////////////////////////////
        public int? SoThuTuThe { get; set; }
        public string DiaChiThuDienTu { get; set; }
        public string DiaChiNguoiNopThue { get; set; }
        public int? ThoiHan { get; set; }
        public int? Lan { get; set; }
        public DateTime? NgayCapNhat { get; set; }
    }

    public class LoaiPhi
    {
        public string TenLoaiPhi { get; set; }
        public decimal? TienPhi { get; set; }
    }

    public class ThongDiepChiTiet2
    {
        public int? STT { get; set; }
        public string MaSoThue { get; set; }
        public string TenToChuc { get; set; }
        public DateTime? NgayTiepNhan { get; set; }
        public string TenLoaiHoaDon { get; set; }
        public object KyHieuMauSoHoaDon { get; set; }
        public string KyHieuHoaDon { get; set; }
        public string MucDich { get; set; }
        public DateTime? TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }
        public string MoTaLoi { get; set; }
        /////////////////////////////////
        public object SoHoaDon { get; set; }
        public DateTime? NgayLap { get; set; }
        /////////////////////////////////
        public string MaCQTCap { get; set; }
        public string LoaiHoaDonDienTuApDung { get; set; }
        public string TinhChatThongBao { get; set; }
        public string TrangThaiTiepNhanCuaCQT { get; set; }
        /////////////////////////////////
        public string LyDoCanRaSoat { get; set; }
    }
}
