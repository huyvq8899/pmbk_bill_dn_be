using System.ComponentModel;

namespace DLL.Enums
{
    public enum LoaiHoaDon
    {
        [Description("Phiếu xuất kho")]
        PhieuXuatKho = -100,
        [Description("Tất cả")]
        TatCa = -1,
        None,
        Pxk = -100,
        Mtt = -99,
        [Description("Hóa đơn GTGT")]
        HoaDonGTGT = 1,
        [Description("Hóa đơn bán hàng")]
        HoaDonBanHang = 2,
        [Description("Hóa đơn bán tài sản công")]
        HoaDonBanTaiSanCong = 3,
        [Description("Hóa đơn bán hàng dự trữ quốc gia")]
        HoaDonBanHangDuTruQuocGia = 4,
        [Description("Các loại hóa đơn khác")]
        CacLoaiHoaDonKhac = 5,
        [Description("Các chứng từ được in, phát hành, sử dụng và quản lý như hóa đơn")]
        CacCTDuocInPhatHanhSuDungVaQuanLyNhuHD = 6,
        [Description("PXK kiêm vận chuyển nội bộ")]
        PXKKiemVanChuyenNoiBo = 7,
        [Description("PXK hàng gửi bán đại lý")]
        PXKHangGuiBanDaiLy = 8,
        [Description("Hóa đơn GTGT")]
        HoaDonGTGTCMTMTT = 9,
        [Description("Hóa đơn bán hàng")]
        HoaDonBanHangCMTMTT = 10,
        [Description("Hóa đơn bán hàng từ máy tính tiền")]
        HoaDonNhieuTyGia = 11,
        [Description("Hóa đơn bán hàng từ máy tính tiền")]
        HoaDonGTGTKiemToKhaiHoanThue = 12,
        [Description("Hóa đơn khác")]
        HoaDonKhacCMTMTT = 13,
        [Description("Tem, vé, điện tử là hóa đơn GTGT")]
        TemVeGTGT = 14,
        [Description("Tem, vé, điện tử là hóa đơn bán hàng")]
        TemVeBanHang = 15
    }
    public enum HinhThucThongBaoSaiSot
    {
        [Description("Tất cả")]
        TatCa = -1,
        [Description("Hủy")]
        Huy = 1,
        [Description("Điều chỉnh")]
        DieuChinh = 2,
        [Description("Thay thế")]
        ThayThe = 3,
        [Description("Giải trình")]
        GiaTrinh = 4,
    }
    public enum LoaiChungTu
    {
        [Description("Tất cả")]
        TatCa = -1,
        [Description("Chứng từ khấu trừ điện tử")]
        ChungTuKhauTru = 1,
        [Description("Biên lại điện tử")]
        BienLai = 2
    }
}
