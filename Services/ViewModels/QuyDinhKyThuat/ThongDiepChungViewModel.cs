﻿using DLL.Enums;
using Services.ViewModels.DanhMuc;
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
        public int? SoLuong { get; set; }
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
        public int LanThu { get; set; } = 0;
        public int LanGui { get; set; } = 0;
        public DateTime? NgayGui { get; set; }
        public DateTime? NgayThongBao { get; set; }
        public string MaThongDiepPhanHoi { get; set; }
        public string NoiNhan { get; set; }
        public List<TaiLieuDinhKemViewModel> TaiLieuDinhKems { get; set; }
        public string IdThamChieu { get; set; } // tham chiếu đến thực thể được đóng gói trong thông điệp (thông báo, tờ khai, etc...)
        public string IdThongDiepGoc { get; set; } // trường hợp thông điệp trả về từ cơ quan thuế, chỉ đến thông điệp gốc đã gửi
        public string SoThongBao { get; set; }
        public string TenThongBao { get; set; }
        public string FileXML { get; set; }
        public DuLieuGuiHDDTViewModel DuLieuGuiHDDT { get; set; }
    }
}
