﻿using DLL.Enums;
using Services.ViewModels.DanhMuc;
using Services.ViewModels.QuanLyHoaDonDienTu;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities;
using System;
using System.Collections.Generic;

namespace Services.ViewModels.QuyDinhKyThuat
{
    public class ThongDiepChungViewModel : ThongTinChungViewModel
    {
        public string ThongDiepChungId { get; set; }
        public string PhienBan { get; set; }
        public string MaNoiGui { get; set; }
        public string MaNoiNhan { get; set; }
        public int MaLoaiThongDiep { get; set; }
        public string MaThongDiep { get; set; }
        public string MaThongDiepThamChieu { get; set; }
        public string MaSoThue { get; set; }
        public int SoLuong { get; set; }
        /// /////////////////////////////////////
        public string TenLoaiThongDiep { get; set; }
        public bool ThongDiepGuiDi { get; set; }
        public int? HinhThuc { get; set; }
        public string TenHinhThuc { get; set; }
        public TrangThaiGuiThongDiep TrangThaiGui { get; set; }
        /////
        public string TenTrangThaiGui { get; set; }
        ////
        public string TenTrangThaiThongBao { get; set; }
        public DateTime? NgayGui { get; set; }
        public DateTime? NgayThongBao { get; set; }
        public string MaThongDiepPhanHoi { get; set; }
        public List<TaiLieuDinhKemViewModel> TaiLieuDinhKems { get; set; }
        public string IdThamChieu { get; set; } // tham chiếu đến thực thể được đóng gói trong thông điệp (thông báo, tờ khai, etc...)// trường hợp thông điệp trả về từ cơ quan thuế, chỉ đến thông điệp gốc đã gửi
        public string SoThongBao { get; set; }
        public string SoTBaoPhanHoiCuaCQT { get; set; }
        public string TenThongBao { get; set; }
        public string FileXML { get; set; }
        public string Type { get; set; }
        public string NguoiThucHien { get; set; }
        public int? ThoiHan { get; set; }
        public DuLieuGuiHDDTViewModel DuLieuGuiHDDT { get; set; }
        public string Key { get; set; }
        public ThongDiepChungViewModel Parent { get; set; }
        public List<ThongDiepChungViewModel> Children { get; set; }
        public string DataXML { get; set; }
        public string Base64 { get; set; }


        public string MaNoiGui999 { get; set; }
        public string MaNoiNhan999 { get; set; }
        public string MaLoaiThongDiep999 { get; set; }
        public string MaThongDiep999 { get; set; }
        public string MaThongDiepThamChieu999 { get; set; }
        public string DataXML999 { get; set; }
        public TrangThaiQuyTrinh TrangThai { get; set; }

        public string IdThongBao { get; set; }
        public string MGDDTu { get; set; }
        public string MSoTBao { get; set; }
        public string SoTBao { get; set; }
        public DateTime? NgayTBao { get; set; }

        //param tạo 206
        public List<HoaDonDienTuViewModel> HoaDons { get; set; }
        public TTChungThongDiep TTChung1 { get; set; }
        public TTChungThongDiep TTChung2 { get; set; }
        public TTChungThongDiep TTChung3 { get; set; }
    }

    public class ThongKeSoLuongThongDiepViewModel
    {
        public string TuNgay { get; set; }
        public string DenNgay { get; set; }
        public int SoLuong { get; set; }
        public int TrangThaiGuiThongDiep { get; set; }
    }

    public class ThongTinThongBao
    {
        public string MGDDTu { get; set; }
        public string MSoTBao { get; set; }
        public string SoTBao { get; set; }
        public DateTime? NgayTBao { get; set; }
    }
}
