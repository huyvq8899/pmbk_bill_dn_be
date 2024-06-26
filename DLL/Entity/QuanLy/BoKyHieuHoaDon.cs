﻿using DLL.Entity.Config;
using DLL.Entity.DanhMuc;
using DLL.Entity.QuanLyHoaDon;
using DLL.Entity.QuyDinhKyThuat;
using DLL.Enums;
using Microsoft.AspNetCore.Routing.Internal;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace DLL.Entity.QuanLy
{
    public class BoKyHieuHoaDon : ThongTinChung
    {
        public string BoKyHieuHoaDonId { get; set; }
        public UyNhiemLapHoaDon UyNhiemLapHoaDon { get; set; }
        public HinhThucHoaDon HinhThucHoaDon { get; set; }
        public LoaiHoaDon LoaiHoaDon { get; set; }
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
        public PhuongThucChuyenDL PhuongThucChuyenDL { get; set; }
        //public MauHoaDon MauHoaDon { get; set; }
        public ThongDiepChung ThongDiepChung { get; set; }

        public List<NhatKyXacThucBoKyHieu> NhatKyXacThucBoKyHieus { get; set; }
        public List<HoaDonDienTu> HoaDonDienTus { get; set; }
        public List<ThietLapTruongDuLieu> ThietLapTruongDuLieus { get; set; }
        /// <summary>
        /// Mã của CQT có mã của máy tính tiền từ 103 trả về
        /// </summary>
        [MaxLength(23)]
        public string MaCuaCQTToKhaiChapNhan { get; set; }
        /// <summary>
        /// Số bắt đầu mã của cơ quan thuế với hóa đơn có mã từ máy tính tiền
        /// </summary>
        public long? SoBatDauCMCQT { get; set; }
    }
}
