using DLL.Entity.DanhMuc;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DLL.Entity.QuanLyHoaDon
{
    public class ThongDiepChiTietGuiCQT
    {
        public string Id { get; set; }
        public string ThongDiepGuiCQTId { get; set; }
        public string HoaDonDienTuId { get; set; }
        public string MaCQTCap { get; set; }
        public string MaCQT12 { get; set; }
        public string MaCQT45 { get; set; }
        public string MaCQTToKhaiChapNhan { get; set; }
        public string MaCQTChuoiKyTuSo { get; set; }
        public string MauHoaDon { get; set; }
        public string MauSoHoaDon_LoaiHoaDon { get; set; }
        public string MauSoHoaDon_SoLienHoaDon { get; set; }    
        public string MauSoHoaDon_SoThuTuMau { get; set; }
        public string KyHieuHoaDon { get; set; }

        //ký hiệu hóa đơn 1450
        public string KyHieu1 { get; set; }
        public string KyHieu23 { get; set; }
        public string KyHieu4 { get; set; }
        public string KyHieu56 { get; set; }

        //ký hiệu hóa đơn khác
        public string KyHieuHoaDon_2KyTuDau_DatIn { get; set; }
        public string KyHieuHoaDon_2KyTuDau { get; set; }
        public string KyHieuHoaDon_2SoCuoiNamThongBao { get; set; }
        public string KyHieuHoaDon_HinhThucHoaDon { get; set; }

        public string SoHoaDon { get; set; }
        public DateTime? NgayLapHoaDon { get; set; }
        public byte LoaiApDungHoaDon { get; set; }
        public byte PhanLoaiHDSaiSot { get; set; }
        public string LyDo { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string ModifyBy { get; set; }
        public int? STT { get; set; }
        public string ThongBaoChiTietHDRaSoatId { get; set; }
        public string ChungTuLienQuan { get; set; }
        public int? TrangThaiHoaDon { get; set; }
        public string DienGiaiTrangThai { get; set; }
        public byte? PhanLoaiHDSaiSotMacDinh { get; set; }
        public string HinhThucHoaDon { get; set; }
    }
}
