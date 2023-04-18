using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities
{
    public class TTChungThongDiep1510_2
    {
        /// <summary>
        /// <para>Phiên bản của thông điệp (Trong Quy định này có giá trị là 2.0.0)</para>
        /// <para>Độ dài tối đa: 6</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [XmlElement]
        [MaxLength(6)]
        public string PBan { get; set; } = "2.0.1";

        /// <summary>
        /// <para>Mã nơi gửi</para>
        /// <para>Độ dài tối đa: 14</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        //[Required]
        [XmlElement]
        [MaxLength(14)]
        public string MNGui { get; set; }

        /// <summary>
        /// <para>Mã nơi nhận</para>
        /// <para>Độ dài tối đa: 14</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        //[Required]
        [XmlElement]
        [MaxLength(14)]
        public string MNNhan { get; set; }

        /// <summary>
        /// <para>Mã loại thông điệp</para>
        /// <para>Độ dài tối đa: 46</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [XmlElement]
        [MaxLength(46)]
        public string MTDiep { get; set; }

        /// <summary>
        /// <para>Mã loại thông điệp</para>
        /// <para>Độ dài tối đa: 3</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc</para>
        /// </summary>
        [Required]
        [MaxLength(3)]
        public string MLTDiep { get; set; }

        /// <summary>
        /// <para>Mã thông điệp tham chiếu<para>
        /// <para>Độ dài tối đa: 46</para>
        /// <para>Kiểu dữ liệu: Chuỗi ký tự</para>
        /// <para>Bắt buộc (Trừ trường hợp hệ thống của bên nhận không bóc tách và lấy được thông điệp gốc)</para>
        /// </summary>
        [MaxLength(46)]
        public string MTDTChieu { get; set; }
    }
}
