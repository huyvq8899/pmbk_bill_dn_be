using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKSOFT_KYSO
{
    public class Constants
    {
        public const string OBJECT_DATA = @"<SignatureProperties>
	<SignatureProperty>
		<SigningTime>{0}</SigningTime>
	</SignatureProperty>
</SignatureProperties>
";

        public const string STARTUP_KEY = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
        public const string STARTUP_OLD_VALUE = "USB-Token-BILL";
        public const string STARTUP_VALUE = "BKSOFT-KYSO";

        public const int SETTING_PORT = 15872;
        public const string URL_PATH_SIGN = "tools/DigitalSignature/BKSOFT-KYSO-SETUP.zip";

        /// <summary>
        /// https://hdbk.pmbk.vn/tools/DigitalSignature/version.json
        /// </summary>
        public const string SETTING_URL = "aHR0cHM6Ly9oZGJrLnBtYmsudm4vdG9vbHMvRGlnaXRhbFNpZ25hdHVyZS92ZXJzaW9uLmpzb24=";

        public const string SETTING_DATE = "10.0.1";
        public const string SETTING_VERSION = "15.04.2021";

        public const string BKSOFT_KYSO_ZIP = "BKSOFT-KYSO.zip";
        public const string KYSO_UPDATE_EXE = "KYSO-UPDATE.exe";

        public const string PDF_SIGNATURE_REASON = "Hóa đơn giá trị gia tăng";
        public const string PDF_SIGNATURE_CONTACT_INFO_LABEL = "Điện thoại: ";
        public const string PDF_SIGNATURE_DATE_LABEL = "\nNgày ký: ";
        public const string PDF_SIGNATURE_REASON_LABEL = "\nReason: ";

        public const string SIGNATURE_GREEN_TICK = "greentick.png";
        public const string SIGNATURE_DIGITAL_SIGNER_LABLE = "Ký bởi: ";
        public const string SIGNATURE_DIGITAL_SIGNER_VALID = "Signature Valid\nKý bởi: ";
        public const string SIGNATURE_DIGITAL_SIGNER_P01 = "\n{0}\nNgày ký: {1}\n\n\n";
        public const string SIGNATURE_DIGITAL_SIGNER_P02 = "\n{0}\n{1}\nNgày ký: {2}\n\n\n";

        public const string MSG_TITLE_DIALOG = "BKSOFT Invoice";
        public const string MSG_NOT_SELECT_CERT = "Không có chứng thư số nào được chọn.";
        public const string MSG_MST_INVAILD = "Mã số thuế của chứng thư không khớp với mã số thuế đăng ký dịch vụ hóa đơn điện tử. Vui lòng kiểm tra lại.";
        public const string MSG_SERIAL_INVAILD = "Serial của chứng thư không khớp với serial đăng ký dịch vụ hóa đơn điện tử. Vui lòng kiểm tra lại.";
        public const string MSG_SERIAL_INVAILD_B = "Serial của chứng thư không khớp với serial đã khai báo tại danh sách chứng thư số sử dụng tại Thông tin người nộp thuế. Vui lòng kiểm tra lại.";
        public const string MSG_DATE_INVAILD = "Chứng thư chỉ ký số trong khoảng thời gian từ {0} đến {1}. Vui lòng kiểm tra lại.";
        public const string MSG_DATE_INV_SIGN_INVAILD = "Ngày ký hóa đơn {0} khác ngày lập hóa đơn {1}. Bạn vui lòng sửa lại trước khi thực hiện ký số hóa đơn.";
    }
}
