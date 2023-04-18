using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j
{
    public partial class TToan
    {
        public List<LTSuat> THTTLTSuat { get; set; }

        /// <summary>
        /// <para>Tổng tiền chưa thuế (Tổng cộng thành tiền chưa có thuế GTGT)</para>
        /// <para>Độ dài tối đa: 21,6</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(21, 6)")]
        public decimal TgTCThue { get; set; }

        /// <summary>
        /// <para>Tổng tiền thuế (Tổng cộng tiền thuế GTGT)</para>
        /// <para>Độ dài tối đa: 21,6</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(21, 6)")]
        public decimal TgTThue { get; set; }

        /// <summary>
        /// <para>Tổng tiền chiết khấu thương mại</para>
        /// <para>Độ dài tối đa: 21,6</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Không bắt buộc (Trừ trường hợp quy định tại điểm đ, khoản 6, Điều 10 Nghị định số 123/2020/NĐ-CP)</para>
        /// </summary>
        [Column(TypeName = "decimal(21, 6)")]
        public decimal? TTCKTMai { get; set; }

        /// <summary>
        /// <para>Tổng tiền thanh toán bằng số</para>
        /// <para>Độ dài tối đa: 21,6</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(21, 6)")]
        public decimal TgTTTBSo { get; set; }

        /// <summary>
        /// <para>Tổng tiền thanh toán bằng chữ</para>
        /// <para>Độ dài tối đa: 255</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string TgTTTBChu { get; set; }

        public List<TTin> TTKhac { get; set; }
    }

    public partial class LTSuat
    {
        /// <summary>
        /// <para>Thuế suất (Thuế suất thuế GTGT)</para>
        /// <para>Độ dài tối đa: 11</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự (Chi tiết tại Phụ lục V kèm theo Quy định này)</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại khoản 14, Điều 10 Nghị định số 123/2020/NĐ-CP và trường hợp TChat có giá trị là “2-Khuyến mại, 4-Ghi chú, diễn giải”)</para>
        /// </summary>
        [MaxLength(11)]
        public string TSuat { get; set; }

        /// <summary>
        /// <para>Thành tiền (Thành tiền chưa có thuế GTGT)</para>
        /// <para>Độ dài tối đa: 21,6</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(21, 6)")]
        public decimal ThTien { get; set; }

        /// <summary>
        /// <para>Tiền thuế (Tiền thuế GTGT)</para>
        /// <para>Độ dài tối đa: 21,6</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại khoản 14, Điều 10 Nghị định số 123/2020/NĐ-CP)</para>
        /// </summary>
        [Column(TypeName = "decimal(21, 6)")]
        public decimal? TThue { get; set; }
    }
}

