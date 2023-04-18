using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.IV.IV
{
    public partial class TTNCNKTru
    {
        /// <summary>
        /// <para>Khoản thu nhập</para>
        /// <para>Độ dài tối đa: 250</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(250)]
        public string KTNhap { get; set; }

        /// <summary>
        /// <para>Từ tháng (Tháng bắt đầu trả thu nhập)</para>
        /// <para>Độ dài tối đa: 2</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(2)]
        public int? TThang { get; set; }

        /// <summary>
        /// <para>Đến Tháng (Tháng cuối cùng trả thu nhập)</para>
        /// <para>Độ dài tối đa: 2</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(2)]
        public int? DThang { get; set; }

        /// <summary>
        /// <para>Năm (Thời điểm trả thu nhập)</para>
        /// <para>Độ dài tối đa: 4</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(4)]
        public int? Nam { get; set; }

        /// <summary>
        /// <para>Bảo hiểm (Khoản đóng bảo hiểm bắt buộc)</para>
        /// <para>Độ dài tối đa: 21, 6</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(21, 6)")]
        public decimal? BHiem { get; set; }

        /// <summary>
        /// <para>Tổng thu nhập chịu thuế (Tổng thu nhập chịu thuế phải khấu trừ)</para>
        /// <para>Độ dài tối đa: 21, 6</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(21, 6)")]
        public decimal? TTNCThue { get; set; }

        /// <summary>
        /// <para>Tổng thu nhập tính thuế </para>
        /// <para>Độ dài tối đa: 21, 6</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(21, 6)")]
        public decimal? TTNTThue { get; set; }

        /// <summary>
        /// <para>Số thuế (Số thuế thu nhập cá nhân đã khấu trừ)</para>
        /// <para>Độ dài tối đa: 21, 6</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(21, 6)")]
        public decimal? SThue { get; set; }

        /// <summary>
        /// <para>Số thu nhập còn được nhận</para>
        /// <para>Độ dài tối đa: 21, 6</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>

        [Column(TypeName = "decimal(21, 6)")]
        public decimal? STNCDNhan { get; set; }
    }
}
