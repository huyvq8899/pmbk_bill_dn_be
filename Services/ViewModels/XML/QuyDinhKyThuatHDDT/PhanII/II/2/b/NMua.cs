using Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.b
{
    public partial class NMua
    {
        /// <summary>
        /// <para>Tên (Họ và tên)</para>
        /// <para>Độ dài tối đa: 100</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Ten { get; set; }

        /// <summary>
        /// <para>Số hộ chiếu (Số hộ chiếu/Giấy tờ nhập xuất cảnh)</para>
        /// <para>Độ dài tối đa: 20</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string SHChieu { get; set; }

        /// <summary>
        /// <para>Ngày cấp hộ chiếu (Ngày cấp hộ chiếu/Giấy tờ nhập xuất cảnh)</para>
        /// <para>Kiểu dữ liệu: Ngày</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public string NCHChieu { get; set; }

        /// <summary>
        /// <para>Ngày hết hạn hộ chiếu (Ngày hết hạn hộ chiếu/Giấy tờ nhập xuất cảnh)</para>
        /// <para>Kiểu dữ liệu: Ngày</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public string NHHHChieu { get; set; }

        /// <summary>
        /// <para>Quốc tịch</para>
        /// <para>Độ dài tối đa: 50</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string QTich { get; set; }

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
        /// <para>Tên ngân hàng</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(400)]
        public string TNHang { get; set; }

        public TTKhac TTKhac { get; set; }
    }
}
