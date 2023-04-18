using System.ComponentModel;

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
        TDGToKhai = 100,

        [Description("Thông điệp gửi tờ khai đăng ký thay đổi thông tin đăng ký sử dụng HĐĐT khi ủy nhiệm/nhận ủy nhiệm lập hóa đơn")]
        TDGToKhaiUN = 101,

        [Description("Thông điệp thông báo về việc tiếp nhận/không tiếp nhận tờ khai đăng ký/thay đổi thông tin sử dụng HĐĐT, tờ khai đăng ký thay đổi thông tin đăng k‎ý sử dụng HĐĐT khi ủy nhiệm/nhận ủy nhiệm lập hoá đơn")]
        TBTNToKhai = 102,

        [Description("Thông điệp thông báo về việc chấp nhận/không chấp nhận đăng ký/thay đổi thông tin sử dụng hóa đơn điện tử")]
        TBCNToKhai = 103,

        [Description("Thông điệp thông báo về việc chấp nhận/không chấp nhận đăng ký thay đổi thông tin đăng ký sử dụng HĐĐT khi ủy nhiệm/nhận ủy nhiệm lập hoá đơn")]
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

        [Description("Thông điệp gửi hóa đơn khởi tạo từ máy tính tiền đã cấp mã tới cơ quan thuế.")]
        TDGHDKTTMTT = 206,

        /// <summary>
        /// Nhóm thông điệp đáp ứng nghiệp vụ xử lý hóa đơn có sai sót
        /// </summary>
        [Description("Thông điệp thông báo về hóa đơn điện tử đã lập có sai sót")]
        TDTBHDDLSSot = 300,

        [Description("Thông điệp gửi thông báo về việc tiếp nhận và kết quả xử lý về việc hóa đơn điện tử đã lập có sai sót")]
        TBTNVKQXLHDDTSSot = 301,

        [Description("Thông điệp thông báo về hóa đơn điện tử cần rà soát")]
        TDTBHDDTCRSoat = 302,

        [Description("Thông điệp thông báo về hóa đơn điện tử khởi tạo từ máy tính tiền đã lập có sai sót")]
        TDTBHDDTKTTMTTCSSot = 303,
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
        /// Thông điệp chuyển dữ liệu hóa đơn điện tử không đủ điều kiện cấp mã đến cơ quan thuế
        /// </summary>
        [Description("Thông điệp chuyển dữ liệu hóa đơn điện tử không đủ điều kiện cấp mã đến cơ quan thuế")]
        TDCDLHDDTKDDKCMCQThue = 503,

        /// <summary>
        /// Nhóm thông điệp chuyển dữ liệu hóa đơn điện tử do T-VAN uỷ quyền cấp mã đến cơ quan thuế
        /// </summary>
        [Description("Thông điệp chuyển dữ liệu gửi thông báo mẫu số 01/TB-KTDL về việc kết quả kiểm tra dữ liệu đã được TCUQ gửi cho NNT đến cơ quan thuế")]
        TDCDLGTBKTDLTCUQNNThue = 504,
        /// <summary>
        /// Nhóm thông điệp cung cấp MST có thay đổi thông tin trong ngày
        /// </summary>
        [Description("Thông điệp cung cấp MST có thay đổi thông tin trong ngày")]
        TDCCMSTCTDTTTNgay = 505,

        /// <summary>
        /// Nhóm thông điệp cung cấp quyết định ngừng/tiếp tục sử dụng hóa đơn điện tử
        /// </summary>
        [Description("Thông điệp cung cấp quyết định ngừng/tiếp tục sử dụng hóa đơn điện tử")]
        TDCCQDNTTSDHDDTu = 506,

        /// <summary>
        /// Nhóm thông điệp cung cấp thông tin đăng ký sử dụng hóa đơn điện tử
        /// </summary>
        [Description("Thông điệp cung cấp thông tin đăng ký sử dụng hóa đơn điện tử")]
        TDCCTTDKSDHDDTu = 507,

        /// <summary>
        /// Nhóm Thông điệp gửi đề nghị ký số hóa đơn cấp mã thành công của các đơn vị được ủy quyền cấp mã
        /// </summary>
        [Description("Thông điệp gửi đề nghị ký số hóa đơn cấp mã thành công của các đơn vị được ủy quyền cấp mã")]
        TDDNKSHDCMTCong = 600,

        /// <summary>
        /// Thông điệp Tổng cục Thuế ký số hóa đơn đã được cấp mã thành công gửi Tổ chức ủy quyền cấp mã
        /// </summary>
        [Description("Thông điệp Tổng cục Thuế ký số hóa đơn đã được cấp mã thành công gửi Tổ chức ủy quyền cấp mã")]
        TDTCTCMTCGTCUQCMa = 601,

        /// <summary>
        /// Nhóm Thông điệp gửi đề nghị ký số lên thông báo của các đơn vị được ủy quyền cấp mã
        /// </summary>
        [Description("Thông điệp gửi đề nghị ký số lên thông báo của các đơn vị được ủy quyền cấp mã")]
        TDGDNKSTBDVUQCMa = 602,

        /// <summary>
        /// Nhóm thông điệp cung cấp thông tin đăng ký sử dụng hóa đơn điện tử
        /// </summary>
        [Description("Thông điệp Tổng cục Thuế ký số Thông báo thành công gửi Tổ chức ủy quyền cấp mã.")]
        TDTCTKSTBTCGTCUQCMa = 603,
        /// <summary>
        /// Nhóm thông điệp khác
        /// </summary>
        [Description("Thông điệp phản hồi kỹ thuật")]
        TDCDLTVANUQCTQThue = 999,

        /// <summary>
        /// Nhóm thông điệp khác
        /// </summary>
        [Description("Thông điệp báo cáo đối soát hàng ngày giữa cơ quan thuế và tổ chức truyền nhận.")]
        TDBCDSHNCQTTCTNhan = 901,

        /// <summary>
        /// Nhóm thông điệp khác
        /// </summary>
        [Description("Thông điệp báo cáo đối soát dữ liệu giữa cơ quan thuế và TCTN trong trường hợp ủy quyền cấp mã")]
        TDBCDSDLCQTTCTNUQCMa = 902,

        /// <summary>
        /// Nhóm thông điệp khác
        /// </summary>
        [Description("Thông điệp phản hồi sai định dạng")]
        TDPHSDDang = -1,

        /// <summary>
        /// Nhóm thông điệp khác
        /// </summary>
        [Description("Thông điệp dữ liệu đề nghị ký số bị lỗi")]
        TDDLDNKBLoi = -2,
    }
}
