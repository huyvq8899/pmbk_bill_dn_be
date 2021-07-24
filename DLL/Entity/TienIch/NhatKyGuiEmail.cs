using DLL.Enums;
using System;

namespace DLL.Entity.TienIch
{
    public class NhatKyGuiEmail : ThongTinChung
    {
        public string NhatKyGuiEmailId { get; set; }
        public string MauSo { get; set; }
        public string KyHieu { get; set; }
        public string So { get; set; }
        public DateTime? Ngay { get; set; }
        public TrangThaiGuiEmail TrangThaiGuiEmail { get; set; }
        public string EmailGui { get; set; }
        public string TenNguoiGui { get; set; }
        public string TenNguoiNhan { get; set; }
        public string EmailNguoiNhan { get; set; }
        public LoaiEmail LoaiEmail { get; set; }
        public string TieuDeEmail { get; set; }
        public string RefId { get; set; }
        public RefType RefType { get; set; }
    }
}
