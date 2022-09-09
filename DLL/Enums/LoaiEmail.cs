using System.ComponentModel;

namespace DLL.Enums
{
    public enum LoaiEmail
    {
        [Description("Gửi hóa đơn nháp")]
        ThongBaoHoaDonNhap=-1,
        [Description("Gửi hóa đơn")]
        ThongBaoPhatHanhHoaDon,
        [Description("Gửi thông báo xóa bỏ hóa đơn")]
        ThongBaoXoaBoHoaDon,
        [Description("Gửi biên bản hủy hóa đơn")]
        ThongBaoBienBanHuyBoHoaDon,
        [Description("Gửi biên bản điều chỉnh hóa đơn")]
        ThongBaoBienBanDieuChinhHoaDon,
        [Description("Gửi thông báo hóa đơn có thông tin sai sót không phải lập lại hóa đơn")]
        ThongBaoSaiThongTinKhongPhaiLapLaiHoaDon,
    }
}
