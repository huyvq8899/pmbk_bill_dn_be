﻿using Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.c
{
    public partial class TToan
    {
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

        public List<TTin> TTKhac { get; set; }
    }
}
