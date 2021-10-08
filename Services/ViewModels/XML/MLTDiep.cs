using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Services.ViewModels.XML
{ 
    /// <summary>
    /// Danh sách các loại thông điệp
    /// </summary>
    public enum MLTDiep
    {
        /// <summary>
        /// Nhóm thông điệp đáp ứng nghiệp vụ đăng ký, thay đổi thông tin sử dụng hoá đơn điện tử, đề nghị cấp hóa đơn điện tử có mã theo từng lần phát sinh
        /// </summary>
        [Description("Thông điệp gửi tờ khai đăng ký/thay đổi thông tin sử dụng hóa đơn điện tử")]
        ThongDiepGuiToKhai = 100,

        [Description("Thông điệp gửi tờ khai đăng ký thay đổi thông tin đăng ký sử dụng HĐĐT khi  ủy nhiệm/nhận ủy nhiệm lập hóa đơn")]
        ThongDiepGuiToKhaiUyNhiem = 101,

        [Description("Thông điệp thông báo về việc tiếp nhận/không tiếp nhận tờ khai đăng ký/thay đổi thông tin sử dụng HĐĐT, tờ khai đăng ký thay đổi thông tin đăng k‎ý sử dụng HĐĐT khi ủy nhiệm/nhận ủy nhiệm lập hoá đơn")]
        ThongBaoTiepNhanToKhai = 102,

        [Description("Thông điệp thông báo về việc chấp nhận/không chấp nhận đăng ký/thay đổi thông tin sử dụng hóa đơn điện tử")]
        ThongBaoChapNhanOrKhongToKhai = 103,

        [Description("Thông điệp thông báo về việc chấp nhận/không chấp nhận đăng ký thay đổi thông tin đăng k‎ý sử dụng HĐĐT khi ủy nhiệm/nhận ủy nhiệm lập hoá đơn")]
        ThongBaoChapNhanOrKhongToKhaiUyNhiem = 104,

        [Description("Thông điệp thông báo về việc hết thời gian sử dụng hóa đơn điện tử có mã qua cổng thông tin điện tử Tổng cục Thuế/qua ủy thác tổ chức cung cấp dịch vụ về hóa đơn điện tử; không thuộc trường hợp sử dụng hóa đơn điện tử không có mã")]
        ThongBaoHetThoiGianSuDungHDDTCoMa = 105,
    }
}
