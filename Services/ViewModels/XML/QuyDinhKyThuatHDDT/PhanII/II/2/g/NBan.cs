using Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.a;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.II._2.g
{
    public partial class NBan
    {
        /// <summary>
        /// <para>Tên (Tên người xuất hàng)</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(400)]
        public string Ten { get; set; }

        /// <summary>
        /// <para>Mã số thuế (MST người xuất hàng)</para>
        /// <para>Độ dài tối đa: 14</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(14)]
        public string MST { get; set; }

        /// <summary>
        /// <para>Hợp đồng kinh tế/số</para>
        /// <para>Độ dài tối đa: 255</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string HDKTSo { get; set; }

        /// <summary>
        /// <para>Hợp đồng kinh tế/ngày</para>
        /// <para>Kiểu dữ liệu: Ngày</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public string HDKTNgay { get; set; }

        /// <summary>
        /// <para>Địa chỉ (Địa chỉ kho xuất hàng)</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(400)]
        public string DChi { get; set; }

        /// <summary>
        /// <para>Họ và tên người xuất hàng</para>
        /// <para>Độ dài tối đa: 100</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(100)]
        public string HVTNXHang { get; set; }

        /// <summary>
        /// <para>Tên người vận chuyển</para>
        /// <para>Độ dài tối đa: 100</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string TNVChuyen { get; set; }

        /// <summary>
        /// <para>Hợp đồng số (Hợp đồng vận chuyển)</para>
        /// <para>Độ dài tối đa: 50</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(50)]
        public string HDSo { get; set; }

        /// <summary>
        /// <para>Phương tiện vận chuyển</para>
        /// <para>Độ dài tối đa: 50</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string PTVChuyen { get; set; }

        public TTKhac TTKhac { get; set; }
    }
}
