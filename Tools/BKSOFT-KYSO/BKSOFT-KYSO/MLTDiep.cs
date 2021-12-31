using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BKSOFT_KYSO
{
    public enum MLTDiep
    {
        /// <summary>
        /// Ký biên bản cho Bên A
        /// </summary>
        [Description("Ký biên bản cho Bên A")]
        BBCBenA = 10,

        /// <summary>
        /// Ký biên bản cho Bên B
        /// </summary>
        [Description("Ký biên bản cho Bên B")]
        BBCBenB = 11,

        /// <summary>
        /// Thông tin chứng thư số
        /// </summary>
        [Description("Thông tin chứng thư số")]
        TTCTSo = 50,

        [Description("Hiển thị chứng thư số")]
        CTSInfo = 60,

        /// <summary>
        /// Nhóm thông điệp đáp ứng nghiệp vụ đăng ký, thay đổi thông tin sử dụng hoá đơn điện tử, đề nghị cấp hóa đơn điện tử có mã theo từng lần phát sinh
        /// </summary>
        [Description("Thông điệp gửi tờ khai đăng ký/thay đổi thông tin sử dụng hóa đơn điện tử")]
        TDGToKhai = 100,

        [Description("Thông điệp gửi tờ khai đăng ký thay đổi thông tin đăng ký sử dụng HĐĐT khi ủy nhiệm/nhận ủy nhiệm lập hóa đơn")]
        TDGToKhaiUN = 101,

        [Description("Thông điệp thông báo về việc tiếp nhận/không tiếp nhận tờ khai đăng ký/thay đổi thông tin sử dụng HĐĐT, tờ khai đăng ký thay đổi thông tin đăng k‎ý sử dụng HĐĐT khi ủy nhiệm/nhận ủy nhiệm lập hoá đơn")]
        TBTNToKhai = 102,

        [Description("Thông điệp thông báo về việc chấp nhận/không chấp nhận đăng ký/thay đổi thông tin sử dụng hóa đơn điện tử")]
        TBCNToKhai = 103,

        [Description("Thông điệp thông báo về việc chấp nhận/không chấp nhận đăng ký thay đổi thông tin đăng k‎ý sử dụng HĐĐT khi ủy nhiệm/nhận ủy nhiệm lập hoá đơn")]
        TBCNToKhaiUN = 104,

        [Description("Thông điệp thông báo về việc hết thời gian sử dụng hóa đơn điện tử có mã qua cổng thông tin điện tử Tổng cục Thuế/qua ủy thác tổ chức cung cấp dịch vụ về hóa đơn điện tử; không thuộc trường hợp sử dụng hóa đơn điện tử không có mã")]
        TBHTGHDDT = 105,

        [Description("Thông điệp gửi Đơn đề nghị cấp hóa đơn điện tử có mã của CQT theo từng lần phát sinh")]
        TDDNCHDDT = 106,



        /// <summary>
        /// Nhóm thông điệp đáp ứng nghiệp vụ lập và gửi hóa đơn điện tử đến cơ quan thuế
        /// </summary>
        [Description("Thông điệp gửi hóa đơn điện tử tới cơ quan thuế để cấp mã")]
        TDGHDDTTCQTCapMa = 200,

        [Description("Thông điệp gửi hóa đơn điện tử tới cơ quan thuế để cấp mã theo từng lần phát sinh")]
        TDGHDDTTCQTCMTLPSinh = 201,

        [Description("Thông điệp thông báo kết quả cấp mã hóa đơn điện tử của cơ quan thuế")]
        TBKQCMHDon = 202,

        [Description("Thông điệp chuyển dữ liệu hóa đơn điện tử không mã đến cơ quan thuế")]
        TDCDLHDKMDCQThue = 203,

        [Description("Thông điệp thông báo về việc kết quả kiểm tra dữ liệu hóa đơn điện tử")]
        TDTBKQKTDLHDon = 204,

        [Description("Thông điệp gửi đề nghị cấp hóa đơn điện tử có mã của CQT theo từng lần phát sinh")]
        TDDNCHDDTCMa = 205,

        [Description("Thông điệp thông báo về việc kết quả kiểm tra <hồ sơ đề nghị cấp hóa đơn điện tử có mã của cơ quan thuế/chứng từ nộp thuế/hóa đơn điện tử có mã của cơ quan thuế > theo từng lần phát sinh")]
        TDTBKQTTPSinh = 206,

        [Description("Người mua ký hóa đơn")]
        TDNMKHDon = 207,

        /// <summary>
        /// Nhóm thông điệp đáp ứng nghiệp vụ xử lý hóa đơn có sai sót
        /// </summary>
        [Description("Thông điệp thông báo về hóa đơn điện tử đã lập có sai sót")]
        TDTBHDDLSSot = 300,

        [Description("Thông điệp gửi thông báo về việc tiếp nhận và kết quả xử lý về việc hóa đơn điện tử đã lập có sai sót")]
        TBTNVKQXLHDDTSSot = 301,

        [Description("Thông điệp thông báo về hóa đơn điện tử cần rà soát")]
        TDTBHDDTCRSoat = 302,

        /// <summary>
        /// Nhóm thông điệp chuyển bảng tổng hợp dữ liệu hóa đơn điện tử đến cơ quan thuế
        /// </summary>
        [Description("Thông điệp chuyển bảng tổng hợp dữ liệu hóa đơn điện tử đến cơ quan thuế")]
        TDCBTHDLHDDDTDCQThue = 400,

        /// <summary>
        /// Nhóm thông điệp chuyển dữ liệu hóa đơn điện tử do T-VAN uỷ quyền cấp mã đến cơ quan thuế
        /// </summary>
        [Description("Thông điệp chuyển dữ liệu hóa đơn điện tử do T-VAN uỷ quyền cấp mã đến cơ quan thuế")]
        TDCDLTVANUQCQThue = 500,

        /// <summary>
        /// Nhóm thông điệp khác
        /// </summary>
        [Description("Thông điệp phản hồi kỹ thuật")]
        TDCDLTVANUQCTQThue = 999
    }
}
