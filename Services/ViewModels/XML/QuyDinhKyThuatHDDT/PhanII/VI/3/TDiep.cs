using Services.ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.PhanII.VI._3
{
    public partial class TDiep
    {
        public TTChungThongDiep1510_2 TTChung { get; set; }
        public DLieu DLieu { get; set; }
    }

    public partial class DLieu
    {
        public TBao TBao { get; set; }
    }

    public partial class TBao
    {
        /// <summary>
        /// <para>Mã lỗi</para>
        /// <para>Độ dài tối đa: 4</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [XmlElement]
        [Required]
        [MaxLength(4)]
        public string MLoi { get; set; }

        /// <summary>
        /// <para>Mô tả</para>
        /// <para>Độ dài tối đa: 255</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [XmlElement]
        [Required]
        [MaxLength(255)]
        public string MTa { get; set; }

        /// <summary>
        /// <para>Mã thông điệp (Mã thông điệp gốc)</para>
        /// <para>Độ dài tối đa: 46</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Trừ trường hợp hệ thống của bên nhận không bóc tách và lấy được thông điệp gốc)</para>
        /// </summary>
        [XmlElement]
        [MaxLength(46)]
        public string MTDiep { get; set; }
    }
}
