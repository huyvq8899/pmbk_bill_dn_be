﻿using Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._3.DSLDoLCMa;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._3
{
    public partial class LCMa
    {
        /// <summary>
        /// <para>Ký hiệu mẫu số hóa đơn</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự (Chi tiết tại Phụ lục II kèm theo Quy định này)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string KHMSHDon { get; set; }

        /// <summary>
        /// <para>Ký hiệu hóa đơn</para>
        /// <para>Độ dài tối đa: 6</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string KHHDon { get; set; }

        /// <summary>
        /// <para>Số hóa đơn (Số hóa đơn điện tử)</para>
        /// <para>Độ dài tối đa: 8</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string SHDon { get; set; }

        /// <summary>
        /// <para>Ngày (Ngày lập hóa đơn)</para>
        /// <para>Kiểu dữ liệu: Ngày</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        public string Ngay { get; set; }

        public DSLDo DSLDo { get; set; }
    }
}