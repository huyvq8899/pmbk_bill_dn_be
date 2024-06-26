﻿using Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.c
{
    public partial class NBan
    {
        /// <summary>
        /// <para>Tên (Đơn vị bán tài sản NN)</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(400)]
        public string Ten { get; set; }

        /// <summary>
        /// <para>Mã số thuế</para>
        /// <para>Độ dài tối đa: 14</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Trường hợp không có Mã đơn vị quan hệ ngân sách)</para>
        /// </summary>
        [MaxLength(14)]
        public string MST { get; set; }

        /// <summary>
        /// <para>Mã đơn vị quan hệ ngân sách (Mã số đơn vị có quan hệ với ngân sách của đơn vị bán tài sản công)</para>
        /// <para>Độ dài tối đa: 7</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Trường hợp không có Mã số thuế)</para>
        /// </summary>
        [MaxLength(7)]
        public string MDVQHNSach { get; set; }

        /// <summary>
        /// <para>Địa chỉ</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(400)]
        public string DChi { get; set; }

        /// <summary>
        /// <para>Số điện thoại</para>
        /// <para>Độ dài tối đa: 20</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(20)]
        public string SDThoai { get; set; }

        /// <summary>
        /// <para>Địa chỉ thư điện tử</para>
        /// <para>Độ dài tối đa: 50</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(50)]
        public string DCTDTu { get; set; }

        /// <summary>
        /// <para>Số tài khoản ngân hàng</para>
        /// <para>Độ dài tối đa: 30</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(30)]
        public string STKNHang { get; set; }

        /// <summary>
        /// <para>Số tài khoản ngân hàng</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(400)]
        public string TNHang { get; set; }

        /// <summary>
        /// <para>Fax</para>
        /// <para>Độ dài tối đa: 20</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(20)]
        public string Fax { get; set; }

        /// <summary>
        /// <para>Website</para>
        /// <para>Độ dài tối đa: 50</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(50)]
        public string Website { get; set; }

        /// <summary>
        /// <para>Số quyết định (Số quyết định bán tài sản)</para>
        /// <para>Độ dài tối đa: 50</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(50)]
        public string SQDinh { get; set; }

        /// <summary>
        /// <para>Ngày quyết định (Ngày quyết định bán tài sản)</para>
        /// <para>Kiểu dữ liệu: Ngày</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public string NQDinh { get; set; }

        /// <summary>
        /// <para>Cơ quan ban hành quyết định (Cơ quan ban hành quyết định bán tài sản)</para>
        /// <para>Độ dài tối đa: 200</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(200)]
        public string CQBHQDinh { get; set; }

        /// <summary>
        /// <para>Hình thức bán</para>
        /// <para>Độ dài tối đa: 200</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(200)]
        public string HTBan { get; set; }

        public List<TTin> TTKhac { get; set; }
    }
}
