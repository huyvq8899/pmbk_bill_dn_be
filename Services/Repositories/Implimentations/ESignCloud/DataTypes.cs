using DLL;
using Services.Helper;
using Services.Helper.Params.HoaDon;
using Services.ViewModels.XML;
using Spire.Doc.Documents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories.Implimentations.ESignCloud
{
    public class DataTypes
    {
        
        
    }

    // Classes defination
    public class SignCloudReq
    {
        public string relyingParty { get; set; }
        public string relyingPartyBillCode { get; set; }
        public string agreementUUID { get; set; }
        public string sharedAgreementUUID { get; set; }
        public string sharedRelyingParty { get; set; }
        public string mobileNo { get; set; }
        public string email { get; set; }
        public string certificateProfile { get; set; }
        public string signingFileUUID { get; set; }
        public byte[] signingFileData { get; set; }
        public string signingFileName { get; set; }
        public string mimeType { get; set; }
        public string notificationTemplate { get; set; }
        public string notificationSubject { get; set; }
        public bool timestampEnabled { get; set; }
        public bool ltvEnabled { get; set; }
        public string language { get; set; }
        public string authorizeCode { get; set; }
        public bool postbackEnabled { get; set; }
        public bool noPadding { get; set; }
        public int authorizeMethod { get; set; }
        public byte[] uploadingFileData { get; set; }
        public string downloadingFileUUID { get; set; }
        public string currentPasscode { get; set; }
        public string newPasscode { get; set; }
        public string hash { get; set; }
        public string hashAlgorithm { get; set; }
        public string encryption { get; set; }
        public string billCode { get; set; }
        public int messagingMode { get; set; }
        public int sharedMode { get; set; }
        public string xslTemplateUUID { get; set; }
        public string xslTemplate { get; set; }
        public string xmlDocument { get; set; }
        public bool p2pEnabled { get; set; }
        public bool csrRequired { get; set; }
        public bool certificateRequired { get; set; }
        public bool keepOldKeysEnabled { get; set; }
        public bool revokeOldCertificateEnabled { get; set; }
        public string certificate { get; set; }


        public List<MultipleSigningFileData> multipleSigningFileData { get; set; }
        public SignCloudMetaData signCloudMetaData { get; set; }
        public AgreementDetails agreementDetails { get; set; }
        public CredentialData credentialData { get; set; }
    }

    public class CredentialData
    {
        public string username { get; set; }
        public string password { get; set; }
        public string signature { get; set; }
        public string pkcs1Signature { get; set; }
        public string timestamp { get; set; }
    }

    public class AgreementDetails
    {
        public string personalName { get; set; }
        public string organization { get; set; }
        public string organizationUnit { get; set; }
        public string title { get; set; }
        public string email { get; set; }
        public string telephoneNumber { get; set; }
        public string location { get; set; }
        public string stateOrProvince { get; set; }
        public string country { get; set; }
        public string personalID { get; set; }
        public string passportID { get; set; }
        public string citizenID { get; set; }
        public string taxID { get; set; }
        public string budgetID { get; set; }

        public byte[] applicationForm { get; set; }
        public byte[] requestForm { get; set; }
        public byte[] authorizeLetter { get; set; }
        public byte[] photoIDCard { get; set; }
        public byte[] photoFrontSideIDCard { get; set; }
        public byte[] photoBackSideIDCard { get; set; }
        public byte[] photoActivityDeclaration { get; set; }
        public byte[] photoAuthorizeDelegate { get; set; }
    }

    public class SignCloudMetaData
    {
        public Dictionary<string, string> singletonSigning { get; set; }
        public Dictionary<string, string> counterSigning { get; set; }
    }

    public class MultipleSigningFileData
    {
        public string hash { get; set; }
        public byte[] signingFileData { get; set; }
        public string signingFileName { get; set; }
        public string mimeType { get; set; }
        public string xslTemplate { get; set; }
        public string xmlDocument { get; set; }
        public SignCloudMetaData signCloudMetaData { get; set; }
    }

    public class SignCloudResp
    {
        public int responseCode { get; set; }
        public string responseMessage { get; set; }
        public string billCode { get; set; }
        public long timestamp { get; set; }
        public int logInstance { get; set; }
        public string notificationMessage { get; set; }
        public int remainingCounter { get; set; }
        public byte[] signedFileData { get; set; }
        public string signedFileName { get; set; }
        public string authorizeCredential { get; set; }
        public string signedFileUUID { get; set; }
        public string mimeType { get; set; }
        public string certificateDN { get; set; }
        public string certificateSerialNumber { get; set; }
        public string certificateThumbprint { get; set; }
        public long validFrom { get; set; }
        public long validTo { get; set; }
        public string issuerDN { get; set; }
        public string uploadedFileUUID { get; set; }
        public string downloadedFileUUID { get; set; }
        public byte[] downloadedFileData { get; set; }
        public string signatureValue { get; set; }
        public int authorizeMethod { get; set; }
        public string notificationSubject { get; set; }
        public string dmsMetaData { get; set; }
        public string csr { get; set; }
        public string certificate { get; set; }
        public int certificateStateID { get; set; }
        public List<MultipleSignedFileData> multipleSignedFileData { get; set; }
    }

    public class MultipleSignedFileData
    {
        public byte[] signedFileData { get; set; }
        public string mimeType { get; set; }
        public string signedFileName { get; set; }
        public string signedFileUUID { get; set; }
        public string dmsMetaData { get; set; }
        public string signatureValue { get; set; }
    }
    public class MessageObj
    {
        // Tool signed contract
        public string Keywords { set; get; }

        // Tool signed TT32
        public int Type { set; get; }

        public NBan NBan { set; get; }

        public string ContentErro { get; set; }

        public int ErrorType { get; set; }

        public bool IsCompression { set; get; }

        // Tool signed TT78
        public byte[] Cert { get; set; }

        public bool IsTT32 { set; get; }

        public bool IsMultipleEnd { set; get; }

        public bool IsNMua { set; get; }

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
        public string UserkeySign { set; get; }
        public string PassCode { set; get; }

        public TTNKy TTNKy { set; get; }

        public bool IsSignBKH { get; set; }

        public MessageObj() { }
    }
    public enum TypeOfError
    {
        NONE = 0,

        [Description("Thẻ /TDiep/DLieu/TKhai/DLTKhai/TTChung/Nlap tờ khai không tồn tại hoặc trống")]
        NLAP_TKHAI_TRONG = 1000,

        [Description("Ngày lập tờ khai không hợp lệ. Ngày lập > ngày hiện tại")]
        NLAP_TKHAI_KHLe = 1001,

        [Description("Thẻ /TDiep/DLieu/TKhai/DLTKhai/TTChung/NLap hóa đơn không tồn tại hoặc trống")]
        NLAP_HDON_TRONG = 2000,

        [Description("Ngày lập tờ khai không hợp lệ. Ngày lập > ngày hiện tại")]
        NLAP_HDON_KHLe = 2001,

        [Description("Ký số XML hóa đơn lỗi")]
        KSO_XML_LOI = 9000,

        [Description("Chứng thư chỉ ký số trong khoảng thời gian từ {0} đến {1}")]
        NKY_KHONG_HLe = 9001,

        [Description("Mã số thuế của chứng thư không khớp với mã số thuế đăng ký dịch vụ hóa đơn điện tử")]
        MST_KHONG_HLe = 9002,

        [Description("Không tìm thấy chữ ký số")]
        CERT_NOT_FOUND = 100,

        [Description("Mã số thuế người bán trống")]
        TAXCODE_SALLER_EMPTY = 102,

        [Description("Mã số thuế của chứng thư không khớp với mã số thuế đăng ký dịch vụ hóa đơn điện tử")]
        TAXCODE_SALLER_DIFF = 103,

        [Description("Serial của chứng thư không khớp với serial đăng ký dịch vụ hóa đơn điện tử")]
        SERIAL_SALLER_DIFF = 104,

        [Description("Serial của chứng thư không khớp với serial đã khai báo tại danh sách chứng thư số sử dụng tại Thông tin người nộp thuế")]
        SERIAL_SALLER_DIFF_B = 112,

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
        SIGN_PDF_ERROR = 110,

        [Description("Không tìm thấy vị trí ký số PDF")]
        SIGN_PDF_NOT_FIND = 111,
    }
}
