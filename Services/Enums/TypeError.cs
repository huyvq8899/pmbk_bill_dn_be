using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Services.Enums
{
    public enum TypeError
    {
        NONE = 0,

        [Description("Không tìm thấy chữ ký số")]
        CERT_NOT_FOUND = 100,

        [Description("Thông tin người bán trống")]
        SALLER_EMPTY = 101,

        [Description("MST người bán trống")]
        TAXCODE_SALLER_EMPTY = 102,

        [Description("Mã số thuế của chứng thư không khớp với mã số thuế đăng ký dịch vụ hóa đơn điện tử")]
        TAXCODE_SALLER_DIFF = 103,

        [Description("Chứng thư chỉ ký số trong khoảng thời gian từ {0} đến {1}")]
        SIGN_DATE_INVAILD = 104,

        [Description("Ngày lập hóa đơn không hợp lệ")]
        DATA_INVOICE_INVAILD = 105,

        [Description("Ngày ký hóa đơn {0} khác ngày lập hóa đơn {1}")]
        DATA_INVOICE_OTHER = 106,

        [Description("Ký số XML hóa đơn lỗi")]
        SIGN_XML_ERROR = 107,

        [Description("Ký số PDF hóa đơn lỗi")]
        SIGN_PDF_ERROR = 108,
    }
}
