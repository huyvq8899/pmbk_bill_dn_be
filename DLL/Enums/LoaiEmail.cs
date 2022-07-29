using System.ComponentModel;

namespace DLL.Enums
{
    public enum LoaiEmail
    {
        [Description("Gửi hóa đơn")]
        ThongBaoPhatHanhHoaDon,
        [Description("Gửi thông báo xóa bỏ HĐ")]
        ThongBaoXoaBoHoaDon,
        [Description("Gửi biên bản hủy HĐ")]
        ThongBaoBienBanHuyBoHoaDon,
        [Description("Gửi biên bản điều chỉnh HĐ")]
        ThongBaoBienBanDieuChinhHoaDon,
        [Description("Gửi thông báo HĐ có thông tin sai sót không phải lập lại HĐ")]
        ThongBaoSaiThongTinKhongPhaiLapLaiHoaDon,
    }
}
