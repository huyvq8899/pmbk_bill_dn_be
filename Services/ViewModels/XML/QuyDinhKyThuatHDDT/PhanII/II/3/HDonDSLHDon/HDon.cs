using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._3.HDonDSLHDon
{
    public partial class HDon
    {
        /// <summary>
        /// <para>Số thứ tự</para>
        /// <para>Độ dài tối đa: 4</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(4)]
        public int? STT { get; set; }

        /// <summary>
        /// <para>Ký hiệu mẫu số</para>
        /// <para>Độ dài tối đa: 11</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự (Chi tiết tại Phụ lục II kèm theo Quy định này)</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại Điều 10, Điều 22 Nghị định số 123/2020/NĐ-CP)</para>
        /// </summary>
        [MaxLength(11)]
        public string KHMSHDon { get; set; }

        /// <summary>
        /// <para>Ký hiệu hóa đơn</para>
        /// <para>Độ dài tối đa: 8</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại Điều 10, Điều 22 Nghị định số 123/2020/NĐ-CP)</para>
        /// </summary>
        [MaxLength(8)]
        public string KHHDon { get; set; }

        /// <summary>
        /// <para>Số hóa đơn</para>
        /// <para>Độ dài tối đa: 8</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại Điều 10, Điều 22 Nghị định số 123/2020/NĐ-CP)</para>
        /// </summary>
        [MaxLength(8)]
        public string SHDon { get; set; }

        /// <summary>
        /// <para>Ngày (Ngày hóa đơn)</para>
        /// <para>Kiểu dữ liệu: Ngày</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại Điều 10, Điều 22 Nghị định số 123/2020/NĐ-CP)</para>
        /// </summary>
        public string Ngay { get; set; }

        /// <summary>
        /// <para>Tên người mua</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Trừ trường hợp quy định tại Điều 10, Điều 22 Nghị định số 123/2020/NĐ-CP)</para>
        /// </summary>
        [MaxLength(400)]
        public string TNMua { get; set; }

        public DSLDo DSLDo { get; set; }
    }

    public partial class DSLDo
    {
        public List<LDo> LDo { get; set; }
    }

    public partial class LDo
    {
        /// <summary>
        /// <para>Mã lỗi</para>
        /// <para>Độ dài tối đa: 4</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(4)]
        public string MLoi { get; set; }

        /// <summary>
        /// <para>Mô tả lỗi</para>
        /// <para>Độ dài tối đa: 255</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string MTLoi { get; set; }

        /// <summary>
        /// <para>Hướng dẫn xử lý</para>
        /// <para>Độ dài tối đa: 255</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string HDXLy { get; set; }

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
