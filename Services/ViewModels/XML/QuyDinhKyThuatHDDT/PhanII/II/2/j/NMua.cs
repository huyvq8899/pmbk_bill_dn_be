using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.j
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
        /// <para>Mã số thuế</para>
        /// <para>Độ dài tối đa: 14</para>
        /// <para>Bắt buộc (Nếu có)</para>
        /// <para>Tham khảo: khoản 5 điều 10 ND 123</para>
        /// </summary>
        [MaxLength(14)]
        public string MST { get; set; }

        /// <summary>
        /// <para>Địa chỉ</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Nếu có)</para>
        /// <para>Tham khảo: khoản 5 điều 10 ND 123</para>
        /// </summary>
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
        /// <para>Căn cước công dân</para>
        /// <para>Độ dài tối đa: 12</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>      
        /// </summary>
        [MaxLength(12)]
        public string CCCDan { get; set; }

        public List<TTin> TTKhac { get; set; }
    }
}
