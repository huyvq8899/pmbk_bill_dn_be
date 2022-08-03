using DLL.Entity.QuanLyHoaDon;
using DLL.Enums;
using System;

namespace Services.ViewModels.TienIch
{
    public class NhatKyGuiEmailViewModel : ThongTinChungViewModel
    {
        public string NhatKyGuiEmailId { get; set; }
        public string MauSo { get; set; }
        public string KyHieu { get; set; }
        public string StrKyHieu { get; set; }
        public string So { get; set; }
        public DateTime? Ngay { get; set; }
        public TrangThaiGuiEmail TrangThaiGuiEmail { get; set; }
        public string EmailGui { get; set; }
        public string TenNguoiNhan { get; set; }
        public string EmailNguoiNhan { get; set; }
        public LoaiEmail LoaiEmail { get; set; }
        public string TieuDeEmail { get; set; }
        public string RefId { get; set; }
        public RefType RefType { get; set; }

        public string TenTrangThaiGuiEmail { get; set; }
        public string TenLoaiEmail { get; set; }
        public string TenNguoiGui { get; set; }
        public string NguoiThucHien { get; set; }

        public NhatKyGuiEmailViewModel Child { get; set; }

        public ThongBaoSaiThongTin thongBaoSaiThongTin { get; set; }
    }
}
