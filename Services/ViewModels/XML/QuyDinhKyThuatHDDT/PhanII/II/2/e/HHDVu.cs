﻿using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.e
{
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
        /// <para>Bắt buộc (trừ trường hợp TChat có giá trị là “2-Khuyến mại, 4-Ghi chú, diễn giải”)</para>
        /// </summary>
        [MaxLength(50)]
        public string DVTinh { get; set; }

        /// <summary>
        /// <para>Số lượng</para>
        /// <para>Độ dài tối đa: 21, 6</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc (trừ trường hợp TChat có giá trị là “2-Khuyến mại, 4-Ghi chú, diễn giải”)</para>
        /// </summary>
        [Column(TypeName = "decimal(21, 6)")]
        public decimal? SLuong { get; set; }

        /// <summary>
        /// <para>Đơn giá</para>
        /// <para>Độ dài tối đa: 21, 6</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [Column(TypeName = "decimal(21, 6)")]
        public decimal? DGia { get; set; }

        /// <summary>
        /// <para>Thành tiền (Thành tiền chưa có thuế GTGT)</para>
        /// <para>Độ dài tối đa: 21, 6</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [Column(TypeName = "decimal(21, 6)")]
        public decimal? ThTien { get; set; }

        public List<TTin> TTKhac { get; set; }
    }
}
