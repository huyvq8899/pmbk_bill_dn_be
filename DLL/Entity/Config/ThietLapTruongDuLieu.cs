using DLL.Entity.QuanLy;
using DLL.Enums;
using System;

namespace DLL.Entity.Config
{
    public class ThietLapTruongDuLieu : ICloneable
    {
        public string ThietLapTruongDuLieuId { get; set; }
        public string BoKyHieuHoaDonId { get; set; }
        public string MaTruong { get; set; }
        public string TenCot { get; set; }
        public string TenTruong { get; set; }
        public string TenTruongHienThi { get; set; }
        public LoaiHoaDon LoaiHoaDon { get; set; }
        public LoaiTruongDuLieu LoaiTruongDuLieu { get; set; }
        public KieuDuLieuThietLapTuyChinh KieuDuLieu { get; set; }
        public string GhiChu { get; set; }
        public int? DoRong { get; set; }
        public int STT { get; set; }
        public bool HienThi { get; set; }
        public string GiaTri { get; set; }

        public BoKyHieuHoaDon BoKyHieuHoaDon { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
