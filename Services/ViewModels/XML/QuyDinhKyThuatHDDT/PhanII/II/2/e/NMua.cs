﻿using Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.e
{
    public partial class NMua
    {
        /// <summary>
        /// <para>Tên (Tên người nhận hàng)</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Đối với trường hợp quy định tại điểm a, khoản 5 Điều 10, Nghị định số 123/2020/NĐ-CP)</para>
        /// </summary>
        [MaxLength(400)]
        public string Ten { get; set; }

        /// <summary>
        /// <para>Mã số thuế (MST người nhận hàng)</para>
        /// <para>Độ dài tối đa: 14</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Đối với trường hợp quy định tại điểm a, khoản 5 Điều 10, Nghị định số 123/2020/NĐ-CP)</para>
        /// </summary>
        [MaxLength(14)]
        public string MST { get; set; }

        /// <summary>
        /// <para>Địa chỉ (Địa chỉ kho nhận hàng)</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(400)]
        public string DChi { get; set; }

        /// <summary>
        /// <para>Họ và tên người nhận hàng</para>
        /// <para>Độ dài tối đa: 100</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(100)]
        public string HVTNNHang { get; set; }

        public List<TTin> TTKhac { get; set; }
    }
}
