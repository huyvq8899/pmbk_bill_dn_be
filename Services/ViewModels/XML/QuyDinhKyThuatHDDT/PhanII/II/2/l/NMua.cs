using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.l
{
    public partial class NMua
    {
        /// <summary>
        /// <para>Tên</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Nếu có)</para>
        /// <para>Tham khảo: khoản 5 điều 10 ND 123</para>
        /// </summary>
        [MaxLength(400)]
        public string Ten { get; set; }

        /// <summary>
        /// <para>Số hộ chiếu (Số hộ chiếu/Giấy tờ nhập xuất cảnh)</para>
        /// <para>Độ dài tối đa: 20</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Nếu có)</para>
        /// <para>Tham khảo: khoản 5 điều 10 ND 123</para>
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string SHChieu { get; set; }

        /// <summary>
        /// <para>Ngày cấp hộ chiếu (Ngày cấp hộ chiếu/Giấy tờ nhập xuất cảnh)</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Ngay</para>
        /// <para>Bắt buộc (Nếu có)</para>
        /// <para>Tham khảo: khoản 5 điều 10 ND 123</para>
        /// </summary>
        [Required]
        
        public string NCHChieu { get; set; }

        /// <summary>
        /// <para>Ngày hết hạn hộ chiếu (Ngày hết hạn hộ chiếu/Giấy tờ nhập xuất cảnh)</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Nếu có)</para>
        /// <para>Tham khảo: khoản 5 điều 10 ND 123</para>
        /// </summary>

        public string NHHHChieu { get; set; }

        /// <summary>
        /// <para>Quốc tịch</para>
        /// <para>Độ dài tối đa: 50</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Nếu có)</para>
        /// <para>Tham khảo: khoản 5 điều 10 ND 123</para>
        /// </summary>
        [MaxLength(50)]
        public string QTich { get; set; }
    }
}
