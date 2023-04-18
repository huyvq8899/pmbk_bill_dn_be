using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.V
{
    public partial class NDTKhai
    {

        [XmlArray("TTSuat")]
        [XmlArrayItem("TTSuat")]
        public List<TTSuat> TTSuat { get; set; }

        /// <summary>
        /// <para>Tổng doanh thu hàng hóa, dịch vụ bán ra chịu thuế GTGT</para>
        /// <para>Độ dài tối đa: 21,6</para>
        /// <para>Chuỗi ký tự </para>
        /// <para>Số</para>
        /// <para> Bắt buộc</para>
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(21, 6)")]
        public decimal TgDThu { get; set; }

        /// <summary>
        /// <para>Tổng số thuế GTGT của hàng hóa, dịch vụ bán ra</para>
        /// <para>Độ dài tối đa: 21,6</para>
        /// <para>Chuỗi ký tự </para>
        /// <para>Số</para>
        /// <para> Bắt buộc</para>
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(21, 6)")]
        public decimal TgTgThueDThu { get; set; }

    }
}
