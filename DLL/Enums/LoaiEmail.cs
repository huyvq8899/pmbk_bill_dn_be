using System.ComponentModel;

namespace DLL.Enums
{
    public enum LoaiEmail
    {
        [Description("Gửi hóa đơn cho khách hàng")]
        ThongBaoPhatHanhHoaDon,
        [Description("Gửi thông báo xóa hóa đơn cho khách hàng")]
        ThongBaoXoaBoHoaDon,
        [Description("Gửi biên bản hủy hóa đơn cho khách hàng")]
        ThongBaoBienBanHuyBoHoaDon,
        [Description("Gửi biên bản điều chỉnh hóa đơn cho khách hàng")]
        ThongBaoBienBanDieuChinhHoaDon,
        [Description("Gửi thông báo sai sót không phải lập lại hóa đơn cho khách hàng")]
        ThongBaoSaiThongTinKhongPhaiLapLaiHoaDon,
    }
}
