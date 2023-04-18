using System;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums
{
    [Serializable]
    public enum LTBao
    {
        /// <summary>
        /// Thông báo hóa đơn không đủ điều kiện cấp mã
        /// </summary>
        [XmlEnum("1")]
        ThongBao1 = 1,
        /// <summary>
        /// Thông báo kết quả kiểm tra thông tin gói dữ liệu hợp lệ
        /// </summary>
        [XmlEnum("2")]
        ThongBao2 = 2,
        /// <summary>
        /// Thông báo kết quả kiểm tra thông tin chi tiết từng hóa đơn không hợp lệ
        /// </summary>
        [XmlEnum("3")]
        ThongBao3 = 3,
        /// <summary>
        /// Thông báo kết quả kiểm tra thông tin của Bảng tổng hợp khác xăng dầu, Tờ khai dữ liệu hóa đơn, chứng từ hàng hóa, dịch vụ bán ra không hợp lệ
        /// </summary>
        [XmlEnum("4")]
        ThongBao4 = 4,
        /// <summary>
        /// Thông báo kết quả kiểm tra thông tin của Bảng tổng hợp xăng dầu không hợp lệ
        /// </summary>
        [XmlEnum("5")]
        ThongBao5 = 5,
        /// <summary>
        /// Loại thông báo là “7- Thông báo kết quả đối chiếu sơ bộ thông tin gói dữ liệu hóa đơn khởi tạo từ máy tính tiền không hợp lệ
        /// </summary>
        [XmlEnum("7")]
        ThongBao7 = 7,
        /// <summary>
        /// Thông báo kết quả kiểm tra thông tin gói dữ liệu không hợp lệ các trường hợp khác
        /// </summary>
        [XmlEnum("9")]
        ThongBao9 = 9
    }
}
