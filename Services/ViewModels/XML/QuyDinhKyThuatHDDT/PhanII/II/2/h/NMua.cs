﻿using Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.h
{
    public partial class NMua
    {
        /// <summary>
        /// <para>Tên</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public string Ten { get; set; }

        /// <summary>
        /// <para>Mã số thuế</para>
        /// <para>Độ dài tối đa: 14</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public string MST { get; set; }

        /// <summary>
        /// <para>Địa chỉ</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public string DChi { get; set; }

        /// <summary>
        /// <para>Mã khách hàng</para>
        /// <para>Độ dài tối đa: 50</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public string MKHang { get; set; }

        /// <summary>
        /// <para>Số điện thoại</para>
        /// <para>Độ dài tối đa: 20</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public string SDThoai { get; set; }

        /// <summary>
        /// <para>Địa chỉ thư điện tử</para>
        /// <para>Độ dài tối đa: 50</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public string DCTDTu { get; set; }

        /// <summary>
        /// <para>Họ và tên người mua hàng</para>
        /// <para>Độ dài tối đa: 100</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public string HVTNMHang { get; set; }

        /// <summary>
        /// <para>Số tài khoản ngân hàng</para>
        /// <para>Độ dài tối đa: 30</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public string STKNHang { get; set; }

        /// <summary>
        /// <para>Tên ngân hàng</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public string TNHang { get; set; }

        public TTKhac TTKhac { get; set; }
    }
}