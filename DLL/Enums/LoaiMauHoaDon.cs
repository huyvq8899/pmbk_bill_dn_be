using System.ComponentModel;

namespace DLL.Enums
{
    public enum LoaiMauHoaDon
    {
        None,
        [Description("Mẫu cơ bản")]
        MauCoBan = 1,
        [Description("Vé vận tải hành khách thể hiện mệnh giá")]
        VeVanTaiHanhKhachTheHienMenhGia = 2,
        [Description("Vé vận tải hành khách không thể hiện mệnh giá")]
        VeVanTaiHanhKhachKhongTheHienMenhGia = 3,
        [Description("Vé dịch vụ thể hiện mệnh giá")]
        VeDichVuTheHienMenhGia = 4,
        [Description("Vé dịch vụ không thể hiện mệnh giá")]
        VeDichVuKhongTheHienMenhGia = 5,
        [Description("Vé dịch vụ công ích thể hiện mệnh giá")]
        VeDichVuCongIchTheHienMenhGia = 6,
        [Description("Vé xe buýt thể hiện mệnh giá")]
        VeXeBuytTheHienMenhGia = 7
    }
}
