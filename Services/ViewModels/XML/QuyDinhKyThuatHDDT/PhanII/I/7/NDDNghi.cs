using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.I._7
{
    public partial class NDDNghi
    {
        /// <summary>
        /// <para>Tên hàng hóa, dịch vụ</para>
        /// <para>Độ dài tối đa: 500</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string THHDVu { get; set; }

        /// <summary>
        /// <para>Tên (Tên người mua hàng hóa, dịch vụ)</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(400)]
        public string Ten { get; set; }

        /// <summary>
        /// <para>Địa chỉ (Địa chỉ người mua hàng hóa, dịch vụ)  </para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(400)]
        public string DChi { get; set; }

        /// <summary>
        /// <para>Mã số thuế (Mã số thuế người mua hàng hóa, dịch vụ)</para>
        /// <para>Độ dài tối đa: 14</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(14)]
        public string MST { get; set; }

        /// <summary>
        /// <para>Số hợp đồng (Số hợp đồng mua bán hàng hóa, dịch vụ)</para>
        /// <para>Độ dài tối đa: 50</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(50)]
        public string SHDong { get; set; }

        /// <summary>
        /// <para>Ngày hợp đồng (Ngày hợp đồng mua bán hàng hóa, dịch vụ)</para>
        /// <para>Kiểu dữ liệu: Ngày</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        public DateTime? NHDong { get; set; }

        /// <summary>
        /// <para>Doanh thu phát sinh</para>
        /// <para>Độ dài tối đa: 19,4</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(19, 4)")]
        public decimal? DTPSinh { get; set; }

    }
}
