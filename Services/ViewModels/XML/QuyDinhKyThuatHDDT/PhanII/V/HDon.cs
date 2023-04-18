using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.V
{
    public partial class HDon
    {
        /// <summary>
        /// <para>Số thứ tự</para>
        /// <para>Độ dài tối đa: 65</para>
        /// <para> Kiểu dữ liệu: Số </para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [MaxLength(6)]
        public int STT { get; set; }


        /// <summary>
        /// <para>Ký hiệu mẫu số hóa đơn</para>
        /// <para>Độ dài tối đa: 11</para>
        /// <para>Chuỗi ký tự (Chi tiết tại Mục 16 Phụ lục kèm theo Quyết định này)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(11)]
        public string KHMSHDon { get; set; }

        /// <summary>
        /// <para>Ký hiệu hóa đơn</para>
        /// <para>Độ dài tối đa: 8</para>
        /// <para>Chuỗi ký tự </para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(8)]
        public string KHHDon { get; set; }

        /// <summary>
        /// <para>Số hóa đơn</para>
        /// <para>Độ dài tối đa: 8</para>
        /// <para>Chuỗi ký tự </para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(8)]
        public string SHDon { get; set; }

        /// <summary>
        /// <para>Ngày lập (Ngày, tháng, năm lập hóa đơn)</para>
        /// <para> Ngày </para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        public DateTime NLap { get; set; }

        /// <summary>
        /// <para>Tên người mua</para>
        /// <para>Độ dài tối đa: 400</para>
        /// <para>Chuỗi ký tự </para>
        /// <para> Không bắt buộc</para>
        /// </summary>
        [MaxLength(400)]
        public string TNMua { get; set; }

        /// <summary>
        /// <para>Mã số thuế người mua</para>
        /// <para>Độ dài tối đa: 14</para>
        /// <para>Chuỗi ký tự </para>
        /// <para> Không bắt buộc</para>
        /// </summary>
        [MaxLength(14)]
        public string MSTNMua { get; set; }

        /// <summary>
        /// <para>Doanh thu chưa có thuế GTGT</para>
        /// <para>Độ dài tối đa: 21,6</para>
        /// <para>Chuỗi ký tự </para>
        /// <para>Số</para>
        /// <para> Bắt buộc</para>
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(21, 6)")]
        public decimal DTCThue { get; set; }

        /// <summary>
        /// <para>Doanh thu chưa có thuế GTGT</para>
        /// <para>Độ dài tối đa: 21,6</para>
        /// <para>Chuỗi ký tự </para>
        /// <para>Số</para>
        /// <para> Bắt buộc</para>
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(21, 6)")]
        public decimal TGTGT { get; set; }

        /// <summary>
        /// <para>Doanh thu chưa có thuế GTGT</para>
        /// <para>Độ dài tối đa: 21,6</para>
        /// <para>Chuỗi ký tự </para>
        /// <para>Số</para>
        /// <para>Không bắt buộc</para>
        /// </summary>
        [MaxLength(255)]
        public decimal GChu { get; set; }
    }
}
