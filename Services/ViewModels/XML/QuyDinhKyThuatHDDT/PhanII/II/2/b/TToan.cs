﻿using Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.b
{
    public partial class TToan
    {
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

        public List<LPhi> DSLPhi { get; set; }

        public TTKhac TTKhac { get; set; }
    }

    public partial class LPhi
    {
        /// <summary>
        /// <para>Tên loại phí</para>
        /// <para>Độ dài tối đa: 100</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(100)]
        public string TLPhi { get; set; }

        /// <summary>
        /// <para>Tiền phí</para>
        /// <para>Độ dài tối đa: 21,6</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [Column(TypeName = "decimal(21, 6)")]
        public decimal? TPhi { get; set; }
    }
}
