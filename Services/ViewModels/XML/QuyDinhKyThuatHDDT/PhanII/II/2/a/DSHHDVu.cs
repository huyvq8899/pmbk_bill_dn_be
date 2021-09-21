using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a
{
    public partial class DSHHDVu
    {
        public List<HHDVu> HHDVu { get; set; }
    }

    public partial class HHDVu
    {
        /// <summary>
        /// <para>Tính chất</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số (Chi tiết tại Phụ lục IV kèm theo Quy định này)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(1)]
        public TChat TChat { get; set; }

        /// <summary>
        /// <para>Số thứ tự</para>
        /// <para>Độ dài tối đa: 4</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(4)]
        public int? STT { get; set; }

        /// <summary>
        /// <para>Mã hàng hóa, dịch vụ</para>
        /// <para>Độ dài tối đa: 50</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc (Trừ trường hợp quy định tại điểm a, khoản 6, Điều 10 Nghị định số 123/2020/NĐ-CP)</para>
        /// </summary>
        [MaxLength(50)]
        public string MHHDVu { get; set; }

        /// <summary>
        /// <para>Tên hàng hóa, dịch vụ</para>
        /// <para>Độ dài tối đa: 500</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string THHDVu { get; set; }

        /// <summary>
        /// <para>Đơn vị tính</para>
        /// <para>Độ dài tối đa: 50</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại khoản 6, khoản 14, Điều 10 Nghị định số 123/2020/NĐ-CP và trường hợp TChat khác giá trị “1- Hàng hóa, dịch vụ” )</para>
        /// </summary>
        [MaxLength(50)]
        public string DVTinh { get; set; }

        /// <summary>
        /// <para>Số lượng</para>
        /// <para>Độ dài tối đa: 19,4</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại khoản 6, khoản 14, Điều 10 Nghị định số 123/2020/NĐ-CP và trường hợp TChat khác giá trị “1- Hàng hóa, dịch vụ” )</para>
        /// </summary>
        [Column(TypeName = "decimal(19, 4)")]
        public decimal? SLuong { get; set; }

        /// <summary>
        /// <para>Đơn giá</para>
        /// <para>Độ dài tối đa: 19,4</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại khoản 6, khoản 14, Điều 10 Nghị định số 123/2020/NĐ-CP và trường hợp TChat khác giá trị “1- Hàng hóa, dịch vụ”)</para>
        /// </summary>
        [Column(TypeName = "decimal(19, 4)")]
        public decimal? DGia { get; set; }

        /// <summary>
        /// <para>Tỷ lệ % chiết khấu (Trong trường hợp muốn thể hiện thông tin chiết khấu theo cột cho từng hàng hóa, dịch vụ)</para>
        /// <para>Độ dài tối đa: 6,4</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Không Bắt buộc</para>
        /// </summary>
        [Column(TypeName = "decimal(6, 4)")]
        public decimal? TLCKhau { get; set; }

        /// <summary>
        /// <para>Số tiền chiết khấu (Trong trường hợp muốn thể hiện thông tin chiết khấu theo cột cho từng hàng hóa, dịch vụ)</para>
        /// <para>Độ dài tối đa: 19,4</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Không Bắt buộc</para>
        /// </summary>
        [Column(TypeName = "decimal(19, 4)")]
        public decimal? STCKhau { get; set; }

        /// <summary>
        /// <para>Thành tiền (Thành tiền chưa có thuế GTGT)</para>
        /// <para>Độ dài tối đa: 19,4</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc (Trừ trường hợp TChat có giá trị là “2-Khuyến mại, 4-Ghi chú, diễn giải”)</para>
        /// </summary>
        [Column(TypeName = "decimal(19, 4)")]
        public decimal? ThTien { get; set; }

        /// <summary>
        /// <para>Thuế suất (Thuế suất thuế GTGT)</para>
        /// <para>Độ dài tối đa: 10</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự (Chi tiết tại Phụ lục V kèm theo Quy định này)</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại khoản 14, Điều 10 Nghị định số 123/2020/NĐ-CP và trường hợp TChat có giá trị là “2-Khuyến mại, 4-Ghi chú, diễn giải”)</para>
        /// </summary>
        [MaxLength(10)]
        public decimal? TSuat { get; set; }

        public TTKhac TTKhac { get; set; }
    }
}
