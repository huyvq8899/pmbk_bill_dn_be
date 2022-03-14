using System.ComponentModel;

namespace DLL.Enums
{
    public enum LoaiThongTinChiTiet
    {
        [Description("Tạm ngừng sử dụng")]
        TamNgungSuDung,

        [Description("Có mã của cơ quan thuế")]
        CoMaCuaCoQuanThue,
        [Description("Không có mã của cơ quan thuế")]
        KhongCoMaCuaCoQuanThue,

        [Description("Hóa đơn GTGT")]
        HoaDonGTGT,
        [Description("Hóa đơn bán hàng")]
        HoaDonBanHang,
        [Description("Hóa đơn bán tài sản công")]
        HoaDonBanTaiSanCong,
        [Description("Hóa đơn bán hàng dự trữ quốc gia")]
        HoaDonBanHangDuTruQuocGia,
        [Description("Các loại hóa đơn khác")]
        CacLoaiHoaDonKhac,
        [Description("Các chứng từ được in, phát hành, sử dụng và quản lý như hóa đơn")]
        CacChungTuDuocInPhatHanhSuDungVaQuanLyNhuHoaDon
    }
}
