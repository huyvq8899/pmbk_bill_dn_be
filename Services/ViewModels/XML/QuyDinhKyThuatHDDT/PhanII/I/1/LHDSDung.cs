using Services.ViewModels.XML.QuyDinhKyThuatHDDT.Enums;
using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._1
{
    public partial class LHDSDung
    {
        /// <summary>
        /// <para>Hóa đơn GTGT</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số(0: không sử dụng,1: sử dụng)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(1)]
        public SDung HDGTGT { get; set; }

        /// <summary>
        /// <para>Hóa đơn bán hàng</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số(0: không sử dụng,1: sử dụng)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(1)]
        public SDung HDBHang { get; set; }

        /// <summary>
        /// <para>Hóa đơn bán tài sản công</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số(0: không sử dụng,1: sử dụng)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(1)]
        public SDung HDBTSCong { get; set; }

        /// <summary>
        /// <para>Hóa đơn bán hàng dự trữ quốc gia</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số(0: không sử dụng,1: sử dụng)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(1)]
        public SDung HDBHDTQGia { get; set; }

        /// <summary>
        /// <para>Hóa đơn khác (Các loại hóa đơn khác)</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số(0: không sử dụng,1: sử dụng)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(1)]
        public SDung HDKhac { get; set; }

        /// <summary>
        /// <para>Chứng từ (Các chứng từ được in, phát hành, sử dụng và quản lý như hóa đơn)</para>
        /// <para>Độ dài tối đa: 1</para>
        /// <para>Kiểu dữ liệu: Số(0: không sử dụng,1: sử dụng)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(1)]
        public SDung CTu { get; set; }
    }
}
