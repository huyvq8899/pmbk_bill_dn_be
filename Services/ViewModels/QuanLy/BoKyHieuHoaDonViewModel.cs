using DLL.Entity.DanhMuc;
using DLL.Entity.QuyDinhKyThuat;
using DLL.Enums;
using System.Collections.Generic;

namespace Services.ViewModels.QuanLy
{
    public class BoKyHieuHoaDonViewModel : ThongTinChungViewModel
    {
        public string BoKyHieuHoaDonId { get; set; }
        public UyNhiemLapHoaDon UyNhiemLapHoaDon { get; set; }
        public HinhThucHoaDon HinhThucHoaDon { get; set; }
        public LoaiHoaDon LoaiHoaDon { get; set; }
        public string KyHieu { get; set; }
        public string KyHieuMauSoHoaDon { get; set; }
        public string KyHieuHoaDon { get; set; }
        public string KyHieu1 { get; set; }
        public string KyHieu23 { get; set; }
        public string KyHieu4 { get; set; }
        public string KyHieu56 { get; set; }
        public int? SoBatDau { get; set; }
        public int? SoLonNhatDaLapDenHienTai { get; set; }
        public int? SoToiDa { get; set; }
        public bool? IsTuyChinh { get; set; } // tùy chỉnh nguyên tắc số hóa đơn
        public string MauHoaDonId { get; set; }
        public string DangKyUyNhiemId { get; set; }
        public TrangThaiSuDung TrangThaiSuDung { get; set; }

        public MauHoaDon MauHoaDon { get; set; }
        public DangKyUyNhiem DangKyUyNhiem { get; set; }

        public List<NhatKyXacThucBoKyHieuViewModel> NhatKyXacThucBoKyHieus { get; set; }
    }
}
