using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.IV._1
{
    public partial class NDBTHDLieu
    {
        [XmlArray("DSDLieu")]
        [XmlArrayItem("DLieu")]
        public List<DLieu> DSDLieu {  get; set; }
    }

    public partial class DLieu
    {
        /// <summary>
        /// <para>Số thứ tự</para>
        /// <para>Độ dài tối đa: 6</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public int? STT { get; set; }

        /// <summary>
        /// <para>Ký hiệu mẫu số hóa đơn</para>
        /// <para>Độ dài tối đa: 11</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự (Chi tiết tại Phụ lục II kèm theo Quy định này)(Chú thích: KHMSHDon.cs)</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại Khoản 14, Điều 10, Nghị định số 123/2020/NĐ-CP và Loại hàng hóa, dịch vụ kinh doanh là 1- Xăng dầu)</para>
        /// </summary>
        [MaxLength(11)]
        public string KHMSHDon { get; set; }

        /// <summary>
        /// <para>Ký hiệu hóa đơn</para>
        /// <para>Độ dài tối đa: 8</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại Khoản 14, Điều 10, Nghị định số 123/2020/NĐ-CP và Loại hàng hóa, dịch vụ kinh doanh là 1- Xăng dầu)</para>
        /// </summary>
        [MaxLength(8)]
        public string KHHDon { get; set; }

        /// <summary>
        /// <para>Số hóa đơn</para>
        /// <para>Độ dài tối đa: 8</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại Khoản 14, Điều 10, Nghị định số 123/2020/NĐ-CP và Loại hàng hóa, dịch vụ kinh doanh là 1- Xăng dầu)</para>
        /// </summary>
        [MaxLength(8)]
        public int? SHDon { get; set; }

        /// <summary>
        /// <para>Ngày lập (Ngày tháng năm lập hóa đơn)</para>
        /// <para>Kiểu dữ liệu: Ngày/para>
        /// <para>Bắt buộc (Trừ trường hợp Loại hàng hóa, dịch vụ kinh doanh là 1- Xăng dầu)</para>
        /// </summary>
        public string NLap { get; set; }

        /// <summary>
        /// <para>Tên người mua</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại Khoản 14, Điều 10, Nghị định số 123/2020/NĐ-CP và Loại hàng hóa, dịch vụ kinh doanh là 1- Xăng dầu)</para>
        /// </summary>
        [MaxLength(400)]
        public string TNMua { get; set; }

        /// <summary>
        /// <para>Mã số thuế người mua</para>
        /// <para>Độ dài tối đa: 14</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại Khoản 14, Điều 10, Nghị định số 123/2020/NĐ-CP và Loại hàng hóa, dịch vụ kinh doanh là 1- Xăng dầu)</para>
        /// </summary>
        [MaxLength(14)]
        public string MSTNMua { get; set; }

        /// <summary>
        /// <para>Mã khách hàng</para>
        /// <para>Độ dài tối đa: 50</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc (Trừ trường hợp quy định tại Điều 22 Nghị định số 123/2020/NĐ-CP)</para>
        /// </summary>
        [MaxLength(50)]
        public string MKHang { get; set; }

        /// <summary>
        /// <para>Mã hàng hóa, dịch vụ</para>
        /// <para>Độ dài tối đa: 50</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Đối với Loại hàng hóa, dịch vụ kinh doanh là 1- Xăng dầu)</para>
        /// </summary>
        [MaxLength(50)]
        public string MHHoa { get; set; }

        /// <summary>
        /// <para>Tên hàng hóa, dịch vụ (Mặt hàng)</para>
        /// <para>Độ dài tối đa: 500</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Đối với Loại hàng hóa, dịch vụ kinh doanh là 1- Xăng dầu)</para>
        /// </summary>
        [MaxLength(500)]
        public string THHDVu { get; set; }

        /// <summary>
        /// <para>Đơn vị tính</para>
        /// <para>Độ dài tối đa: 50</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Đối với Loại hàng hóa, dịch vụ kinh doanh là 1- Xăng dầu (trừ trường hợp Trạng thái là Điều chỉnh, Giải trình, Sai sót do tổng hợp))</para>
        /// </summary>
        [MaxLength(50)]
        public string DVTinh { get; set; }

        /// <summary>
        /// <para>Số lượng hàng hóa</para>
        /// <para>Độ dài tối đa: 19,4</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc (Đối với Loại hàng hóa, dịch vụ kinh doanh là 1- Xăng dầu (trừ trường hợp Trạng thái là Điều chỉnh, Giải trình, Sai sót do tổng hợp))</para>
        /// </summary>
        [Column(TypeName = "decimal(19, 4)")]
        public decimal? SLuong { get; set; }

        /// <summary>
        /// <para>Tổng giá trị hàng hóa, dịch vụ bán ra chưa có thuế GTGT</para>
        /// <para>Độ dài tối đa: 19,4</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại Điều 10, Điều 22 Nghị định 123/2020/NĐ-CP)</para>
        /// </summary>
        [Column(TypeName = "decimal(19, 4)")]
        public decimal? TTCThue { get; set; }

        /// <summary>
        /// <para>Thuế suất (Thuế suất thuế GTGT)</para>
        /// <para>Độ dài tối đa: 5</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự (chi tiết tại Phụ lục V kèm theo Quy định này)(Chú thích: TSuat.cs)</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại Điều 10, Điều 22 Nghị định 123/2020/NĐ-CP)</para>
        /// </summary>
        [MaxLength(5)]
        public string TSuat { get; set; }

        /// <summary>
        /// <para>Tổng tiền thuế (Tổng tiền thuế GTGT)</para>
        /// <para>Độ dài tối đa: 19,4</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại Điều 10, Điều 22 Nghị định 123/2020/NĐ-CP)</para>
        /// </summary>
        [Column(TypeName = "decimal(19, 4)")]
        public decimal? TgTThue { get; set; }

        /// <summary>
        /// <para>Tổng tiền thanh toán</para>
        /// <para>Độ dài tối đa: 19,4</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại Điều 10, Điều 22 Nghị định 123/2020/NĐ-CP)</para>
        /// </summary>
        [Column(TypeName = "decimal(19, 4)")]
        public decimal? TgTTToan { get; set; }

        /// <summary>
        /// <para>Trạng thái</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(1)]
        public TCTBao TThai { get; set; }

        /// <summary>
        /// <para>Loại hóa đơn có liên quan (Loại áp dụng hóa đơn của HĐ có liên quan)</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc (Đối với trường hợp điều chỉnh, thay thế cho hóa đơn có Ký hiệu mẫu số hóa đơn, Ký hiệu hóa đơn, Số hóa đơn) </para>
        /// </summary>
        [MaxLength(1)]
        public int? LHDCLQuan { get; set; }

        /// <summary>
        /// <para>Ký hiệu mẫu số hóa đơn có liên quan (Ký hiệu mẫu số hóa đơn bị thay thế/điều chỉnh)</para>
        /// <para>Độ dài tối đa: 11</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự (Chi tiết tại Phụ lục II kèm theo Quy định này)(Chú thích: KHMSHDon.cs)</para>
        /// <para>Bắt buộc (Đối với trường hợp điều chỉnh, thay thế cho hóa đơn có Ký hiệu mẫu số hóa đơn, Ký hiệu hóa đơn, Số hóa đơn)</para>
        /// </summary>
        [MaxLength(11)]
        public string KHMSHDCLQuan { get; set; }

        /// <summary>
        /// <para>Ký hiệu hóa đơn có liên quan (Ký hiệu hóa đơn bị thay thế/điều chỉnh)</para>
        /// <para>Độ dài tối đa: 8</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Đối với trường hợp điều chỉnh, thay thế cho hóa đơn có Ký hiệu mẫu số hóa đơn, Ký hiệu hóa đơn, Số hóa đơn)</para>
        /// </summary>
        [MaxLength(8)]
        public string KHHDCLQuan { get; set; }

        /// <summary>
        /// <para>Số hóa đơn có liên quan (Số hóa đơn bị thay thế/điều chỉnh)</para>
        /// <para>Độ dài tối đa: 8</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Đối với trường hợp điều chỉnh, thay thế cho hóa đơn có Ký hiệu mẫu số hóa đơn, Ký hiệu hóa đơn, Số hóa đơn) </para>
        /// </summary>
        [MaxLength(8)]
        public string SHDCLQuan { get; set; }

        /// <summary>
        /// <para>Loại kỳ dữ liệu điều chỉnh</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para><para>Kiểu dữ liệu: Chuỗi ký tự (T: tháng, Q: quý, N: năm)</para></para>
        /// <para>Bắt buộc (Đối với trường hợp điều chỉnh cho hóa đơn không có Ký hiệu mẫu số hóa đơn, Ký hiệu hóa đơn, Số hóa đơn hoặc Loại hàng hóa, dịch vụ kinh doanh là 1- Xăng dầu)</para>
        /// </summary>
        [MaxLength(1)]
        public int? LKDLDChinh { get; set; }

        /// <summary>
        /// <para>Kỳ dữ liệu điều chỉnh</para>
        /// <para>Độ dài tối đa: 10</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự(Định dạng trường kỳ theo tháng, quý: N1N2/Y1Y2Y3Y4, Định dạng trường kỳ theo ngày: N1N2/N3N4/Y1Y2Y3Y4)(Chú thích: KDLieu.cs)</para>
        /// <para>Bắt buộc (Đối với trường hợp điều chỉnh cho hóa đơn không có Ký hiệu mẫu số hóa đơn, Ký hiệu hóa đơn, Số hóa đơn hoặc Loại hàng hóa, dịch vụ kinh doanh là 1- Xăng dầu)</para>
        /// </summary>
        [MaxLength(10)]
        public string KDLDChinh { get; set; }

        /// <summary>
        /// <para>Số thông báo (Thông báo về hóa đơn điện tử cần rà soát)</para>
        /// <para>Độ dài tối đa: 20</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(20)]
        public string STBao { get; set; }

        /// <summary>
        /// <para>Ngày thông báo (Ngày thông báo về hóa đơn điện tử cần rà soát)</para>
        /// <para>Kiểu dữ liệu: Ngày</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public string NTBao { get; set; }

        /// <summary>
        /// <para>Ghi chú</para>
        /// <para>Độ dài tối đa: 255</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(255)]
        public string GChu { get; set; }

    }
}
