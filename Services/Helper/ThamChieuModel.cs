using Services.Enums;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Services.Helper
{
    public class ThamChieuModel
    {
        public string ChungTuId { get; set; }
        public long? SoChungTu { get; set; }
        public DateTime? NgayHoaDon { get; set; }
        public BusinessOfType KieuChungTu { get; set; }
        public BusinessOfType LoaiChungTu { get; set; }
        public string TenLoaiChungTu { get; set; }
        public string Message { get; set; }
        public string Message2 { get; set; }
        public List<ThamChieuModel> List { get; set; }
        public List<ThamChieuModel> RemovedList { get; set; }
        public LoaiChungTuLienQuan LoaiChungTuLienQuan { get; set; }
        public string TenLoaiChungTuLienQuan { get; set; }
        public string DoiTuongId { get; set; }
        public string MaDoiTuong { get; set; }
        public int? SoChungTuDuocXuLy { get; set; }
        public int? SoChungTuThanhCong { get; set; }
        public int? SoChungTuKhongThanhCong { get; set; }
        public bool? IsMuaHangHoa { get; set; }
        public string TrichYeu { get; set; }

        public string Ma { get; set; }
        public string Ten { get; set; }
    }

    public enum LoaiChungTuLienQuan
    {
        HoaDonGTGT,
        HoaDonBanHang
    }

    public class ThamChieuModelV2
    {
        public string ChungTuId { get; set; }
        public string NgayHachToan { get; set; }
        public string NgayChungTu { get; set; }
        public string SoChungTu { get; set; }
        public string DienGiai { get; set; }
        //public GiaTriLoaiChungTu GiaTriLoaiChungTu { get; set; }
        public BusinessOfType GiaTriLoaiChungTu { get; set; }
    }
}
