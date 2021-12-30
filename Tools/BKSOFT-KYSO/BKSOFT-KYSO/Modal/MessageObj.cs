using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BKSOFT_KYSO.Modal
{
    public class MessageObj
    {
        // Tool signed TT32
        public int Type { set; get; }

        public NBan NBan { set; get; }

        public string ContentErro { get; set; }

        public int ErrorType { get; set; }

        public bool IsTT32 { set; get; }

        public bool IsCompression { set; get; }

        // Tool signed TT78
        public MLTDiep MLTDiep { set; get; }

        public string MST { set; get; }

        public List<string> Serials { set; get; }

        public string DataXML { set; get; }

        public string DataJson { set; get; }

        public string UrlXML { set; get; }

        public string XMLSigned { set; get; }

        public string DataPDF { set; get; }

        public string UrlPDF { set; get; }

        public string PDFSigned { set; get; }

        public string SerialSigned { set; get; }

        public TypeOfError TypeOfError { set; get; }

        public string Exception { set; get; }

        public TTNKy TTNKy { set; get; }

        public byte[] Cert { get; set; }

        public MessageObj() { }
    }

    public enum TYPE_MESSAGE
    {
        SIGN_INVOICE = 1000,
        SIGN_INVOICE_XML = 1001,
        SIGN_RECORD = 1002,
        SIGN_MULTIPLE_INVOICE = 1003,
        SIGN_RECORD_FOR_A = 1004,
        SIGN_RECORD_FOR_B = 1005,

        REP_SIGN_SUC = 2000,
        REP_SIGN_ERR = 2001,
    }

    public enum TypeOfError
    {
        NONE = 0,

        [Description("Không tìm thấy chữ ký số")]
        CERT_NOT_FOUND = 100,

        [Description("Mã số thuế người bán trống")]
        TAXCODE_SALLER_EMPTY = 102,

        [Description("Mã số thuế của chứng thư không khớp với mã số thuế đăng ký dịch vụ hóa đơn điện tử")]
        TAXCODE_SALLER_DIFF = 103,

        [Description("Serial của chứng thư không khớp với serial đăng ký dịch vụ hóa đơn điện tử")]
        SERIAL_SALLER_DIFF = 104,

        [Description("Chứng thư chỉ ký số trong khoảng thời gian từ {0} đến {1}")]
        SIGN_DATE_INVAILD = 105,

        [Description("Ngày lập tờ khai không hợp lệ")]
        DATE_TKHAI_INVAILD = 106,

        [Description("Ngày lập hóa đơn không hợp lệ")]
        DATE_INVOICE_INVAILD = 107,

        [Description("Ngày ký hóa đơn {0} khác ngày lập hóa đơn {1}")]
        DATE_INVOICE_OTHER = 108,

        [Description("Ký số XML hóa đơn lỗi")]
        SIGN_XML_ERROR = 109,

        [Description("Ký số PDF hóa đơn lỗi")]
        SIGN_PDF_ERROR = 110
    }
}
