using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.V
{
    public partial class TTSuat
    {
        /// <summary>
        /// <para>Thuế suất (Thuế suất thuế GTGT, Tỷ lệ % GTGT)</para>
        /// <para>Độ dài tối đa: 11</para>
        /// <para>Chuỗi ký tự (Chi tiết tại Mục 17 Phụ lục kèm theo Quyết định này)</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(11)]
        public string TSuat { get; set; }

        [XmlArray("DSHDon")]
        [XmlArrayItem("HDon")]
        public List<HDon> DSHDon { get; set; }

        /// <summary>
        /// Tổng doanh thu chưa có thuế GTGT 
        /// <para>Độ dài tối đa: 21,6</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(21, 6)")]
        public decimal TgDTCThue { get; set; }

        /// <summary>
        /// Tổng thuế GTGT
        /// <para>Độ dài tối đa: 21,6</para>
        /// <para>Kiểu dữ liệu: Số</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(21, 6)")]
        public decimal TgTGTGT { get; set; }


    }
}
