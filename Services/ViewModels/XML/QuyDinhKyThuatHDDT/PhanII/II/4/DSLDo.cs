﻿using System.Collections.Generic;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._4
{
    public partial class DSLDo
    {
        public List<LDo> LDo { get; set; }
    }

    public partial class LDo
    {
        /// <summary>
        /// <para>Số thứ tự</para>
        /// <para>Độ dài tối đa: 4</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public int? STT { get; set; }

        /// <summary>
        /// <para>Nội dung</para>
        /// <para>Độ dài tối đa: 255</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string NDung { get; set; }

        /// <summary>
        /// <para>Hướng dẫn xử lý</para>
        /// <para>Độ dài tối đa: 255</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string HDXLy { get; set; }

        /// <summary>
        /// <para>Ghi chú</para>
        /// <para>Độ dài tối đa: 255</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public string GChu { get; set; }
    }
}